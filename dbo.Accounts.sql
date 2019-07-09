CREATE TABLE [dbo].[Accounts] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Benutzername] VARCHAR (50)     NULL,
    [Passwort]     VARCHAR (50)     NULL,
    [EMail]        VARCHAR (50)     NULL,
    [istAdmin] BIT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

