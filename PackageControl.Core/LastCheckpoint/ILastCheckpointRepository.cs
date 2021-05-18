using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PackageControl.Core.LastCheckpoint
{
    public interface ILastCheckpointRepository
    {
        Task<long> InsertLastCheckpoint(string country, string city, byte typeOfControl, byte placeType);
    }
}
