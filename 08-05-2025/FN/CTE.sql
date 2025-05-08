USE pubs
GO

WITH CTE_Authors AS (
    SELECT AU_ID, CONCAT(AU_FNAME, ' ', AU_LNAME) AS Author_Name
    FROM Authors
)
SELECT *
FROM CTE_Authors;




DECLARE @Page INT = 2, @PageSize INT = 10;

WITH PaginatedBooks AS
(
    SELECT TITLE_ID, TITLE, PRICE, ROW_NUMBER() OVER (ORDER BY PRICE DESC) AS RowNum
    FROM TITLES
)


SELECT * 
FROM PaginatedBooks 
WHERE RowNum BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize);


--create a sp that will take the page number and size as param and print the books

CREATE OR ALTER PROCEDURE Proc_PaginateTitles (@Page INT = 1, @PageSize INT = 10)
AS
BEGIN
    WITH PaginatedBooks AS
    (
        SELECT Title_Id, Title, Price, 
               ROW_NUMBER() OVER (ORDER BY Price DESC) AS RowNum
        FROM Titles
    )
    SELECT * 
    FROM PaginatedBooks 
    WHERE RowNum BETWEEN ((@Page - 1) * @PageSize + 1) AND (@Page * @PageSize);
END;

EXEC Proc_PaginateTitles 2, 5;

-- Alternative using OFFSET-FETCH, iNTRODUCED IN 2012
SELECT Title_Id, Title, Price
FROM Titles
ORDER BY Price DESC
OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;
