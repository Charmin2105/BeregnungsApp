CREATE TABLE [dbo].[BeregnungsDatens]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [StartDatum] DATETIMEOFFSET NULL, 
    [StartUhrzeit] DATETIME NULL, 
    [EndDatum] DATETIMEOFFSET NULL, 
    [Betrieb] NVARCHAR(50) NULL, 
    [SchlagId] UNIQUEIDENTIFIER NOT NULL, 
    [Duesse] NVARCHAR(50) NULL, 
    [WasseruhrAnfang] INT NULL, 
    [WasseruhrEnde] INT NULL, 
    [Vorkomnisse] NVARCHAR(50) NULL, 
    [IstAbgeschlossen] BIT NULL
)
