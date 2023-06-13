
using DefontanaCL.Data.ViewModels;
using DefontanaCL.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DefontanaCL.Data.Services
{
    public class VentasModelServices
    {

        public static async Task<IEnumerable<Ventum>> GetAllVentas(int NumberOfDays)
        {
            try
            {


                using (var context = new PruebaContext())
                {

                    var query = context.Venta
                        .Include(a => a.IdLocalNavigation)
                        .Include(x => x.VentaDetalles)
                            .ThenInclude(j => j.IdProductoNavigation)
                            .ThenInclude(c => c.IdMarcaNavigation)
                        .Where(v => v.Fecha >= DateTime.Now.AddDays(-NumberOfDays))
                        .ToListAsync();

                    return await query;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
