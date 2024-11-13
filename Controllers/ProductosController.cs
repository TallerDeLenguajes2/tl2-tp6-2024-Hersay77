using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;

    public ProductosController()
    {
        productoRepository = new ProductoRepository("Data Source=db/Tienda.db;Cache=Shared");
    }

    //En el controlador de Productos : Listar, Crear, Modificar y Eliminar Productos
    [HttpGet("ListarProductos")]
    public IActionResult Index()
    {
        return View(productoRepository.ObtenerProductos());
    }

    [HttpGet("Crear")]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost("CrearProducto")]
    public IActionResult CrearProducto(Producto producto)
    {
        productoRepository.CrearProducto(producto);
        return RedirectToAction("Index");
    }

    public IActionResult Modificar()
    {
        return View();
    }

    public IActionResult EliminarProductos()
    {
        return View();
    }

}