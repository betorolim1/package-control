CREATE TABLE Package(
	PackageId BIGINT PRIMARY KEY,
	LastCheckpoint BIGINT,
	ReceiveDate DATETIME,
	IsStoppedInCustoms BIT,
	HasValueToPay BIT,
	TrackingCode VARCHAR(255),
	PackageWeight INT,
	Size TINYINT,
	IsFragile BIT,
	ActualStatus TINYINT
)