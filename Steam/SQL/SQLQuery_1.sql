create DATABASE Steam

use Steam

create table Logs(
    [Id] int primary key identity,
    [UserId] nvarchar(max),
    [Url] NVARCHAR(max),
    [MethodType] nvarchar(max),
    [StatusCode] nvarchar(max),
    [RequestBody] NVARCHAR(max),
)

ALTER TABLE Logs
ADD ResponseBody nvarchar(max);