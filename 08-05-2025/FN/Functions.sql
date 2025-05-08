USE pubs
GO

--------------------------------------------------------------------------------
--				F U N C T I O N S
--------------------------------------------------------------------------------

CREATE FUNCTION Fn_CalculateTax (@BasePrice FLOAT, @Tax FLOAT)
RETURNS FLOAT
AS
BEGIN
    RETURN (@BasePrice + (@BasePrice * @Tax / 100))
END;

-- Example: Call the function with specific values
SELECT DBO.Fn_CalculateTax(1000, 10);

-- Use the function to calculate tax-adjusted prices from the Titles table
SELECT Title, DBO.Fn_CalculateTax(Price, 12) AS PriceWithTax
FROM Titles;




-- Inline Table-Valued Function
CREATE FUNCTION Fn_TableSample (@MinPrice FLOAT)
RETURNS TABLE
AS
    RETURN 
    SELECT Title, Price 
    FROM Titles 
    WHERE Price >= @MinPrice;

-- Drop the function if needed
DROP FUNCTION Fn_TableSample;

-- Usage of inline function
SELECT * FROM DBO.Fn_TableSample(10);


-- Multi-statement Table-Valued Function (older and slower, but supports more logic)
CREATE FUNCTION Fn_TableSampleOld (@MinPrice FLOAT)
RETURNS @Result TABLE (
    Book_Name NVARCHAR(100), 
    Price FLOAT
)
AS
BEGIN
    INSERT INTO @Result
    SELECT Title, Price 
    FROM Titles 
    WHERE Price >= @MinPrice;
    RETURN;
END;

-- Usage of multi-statement function
SELECT * FROM DBO.Fn_TableSampleOld(10);
