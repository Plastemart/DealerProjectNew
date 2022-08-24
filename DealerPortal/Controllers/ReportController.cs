using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DealerPortal.Common;
using DealerPortal.Models;

namespace DealerPortal.Controllers
{
    public class ReportController : Controller
    {
        #region Common
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #endregion

        #region  Stock Ledger Report
        public ActionResult StockLedgerReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        [ValidateAntiForgeryToken]
        public ActionResult ItemMasterGetByRetailCategoryId(int RetailCategoryId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemMasterGetByRetailCategoryId?RetailCategoryId=" + RetailCategoryId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlItemMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        public ActionResult ItemLedgerGetByItemIdFromDateToDate(int SubCategoryId, int ItemId, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlItemLedgerReport objreturn = new requestdbmlItemLedgerReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemLedgerGetByItemIdFromDateToDate?SubCategoryId=" + SubCategoryId + "&ItemId=" + ItemId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlItemLedgerReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlItemLedgerReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ItemLedgerDetailGetByItemIdFromDateToDate(int ItemId, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlitemLedgerDetails objreturn = new requestdbmlitemLedgerDetails();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;
                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemLedgerDetailGetByItemIdFromDateToDate?ItemId=" + ItemId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlitemLedgerDetails>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlitemLedgerDetails, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Inventory Report
        public ActionResult InventoryReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        //[ValidateAntiForgeryToken]
        public ActionResult InventoryFromDateToDateReport(int SubCategoryId, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlItemLedgerReport objreturn = new requestdbmlItemLedgerReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;

                if (Session["UserTypeId"].ToString() == "22")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "InventoryFromDateToDateReport?SubCategoryId=" + SubCategoryId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlItemLedgerReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlItemLedgerReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  PartyProductWiseSummary Report
        public ActionResult PartyProductWiseSummary()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            //returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            //string responseBody = "";
            string responseBody2 = "";

            //var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            //responseBody = response1.Content.ReadAsStringAsync().Result;
            //objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            //ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result; 
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        //[ValidateAntiForgeryToken]
        public ActionResult PartyProductWiseSummaryFromDateToDateReport(string SuperStockistID, string Fromdate, string Todate, int CustomerID, int ReportType)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlSalesReport objreturn = new requestdbmlSalesReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string PartyIdVal = "0";

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = SuperStockistID;
                else
                    //PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                    PartyIdVal = Session["PartyId"].ToString();

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyProductWiseSummaryFromDateToDateReport?Fromdate=" + Fromdate + "&Todate=" + Todate + "&SuperStockistID=" + PartyIdVal + "&CustomerID=" + CustomerID + "&ReportType=" + ReportType).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlSalesReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlSalesReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Download CSV Report
        public ActionResult DownloadReports()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            //var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            //responseBody = response1.Content.ReadAsStringAsync().Result;
            //objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            //ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        //[ValidateAntiForgeryToken]
        public ActionResult DownloadFromDateToDateReport(int DowloadReportType, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlDownloadInvoiceReport objreturn = new requestdbmlDownloadInvoiceReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;

                if (Session["UserTypeId"].ToString() == "22")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "DownloadFromDateToDateReport?DowloadReportType=" + DowloadReportType + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlDownloadInvoiceReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlDownloadInvoiceReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Pending web order
        public ActionResult PendingWebOrderStatusReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        [ValidateAntiForgeryToken]
        public ActionResult PendingWebOrderStatusReportFromDateToDate(int SubCategoryId, int ItemId, string Fromdate, string Todate, int PartyId, int DateTypeFilter)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmPendingWebOrderStatusReport objreturn = new returndbmPendingWebOrderStatusReport();
            try
            {
                string apiUrlDP = System.Configuration.ConfigurationManager.AppSettings["apiUrlDP"];
                int PartyIdVal = 0;

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrlDP + "PendingWebOrderStatusReportFromDateToDate?SubCategoryId=" + SubCategoryId + "&ItemId=" + ItemId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal + "&DateTypeFilter=" + DateTypeFilter).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmPendingWebOrderStatusReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlPendingWebOrderStatusReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region  Stock Ledger Valuation Report
        public ActionResult StockReportValuation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }



        [ValidateAntiForgeryToken]
        public ActionResult GetStockReportValuation(int SubCategoryId, int ItemId, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlItemLedgerValuationReport objreturn = new requestdbmlItemLedgerValuationReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetStockReportValuation?SubCategoryId=" + SubCategoryId + "&ItemId=" + ItemId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlItemLedgerValuationReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlItemLedgerValuationReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GetStockReportDetailValuation(int ItemId, string Fromdate, string Todate, int PartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlitemLedgerDetails objreturn = new requestdbmlitemLedgerDetails();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                int PartyIdVal = 0;
                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = PartyId;
                else
                    PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetStockReportDetailValuation?ItemId=" + ItemId + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&PartyId=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<requestdbmlitemLedgerDetails>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlitemLedgerDetails, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region  PartyProductWiseSummarySalesReport
        public ActionResult PartyProductWiseSummarySalesReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        //[ValidateAntiForgeryToken]
        public ActionResult PartyProductWiseSummarySalesReportFromDateToDateReport(string SuperStockistID, string Fromdate, string Todate, int CustomerID, int ReportType, string SubCategoryId, string ItemId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlSalesReport objreturn = new requestdbmlSalesReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string PartyIdVal = "0";

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = SuperStockistID;
                else
                    //PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                    PartyIdVal = Session["PartyId"].ToString();

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyProductWiseSummarySalesReportFromDateToDateReport?Fromdate=" + Fromdate + "&Todate=" + Todate + "&SuperStockistID=" + PartyIdVal + "&CustomerID=" + CustomerID + "&ReportType=" + ReportType + "&SubCategoryId=" + SubCategoryId + "&ItemId=" + ItemId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                objreturn = (serializer).Deserialize<requestdbmlSalesReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;


            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlSalesReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // sandeep 9-8-2022
        #region  InvoiceStatisticsData

        public ActionResult InvoiceStatisticsDataReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }

        //[ValidateAntiForgeryToken]
        public ActionResult InvoiceStatisticsDataReportFromDateToDateReport(string SuperStockistID, int CustomerID, string Fromdate, string Todate)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlStatisticsReport objreturn = new requestdbmlStatisticsReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string PartyIdVal = "0";

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = SuperStockistID;
                else
                    //PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                    PartyIdVal = Session["PartyId"].ToString();

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "InvoiceStatisticsDataReportFromDateToDateReport?Fromdate=" + Fromdate + "&Todate=" + Todate + "&SuperStockistID=" + PartyIdVal + "&CustomerID=" + CustomerID ).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                objreturn = (serializer).Deserialize<requestdbmlStatisticsReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlStatisticsReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // sandeep 13-8-2022
        #region  ReprimarySaleReport

        public ActionResult ReprimarySaleReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }
        //[ValidateAntiForgeryToken]
        public ActionResult ReprimarySaleReportFromDateToDateReport(string SuperStockistID, string Fromdate, string Todate)
         {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlReprimarySaleReport objreturn = new requestdbmlReprimarySaleReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string PartyIdVal = "0";

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = SuperStockistID;
                else
                    //PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                    PartyIdVal = Session["PartyId"].ToString();

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ReprimarySaleReportFromDateToDateReport?Fromdate=" + Fromdate + "&Todate=" + Todate + "&SuperStockistID=" + PartyIdVal).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                objreturn = (serializer).Deserialize<requestdbmlReprimarySaleReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlReprimarySaleReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //Added by Piyush on 22-08-2022
        public ActionResult ReprimarySalesRegionWiseReport()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            returndbmlPartyMaster objreturn2 = new returndbmlPartyMaster();
            string responseBody = "";
            string responseBody2 = "";

            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentIdForDropdown?ParentId=-1").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            //var response2 = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
            var response2 = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
            responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlPartyMaster.ToList();
            ViewBag.UserTypeID = Session["UserTypeId"].ToString();
            return View();

        }
        //[ValidateAntiForgeryToken]
        public ActionResult ReprimarySalesRegionWiseDateReport(string SuperStockistID,string RunDate)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            requestdbmlReprimarySaleRegionWiseReport objreturn = new requestdbmlReprimarySaleRegionWiseReport();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string PartyIdVal = "0";

                if (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23")
                    PartyIdVal = SuperStockistID;
                else
                    //PartyIdVal = Convert.ToInt32(Session["PartyId"]);
                    PartyIdVal = Session["PartyId"].ToString();

                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ReprimarySalesRegionWiseDateReport?SuperStockistID=" + PartyIdVal + "&RunDate=" + RunDate).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                objreturn = (serializer).Deserialize<requestdbmlReprimarySaleRegionWiseReport>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlReprimarySaleRegionWiseReport, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

    }
}