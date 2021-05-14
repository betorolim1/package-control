CREATE TABLE LastCheckpoint(
	LastCheckpointId BIGINT PRIMARY KEY,
	Country VARCHAR(3),
	City VARCHAR(255),
	TypeOfControl TINYINT,
	PlaceType TINYINT
)