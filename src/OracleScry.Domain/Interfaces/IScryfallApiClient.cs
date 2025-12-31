namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Scryfall bulk data metadata from API.
/// </summary>
public record ScryfallBulkDataInfo(
    string Id,
    string Type,
    DateTime UpdatedAt,
    string DownloadUri,
    long Size,
    string ContentType
);

/// <summary>
/// Client for Scryfall bulk data API.
/// Defined in Domain layer, implemented in Infrastructure layer.
/// </summary>
public interface IScryfallApiClient
{
    /// <summary>Get metadata about the oracle-cards bulk data file</summary>
    Task<ScryfallBulkDataInfo> GetBulkDataInfoAsync(CancellationToken ct = default);

    /// <summary>Download the bulk data file as a stream</summary>
    Task<Stream> DownloadBulkDataAsync(string downloadUri, CancellationToken ct = default);
}
