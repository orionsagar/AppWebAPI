using Dapper;
using PVSAPI.Interface;
using PVSAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace PVSAPI.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _db;
        public OrderRepository()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionSecond"].ConnectionString);
        }

        public bool Cancel(string orderId)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string salesMaster = @"Update [SaleMaster] Set [InvStatus] = @InvStatus " +
                                    "Where [InvID] = @InvID";
            int salesMasterID = _db.Query<int>(salesMaster, new
            {
                InvStatus = "Cancel",
                InvID = orderId
            }).FirstOrDefault();


            if (salesMasterID >= 0)
            {
                string salesDetails = @"Update [SaleDetails] Set [SuppStatus] = @SuppStatus " +
                                    "Where [InvID] = @InvID";
                int salesDetailsID = _db.Query<int>(salesDetails, new
                {
                    SuppStatus = "Cancel",
                    InvID = orderId
                }).FirstOrDefault();

                return true;
            }

            return false;
        }

        public bool CheckIsExist(string orderid)
        {
            int rowsAffected = this._db.Execute(@"Select count(0) cnt from SaleMaster Where InvNo = @InvNo",
                new { InvNo = orderid });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public bool Delete(int orderId)
        {
            int rowsAffected = this._db.Execute(@"DELETE FROM [SaleMaster] WHERE InvID = @InvID",
               new { InvID = orderId });

            int rowsSalesDetails = this._db.Execute(@"DELETE FROM [SaleDetails] WHERE InvID = @InvID",
               new { InvID = orderId });

            if (rowsAffected > 0 && rowsSalesDetails > 0)
            {
                return true;
            }

            return false;
        }

        public List<Order> Get()
        {
            List<Order> saleMaster = this._db.Query<Order>("Select [InvID],[InvNo],[SubTotal],[DelvCharge],[DiscAmnt],[NetAmnt],[CourrierCharge],[InvStatus],[InvStatusforBuyer],[PaymentType],[DiscID] " +
                                    " ,[BuyerID],[BuyerFName],[BuyerLName],[Address],[BCity],[BEmail],[BPhone1],[BPhone2],[BNote],[DelvDiff],[DFname],[DLname],[DAddress] " +
                                    " ,[DCity],[DEmail],[DPhone1],[DPhone2],[DCityID],[DNote],[AffiliateID],[AffiliateComm],[CardNo],[CardOwner],[CardType],[GatewayCharge] " +
                                    " ,[BankName],[GatewayTransID],[BKashNo],[Operator],[AcceptDate],[DeliveryDate],[DeliveryBy],[Comment],[CountryID],[EntryDate],[DCountryID] " +
                                    " ,[CreateDate],[CourierID],[District],[DDistrict],[Advance] from SaleMaster ").ToList();

            var saleMasterID = saleMaster.Select(x => x.InvID);

            var salesDetails = _db.Query<SaleDetails>("select [InvDeID],[InvID],[SaleDetails].[ProductID],P.ProductName,P.PartsNo,b.BrandName,[Quantity],[Price],[SuppID],[SuppStatus],[CommAmnt],[Size],[Color] from [SaleDetails] " +
                                                        " Inner Join Products p on p.ProductID = SaleDetails.ProductID " +
                                                        " Left outer Join Brand b on b.BrandID = p.BrandID " +
                                                        " Where InvID in @InvID", new { InvID = saleMasterID });

            // Group product image by Productid
            var salesDetailsLookup = salesDetails.ToLookup(x => x.InvID);

            // Use the lookups above to populate product image
            saleMaster.ForEach(x => x.SaleDetails = salesDetailsLookup[x.InvID].ToList());
            return saleMaster;
        }

        public List<Order> GetSingle(int orderID)
        {
            List<Order> saleMaster = this._db.Query<Order>("Select [InvID],[InvNo],[SubTotal],[DelvCharge],[DiscAmnt],[NetAmnt],[CourrierCharge],[InvStatus],[InvStatusforBuyer],[PaymentType],[DiscID] " +
                                     " ,[BuyerID],[BuyerFName],[BuyerLName],[Address],[BCity],[BEmail],[BPhone1],[BPhone2],[BNote],[DelvDiff],[DFname],[DLname],[DAddress] " +
                                     " ,[DCity],[DEmail],[DPhone1],[DPhone2],[DCityID],[DNote],[AffiliateID],[AffiliateComm],[CardNo],[CardOwner],[CardType],[GatewayCharge] " +
                                     " ,[BankName],[GatewayTransID],[BKashNo],[Operator],[AcceptDate],[DeliveryDate],[DeliveryBy],[Comment],[CountryID],[EntryDate],[DCountryID] " +
                                     " ,[CreateDate],[CourierID],[District],[DDistrict],[Advance] from SaleMaster Where InvID = @InvID", new { InvID = orderID }).ToList();

            var saleMasterID = saleMaster.Select(x => x.InvID);

            var salesDetails = _db.Query<SaleDetails>("select [InvDeID],[InvID],[SaleDetails].[ProductID],P.ProductName,P.PartsNo,b.BrandName,[Quantity],[Price],[SuppID],[SuppStatus],[CommAmnt],[Size],[Color] from [SaleDetails] " +
                                                        " Inner Join Products p on p.ProductID = SaleDetails.ProductID " +
                                                        " Left outer Join Brand b on b.BrandID = p.BrandID " +
                                                        " Where InvID in @InvID", new { InvID = saleMasterID });

            // Group product image by Productid
            var salesDetailsLookup = salesDetails.ToLookup(x => x.InvID);

            // Use the lookups above to populate product image
            saleMaster.ForEach(x => x.SaleDetails = salesDetailsLookup[x.InvID].ToList());
            return saleMaster;
        }

        public int Insert(Order order)
        {
            var CrDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

            string sql = @"INSERT [SaleMaster] ([InvNo],[SubTotal],[DelvCharge],[DiscAmnt],[NetAmnt],[CourrierCharge],[InvStatus],[InvStatusforBuyer],[PaymentType],[DiscID] " +
                        " ,[BuyerID],[BuyerFName],[BuyerLName],[Address],[BCity],[BEmail],[BPhone1],[BPhone2],[BNote],[DelvDiff],[DFname],[DLname],[DAddress] " +
                        " ,[DCity],[DEmail],[DPhone1],[DPhone2],[DCityID],[DNote],[AffiliateID],[AffiliateComm],[CardNo],[CardOwner],[CardType],[GatewayCharge] " +
                        " ,[BankName],[GatewayTransID],[BKashNo],[Operator],[AcceptDate],[DeliveryDate],[DeliveryBy],[Comment],[CountryID],[EntryDate],[DCountryID]" +
                        " ,[CreateDate],[CourierID],[District],[DDistrict],[Advance]) " +
                        "values (@InvNo,@SubTotal,@DelvCharge,@DiscAmnt,@NetAmnt,@CourrierCharge,@InvStatus,@InvStatusforBuyer,@PaymentType,@DiscID " +
                        " ,@BuyerID, @BuyerFName, @BuyerLName, @Address, @BCity, @BEmail, @BPhone1, @BPhone2, @BNote, @DelvDiff, @DFname, @DLname, @DAddress " +
                        " ,@DCity, @DEmail, @DPhone1, @DPhone2, @DCityID, @DNote, @AffiliateID, @AffiliateComm, @CardNo, @CardOwner, @CardType, @GatewayCharge " +
                        " ,@BankName, @GatewayTransID, @BKashNo, @Operator, @AcceptDate, @DeliveryDate, @DeliveryBy, @Comment, @CountryID, @EntryDate, @DCountryID " +
                        " ,@CreateDate, @CourierID, @District, @DDistrict, @Advance); SELECT CAST(SCOPE_IDENTITY() as int)";


            int returnID = _db.Query<int>(sql, new
            {
                InvNo = order.InvNo,
                SubTotal = order.SubTotal,
                DelvCharge = order.DelvCharge,
                DiscAmnt = order.DiscAmnt,
                NetAmnt = order.NetAmnt,
                CourrierCharge = order.CourrierCharge,
                InvStatus = order.InvStatus,
                InvStatusforBuyer = order.InvStatusforBuyer,
                PaymentType = order.PaymentType,
                DiscID = order.DiscID,
                BuyerID = order.BuyerID,
                BuyerFName = order.BuyerFName,
                BuyerLName = order.BuyerLName,
                Address = order.Address,
                BCity = order.BCity,
                BEmail = order.BEmail,
                BPhone1 = order.BPhone1,
                BPhone2 = order.BPhone2,
                BNote = order.BNote,
                DelvDiff = order.DelvDiff,
                DFname = order.DFname,
                DLname = order.DLname,
                DAddress = order.DAddress,
                DCity = order.DCity,
                DEmail = order.DEmail,
                DPhone1 = order.DPhone1,
                DPhone2 = order.DPhone2,
                DCityID = order.DCityID,
                DNote = order.DNote,
                AffiliateID = order.AffiliateID,
                AffiliateComm = order.AffiliateComm,
                CardNo = order.CardNo,
                CardOwner = order.CardOwner,
                CardType = order.CardType,
                GatewayCharge = order.GatewayCharge,
                BankName = order.BankName,
                GatewayTransID = order.GatewayTransID,
                BKashNo = order.BKashNo,
                Operator = order.Operator,
                AcceptDate = order.AcceptDate,
                DeliveryDate = CrDate,
                DeliveryBy = order.DeliveryBy,
                Comment = order.Comment,
                CountryID = order.CountryID,
                EntryDate = CrDate,
                DCountryID = order.DCountryID,
                CreateDate = order.CreateDate,
                CourierID = order.CourierID,
                District = order.District,
                DDistrict = order.DDistrict,
                Advance = order.Advance
            }).Single();






            foreach (SaleDetails sd in order.SaleDetails)
            {
                string salesDetails = @"INSERT [SaleDetails] ([InvID],[ProductID],[Quantity],[Price],[SuppID],[SuppStatus],[CommAmnt],[Size],[Color])" +
                                    "values (@InvID,@ProductID,@Quantity,@Price,@SuppID,@SuppStatus,@CommAmnt,@Size,@Color)";
                _db.Query<int>(salesDetails, new
                {
                    InvID = returnID,
                    ProductID = sd.ProductID,
                    Quantity = sd.Quantity,
                    Price = sd.Price,
                    SuppID = sd.SuppID,
                    SuppStatus = sd.SuppStatus,
                    CommAmnt = sd.CommAmnt,
                    Size = sd.Size,
                    Color = sd.Color
                });
            }

            return returnID;
        }

        public bool Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}