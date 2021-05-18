CREATE TABLE Package(
	PackageId BIGINT PRIMARY KEY IDENTITY(1,1),
	LastCheckpointId BIGINT,
	ReceiveDate DATETIME,
	IsStoppedInCustoms BIT,
	HasValueToPay BIT,
	TrackingCode VARCHAR(255),
	PackageWeight INT,
	Size TINYINT,
	IsFragile BIT,
	ActualStatus TINYINT,
	Price INT,
	AreaToDeliver VARCHAR(255)
)