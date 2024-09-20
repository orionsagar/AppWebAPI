using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class PartsProduct
    {
        public int ProductID { get; set; }
        public string PartsNo { get; set; }
        public string ProductName { get; set; }
        public int CatID { get; set; }
        public string CatName { get; set; }
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string ModelID { get; set; }
        public string ModelName { get; set; }
        public string GradeID { get; set; }
        public string GradeName { get; set; }
        public string ModelYear { get; set; }
        public string ProductFor { get; set; }
        public string ProductCode { get; set; }
        public string Origin { get; set; }
        public int CurrentStock { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public double ShowPrice { get; set; }
        public int SupplierID { get; set; }
        public double Commission { get; set; }
        public string ProductDetails { get; set; }
        public string TagWords { get; set; }
        public bool Featured { get; set; }
        public bool MostPopular { get; set; }
        public int TotalSaleQty { get; set; }
        public int TotalVisitor { get; set; }
        public int TotalQuery { get; set; }
        public string ProductType { get; set; }
        public string SourceType { get; set; }
        public string ProductImage { get; set; }
        public string AdditionalInfo { get; set; }
        public string smalldescription { get; set; }
        public string Note { get; set; }
        public int MainCatID { get; set; }
        public string TagLine { get; set; }
        public string Status { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserName { get; set; }
        public DateTime Updatedate { get; set; }
        public string PriceUpdateBy { get; set; }
        public string SupplierPCode { get; set; }
        public bool freeshipping { get; set; }
        public int UserID { get; set; }
        public int Exclusive { get; set; }
        public double PrevSalePrice { get; set; }
        public double PrevShowPrice { get; set; }
        public int Currency { get; set; }
        public string Weight { get; set; }
        public int Unit { get; set; }
        public string Remark { get; set; }
        public string CategoryName { get; set; }
        public string Descriptions { get; set; }
        public List<ProductImage> ProductImage_attributes { get; set; }
    }

    public class ProductImage
    {
        public int ImgID { get; set; }
        public int ProductID { get; set; }
        public string MainImg { get; set; }
        public string LargeImg { get; set; }
        public string MidImg { get; set; }
        public string SmallImg { get; set; }
        public string XSmallImg { get; set; }
        public int SortIndex { get; set; }
        public bool PrimaryImage { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class PartList
    {
        public List<PartsProduct> items { get; set; }
        //public PagingParameterModel parameterModel { get; set; }
        public int TotalCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string previousPage { get; set; }
        public string nextPage { get; set; }
    }
}