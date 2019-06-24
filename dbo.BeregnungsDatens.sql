CREATE TABLE [dbo].[BeregnungsDatens] (
    [ID]               UNIQUEIDENTIFIER   NOT NULL,
    [StartDatum]       DATETIMEOFFSET (7) NULL,
    [StartUhrzeit]     DATETIME           NULL,
    [EndDatum]         DATETIMEOFFSET (7) NULL,
    [BetriebID]        UNIQUEIDENTIFIER   NOT NULL,
    [SchlagId]         UNIQUEIDENTIFIER   NOT NULL,
    [Duese]            NVARCHAR (50)      NULL,
    [WasseruhrAnfang]  INT                NULL,
    [WasseruhrEnde]    INT                NULL,
    [Vorkomnisse]      NVARCHAR (50)      NULL,
    [IstAbgeschlossen] BIT                NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_BeregnungsDaten_Betrieb_BetriebID] FOREIGN KEY ([BetriebID]) REFERENCES [dbo].[Betriebe] ([Id]) ON DELETE CASCADE
);

