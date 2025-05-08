--1) List all orders with the customer name and the employee who handled the order.
-- (Join Orders, Customers, and Employees)

SELECT 
    Customers.CompanyName AS CustomerName,
    Employees.FirstName + ' ' + Employees.LastName AS EmployeeName,
    Orders.OrderDate
FROM 
    Orders
JOIN 
    Customers ON Orders.CustomerID = Customers.CustomerID
JOIN 
    Employees ON Orders.EmployeeID = Employees.EmployeeID
ORDER BY 
    Orders.OrderID;



--2) Get a list of products along with their category and supplier name.
-- (Join Products, Categories, and Suppliers)

SELECT 
    Products.ProductName,
    Categories.CategoryName,
    Suppliers.CompanyName AS SupplierName
FROM 
    Products
JOIN 
    Categories ON Products.CategoryID = Categories.CategoryID
JOIN 
    Suppliers ON Products.SupplierID = Suppliers.SupplierID
ORDER BY 
    Products.ProductName;

-- 3) Show all orders and the products included in each order with quantity and unit price.
-- (Join Orders, Order Details, Products)

---------------------------------------------------------------------------------------------------------------------
--     Just learnt that use table having space in it's name inside a square bracket
---------------------------------------------------------------------------------------------------------------------
SELECT 
    Products.ProductName,
    [Order Details].Quantity,
    [Order Details].UnitPrice
FROM 
    Orders
JOIN 
    [Order Details] ON Orders.OrderID = [Order Details].OrderID
JOIN 
    Products ON [Order Details].ProductID = Products.ProductID
ORDER BY 
    Orders.OrderID;

-- 4) List employees who report to other employees (manager-subordinate relationship).
-- (Self join on Employees)
SELECT 
    E.EmployeeID,
    E.FirstName + ' ' + E.LastName AS EmployeeName,
    M.EmployeeID AS ManagerID,
    M.FirstName + ' ' + M.LastName AS ManagerName
FROM 
    Employees E
JOIN 
    Employees M ON E.ReportsTo = M.EmployeeID
ORDER BY 
    E.EmployeeID;

-- 5) Display each customer and their total order count.
-- (Join Customers and Orders, then GROUP BY)
SELECT 
    Customers.CustomerID,
    Customers.CompanyName,
    COUNT(Orders.OrderID) AS TotalOrders
FROM 
    Customers
LEFT JOIN 
    Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY 
    Customers.CustomerID, Customers.CompanyName
ORDER BY 
    TotalOrders DESC;


-- 6) Find the average unit price of products per category.
-- Use AVG() with GROUP BY
SELECT 
    Categories.CategoryName,
    AVG(Products.UnitPrice) AS AverageUnitPrice
FROM 
    Products
JOIN 
    Categories ON Products.CategoryID = Categories.CategoryID
GROUP BY 
    Categories.CategoryName
ORDER BY 
    AverageUnitPrice DESC;


-- 7) List customers where the contact title starts with 'Owner'.
-- Use LIKE or LEFT(ContactTitle, 5)

SELECT 
    CustomerID,
    CompanyName,
    ContactName,
    ContactTitle
FROM 
    Customers
WHERE 
    ContactTitle LIKE 'Owner%';

-- 8) Show the top 5 most expensive products.
-- Use ORDER BY UnitPrice DESC and TOP 5
SELECT TOP 5 
    ProductName,
    UnitPrice
FROM 
    Products
ORDER BY 
    UnitPrice DESC;

-- 9) Return the total sales amount (quantity × unit price) per order.
-- Use SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) and GROUP BY
SELECT 
    Orders.OrderID,
    SUM([Order Details].Quantity * [Order Details].UnitPrice) AS TotalSalesAmount
FROM 
    [Order Details]
JOIN 
    Orders ON [Order Details].OrderID = Orders.OrderID
GROUP BY 
    Orders.OrderID
ORDER BY 
    TotalSalesAmount DESC;



-- 10) Create a stored procedure that returns all orders for a given customer ID.
-- Input: @CustomerID
CREATE PROCEDURE proc_GetOrdersByCustomer
    @CustomerID NVARCHAR(5)
AS
BEGIN
    SELECT 
        Orders.OrderID,
        Orders.OrderDate,
        Orders.ShippedDate,
        Customers.CompanyName AS CustomerName
    FROM 
        Orders
    JOIN 
        Customers ON Orders.CustomerID = Customers.CustomerID
    WHERE 
        Orders.CustomerID = @CustomerID
    ORDER BY 
        Orders.OrderDate DESC;
END;
GO 

