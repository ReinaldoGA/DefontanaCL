using DefontanaCL.Model;

namespace DefontanaCL.Data.ViewModels
{
    public class VentasViewModel
    {
        public long IdVenta { get; set; }

        public int Total { get; set; }

        public DateTime Fecha { get; set; }

        public long IdLocal { get; set; }

        public virtual LocalViewModel IdLocalNavigation { get; set; } = null!;

        public virtual ICollection<VentaDetalleViewModel> VentaDetalles { get; set; } = new List<VentaDetalleViewModel>();

    }
}