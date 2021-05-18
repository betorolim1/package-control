using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbSession _session;

        public UnitOfWork(DbSession session)
        {
            _session = session;
        }

        public async Task BeginTransactionAsync()
        {
            if (_session.Connection.State != System.Data.ConnectionState.Open)
                await _session.OpenAsync();

            _session.Transaction = _session.Connection.BeginTransaction();
        }

        public void Commit()
        {
            _session.Transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _session.Transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _session.Transaction?.Dispose();
        }
    }
}
