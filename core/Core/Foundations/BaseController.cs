using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Waffle.Foundations;

[Authorize]
[Route("api/[controller]")]
public class BaseController : Controller
{
}
