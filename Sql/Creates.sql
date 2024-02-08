create database TurboazDb;

use TurboazDb;

create table TransmissionTypes
(
    [Id] int primary key identity,
    [Type] nvarchar(max)
)

create table Drivetrains
(
    [Id] int primary key identity,
    [Type] nvarchar(max)
)

create table Users 
(
    [Id] int primary key identity,
    [Email] nvarchar(100),
    [Login] nvarchar(100),
    [Password] nvarchar(100),
)

create table Vehicles 
(
    [Id] int primary key identity,
    [UserLogin] nvarchar(100),
    [BrandName] nvarchar(100),
    [ModelName] nvarchar(100),
    [Price] money,
    [EngineVolume] int,
    [ImageUrl] nvarchar(max),
    [HorsePowers] int,
    [SeatsCount] int,
    [Color] nvarchar(100),
    [TransmissionType] int foreign key references TransmissionTypes(Id) on delete cascade,
    [Drivetrain] int foreign key references Drivetrains(Id) on delete cascade,
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