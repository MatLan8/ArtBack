# SQL Query Examples for Profits Report Endpoints

These are example SQL queries (for SQL Server/MySQL) that show how to implement the three backend endpoints.
Adjust syntax for your specific database system.

---

## 1. Sales Report Query

**Endpoint**: `GET /Vendor/SalesReport/{vendorId}`

This query returns all the data needed for the main report.

```sql
-- Get vendor's total revenue and sold count
DECLARE @VendorId NVARCHAR(36) = ?; -- Parameter: vendorId

-- Calculate basic metrics
SELECT 
    SUM(CAST(oa.TotalSum AS DECIMAL(18,2))) AS totalRevenue,
    COUNT(DISTINCT oa.Id) AS totalSoldArtworks,
    AVG(CAST(a.Price AS DECIMAL(18,2))) AS averagePrice
FROM OrderArtworks oa
JOIN Artworks a ON oa.ArtworkId = a.Id
WHERE a.VendorId = @VendorId
    AND oa.TotalSum > 0;

-- Get highest priced artwork with sales
SELECT TOP 1
    a.Name AS name,
    CAST(a.Price AS DECIMAL(18,2)) AS price,
    COUNT(oa.Id) AS soldCount
FROM Artworks a
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
WHERE a.VendorId = @VendorId
ORDER BY a.Price DESC;

-- Get most popular artwork
SELECT TOP 1
    a.Name AS name,
    COUNT(oa.Id) AS soldCount,
    SUM(CAST(oa.TotalSum AS DECIMAL(18,2))) AS revenue
FROM Artworks a
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
WHERE a.VendorId = @VendorId
GROUP BY a.Id, a.Name
ORDER BY COUNT(oa.Id) DESC;

-- Price distribution
SELECT 
    CASE 
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 100 THEN '$0-$100'
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 500 THEN '$100-$500'
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 1000 THEN '$500-$1k'
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 5000 THEN '$1k-$5k'
        ELSE '$5k+'
    END AS range,
    COUNT(DISTINCT oa.Id) AS count,
    ISNULL(SUM(CAST(oa.TotalSum AS DECIMAL(18,2))), 0) AS revenue
FROM Artworks a
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
WHERE a.VendorId = @VendorId
GROUP BY CASE 
    WHEN CAST(a.Price AS DECIMAL(18,2)) < 100 THEN '$0-$100'
    WHEN CAST(a.Price AS DECIMAL(18,2)) < 500 THEN '$100-$500'
    WHEN CAST(a.Price AS DECIMAL(18,2)) < 1000 THEN '$500-$1k'
    WHEN CAST(a.Price AS DECIMAL(18,2)) < 5000 THEN '$1k-$5k'
    ELSE '$5k+'
END
ORDER BY 
    CASE 
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 100 THEN 1
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 500 THEN 2
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 1000 THEN 3
        WHEN CAST(a.Price AS DECIMAL(18,2)) < 5000 THEN 4
        ELSE 5
    END;

-- Monthly sales data
SELECT 
    FORMAT(o.CreatedAt, 'yyyy-MM') AS month,
    SUM(CAST(o.TotalSum AS DECIMAL(18,2))) AS revenue,
    SUM(oa.ArtworkCount) AS soldCount
FROM Orders o
JOIN OrderArtworks oa ON o.Id = oa.OrderId
JOIN Artworks a ON oa.ArtworkId = a.Id
WHERE a.VendorId = @VendorId
    AND o.TotalSum > 0
GROUP BY FORMAT(o.CreatedAt, 'yyyy-MM')
ORDER BY month ASC;

-- Category performance
SELECT 
    CASE 
        WHEN c.Style = 0 THEN 'Modern'
        WHEN c.Style = 1 THEN 'Classical'
        WHEN c.Style = 2 THEN 'Contemporary'
        WHEN c.Style = 3 THEN 'Abstract'
        WHEN c.Style = 4 THEN 'Digital'
        ELSE 'Unknown'
    END AS categoryName,
    COUNT(DISTINCT oa.Id) AS soldCount,
    SUM(CAST(oa.TotalSum AS DECIMAL(18,2))) AS revenue
FROM Artworks a
JOIN Categories c ON a.CategoryId = c.Id
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
WHERE a.VendorId = @VendorId
GROUP BY c.Style
ORDER BY revenue DESC;

-- Recent orders (TOP 20)
SELECT TOP 20
    o.Id AS orderId,
    u.FirstName + ' ' + u.LastName AS buyerName,
    a.Name AS artworkName,
    CAST(a.Price AS DECIMAL(18,2)) AS price,
    o.CreatedAt AS soldDate,
    oa.ArtworkCount AS quantity
FROM Orders o
JOIN OrderArtworks oa ON o.Id = oa.OrderId
JOIN Artworks a ON oa.ArtworkId = a.Id
JOIN Clients cl ON o.ClientId = cl.Id
JOIN Users u ON cl.Id = u.Id
WHERE a.VendorId = @VendorId
ORDER BY o.CreatedAt DESC;
```

---

## 2. Artworks Sales Details Query

**Endpoint**: `GET /Vendor/ArtworksSalesDetails/{vendorId}`

Returns sales data for each individual artwork.

```sql
DECLARE @VendorId NVARCHAR(36) = ?; -- Parameter: vendorId

SELECT 
    a.Id AS artworkId,
    a.Name AS name,
    CAST(a.Price AS DECIMAL(18,2)) AS price,
    ISNULL(COUNT(DISTINCT oa.Id), 0) AS totalSold,
    ISNULL(SUM(CAST(oa.TotalSum AS DECIMAL(18,2))), 0) AS totalRevenue,
    a.Author AS author,
    a.CreatedAt AS createdAt
FROM Artworks a
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
WHERE a.VendorId = @VendorId
    AND a.isDeleted = 0
GROUP BY a.Id, a.Name, a.Price, a.Author, a.CreatedAt
ORDER BY totalRevenue DESC;
```

