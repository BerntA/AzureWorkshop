using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Portal.Services;

namespace Portal.Controllers;

public class DataController : Controller
{
    private readonly IDlsApiClientService _dlsService;

    public DataController(IDlsApiClientService dlsService)
    {
        _dlsService = dlsService;
    }

    [AuthorizeForScopes(ScopeKeySection = "DlsApi:Scopes")]
    public async Task<IActionResult> Index()
    {
        return View(await _dlsService.GetAllFolderNames());
    }
}