EXEC proc_GetOrdersByCustomer @CustomerID = 'ALFKI';



-- 11) Write a stored procedure that inserts a new product.
-- Inputs: ProductName, SupplierID, CategoryID, UnitPrice, etc.
CREATE PROCEDURE proc_InsertNewProduct
    @ProductName NVARCHAR(40),
    @SupplierID INT,
    @CategoryID INT,
    @QuantityPerUnit NVARCHAR(20),
    @UnitPrice MONEY,
    @UnitsInStock SMALLINT,
    @UnitsOnOrder SMALLINT,
    @ReorderLevel SMALLINT,
    @Discontinued BIT
AS
BEGIN
    INSERT INTO Products (
        ProductName,
        SupplierID,
        CategoryID,
        QuantityPerUnit,
        UnitPrice,
        UnitsInStock,
        UnitsOnOrder,
        ReorderLevel,
        Discontinued
    )
    VALUES (
        @ProductName,
        @SupplierID,
        @CategoryID,
        @QuantityPerUnit,
        @UnitPrice,
        @UnitsInStock,
        @UnitsOnOrder,
        @ReorderLevel,
        @Discontinued
    );
END;
GO


EXEC proc_InsertNewProduct 
    @ProductName = 'Organic Honey',
    @SupplierID = 5,
    @CategoryID = 2,
    @QuantityPerUnit = '500 ml bottle',
    @UnitPrice = 10.50,
    @UnitsInStock = 100,
    @UnitsOnOrder = 0,
    @ReorderLevel = 10,
    @Discontinued = 0;



-- 12) Create a stored procedure that returns total sales per employee.
-- Join Orders, Order Details, and Employees

CREATE PROCEDURE proc_GetTotalSalesPerEmployee
AS
BEGIN
    SELECT 
        E.EmployeeID,
        E.FirstName + ' ' + E.LastName AS EmployeeName,
        SUM(OD.UnitPrice * OD.Quantity) AS TotalSales
    FROM 
        Employees E
    JOIN 
        Orders O ON E.EmployeeID = O.EmployeeID
    JOIN 
        [Order Details] OD ON O.OrderID = OD.OrderID
    GROUP BY 
        E.EmployeeID, E.FirstName, E.LastName
    ORDER BY 
        TotalSales DESC;
END;
GO

EXEC proc_GetTotalSalesPerEmployee;



-- 13) Use a CTE to rank products by unit price within each category.
-- Use ROW_NUMBER() or RANK() with PARTITION BY CategoryID

WITH cte_RankedProducts AS (
    SELECT 
        ProductID,
        ProductName,
        CategoryID,
        UnitPrice,
        RANK() OVER (
            PARTITION BY CategoryID 
            ORDER BY UnitPrice DESC
        ) AS PriceRank
    FROM 
        Products
)
SELECT 
    ProductID,
    ProductName,
    CategoryID,
    UnitPrice,
    PriceRank
FROM 
    cte_RankedProducts
ORDER BY 
    CategoryID, PriceRank;




-- 14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

WITH cte_ProductRevenue AS (
    SELECT 
        P.ProductName,
        SUM(OD.UnitPrice * OD.Quantity) AS TotalRevenue
    FROM 
        Products P
    JOIN 
        [Order Details] OD ON P.ProductID = OD.ProductID
    GROUP BY 
        P.ProductName
)
SELECT 
    ProductName,
    TotalRevenue
FROM 
    cte_ProductRevenue
WHERE 
    TotalRevenue > 10000
ORDER BY 
    TotalRevenue DESC;



-- 15) Use a CTE with recursion to display employee hierarchy.
-- Start from top-level employee (ReportsTo IS NULL) and drill down

WITH EmployeeHierarchy AS (
    -- top level, like ceo, cto etc......
    SELECT 
        EmployeeID,
        FirstName + ' ' + LastName AS EmployeeName,
        ReportsTo,
        0 AS Level
    FROM 
        Employees
    WHERE 
        ReportsTo IS NULL

    UNION ALL

    -- this is recursion part
    SELECT 
        E.EmployeeID,
        E.FirstName + ' ' + E.LastName AS EmployeeName,
        E.ReportsTo,
        EH.Level + 1
    FROM 
        Employees E
    INNER JOIN 
        EmployeeHierarchy EH ON E.ReportsTo = EH.EmployeeID
)
SELECT 
    EmployeeID,
    EmployeeName,
    ReportsTo,
    Level
FROM 
    EmployeeHierarchy
ORDER BY 
    Level, ReportsTo, EmployeeID;
--------- L E A R N     R E C U R S I V E    C T E    IN DEPTH!   ------------------
