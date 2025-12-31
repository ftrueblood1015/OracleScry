namespace OracleScry.Application.DTOs.Import;

/// <summary>
/// DTO for individual card import error.
/// </summary>
public record CardImportErrorDto(
    Guid Id,
    Guid? OracleId,
    string? CardName,
    string ErrorMessage
);
