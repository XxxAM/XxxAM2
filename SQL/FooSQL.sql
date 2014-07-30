-- SQL Statement for single relation.

USE Northwind
GO

--SELECT od.*
--  FROM [dbo].[Orders] o
--  JOIN [dbo].[Order Details] od
--  ON o.OrderID = od.OrderID
  --AND od.ProductID = (
  --  SELECT TOP(1) [dbo].[Order Details].ProductID
  --  FROM [dbo].[Order Details]
  --  WHERE [dbo].[Order Details].[OrderID] = o.OrderID
  --)


--SELECT x.OrderID, x.Discount 
--FROM [dbo].[Order Details] x
--JOIN (
--  SELECT od.OrderID
--  FROM [dbo].[Order Details] od
--  GROUP BY od.OrderID 
--) y ON y.OrderID = x.OrderID

--SELECT
--  od.OrderID
--FROM [dbo].[Order Details] od
--GROUP BY OrderID

SELECT o.*, x.*
FROM [dbo].[Orders] o
JOIN 
(
  SELECT   y.OrderID
  , MIN(y.Row) AS Row
  , Min(y.ProductID) AS ProductID
  , Min(y.UnitPrice) AS UnitPrice
  , Min(y.Quantity) AS Quantity
  , Min(y.Discount) AS Discount FROM
  (
    SELECT ROW_NUMBER() OVER (ORDER BY od.OrderID) AS Row, od.*
    FROM [dbo].[Order Details] od
  ) AS y
  GROUP BY y.OrderID
) AS x ON x.OrderID = o.OrderID

--SELECT od.OrderID, count(*)
--FROM [dbo].[Order Details] od
--GROUP BY od.OrderID