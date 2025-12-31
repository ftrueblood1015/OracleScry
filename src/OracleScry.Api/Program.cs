using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using OracleScry.Api.Auth;
using OracleScry.Api.Extensions;
using OracleScry.Api.Middleware;
using OracleScry.Application;
using OracleScry.Application.BackgroundJobs;
using OracleScry.Domain.Identity;
using OracleScry.Infrastructure;
using OracleScry.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();
builder.Services.AddCorsPolicy();

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<OracleScryDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Add Hangfire
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection not configured");

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        SchemaName = "Hangfire",
        PrepareSchemaIfNecessary = true,
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 1; // Single worker for import jobs
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "OracleScry API v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Configure Hangfire Dashboard with Basic Auth
var hangfireConfig = builder.Configuration.GetSection("Hangfire:Dashboard");
var hangfireUsername = hangfireConfig["Username"] ?? "admin";
var hangfirePassword = hangfireConfig["Password"]
    ?? throw new InvalidOperationException("Hangfire dashboard password not configured");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireAuthorizationFilter(hangfireUsername, hangfirePassword)],
    DashboardTitle = "OracleScry Jobs"
});

// Configure recurring job - daily at 3:00 AM UTC
RecurringJob.AddOrUpdate<ScryfallImportJob>(
    "scryfall-daily-import",
    job => job.ExecuteAsync(CancellationToken.None),
    "0 3 * * *", // 3:00 AM UTC daily
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc }
);

app.MapControllers();

app.Run();
