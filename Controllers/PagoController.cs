using metalurgicaMVC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class PagoController(IPagoRepository repo): Controller
{
    private readonly IPagoRepository _repo = repo;
}