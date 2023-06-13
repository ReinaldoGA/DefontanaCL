using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaCL.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IVentasRepository Ventas { get; }

    }
}
