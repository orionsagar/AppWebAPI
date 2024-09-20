using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVSAPI.Models
{
    public class Order
    {
		public int InvID { get; set; }
		public string InvNo { get; set; }
		public double SubTotal { get; set; }
		public double DelvCharge { get; set; }
		public double DiscAmnt { get; set; }
		public double NetAmnt { get; set; }
		public double CourrierCharge { get; set; }
		public string InvStatus { get; set; }
		public string InvStatusforBuyer { get; set; }
		public string PaymentType { get; set; }
		public int DiscID { get; set; }
		public int BuyerID { get; set; }
		public string BuyerFName { get; set; }
		public string BuyerLName { get; set; }
		public string Address { get; set; }
		public string BCity { get; set; }
		public string BEmail { get; set; }
		public string BPhone1 { get; set; }
		public string BPhone2 { get; set; }
		public string BNote { get; set; }
		public bool DelvDiff { get; set; }
		public string DFname { get; set; }
		public string DLname { get; set; }
		public string DAddress { get; set; }
		public string DCity { get; set; }
		public string DEmail { get; set; }
		public string DPhone1 { get; set; }
		public string DPhone2 { get; set; }
		public int DCityID { get; set; }
		public string DNote { get; set; }
		public int AffiliateID { get; set; }
		public double AffiliateComm { get; set; }
		public string CardNo { get; set; }
		public string CardOwner { get; set; }
		public string CardType { get; set; }
		public double GatewayCharge { get; set; }
		public string BankName { get; set; }
		public string GatewayTransID { get; set; }
		public string BKashNo { get; set; }
		public int Operator { get; set; }
		public DateTime AcceptDate { get; set; }
		public DateTime DeliveryDate { get; set; }
		public string DeliveryBy { get; set; }
		public string Comment { get; set; }
		public int CountryID { get; set; }
		public DateTime EntryDate { get; set; }
		public int DCountryID { get; set; }
		public DateTime CreateDate { get; set; }
		public int CourierID { get; set; }
		public string District { get; set; }
		public string DDistrict { get; set; }
		public double Advance { get; set; }

		public List<SaleDetails> SaleDetails { get; set; }

	}


	public partial class SaleDetails
	{
		public int InvDeID { get; set; }
		public int InvID { get; set; }
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public string PartsNo { get; set; }
		public string BrandName { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
		public int SuppID { get; set; }
		public string SuppStatus { get; set; }
		public double CommAmnt { get; set; }
		public string Size { get; set; }
		public string Color { get; set; }
	}

	public class OrderList
	{
		public List<Order> items { get; set; }
		//public PagingParameterModel parameterModel { get; set; }
		public int TotalCount { get; set; }
		public int pageSize { get; set; }
		public int currentPage { get; set; }
		public string previousPage { get; set; }
		public string nextPage { get; set; }
	}
}