using Microsoft.Data.Sqlite;

public class PresupuestoRepository : IPresupuestosRepository
{
    private string cadenaDeConexion;
    public PresupuestoRepository(string cadenaDeConexion) //constructor del repositorio recibe la cadena de conexion
    {
        this.cadenaDeConexion = cadenaDeConexion;
    }
    public bool CrearPresupuesto(Presupuesto presupuesto)
    {
        //control que los productos en lista de presupuestos detalles existan
        ProductoRepository productoRepository = new ProductoRepository(cadenaDeConexion);

        foreach (var detalle in presupuesto.Detalle)
        {
            if (productoRepository.ObtenerProducto(detalle.Producto.IdProducto) == null) return false; //usando el repositorio producto intento obtener un producto con el mismo id, si no encuentra devuelve null por lo tanto no se puede realizar el presupuesto de un proudtco que no exsta
        }
        //si todos los producto en la lista presupuesto detalle existen se continua creando el presupuesto

        string query1 = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)"; //consulta para tabla presupuesto, idPresupuesto es una primarykey autoincremental
        string query2 = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";//consulta para tabla detallepresupuesto


        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion)) //creo conexion
        {
            connection.Open(); //abro conexion

            SqliteCommand command = new SqliteCommand(query1, connection); //comando con la consulta y conexion
            command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
            command.ExecuteNonQuery(); //ejecuto el comando

            long idPresupuesto;
            string queryUltimoIdCreado = "SELECT last_insert_rowid();"; //consulta para obtener ultimo id
            using (command = new SqliteCommand(queryUltimoIdCreado, connection))
            {
                idPresupuesto = (long)command.ExecuteScalar(); //executeEscalar devuelve el valor solicitado, en el comando anterior ya se guardo un registro por lo que la tabla siempre tendra uno y no devolvera nunca null, permitiendo guardarlo en la tabla presupuestodetalle
            }
            SqliteCommand command2 = new SqliteCommand(query2, connection); //comando con la consulta y conexion
            foreach (var detalle in presupuesto.Detalle)
            {

                command2.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                command2.Parameters.AddWithValue("@idProducto", detalle.Producto.IdProducto);
                command2.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                command2.ExecuteNonQuery();
            }
            connection.Close(); //cierro conexion
        }
        return true;
    }

    public List<Presupuesto> ObtenerPresupuestos()
    {
        var lista = new List<Presupuesto>();
        string query = @"SELECT 
                            P.idPresupuesto,
                            P.NombreDestinatario,
                            P.FechaCreacion
                            FROM 
                            Presupuestos P;";
        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
        {
            connection.Open(); //abro conexion
            SqliteCommand command = new SqliteCommand(query, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    lista.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return lista;
    }

    public Presupuesto ObtenerPresupuesto(int idPresupuesto)
    {
        Presupuesto presupuesto = null;
        string query = @"SELECT 
                            P.idPresupuesto,
                            P.NombreDestinatario,
                            P.FechaCreacion,
                            PR.idProducto,
                            PR.Descripcion AS Producto,
                            PR.Precio,
                            PD.Cantidad,
                            (PR.Precio * PD.Cantidad) AS Subtotal
                        FROM
                            Presupuestos P
                        JOIN
                            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                        JOIN
                            Productos PR ON PD.idProducto = PR.idProducto
                        WHERE
                            P.idPresupuesto = @idPresupuesto";
        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
        {
            connection.Open(); //abro conexion
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (presupuesto == null)
                    {
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestosDetalle detalle = new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    presupuesto.Detalle.Add(detalle);
                }
            }
            connection.Close();
        }

        return presupuesto;
    }

    public bool AgregarProductoYCantidad(int idPresupuesto, int idProducto, int cantidad)
    {
        ProductoRepository productoRepository = new ProductoRepository(cadenaDeConexion);
        if (ObtenerPresupuesto(idPresupuesto) == null || productoRepository.ObtenerProducto(idProducto) == null) //Control que exista el presupuesto y el producto
        {
            return false;
        }
        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad);";
        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
        {
            connection.Open(); //abro conexion
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.Parameters.AddWithValue("@cantidad", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }

        return true;
    }

    public bool EliminarPresupuesto(int idPresupuesto)
    {
        if (ObtenerPresupuesto(idPresupuesto) == null) //Control que exista el presupuesto 
        {
            return false;
        }
        string query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto;";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto;";
        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteCommand command2 = new SqliteCommand(query2, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command2.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command2.ExecuteNonQuery();
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }
}