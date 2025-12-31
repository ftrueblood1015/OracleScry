using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Services;

/// <summary>
/// HTTP client for Scryfall API bulk data operations.
/// </summary>
public class ScryfallApiClient : IScryfallApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ScryfallApiClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public ScryfallApiClient(HttpClient httpClient, ILogger<ScryfallApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://api.scryfall.com/");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "OracleScry/1.0");
    }

    public async Task<ScryfallBulkDataInfo> GetBulkDataInfoAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Fetching bulk data info from Scryfall API");

        var response = await _httpClient.GetAsync("bulk-data/oracle-cards", ct);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<ScryfallBulkDataResponse>(JsonOptions, ct)
            ?? throw new InvalidOperationException("Failed to deserialize Scryfall bulk data response");

        return new ScryfallBulkDataInfo(
            data.Id,
            data.Type,
            data.UpdatedAt,
            data.DownloadUri,
            data.Size,
            data.ContentType
        );
    }

    public async Task<Stream> DownloadBulkDataAsync(string downloadUri, CancellationToken ct = default)
    {
        _logger.LogInformation("Downloading bulk data from {Uri}", downloadUri);

        var response = await _httpClient.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync(ct);
    }

    private record ScryfallBulkDataResponse(
        string Id,
        string Type,
        DateTime UpdatedAt,
        string DownloadUri,
        long Size,
        string ContentType
    );
}
