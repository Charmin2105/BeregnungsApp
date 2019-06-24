CREATE TABLE [dbo].[Mitarbeiters] (
    [Id]         UNIQUEIDENTIFIER   NOT NULL,
    [Vorname]    VARCHAR (50)       NULL,
    [Nachname]   VARCHAR (50)       NULL,
    [Geburtstag] DATETIMEOFFSET (7) NULL,
    [BetriebID]  UNIQUEIDENTIFIER   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Mitarbeiter_Betrieb_BetriebID] FOREIGN KEY ([BetriebID]) REFERENCES [dbo].[Betriebe] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Mitarbeiters_BetriebID]
    ON [dbo].[Mitarbeiters]([BetriebID] ASC);

