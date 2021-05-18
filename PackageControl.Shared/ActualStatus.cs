namespace PackageControl.Shared
{
    public enum ActualStatus
    {
        Received = 0,
        InTransit = 1,
        StoppedByLegal = 2,
        AttempedDelivery = 3,
        Returning = 4,
        Delivered = 5
    }
}
