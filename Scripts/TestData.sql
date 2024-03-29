Use bookish;

DELETE FROM Loans;
DELETE FROM BookCopies;
DELETE FROM Books;

-- Default Test values

INSERT INTO Books VALUES 
('9780132350884', 'Clean Code', 'Robert C. Martin'),
('9780590353403', 'Harry Potter And The Sorcerers Stone', 'J. K. Rowling'),
('9780439064866', 'Harry Potter and the Chamber of Secrets', 'J. K. Rowling');

INSERT INTO BookCopies VALUES
('9780132350884', 0),
('9780132350884', 1),
('9780132350884', 1);

INSERT INTO Loans VALUES 
('2', 'person', '2019-09-12', 0),
('3', 'person2', '2019-09-10', 0);


SELECT *
FROM Books;

SELECT *
FROM BookCopies;

SELECT *
FROM Loans;