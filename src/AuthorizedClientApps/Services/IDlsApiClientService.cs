namespace Portal.Services;

public interface IDlsApiClientService
{
    Task<List<string>> GetAllFolderNames();
}
