namespace PackageControl.Core.Package.Commands
{
    public class InsertPackageCommand
    {
        public bool IsStoppedInCustoms { get; set; }
        public bool HasValueToPay { get; set; }
        public string AreaToDeliver { get; set; }
        public int PackageWeight { get; set; }
        public byte Size { get; set; }
        public bool IsFragile { get; set; }
        public byte ActualStatus { get; set; }
        public decimal Price { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public byte TypeOfControl { get; set; }
        public byte PlaceType { get; set; }
    }
}
