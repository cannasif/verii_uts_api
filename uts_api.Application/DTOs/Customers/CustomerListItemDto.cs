namespace uts_api.Application.DTOs.Customers;

public sealed class CustomerListItemDto
{
    public long Id { get; set; }
    public string CustomerCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? TcknNumber { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public short BranchCode { get; set; }
    public bool IsErpIntegrated { get; set; }
    public DateTime? LastSyncDateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
