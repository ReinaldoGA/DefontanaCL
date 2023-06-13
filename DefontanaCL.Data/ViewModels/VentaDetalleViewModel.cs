using DefontanaCL.Model;

namespace DefontanaCL.Data.ViewModels
{
    public class VentaDetalleViewModel
    {
        public long IdVentaDetalle { get; set; }

        public long IdVenta { get; set; }

        public int PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public int TotalLinea { get; set; }

        public long IdProducto { get; set; }

        public virtual ProductoViewModel IdProductoNavigation { get; set; } = null!;

        public static explicit operator VentaDetalleViewModel(VentaDetalle v)
        {
            throw new NotImplementedException();
        }
    }

}