---

## 3. Vendor Stats Query

**Endpoint**: `GET /Vendor/Stats/{vendorId}`

Returns quick statistics about the vendor.

```sql
DECLARE @VendorId NVARCHAR(36) = ?; -- Parameter: vendorId

SELECT 
    v.Id AS vendorId,
    COUNT(DISTINCT a.Id) AS totalArtworksCreated,
    ISNULL(COUNT(DISTINCT oa.Id), 0) AS totalArtworksSold,
    ISNULL(SUM(CAST(o.TotalSum AS DECIMAL(18,2))), 0) AS totalRevenue,
    ISNULL(AVG(CAST(a.Price AS DECIMAL(18,2))), 0) AS averagePrice
FROM Vendors v
LEFT JOIN Artworks a ON v.Id = a.VendorId AND a.isDeleted = 0
LEFT JOIN OrderArtworks oa ON a.Id = oa.ArtworkId
LEFT JOIN Orders o ON oa.OrderId = o.Id
WHERE v.Id = @VendorId
GROUP BY v.Id;
```

---

## Alternative: Using Stored Procedures

If you prefer stored procedures, here's an example:

```sql
CREATE PROCEDURE sp_GetVendorSalesReport
    @VendorId NVARCHAR(36)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Create temporary table for results
    CREATE TABLE #SalesData (
        DataType NVARCHAR(50),
        JsonData NVARCHAR(MAX)
    );
    
    -- Basic metrics
    INSERT INTO #SalesData
    SELECT 'metrics' AS DataType,
        (
            SELECT 
                SUM(CAST(oa.TotalSum AS DECIMAL(18,2))) AS totalRevenue,
                COUNT(DISTINCT oa.Id) AS totalSoldArtworks,
                AVG(CAST(a.Price AS DECIMAL(18,2))) AS averagePrice
            FROM OrderArtworks oa
            JOIN Artworks a ON oa.ArtworkId = a.Id
            WHERE a.VendorId = @VendorId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
    
    -- Monthly trends
    INSERT INTO #SalesData
    SELECT 'monthlySalesData' AS DataType,
        (
            SELECT 
                FORMAT(o.CreatedAt, 'yyyy-MM') AS month,
                SUM(CAST(o.TotalSum AS DECIMAL(18,2))) AS revenue,
                SUM(oa.ArtworkCount) AS soldCount
            FROM Orders o
            JOIN OrderArtworks oa ON o.Id = oa.OrderId
            JOIN Artworks a ON oa.ArtworkId = a.Id
            WHERE a.VendorId = @VendorId
                AND o.TotalSum > 0
            GROUP BY FORMAT(o.CreatedAt, 'yyyy-MM')
            ORDER BY month ASC
            FOR JSON PATH
        );
    
    SELECT * FROM #SalesData;
    DROP TABLE #SalesData;
END;
```

---

## Notes for Implementation

### Performance Considerations

1. **Index these columns for better performance:**
   ```sql
   CREATE INDEX IX_Artworks_VendorId ON Artworks(VendorId);
   CREATE INDEX IX_OrderArtworks_ArtworkId ON OrderArtworks(ArtworkId);
   CREATE INDEX IX_Orders_ClientId ON Orders(ClientId);
   CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt);
   ```

2. **Consider caching:**
   - Cache vendor stats for 1 hour
   - Cache sales report for 15-30 minutes
   - Invalidate cache on new order

3. **Pagination:**
   - For `recentOrders`, implement pagination (OFFSET/FETCH)
   - Default to TOP 20-50, allow configurable limit

### Error Handling

- Return 404 if vendor doesn't exist
- Return empty arrays if no sales data
- Handle NULL values appropriately
- Ensure decimal precision (18,2) for currency

### Security

- Always use parameterized queries (above uses `?` placeholders)
- Validate `vendorId` format (should be valid GUID)
- Ensure user can only view their own vendor data
- Add authentication check before serving data

---

## Testing Data

You can use these INSERT statements to add test data:

```sql
-- Add a test order for vendor 4a6e4e66-d788-11f0-8b8d-00505684821f
INSERT INTO Orders (Id, CreatedAt, TotalSum, PaymentMethod, DeliveryAddress, 
    DeliveryStatus, DeliveryDate, TrackingNumber, Comment, DeliveryMethod, ClientId)
VALUES (
    NEWID(),
    GETDATE(),
    5600.00,
    'Credit Card',
    '123 Main St',
    'Delivered',
    GETDATE(),
    'TRACK123',
    'Great item',
    0,
    'ec1c93b7-d82a-11f0-8b8d-00505684821f'
);

-- Link the order to an artwork
INSERT INTO OrderArtworks (Id, TotalSum, ArtworkCount, OrderId, ArtworkId)
SELECT 
    NEWID(),
    5600.00,
    1,
    (SELECT TOP 1 Id FROM Orders ORDER BY CreatedAt DESC),
    '59eab01d-8823-4b03-a8e7-4964dae4e8da'
WHERE NOT EXISTS (
    SELECT 1 FROM OrderArtworks 
    WHERE OrderId = (SELECT TOP 1 Id FROM Orders ORDER BY CreatedAt DESC)
);
```

---

**Last Updated**: December 13, 2025
