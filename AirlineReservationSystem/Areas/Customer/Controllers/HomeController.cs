using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
