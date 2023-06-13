using DefontanaCL.Model;

namespace DefontanaCL.Data.ViewModels
{
    public class MasterViewModel
    {
        public VentasViewModel? Ventas { get; set; }
        public VentaDetalleViewModel? Detalle { get; set; }
    }
}