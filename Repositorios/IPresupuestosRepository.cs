public interface IPresupuestosRepository
{
    public bool CrearPresupuesto(Presupuesto presupuesto);

    public List<Presupuesto> ObtenerPresupuestos();
    public Presupuesto ObtenerPresupuesto(int id);

    public bool AgregarProductoYCantidad(int idPresupuesto, int idProducto, int cantidad);
    public bool EliminarPresupuesto(int idPresupuesto);

}