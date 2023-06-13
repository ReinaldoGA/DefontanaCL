using DefontanaCL.Model;

namespace DefontanaCL.Data.ViewModels
{
    public class MarcaViewModel
    {
        public long IdMarca { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<ProductoViewModel> Productos { get; set; } = new List<ProductoViewModel>();
    }
}