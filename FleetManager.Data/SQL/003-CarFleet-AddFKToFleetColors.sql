/*
FK link from CarFleet to FleetColors with FK constraint.
*/
ALTER TABLE [CarFleet] ALTER COLUMN [Color_Id] BIGINT NOT NULL
ALTER TABLE [CarFleet] 
	ADD CONSTRAINT 
	FK_CarFleet_FleetColors FOREIGN KEY  (Color_Id) REFERENCES FleetColors(Id)