# Profits Report API Endpoints Documentation

This document outlines the API endpoints needed to support the ProfitsReport component. These are fake endpoints that you'll need to implement in your backend.

## Base URL
All endpoints are relative to: `{VITE_BASE_URL}/Vendor`

---

## 1. GET `/Vendor/SalesReport/{vendorId}`

**Description**: Returns comprehensive sales report data for a specific vendor.

**Parameters**:
- `vendorId` (path parameter, string): The UUID of the vendor

**Response**:
```json
{
  "totalRevenue": 45000.50,
  "totalSoldArtworks": 15,
  "averagePrice": 3000.03,
  "highestPricedArt": {
    "name": "Echoes of Spring",
    "price": 5600.00,
    "soldCount": 2
  },
  "mostPopularArt": {
    "name": "Digital Dreams",
    "soldCount": 5,
    "revenue": 20500.00
  },
  "priceDistribution": [
    {
      "range": "$0-$100",
      "count": 2,
      "revenue": 150.00
    },
    {
      "range": "$100-$500",
      "count": 3,
      "revenue": 1200.00
    },
    {
      "range": "$500-$1k",
      "count": 4,
      "revenue": 3200.00
    },
    {
      "range": "$1k-$5k",
      "count": 5,
      "revenue": 18000.00
    },
    {
      "range": "$5k+",
      "count": 1,
      "revenue": 5600.00
    }
  ],
  "monthlySalesData": [
    {
      "month": "2025-01",
      "revenue": 8000.00,
      "soldCount": 3
    },
    {
      "month": "2025-02",
      "revenue": 12500.00,
      "soldCount": 4
    },
    {
      "month": "2025-03",
      "revenue": 24500.50,
      "soldCount": 8
    }
  ],
  "categoryPerformance": [
    {
      "categoryName": "Abstract Art",
      "soldCount": 7,
      "revenue": 21000.00
    },
    {
      "categoryName": "Landscape",
      "soldCount": 5,
      "revenue": 15000.00
    },
    {
      "categoryName": "Digital Art",
      "soldCount": 3,
      "revenue": 9000.50
    }
  ],
  "recentOrders": [
    {
      "orderId": "550e8400-e29b-41d4-a716-446655440000",
      "buyerName": "John Doe",
      "artworkName": "Echoes of Spring",
      "price": 5600.00,
      "soldDate": "2025-03-10T10:30:00Z",
      "quantity": 1
    },
    {
      "orderId": "550e8400-e29b-41d4-a716-446655440001",
      "buyerName": "Jane Smith",
      "artworkName": "Digital Dreams",
      "price": 4100.00,
      "soldDate": "2025-03-08T14:15:00Z",
      "quantity": 1
    }
  ]
}
```

---

## 2. GET `/Vendor/ArtworksSalesDetails/{vendorId}`

**Description**: Returns detailed sales data for each artwork created by the vendor.

**Parameters**:
- `vendorId` (path parameter, string): The UUID of the vendor

**Response**:
```json
[
  {
    "artworkId": "9bd8b320-bf6a-408e-b190-1251b17123e9",
    "name": "Industrial Dreams",
    "price": 6200.00,
    "totalSold": 3,
    "totalRevenue": 18600.00,
    "author": "Marcus Black",
    "createdAt": "2025-12-12T21:23:02.731174Z"
  },
  {
    "artworkId": "b97433f4-48a5-4833-a2ee-2660d5917a7c",
    "name": "A Thousand Oceans Deep Painting",
    "price": 7180.00,
    "totalSold": 2,
    "totalRevenue": 14360.00,
    "author": "Nestor Toro",
    "createdAt": "2025-12-12T21:22:33.333297Z"
  },
  {
    "artworkId": "59eab01d-8823-4b03-a8e7-4964dae4e8da",
    "name": "Echoes of Spring",
    "price": 5600.00,
    "totalSold": 2,
    "totalRevenue": 11200.00,
    "author": "Sofia Chen",
    "createdAt": "2025-12-12T21:22:57.125434Z"
  }
]
```

---

## 3. GET `/Vendor/Stats/{vendorId}`

**Description**: Returns basic statistics for a vendor.

**Parameters**:
- `vendorId` (path parameter, string): The UUID of the vendor

**Response**:
```json
{
  "vendorId": "4a6e4e66-d788-11f0-8b8d-00505684821f",
  "totalArtworksCreated": 10,
  "totalArtworksSold": 15,
  "totalRevenue": 45000.50,
  "averagePrice": 3000.03
}
```

---

## Implementation Notes

### Database Relationships

Based on the database schema, these endpoints need to query:

1. **For SalesReport**:
   - Join `Vendors` with `Artworks` on `VendorId`
   - Join `Artworks` with `OrderArtworks` to count sold items
   - Join `OrderArtworks` with `Orders` to get dates and prices
   - Join `Orders` with `Clients` and `Users` to get buyer information
   - Group by month for monthly trends
   - Categorize by artwork categories

2. **For ArtworksSalesDetails**:
   - Get all artworks where `VendorId` matches
   - Count total sales per artwork from `OrderArtworks`
   - Sum revenue per artwork

3. **For Stats**:
   - Get vendor from `Vendors` table
   - Count artworks in `Artworks` where `VendorId` matches
   - Count total sold and total revenue from orders

### Query Example (Pseudo-SQL)

```sql
-- For getting sold artworks with order details
SELECT 
  o.Id as orderId,
  u.FirstName + ' ' + u.LastName as buyerName,
  a.Name as artworkName,
  a.Price as price,
  o.CreatedAt as soldDate,
  oa.ArtworkCount as quantity
FROM Orders o
JOIN OrderArtworks oa ON o.Id = oa.OrderId
JOIN Artworks a ON oa.ArtworkId = a.Id
JOIN Clients c ON o.ClientId = c.Id
JOIN Users u ON c.Id = u.Id
WHERE a.VendorId = @vendorId
ORDER BY o.CreatedAt DESC
```

### Error Handling

All endpoints should:
- Return 404 if vendor doesn't exist
- Return 200 with empty arrays if vendor has no sales data
- Handle invalid UUID format gracefully

---

## Frontend Integration

The frontend component (`ProfitsReport.tsx`) uses these custom hooks:
- `useGetVendorSalesReport` - Fetches from endpoint #1
- `useGetVendorArtworksSalesDetails` - Fetches from endpoint #2
- `useGetVendorStats` - Fetches from endpoint #3

All three hooks are enabled by the presence of a `vendorId` in sessionStorage (saved during login as `userId` for vendors).
