using PackageControl.Shared;
using System;

namespace PackageControl.Query.Package.Result
{
    public class PackageResult
    {
        public long Id { get; set; }
        public DateTime ReceiveDate { get; set; }
        public bool IsStoppedInCustoms { get; set; }
        public bool HasValueToPay { get; set; }
        public string TrackingCode { get; set; }
        public int PackageWeight { get; set; }
        public string Size { get; set; }
        public bool IsFragile { get; set; }
        public string ActualStatus { get; set; }
        public decimal Price { get; set; }
        public string AreaToDeliver { get; set; }

        public LastCheckpointResult LastCheckpoint { get; set; }
    }
}
