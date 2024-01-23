create database VehiclesDb;

use VehiclesDb;

create table Vehicles 
(
    [Id] int primary key identity,
    [BrandName] nvarchar(100),
    [ModelName] nvarchar(100),
    [Price] money,
    [EngineVolume] int,
    [ImageUrl] nvarchar(max)
)