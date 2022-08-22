////--Swati joshi 23092019, 4.40 ////
///
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Http;
using System.Web.Http.Results;
using DevExtreme.AspNet.Data;
using System.Web.Http.Cors;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using DealerPortal.Common;
using DealerPortal.Models;
using System.Reflection;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Data.SqlClient;
using System.Web;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace DealerPortal.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class OrdersController : ApiController
    {
        GeneralFunction co = new GeneralFunction();
        string strEncryptionDecryption = System.Configuration.ConfigurationManager.AppSettings["EncryptionDecryption"];

        #region Login

        [HttpPost]
        public returndbmlTempLoginMaster UserLoginPwdVerification(dbmlCommon objinput)
        {
            ObservableCollection<dbmlTempLoginMaster> ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>();
            returndbmlTempLoginMaster objreturn = new returndbmlTempLoginMaster();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[UserLoginPwdVerification]", objinput.StringOne, objinput.StringTwo);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "LoginMaster", "Statustb" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["LoginMaster"].Rows.Count > 0)
                {
                    if (!Cryptographer.CompareHash("DealerPortal", objinput.StringTwo, dataSet.Tables["LoginMaster"].Rows[0]["Password"].ToString()))
                    {
                        objreturn.objdbmlTempLoginMaster = null;
                        objreturn.objdbmlStatus.StatusId = -3;
                        objreturn.objdbmlStatus.Status = "Invalid User Pwd";
                    }
                    else
                    {

                        ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>(from dRows in dataSet.Tables["LoginMaster"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlTempLoginMaster>(dRows)));

                        objreturn.objdbmlTempLoginMaster = ObjdbmlTempLoginMaster;
                        objreturn.objdbmlStatus.StatusId = Convert.ToInt32(dataSet.Tables["Statustb"].Rows[0]["StatusId"]);
                        objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["Statustb"].Rows[0]["Status"]);
                    }
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = Convert.ToInt32(dataSet.Tables["Statustb"].Rows[0]["StatusId"]);
                    objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["Statustb"].Rows[0]["Status"]);
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpPost]
        public returndbmlTempLoginMaster UserGetByEmailId(dbmlCommon objinput)
        {
            ObservableCollection<dbmlTempLoginMaster> ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>();
            returndbmlTempLoginMaster objreturn = new returndbmlTempLoginMaster();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[UserGetByEmailId]", objinput.StringOne);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "LoginMaster", "Statustb" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["LoginMaster"].Rows.Count > 0)
                {
                    ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>(from dRows in dataSet.Tables["LoginMaster"].AsEnumerable()
                                                                                           select (co.ConvertTableToListNew<dbmlTempLoginMaster>(dRows)));

                    objreturn.objdbmlTempLoginMaster = ObjdbmlTempLoginMaster;
                    objreturn.objdbmlStatus.StatusId = Convert.ToInt32(dataSet.Tables["Statustb"].Rows[0]["StatusId"]);
                    objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["Statustb"].Rows[0]["Status"]);

                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = Convert.ToInt32(dataSet.Tables["Statustb"].Rows[0]["StatusId"]);
                    objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["Statustb"].Rows[0]["Status"]);
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpPost]
        public returndbmlStatus UserGetByUserId(dbmlCommon objinput)
        {
            returndbmlStatus objreturn = new returndbmlStatus();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[UserGetByUserId]", objinput.StringOne);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "LoginMaster" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["LoginMaster"].Rows.Count > 0)
                {
                    objreturn.objdbmlStatus.StatusId = Convert.ToInt32(dataSet.Tables["LoginMaster"].Rows[0]["UserId"]);
                    objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["LoginMaster"].Rows[0]["EmailID"]);
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "User Not Found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region Place Order

        [HttpGet]
        public returndbmlRetailCategoryMaster RetailCategoryMasterGetByParentId(string ParentId)
        {
            ObservableCollection<dbmlRetailCategoryMaster> objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[RetailCategoryMasterGetByParentId]", ParentId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "RetailCategory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["RetailCategory"].Rows.Count > 0)
                {
                    objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>(from dRows in dataSet.Tables["RetailCategory"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlRetailCategoryMaster>(dRows)));

                    objreturn.objdbmlRetailCategoryMaster = objdbmlRetailCategoryMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlMRPCategoryMaster MRPCategoryMasterGetAll()
        {
            ObservableCollection<dbmlMRPCategoryMaster> objdbmlMRPCategoryMaster = new ObservableCollection<dbmlMRPCategoryMaster>();
            returndbmlMRPCategoryMaster objreturn = new returndbmlMRPCategoryMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[MRPCategoryMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "MRPCategory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["MRPCategory"].Rows.Count > 0)
                {
                    objdbmlMRPCategoryMaster = new ObservableCollection<dbmlMRPCategoryMaster>(from dRows in dataSet.Tables["MRPCategory"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlMRPCategoryMaster>(dRows)));

                    objreturn.objdbmlMRPCategoryMaster = objdbmlMRPCategoryMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlItemDetailView ItemMasterGetByCategoryGroupIdMRPCategoryIdPartyIdUserId(string CategoryId, string CategoryGroupId, string MRPCategoryId, string PartyId, string UserId)
        {
            ObservableCollection<dbmlTempItemMaster> objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>();
            ObservableCollection<dbmlTempCartView> objdbmlTempCartView = new ObservableCollection<dbmlTempCartView>();
            returndbmlItemDetailView objreturn = new returndbmlItemDetailView();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ItemMasterGetByCategoryGroupIdMRPCategoryIdPartyIdUserId]", CategoryId, CategoryGroupId, MRPCategoryId, PartyId, UserId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "ItemMaster", "CartView" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["ItemMaster"].Rows.Count > 0)
                {
                    objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>(from dRows in dataSet.Tables["ItemMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlTempItemMaster>(dRows)));

                    objreturn.objdbmlTempItemMaster = objdbmlTempItemMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }

                if (dataSet.Tables.Count > 0 && dataSet.Tables["CartView"].Rows.Count > 0)
                {
                    objdbmlTempCartView = new ObservableCollection<dbmlTempCartView>(from dRows in dataSet.Tables["CartView"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlTempCartView>(dRows)));

                    objreturn.objdbmlTempCartView = objdbmlTempCartView;
                }

                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlItemDetailView CartInsert(RequestCartInsert objinput)
        {
            returndbmlItemDetailView objReturn = new returndbmlItemDetailView();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();


                foreach (dbmlCart item in objinput.objdbmlCart)
                {
                    dbCmd = db.GetStoredProcCommand("[Trans].[CartInsert]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = ItemMasterGetByCategoryGroupIdMRPCategoryIdPartyIdUserId(objinput.objdbmlCommon.StringFive, objinput.objdbmlCommon.StringOne, objinput.objdbmlCommon.StringTwo, objinput.objdbmlCommon.StringThree, objinput.objdbmlCommon.StringFour);


            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlTempCartMaster CartGetByUserId(string UserId, string PartyId)
        {
            ObservableCollection<dbmlTempCartMaster> objdbmlTempCartMaster = new ObservableCollection<dbmlTempCartMaster>();
            returndbmlTempCartMaster objreturn = new returndbmlTempCartMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[CartGetByUserId]", Convert.ToInt32(UserId), Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Cart");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Cart"].Rows.Count > 0)
                {
                    objdbmlTempCartMaster = new ObservableCollection<dbmlTempCartMaster>(from dRows in dataSet.Tables["Cart"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlTempCartMaster>(dRows)));

                    objreturn.objdbmlTempCartMaster = objdbmlTempCartMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlTempCartMaster CartDelete(dbmlCommon objinput)
        {
            returndbmlTempCartMaster objReturn = new returndbmlTempCartMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intCartId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Trans].[CartDelete]", intCartId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = CartGetByUserId(objinput.StringTwo, objinput.StringThree);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public dbmlStatus OrderInsert(RequestOrderInsert objinput)
        {
            dbmlStatus objReturn = new dbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                //dbmlCart dr = objinput.objdbmlCart.FirstOrDefault();
                decimal? OrderTotalAmount = 0;
                int intOut = 0;

                foreach (dbmlTempCartMaster item in objinput.objdbmlTempCartMaster)
                {
                    OrderTotalAmount = OrderTotalAmount + item.TotalAmount;
                }
                foreach (dbmlTempCartMaster item in objinput.objdbmlTempCartMaster)
                {
                    //OrderTotalAmount = OrderTotalAmount  + item.TotalAmount;

                    if (intOut == 0)
                    {
                        dbCmd = db.GetStoredProcCommand("[Trans].[OrderHeaderInsert]");
                        db.AddInParameter(dbCmd, "OrderHeaderId", DbType.Int32, 0);
                        db.AddInParameter(dbCmd, "RetailCategoryId", DbType.Int32, Convert.ToInt32(objinput.objdbmlCommon.StringTwo));
                        db.AddInParameter(dbCmd, "MRPCategoryId", DbType.Int32, Convert.ToInt32(objinput.objdbmlCommon.StringThree));
                        db.AddInParameter(dbCmd, "OrderNo", DbType.String, 0);
                        db.AddInParameter(dbCmd, "OrderDate", DbType.DateTime, System.DateTime.Now);
                        db.AddInParameter(dbCmd, "PartyId", DbType.Int32, Convert.ToInt32(item.PartyId));
                        db.AddInParameter(dbCmd, "OrderItemCount", DbType.Int32, objinput.objdbmlTempCartMaster.Count);
                        db.AddInParameter(dbCmd, "OrderAmount", DbType.Decimal, OrderTotalAmount);
                        db.AddInParameter(dbCmd, "CreateId", DbType.Int32, objinput.objdbmlCommon.StringOne);
                        db.AddInParameter(dbCmd, "CreateDate", DbType.DateTime, System.DateTime.Now);
                        db.AddInParameter(dbCmd, "UpdatedId", DbType.Int32, objinput.objdbmlCommon.StringOne);
                        db.AddInParameter(dbCmd, "UpdateDate", DbType.DateTime, System.DateTime.Now);
                        db.AddInParameter(dbCmd, "remark", DbType.String, objinput.objdbmlCommon.StringFour); // Ruchi 25/10/2019
                        db.AddOutParameter(dbCmd, "DocIdOut", DbType.Int32, 0);
                        db.ExecuteNonQuery(dbCmd, dbTrans);
                        intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@DocIdOut"));
                    }
                    //foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    //{
                    //    DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                    //    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    //}

                    dbCmd = db.GetStoredProcCommand("[Trans].[OrderDetailInsert]");
                    db.AddInParameter(dbCmd, "OrderDetailId", DbType.Int32, 0);
                    db.AddInParameter(dbCmd, "OrderHeaderId", DbType.Int32, intOut);
                    db.AddInParameter(dbCmd, "ItemId", DbType.Int32, item.ItemId);
                    db.AddInParameter(dbCmd, "PackSize", DbType.Decimal, Convert.ToInt32(item.ZZITemPackSize));
                    db.AddInParameter(dbCmd, "OrderQty", DbType.Decimal, item.QuantityAdded);
                    db.AddInParameter(dbCmd, "OrderUnits", DbType.Decimal, item.OrderUnits);
                    db.AddInParameter(dbCmd, "MRP", DbType.Decimal, item.MRP);
                    db.AddInParameter(dbCmd, "DealerPrice", DbType.Decimal, item.DP);
                    //db.AddInParameter(dbCmd, "DiscountPercentage1", DbType.Decimal, item.SchemeDiscountPercentage);
                    //db.AddInParameter(dbCmd, "DiscountPercentage2", DbType.Decimal, item.TradeDiscountPercentage);
                    //db.AddInParameter(dbCmd, "DiscountPercentage3", DbType.Decimal, item.AddiDiscountPercentage);
                    db.AddInParameter(dbCmd, "DiscountPercentage1", DbType.Decimal, item.TradeDiscountPercentage);
                    db.AddInParameter(dbCmd, "DiscountPercentage2", DbType.Decimal, item.SchemeDiscountPercentage);
                    db.AddInParameter(dbCmd, "DiscountPercentage3", DbType.Decimal, item.AddiDiscountPercentage);

                    db.AddInParameter(dbCmd, "DiscountPercentage4", DbType.Decimal, 0.00);
                    db.AddInParameter(dbCmd, "DiscountPercentage5", DbType.Decimal, 0.00);
                    db.AddInParameter(dbCmd, "GSTRate", DbType.Decimal, item.GSTPercentage);
                    db.AddInParameter(dbCmd, "Tax", DbType.Decimal, item.TaxAmount);
                    db.AddInParameter(dbCmd, "TotalAmount", DbType.Decimal, item.TotalAmount);
                    db.AddInParameter(dbCmd, "CreateId", DbType.Int32, objinput.objdbmlCommon.StringOne);
                    db.AddInParameter(dbCmd, "CreateDate", DbType.DateTime, System.DateTime.Now);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }

                dbCmd = db.GetStoredProcCommand("[Trans].[CartDeleteByUserIdPartyId]", objinput.objdbmlCommon.StringOne, objinput.objdbmlTempCartMaster.FirstOrDefault().PartyId);
                db.ExecuteNonQuery(dbCmd, dbTrans);

                dbTrans.Commit();
                objReturn.StatusId = 1;
                objReturn.Status = Convert.ToString(intOut);

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.StatusId = 99;
                objReturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlItemDetailView CartViewGetByUserId(string UserId, string ParentId)
        {
            ObservableCollection<dbmlTempCartView> objdbmlTempCartView = new ObservableCollection<dbmlTempCartView>();
            returndbmlItemDetailView objreturn = new returndbmlItemDetailView();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[CartViewGetByUserId]", UserId, ParentId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CartView");

                if (dataSet.Tables.Count > 0 && dataSet.Tables["CartView"].Rows.Count > 0)
                {
                    objdbmlTempCartView = new ObservableCollection<dbmlTempCartView>(from dRows in dataSet.Tables["CartView"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlTempCartView>(dRows)));

                    objreturn.objdbmlTempCartView = objdbmlTempCartView;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Sucess";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlTempOrderHistory OrderHeaderGetByOrderId(string OrderHeaderId)
        {
            ObservableCollection<dbmlTempOrderHistory> objdbmlTempOrderHistory = new ObservableCollection<dbmlTempOrderHistory>();
            returndbmlTempOrderHistory objreturn = new returndbmlTempOrderHistory();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrderHeaderGetByOrderId]", Convert.ToInt32(OrderHeaderId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "OrderHistory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderHistory"].Rows.Count > 0)
                {
                    objdbmlTempOrderHistory = new ObservableCollection<dbmlTempOrderHistory>(from dRows in dataSet.Tables["OrderHistory"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlTempOrderHistory>(dRows)));

                    objreturn.objdbmlTempOrderHistory = objdbmlTempOrderHistory;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlTempOrderHeaderId OrderGetByOrderId(string OrderHeaderId)
        {
            ObservableCollection<dbmlTempOrderHeaderId> objdbmlTempOrderHeaderId = new ObservableCollection<dbmlTempOrderHeaderId>();
            ObservableCollection<dbmlTempOrderHeaderDetailId> objdbmlTempOrderHeaderDetailId = new ObservableCollection<dbmlTempOrderHeaderDetailId>();
            returndbmlTempOrderHeaderId objreturn = new returndbmlTempOrderHeaderId();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrderGetByOrderId]", Convert.ToInt32(OrderHeaderId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "OrderHeader", "OrderDetail" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderHeader"].Rows.Count > 0)
                {
                    objdbmlTempOrderHeaderId = new ObservableCollection<dbmlTempOrderHeaderId>(from dRows in dataSet.Tables["OrderHeader"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlTempOrderHeaderId>(dRows)));

                    objreturn.objdbmlTempOrderHeaderId = objdbmlTempOrderHeaderId;

                    if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderDetail"].Rows.Count > 0)
                    {
                        objdbmlTempOrderHeaderDetailId = new ObservableCollection<dbmlTempOrderHeaderDetailId>(from dRows in dataSet.Tables["OrderDetail"].AsEnumerable()
                                                                                                               select (co.ConvertTableToListNew<dbmlTempOrderHeaderDetailId>(dRows)));

                        objreturn.objdbmlTempOrderHeaderDetailId = objdbmlTempOrderHeaderDetailId;
                    }
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpPost]
        public returndbmlStatus OrderHeaderUpdate(dbmlCommon objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intOrderHeaderId = Convert.ToInt32(objinput.StringOne);
                int intSalesOrderID = Convert.ToInt32(objinput.StringTwo);

                dbCmd = db.GetStoredProcCommand("[Trans].[OrderHeaderUpdate]", intOrderHeaderId, intSalesOrderID);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn.objdbmlStatus.Status = "Successful";
                objReturn.objdbmlStatus.StatusId = 1;
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public ReturndbmlTempOrderNotInserted OrdersNotInserted()
        {
            ObservableCollection<dbmlTempOrderNotInserted> objdbmlTempOrderNotInserted = new ObservableCollection<dbmlTempOrderNotInserted>();
            ReturndbmlTempOrderNotInserted objreturn = new ReturndbmlTempOrderNotInserted();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrdersNotInserted]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "OrdersNotInserted" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OrdersNotInserted"].Rows.Count > 0)
                {
                    objdbmlTempOrderNotInserted = new ObservableCollection<dbmlTempOrderNotInserted>(from dRows in dataSet.Tables["OrdersNotInserted"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlTempOrderNotInserted>(dRows)));

                    objreturn.objdbmlTempOrderNotInserted = objdbmlTempOrderNotInserted;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlTempOrderNotInserted = null;
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "No Record Pending";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlTempOrderNotInserted = null;
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region SalesChallan

        [HttpPost]
        public dbmlStatus SalesChallanInsert(requestdbmlsalesChallan objinput)
        {
            dbmlStatus objReturn = new dbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlSalesChallan item in objinput.objdbmlSalesChallan)
                {
                    dbCmd = db.GetStoredProcCommand("[Trans].[SalesChallanInsert]");
                    db.AddInParameter(dbCmd, "SalesChallanID", DbType.Int32, item.SalesChallanID);
                    db.AddInParameter(dbCmd, "SalesChallanDate", DbType.DateTime, item.SalesChallanDate);
                    db.AddInParameter(dbCmd, "ClientID", DbType.Int32, item.ClientID);
                    db.AddInParameter(dbCmd, "Client", DbType.String, item.Client);
                    db.AddInParameter(dbCmd, "CreatedBy", DbType.Int32, item.CreatedBy);
                    db.AddInParameter(dbCmd, "CreatedDate", DbType.DateTime, item.CreatedDate);
                    db.AddInParameter(dbCmd, "EditedBy", DbType.Int32, item.EditedBy);
                    db.AddInParameter(dbCmd, "EditedDate", DbType.DateTime, item.EditedDate);
                    db.AddInParameter(dbCmd, "Status", DbType.Decimal, item.Status);
                    db.AddInParameter(dbCmd, "YearId", DbType.Int32, item.YearId);
                    db.AddInParameter(dbCmd, "SrNo", DbType.Int32, item.SrNo);
                    db.AddInParameter(dbCmd, "LRNo", DbType.String, item.LRNo);
                    db.AddInParameter(dbCmd, "TransporterName", DbType.String, item.TransporterName);
                    db.AddInParameter(dbCmd, "VehicleNo", DbType.String, item.VehicleNo);
                    db.AddInParameter(dbCmd, "LRDate", DbType.DateTime, item.LRDate);
                    db.AddInParameter(dbCmd, "SalesInvoiceNo", DbType.String, item.SalesInvoiceNo);
                    db.AddInParameter(dbCmd, "CurrencyID", DbType.Int32, item.CurrencyID);
                    db.AddInParameter(dbCmd, "GrossWeight", DbType.Decimal, item.GrossWeight);
                    db.AddInParameter(dbCmd, "NetWeight", DbType.Decimal, item.NetWeight);
                    db.AddInParameter(dbCmd, "ItemWeightUOMID", DbType.Int32, item.ItemWeightUOMID);
                    db.AddInParameter(dbCmd, "ConsigneeID", DbType.Int32, item.ConsigneeID);
                    db.AddInParameter(dbCmd, "DriverName", DbType.String, item.DriverName);
                    db.AddInParameter(dbCmd, "DriverContactNo", DbType.String, item.DriverContactNo);
                    db.AddInParameter(dbCmd, "InvoiceRemarks", DbType.String, item.InvoiceRemarks);
                    db.AddInParameter(dbCmd, "ClientAddress", DbType.String, item.ClientAddress);
                    db.AddInParameter(dbCmd, "ConsigneeAddress", DbType.String, item.ConsigneeAddress);
                    db.AddInParameter(dbCmd, "ClientGST", DbType.String, item.ClientGST);
                    db.AddInParameter(dbCmd, "ConsigneeGST", DbType.String, item.ConsigneeGST);
                    db.AddInParameter(dbCmd, "NetAmount", DbType.Decimal, item.NetAmount);
                    db.AddInParameter(dbCmd, "OtherCharges", DbType.Decimal, item.OtherCharges);
                    db.AddInParameter(dbCmd, "TaxAmount", DbType.Decimal, item.TaxAmount);
                    db.AddInParameter(dbCmd, "TotalAmount", DbType.Decimal, item.TotalAmount);
                    db.AddInParameter(dbCmd, "RoundOff", DbType.Decimal, item.RoundOff);
                    db.AddInParameter(dbCmd, "RoundedTotalAmount", DbType.Decimal, item.RoundedTotalAmount);
                    db.AddInParameter(dbCmd, "EWayBillNo", DbType.String, item.EWayBillNo);
                    db.AddInParameter(dbCmd, "EWayBillDate", DbType.DateTime, item.EWayBillDate);
                    db.AddInParameter(dbCmd, "ClientCity", DbType.String, item.ClientCity);
                    db.AddInParameter(dbCmd, "ConsigneeCity", DbType.String, item.ConsigneeCity);
                    db.AddInParameter(dbCmd, "ConsigneePin", DbType.String, item.ConsigneePin);
                    db.AddInParameter(dbCmd, "ClientPin", DbType.String, item.ClientPin);
                    db.ExecuteNonQuery(dbCmd, dbTrans);

                    ObservableCollection<dbmlSalesChallanDetail> objDetail = new ObservableCollection<dbmlSalesChallanDetail>(objinput.objdbmlSalesChallanDetail.Where(t => t.SalesChallanID == item.SalesChallanID));

                    foreach (dbmlSalesChallanDetail itemd in objDetail)
                    {
                        dbCmd = db.GetStoredProcCommand("[Trans].[SalesChallanDetailInsert]");
                        db.AddInParameter(dbCmd, "SalesChallanDetailId", DbType.Int32, itemd.SalesChallanDetailID);
                        db.AddInParameter(dbCmd, "SalesChallanID", DbType.Int32, itemd.SalesChallanID);
                        db.AddInParameter(dbCmd, "SalesOrderDeliveryID", DbType.Int32, itemd.SalesOrderDeliveryID);
                        db.AddInParameter(dbCmd, "ItemID", DbType.Int32, itemd.ItemID);
                        db.AddInParameter(dbCmd, "Rate", DbType.Decimal, itemd.Rate);
                        db.AddInParameter(dbCmd, "UOMId", DbType.Int32, itemd.UOMId);
                        db.AddInParameter(dbCmd, "DeliverDate", DbType.DateTime, itemd.DeliverDate);
                        db.AddInParameter(dbCmd, "DispatchQuantity", DbType.Decimal, itemd.DispatchQuantity);
                        db.AddInParameter(dbCmd, "SalesOrderID", DbType.Int32, itemd.SalesOrderID);
                        db.AddInParameter(dbCmd, "Remarks", DbType.String, itemd.Remarks);
                        db.AddInParameter(dbCmd, "SOITEMID", DbType.Int32, itemd.SOITEMID);
                        db.AddInParameter(dbCmd, "BasicRate", DbType.Decimal, itemd.BasicRate);
                        db.AddInParameter(dbCmd, "Discount1", DbType.Decimal, itemd.Discount1);
                        db.AddInParameter(dbCmd, "Discount2", DbType.Decimal, itemd.Discount2);
                        db.AddInParameter(dbCmd, "Discount3", DbType.Decimal, itemd.Discount3);
                        db.AddInParameter(dbCmd, "Discount4", DbType.Decimal, itemd.Discount4);
                        db.AddInParameter(dbCmd, "SchemeDiscount", DbType.Decimal, itemd.SchemeDiscount);
                        db.AddInParameter(dbCmd, "Dis1Rate", DbType.Decimal, itemd.Dis1Rate);
                        db.AddInParameter(dbCmd, "Dis2Rate", DbType.Decimal, itemd.Dis2Rate);
                        db.AddInParameter(dbCmd, "SchemePercentage", DbType.Decimal, itemd.SchemePercentage);
                        db.AddInParameter(dbCmd, "Dis3rate", DbType.Decimal, itemd.Dis3rate);
                        db.AddInParameter(dbCmd, "Dis4rate", DbType.Decimal, itemd.Dis4rate);
                        db.AddInParameter(dbCmd, "GrossValue", DbType.Decimal, itemd.GrossValue);
                        db.ExecuteNonQuery(dbCmd, dbTrans);
                    }
                }

                dbTrans.Commit();
                objReturn.StatusId = 1;
                objReturn.Status = "Successful";
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.StatusId = 99;
                objReturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public ReturndbmlTempSalesChallan SalesChallanGetByFromDateToDateDPStatus(string FromDate, string ToDate, string DPStatusId, int UserId)
        {
            ObservableCollection<dbmlTempSalesChallan> objdbmlTempSalesChallan = new ObservableCollection<dbmlTempSalesChallan>();
            ReturndbmlTempSalesChallan objreturn = new ReturndbmlTempSalesChallan();
            try
            {
                DateTime FDate = DateTime.Now;
                DateTime TDate = DateTime.Now;

                if (FromDate != null && FromDate != String.Empty && FromDate != "")
                {
                    FDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (ToDate != null && ToDate != String.Empty && ToDate != "")
                {
                    TDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                //int UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[SalesChallanGetByFromDateToDateDPStatus]",
                                                                        Convert.ToDateTime(FDate),
                                                                        Convert.ToDateTime(TDate),
                                                                        Convert.ToInt32(DPStatusId),
                                                                        UserId
                                                                      );
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "SalesChallan");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["SalesChallan"].Rows.Count > 0)
                {
                    objdbmlTempSalesChallan = new ObservableCollection<dbmlTempSalesChallan>(from dRows in dataSet.Tables["SalesChallan"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlTempSalesChallan>(dRows)));

                    objreturn.objdbmlTempSalesChallan = objdbmlTempSalesChallan;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpPost]
        public returndbmlStatus SalesChallanUpdateDPStatus(ReqSalesChallanUpdt objReqSalesChallanUpdt)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                foreach (dbmlSalesChallanUpdtReq obj in objReqSalesChallanUpdt.objdbmlSalesChallanUpdtReq)
                {
                    int intSalesChallanId = Convert.ToInt32(obj.SalesChallanID);
                    int DPStatusParaId = Convert.ToInt32(obj.DPStatusParaId);

                    dbCmd = db.GetStoredProcCommand("[Trans].[SalesChallanUpdateDPStatus]", intSalesChallanId, DPStatusParaId);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Update Successfuly";
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public ReturnSalesChallanDetail SalesChallanGetBySalesChallanId(int SalesChallanId)
        {
            ObservableCollection<dbmlSalesChallanDetailView> objdbmlTempSalesChallan = new ObservableCollection<dbmlSalesChallanDetailView>();
            ReturnSalesChallanDetail objreturn = new ReturnSalesChallanDetail();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[SalesChallanGetBySalesChallanId]",
                                                                        SalesChallanId
                                                                      );
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "SalesChallanDetail");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["SalesChallanDetail"].Rows.Count > 0)
                {
                    objdbmlTempSalesChallan = new ObservableCollection<dbmlSalesChallanDetailView>(from dRows in dataSet.Tables["SalesChallanDetail"].AsEnumerable()
                                                                                                   select (co.ConvertTableToListNew<dbmlSalesChallanDetailView>(dRows)));

                    objreturn.objdbmlSalesChallanDetailView = objdbmlTempSalesChallan;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        //
        //@SalesChallanId int
        #endregion

        #region Parameter

        [HttpGet]
        public returndbmlParameter ParameterGetbyParameterTypeId(string ParameterTypeId)
        {
            ObservableCollection<dbmlParameter> objdbmlParameter = new ObservableCollection<dbmlParameter>();
            returndbmlParameter objreturn = new returndbmlParameter();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ParameterGetbyParameterTypeId]", Convert.ToInt32(ParameterTypeId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Parameterview");

                if (dataSet.Tables.Count > 0 && dataSet.Tables["Parameterview"].Rows.Count > 0)
                {
                    objdbmlParameter = new ObservableCollection<dbmlParameter>(from dRows in dataSet.Tables["Parameterview"].AsEnumerable()
                                                                               select (co.ConvertTableToListNew<dbmlParameter>(dRows)));

                    objreturn.objdbmlParameter = objdbmlParameter;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Sucess";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlParameter ParameterGetAllForOptionList()
        {
            ObservableCollection<dbmlParameter> objdbmlParameterMaster = new ObservableCollection<dbmlParameter>();
            returndbmlParameter objreturn = new returndbmlParameter();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ParameterGetAllForOptionList]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Parameter");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Parameter"].Rows.Count > 0)
                {
                    objdbmlParameterMaster = new ObservableCollection<dbmlParameter>(from dRows in dataSet.Tables["Parameter"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlParameter>(dRows)));

                    objreturn.objdbmlParameter = objdbmlParameterMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlParameter ParameterGetAll()
        {
            ObservableCollection<dbmlParameter> objdbmlParameterMaster = new ObservableCollection<dbmlParameter>();
            returndbmlParameter objreturn = new returndbmlParameter();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ParameterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Parameter");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Parameter"].Rows.Count > 0)
                {
                    objdbmlParameterMaster = new ObservableCollection<dbmlParameter>(from dRows in dataSet.Tables["Parameter"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlParameter>(dRows)));

                    objreturn.objdbmlParameter = objdbmlParameterMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

        #region Order History

        [HttpGet]
        public returndbmlTempOrderHistory OrderHistoryGetByFromDateToDatePartyId(string FromDate, string ToDate, string PartyId)
        {
            ObservableCollection<dbmlTempOrderHistory> objdbmlTempOrderHistory = new ObservableCollection<dbmlTempOrderHistory>();
            returndbmlTempOrderHistory objreturn = new returndbmlTempOrderHistory();
            try
            {
                string FFDate = "";
                string TTDate = "";
                var FDate = FromDate.Split('/');
                var TDate = ToDate.Split('/');
                FFDate = FDate[1] + "/" + FDate[0] + "/" + FDate[2];
                TTDate = TDate[1] + "/" + TDate[0] + "/" + TDate[2];

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrderHistoryGetByFromDateToDatePartyId]", FFDate, TTDate, Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "OrderHistory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderHistory"].Rows.Count > 0)
                {
                    objdbmlTempOrderHistory = new ObservableCollection<dbmlTempOrderHistory>(from dRows in dataSet.Tables["OrderHistory"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlTempOrderHistory>(dRows)));

                    objreturn.objdbmlTempOrderHistory = objdbmlTempOrderHistory;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlTempOrderDetail OrderDetailGetByOrderId(string OrderHeaderId)
        {
            ObservableCollection<dbmlTempOrderDetail> objdbmlTempOrderDetail = new ObservableCollection<dbmlTempOrderDetail>();
            returndbmlTempOrderDetail objreturn = new returndbmlTempOrderDetail();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrderDetailGetByOrderId]", OrderHeaderId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "OrderDetail");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderDetail"].Rows.Count > 0)
                {
                    objdbmlTempOrderDetail = new ObservableCollection<dbmlTempOrderDetail>(from dRows in dataSet.Tables["OrderDetail"].AsEnumerable()
                                                                                           select (co.ConvertTableToListNew<dbmlTempOrderDetail>(dRows)));

                    objreturn.objdbmlTempOrderDetail = objdbmlTempOrderDetail;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region User Master

        [HttpPost]
        public returndbmlUserMaster UserMasterInsert(RequestUserInsert objinput)
        {
            returndbmlUserMaster objReturn = new returndbmlUserMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            int intOut = 0;
            string strMailId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlUserMaster item in objinput.objdbmlUser)
                {
                    dbCmd = db.GetStoredProcCommand("[Security].[UserMasterInsert]");
                    string strUserPassword = Cryptographer.CreateHash("DealerPortal", item.Password.ToString());
                    strMailId = Convert.ToString(item.EmailID);

                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        if (PropInfoCol.Name == "Password")
                        {
                            db.AddInParameter(dbCmd, PropInfoCol.Name, DbType.String, strUserPassword);
                        }
                        else
                        {
                            DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                        }
                    }
                    db.AddOutParameter(dbCmd, "DocIdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@DocIdOut"));
                }
                if (intOut >= 0)
                {
                    dbTrans.Commit();
                    objReturn = UserMasterGetAll();
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = intOut;
                    objReturn.objdbmlStatus.Status = "User with MailId already exist - " + strMailId;
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlUserMaster UserMasterUpdate(RequestUserUpdate objinput)
        {
            returndbmlUserMaster objReturn = new returndbmlUserMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                dbCmd = db.GetStoredProcCommand("[Security].[UserMasterUpdate]");
                foreach (PropertyInfo PropInfoCol in objinput.objdbmlUserMasterUpdateReq.GetType().GetProperties())
                {
                    DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlUserMasterUpdateReq, null));
                }
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = UserMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlUserMaster UserMasterStatusUpdate(RequestUserInsert objinput)
        {
            returndbmlUserMaster objReturn = new returndbmlUserMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;

                //  SqlCommand dbsqlCmd = new SqlCommand();
                // SqlConnection dbsqlCon = new SqlConnection();
                // dbsqlCon.ConnectionString = co.StrSetConnection();

                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlUserMaster item in objinput.objdbmlUser)
                {
                    dbCmd = db.GetStoredProcCommand("[Security].[UserMasterUpdateStatus]", item.UserId, item.IsActive);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = UserMasterGetAll();

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlUserMaster UserMasterGetAll()
        {
            ObservableCollection<dbmlUserMasterView> objdbmlUserMaster = new ObservableCollection<dbmlUserMasterView>();
            returndbmlUserMaster objreturn = new returndbmlUserMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[UserMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "UserMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["UserMaster"].Rows.Count > 0)
                {
                    objdbmlUserMaster = new ObservableCollection<dbmlUserMasterView>(from dRows in dataSet.Tables["UserMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlUserMasterView>(dRows)));

                    objreturn.objdbmlUserMasterView = objdbmlUserMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlUserMaster UserMasterDelete(dbmlCommon objinput)
        {
            returndbmlUserMaster objReturn = new returndbmlUserMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intUserId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Security].[UserMasterDelete]", intUserId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = UserMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlStatus UserMasterChangePassword(dbmlCommon objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intUserId = Convert.ToInt32(objinput.StringOne);
                string strPwd = Cryptographer.CreateHash("DealerPortal", objinput.StringTwo);
                dbCmd = db.GetStoredProcCommand("[Security].[UserMasterChangePassword]", intUserId, strPwd);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Password Change Successfuly";
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlStatus UserMasterGetByEmailId(dbmlCommon objinput)
        {
            ObservableCollection<dbmlTempLoginMaster> ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>();
            returndbmlStatus objreturn = new returndbmlStatus();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[UserMasterGetByEmailId]", objinput.StringTwo);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "LoginMaster" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["LoginMaster"].Rows.Count > 0)
                {
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = Convert.ToString(dataSet.Tables["LoginMaster"].Rows[0]["EmailID"]);
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "User's MailId not Found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region Item Master

        [HttpGet]
        public returndbmlItemMaster ItemMasterGetAll()
        {
            ObservableCollection<dbmlItemMasterView> objdbmlitemMaster = new ObservableCollection<dbmlItemMasterView>();
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[itemMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "itemMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["itemMaster"].Rows.Count > 0)
                {
                    objdbmlitemMaster = new ObservableCollection<dbmlItemMasterView>(from dRows in dataSet.Tables["itemMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlItemMasterView>(dRows)));

                    objreturn.objdbmlItemMasterView = objdbmlitemMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }


        [HttpGet]
        public returndbmlItemMaster ItemMasterGetAllBySuperStockist(string SuperStockistID)
        {
            ObservableCollection<dbmlItemMasterView> objdbmlItemMasterView = new ObservableCollection<dbmlItemMasterView>();
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ItemMasterGetAllBySuperStockist]", SuperStockistID);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlItemMasterView = new ObservableCollection<dbmlItemMasterView>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlItemMasterView>(dRows)));

                    objreturn.objdbmlItemMasterView = objdbmlItemMasterView;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlStatus ItemMasterUpsert(requestdbmlTempItemInsertUpdate objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempItemInsertUpdate item in objinput.objdbmlTempItemInsertUpdate)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[ItemMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Party Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlItemMaster ItemMasterGetAllForStockAdjustment(string PartyID)
        {
            ObservableCollection<dbmlItemMasterView> objdbmlitemMaster = new ObservableCollection<dbmlItemMasterView>();
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ItemMasterGetAllForStockAdjustment]", Convert.ToInt32(PartyID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "itemMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["itemMaster"].Rows.Count > 0)
                {
                    objdbmlitemMaster = new ObservableCollection<dbmlItemMasterView>(from dRows in dataSet.Tables["itemMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlItemMasterView>(dRows)));

                    objreturn.objdbmlItemMasterView = objdbmlitemMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

        #region Party Master

        [HttpGet]
        public returndbmlTempPartyMasterIdentity PartyMasterGetForIdentity()
        {
            ObservableCollection<dbmlTempPartyMasterIdentity> objdbmlTempPartyMasterIdentity = new ObservableCollection<dbmlTempPartyMasterIdentity>();
            returndbmlTempPartyMasterIdentity objreturn = new returndbmlTempPartyMasterIdentity();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[PartyMasterGetForIdentity]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "PartyMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["PartyMaster"].Rows.Count > 0)
                {
                    objdbmlTempPartyMasterIdentity = new ObservableCollection<dbmlTempPartyMasterIdentity>(from dRows in dataSet.Tables["PartyMaster"].AsEnumerable()
                                                                                                           select (co.ConvertTableToListNew<dbmlTempPartyMasterIdentity>(dRows)));

                    objreturn.objdbmlTempPartyMasterIdentity = objdbmlTempPartyMasterIdentity;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlStatus PartyMasterUpsert(requestdbmlTempPartyMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempPartyMaster item in objinput.objdbmlTempPartyMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[PartyMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Party Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlPartyMaster PartyMasterGetAll()
        {
            ObservableCollection<dbmlTempPartyMaster> objdbmlPartyMaster = new ObservableCollection<dbmlTempPartyMaster>();
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[PartyMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "PartyMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["PartyMaster"].Rows.Count > 0)
                {
                    objdbmlPartyMaster = new ObservableCollection<dbmlTempPartyMaster>(from dRows in dataSet.Tables["PartyMaster"].AsEnumerable()
                                                                                       select (co.ConvertTableToListNew<dbmlTempPartyMaster>(dRows)));

                    objreturn.objdbmlPartyMaster = objdbmlPartyMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlPartyMaster PartyMasterGetByPartyId(string PartyId)
        {
            ObservableCollection<dbmlTempPartyMaster> objdbmlTempPartyMaster = new ObservableCollection<dbmlTempPartyMaster>();
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[PartyMasterGetByPartyId]", Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "PartyMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["PartyMaster"].Rows.Count > 0)
                {
                    objdbmlTempPartyMaster = new ObservableCollection<dbmlTempPartyMaster>(from dRows in dataSet.Tables["PartyMaster"].AsEnumerable()
                                                                                           select (co.ConvertTableToListNew<dbmlTempPartyMaster>(dRows)));

                    objreturn.objdbmlPartyMaster = objdbmlTempPartyMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region Country Master

        [HttpPost]
        public returndbmlStatus CountryMasterUpsert(requestdbmlTempCountryMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempCountryMaster item in objinput.objdbmlTempCountryMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[CountryMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Country Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region State Master

        [HttpPost]
        public returndbmlStatus StateMasterUpsert(requestdbmlTempStateMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempStateMaster item in objinput.objdbmlTempStateMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[StateMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "State Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region City Master

        [HttpPost]
        public returndbmlStatus CityMasterUpsert(requestdbmlTempCityMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempCityMaster item in objinput.objdbmlTempCityMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[CityMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "City Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region Master Update

        [HttpGet]
        public returndbmlMasterUpdate MasterUpdateGetAll()
        {
            ObservableCollection<dbmlMasterUpdate> objdbmlMasterUpdate = new ObservableCollection<dbmlMasterUpdate>();
            returndbmlMasterUpdate objreturn = new returndbmlMasterUpdate();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Settings].[MasterUpdateGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "MasterUpdate");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["MasterUpdate"].Rows.Count > 0)
                {
                    objdbmlMasterUpdate = new ObservableCollection<dbmlMasterUpdate>(from dRows in dataSet.Tables["MasterUpdate"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlMasterUpdate>(dRows)));

                    objreturn.objdbmlMasterUpdate = objdbmlMasterUpdate;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public dbmlStatus GetCurrentDate12()
        {
            dbmlStatus objreturn = new dbmlStatus();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[GetCurrentDate]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CurrentDate");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CurrentDate"].Rows.Count > 0)
                {

                    objreturn.StatusId = 1;
                    objreturn.Status = Convert.ToString(dataSet.Tables["CurrentDate"].Rows[0]["CurrentDate"]);
                }
                else
                {
                    objreturn.StatusId = 2;
                    objreturn.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.StatusId = 99;
                objreturn.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlStatus MasterUpdateTableUpdate(dbmlCommon objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                int intMasterUpdateId = Convert.ToInt32(objinput.StringOne);
                DateTime dtUpdateDate = Convert.ToDateTime(objinput.StringTwo);

                dbCmd = db.GetStoredProcCommand("[Settings].[MasterUpdateTableUpdate]", intMasterUpdateId, dtUpdateDate);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Date Update Successfuly";
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region Item Party Link

        [HttpGet]
        public ReturndbmlTempIPL ItemPartyLinkGetByPartyId(string strPartyId)
        {
            ObservableCollection<dbmlTempIPL> objdbmlTempIPLL = new ObservableCollection<dbmlTempIPL>();
            ObservableCollection<dbmlTempIPL> objdbmlTempIPLNL = new ObservableCollection<dbmlTempIPL>();
            ReturndbmlTempIPL objreturn = new ReturndbmlTempIPL();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ItemPartyLinkGetByPartyId]", Convert.ToInt32(strPartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "IPLLinked", "IPLNotLinked" });
                if (dataSet.Tables.Count > 0 &&
                        ((dataSet.Tables["IPLLinked"].Rows.Count > 0) || (dataSet.Tables["IPLNotLinked"].Rows.Count > 0)))
                {
                    objdbmlTempIPLL = new ObservableCollection<dbmlTempIPL>(from dRows in dataSet.Tables["IPLLinked"].AsEnumerable()
                                                                            select (co.ConvertTableToListNew<dbmlTempIPL>(dRows)));

                    objreturn.objdbmlTempIPLLinked = objdbmlTempIPLL;

                    objdbmlTempIPLNL = new ObservableCollection<dbmlTempIPL>(from dRows in dataSet.Tables["IPLNotLinked"].AsEnumerable()
                                                                             select (co.ConvertTableToListNew<dbmlTempIPL>(dRows)));

                    objreturn.objdbmlTempIPLNotLinked = objdbmlTempIPLNL;

                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public ReturndbmlTempIPL ItemPartyLinkInsert(RequestdbmlItemPartyLinkReq objinput)
        {
            ReturndbmlTempIPL objReturn = new ReturndbmlTempIPL();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            string strPartyId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlItemPartyLinkReq item in objinput.objdbmlItemPartyLinkReq)
                {
                    strPartyId = Convert.ToString(item.PartyId);

                    dbCmd = db.GetStoredProcCommand("[Master].[ItemPartyLinkInsert]");

                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = ItemPartyLinkGetByPartyId(strPartyId);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public ReturndbmlTempIPL ItemPartyLinkDelete(RequestdbmlItemPartyLinkReq objinput)
        {
            ReturndbmlTempIPL objReturn = new ReturndbmlTempIPL();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            string strPartyId = "";
            string strItemId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlItemPartyLinkReq item in objinput.objdbmlItemPartyLinkReq)
                {
                    strPartyId = Convert.ToString(item.PartyId);
                    strItemId = Convert.ToString(item.ItemId);

                    dbCmd = db.GetStoredProcCommand("[Master].[ItemPartyLinkDelete]", Convert.ToInt32(strItemId), Convert.ToInt32(strPartyId));
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = ItemPartyLinkGetByPartyId(strPartyId);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public ReturndbmlTempIPL ItemPartyLinkCopyPaste(dbmlCommon objinput)
        {
            ReturndbmlTempIPL objReturn = new ReturndbmlTempIPL();

            ReturndbmlTempIPL objPartyFrom = new ReturndbmlTempIPL();
            ReturndbmlTempIPL objPartyTo = new ReturndbmlTempIPL();

            DbConnection dbCon = null;
            DbTransaction dbTrans = null;

            string strPartyIdFrom = objinput.StringOne;
            string strPartyIdto = objinput.StringTwo;
            string strUserId = objinput.StringThree;

            string strItemId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                objPartyTo = ItemPartyLinkGetByPartyId(strPartyIdto);

                foreach (dbmlTempIPL item in objPartyTo.objdbmlTempIPLLinked)
                {
                    strItemId = Convert.ToString(item.ItemId);

                    dbCmd = db.GetStoredProcCommand("[Master].[ItemPartyLinkDelete]", Convert.ToInt32(strItemId), Convert.ToInt32(strPartyIdto));
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }

                objPartyFrom = ItemPartyLinkGetByPartyId(strPartyIdFrom);

                foreach (dbmlTempIPL item in objPartyFrom.objdbmlTempIPLLinked)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[ItemPartyLinkInsert]");
                    db.AddInParameter(dbCmd, "ItemId", DbType.Int32, item.ItemId);
                    db.AddInParameter(dbCmd, "PartyId", DbType.Int32, strPartyIdto);
                    db.AddInParameter(dbCmd, "UserId", DbType.Int32, Convert.ToInt32(strUserId));
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }

                dbTrans.Commit();
                objReturn = ItemPartyLinkGetByPartyId(strPartyIdto);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region ItemLedger
        [HttpPost]
        public ReturndbmlItemLedger ItemLedgerInsert(RequestItemLedgerInsert objinput)
        {
            ReturndbmlItemLedger objReturn = new ReturndbmlItemLedger();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlItemLedger item in objinput.objdbmlItemLedgerreq)
                {
                    dbCmd = db.GetStoredProcCommand("[Stock].[ItemLedgerInsert]");

                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {

                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));

                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }

                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Successfully Inserted";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }
        #endregion

        #region Multiple Masters Get Data only

        [HttpGet]
        public returndbmlYearMaster YearMasterGetByDate(string OrderDate)
        {
            ObservableCollection<dbmlTempYearMaster> objdbmlTempYearMaster = new ObservableCollection<dbmlTempYearMaster>();
            returndbmlYearMaster objreturn = new returndbmlYearMaster();
            try
            {
                //string FFDate = "";
                //var FDate = OrderDate.Split('/');
                //FFDate = FDate[1] + "/" + FDate[0] + "/" + FDate[2];

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[YearMasterGetByDate]", OrderDate);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "YearMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["YearMaster"].Rows.Count > 0)
                {
                    objdbmlTempYearMaster = new ObservableCollection<dbmlTempYearMaster>(from dRows in dataSet.Tables["YearMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlTempYearMaster>(dRows)));

                    objreturn.objdbmlYearMaster = objdbmlTempYearMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlCurrencyMaster CurrencyMasterGetByCurrencyName(string strCurrencyName)
        {
            ObservableCollection<dbmlTempCurrencyMaster> objdbmlTempCurrencyMaster = new ObservableCollection<dbmlTempCurrencyMaster>();
            returndbmlCurrencyMaster objreturn = new returndbmlCurrencyMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[CurrencyMasterGetByCurrencyName]", strCurrencyName);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CurrencyMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CurrencyMaster"].Rows.Count > 0)
                {
                    objdbmlTempCurrencyMaster = new ObservableCollection<dbmlTempCurrencyMaster>(from dRows in dataSet.Tables["CurrencyMaster"].AsEnumerable()
                                                                                                 select (co.ConvertTableToListNew<dbmlTempCurrencyMaster>(dRows)));

                    objreturn.objdbmlCurrencyMaster = objdbmlTempCurrencyMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion

        #region Option List

        [HttpPost]
        public returndbmlOptionList OptionListUpsert(returndbmlOptionList objinput)
        {
            returndbmlOptionList objReturn = new returndbmlOptionList();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            string strParameterId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlOptionList item in objinput.objdbmlOptionList)
                {
                    strParameterId = Convert.ToString(item.ParameterId);

                    if (item.OptionListId == 0)
                    {
                        dbCmd = db.GetStoredProcCommand("[Settings].[OptionListInsert]");
                    }
                    else
                    {
                        dbCmd = db.GetStoredProcCommand("[Settings].[OptionListUpdate]");
                    }

                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = OptionListGetByParameterId(strParameterId);
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlOptionList OptionListDelete(dbmlCommon objinput)
        {
            returndbmlOptionList objReturn = new returndbmlOptionList();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            string strParameterId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                strParameterId = Convert.ToString(objinput.StringTwo);
                int intId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Settings].[OptionListDelete]", intId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = OptionListGetByParameterId(strParameterId);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlOptionList OptionListGetByParameterId(string ParameterId)
        {
            ObservableCollection<dbmlOptionList> objdbmlOptionListTemp = new ObservableCollection<dbmlOptionList>();
            returndbmlOptionList objreturn = new returndbmlOptionList();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Settings].[OptionListGetByParameterId]", Convert.ToInt32(ParameterId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "OptionList");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OptionList"].Rows.Count > 0)
                {
                    objdbmlOptionListTemp = new ObservableCollection<dbmlOptionList>(from dRows in dataSet.Tables["OptionList"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlOptionList>(dRows)));

                    objreturn.objdbmlOptionList = objdbmlOptionListTemp;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlOptionList OptionListByStockAdjustmentType(string ParameterId, string AdType)
        {
            ObservableCollection<dbmlOptionList> objdbmlOptionListTemp = new ObservableCollection<dbmlOptionList>();
            returndbmlOptionList objreturn = new returndbmlOptionList();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Settings].[OptionListByStockAdjustmentType]", Convert.ToInt32(ParameterId), Convert.ToInt32(AdType));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "OptionList");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["OptionList"].Rows.Count > 0)
                {
                    objdbmlOptionListTemp = new ObservableCollection<dbmlOptionList>(from dRows in dataSet.Tables["OptionList"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlOptionList>(dRows)));

                    objreturn.objdbmlOptionList = objdbmlOptionListTemp;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }
        #endregion

        #region Retail Category Master

        [HttpPost]
        public returndbmlStatus RetailCategoryMasterUpsert(requestdbmlTempRetailCategoryMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempItemTypeMaster item in objinput.objdbmlTempItemTypeMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[RetailCategoryMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Retail Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region HSNCodeMaster

        [HttpPost]
        public returndbmlStatus HSNCodeMasterUpsert(requestdbmlTempHSNCodeMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempHSNCodeMaster item in objinput.objdbmlTempHSNCodeMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[HSNCodeMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "HSN Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region DiscountMaster

        [HttpPost]
        public returndbmlStatus DiscountMasterUpsert(requestdbmlTempDiscountMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempDiscountMaster item in objinput.objdbmlTempDiscountMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[DiscountMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Discount Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        #region PriceListMaster

        [HttpPost]
        public returndbmlStatus PriceListMasterUpsert(requestdbmlTempPriceListMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempPriceListMaster item in objinput.objdbmlTempPriceListMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[PriceListMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Retail Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion
        #region RegionMaster

        [HttpPost]
        public returndbmlStatus RegionMasterUpsert(requestdbmlTempRegionMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempRegionMaster item in objinput.objdbmlTempRegionMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[RegionMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion
        #region RegionMaster

        [HttpPost]
        public returndbmlStatus SalesZoneMasterUpsert(requestdbmlTempSalesZoneMaster objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlTempSalesZoneMaster item in objinput.objdbmlTempSalesZoneMaster)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[SalesZoneMasterInsertUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = "Data Inserted Successfully";

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        #endregion

        [HttpGet]
        public returndbmlTempCustomer GetCustmerBySuperStockistId(string PartyId)
        {
            ObservableCollection<TempCustomerMaster> objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>();
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetCustmerBySuperStockistId]", Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Customer");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Customer"].Rows.Count > 0)
                {
                    objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>(from dRows in dataSet.Tables["Customer"].AsEnumerable()
                                                                                       select (co.ConvertTableToListNew<TempCustomerMaster>(dRows)));

                    objreturn.objdbmlTempCustomer = objdbmlTempCustomer;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }
        [HttpGet]
        public returnTempCustomerDiscountItemView GetItemsbySuperStockistId(string PartyID)
        {
            ObservableCollection<TempCustomerDiscountItemView> objdbmlTempCustomer = new ObservableCollection<TempCustomerDiscountItemView>();
            returnTempCustomerDiscountItemView objreturn = new returnTempCustomerDiscountItemView();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetItemsbySuperStockistId]", Convert.ToInt32(PartyID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Customer");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Customer"].Rows.Count > 0)
                {
                    objdbmlTempCustomer = new ObservableCollection<TempCustomerDiscountItemView>(from dRows in dataSet.Tables["Customer"].AsEnumerable()
                                                                                                 select (co.ConvertTableToListNew<TempCustomerDiscountItemView>(dRows)));

                    objreturn.objTempCustomerDiscountItemView = objdbmlTempCustomer;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlTempCustomer GetCustmerByUserId(string UserId)
        {
            ObservableCollection<TempCustomerMaster> objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>();
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetCustmerByUserId]", Convert.ToInt32(UserId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Customer");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Customer"].Rows.Count > 0)
                {
                    objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>(from dRows in dataSet.Tables["Customer"].AsEnumerable()
                                                                                       select (co.ConvertTableToListNew<TempCustomerMaster>(dRows)));

                    objreturn.objdbmlTempCustomer = objdbmlTempCustomer;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }
        [HttpGet]
        public returndbmlTempCustomer GetCustomerForCheck(string PartyId)
        {
            ObservableCollection<TempCustomerMaster> objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>();
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetCustomerForCheck]", Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "Customer");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Customer"].Rows.Count > 0)
                {
                    objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>(from dRows in dataSet.Tables["Customer"].AsEnumerable()
                                                                                       select (co.ConvertTableToListNew<TempCustomerMaster>(dRows)));

                    objreturn.objdbmlTempCustomer = objdbmlTempCustomer;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }
        [HttpGet]
        public returndbmlTempCustomerAddress GetCustmerAddress(string PartyId)
        {
            ObservableCollection<TempCustomerAddressMaster> objdbmlTempCustomerAddress = new ObservableCollection<TempCustomerAddressMaster>();
            returndbmlTempCustomerAddress objreturn = new returndbmlTempCustomerAddress();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetCustmerAddress]", Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CustomerAddress");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CustomerAddress"].Rows.Count > 0)
                {
                    objdbmlTempCustomerAddress = new ObservableCollection<TempCustomerAddressMaster>(from dRows in dataSet.Tables["CustomerAddress"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<TempCustomerAddressMaster>(dRows)));

                    objreturn.objdbmlTempCustomerAddress = objdbmlTempCustomerAddress;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }
        [HttpPost]
        public returndbmlTempCustomer DeleteCustomerMaster(dbmlCommon objinput)
        {
            returndbmlTempCustomer objReturn = new returndbmlTempCustomer();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intPartyId = Convert.ToInt32(objinput.StringOne);
                string intUserid = (objinput.StringTwo);
                dbCmd = db.GetStoredProcCommand("[Master].[DeleteCustomerMaster]", intPartyId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = GetCustmerByUserId(intUserid);
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }
        [HttpPost]
        public returndbmlTempCustomer CustomerMasterInsertUpdate(RequestCustomerUpdate objinput)
        {
            returndbmlTempCustomer objReturn = new returndbmlTempCustomer();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            int intOut = 0;
            string setMsg = "";
            string strMailId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();


                dbCmd = db.GetStoredProcCommand("[Master].[CustomerMasterInsertUpdate]");


                foreach (PropertyInfo PropInfoCol in objinput.objdbmlCustomerUpdateReq.GetType().GetProperties())
                {
                    DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlCustomerUpdateReq, null));
                }
                db.AddOutParameter(dbCmd, "DocIdOut", DbType.Int32, 0);
                db.AddOutParameter(dbCmd, "DocErrOut", DbType.String, 500);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@DocIdOut"));
                setMsg = db.GetParameterValue(dbCmd, "@DocErrOut").ToString();


                if (intOut >= 0)
                {
                    dbTrans.Commit();
                    //objReturn = UserMasterGetAll();
                    objReturn.objdbmlStatus.StatusId = intOut;
                    objReturn.objdbmlStatus.Status = setMsg;
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = intOut;
                    objReturn.objdbmlStatus.Status = setMsg;
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }
        [HttpPost]
        public returndbmlTempCustomerAddress CustomerAddressMasterInsertUpdate(RequestCustomerAddressUpdate objinput)
        {
            returndbmlTempCustomerAddress objReturn = new returndbmlTempCustomerAddress();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            int intOut = 0;
            string strMailId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();


                dbCmd = db.GetStoredProcCommand("[Master].[CustomerAddressMasterInsertUpdate]");


                foreach (PropertyInfo PropInfoCol in objinput.objdbmlCustomerAddressUpdateReq.GetType().GetProperties())
                {
                    DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlCustomerAddressUpdateReq, null));
                }
                db.AddOutParameter(dbCmd, "DocIdOut", DbType.Int32, 0);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@DocIdOut"));

                if (intOut >= 0)
                {
                    dbTrans.Commit();
                    //objReturn = UserMasterGetAll();
                    objReturn.objdbmlStatus.StatusId = intOut;
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = intOut;
                    objReturn.objdbmlStatus.Status = "Success";
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }
        [HttpGet]
        public returnTempDealerSalesDiscountMaster ReadDiscountMaster(string SuperStockistId, string CustomerId, string ItemId, string PartyTypeId)
        {
            ObservableCollection<TempDealerSalesDiscountMaster> objTempDealerSalesDiscountMaster = new ObservableCollection<TempDealerSalesDiscountMaster>();


            returnTempDealerSalesDiscountMaster objreturn = new returnTempDealerSalesDiscountMaster();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ReadDiscountMaster]", Convert.ToInt32(SuperStockistId), Convert.ToInt32(CustomerId), Convert.ToInt32(ItemId), Convert.ToInt32(PartyTypeId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "DiscountMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["DiscountMaster"].Rows.Count > 0)
                {
                    objTempDealerSalesDiscountMaster = new ObservableCollection<TempDealerSalesDiscountMaster>(from dRows in dataSet.Tables["DiscountMaster"].AsEnumerable()
                                                                                                               select (co.ConvertTableToListNew<TempDealerSalesDiscountMaster>(dRows)));

                    objreturn.objTempDealerSalesDiscountMaster = objTempDealerSalesDiscountMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }

                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }
        [HttpPost]
        public returnTempDealerSalesDiscountMaster SaveDiscountMaster(requestTempDealerSalesDiscountMaster objinput)
        {
            returnTempDealerSalesDiscountMaster objReturn = new returnTempDealerSalesDiscountMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                //int IDOut = 0;
                string retErrMsg = "";


                foreach (TempDealerSalesDiscountMaster itemD in objinput.objTempDealerSalesDiscountMasterReq)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[SaveDiscountMaster]");
                    foreach (PropertyInfo PropInfoCol in itemD.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(itemD, null));

                    }
                    db.AddOutParameter(dbCmd, "pERRMSG", DbType.String, 500);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    retErrMsg = (db.GetParameterValue(dbCmd, "@pERRMSG")).ToString();

                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = retErrMsg;

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }
        #region Stock Adjustment

        [HttpPost]
        public returndbmlItemAdjustment ItemAdjustmentInsert(requestdbmlItemAdjustment objinput)
        {
            returndbmlItemAdjustment objReturn = new returndbmlItemAdjustment();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int IDOut = 0, IDDetailOut = 0;

                foreach (dbmlItemAdjustment item in objinput.objdbmlItemAdjustment)
                {
                    //dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentInsertTesting]");
                    dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentInsert]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.AddOutParameter(dbCmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    IDOut = (int)db.GetParameterValue(dbCmd, "@IdOut");


                    if (IDOut > 0)
                    {
                        foreach (dbmlItemAdjustmentDetail itemD in objinput.objdbmlItemAdjustmentDetail)
                        {
                            dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentDetailInsert]");
                            foreach (PropertyInfo PropInfoCol in itemD.GetType().GetProperties())
                            {
                                DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                                if (PropInfoCol.Name == "ItemAdjustmentId")
                                {
                                    db.AddInParameter(dbCmd, "ItemAdjustmentId", dbt, IDOut);
                                }
                                else
                                {
                                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(itemD, null));
                                }

                            }
                            db.AddOutParameter(dbCmd, "IdDetailOut", DbType.Int32, 0);
                            db.ExecuteNonQuery(dbCmd, dbTrans);
                            IDDetailOut = (int)db.GetParameterValue(dbCmd, "@IdDetailOut");
                            // insert into Ledger


                            //RequestItemLedgerInsert objRequestItemLedgerInsert = new RequestItemLedgerInsert();
                            //dbmlItemLedger objIL = new dbmlItemLedger();
                            //objIL.ItemLedgerId = 0;
                            //objIL.ItemId = itemD.ItemId;
                            //objIL.LedgerDate = objinput.objdbmlItemAdjustment.FirstOrDefault().VoucherDate;
                            //objIL.PartyId = objinput.objdbmlItemAdjustment.FirstOrDefault().PartyId;
                            //objIL.BPId = 3;
                            //objIL.SalesChallanID = IDOut;
                            //objIL.SalesChallanDetailID = IDDetailOut;
                            //objIL.BasicRate = itemD.Rate;
                            //if (itemD.QuantityIn > 0)
                            //{
                            //    objIL.Amount = itemD.QuantityIn * itemD.Rate;
                            //}
                            //else
                            //{ objIL.Amount = itemD.QuantityOut * itemD.Rate; }
                            //objIL.QuantityIn = itemD.QuantityIn;
                            //objIL.QuantityOut = itemD.QuantityOut;
                            //objIL.Narration = "Stock Adjustment";
                            //objIL.OppNarration = "";
                            //objIL.CreatedBy = itemD.CreatedBy;
                            //objIL.CreatedDate = itemD.CreatedDate;
                            //objIL.EditedBy = itemD.EditedBy;
                            //objIL.EditedDate = itemD.EditedDate;
                            //objRequestItemLedgerInsert.objdbmlItemLedgerreq.Add(objIL);

                            //ReturndbmlItemLedger objdbml = ItemLedgerInsert(objRequestItemLedgerInsert);



                        }


                        dbTrans.Commit();
                        objReturn.objdbmlStatus.StatusId = 1;
                        objReturn.objdbmlStatus.Status = " Data Inserted Successfully";

                    }
                    else
                    {
                        dbTrans.Commit();
                        objReturn.objdbmlStatus.StatusId = 2;
                        objReturn.objdbmlStatus.Status = " Duplicate Voucher..";
                    }

                }

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlItemAdjustment ItemAdjustmentEdit(requestdbmlItemAdjustment objinput)
        {
            returndbmlItemAdjustment objReturn = new returndbmlItemAdjustment();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try

            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int IDOut = 0, IDDetailOut = 0;

                foreach (dbmlItemAdjustment item in objinput.objdbmlItemAdjustment)
                {
                    //dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentUpdateTesting]");
                    dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentUpdate]");
                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    // IDOut = (int)db.GetParameterValue(dbCmd, "@IdOut");

                }
                foreach (dbmlItemAdjustmentDetail itemD in objinput.objdbmlItemAdjustmentDetail)
                {
                    if (itemD.ItemAdjustmentDetailId == 0)
                    {
                        dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentDetailInsert]");
                        foreach (PropertyInfo PropInfoCol in itemD.GetType().GetProperties())
                        {
                            DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                            if (PropInfoCol.Name == "ItemAdjustmentId")
                            {
                                db.AddInParameter(dbCmd, "ItemAdjustmentId", dbt, itemD.ItemAdjustmentId);
                            }
                            else
                            {
                                db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(itemD, null));
                            }

                        }
                        db.AddOutParameter(dbCmd, "IdDetailOut", DbType.Int32, 0);
                        db.ExecuteNonQuery(dbCmd, dbTrans);
                        IDDetailOut = (int)db.GetParameterValue(dbCmd, "@IdDetailOut");
                    }
                    else
                    {
                        dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentDetailUpdate]");
                        foreach (PropertyInfo PropInfoCol in itemD.GetType().GetProperties())
                        {
                            DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                            db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(itemD, null));
                            //  }
                        }
                        db.ExecuteNonQuery(dbCmd, dbTrans);
                    }

                }
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = " Data Updated Successfully";
                //if (objinput.objdbmlItemAdjustment != null)
                //{

                //    Database db = new SqlDatabase(co.StrSetConnection());
                //    DbCommand dbCmd = null;
                //    dbCon = db.CreateConnection();
                //    dbCon.Open();
                //    dbTrans = dbCon.BeginTransaction();



                //    int intItemAdjustmentId = objinput.objdbmlItemAdjustment.FirstOrDefault().ItemAdjustmentId;
                //    // ------------ Delete Item First -----------------
                //    objReturn = ItemAdjustmentDelete(intItemAdjustmentId);
                //    if (Convert.ToInt32(objReturn.objdbmlStatus.StatusId) == 1)
                //    {
                //        objReturn = ItemAdjustmentInsert(objinput);
                //    }
                //    dbTrans.Commit();
                //    objReturn.objdbmlStatus.StatusId = 1;
                //    objReturn.objdbmlStatus.Status = " Data Updated Successfully";
                //}
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }



        [HttpPost]
        public returndbmlItemAdjustment ItemAdjustmentDelete(dbmlCommon objinput)
        {
            returndbmlItemAdjustment objReturn = new returndbmlItemAdjustment();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intItemAdjustmentId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentDelete]", intItemAdjustmentId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = " Data Deleted Successfully";
                // objReturn = UserMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlItemAdjustment ItemAdjustmentGetById(string ItemAdjustmentId)
        {
            ObservableCollection<dbmlItemAdjustment> objdbmlItemAdjustmentTemp = new ObservableCollection<dbmlItemAdjustment>();
            ObservableCollection<dbmlItemAdjustmentDetail> objdbmlItemAdjustmentDetailTemp = new ObservableCollection<dbmlItemAdjustmentDetail>();

            returndbmlItemAdjustment objreturn = new returndbmlItemAdjustment();
            try
            {
                DataSet dataSet = new DataSet();
                DataSet dataSetD = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[ItemAdjustmentGetByItemAdjustmentId]", Convert.ToInt32(ItemAdjustmentId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "ItemAdjustment");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["ItemAdjustment"].Rows.Count > 0)
                {
                    objdbmlItemAdjustmentTemp = new ObservableCollection<dbmlItemAdjustment>(from dRows in dataSet.Tables["ItemAdjustment"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlItemAdjustment>(dRows)));

                    objreturn.objdbmlItemAdjustment = objdbmlItemAdjustmentTemp;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dbCommand = database.GetStoredProcCommand("[Stock].[ItemAdjustmentDetailGetByItemAdjustmentId]", Convert.ToInt32(ItemAdjustmentId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSetD, "ItemAdjustmentDetail");
                if (dataSetD.Tables.Count > 0 && dataSetD.Tables["ItemAdjustmentDetail"].Rows.Count > 0)
                {
                    objdbmlItemAdjustmentDetailTemp = new ObservableCollection<dbmlItemAdjustmentDetail>(from dRows in dataSetD.Tables["ItemAdjustmentDetail"].AsEnumerable()
                                                                                                         select (co.ConvertTableToListNew<dbmlItemAdjustmentDetail>(dRows)));

                    objreturn.objdbmlItemAdjustmentDetail = objdbmlItemAdjustmentDetailTemp;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlItemAdjustmentDateWIse ItemAdjustmentGetByFromDateToDate(string FromDate, string ToDate, int SuperStockistID, string AdjustTypeList)
        {
            ObservableCollection<dbmlItemAdjustmentDateWIse> objdbmlItemAdjustmentTemp = new ObservableCollection<dbmlItemAdjustmentDateWIse>();


            returndbmlItemAdjustmentDateWIse objreturn = new returndbmlItemAdjustmentDateWIse();
            try
            {
                //string FFDate = "";
                //string TTDate = "";
                //var FDate = FromDate.Split('/');
                //var TDate = ToDate.Split('/');
                //FFDate = FDate[1] + "/" + FDate[0] + "/" + FDate[2];
                //TTDate = TDate[1] + "/" + TDate[0] + "/" + TDate[2];
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[ItemAdjustmentGetByFromDateToDate]", FromDate, ToDate, SuperStockistID, AdjustTypeList);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "ItemAdjustment");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["ItemAdjustment"].Rows.Count > 0)
                {
                    objdbmlItemAdjustmentTemp = new ObservableCollection<dbmlItemAdjustmentDateWIse>(from dRows in dataSet.Tables["ItemAdjustment"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlItemAdjustmentDateWIse>(dRows)));

                    objreturn.objdbmlItemAdjustment = objdbmlItemAdjustmentTemp;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }

                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }



        [HttpPost]
        public returndbmlItemAdjustment ItemAdjustmentDetailDelete(dbmlCommon objinput)
        {
            returndbmlItemAdjustment objReturn = new returndbmlItemAdjustment();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intItemAdjustmentDetailId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Stock].[ItemAdjustmentDetailDelete]", intItemAdjustmentDetailId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn.objdbmlStatus.StatusId = 1;
                objReturn.objdbmlStatus.Status = " Data Deleted Successfully";
                // objReturn = UserMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlTempItemMaster GetAllItemsDPRate(string PartyID, string ItemId, string VoucherDate)
        {
            ObservableCollection<dbmlTempItemMasterDPRate> objdbmlTempItemMasterDPRate = new ObservableCollection<dbmlTempItemMasterDPRate>();
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetAllItemsDPRate]", Convert.ToInt32(PartyID), Convert.ToInt32(ItemId), DateTime.ParseExact(VoucherDate, "yyyy-MM-dd", null));
                dbCommand.CommandTimeout = 80000000;

                database.LoadDataSet(dbCommand, dataSet, new string[] { "TempItemMasterDPRate" });
                if (dataSet.Tables["TempItemMasterDPRate"].Rows.Count > 0)
                {
                    objdbmlTempItemMasterDPRate = new ObservableCollection<dbmlTempItemMasterDPRate>(from dRows in dataSet.Tables["TempItemMasterDPRate"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlTempItemMasterDPRate>(dRows)));

                    objreturn.objdbmlTempItemMasterDPRate = objdbmlTempItemMasterDPRate;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }
        #endregion

        #region SalesInvoice
        private Boolean ValidColumn(string str)
        {

            if (str.Length <= 2)
                str = str + "modified";
            if (str.Substring(0, 2).ToUpper() != "ZZ" && str != "DUMMY" && str != "ZZDumSeq")
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public returndbmlCustomerMaster GetBuyersbyPartyID(string PartyID)
        {
            ObservableCollection<dbmlCustomerMaster> objdbmlCustomerMaster = new ObservableCollection<dbmlCustomerMaster>();
            returndbmlCustomerMaster objreturn = new returndbmlCustomerMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetBuyersbyPartyID]", Convert.ToInt32(PartyID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CustomerMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CustomerMaster"].Rows.Count > 0)
                {
                    objdbmlCustomerMaster = new ObservableCollection<dbmlCustomerMaster>(from dRows in dataSet.Tables["CustomerMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlCustomerMaster>(dRows)));

                    objreturn.objdbmlCustomerMaster = objdbmlCustomerMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlFreightTypeMaster GetFreightTypeMaster()
        {
            ObservableCollection<dbmlFreightTypeMaster> objdbmlFreightTypeMaster = new ObservableCollection<dbmlFreightTypeMaster>();
            returndbmlFreightTypeMaster objreturn = new returndbmlFreightTypeMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetFreightTypeMaster]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "FreightTypeMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["FreightTypeMaster"].Rows.Count > 0)
                {
                    objdbmlFreightTypeMaster = new ObservableCollection<dbmlFreightTypeMaster>(from dRows in dataSet.Tables["FreightTypeMaster"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlFreightTypeMaster>(dRows)));

                    objreturn.objdbmlFreightTypeMaster = objdbmlFreightTypeMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlCustomerAddressMaster GetBuyersAddressbyPartyID(string PartyID)
        {
            ObservableCollection<dbmlCustomerAddressMaster> objdbmlCustomerAddressMaster = new ObservableCollection<dbmlCustomerAddressMaster>();
            returndbmlCustomerAddressMaster objreturn = new returndbmlCustomerAddressMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetBuyersAddressbyPartyID]", Convert.ToInt32(PartyID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CustomerMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CustomerMaster"].Rows.Count > 0)
                {
                    objdbmlCustomerAddressMaster = new ObservableCollection<dbmlCustomerAddressMaster>(from dRows in dataSet.Tables["CustomerMaster"].AsEnumerable()
                                                                                                       select (co.ConvertTableToListNew<dbmlCustomerAddressMaster>(dRows)));

                    objreturn.objdbmlCustomerAddressMaster = objdbmlCustomerAddressMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlTempItemMaster GetItemsbyPartyID(string PartyID, string BuyerID, string InvoiceDate)
        {
            ObservableCollection<dbmlTempItemMaster> objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>();
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetItemsbyPartyID]", Convert.ToInt32(PartyID), Convert.ToInt32(BuyerID), DateTime.ParseExact(InvoiceDate, "yyyy-MM-dd", null));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "TempItemMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["TempItemMaster"].Rows.Count > 0)
                {
                    objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>(from dRows in dataSet.Tables["TempItemMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlTempItemMaster>(dRows)));

                    objreturn.objdbmlTempItemMaster = objdbmlTempItemMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpPost]
        public returndbmlDealerSalesChallan DealerSalesChallanInsert(requestdbmlDealerSalesChallan objinput)
        {
            returndbmlDealerSalesChallan objReturn = new returndbmlDealerSalesChallan();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            Boolean TComm = true;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                int IDOut = 0;
                String IDOutDtl = "";

                dbCmd = db.GetStoredProcCommand("[Trans].[CheckDuplicateInvoiceNo]", objinput.objdbmlDealerSalesChallan.InvoiceNo, 0, objinput.objdbmlDealerSalesChallan.SuperStockistID, objinput.objdbmlDealerSalesChallan.DealerSalesChallanID, 0);
                db.ExecuteNonQuery(dbCmd);
                IDOut = (int)db.GetParameterValue(dbCmd, "@IdOut");

                if (IDOut == 0)
                {
                    if (objinput.objdbmlDealerSalesChallan.ZZInvoiceDate != null)
                        objinput.objdbmlDealerSalesChallan.InvoiceDate = DateTime.ParseExact(objinput.objdbmlDealerSalesChallan.ZZInvoiceDate, "yyyy-MM-dd", null);
                    if (objinput.objdbmlDealerSalesChallan.ZZLRDate != null)
                        objinput.objdbmlDealerSalesChallan.LRDate = DateTime.ParseExact(objinput.objdbmlDealerSalesChallan.ZZLRDate, "yyyy-MM-dd", null);
                    if (objinput.objdbmlDealerSalesChallan.ZZEWayBillDate != null)
                        objinput.objdbmlDealerSalesChallan.EWayBillDate = DateTime.ParseExact(objinput.objdbmlDealerSalesChallan.ZZEWayBillDate, "yyyy-MM-dd", null);

                    dbTrans = dbCon.BeginTransaction();
                    dbCmd = db.GetStoredProcCommand("[Trans].[DealerSalesChallanInsert]");
                    //dbCmd = db.GetStoredProcCommand("[Trans].[DealerSalesChallanPurchaseReturnInsert]");
                    foreach (PropertyInfo PropInfoCol in objinput.objdbmlDealerSalesChallan.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        //commented since we are passig DealerSalesChallanId
                        //if (PropInfoCol.Name != "DealerSalesChallanID" && ValidColumn(PropInfoCol.Name))
                        if (ValidColumn(PropInfoCol.Name) && PropInfoCol.Name != "IsRoundOff" && PropInfoCol.Name != "InvoiceDateVal")
                        {
                            db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlDealerSalesChallan, null));
                        }
                    }
                    db.AddOutParameter(dbCmd, "IdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    IDOut = (int)db.GetParameterValue(dbCmd, "@IdOut");

                    foreach (dbmlDealerSalesChallanDetail itemD in objinput.objdbmlDealerSalesChallanDetail)
                    {
                        IDOutDtl = "";
                        //commented on 22 Feb 2022 to save DPRate from dropdown
                        //dbCmd = db.GetStoredProcCommand("[Trans].[DealerSalesChallanDetailsInsert]");
                        //uncomment the below line and comment the above line when DPdropdown is implemented
                        dbCmd = db.GetStoredProcCommand("[Trans].[DealerSalesChallanDetailsInsertDPRate]");
                        foreach (PropertyInfo PropInfoCol in itemD.GetType().GetProperties())
                        {
                            DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                            //remove  && (PropInfoCol.Name != "DPRate" when DPdropdown is implemented
                            if (PropInfoCol.Name != "DealerSalesChallanDetailID" && ValidColumn(PropInfoCol.Name))
                            {
                                if (PropInfoCol.Name == "DealerSalesChallanID")
                                {
                                    db.AddInParameter(dbCmd, "DealerSalesChallanID", dbt, IDOut);
                                }
                                else
                                {
                                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(itemD, null));
                                }
                            }
                        }
                        db.AddOutParameter(dbCmd, "IdOut", DbType.String, 500);
                        db.ExecuteNonQuery(dbCmd, dbTrans);
                        IDOutDtl = (String)db.GetParameterValue(dbCmd, "@IdOut");

                        if (IDOutDtl != "1")
                        {
                            dbTrans.Rollback();
                            objReturn.objdbmlStatus.StatusId = 99;
                            objReturn.objdbmlStatus.Status = IDOutDtl + "- Qty should be less than or equal to Stock Qty.";
                            TComm = false;
                            break;
                        }

                    }
                    if (TComm == true)
                    {
                        dbTrans.Commit();
                        objReturn.objdbmlStatus.StatusId = 1;
                        objReturn.objdbmlStatus.Status = " Data Inserted Successfully";
                    }
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = 99;
                    objReturn.objdbmlStatus.Status = "Invoice No. already exists.";
                }
            }
            catch (Exception ex)
            {
                if (TComm != false)
                {
                    dbTrans.Rollback();
                    objReturn.objdbmlStatus.StatusId = 99;
                    objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
                }
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlChargeMaster GetChargeMaster()
        {
            ObservableCollection<dbmlChargeMaster> objdbmlChargeMaster = new ObservableCollection<dbmlChargeMaster>();
            returndbmlChargeMaster objreturn = new returndbmlChargeMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetChargeMaster]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "ChargeMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["ChargeMaster"].Rows.Count > 0)
                {
                    objdbmlChargeMaster = new ObservableCollection<dbmlChargeMaster>(from dRows in dataSet.Tables["ChargeMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlChargeMaster>(dRows)));

                    objreturn.objdbmlChargeMaster = objdbmlChargeMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returnDealerSalesChallanGetByFromDateToDateResult DealerSalesChallanGetByFromDateToDate(string PartyID, string FromDate, string ToDate, string InvoiceNo)
        {
            ObservableCollection<DealerSalesChallanGetByFromDateToDateResult> objDealerSalesChallanGetByFromDateToDateResult = new ObservableCollection<DealerSalesChallanGetByFromDateToDateResult>();
            returnDealerSalesChallanGetByFromDateToDateResult objreturn = new returnDealerSalesChallanGetByFromDateToDateResult();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[DealerSalesChallanGetByFromDateToDate]", Convert.ToInt32(PartyID), DateTime.ParseExact(FromDate, "MM/dd/yyyy", null), DateTime.ParseExact(ToDate, "MM/dd/yyyy", null), InvoiceNo);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "DealerSalesChallanGetByFromDateToDate");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["DealerSalesChallanGetByFromDateToDate"].Rows.Count > 0)
                {
                    objDealerSalesChallanGetByFromDateToDateResult = new ObservableCollection<DealerSalesChallanGetByFromDateToDateResult>(from dRows in dataSet.Tables["DealerSalesChallanGetByFromDateToDate"].AsEnumerable()
                                                                                                                                           select (co.ConvertTableToListNew<DealerSalesChallanGetByFromDateToDateResult>(dRows)));

                    objreturn.objDealerSalesChallanGetByFromDateToDateResult = objDealerSalesChallanGetByFromDateToDateResult;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlDealerSalesChallan DealerSalesChallanGetById(string DealerSalesChallanID)
        {
            ObservableCollection<dbmlDealerSalesChallan> objdbmlDealerSalesChallan = new ObservableCollection<dbmlDealerSalesChallan>();
            ObservableCollection<dbmlDealerSalesChallanDetail> objdbmlDealerSalesChallanDetail = new ObservableCollection<dbmlDealerSalesChallanDetail>();
            returndbmlDealerSalesChallan objreturn = new returndbmlDealerSalesChallan();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[DealerSalesChallanGetById]", Convert.ToInt32(DealerSalesChallanID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "DealerSalesChallan", "DealerSalesChallanDetails" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["DealerSalesChallan"].Rows.Count > 0 && dataSet.Tables["DealerSalesChallanDetails"].Rows.Count > 0)
                {
                    objdbmlDealerSalesChallan = new ObservableCollection<dbmlDealerSalesChallan>(from dRows in dataSet.Tables["DealerSalesChallan"].AsEnumerable()
                                                                                                 select (co.ConvertTableToListNew<dbmlDealerSalesChallan>(dRows)));
                    objdbmlDealerSalesChallanDetail = new ObservableCollection<dbmlDealerSalesChallanDetail>(from dRows in dataSet.Tables["DealerSalesChallanDetails"].AsEnumerable()
                                                                                                             select (co.ConvertTableToListNew<dbmlDealerSalesChallanDetail>(dRows)));

                    objreturn.objdbmlDealerSalesChallan = objdbmlDealerSalesChallan[0];
                    objreturn.objdbmlDealerSalesChallanDetail = objdbmlDealerSalesChallanDetail;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returnGetSalesInvoicePrintResult GetSalesInvoicePrint(string DealerSalesChallanID)
        {
            returnGetSalesInvoicePrintResult objreturn = new returnGetSalesInvoicePrintResult();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetSalesInvoicePrint]", Convert.ToInt32(DealerSalesChallanID));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "DealerSalesChallan", "DealerSalesChallanDetails", "DealerSalesChallanGST" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["DealerSalesChallan"].Rows.Count > 0 && dataSet.Tables["DealerSalesChallanDetails"].Rows.Count > 0 && dataSet.Tables["DealerSalesChallanGST"].Rows.Count > 0)
                {
                    DataTable dtDealerSalesChallan = dataSet.Tables["DealerSalesChallan"];
                    DataTable dtDealerSalesChallanDetails = dataSet.Tables["DealerSalesChallanDetails"];
                    DataTable dtDealerSalesChallanGST = dataSet.Tables["DealerSalesChallanGST"];

                    objreturn.dtDealerSalesChallan = DataTableToJSONWithStringBuilder(dtDealerSalesChallan);
                    objreturn.dtDealerSalesChallanDetails = DataTableToJSONWithStringBuilder(dtDealerSalesChallanDetails);
                    objreturn.dtDealerSalesChallanGST = DataTableToJSONWithStringBuilder(dtDealerSalesChallanGST);
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        #endregion

        #region Stock Ledger Report
        [HttpGet]
        public returndbmlRetailCategoryMaster RetailCategoryMasterGetAll()
        {
            ObservableCollection<dbmlRetailCategoryMaster> objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[RetailCategoryMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "RetailCategory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["RetailCategory"].Rows.Count > 0)
                {
                    objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>(from dRows in dataSet.Tables["RetailCategory"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlRetailCategoryMaster>(dRows)));

                    objreturn.objdbmlRetailCategoryMaster = objdbmlRetailCategoryMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlItemMaster ItemMasterGetByRetailCategoryId(string RetailCategoryId)
        {
            ObservableCollection<dbmlItemMasterView> objdbmlItemMasterView = new ObservableCollection<dbmlItemMasterView>();
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[ItemMasterGetByRetailCategoryId]", RetailCategoryId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlItemMasterView = new ObservableCollection<dbmlItemMasterView>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlItemMasterView>(dRows)));

                    objreturn.objdbmlItemMasterView = objdbmlItemMasterView;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public requestdbmlItemLedgerReport ItemLedgerGetByItemIdFromDateToDate(string SubCategoryId, string ItemId, string Fromdate, string Todate, string PartyId)
        {
            ObservableCollection<dbmlItemLedgerReport> objdbmlItemLedgerReport = new ObservableCollection<dbmlItemLedgerReport>();
            requestdbmlItemLedgerReport objreturn = new requestdbmlItemLedgerReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[ItemLedgerGetByItemIdFromDateToDate]", SubCategoryId, ItemId, Fromdate, Todate, PartyId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlItemLedgerReport = new ObservableCollection<dbmlItemLedgerReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlItemLedgerReport>(dRows)));

                    objreturn.objdbmlItemLedgerReport = objdbmlItemLedgerReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public requestdbmlitemLedgerDetails ItemLedgerDetailGetByItemIdFromDateToDate(string ItemId, string Fromdate, string Todate, string PartyId)
        {
            ObservableCollection<dbmlitemLedgerDetails> objdbmlitemLedgerDetails = new ObservableCollection<dbmlitemLedgerDetails>();
            requestdbmlitemLedgerDetails objreturn = new requestdbmlitemLedgerDetails();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[ItemLedgerDetailGetByItemIdFromDateToDate]", ItemId, Fromdate, Todate, PartyId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlitemLedgerDetails = new ObservableCollection<dbmlitemLedgerDetails>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlitemLedgerDetails>(dRows)));

                    objreturn.objdbmlitemLedgerDetails = objdbmlitemLedgerDetails;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion


        #region News Master

        [HttpPost]
        public returndbmlNewsMaster NewsMasterInsert(RequestNewsInsert objinput)
        {
            returndbmlNewsMaster objReturn = new returndbmlNewsMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            int intOut = 0;
            string strMailId = "";
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlNewsMaster item in objinput.objdbmlNews)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[NewsMasterInsert]");
                    //string strNewsPassword = Cryptographer.CreateHash("DealerPortal", item.Password.ToString());
                    //strMailId = Convert.ToString(item.EmailID);

                    foreach (PropertyInfo PropInfoCol in item.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(item, null));
                    }
                    db.AddOutParameter(dbCmd, "DocIdOut", DbType.Int32, 0);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@DocIdOut"));
                }
                if (intOut >= 0)
                {
                    dbTrans.Commit();
                    objReturn = NewsMasterGetAll();
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = intOut;
                    objReturn.objdbmlStatus.Status = "News with Title already exist - " + strMailId;
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlNewsMaster NewsMasterUpdate(RequestNewsUpdate objinput)
        {
            returndbmlNewsMaster objReturn = new returndbmlNewsMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                dbCmd = db.GetStoredProcCommand("[Master].[NewsMasterUpdate]");
                foreach (PropertyInfo PropInfoCol in objinput.objdbmlNewsMasterUpdateReq.GetType().GetProperties())
                {
                    DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                    db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlNewsMasterUpdateReq, null));
                }
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = NewsMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpPost]
        public returndbmlNewsMaster NewsMasterStatusUpdate(RequestNewsInsert objinput)
        {
            returndbmlNewsMaster objReturn = new returndbmlNewsMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;

                //  SqlCommand dbsqlCmd = new SqlCommand();
                // SqlConnection dbsqlCon = new SqlConnection();
                // dbsqlCon.ConnectionString = co.StrSetConnection();

                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();

                foreach (dbmlNewsMaster item in objinput.objdbmlNews)
                {
                    dbCmd = db.GetStoredProcCommand("[Master].[NewsMasterUpdateStatus]", item.NewsId, item.Active);
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                }
                dbTrans.Commit();
                objReturn = NewsMasterGetAll();

            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }

        [HttpGet]
        public returndbmlNewsMaster NewsMasterGetAll()
        {
            ObservableCollection<dbmlNewsMasterView> objdbmlNewsMaster = new ObservableCollection<dbmlNewsMasterView>();
            returndbmlNewsMaster objreturn = new returndbmlNewsMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[NewsMasterGetAll]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "NewsMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["NewsMaster"].Rows.Count > 0)
                {
                    objdbmlNewsMaster = new ObservableCollection<dbmlNewsMasterView>(from dRows in dataSet.Tables["NewsMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlNewsMasterView>(dRows)));

                    objreturn.objdbmlNewsMasterView = objdbmlNewsMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpPost]
        public returndbmlNewsMaster NewsMasterDelete(dbmlCommon objinput)
        {
            returndbmlNewsMaster objReturn = new returndbmlNewsMaster();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intNewsId = Convert.ToInt32(objinput.StringOne);
                dbCmd = db.GetStoredProcCommand("[Master].[NewsMasterDelete]", intNewsId);
                db.ExecuteNonQuery(dbCmd, dbTrans);
                dbTrans.Commit();
                objReturn = NewsMasterGetAll();
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }


        [HttpGet]
        public returndbmlChargeMaster GetTCSChargeMaster()
        {
            ObservableCollection<dbmlChargeMaster> objdbmlChargeMaster = new ObservableCollection<dbmlChargeMaster>();
            returndbmlChargeMaster objreturn = new returndbmlChargeMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetTCSChargeMaster]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "ChargeMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["ChargeMaster"].Rows.Count > 0)
                {
                    objdbmlChargeMaster = new ObservableCollection<dbmlChargeMaster>(from dRows in dataSet.Tables["ChargeMaster"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlChargeMaster>(dRows)));

                    objreturn.objdbmlChargeMaster = objdbmlChargeMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }
        #endregion


        #region Inventory Report
        [HttpGet]
        public requestdbmlItemLedgerReport InventoryFromDateToDateReport(string SubCategoryId, string Fromdate, string Todate, string PartyId)
        {
            ObservableCollection<dbmlItemLedgerReport> objdbmlItemLedgerReport = new ObservableCollection<dbmlItemLedgerReport>();
            requestdbmlItemLedgerReport objreturn = new requestdbmlItemLedgerReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[InventoryFromDateToDateReport]", SubCategoryId, Fromdate, Todate, PartyId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlItemLedgerReport = new ObservableCollection<dbmlItemLedgerReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlItemLedgerReport>(dRows)));

                    objreturn.objdbmlItemLedgerReport = objdbmlItemLedgerReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

        #region PartyProductWiseSummary Report
        [HttpGet]
        public requestdbmlSalesReport PartyProductWiseSummaryFromDateToDateReport(string SuperStockistID, string Fromdate, string Todate, int CustomerID, int ReportType)
        {
            ObservableCollection<dbmlSalesReport> objdbmlSalesReport = new ObservableCollection<dbmlSalesReport>();
            requestdbmlSalesReport objreturn = new requestdbmlSalesReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetSalesReport]", SuperStockistID, Fromdate, Todate, CustomerID, ReportType);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlSalesReport = new ObservableCollection<dbmlSalesReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                   select (co.ConvertTableToListNew<dbmlSalesReport>(dRows)));

                    objreturn.objdbmlSalesReport = objdbmlSalesReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion
        // Sandeep 8/8/2022
        #region Invoice Statistics Data Report
        [HttpGet]
        public requestdbmlStatisticsReport InvoiceStatisticsDataReportFromDateToDateReport(string SuperStockistID, int CustomerID, string Fromdate, string Todate)
        {
            ObservableCollection<dbmlStatisticsReport> objdbmlStatisticsReport = new ObservableCollection<dbmlStatisticsReport>();
            requestdbmlStatisticsReport objreturn = new requestdbmlStatisticsReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetInvoiceStatiticsData]", SuperStockistID, 0, Fromdate, Todate);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlStatisticsReport = new ObservableCollection<dbmlStatisticsReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                   select (co.ConvertTableToListNew<dbmlStatisticsReport>(dRows)));

                    objreturn.objdbmlStatisticsReport = objdbmlStatisticsReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion


        // Sandeep 13/8/2022
        #region ReprimarySaleReport
        [HttpGet]
        public requestdbmlReprimarySaleReport ReprimarySaleReportFromDateToDateReport(string Fromdate, string Todate, string SuperStockistID, int ReportTypeID )
        {
            ObservableCollection<dbmlReprimarySaleReport> objdbmlReprimarySaleReport = new ObservableCollection<dbmlReprimarySaleReport>();
            requestdbmlReprimarySaleReport objreturn = new requestdbmlReprimarySaleReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetRePrimarySalesReportData]", SuperStockistID, ReportTypeID, Fromdate, Todate);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlReprimarySaleReport = new ObservableCollection<dbmlReprimarySaleReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                             select (co.ConvertTableToListNew<dbmlReprimarySaleReport>(dRows)));

                    objreturn.objdbmlReprimarySaleReport = objdbmlReprimarySaleReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

        #region Download Report
        [HttpGet]
        public requestdbmlDownloadInvoiceReport DownloadFromDateToDateReport(string DowloadReportType, string Fromdate, string Todate, string PartyId)
        {
            ObservableCollection<dbmlDownloadInvoiceReport> objdbmlDownloadInvoiceReport = new ObservableCollection<dbmlDownloadInvoiceReport>();
            requestdbmlDownloadInvoiceReport objreturn = new requestdbmlDownloadInvoiceReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetReport]", PartyId, Fromdate, Todate, DowloadReportType);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlDownloadInvoiceReport = new ObservableCollection<dbmlDownloadInvoiceReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                                       select (co.ConvertTableToListNew<dbmlDownloadInvoiceReport>(dRows)));

                    objreturn.objdbmlDownloadInvoiceReport = objdbmlDownloadInvoiceReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

        #region Stock Report Valuation

        [HttpGet]
        public requestdbmlItemLedgerValuationReport GetStockReportValuation(string SubCategoryId, string ItemId, string Fromdate, string Todate, string PartyId)
        {
            ObservableCollection<dbmlItemLedgerValuationReport> objdbmlItemLedgerValuationReport = new ObservableCollection<dbmlItemLedgerValuationReport>();
            requestdbmlItemLedgerValuationReport objreturn = new requestdbmlItemLedgerValuationReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Stock].[GetStockReportValuation]", SubCategoryId, ItemId, PartyId, Fromdate, Todate);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlItemLedgerValuationReport = new ObservableCollection<dbmlItemLedgerValuationReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                                               select (co.ConvertTableToListNew<dbmlItemLedgerValuationReport>(dRows)));

                    objreturn.objdbmlItemLedgerValuationReport = objdbmlItemLedgerValuationReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }
        #endregion

        [HttpGet]
        public returndbmlRetailCategoryMaster RetailCategoryMasterGetByParentIdForDropdown(string ParentId)
        {
            ObservableCollection<dbmlRetailCategoryMaster> objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[RetailCategoryMasterGetByParentIdForDropdown]", ParentId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "RetailCategory");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["RetailCategory"].Rows.Count > 0)
                {
                    objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>(from dRows in dataSet.Tables["RetailCategory"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlRetailCategoryMaster>(dRows)));

                    objreturn.objdbmlRetailCategoryMaster = objdbmlRetailCategoryMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }


        [HttpGet]
        public returndbmlUOMMaster GetUOMMaster()
        {
            ObservableCollection<dbmlUOMMaster> objdbmlUOMMaster = new ObservableCollection<dbmlUOMMaster>();
            returndbmlUOMMaster objreturn = new returndbmlUOMMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetUOMMaster]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "UOMMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["UOMMaster"].Rows.Count > 0)
                {
                    objdbmlUOMMaster = new ObservableCollection<dbmlUOMMaster>(from dRows in dataSet.Tables["UOMMaster"].AsEnumerable()
                                                                               select (co.ConvertTableToListNew<dbmlUOMMaster>(dRows)));

                    objreturn.objdbmlUOMList = objdbmlUOMMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }



        [HttpGet]
        public returndbmlHSNCodeMaster GetHSNCodeMaster()
        {
            ObservableCollection<dbmlTempHSNCodeMaster> objdbmlHSNCodeMaster = new ObservableCollection<dbmlTempHSNCodeMaster>();
            returndbmlHSNCodeMaster objreturn = new returndbmlHSNCodeMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetHSNCodeMaster]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "HSNCodeMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["HSNCodeMaster"].Rows.Count > 0)
                {
                    objdbmlHSNCodeMaster = new ObservableCollection<dbmlTempHSNCodeMaster>(from dRows in dataSet.Tables["HSNCodeMaster"].AsEnumerable()
                                                                                           select (co.ConvertTableToListNew<dbmlTempHSNCodeMaster>(dRows)));

                    objreturn.objdbmlHSNCodeList = objdbmlHSNCodeMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        [HttpPost]
        public returndbmlStatus AddEditItemMasterForm(requestdbmlTempItemInsertUpdateNew objinput)
        {
            returndbmlStatus objReturn = new returndbmlStatus();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                int IDOut = 0;
                String IDOutDtl = "";

                dbCmd = db.GetStoredProcCommand("[Master].[CheckDuplicateItemName]", objinput.objdbmlTempItemInsertUpdateNew.ItemId, objinput.objdbmlTempItemInsertUpdateNew.ItemName, 0);
                db.ExecuteNonQuery(dbCmd);
                IDOut = (int)db.GetParameterValue(dbCmd, "@IdOut");

                if (IDOut == 0)
                {
                    dbTrans = dbCon.BeginTransaction();

                    dbCmd = db.GetStoredProcCommand("[Master].[AddEditItemMasterForm]");
                    foreach (PropertyInfo PropInfoCol in objinput.objdbmlTempItemInsertUpdateNew.GetType().GetProperties())
                    {
                        DbType dbt = co.ConvertNullableIntoDatatype(PropInfoCol);
                        db.AddInParameter(dbCmd, PropInfoCol.Name, dbt, PropInfoCol.GetValue(objinput.objdbmlTempItemInsertUpdateNew, null));
                    }
                    db.ExecuteNonQuery(dbCmd, dbTrans);
                    dbTrans.Commit();
                    objReturn.objdbmlStatus.StatusId = 1;
                    objReturn.objdbmlStatus.Status = "Item Data Saved Successfully";
                }
                else
                {
                    objReturn.objdbmlStatus.StatusId = 99;
                    objReturn.objdbmlStatus.Status = "Item Name already exists.";
                }
            }
            catch (Exception ex)
            {
                dbTrans.Rollback();
                objReturn.objdbmlStatus.StatusId = 99;
                objReturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objReturn;
        }


        [HttpGet]
        public returndbmlMailServerDetails GetMailServerDetailsByOrderId(string MailTypeID, string SuperStockistID, string OrderID)
        {
            ObservableCollection<dbmlMailServerDetails> objdbmlMailServerDetails = new ObservableCollection<dbmlMailServerDetails>();
            returndbmlMailServerDetails objreturn = new returndbmlMailServerDetails();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[GetMailServerDetailsByOrderId]", MailTypeID, SuperStockistID, OrderID);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "MailType");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["MailType"].Rows.Count > 0)
                {
                    objdbmlMailServerDetails = new ObservableCollection<dbmlMailServerDetails>(from dRows in dataSet.Tables["MailType"].AsEnumerable()
                                                                                               select (co.ConvertTableToListNew<dbmlMailServerDetails>(dRows)));

                    objreturn.objdbmlMailServerDetails = objdbmlMailServerDetails;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        //[HttpPost]
        public returndbmlTempLoginMaster GetSuperStockistLoginSessionDetails(string SuperStockist)
        {
            ObservableCollection<dbmlTempLoginMaster> ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>();
            returndbmlTempLoginMaster objreturn = new returndbmlTempLoginMaster();
            try
            {

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Security].[SuperStockistLoginSessionDetails]", Convert.ToInt32(SuperStockist));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "LoginMaster" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["LoginMaster"].Rows.Count > 0)
                {
                    ObjdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>(from dRows in dataSet.Tables["LoginMaster"].AsEnumerable()
                                                                                           select (co.ConvertTableToListNew<dbmlTempLoginMaster>(dRows)));

                    objreturn.objdbmlTempLoginMaster = ObjdbmlTempLoginMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        [HttpGet]
        public returndbmlPartyMaster PartyMasterGetAllByUserId(string UserId)
        {
            ObservableCollection<dbmlTempPartyMaster> objdbmlPartyMaster = new ObservableCollection<dbmlTempPartyMaster>();
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Master].[PartyMasterGetAll]", Convert.ToInt32(UserId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "PartyMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["PartyMaster"].Rows.Count > 0)
                {
                    objdbmlPartyMaster = new ObservableCollection<dbmlTempPartyMaster>(from dRows in dataSet.Tables["PartyMaster"].AsEnumerable()
                                                                                       select (co.ConvertTableToListNew<dbmlTempPartyMaster>(dRows)));

                    objreturn.objdbmlPartyMaster = objdbmlPartyMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        [HttpGet]
        public returndbmlTempItemMaster GetItemsbyPartyIDMultipleDPRate(string PartyID, string BuyerID, string InvoiceDate)
        {
            ObservableCollection<dbmlTempItemMaster> objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>();
            ObservableCollection<dbmlTempItemMasterDPRate> objdbmlTempItemMasterDPRate = new ObservableCollection<dbmlTempItemMasterDPRate>();
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetItemsbyPartyIDMultipleDPRate]", Convert.ToInt32(PartyID), Convert.ToInt32(BuyerID), DateTime.ParseExact(InvoiceDate, "yyyy-MM-dd", null));
                dbCommand.CommandTimeout = 80000000;
                //database.LoadDataSet(dbCommand, dataSet, "TempItemMaster");
                //if (dataSet.Tables.Count > 0 && dataSet.Tables["TempItemMaster"].Rows.Count > 0)
                //{
                //    objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>(from dRows in dataSet.Tables["TempItemMaster"].AsEnumerable()
                //                                                                         select (co.ConvertTableToListNew<dbmlTempItemMaster>(dRows)));

                //    objreturn.objdbmlTempItemMaster = objdbmlTempItemMaster;
                //    objreturn.objdbmlStatus.StatusId = 1;
                //    objreturn.objdbmlStatus.Status = "Success";
                //}
                //else
                //{
                //    objreturn.objdbmlStatus.StatusId = 2;
                //    objreturn.objdbmlStatus.Status = "Record not found";
                //}

                database.LoadDataSet(dbCommand, dataSet, new string[] { "TempItemMaster", "TempItemMasterDPRate" });
                if (dataSet.Tables.Count > 0 && dataSet.Tables["TempItemMaster"].Rows.Count > 0 && dataSet.Tables["TempItemMasterDPRate"].Rows.Count > 0)
                {
                    objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>(from dRows in dataSet.Tables["TempItemMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlTempItemMaster>(dRows)));

                    objdbmlTempItemMasterDPRate = new ObservableCollection<dbmlTempItemMasterDPRate>(from dRows in dataSet.Tables["TempItemMasterDPRate"].AsEnumerable()
                                                                                                     select (co.ConvertTableToListNew<dbmlTempItemMasterDPRate>(dRows)));

                    objreturn.objdbmlTempItemMaster = objdbmlTempItemMaster;
                    objreturn.objdbmlTempItemMasterDPRate = objdbmlTempItemMasterDPRate;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #region EInvoice

        [HttpGet]
        public returndbmlTempEInvoice GetDocumentListForEnvoice(string DocDate, string PartyId)
        {
            ObservableCollection<dbmlTempEInvoice> objdbmlTempEInvoice = new ObservableCollection<dbmlTempEInvoice>();
            returndbmlTempEInvoice objreturn = new returndbmlTempEInvoice();
            try
            {
                string FFDate = "";
                string TTDate = "";
                var FDate = DocDate.Split('/');
                FFDate = FDate[1] + "/" + FDate[0] + "/" + FDate[2];

                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetDocumentListForEnvoice]", Convert.ToInt32(PartyId), FFDate, "INV");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "EInvoice");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["EInvoice"].Rows.Count > 0)
                {
                    objdbmlTempEInvoice = new ObservableCollection<dbmlTempEInvoice>(from dRows in dataSet.Tables["EInvoice"].AsEnumerable()
                                                                                     select (co.ConvertTableToListNew<dbmlTempEInvoice>(dRows)));

                    objreturn.objdbmlTempEInvoice = objdbmlTempEInvoice;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        [HttpGet]
        public returndbmlTempAPIAuthDetails GetAPIAuthDetailsForEInv(string PartyId)
        {
            ObservableCollection<dbmlTempAPIAuthDetails> objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>();
            returndbmlTempAPIAuthDetails objreturn = new returndbmlTempAPIAuthDetails();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Settings].[GetAPIAuthDetailsForEInv]", Convert.ToInt32(PartyId));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "APIAuthDetails");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["APIAuthDetails"].Rows.Count > 0)
                {
                    objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>(from dRows in dataSet.Tables["APIAuthDetails"].AsEnumerable()
                                                                                                 select (co.ConvertTableToListNew<dbmlTempAPIAuthDetails>(dRows)));

                    objreturn.objdbmlTempAPIAuthDetails = objdbmlTempAPIAuthDetails;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        [HttpGet]
        public returndbmlTempAPIAuthDetails UpdateEInvAuthTokenDetails(string PartyId, string GSTNo, string AuthToken, string Apikey, string Sekey, string Expdate)
        {
            ObservableCollection<dbmlTempAPIAuthDetails> objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>();
            returndbmlTempAPIAuthDetails objreturn = new returndbmlTempAPIAuthDetails();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Settings].[SPUpdateEInvAuthTokenDetails]", Convert.ToInt32(PartyId), GSTNo, AuthToken, Apikey, Sekey, DateTime.ParseExact(Expdate, "yyyy-MM-dd HH:mm:ss", null));
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "APIAuthDetails");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["APIAuthDetails"].Rows.Count > 0)
                {
                    objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>(from dRows in dataSet.Tables["APIAuthDetails"].AsEnumerable()
                                                                                                 select (co.ConvertTableToListNew<dbmlTempAPIAuthDetails>(dRows)));

                    objreturn.objdbmlTempAPIAuthDetails = objdbmlTempAPIAuthDetails;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        [HttpGet]
        public ReqPlGenIRN GetIRNForEnvoice(string PartyId, int DSChallanId)
        {
            ReqPlGenIRN reqPlGenIRN = new ReqPlGenIRN();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetDataforEInvoice]", Convert.ToInt32(PartyId), DSChallanId, "INV");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "TransDtls", "DocDtls", "SellersDtl", "BuyerDtls", "DispDtls", "ShipDtls", "ItemList", "ValDtls", "PayDtls", "RefDtls", "AddDtls", "ExpDtls", "EwbDtls" });
                //if (dataSet.Tables.Count > 0 && dataSet.Tables["APIAuthDetails"].Rows.Count > 0)
                //{
                //    objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>(from dRows in dataSet.Tables["APIAuthDetails"].AsEnumerable()
                //                                                                                 select (co.ConvertTableToListNew<dbmlTempAPIAuthDetails>(dRows)));

                //    objreturn.objdbmlTempAPIAuthDetails = objdbmlTempAPIAuthDetails;
                //    objreturn.objdbmlStatus.StatusId = 1;
                //    objreturn.objdbmlStatus.Status = "Success";
                //}
                //else
                //{
                //    objreturn.objdbmlStatus.StatusId = 2;
                //    objreturn.objdbmlStatus.Status = "Record not found";
                //}
                


                if (dataSet.Tables.Count > 0)
                {
                    reqPlGenIRN.Version = "1.1";
                    // Start Trans Details
                    reqPlGenIRN.TranDtls = new ReqPlGenIRN.TranDetails();
                    reqPlGenIRN.TranDtls.TaxSch = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["TaxSch"].ToString()) ? null : dataSet.Tables[0].Rows[0]["TaxSch"].ToString();
                    reqPlGenIRN.TranDtls.SupTyp = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["SupTyp"].ToString()) ? null : dataSet.Tables[0].Rows[0]["SupTyp"].ToString();
                    reqPlGenIRN.TranDtls.RegRev = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["RegRev"].ToString()) ? null : dataSet.Tables[0].Rows[0]["RegRev"].ToString();
                    reqPlGenIRN.TranDtls.EcmGstin = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["EcmGstin"].ToString()) ? null : dataSet.Tables[0].Rows[0]["EcmGstin"].ToString();
                    reqPlGenIRN.TranDtls.IgstOnIntra = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["IgstOnIntra"].ToString()) ? null : dataSet.Tables[0].Rows[0]["IgstOnIntra"].ToString();

                    // Start Doc Details
                    reqPlGenIRN.DocDtls = new ReqPlGenIRN.DocSetails();
                    reqPlGenIRN.DocDtls.Typ = string.IsNullOrEmpty(dataSet.Tables[1].Rows[0]["Typ"].ToString()) ? null : dataSet.Tables[1].Rows[0]["Typ"].ToString();
                    reqPlGenIRN.DocDtls.No = string.IsNullOrEmpty(dataSet.Tables[1].Rows[0]["Nos"].ToString()) ? null : dataSet.Tables[1].Rows[0]["Nos"].ToString();
                    reqPlGenIRN.DocDtls.Dt = string.IsNullOrEmpty(dataSet.Tables[1].Rows[0]["Dt"].ToString()) ? null : dataSet.Tables[1].Rows[0]["Dt"].ToString();

                    // Start Seller Details
                    reqPlGenIRN.SellerDtls = new ReqPlGenIRN.SellerDetails();
                    reqPlGenIRN.SellerDtls.Gstin = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["Gstin"].ToString()) ? null : dataSet.Tables[2].Rows[0]["Gstin"].ToString();//"06AACCC1596Q002";//dataSet.Tables[2].Rows[0]["Gstin"].ToString();
                    reqPlGenIRN.SellerDtls.LglNm = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["LglNm"].ToString()) ? null : dataSet.Tables[2].Rows[0]["LglNm"].ToString();
                    reqPlGenIRN.SellerDtls.TrdNm = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["TrdNm"].ToString()) ? null : dataSet.Tables[2].Rows[0]["TrdNm"].ToString(); ;
                    reqPlGenIRN.SellerDtls.Addr1 = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["Addr1"].ToString()) ? null : dataSet.Tables[2].Rows[0]["Addr1"].ToString();
                    reqPlGenIRN.SellerDtls.Addr2 = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["Addr2"].ToString()) ? null : dataSet.Tables[2].Rows[0]["Addr2"].ToString(); ;
                    reqPlGenIRN.SellerDtls.Loc = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["Loc"].ToString()) ? null : dataSet.Tables[2].Rows[0]["Loc"].ToString();
                    reqPlGenIRN.SellerDtls.Pin = Convert.ToInt32(dataSet.Tables[2].Rows[0]["Pin"].ToString());
                    reqPlGenIRN.SellerDtls.Stcd = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["StCd"].ToString()) ? null : dataSet.Tables[2].Rows[0]["StCd"].ToString(); ;//dataSet.Tables[2].Rows[0]["StCd"].ToString();
                    reqPlGenIRN.SellerDtls.Ph = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["Ph"].ToString()) ? null : dataSet.Tables[2].Rows[0]["Ph"].ToString();
                    reqPlGenIRN.SellerDtls.Em = string.IsNullOrEmpty(dataSet.Tables[2].Rows[0]["EM"].ToString()) ? null : dataSet.Tables[2].Rows[0]["EM"].ToString();

                    //Start Buyer Details
                    reqPlGenIRN.BuyerDtls = new ReqPlGenIRN.BuyerDetails();
                    reqPlGenIRN.BuyerDtls.Gstin = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["ClientGST"].ToString()) ? null : dataSet.Tables[3].Rows[0]["ClientGST"].ToString();
                    reqPlGenIRN.BuyerDtls.LglNm = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["Client"].ToString()) ? null : dataSet.Tables[3].Rows[0]["Client"].ToString();
                    reqPlGenIRN.BuyerDtls.TrdNm = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["TrdNm"].ToString()) ? null : dataSet.Tables[3].Rows[0]["TrdNm"].ToString();
                    reqPlGenIRN.BuyerDtls.Pos = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["pos"].ToString()) ? null : dataSet.Tables[3].Rows[0]["pos"].ToString();
                    reqPlGenIRN.BuyerDtls.Addr1 = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["Addr1"].ToString()) ? null : dataSet.Tables[3].Rows[0]["Addr1"].ToString();
                    reqPlGenIRN.BuyerDtls.Addr2 = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["Addr2"].ToString()) ? null : dataSet.Tables[3].Rows[0]["Addr2"].ToString();
                    reqPlGenIRN.BuyerDtls.Loc = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["InvoiceCity"].ToString()) ? null : dataSet.Tables[3].Rows[0]["InvoiceCity"].ToString();
                    reqPlGenIRN.BuyerDtls.Pin = Convert.ToInt32(dataSet.Tables[3].Rows[0]["InvoicePin"].ToString());
                    reqPlGenIRN.BuyerDtls.Stcd = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["stcd"].ToString()) ? null : dataSet.Tables[3].Rows[0]["stcd"].ToString();
                    reqPlGenIRN.BuyerDtls.Ph = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["ph"].ToString()) ? null : dataSet.Tables[3].Rows[0]["ph"].ToString();
                    reqPlGenIRN.BuyerDtls.Em = string.IsNullOrEmpty(dataSet.Tables[3].Rows[0]["em"].ToString()) ? null : dataSet.Tables[3].Rows[0]["em"].ToString();

                    //Start DispDetails

                    if (Convert.ToInt32(dataSet.Tables[4].Rows[0]["isDispatcherDetails"].ToString()) == 1)
                    {
                        reqPlGenIRN.DispDtls = new ReqPlGenIRN.DispatchedDetails();
                        reqPlGenIRN.DispDtls.Nm = string.IsNullOrEmpty(dataSet.Tables[4].Rows[0]["Nm"].ToString()) ? null : dataSet.Tables[4].Rows[0]["Nm"].ToString();
                        reqPlGenIRN.DispDtls.Addr1 = string.IsNullOrEmpty(dataSet.Tables[4].Rows[0]["Addr1"].ToString()) ? null : dataSet.Tables[4].Rows[0]["Addr1"].ToString();
                        reqPlGenIRN.DispDtls.Addr2 = string.IsNullOrEmpty(dataSet.Tables[4].Rows[0]["Addr2"].ToString()) ? null : dataSet.Tables[4].Rows[0]["Addr2"].ToString();
                        reqPlGenIRN.DispDtls.Loc = string.IsNullOrEmpty(dataSet.Tables[4].Rows[0]["Loc"].ToString()) ? null : dataSet.Tables[4].Rows[0]["Loc"].ToString();
                        reqPlGenIRN.DispDtls.Pin = Convert.ToInt32(dataSet.Tables[4].Rows[0]["Pin"].ToString());
                        reqPlGenIRN.DispDtls.Stcd = string.IsNullOrEmpty(dataSet.Tables[4].Rows[0]["Stcd"].ToString()) ? null : dataSet.Tables[4].Rows[0]["Stcd"].ToString();
                    }
                    else
                    {
                        reqPlGenIRN.DispDtls = null;
                    }
                    //reqPlGenIRN.DispDtls = null;
                    //Start Shipped details
                    if (Convert.ToInt32(dataSet.Tables[5].Rows[0]["ShipDetailsRequired"].ToString()) == 1)
                    {
                        reqPlGenIRN.ShipDtls = new ReqPlGenIRN.ShippedDetails();
                        reqPlGenIRN.ShipDtls.Gstin = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["ConsigneeGST"].ToString()) ? null : dataSet.Tables[5].Rows[0]["ConsigneeGST"].ToString();
                        reqPlGenIRN.ShipDtls.LglNm = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["LglNm"].ToString()) ? null : dataSet.Tables[5].Rows[0]["LglNm"].ToString();
                        reqPlGenIRN.ShipDtls.TrdNm = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["TrdNm"].ToString()) ? null : dataSet.Tables[5].Rows[0]["TrdNm"].ToString();
                        reqPlGenIRN.ShipDtls.Addr1 = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["Addr1"].ToString()) ? null : dataSet.Tables[5].Rows[0]["Addr1"].ToString();
                        reqPlGenIRN.ShipDtls.Addr2 = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["Addr2"].ToString()) ? null : dataSet.Tables[5].Rows[0]["Addr2"].ToString();
                        reqPlGenIRN.ShipDtls.Loc = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["ShippingCity"].ToString()) ? null : dataSet.Tables[5].Rows[0]["ShippingCity"].ToString();
                        reqPlGenIRN.ShipDtls.Pin = Convert.ToInt32(dataSet.Tables[5].Rows[0]["ShippingPin"].ToString());
                        reqPlGenIRN.ShipDtls.Stcd = string.IsNullOrEmpty(dataSet.Tables[5].Rows[0]["stcd"].ToString()) ? null : dataSet.Tables[5].Rows[0]["stcd"].ToString();
                    }
                    else
                        reqPlGenIRN.ShipDtls = null;
                    //Start ItemList details
                    reqPlGenIRN.ItemList = new List<ReqPlGenIRN.ItmList>();

                    foreach (DataRow itemrow in dataSet.Tables[6].Rows)
                    {
                        ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
                        itm.SlNo = (itemrow["SlNo"].ToString());
                        itm.IsServc = string.IsNullOrEmpty(itemrow["IsServc"].ToString()) ? null : itemrow["IsServc"].ToString();
                        itm.PrdDesc = string.IsNullOrEmpty(itemrow["PrdDesc"].ToString()) ? null : itemrow["PrdDesc"].ToString();
                        itm.HsnCd = string.IsNullOrEmpty(itemrow["HsnCd"].ToString()) ? null : itemrow["HsnCd"].ToString();
                        itm.Barcde = string.IsNullOrEmpty(itemrow["BarCode"].ToString()) ? null : itemrow["BarCode"].ToString();
                        itm.BchDtls = null;
                        itm.Qty = Math.Round(Convert.ToDouble(itemrow["Qty"].ToString()), 3);
                        itm.FreeQty = Math.Round(Convert.ToDouble(itemrow["FreeQty"].ToString()), 3);
                        itm.Unit = string.IsNullOrEmpty(itemrow["Unit"].ToString()) ? null : itemrow["Unit"].ToString();
                        itm.UnitPrice = Math.Round(Convert.ToDouble(itemrow["UnitPrice"].ToString()), 2); //10.00;
                        itm.TotAmt = Math.Round(Convert.ToDouble(itemrow["TotAmt"].ToString()), 2);//10.00;
                        itm.Discount = Math.Round(Convert.ToDouble(itemrow["Discount"].ToString()), 2); //0.0;
                        itm.PreTaxVal = Math.Round(Convert.ToDouble(itemrow["PreTaxVal"].ToString()), 2); //1;
                        itm.AssAmt = Math.Round(Convert.ToDouble(itemrow["AssAmt"].ToString()), 2);//10.00;
                        itm.GstRt = Math.Round(Convert.ToDouble(itemrow["GstRt"].ToString()), 2);//10.00;
                        itm.SgstAmt = Math.Round(Convert.ToDouble(itemrow["SGSTAmt"].ToString()), 2);//0.0;
                        itm.IgstAmt = Math.Round(Convert.ToDouble(itemrow["IGSTAmt"].ToString()), 2);// 0.0;
                        itm.CgstAmt = Math.Round(Convert.ToDouble(itemrow["CgstAmt"].ToString()), 2);//0.0;
                        itm.CesRt = Math.Round(Convert.ToDouble(itemrow["CesRt"].ToString()), 2);//0.0;
                        itm.CesAmt = Math.Round(Convert.ToDouble(itemrow["CesAmt"].ToString()), 2);//0.0;
                        itm.CesNonAdvlAmt = Math.Round(Convert.ToDouble(itemrow["CesNonAdvlAmt"].ToString()), 2);//0.0;
                        itm.StateCesRt = Math.Round(Convert.ToDouble(itemrow["StateCesRt"].ToString()), 2);//0.0;
                        itm.StateCesAmt = Math.Round(Convert.ToDouble(itemrow["StateCesAmt"].ToString()), 2);//0.0;
                        itm.StateCesNonAdvlAmt = Math.Round(Convert.ToDouble(itemrow["StateCesNonAdvlAmt"].ToString()), 2);//0.0;
                        itm.OthChrg = Math.Round(Convert.ToDouble(itemrow["OthChrg"].ToString()), 2);//0.0;
                        itm.TotItemVal = Math.Round(Convert.ToDouble(itemrow["TotItemVal"].ToString()), 2);//10.00;
                        itm.OrdLineRef = string.IsNullOrEmpty(itemrow["OrdLineRef"].ToString()) ? null : itemrow["OrdLineRef"].ToString();
                        itm.OrgCntry = string.IsNullOrEmpty(itemrow["OrgCntry"].ToString()) ? null : itemrow["OrgCntry"].ToString();
                        itm.PrdSlNo = string.IsNullOrEmpty(itemrow["PrdSlNo"].ToString()) ? null : itemrow["PrdSlNo"].ToString();
                        itm.AttribDtls = null;
                        reqPlGenIRN.ItemList.Add(itm);
                    }


                    //Start Value details
                    reqPlGenIRN.ValDtls = new ReqPlGenIRN.ValDetails();
                    reqPlGenIRN.ValDtls.AssVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["AssVal"].ToString()), 2); ;
                    reqPlGenIRN.ValDtls.CgstVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["CgstVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.SgstVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["SgstVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.IgstVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["IgstVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.CesVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["CesVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.StCesVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["StCesVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.Discount = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["Discount"].ToString()), 2);
                    reqPlGenIRN.ValDtls.OthChrg = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["OthChrg"].ToString()), 2);
                    reqPlGenIRN.ValDtls.RndOffAmt = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["RndOffAmt"].ToString()), 2);
                    reqPlGenIRN.ValDtls.TotInvVal = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["TotInvVal"].ToString()), 2);
                    reqPlGenIRN.ValDtls.TotInvValFc = Math.Round(Convert.ToDouble(dataSet.Tables[7].Rows[0]["TotInvValFc"].ToString()), 2);

                    reqPlGenIRN.PayDtls = null;
                    reqPlGenIRN.RefDtls = null;
                    reqPlGenIRN.AddlDocDtls = null;
                    //reqPlGenIRN.ExpDtls = null;

                    //Start Export Details
                    if (Convert.ToInt32(dataSet.Tables[11].Rows[0]["IsExportDetails"].ToString()) == 1)
                    {
                        reqPlGenIRN.ExpDtls = new ReqPlGenIRN.ExpDetails();
                        reqPlGenIRN.ExpDtls.ShipBNo = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["ShipBNo"].ToString()) ? null : dataSet.Tables[11].Rows[0]["ShipBNo"].ToString();
                        reqPlGenIRN.ExpDtls.ShipBDt = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["ShipBDt"].ToString()) ? null : dataSet.Tables[11].Rows[0]["ShipBDt"].ToString();
                        reqPlGenIRN.ExpDtls.Port = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["PortCode"].ToString()) ? null : dataSet.Tables[11].Rows[0]["PortCode"].ToString();
                        reqPlGenIRN.ExpDtls.RefClm = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["RefClm"].ToString()) ? null : dataSet.Tables[11].Rows[0]["RefClm"].ToString();
                        reqPlGenIRN.ExpDtls.ForCur = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["ForCur"].ToString()) ? null : dataSet.Tables[11].Rows[0]["ForCur"].ToString().ToUpper();
                        reqPlGenIRN.ExpDtls.CntCode = string.IsNullOrEmpty(dataSet.Tables[11].Rows[0]["CntCode"].ToString()) ? null : dataSet.Tables[11].Rows[0]["CntCode"].ToString().ToUpper();
                    }
                    else
                        reqPlGenIRN.ExpDtls = null;

                    reqPlGenIRN.EwbDtls = null;

                    //ReqPlGenIRN.StatusId = 1;
                    //ReqPlGenIRN.Status = "Success";
                    //string ReqJson = JsonConvert.SerializeObject(reqPlGenIRN);
                    return reqPlGenIRN;
                }
                else
                {
                   // ReqPlGenIRN.StatusId = 99;
                    //ReqPlGenIRN.Status = "Record Not Found";
                    //string ReqJson = JsonConvert.SerializeObject(reqPlGenIRN);
                    return reqPlGenIRN;
                }
            }
            catch (Exception ex)
            {
               // ReqPlGenIRN.StatusId = 99;
               // ReqPlGenIRN.Status = ex.Message + " " + ex.StackTrace;
                //string ReqJson = JsonConvert.SerializeObject(reqPlGenIRN);
                return reqPlGenIRN;
            }
        }


        [HttpPost]
        public RespPlGenIRN UpdateEINVIRNDetails(QrCodeModel qr)
        {
            RespPlGenIRN objreturn = new RespPlGenIRN();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intOut = 0;
                string OutMess = "";

                dbCmd = db.GetStoredProcCommand("[Trans].[SPUpdateEINVDetails]");
                db.AddInParameter(dbCmd, "DocumentID", DbType.Int32, qr.DSCId);
                db.AddInParameter(dbCmd, "AckNo", DbType.String, qr.AckNo);
                db.AddInParameter(dbCmd, "AckDt", DbType.String, qr.AckDt);
                db.AddInParameter(dbCmd, "IRNNo", DbType.String, qr.Irn);
                db.AddInParameter(dbCmd, "QRCodeText", DbType.String, qr.SignQRCode);
                db.AddInParameter(dbCmd, "image", DbType.Binary, qr.vByteimage);
                db.AddInParameter(dbCmd, "DocType", DbType.String, qr.DocType);
                db.AddInParameter(dbCmd, "QRCodeBase64Text", DbType.String, qr.QrCodeBase64String.ToString());
                db.AddOutParameter(dbCmd, "pErrNo", DbType.Int32, 0);
                db.AddOutParameter(dbCmd, "pErrMsg", DbType.String,4500);
                db.ExecuteNonQuery(dbCmd, dbTrans);

                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@pErrNo"));
                OutMess = db.GetParameterValue(dbCmd, "@pErrMsg").ToString();

                dbTrans.Commit();
                objreturn.StatusId = 1;
                objreturn.Status = Convert.ToString(OutMess);

                dbCmd.Dispose();
                db = null;
                //DataSet dataSet = new DataSet();
                //Database database = new SqlDatabase(co.StrSetConnection());

                //DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[SPUpdateEINVDetails]", Convert.ToInt32(DSCId), AckNo, AckDt, Irn, SignQRCode, vByteimage, DocType, QRCodeBase64Text);
                //dbCommand.CommandTimeout = 80000000;
                //database.LoadDataSet(dbCommand, dataSet, "APIAuthDetails");
                //if (dataSet.Tables.Count > 0 && dataSet.Tables["APIAuthDetails"].Rows.Count > 0)
                //{
                //    objreturn.StatusId = 1;
                //    objreturn.Status = "Success";
                //}
                //else
                //{
                //    objreturn.StatusId = 2;
                //    objreturn.Status = "Record not found";
                //}
                //dataSet.Dispose();
                //dbCommand.Dispose();
                //database = null;
            }
            catch (Exception ex)
            {
                objreturn.StatusId = 99;
                objreturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objreturn;
        }


        [HttpGet]
        public ReqPlGenEwbByIRN GetEWBForEnvoice(string PartyId, int DSChallanId, string IRNNo)
        {
            ReqPlGenEwbByIRN reqPlGenEwbByIRN = new ReqPlGenEwbByIRN();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());

                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetEwayBillJSonData]", DSChallanId, "INV");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, new string[] { "TransDtls" });

                if (dataSet.Tables.Count > 0)
                {
                    reqPlGenEwbByIRN.Irn = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["IRNNo"].ToString()) ? null : dataSet.Tables[0].Rows[0]["IRNNo"].ToString();
                    reqPlGenEwbByIRN.TransId = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["transporterId"].ToString()) ? null : dataSet.Tables[0].Rows[0]["transporterId"].ToString();
                    reqPlGenEwbByIRN.TransMode = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["transMode"].ToString()) ? null : dataSet.Tables[0].Rows[0]["transMode"].ToString();
                    reqPlGenEwbByIRN.TransDocNo = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["transDocNo"].ToString()) ? null : dataSet.Tables[0].Rows[0]["transDocNo"].ToString();
                    reqPlGenEwbByIRN.TransDocDt = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["transDocDate"].ToString()) ? null : dataSet.Tables[0].Rows[0]["transDocDate"].ToString();
                    reqPlGenEwbByIRN.VehNo = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["vehicleNo"].ToString()) ? null : dataSet.Tables[0].Rows[0]["vehicleNo"].ToString();
                    reqPlGenEwbByIRN.Distance = Convert.ToInt32(dataSet.Tables[0].Rows[0]["transDistance"].ToString());
                    reqPlGenEwbByIRN.VehType = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["vehicleType"].ToString()) ? null : dataSet.Tables[0].Rows[0]["vehicleType"].ToString();
                    reqPlGenEwbByIRN.TransName = string.IsNullOrEmpty(dataSet.Tables[0].Rows[0]["transporterName"].ToString()) ? null : dataSet.Tables[0].Rows[0]["transporterName"].ToString();
                    
                    reqPlGenEwbByIRN.ExpShipDtls = null;
                    reqPlGenEwbByIRN.DispDtls = null;

                    reqPlGenEwbByIRN.StatusId = 1;
                    reqPlGenEwbByIRN.Status = "Success";
                    string ReqJson = JsonConvert.SerializeObject(reqPlGenEwbByIRN);
                    return reqPlGenEwbByIRN;
                }
                else
                {
                    reqPlGenEwbByIRN.StatusId = 99;
                    reqPlGenEwbByIRN.Status = "Record Not Found";
                    string ReqJson = JsonConvert.SerializeObject(reqPlGenEwbByIRN);
                    return reqPlGenEwbByIRN;
                }
            }
            catch (Exception ex)
            {
                reqPlGenEwbByIRN.StatusId = 99;
                reqPlGenEwbByIRN.Status = ex.Message + " " + ex.StackTrace;
                string ReqJson = JsonConvert.SerializeObject(reqPlGenEwbByIRN);
                return reqPlGenEwbByIRN;
            }
        }



        [HttpPost]
        public RespPlGetEWBByIRN UpdateEINVEWBDetails(RespPlGetEWBByIRN objEWB)
        {
            RespPlGetEWBByIRN objreturn = new RespPlGetEWBByIRN();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intOut = 0;
                string OutMess = "";

                dbCmd = db.GetStoredProcCommand("[Trans].[SPUpdateEwayBillDetailsByIRN]");
                db.AddInParameter(dbCmd, "DocumentID", DbType.Int32, objEWB.DSCId);
                db.AddInParameter(dbCmd, "EwayBillno", DbType.String, objEWB.EwbNo);
                db.AddInParameter(dbCmd, "EwayBillDate", DbType.String, objEWB.EwbDt);
                db.AddInParameter(dbCmd, "Validupto", DbType.String, objEWB.EwbValidTill);
                db.AddInParameter(dbCmd, "DocType", DbType.String, objEWB.DocType);
                db.AddOutParameter(dbCmd, "pErrNo", DbType.Int32, 0);
                db.AddOutParameter(dbCmd, "pErrMsg", DbType.String, 4500);
                db.ExecuteNonQuery(dbCmd, dbTrans);

                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@pErrNo"));
                OutMess = db.GetParameterValue(dbCmd, "@pErrMsg").ToString();

                dbTrans.Commit();
                objreturn.StatusId = 1;
                objreturn.Status = Convert.ToString(OutMess);

                dbCmd.Dispose();
                db = null;
            }
            catch (Exception ex)
            {
                objreturn.StatusId = 99;
                objreturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objreturn;
        }

        [HttpPost]
        public RespPlCancelIRN UpdateIRNOREwayCancellationDetails(RespPlCancelIRN objINR)
        {
            RespPlCancelIRN objreturn = new RespPlCancelIRN();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intOut = 0;
                string OutMess = "";

                dbCmd = db.GetStoredProcCommand("[Trans].[SPUpdateIRNOREwayCancellationDetails]");
                db.AddInParameter(dbCmd, "pType", DbType.String, "IRN");
                db.AddInParameter(dbCmd, "pDocumentID", DbType.Int32, objINR.DSCId);
                db.AddInParameter(dbCmd, "pCancellationDt", DbType.String, objINR.CancelDate);
                db.AddInParameter(dbCmd, "pUserId", DbType.String, objINR.UserId);
                db.AddInParameter(dbCmd, "DocType", DbType.String, objINR.DocType);
                db.AddInParameter(dbCmd, "CancellationReason", DbType.String, objINR.CancelReason);
                db.AddOutParameter(dbCmd, "pErrNo", DbType.Int32, 0);
                db.AddOutParameter(dbCmd, "pErrMsg", DbType.String, 500);
                db.ExecuteNonQuery(dbCmd, dbTrans);

                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@pErrNo"));
                OutMess = db.GetParameterValue(dbCmd, "@pErrMsg").ToString();

                dbTrans.Commit();
                objreturn.StatusId = 1;
                objreturn.Status = Convert.ToString(OutMess);

                dbCmd.Dispose();
                db = null;
            }
            catch (Exception ex)
            {
                objreturn.StatusId = 99;
                objreturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objreturn;
        }

        [HttpPost]
        public RespPlCancelEWB UpdateIRNOREwayCancellationDetailsEWB(RespPlCancelEWB objEWB)
        {
            RespPlCancelEWB objreturn = new RespPlCancelEWB();
            DbConnection dbCon = null;
            DbTransaction dbTrans = null;
            try
            {
                Database db = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCmd = null;
                dbCon = db.CreateConnection();
                dbCon.Open();
                dbTrans = dbCon.BeginTransaction();
                int intOut = 0;
                string OutMess = "";

                dbCmd = db.GetStoredProcCommand("[Trans].[SPUpdateIRNOREwayCancellationDetails]");
                db.AddInParameter(dbCmd, "pType", DbType.String, "EWB");
                db.AddInParameter(dbCmd, "pDocumentID", DbType.Int32, objEWB.DSCId);
                db.AddInParameter(dbCmd, "pCancellationDt", DbType.String, objEWB.cancelDate);
                db.AddInParameter(dbCmd, "pUserId", DbType.String, objEWB.UserId);
                db.AddInParameter(dbCmd, "DocType", DbType.String, objEWB.DocType);
                db.AddInParameter(dbCmd, "CancellationReason", DbType.String, objEWB.CancelReason);
                db.AddOutParameter(dbCmd, "pErrNo", DbType.Int32, 0);
                db.AddOutParameter(dbCmd, "pErrMsg", DbType.String, 500);
                db.ExecuteNonQuery(dbCmd, dbTrans);

                intOut = Convert.ToInt32(db.GetParameterValue(dbCmd, "@pErrNo"));
                OutMess = db.GetParameterValue(dbCmd, "@pErrMsg").ToString();

                dbTrans.Commit();
                objreturn.StatusId = 1;
                objreturn.Status = Convert.ToString(OutMess);

                dbCmd.Dispose();
                db = null;
            }
            catch (Exception ex)
            {
                objreturn.StatusId = 99;
                objreturn.Status = ex.Message + " " + ex.StackTrace;
            }
            finally
            {
                if (dbCon != null && dbCon.State == ConnectionState.Open)
                {
                    dbCon.Close();
                    dbCon.Dispose();
                }
            }
            return objreturn;
        }

        #endregion

        #region Purchase Return

        [HttpGet]
        public returndbmlCustomerMaster GetPurchaseReturnParty()
        {
            ObservableCollection<dbmlCustomerMaster> objdbmlCustomerMaster = new ObservableCollection<dbmlCustomerMaster>();
            returndbmlCustomerMaster objreturn = new returndbmlCustomerMaster();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetPurchaseReturnParty]");
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "CustomerMaster");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["CustomerMaster"].Rows.Count > 0)
                {
                    objdbmlCustomerMaster = new ObservableCollection<dbmlCustomerMaster>(from dRows in dataSet.Tables["CustomerMaster"].AsEnumerable()
                                                                                         select (co.ConvertTableToListNew<dbmlCustomerMaster>(dRows)));

                    objreturn.objdbmlCustomerMaster = objdbmlCustomerMaster;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }


        [HttpGet]
        public returnDealerSalesChallanGetByFromDateToDateResult DealerSalesChallanPurchaseReturnGetByFromDateToDate(string PartyID, string FromDate, string ToDate, string InvoiceNo)
        {
            ObservableCollection<DealerSalesChallanGetByFromDateToDateResult> objDealerSalesChallanGetByFromDateToDateResult = new ObservableCollection<DealerSalesChallanGetByFromDateToDateResult>();
            returnDealerSalesChallanGetByFromDateToDateResult objreturn = new returnDealerSalesChallanGetByFromDateToDateResult();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[DealerSalesChallanPurchaseReturnGetByFromDateToDate]", Convert.ToInt32(PartyID), DateTime.ParseExact(FromDate, "MM/dd/yyyy", null), DateTime.ParseExact(ToDate, "MM/dd/yyyy", null), InvoiceNo);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "DealerSalesChallanGetByFromDateToDate");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["DealerSalesChallanGetByFromDateToDate"].Rows.Count > 0)
                {
                    objDealerSalesChallanGetByFromDateToDateResult = new ObservableCollection<DealerSalesChallanGetByFromDateToDateResult>(from dRows in dataSet.Tables["DealerSalesChallanGetByFromDateToDate"].AsEnumerable()
                                                                                                                                           select (co.ConvertTableToListNew<DealerSalesChallanGetByFromDateToDateResult>(dRows)));

                    objreturn.objDealerSalesChallanGetByFromDateToDateResult = objDealerSalesChallanGetByFromDateToDateResult;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }
                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;
        }

        #endregion


        #region PartyProductWiseSummarySalesReport
        [HttpGet]
        public requestdbmlSalesReport PartyProductWiseSummarySalesReportFromDateToDateReport(string SuperStockistID, string Fromdate, string Todate, int CustomerID, int ReportType, string SubCategoryId, string ItemId)
        {
            ObservableCollection<dbmlSalesReport> objdbmlSalesReport = new ObservableCollection<dbmlSalesReport>();
            requestdbmlSalesReport objreturn = new requestdbmlSalesReport();
            try
            {
                DataSet dataSet = new DataSet();
                Database database = new SqlDatabase(co.StrSetConnection());
                DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[GetSalesReportNew]", SuperStockistID, Fromdate, Todate, CustomerID, ReportType, SubCategoryId, ItemId);
                dbCommand.CommandTimeout = 80000000;
                database.LoadDataSet(dbCommand, dataSet, "item");
                if (dataSet.Tables.Count > 0 && dataSet.Tables["Item"].Rows.Count > 0)
                {
                    objdbmlSalesReport = new ObservableCollection<dbmlSalesReport>(from dRows in dataSet.Tables["item"].AsEnumerable()
                                                                                   select (co.ConvertTableToListNew<dbmlSalesReport>(dRows)));

                    objreturn.objdbmlSalesReport = objdbmlSalesReport;
                    objreturn.objdbmlStatus.StatusId = 1;
                    objreturn.objdbmlStatus.Status = "Success";
                }
                else
                {
                    objreturn.objdbmlStatus.StatusId = 2;
                    objreturn.objdbmlStatus.Status = "Record not found";
                }


                dataSet.Dispose();
                dbCommand.Dispose();
                database = null;
            }
            catch (Exception ex)
            {
                objreturn.objdbmlStatus.StatusId = 99;
                objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
            }
            return objreturn;

        }

        #endregion

    }
}

//[HttpGet]
//public returndbmlTempOrderDetail OrderDetailGetByOrderNo(string OrderNo)
//{
//    ObservableCollection<dbmlTempOrderDetail> objdbmlTempOrderDetail = new ObservableCollection<dbmlTempOrderDetail>();
//    returndbmlTempOrderDetail objreturn = new returndbmlTempOrderDetail();
//    try
//    {
//        DataSet dataSet = new DataSet();
//        Database database = new SqlDatabase(co.StrSetConnection());
//        DbCommand dbCommand = database.GetStoredProcCommand("[Trans].[OrderDetailGetByOrderNo]", OrderNo);
//        dbCommand.CommandTimeout = 80000000;
//        database.LoadDataSet(dbCommand, dataSet, "OrderDetail");
//        if (dataSet.Tables.Count > 0 && dataSet.Tables["OrderDetail"].Rows.Count > 0)
//        {
//            objdbmlTempOrderDetail = new ObservableCollection<dbmlTempOrderDetail>(from dRows in dataSet.Tables["OrderDetail"].AsEnumerable()
//                                                                                   select (co.ConvertTableToListNew<dbmlTempOrderDetail>(dRows)));

//            objreturn.objdbmlTempOrderDetail = objdbmlTempOrderDetail;
//            objreturn.objdbmlStatus.StatusId = 1;
//            objreturn.objdbmlStatus.Status = "Success";
//        }
//        else
//        {
//            objreturn.objdbmlStatus.StatusId = 2;
//            objreturn.objdbmlStatus.Status = "Record not found";
//        }
//        dataSet.Dispose();
//        dbCommand.Dispose();
//        database = null;
//    }
//    catch (Exception ex)
//    {
//        objreturn.objdbmlStatus.StatusId = 99;
//        objreturn.objdbmlStatus.Status = ex.Message + " " + ex.StackTrace;
//    }
//    return objreturn;
//}