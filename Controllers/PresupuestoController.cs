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
        return RedirectToAction("Index");
    }

    ////////////////////////////////

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        return View(presupuestoRepository.ObtenerPresupuesto(id));
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
    {
        presupuestoRepository.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }

    /////////////////////

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        return View(presupuestoRepository.ObtenerPresupuesto(id));
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        presupuestoRepository.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }

    //////////////////////////

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        ProductoRepository productoRepository = new ProductoRepository("Data Source=db/Tienda.db;Cache=Shared");
        List<Producto> productos = productoRepository.ObtenerProductos();
        PresupuestoDetalleViewModel presupuestoDetalleViewModel = new PresupuestoDetalleViewModel(id, productos);

        return View(presupuestoDetalleViewModel);
    }

    [HttpPost]
    public IActionResult AgregarProductoEnPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        presupuestoRepository.AgregarProductoYCantidad(idPresupuesto, idProducto, cantidad);
        return RedirectToAction("Index");
    }

    //////////////////
    [HttpGet]
    public IActionResult DetallesDelPresupuesto(int id)
    {
        return View(presupuestoRepository.ObtenerPresupuesto(id));
    }

    ///////////////////

    public IActionResult EliminarProducto(int id)
    {
       
        return View( presupuestoRepository.ObtenerPresupuesto(id));
    }

    [HttpPost]
    public IActionResult EliminarProductoDePresupuesto(int idPresupuesto, int idProducto)
    {
        presupuestoRepository.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction("Index");
    }



}