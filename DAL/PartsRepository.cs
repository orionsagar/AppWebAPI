using Dapper;
using PVSAPI.Interface;
using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PVSAPI.DAL
{
    public class PartsRepository : IPartsRepository
    {
        private readonly IDbConnection _db;
        public PartsRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }
        public List<PartsProduct> Get()
        {

            //" left join Brand m on m.BrandID = p.BrandID " +
            //                                            " left join Model md on md.ModelID = p.ModelID " +
            //                                            " left join Category c on c.CatID = p.CatID
            List<PartsProduct> parts = this._db.Query<PartsProduct>("Select p.[ProductID],p.[PartsNo],p.[ProductName],p.[CatID],p.[CatName],p.[BrandID],p.[BrandName],p.[ModelID],p.[ModelName],p.[GradeID],p.[GradeName],p.[ModelYear] " +
                                                        " ,p.[ProductFor],p.[ProductCode],p.[Origin],p.[CurrentStock],p.[PurchasePrice],p.[SalePrice],p.[ShowPrice],p.[SupplierID],p.[Commission],p.[ProductDetails] " +
                                                        " ,p.[TagWords],p.[Featured],p.[MostPopular],p.[TotalSaleQty],p.[TotalVisitor],p.[TotalQuery],p.[ProductType],p.[SourceType],p.[ProductImage],p.[AdditionalInfo],p.[smalldescription] " +
                                                        " ,p.[Note],p.[MainCatID],p.[TagLine],p.[Status],p.[EntryDate],p.[UserName],p.[Updatedate],p.[PriceUpdateBy],p.[SupplierPCode],p.[freeshipping],p.[UserID] " +
                                                        " ,p.[Exclusive],p.[PrevSalePrice],p.[PrevShowPrice],p.[Currency],p.[Weight],p.[Unit],p.[Remark], p.CatName,(p.BrandName +', '+ p.ModelName)Descriptions from Products p ").ToList();
            var partID = parts.Select(x => x.ProductID);

            var producImg = _db.Query<ProductImage>("select [ImgID],[ProductID],[MainImg],[LargeImg],[MidImg],[SmallImg],[XSmallImg],[SortIndex],[PrimaryImage],[IsPrimary] from [ProductImage] Where ProductID in @ProductID", new { ProductID = partID });

            // Group product image by Productid
            var productimgLookup = producImg.ToLookup(x => x.ProductID);

            // Use the lookups above to populate product image
            parts.ForEach(x => x.ProductImage_attributes = productimgLookup[x.ProductID].ToList());
            return parts;
        }

        public List<PartsProduct> GetSingle(int ProductID)
        {
            var parts = this._db.Query<PartsProduct>("Select top 1 p.[ProductID],p.[PartsNo],p.[ProductName],p.[CatID],p.[CatName],p.[BrandID],p.[BrandName],p.[ModelID],p.[ModelName],p.[GradeID],p.[GradeName],p.[ModelYear] " +
                                                        " ,p.[ProductFor],p.[ProductCode],p.[Origin],p.[CurrentStock],p.[PurchasePrice],p.[SalePrice],p.[ShowPrice],p.[SupplierID],p.[Commission],p.[ProductDetails] " +
                                                        " ,p.[TagWords],p.[Featured],p.[MostPopular],p.[TotalSaleQty],p.[TotalVisitor],p.[TotalQuery],p.[ProductType],p.[SourceType],p.[ProductImage],p.[AdditionalInfo],p.[smalldescription] " +
                                                        " ,p.[Note],p.[MainCatID],p.[TagLine],p.[Status],p.[EntryDate],p.[UserName],p.[Updatedate],p.[PriceUpdateBy],p.[SupplierPCode],p.[freeshipping],p.[UserID] " +
                                                        " ,p.[Exclusive],p.[PrevSalePrice],p.[PrevShowPrice],p.[Currency],p.[Weight],p.[Unit],p.[Remark], p.CatName,(p.BrandName +', '+ p.ModelName)Descriptions from Products p " +
                                                        " Where P.[ProductID] = @ProductID", new { ProductID = ProductID }).ToList();
            var partID = parts.Select(x => x.ProductID);

            var producImg = _db.Query<ProductImage>("select [ImgID],[ProductID],[MainImg],[LargeImg],[MidImg],[SmallImg],[XSmallImg],[SortIndex],[PrimaryImage],[IsPrimary] from [ProductImage] where ProductID = @ProductID", new { ProductID = partID });

            // Group product image by Productid
            var productimgLookup = producImg.ToLookup(x => x.ProductID);

            // Use the lookups above to populate product image
            parts.ForEach(x => x.ProductImage_attributes = productimgLookup[x.ProductID].ToList());
            return parts;
        }
    }
}