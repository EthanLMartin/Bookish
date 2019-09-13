DROP DATABASE IF EXISTS bookish;
CREATE DATABASE bookish;
USE bookish;

CREATE TABLE Books (
    ISBN CHAR(13) NOT NULL PRIMARY KEY,
    title VARCHAR(80),
    author VARCHAR(255)
);


CREATE TABLE BookCopies (
    barcode INT IDENTITY(1, 1) PRIMARY KEY,
    ISBN CHAR(13) NOT NULL FOREIGN KEY REFERENCES Books(ISBN),
    borrowed BIT NOT NULL
);

CREATE TABLE Loans (
    barcode INT FOREIGN KEY REFERENCES BookCopies(barcode),
    userId VARCHAR(30) NOT NULL,
    dueDate DATE,
    completed BIT NOT NULL
);

USE master;
