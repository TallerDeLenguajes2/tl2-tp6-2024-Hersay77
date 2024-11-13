using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;

    public ProductosController()
    {
        productoRepository = new ProductoRepository("Data Source=db/Tienda.db;Cache=Shared");
    }

    //En el controlador de Productos : Listar, Crear, Modificar y Eliminar Productos

    public IActionResult Index()
    {
        return View(productoRepository.ObtenerProductos());
    }

    public IActionResult Crear()
    {
        return View();
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