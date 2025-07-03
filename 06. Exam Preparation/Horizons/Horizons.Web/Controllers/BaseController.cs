using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Horizons.Web.Controllers;

[Authorize]
public class BaseController : Controller
{
	protected bool IsUserAuthenticated() => User.Identity?.IsAuthenticated ?? false;

	protected string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
}
