create database StackOverFlow_Second

use StackOverFlow_Second

create table Forum(
    [Id] int PRIMARY KEY IDENTITY(1,1),
    [Title] NVARCHAR(max),
    [Description] NVARCHAR(max),
    [Like] int,
    [Dislike] int,
)

