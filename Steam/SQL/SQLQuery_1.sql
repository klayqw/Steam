create DATABASE Steam

use Steam

create table Logs(
    [Id] int primary key identity,
    [UserId] nvarchar(max),
    [Url] NVARCHAR(max),
    [MethodType] nvarchar(10),
    [StatusCode] nvarchar(10),
)

drop table Users

create table Users(
    [Id] int PRIMARY key IDENTITY,
    [Login] NVARCHAR(20) not null,
    [Password] NVARCHAR(20) not null,
    [Email] NVARCHAR(50) not null,
)


drop table Games

create table Games(
    [Id] int PRIMARY KEY IDENTITY,
    [Title] NVARCHAR(50) not null,
    [Description] NVARCHAR(max) not null,
    [Devoloper] NVARCHAR(100) not null,
    [Publisher] NVARCHAR(100) not null,
    [Price] FLOAT not null,
    [ReleaseDate] datetime not null,
    [Genre] NVARCHAR(100) not null,
)

select * from Logs

SELECT * from Games

select * from Users