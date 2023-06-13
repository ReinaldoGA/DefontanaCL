using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DefontanaCL.Data.ViewModels;
using DefontanaCL.Model;

namespace DefontanaCL.Data.Mapper
{
    public class VentasProfile : Profile
    {
        public VentasProfile() 
        {
            CreateMap<Ventum, VentasViewModel>();
            CreateMap<VentaDetalle, VentaDetalleViewModel>();
            CreateMap<Local, LocalViewModel>();
            CreateMap<Producto, ProductoViewModel>();
            CreateMap<Marca, MarcaViewModel>();
        }
    }
}
