namespace uts_api.Application.Common.Models;

public sealed class FilterRule
{
    public string Column { get; set; } = string.Empty;
    public string Operator { get; set; } = "eq";
    public string Value { get; set; } = string.Empty;
}
