----------------------------------------------------------------------------------
-- STORED PROCEDURES
----------------------------------------------------------------------------------
CREATE DATABASE StoredProceduresDB;
USE StoredProceduresDB;

-- queries we write in general are called 'adhoc' queries ---> anyone can write this
-- this creates an execution plan for every query
-- General control flow:
	-- When you run a SQL query, the database engine:
	-- Parses the SQL,
	-- Validates it,
	-- Optimizes it, and
	-- Creates an execution plan to determine how to retrieve the data.

-- Stored procedures - Benefits
-- 1. The execution plan is generated once (on first execution) and cached. 
-- 2. Future executions reuse the cached plan, saving time on compilation and execution plan are skipped.
-- 3. We can encrypt
-- 4. Secure
-- 5. Reducing complexity

-- A procedure must be the only statement in the batch

CREATE PROCEDURE proc_FirstProcedure
AS
BEGIN
PRINT 'Hello, world!'
END
GO
EXEC proc_FirstProcedure;

-- Create Products table
CREATE TABLE Products (
    Id INT IDENTITY(1,1) CONSTRAINT PK_ProductId PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Details NVARCHAR(MAX)
);
GO

-- Procedure to insert a product
CREATE PROC Proc_InsertProduct (
    @PName NVARCHAR(100),
    @PDetails NVARCHAR(MAX)
)
AS
BEGIN
    INSERT INTO Products (Name, Details)
    VALUES (@PName, @PDetails);
END;
GO

-- Insert sample product using the procedure
EXEC Proc_InsertProduct 
    'Laptop', 
    '{"brand":"HP","spec":{"ram":"16GB","cpu":"i5"}}';
GO

-- View all products
SELECT * 
FROM Products;

-- Extract the 'spec' part of JSON from Details
SELECT 
    JSON_QUERY(Details, '$.spec') AS Product_Specification
FROM Products;

-- Procedure to update RAM specification
CREATE PROC Proc_UpdateProductSpec (
    @PId INT,
    @NewValue VARCHAR(20)
)
AS
BEGIN
    UPDATE Products
    SET Details = JSON_MODIFY(Details, '$.spec.ram', @NewValue)
    WHERE Id = @PId;
END;
GO

-- Update the RAM spec to 24GB for product with Id = 1
EXEC Proc_UpdateProductSpec 1, '24GB';


SELECT id, name, JSON_VALUE(details, '$.brand') BrandName from Products;

-- Create Posts table
CREATE TABLE Posts (
    Id INT PRIMARY KEY,
    Title NVARCHAR(100),
    User_Id INT,
    Body NVARCHAR(MAX)
);
GO

-- View all posts
SELECT * FROM Posts;

-- Create stored procedure to bulk insert posts from JSON
CREATE PROCEDURE Proc_BulkInsertPosts (@JsonData NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO Posts (User_Id, Id, Title, Body)
    SELECT 
        userId, 
        id, 
        title, 
        body
    FROM OPENJSON(@JsonData)
    WITH (
        userId INT,
        id INT,
        title NVARCHAR(100),
        body NVARCHAR(MAX)
    );
END;
GO

-- Delete existing posts
DELETE FROM Posts;

-- Execute stored procedure to insert posts
EXEC Proc_BulkInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }
]';

DROP PROCEDURE Proc_BulkInsertPosts;


----
SELECT * FROM Products WHERE 
  TRY_CAST(JSON_VALUE(details,'$.spec.cpu') as nvarchar(20)) ='i5'

--------------------------------------------------------------------------------
					--			T A S K 
--------------------------------------------------------------------------------
-- create a procedure that brings posts by taking user_id as paramter

CREATE PROCEDURE Proc_GetPostsByUserId (@UserID INT)
AS 
	BEGIN
	SELECT * FROM Posts 
	WHERE user_id = @UserID;
END;
EXEC Proc_GetPostsByUserId 1;
