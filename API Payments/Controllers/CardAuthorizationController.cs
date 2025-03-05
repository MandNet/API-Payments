using Microsoft.AspNetCore.Mvc;

namespace API_Payments.Controllers
{
    public class CardAuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
