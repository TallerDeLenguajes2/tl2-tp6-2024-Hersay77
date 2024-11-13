using Microsoft.AspNetCore.Mvc;

public class PresupuestoController : Controller
{
    private PresupuestoRepository presupuestoRepository;

    public PresupuestoController()
    {
        presupuestoRepository = new PresupuestoRepository("Data Source=db/Tienda.db;Cache=Shared");
    }

    /*
    En el controlador de Presupuestos: Listar, Crear, Modificar y Eliminar Presupuestos.
    ○ Tiene que poder cargar productos a un presupuesto específico
    ○ Tiene que poder ver un presupuesto con el listado de productos
    correspondientes.
    */

}