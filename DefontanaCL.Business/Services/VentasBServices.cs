using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefontanaCL.Application.ViewModels;
using DefontanaCL.Data.Services;

namespace DefontanaCL.Business.Services
{
    public class VentasBServices 
    {
        private VentasModelServices consultaData = new VentasModelServices();


        public async Task<IEnumerable<VentasViewModel>> GetAllVentasWithIncludeAsync()
        {
             return await consultaData.GetAllVentas();
        }
    }
}
