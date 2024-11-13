using Microsoft.Data.Sqlite;

using System.Data;

    public class ProductoRepository : IProductoRepository
    {
        private string cadenaDeConexion;
        public ProductoRepository(string cadenaDeConexion) //constructor del repositorio recibe la cadena de conexion
        {
            this.cadenaDeConexion = cadenaDeConexion;
        }

        public void CrearProducto(Producto producto)
        {

            string queryString = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)"; //consulta

            using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion)) //creo conexion
            {
                connection.Open(); //abro conexion
                SqliteCommand command = new SqliteCommand(queryString, connection); //comando con la consulta y conexion
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion); //parametrizo la consulta
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.ExecuteNonQuery(); //ejecuto el comando
                connection.Close(); //cierro conexion
            }

        }

        public void ModificarProducto(int idProducto, Producto producto)
        {
            string queryString = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id"; //actualizo descripcion y precio donde el id del producto sea igual al id recibido

            using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion); //parametrizando consulta
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Id", idProducto);
                command.ExecuteNonQuery(); //ejecuto el comando actualizando
                connection.Close();
            }
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>(); //creo lista de productos donde agregare productos que lea el reader
            string queryString = @"SELECT * FROM Productos"; //consulta

            using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion)) //creo conexion
            {
                connection.Open(); //abro conexion
                SqliteCommand command = new SqliteCommand(queryString, connection); //comando con la consulta y conexion
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Producto producto = new Producto();
                        producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                        producto.Descripcion = reader["Descripcion"].ToString();
                        producto.Precio = Convert.ToInt32(reader["Precio"]);
                        productos.Add(producto); //agrego el nuevo producto a la lista que se devolvera
                    }
                }
                connection.Close(); //cierro conexion
            }
            return productos;
        }

        public Producto ObtenerProducto(int idProducto)
        {
            Producto producto = null;
            string queryString = @"SELECT * FROM Productos WHERE idProducto = @id ";

            using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@id", idProducto);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = new Producto();
                        producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                        producto.Descripcion = reader["Descripcion"].ToString();
                        producto.Precio = Convert.ToInt32(reader["Precio"]);
                    }

                }
                connection.Close();
            }
            return producto;
        }

        public void EliminarProducto(int idProducto)
        {
            string queryString = @"DELETE  FROM Productos WHERE idProducto = @id ";
            using (SqliteConnection connection = new SqliteConnection(cadenaDeConexion))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.AddWithValue("@id", idProducto);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }