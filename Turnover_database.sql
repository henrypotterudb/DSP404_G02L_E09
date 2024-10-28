-- Creación de la base de datos
CREATE DATABASE Turnover_database;
GO

USE Turnover_database;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Contraseña NVARCHAR(100) NOT NULL
);
GO

-- Tabla de Conciertos
CREATE TABLE Conciertos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    FechaConcierto DATETIME NOT NULL,
    Lugar NVARCHAR(200) NOT NULL,
    EntradasPlateaDisponibles INT NOT NULL,
    EntradasVIPDisponibles INT NOT NULL,
    EntradasGeneralDisponibles INT NOT NULL
);
GO

-- Tabla de Entradas
CREATE TABLE Entradas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    ConciertoId INT NOT NULL,
    Seccion NVARCHAR(50) NOT NULL,  -- Platea, VIP, General
    Cantidad INT NOT NULL,
    FechaCompra DATETIME NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (ConciertoId) REFERENCES Conciertos(Id)
);
GO

-- Tabla de Historial de Compras (opcional si quieres un registro adicional)
CREATE TABLE HistorialCompras (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    ConciertoId INT NOT NULL,
    FechaCompra DATETIME NOT NULL,
    TotalComprado INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (ConciertoId) REFERENCES Conciertos(Id)
);
GO

USE Turnover_database;
GO

UPDATE Usuarios
SET Rol = 'Administrador'
WHERE Correo = 'henry_daniel.9@hotmail.com';


ALTER TABLE Usuarios
ADD Rol NVARCHAR(20);


ALTER TABLE Entradas
ADD TotalPagado DECIMAL(10, 2) NOT NULL DEFAULT 0.00;

ALTER TABLE Conciertos
ADD PrecioPlatea DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    PrecioVIP DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    PrecioGeneral DECIMAL(10, 2) NOT NULL DEFAULT 0.00;
