create DATABASE Steam

use Steam

create table Logs(
    [Id] int primary key identity,
    [UserId] nvarchar(max),
    [Url] NVARCHAR(max),
    [MethodType] nvarchar(10),
    [StatusCode] nvarchar(10),
    [RequestBody] NVARCHAR(max),
    [ResponseBody] NVARCHAR(max)
)


create table Users(
    [Id] int PRIMARY key IDENTITY,
    [Login] NVARCHAR(20) not null,
    [Password] NVARCHAR(20) not null,
    [Email] NVARCHAR(50) not null,
)

select * from Logs
select * from Users
