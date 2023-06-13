using AutoMapper;
using DefontanaCL.Application.Interfaces.Repositories;
using DefontanaCL.Data.Services;
using DefontanaCL.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaCL.Application.Services
{
    public  class VentasService : IVentasRepository
    {
         VentasModelServices ventas = new VentasModelServices();
         private  IMapper _mapper;

        public VentasService( IMapper mapper)
        {
            this._mapper = mapper;
        }

        public   async Task<IEnumerable<VentasViewModel>> GetAllVentasWithIncludeAsync(int NumberOfDays)
        {
            var datos = await VentasModelServices.GetAllVentas(NumberOfDays);
            return _mapper.Map<IEnumerable<VentasViewModel>>(datos); 
        }
    }
}
