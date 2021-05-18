using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PackageControl.Shared
{
    public sealed class DbSession : IDisposable
    {
        public SqlConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IConfiguration configuration)
        {
            var conn = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;

            Connection = new SqlConnection(conn);
        }

        public async Task OpenAsync()
        {
            if (Connection.State != ConnectionState.Open)
                await Connection.OpenAsync();
        }

        public async Task CloseAsync()
        {
            await Connection?.CloseAsync();
        }

        public void Dispose()
        {
            Connection?.Close();
        }
    }
}
