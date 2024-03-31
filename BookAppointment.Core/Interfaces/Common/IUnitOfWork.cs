using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
