-- Create the database 'test' if it does not exist
DROP DATABASE test;
CREATE DATABASE IF NOT EXISTS `test`;

-- Use the database 'test'
USE `test`;

-- Drop existing tables if they exist
DROP TABLE IF EXISTS `Rentals`;
DROP TABLE IF EXISTS `Equipments`;
DROP TABLE IF EXISTS `Customers`;
DROP TABLE IF EXISTS `Categories`;

-- Create Categories table
CREATE TABLE `Categories`
(
    `Id`  INT PRIMARY KEY,
    `Name`        VARCHAR(255) NOT NULL UNIQUE,
    `Description` VARCHAR(255) NOT NULL
);

-- Create Customers table
CREATE TABLE `Customers`
(
    `Id` INT PRIMARY KEY,
    `FirstName`  VARCHAR(255) NOT NULL,
    `LastName`   VARCHAR(255) NOT NULL,
    `Phone`      VARCHAR(255) NOT NULL UNIQUE,
    `Email`      VARCHAR(255) NOT NULL UNIQUE
);

-- Create Equipments table
CREATE TABLE `Equipments`
(
    `Id` INT PRIMARY KEY,
    `CategoryId`  INT          NOT NULL,
    `Name`        VARCHAR(255) NOT NULL UNIQUE,
    `Description` VARCHAR(255) NOT NULL,
    `DailyCost`   DOUBLE       NOT NULL,
    FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`)
);

-- Create Rentals table
CREATE TABLE `Rentals`
(
    `Id`    INT PRIMARY KEY,
    `Date`        DATETIME NOT NULL,
    `CustomerId`  INT      NOT NULL,
    `EquipmentId` INT      NOT NULL,
    FOREIGN KEY (`CustomerId`) REFERENCES `Customers` (`Id`),
    FOREIGN KEY (`EquipmentId`) REFERENCES `Equipments` (`Id`)
);

-- Insert categories
INSERT INTO `Categories` (`Id`, `Name`, `Description`)
VALUES (10, 'Power tools', 'Tools powered by an external source or mechanism.'),
       (20, 'Yard equipment', 'Equipment used for yard and garden maintenance.'),
       (30, 'Compressors', 'Machines that increase the pressure of a gas by reducing its volume.'),
       (40, 'Generators', 'Devices that convert mechanical energy into electrical energy.'),
       (50, 'Air Tools', 'Tools powered by compressed air.');

-- Insert customers
INSERT INTO `Customers` (`Id`, `FirstName`, `LastName`, `Phone`, `Email`)
VALUES (1, 'John', 'Doe', '(555) 555-1212', 'jd@sample.net'),
       (2, 'Jane', 'Smith', '(555) 555-3434', 'js@live.com'),
       (3, 'Michael', 'Lee', '(555) 555-5656', 'ml@sample.net');

-- Insert equipment
INSERT INTO `Equipments` (`Id`, `CategoryId`, `Name`, `Description`, `DailyCost`)
VALUES (1, 10, 'Hammer drill', 'Powerful drill for concrete and masonry', 25.99),
       (2, 20, 'Chainsaw', 'Gas-powered chainsaw for cutting wood', 49.99),
       (3, 20, 'Lawn mower', 'Self-propelled lawn mower with mulching function', 19.99),
       (4, 30, 'Small Compressor', '5 Gallon Compressor-Portable', 14.99),
       (5, 50, 'Brad Nailer', 'Brad Nailer. Requires 3/4 to 1 1/2 Brad Nails', 10.99);
