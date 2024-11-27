using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VibeNet.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

    }
}
