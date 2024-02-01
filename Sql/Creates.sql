create database TurboazDb;

use TurboazDb;

create table Vehicles 
(
    [Id] int primary key identity,
    [BrandName] nvarchar(100),
    [ModelName] nvarchar(100),
    [Price] money,
    [EngineVolume] int,
    [ImageUrl] nvarchar(max),
)

create table Users 
(
    [Id] int primary key identity,
    [Email] nvarchar(100),
    [Login] nvarchar(100),
    [Password] nvarchar(100),
)

create table Logs
(
    [LogId] int primary key identity,
    [UserId] int,
    [Url] nvarchar(max),
    [MethodType] nvarchar(20),
    [StatusCode] int,
    [RequestBody] nvarchar(max),
    [ResponseBody] nvarchar(max)
)