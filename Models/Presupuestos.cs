    public class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateTime fechaCreacion;
        private List<PresupuestosDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
        public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }

        public Presupuesto(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion)
        {
            this.idPresupuesto = idPresupuesto;
            this.nombreDestinatario = nombreDestinatario;
            this.fechaCreacion = fechaCreacion;
            detalle = new List<PresupuestosDetalle>();
        }

        public Presupuesto()
        {

        }

        public float MontoPresupuesto()
        {
            return 0;
        }

        public float MontoPresupuestoConIva()
        {
            return 0;
        }
        public int CantidadProductos()
        {
            return 0;
        }

    }
