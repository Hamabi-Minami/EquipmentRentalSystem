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
CREATE TABLE `Categories` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Name` VARCHAR(255) NOT NULL UNIQUE,
    `Description` VARCHAR(255) NOT NULL
);

-- Create Customers table
CREATE TABLE `Customers` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `FirstName` VARCHAR(255) NOT NULL,
    `LastName` VARCHAR(255) NOT NULL,
    `Phone` VARCHAR(255) NOT NULL UNIQUE,
    `Email` VARCHAR(255) NOT NULL UNIQUE
);

-- Create Equipments table
CREATE TABLE `Equipments` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `CategoryId` INT NOT NULL,
    `Name` VARCHAR(255) NOT NULL UNIQUE,
    `Description` VARCHAR(255) NOT NULL,
    `DailyCost` DOUBLE NOT NULL,
    FOREIGN KEY (`CategoryId`) REFERENCES `Categories`(`Id`)
);

-- Create Rentals table
CREATE TABLE `Rentals` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Date` DATETIME NOT NULL,
    `CustomerId` INT NOT NULL,
    `EquipmentId` INT NOT NULL,
    FOREIGN KEY (`CustomerId`) REFERENCES `Customers`(`Id`),
    FOREIGN KEY (`EquipmentId`) REFERENCES `Equipments`(`Id`)
);

-- Insert categories
INSERT INTO `Categories` (`Name`, `Description`)
VALUES
('Power tools', 'Tools powered by an external source or mechanism.'),
('Yard equipment', 'Equipment used for yard and garden maintenance.'),
('Compressors', 'Machines that increase the pressure of a gas by reducing its volume.'),
('Generators', 'Devices that convert mechanical energy into electrical energy.'),
('Air Tools', 'Tools powered by compressed air.');

-- Insert customers
INSERT INTO `Customers` (`FirstName`, `LastName`, `Phone`, `Email`)
VALUES
('John', 'Doe', '(555) 555-1212', 'jd@sample.net'),
('Jane', 'Smith', '(555) 555-3434', 'js@live.com'),
('Michael', 'Lee', '(555) 555-5656', 'ml@sample.net');

-- Insert equipment
INSERT INTO `Equipments` (`CategoryId`, `Name`, `Description`, `DailyCost`)
VALUES
(1, 'Hammer drill', 'Powerful drill for concrete and masonry', 25.99),
(2, 'Chainsaw', 'Gas-powered chainsaw for cutting wood', 49.99),
(2, 'Lawn mower', 'Self-propelled lawn mower with mulching function', 19.99),
(3, 'Small Compressor', '5 Gallon Compressor-Portable', 14.99),
(5, 'Brad Nailer', 'Brad Nailer. Requires 3/4 to 1 1/2 Brad Nails', 10.99);
