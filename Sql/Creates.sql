create database VehiclesDb;

use VehiclesDb;

create table Vehicles 
(
    [Id] int primary key identity,
    [Brand] nvarchar(100),
    [Model] nvarchar(100),
    [Price] money,
    [EngineVolume] int,
)