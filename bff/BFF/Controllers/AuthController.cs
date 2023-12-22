using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BFF.Controllers;

public class AuthController : Controller
{
    public ActionResult Login(string returnUrl = "/")
    {
        return new ChallengeResult("oidc", new AuthenticationProperties 
        { 
            RedirectUri = returnUrl
        });
    }

    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return new SignOutResult("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });
    }

    public ActionResult GetUser()
    {
        if (User.Identity.IsAuthenticated)
        {
            var claims = ((ClaimsIdentity)this.User.Identity).Claims.Select(c =>
                new { type = c.Type, value = c.Value })
                .ToArray();

            return Json(new { isAuthenticated = true, claims });
        }

        return Json(new { isAuthenticated = false });
    }
}

