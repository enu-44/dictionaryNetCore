namespace FED
{
    public class Libro
    {
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string autor { get; set; }
        public int añoPublicacion { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double costoProduccion { get; set; }
    }

    public class VentaLibro {
        public double costoTotal {get; set;}
        public double descuentoTotal {get; set;}
        public double porcentajeDescuento {get; set;}
    }
}