CREATE TABLE [dbo].[Schlaege] (
    [Id]   UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (50)    NULL,
    [BetriebID]        UNIQUEIDENTIFIER   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Schlaege_Betrieb_BetriebID] FOREIGN KEY ([BetriebID]) REFERENCES [dbo].[Betriebe] ([Id]) ON DELETE CASCADE
);

