CREATE TABLE LastCheckpoint(
	LastCheckpointId BIGINT PRIMARY KEY IDENTITY(1,1),
	Country VARCHAR(255),
	City VARCHAR(255),
	TypeOfControl TINYINT,
	PlaceType TINYINT
)