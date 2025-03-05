using Microsoft.AspNetCore.Mvc;

namespace API_Payments.Controllers
{
    public class PaymentFeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
