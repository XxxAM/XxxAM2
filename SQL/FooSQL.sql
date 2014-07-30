-- SQL Statement for single relation.

USE Northwind
GO

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
