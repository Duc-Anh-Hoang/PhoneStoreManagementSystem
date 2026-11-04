namespace PhoneStoreAPI.Common.Services;

public interface ICodeGenerator
{
    Task<string> NextAsync(string prefix, string? sequenceName = null);
}
