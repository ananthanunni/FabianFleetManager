/*
FK link from CarFleet to FleetMakes with FK constraint.
*/
ALTER TABLE [CarFleet] ALTER COLUMN [FleetMakes_Id] BIGINT NOT NULL
ALTER TABLE [CarFleet] 
	ADD CONSTRAINT 
	FK_CarFleet_FleetMakes FOREIGN KEY  (FleetMakes_Id) REFERENCES FleetMakes(Id)