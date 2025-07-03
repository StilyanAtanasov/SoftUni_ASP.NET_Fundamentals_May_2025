namespace Horizons.Services.Core.Utils;

public class ServiceResult
{
    public ServiceResult() => Errors = new();
    
    public bool Success { get; set; } = true;

    public bool Found { get; set; } = true;

    public bool HasPermission { get; set; } = true;

    public Dictionary<string, string> Errors { get; set; }
}