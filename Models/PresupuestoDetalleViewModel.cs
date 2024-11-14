public class PresupuestoDetalleViewModel
{
    private int idPresupuesto;
    private List<Producto> productos;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public List<Producto> Productos { get => productos; set => productos = value; }
    
    public PresupuestoDetalleViewModel(int idPresupuesto, List<Producto> productos)
    {
        this.IdPresupuesto = idPresupuesto;
        this.Productos = productos;
    }


}