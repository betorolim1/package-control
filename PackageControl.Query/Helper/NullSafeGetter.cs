using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Query.Helper
{
    public static class NullSafeGetter
    {
        public static async Task<T> GetValueOrDefaultAsync<T>(this DbDataReader reader, string campo)
        {
            var num = reader.GetOrdinal(campo);

            if (await reader.IsDBNullAsync(num))
                return default(T);

            return await reader.GetFieldValueAsync<T>(num);
        }
    }
}
