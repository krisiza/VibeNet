using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VibeNet.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

    }
}
