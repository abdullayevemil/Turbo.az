create database TurboazDb;

use TurboazDb;

create table Vehicles (
    [Id] int primary key identity,
    [Price] money,
    [Brand] nvarchar(max),
    [Model] nvarchar(max),
    [EngineVolume] int
)