using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers;

[Authorize]
public class HomeController : Controller
{
    public async Task<ActionResult> Index()
    {
        return View();
    }
}
