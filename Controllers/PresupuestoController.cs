using Microsoft.AspNetCore.Mvc;

public class PresupuestosController : Controller
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestosController()
    {
        presupuestoRepository = new PresupuestoRepository("Data Source=db/Tienda.db;Cache=Shared");
    }
    ///////////////////////
    [HttpGet]
    public IActionResult Index()
    {
        return View(presupuestoRepository.ObtenerPresupuestos());
    }
    /////////////////////

    public IActionResult Crear()
    {
        return View();
    }

    public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    {
        presupuestoRepository.CrearPresupuesto(presupuesto);
        return RedirectToAction ("Index");
    }

    ////////////////////////////////
    ///
    
    
}