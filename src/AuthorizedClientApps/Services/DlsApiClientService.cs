using Microsoft.Identity.Web;
using System.Net;
using System.Net.Http.Headers;

namespace Portal.Services;

public class DlsApiClientService : IDlsApiClientService
{
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly HttpClient _httpClient;
    private readonly string _scope;
    private readonly string _baseAddress;

    public DlsApiClientService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, IConfiguration config)
    {
        _tokenAcquisition = tokenAcquisition;
        _httpClient = httpClient;
        _scope = config["DlsApi:Scopes"];
        _baseAddress = $"{config["DlsApi:Url"]}/api";
    }

    private async Task AuthenticateClient()
    {
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { _scope });
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task<HttpResponseMessage> GetAsync(string path, bool bUseAuth = true)
    {
        if (bUseAuth) await AuthenticateClient();
        return await _httpClient.GetAsync($"{_baseAddress}/{path}");
    }

    private async Task<T> Response<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsAsync<T>();

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                throw new UnauthorizedAccessException(response.ReasonPhrase);
            default:
                throw new HttpRequestException(response.ReasonPhrase);
        }
    }

    public async Task<List<string>> GetAllFolderNames()
    {
        var httpResponseMesssage = await GetAsync("folder-names-all");
        return await Response<List<string>>(httpResponseMesssage);
    }
}
