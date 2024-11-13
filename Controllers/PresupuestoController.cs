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

    
    /*
    En el controlador de Presupuestos: Listar, Crear, Modificar y Eliminar Presupuestos.
        Tiene que poder cargar productos a un presupuesto específico
        Tiene que poder ver un presupuesto con el listado de productos
    correspondientes.
    */

}