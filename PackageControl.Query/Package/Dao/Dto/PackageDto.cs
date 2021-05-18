using System;
using System.Collections.Generic;
using System.Text;

namespace PackageControl.Query.Package.Dao.Dto
{
    public class PackageDto
    {
        public long Id { get; set; }
        public DateTime ReceiveDate { get; set; }
        public bool IsStoppedInCustoms { get; set; }
        public bool HasValueToPay { get; set; }
        public string TrackingCode { get; set; }
        public int PackageWeight { get; set; }
        public byte Size { get; set; }
        public bool IsFragile { get; set; }
        public byte ActualStatus { get; set; }
        public decimal Price { get; set; }
        public string AreaToDeliver { get; set; }

        public LastCheckpointDto LastCheckpoint { get; set; }
    }
}
