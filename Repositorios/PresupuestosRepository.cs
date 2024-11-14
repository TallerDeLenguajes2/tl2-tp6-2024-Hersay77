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
        string query1 = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion)"; //consulta para tabla presupuesto, idPresupuesto es una primarykey autoincremental

        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion)) //creo conexion
        {
            connection.Open(); //abro conexion
            SqliteCommand command = new SqliteCommand(query1, connection); //comando con la consulta y conexion
            command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
            command.ExecuteNonQuery(); //ejecuto el comando
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

        //a diferencia del tp5 uso left join para las consultas asi devuelva filas con los valores DBnull en caso de que solo haya datos en tabla presupuestos y no se hallan agregado detalles por lo tanto sno habra productos asociados
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
                        LEFT JOIN
                            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
                        LEFT JOIN
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
                    if (reader["idProducto"] != DBNull.Value) //se controla que en la fila no haya un valor nulo entonces no se intenta convertir ni agregar detales al presupuesto
                    //tambien podria usar if (!reader.IsDBNull(reader.GetOrdinal("idProducto"))) Aquí, reader.GetOrdinal("idProducto") busca el índice de la columna idProducto en la fila actual, y luego IsDBNull() verifica si el valor en esa posición es NULL. Por si el orden de la consulta cambia y el siguiente datos a obtener no es idproducto, no importara ya que devolvera DBnull el indice de idproducto sea en el orden que este en la consulta
                    {
                        Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                        PresupuestosDetalle detalle = new PresupuestosDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalle.Add(detalle);
                    }
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

    public void ModificarPresupuesto(Presupuesto presupuesto)
    {
        string query = @"UPDATE Presupuestos SET NombreDestinatario = @destinatario, FechaCreacion = @fecha WHERE idPresupuesto = @Id";

        using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.Parameters.AddWithValue("@Id", presupuesto.IdPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}