using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class TrabajoController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}