CREATE TABLE [dbo].[AddressBook] (
    [AddBookID] INT            IDENTITY (1, 1) NOT NULL,
    [Fname]     NVARCHAR (MAX) NOT NULL,
    [Lname]     NVARCHAR (MAX) NOT NULL,
    [Address]   NVARCHAR (MAX) NOT NULL,
    [Nickname]  NVARCHAR (MAX) NULL,
    [PhoneNo]   BIGINT         NULL,
    [FaxNo]     BIGINT         NULL,
    [Email]     NVARCHAR (MAX) NULL,
    [Notes]     NVARCHAR (MAX) NULL,
    [Id]   NVARCHAR (450)            NOT NULL,
    CONSTRAINT [PK_dbo.AddressBook] PRIMARY KEY CLUSTERED ([AddBookID] ASC),
    CONSTRAINT [FK_dbo.AddressBook_dbo.AspNetUsers_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);