using DefontanaCL.Model;

namespace DefontanaCL.Data.ViewModels
{
    public class ProductoViewModel
    {
        public long IdProducto { get; set; }

        public string Nombre { get; set; } = null!;

        public string Codigo { get; set; } = null!;

        public long IdMarca { get; set; }

        public string Modelo { get; set; } = null!;

        public int CostoUnitario { get; set; }

        public virtual MarcaViewModel IdMarcaNavigation { get; set; } = null!;


    }
}