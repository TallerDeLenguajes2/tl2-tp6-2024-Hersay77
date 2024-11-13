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
    ///////////
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
    /////////
    [HttpGet]
    public IActionResult Modificar(int id)
    {
        return View(productoRepository.ObtenerProducto(id));
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        productoRepository.ModificarProducto(producto.IdProducto, producto);
        return RedirectToAction("Index");
    }
    //////
    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        return View(productoRepository.ObtenerProducto(id));
    }

    [HttpPost]
    public IActionResult EliminarProducto(int id)
    {
        productoRepository.EliminarProducto(id);
        return RedirectToAction ("Index"); 
    }

}