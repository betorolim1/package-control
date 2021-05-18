using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        void Commit();
        void Rollback();
    }
}
