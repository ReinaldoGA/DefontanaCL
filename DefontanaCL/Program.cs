// See https://aka.ms/new-console-template for more information
using AutoMapper;
using DefontanaCL.Application.Interfaces.Repositories;
using DefontanaCL.Application.Services;
using DefontanaCL.Data.Mapper;
using DefontanaCL.Data.ViewModels;
using DefontanaCL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DefontanaCL
{
    internal class Program
    {
       
        static async Task Main(string[] args)
        {
            #region "Initial Configuration"
            var builder = new ConfigurationBuilder()
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var services = new ServiceCollection();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDbContext<PruebaContext>(options =>
                options.UseSqlServer(builder.GetConnectionString("DefaultConnection")));
            #endregion

            #region "Mapper Configuration"
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VentasProfile>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            #endregion

            VentasService instance = new VentasService(mapper);

            var result = await instance.GetAllVentasWithIncludeAsync(30);

            GetAndPrintOnScreen(result);

            Console.WriteLine("Presione una tecla para cerrar el programa");
            Console.ReadLine();

        }

        private static void GetAndPrintOnScreen(IEnumerable<VentasViewModel> result)
        {

            // El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)
            decimal totalSalesAmount = result.Sum(s => s.Total);
            int totalSalesCount = result.Count();

            Console.WriteLine("El total de ventas de los últimos 30 días (monto total y cantidad total de ventas)");
            Console.WriteLine($"Total: {totalSalesAmount.ToString("n2")} Cantidad:  {totalSalesCount}");
            Console.WriteLine($"---------------------------------------------------------------------");
            
            // El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto)
            var saleWithHighestAmount = result
                .OrderByDescending(s => s.Total)
                .FirstOrDefault();
            DateTime? highestSaleDateTime = saleWithHighestAmount?.Fecha;
            decimal? highestSaleAmount = saleWithHighestAmount?.Total;
            
            Console.WriteLine("El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto)");
            Console.WriteLine($"Fecha/Hora: {highestSaleDateTime} Monto Mas Alto:  {highestSaleAmount.GetValueOrDefault().ToString("n2")}");
            Console.WriteLine($"---------------------------------------------------------------------");


            // Indicar cuál es el producto con mayor monto total de ventas
            var productWithHighestTotalSales = result
                    .Join(result.SelectMany(x => x.VentaDetalles), venta => venta.IdVenta, detalle => detalle.IdVenta, (venta, detalle) => new { Venta = venta, Detalle = detalle })
                    .GroupBy(x => x.Detalle.IdProducto)
                    .Select(group => new
                    {
                        ProductID = group.Key,
                        TotalSales = group.Sum(x=>x.Venta.Total)
                    })
                    .OrderByDescending(x => x.TotalSales)
                    .FirstOrDefault();

            decimal? productIDWithHighestTotalSales = productWithHighestTotalSales.TotalSales;
            var  productID = productWithHighestTotalSales.ProductID;
            var productWithHighestTotalSalesName = result?.SelectMany(x => x.VentaDetalles)?.Where(x=>x.IdProducto == productID)?.Select(x=>x.IdProductoNavigation)?.FirstOrDefault()?.Nombre ;
            
            Console.WriteLine("Indicar cuál es el producto con mayor monto total de ventas");
            Console.WriteLine($"Producto: {productWithHighestTotalSalesName}");
            Console.WriteLine($"---------------------------------------------------------------------");


            // Indicar el local con mayor monto de ventas
            var storeWithHighestSales = result?
                .GroupBy(s => new { s.IdLocalNavigation.Nombre} )
                .Select(group => new
                {
                    Store = group.Key,
                    TotalSalesAmount = group.Sum(s => s.Total)
                })
               .OrderByDescending(s => s.TotalSalesAmount)
                .FirstOrDefault();
            string? storeWithHighestSalesName = storeWithHighestSales?.Store.Nombre;

            Console.WriteLine("Indicar el local con mayor monto de ventas");
            Console.WriteLine($"Local: {storeWithHighestSalesName}");
            Console.WriteLine($"---------------------------------------------------------------------");


            // ¿Cuál es la marca con mayor margen de ganancias?

            var brandWithHighestProfitMargin = result
            .Join(result?.Select(x => x.VentaDetalles)?.Select(a => a)?.SelectMany(p => p), venta => venta.IdVenta, detalle => detalle.IdVenta, (venta, detalle) => new { Venta = venta, Detalle = detalle })
            .Join(result.Select(x => x.VentaDetalles).Select(a => a.Select(x => x.IdProductoNavigation)).SelectMany(p => p), ventaDetalle => ventaDetalle.Detalle.IdProducto, producto => producto.IdProducto, (ventaDetalle, producto) => new { VentaDetalle = ventaDetalle, Producto = producto })
            .Join(result.Select(x => x.VentaDetalles).Select(a => a.Select(x => x.IdProductoNavigation).Select(q => q.IdMarcaNavigation)).SelectMany(p => p), producto => producto.Producto.IdMarca, marca => marca.IdMarca, (producto, marca) => new { Producto = producto, Marca = marca })
            .Select(x => new
            {
                Marca = x.Marca,
                ProfitMargin = (x.Producto.VentaDetalle.Detalle.TotalLinea - (x.Producto.Producto.CostoUnitario * x.Producto.VentaDetalle.Detalle.Cantidad)) / (x.Producto.Producto.CostoUnitario * x.Producto.VentaDetalle.Detalle.Cantidad)
            })
            .OrderByDescending(x => x.ProfitMargin)
            .FirstOrDefault();

            string? brandWithHighestProfitMarginName = brandWithHighestProfitMargin?.Marca?.Nombre;

            Console.WriteLine("¿Cuál es la marca con mayor margen de ganancias?");
            Console.WriteLine($"Marca: {brandWithHighestProfitMarginName}");
            Console.WriteLine($"---------------------------------------------------------------------");



            // ¿Cómo obtendrías cuál es el producto que más se vende en cada local?
        

            var topSellingProductsByLocal = result
                .GroupJoin(result.SelectMany(x=>x.VentaDetalles),
                    venta => venta.IdVenta,
                    detalle => detalle.IdVenta,
                    (venta, detalles) => new { Venta = venta, Detalles = detalles })
                .SelectMany(x => x.Detalles,
                    (venta, detalle) => new { Venta = venta.Venta, Detalle = detalle })
                .GroupBy(x => new { x.Venta.IdLocal, x.Detalle.IdProducto})
                .Select(group => new
                {
                    LocalID = group.Key.IdLocal,
                    ProductID = group.Key.IdProducto,
                    TotalSales = group.Sum(x=>x.Detalle.Cantidad),
                    Product = group.First().Detalle.IdProductoNavigation,
                    Local = group.First().Venta.IdLocalNavigation
                })
                .GroupBy(x => x.LocalID)
                .Select(group => group.OrderByDescending(x => x.TotalSales).FirstOrDefault())
                .ToList();
    

            Console.WriteLine("¿Cómo obtendrías cuál es el producto que más se vende en cada local?");


            Console.WriteLine("-----------------------------");
            Console.WriteLine("|Producto          |  Local |");
            Console.WriteLine("-----------------------------");

            foreach (var sale in topSellingProductsByLocal)
            {
                Console.WriteLine($"| {sale.Product.Nombre}| {sale?.Local.Nombre} |");
            }

            Console.WriteLine("-----------------------------");

            Console.WriteLine($"---------------------------------------------------------------------");


        }
    }


}