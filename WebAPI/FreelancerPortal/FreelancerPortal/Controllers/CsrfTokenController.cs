using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FreelancerPortal.Controllers
{
    public class CsrfTokenController : Controller
    {
        private readonly IAntiforgery _antiforgery;

        public CsrfTokenController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }


        [HttpGet("token")]
        public IActionResult GetToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new { token = tokens.RequestToken });
        }
    }
}
