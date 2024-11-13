

public interface IProductoRepository
{
    public void CrearProducto(Producto producto);

    public void ModificarProducto(int idProducto, Producto producto);

    public List<Producto> ObtenerProductos();

    public Producto ObtenerProducto(int idProducto);

    public void EliminarProducto(int id);
}