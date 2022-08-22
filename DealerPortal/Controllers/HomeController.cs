///// --Swati joshi 23092019, 4.24 ////
using DealerPortal.Common;
using DealerPortal.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using ZXing.QrCode;

namespace DealerPortal.Controllers
{
    public class HomeController : Controller
    {

        AesOperation objAesOperation = new AesOperation();

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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult CustomerMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["FreightTypeList"] = GetFreightTypeMaster();
            return View();
        }
        public ActionResult CustomerDiscountMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            //string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            //HttpClient client = new HttpClient();
            //var response = client.GetAsync(apiUrl + "GetCustmerBySuperStockistId?PartyId=" + Session["PartyId"]).Result;
            //string responseBody = response.Content.ReadAsStringAsync().Result;
            //objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
            //ViewBag.CustomerListDM = objreturn.objdbmlTempCustomer.ToList();
            //DataTable dt = (new JavaScriptSerializer()).Deserialize<DataTable>(responseBody);

            //returnTempCustomerDiscountItemView objOL = new returnTempCustomerDiscountItemView();
            //string apiUrl1 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            //HttpClient client1 = new HttpClient();
            //var response1 = client1.GetAsync(apiUrl1 + "GetItemsbySuperStockistId?PartyId=" + Session["PartyId"]).Result;
            //string responseBody1 = response1.Content.ReadAsStringAsync().Result;
            //objOL = (new JavaScriptSerializer()).Deserialize<returnTempCustomerDiscountItemView>(responseBody1);
            //ViewBag.ItemListDM = objOL.objTempCustomerDiscountItemView.ToList();


            DealerPortal_devEntities db = new DealerPortal_devEntities();
            List<PartyType> ObjList = new List<PartyType>();
            //Note : you can bind same list from database  

            var studentList = db.PartyTypes.ToList();
            foreach (PartyType item in studentList)
            {
                ObjList.Add(new PartyType() { PartyTypeID = Convert.ToInt32(item.PartyTypeID), PartyType1 = item.PartyType1, IsActive = true });
            }
            ViewBag.PartyTypeDM = ObjList.ToList();


            return View();
        }
        #endregion

        #region Login

        [ValidateAntiForgeryToken]
        public ActionResult UserLoginPwdVerification(string EmailId, String Password)
        {
            returndbmlTempLoginMaster objreturndbmlTempLoginMaster = new returndbmlTempLoginMaster();
            dbmlCommon objdbmlCommon = new dbmlCommon();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objdbmlCommon.StringOne = EmailId;
                objdbmlCommon.StringTwo = Password;
                string inputJson = (new JavaScriptSerializer()).Serialize(objdbmlCommon);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserLoginPwdVerification", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturndbmlTempLoginMaster = (new JavaScriptSerializer()).Deserialize<returndbmlTempLoginMaster>(responseBody);
                intStatusId = objreturndbmlTempLoginMaster.objdbmlStatus.StatusId;
                strStatus = objreturndbmlTempLoginMaster.objdbmlStatus.Status;
                if (objreturndbmlTempLoginMaster.objdbmlStatus.StatusId == 1)
                {
                    Session["UserId"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].UserId;
                    Session["PartyId"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].IdentityId;
                    Session["UserName"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].ZZName;
                    Session["UserTypeId"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].UserTypeParaId;
                    Session["EmailId"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].EmailID;
                    Session["PartyName"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].ZZPartyName;
                    Session["StateID"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].StateID;
                    Session["State"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].ZZState;
                    Session["IsUT"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].ZZIsUT;
                    Session["DownLoadType"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].DownLoadType;
                    Session["IsInvoiceAuto"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].IsInvoiceAuto;
                    Session["isEInvoiceApplicable"] = objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].isEInvoiceApplicable;
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            Session["PartyId"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");

        }

        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordSubmit(string EmailId)
        {
            int StatusId = 99;
            string strStatus = "Invalid";
            try
            {
                returndbmlTempLoginMaster objreturndbmlTempLoginMaster = new returndbmlTempLoginMaster();
                dbmlCommon objdbmlCommon = new dbmlCommon();
                objdbmlCommon.StringOne = EmailId;
                string inputJson = (new JavaScriptSerializer()).Serialize(objdbmlCommon);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserGetByEmailId", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturndbmlTempLoginMaster = (new JavaScriptSerializer()).Deserialize<returndbmlTempLoginMaster>(responseBody);
                StatusId = objreturndbmlTempLoginMaster.objdbmlStatus.StatusId;
                strStatus = objreturndbmlTempLoginMaster.objdbmlStatus.Status;
                if (objreturndbmlTempLoginMaster.objdbmlStatus.StatusId == 1)
                {
                    //string strUserPassword = Cryptographer.CreateHash("DealerPortal", objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].EmailID);
                    string strUserPassword = objAesOperation.Encrypt(objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].EmailID, "DealerPortal");
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    string mailbody = "";
                    string FromEmailId = System.Configuration.ConfigurationManager.AppSettings["Emaild"];
                    string FromPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
                    string ResetURL = System.Configuration.ConfigurationManager.AppSettings["ResetURL"] + "?UserId=" + objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].UserId + "&EmailId=" + strUserPassword + "";


                    mail.Subject = "Dealer Portal";
                    mailbody += "<div class='wrapper' style='width:40%; margin:50px auto;text-align:center;'>";
                    mailbody += "<div class='thank' style='width:100%; background: #fff;box-shadow: 0 1px 2px #888;margin-top:20px; border:1px solid #ccc;'>";
                    mailbody += "<div class='top-head' style='background: #ffffff; text-align: center; padding: 20px 0;border-bottom: 2px solid #e32228;'>";
                    mailbody += "<img src='http://103.87.174.226:8086/Template/images/logo.png' />";
                    mailbody += "</div>";
                    mailbody += "<div class='middle' style='padding:30px;'>";
                    mailbody += "<h3>Hello,<b>Mr." + objreturndbmlTempLoginMaster.objdbmlTempLoginMaster[0].ZZName + "</b></h3><br/>";
                    mailbody += "<p style='margin:0; font-weight:600;'>Click on the below link to change your password.</p><br/>";
                    mailbody += "<a href='" + ResetURL + "'> Click here to reset Password</a>";
                    mailbody += "</div>";
                    mailbody += "</div>";
                    mailbody += "</div>";
                    mail.To.Add(EmailId);
                    mail.From = new MailAddress(FromEmailId, "Dealer Portal", System.Text.Encoding.UTF8);
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = mailbody;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient SMclient = new SmtpClient();
                    SMclient.Credentials = new System.Net.NetworkCredential(FromEmailId, FromPassword);
                    SMclient.Port = 587;
                    SMclient.Host = "smtp.gmail.com";
                    SMclient.EnableSsl = true;
                    SMclient.Send(mail);
                    StatusId = 1;
                }

            }
            catch (Exception ex)
            {
                StatusId = 99;
                strStatus = ex.Message.ToString() + " " + ex.StackTrace.ToString();
            }
            return Json(new { Status = strStatus, StatusId = StatusId }, JsonRequestBehavior.AllowGet);

        }

        #region Change Password
        public ActionResult UserMasterChangePassword(string EmailId, string NewPassword)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            dbmlCommon objinput = new dbmlCommon();
            objinput.StringOne = Convert.ToString(Session["UserId"]);
            objinput.StringTwo = NewPassword;
            returndbmlStatus objdbmlUserMasterView = new returndbmlStatus();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterChangePassword", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
                intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlUserMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserMasterChangePasswordNew(string UserId, string NewPassword)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            dbmlCommon objinput = new dbmlCommon();
            objinput.StringOne = UserId;
            objinput.StringTwo = NewPassword;
            returndbmlStatus objdbmlUserMasterView = new returndbmlStatus();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterChangePassword", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
                intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlUserMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserMasterGetByEmailId(string EmailId, string Password)
        {

            returndbmlStatus objdbmlUserMasterView = new returndbmlStatus();
            dbmlCommon objinput = new dbmlCommon();
            objinput.StringOne = Convert.ToString(Session["UserId"]);
            objinput.StringTwo = EmailId;
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterGetByEmailId", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
                intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlUserMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reset Password

        public ActionResult ResetPassword()
        {
            if (Request.QueryString["UserId"] == null || Request.QueryString["EmailId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int ResetUserId = Convert.ToInt32(Request.QueryString["UserId"]);
            string ResetEmailId = Convert.ToString(Request.QueryString["EmailId"]);


            returndbmlStatus objreturn = new returndbmlStatus();
            dbmlCommon objinput = new dbmlCommon();
            objinput.StringOne = Convert.ToString(ResetUserId);

            string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
            HttpClient client = new HttpClient();
            HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            var response = client.PostAsync(apiUrl + "UserGetByUserId", inputContent).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
            string EmailDescript = objAesOperation.Decrypt(ResetEmailId, "DealerPortal");
            if (EmailDescript != objreturn.objdbmlStatus.Status)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["ResetUserId"] = ResetUserId;
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordSubmit(string Password)
        {

            dbmlCommon objinput = new dbmlCommon();
            objinput.StringOne = Convert.ToString(TempData["ResetUserId"]);
            objinput.StringTwo = Password;
            returndbmlStatus objdbmlUserMasterView = new returndbmlStatus();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterChangePassword", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
                intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlUserMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Place Order
        public ActionResult PlaceOrder()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            string responseBody = "";


            var response1 = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentId?ParentId=0").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
            ViewBag.RetailCategoryList = objreturn.objdbmlRetailCategoryMaster.ToList();

            returndbmlMRPCategoryMaster objreturndbmlMRPCategoryMaster = new returndbmlMRPCategoryMaster();
            var response2 = client.GetAsync(apiUrl + "MRPCategoryMasterGetAll").Result;
            responseBody = response2.Content.ReadAsStringAsync().Result;
            objreturndbmlMRPCategoryMaster = (new JavaScriptSerializer()).Deserialize<returndbmlMRPCategoryMaster>(responseBody);
            ViewBag.MRPCategoryGroupList = objreturndbmlMRPCategoryMaster.objdbmlMRPCategoryMaster.ToList();


            returndbmlItemDetailView objreturndbmlItemDetailView = new returndbmlItemDetailView();
            var response3 = client.GetAsync(apiUrl + "CartViewGetByUserId?UserId=" + Session["UserId"] + "&ParentId=" + Session["PartyId"]).Result;
            responseBody = response3.Content.ReadAsStringAsync().Result;
            objreturndbmlItemDetailView = (new JavaScriptSerializer()).Deserialize<returndbmlItemDetailView>(responseBody);
            ViewBag.TempCartViewList = objreturndbmlItemDetailView.objdbmlTempCartView.ToList();



            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult RetailCategoryMasterGetByParentId(int ParentId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlRetailCategoryMaster objreturn = new returndbmlRetailCategoryMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "RetailCategoryMasterGetByParentId?ParentId=" + ParentId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlRetailCategoryMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlRetailCategoryMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ItemMasterGetByCategoryGroupIdMRPCategoryIdPartyIdUserId(int CategoryId, int CategoryGroupId, int CategoryMRPId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemDetailView objreturn = new returndbmlItemDetailView();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemMasterGetByCategoryGroupIdMRPCategoryIdPartyIdUserId?CategoryId=" + CategoryId + "&CategoryGroupId=" + CategoryGroupId + "&MRPCategoryId=" + CategoryMRPId + "&PartyId=" + Session["PartyId"] + "&UserId=" + Session["UserId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemDetailView>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempItemMaster, CartList = objreturn.objdbmlTempCartView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CartInsert(ObservableCollection<dbmlCart> objdbmlCart, dbmlCommon ObjdbmlCommon)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlItemDetailView objreturndbmlItemDetailView = new returndbmlItemDetailView();
            RequestCartInsert objinput = new RequestCartInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in objdbmlCart)
                {
                    item.UpdateDate = DateTime.Now;
                    item.UserId = Convert.ToInt32(Session["UserId"]);

                }
                ObjdbmlCommon.StringThree = Convert.ToString(Session["PartyId"]);
                ObjdbmlCommon.StringFour = Convert.ToString(Session["UserId"]);

                objinput.objdbmlCart = objdbmlCart;
                objinput.objdbmlCommon = ObjdbmlCommon;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "CartInsert", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturndbmlItemDetailView = (new JavaScriptSerializer()).Deserialize<returndbmlItemDetailView>(responseBody);
                intStatusId = objreturndbmlItemDetailView.objdbmlStatus.StatusId;
                strStatus = objreturndbmlItemDetailView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objreturndbmlItemDetailView.objdbmlTempItemMaster, CartList = objreturndbmlItemDetailView.objdbmlTempCartView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CartGetByUserId()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCartMaster objreturn = new returndbmlTempCartMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "CartGetByUserId?UserId=" + Session["UserId"] + "&PartyId=" + Session["PartyId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCartMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCartMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CartDelete(int intCartId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCartMaster objreturn = new returndbmlTempCartMaster();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intCartId);
                objinput.StringTwo = Convert.ToString(Session["UserId"]);
                objinput.StringThree = Convert.ToString(Session["PartyId"]);
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "CartDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCartMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCartMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderInsert(ObservableCollection<dbmlTempCartMaster> objdbmlTempCartMaster, dbmlCommon ObjdbmlCommon)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            dbmlStatus objdbmlStatus = new dbmlStatus();
            RequestOrderInsert objinput = new RequestOrderInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in objdbmlTempCartMaster)
                {
                    item.PartyId = Convert.ToInt32(Session["PartyId"]);
                }
                ObjdbmlCommon.StringOne = Convert.ToString(Session["UserId"]);
                objinput.objdbmlTempCartMaster = objdbmlTempCartMaster;
                objinput.objdbmlCommon = ObjdbmlCommon;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "OrderInsert", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlStatus = (new JavaScriptSerializer()).Deserialize<dbmlStatus>(responseBody);
                intStatusId = objdbmlStatus.StatusId;
                strStatus = objdbmlStatus.Status;
                if (intStatusId == 1)
                {
                    dbmlStatus objdbmlStatus1 = SalesOrderInsert(strStatus);
                    intStatusId = objdbmlStatus1.StatusId;
                    //strStatus = objdbmlStatus1.Status;

                    if (intStatusId == 1)
                    {
                        dbmlCommon objinputOrderUpdt = new dbmlCommon();
                        objinputOrderUpdt.StringOne = strStatus;
                        objinputOrderUpdt.StringTwo = objdbmlStatus1.Status;

                        string inputJsonOrderUpdt = (new JavaScriptSerializer()).Serialize(objinputOrderUpdt);
                        HttpContent inputContentOrderUpdt = new StringContent(inputJsonOrderUpdt, Encoding.UTF8, "application/json");

                        var response1 = client.PostAsync(apiUrl + "OrderHeaderUpdate", inputContentOrderUpdt).Result;
                        string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                        objdbmlStatus = (new JavaScriptSerializer()).Deserialize<dbmlStatus>(responseBody1);
                    }
                    else
                    {
                        intStatusId = 11;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public dbmlStatus SalesOrderInsert(string strStatus)
        {
            dbmlStatus objdbmlStatus = new dbmlStatus();
            try
            {
                string OrderHeaderId = strStatus;
                string strSalesOrderNo = "";

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OrderGetByOrderId?OrderHeaderId=" + OrderHeaderId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                returndbmlTempOrderHeaderId objreturndbmlTempOrderHeaderId = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderHeaderId>(responseBody);

                returndbmlPartyMaster objreturndbmlPartyMaster = PartyMasterGetByPartyId(objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().PartyId);

                returndbmlCurrencyMaster objdbmlCurrencyMaster = CurrencyMasterGetByCurrencyName("INR");

                returndbmlYearMaster objreturndbmlYearMaster = YearMasterGetByDate(Convert.ToString(objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().ZZOrderDate));

                returndbmlOptionList objreturndbmlOptionList = OptionListGetByParameterId("2");

                returndbmlOptionList objreturndbmlOptionListFactory = OptionListGetByParameterId("3");

                returndbmlOptionList objreturndbmlOptionListinvoice = OptionListGetByParameterId("4");

                strSalesOrderNo = objreturndbmlOptionListinvoice.objdbmlOptionList.Where(t => t.OptionListCode == "Prefix").FirstOrDefault().OptionListDesc
                    + "/" +
                    objreturndbmlOptionList.objdbmlOptionList.Where(t => t.OptionListCode == "RegisterName").FirstOrDefault().OptionListDesc
                    + "/" +
                    objreturndbmlOptionListFactory.objdbmlOptionList.Where(t => t.OptionListCode == "ShortName").FirstOrDefault().OptionListDesc
                    + "/" +
                    objreturndbmlYearMaster.objdbmlYearMaster.FirstOrDefault().YearName
                ;

                //SET @SALESORDERNO = @PREFIX + '/' + @RegShortName + '/' + @FACTORYSHORTNAME + '/' + @YEAR + '/' + CAST(@SRNO AS VARCHAR)

                dbmlTempSalesOrder objdbmlTempSalesOrder = new dbmlTempSalesOrder();
                objdbmlTempSalesOrder.OrderNo = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().OrderNo;
                objdbmlTempSalesOrder.OrderDate = Convert.ToDateTime(objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().OrderDate).ToString("dd/MMM/yyyy");
                objdbmlTempSalesOrder.PartyId = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().PartyId;
                objdbmlTempSalesOrder.remark = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().remark;
                objdbmlTempSalesOrder.CreateId = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().CreateId;

                objdbmlTempSalesOrder.UpdatedId = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().UpdatedId;

                objdbmlTempSalesOrder.CurrencyID = objdbmlCurrencyMaster.objdbmlCurrencyMaster.FirstOrDefault().CurrencyID;
                objdbmlTempSalesOrder.YearID = objreturndbmlYearMaster.objdbmlYearMaster.FirstOrDefault().YearID;
                objdbmlTempSalesOrder.PartyGroupId = Convert.ToInt32(objreturndbmlPartyMaster.objdbmlPartyMaster.FirstOrDefault().PartyGroupID);
                objdbmlTempSalesOrder.DistributorID = Convert.ToInt32(objreturndbmlPartyMaster.objdbmlPartyMaster.FirstOrDefault().DistributorID);
                objdbmlTempSalesOrder.RegisterID = Convert.ToInt32(objreturndbmlOptionList.objdbmlOptionList.Where(t => t.OptionListCode == "RegisterId").FirstOrDefault().OptionListDesc);
                objdbmlTempSalesOrder.SoTypeId = Convert.ToInt32(objreturndbmlOptionList.objdbmlOptionList.Where(t => t.OptionListCode == "SOTypeId").FirstOrDefault().OptionListDesc);

                objdbmlTempSalesOrder.FactoryId = Convert.ToInt32(objreturndbmlOptionListFactory.objdbmlOptionList.Where(t => t.OptionListCode == "Id").FirstOrDefault().OptionListDesc);
                objdbmlTempSalesOrder.SalesOrderNo = strSalesOrderNo;

                objdbmlTempSalesOrder.MenuID = Convert.ToInt32(objreturndbmlOptionList.objdbmlOptionList.Where(t => t.OptionListCode == "MenuID").FirstOrDefault().OptionListDesc);
                objdbmlTempSalesOrder.ConsigneeID = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().PartyId;
                //  objdbmlTempSalesOrder.ExpiryDate = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().OrderDate;
                //objdbmlTempSalesOrder.CreateDate = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().CreateDate;
                //objdbmlTempSalesOrder.SrNo = Convert.ToInt32(objreturndbmlOptionList.objdbmlOptionList.Where(t => t.OptionListCode == "SrNo").FirstOrDefault().OptionListDesc);
                //objdbmlTempSalesOrder.UpdateDate = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().UpdateDate;

                requestdbmlSalesOrder objinputDP = new requestdbmlSalesOrder();
                objinputDP.objdbmlTempSalesOrder.Add(objdbmlTempSalesOrder);

                for (int i = 0; i < objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId.Count; i++)
                {
                    dbmlTempSalesOrderItem objdbmlTempSalesOrderItem = new dbmlTempSalesOrderItem();
                    objdbmlTempSalesOrderItem.SalesOrderId = 0;
                    objdbmlTempSalesOrderItem.ItemId = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].ItemId;
                    objdbmlTempSalesOrderItem.OrderUnits = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].OrderUnits;
                    objdbmlTempSalesOrderItem.DealerPrice = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].DealerPrice;
                    objdbmlTempSalesOrderItem.PackSize = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].PackSize;
                    objdbmlTempSalesOrderItem.MRP = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].MRP;
                    objdbmlTempSalesOrderItem.DiscountPercentage1 = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].DiscountPercentage1;
                    objdbmlTempSalesOrderItem.DiscountPercentage2 = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].DiscountPercentage2;
                    objdbmlTempSalesOrderItem.DiscountPercentage3 = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].DiscountPercentage3;
                    objdbmlTempSalesOrderItem.DiscountPercentage4 = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].DiscountPercentage4;
                    objdbmlTempSalesOrderItem.OrderQty = objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderDetailId[i].OrderQty;

                    objinputDP.objdbmlTempSalesOrderItem.Add(objdbmlTempSalesOrderItem);
                }

                string inputJsonDP = (new JavaScriptSerializer()).Serialize(objinputDP);
                HttpClient clientDP = new HttpClient();
                HttpContent inputContentDP = new StringContent(inputJsonDP, Encoding.UTF8, "application/json");

                returndbmlStatus OBJreturndbmlStatus = new returndbmlStatus();
                string apiUrlDP = System.Configuration.ConfigurationManager.AppSettings["apiUrlDP"];
                var responseDP = client.PostAsync(apiUrlDP + "SalesOrderInsert", inputContentDP).Result;
                string responseBodyDP = responseDP.Content.ReadAsStringAsync().Result;
                OBJreturndbmlStatus = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBodyDP);

                objdbmlStatus = OBJreturndbmlStatus.objdbmlStatus;

                SendMailAfterPlaceOrder(objreturndbmlTempOrderHeaderId.objdbmlTempOrderHeaderId.FirstOrDefault().OrderNo, OrderHeaderId);
            }
            catch (Exception ex)
            {
                objdbmlStatus.Status = ex.Message.ToString() + "/n" + ex.StackTrace.ToString();
                objdbmlStatus.StatusId = 99;
            }

            return objdbmlStatus;
        }
        public returndbmlPartyMaster PartyMasterGetByPartyId(int PartyId)
        {

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyMasterGetByPartyId?PartyId=" + PartyId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return objreturn;
        }

        public returndbmlOptionList OptionListGetByParameterId(string ParameterId)
        {

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlOptionList objreturn = new returndbmlOptionList();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OptionListGetByParameterId?ParameterId=" + ParameterId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return objreturn;
        }

        public returndbmlYearMaster YearMasterGetByDate(string Orderdate)
        {

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlYearMaster objreturn = new returndbmlYearMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "YearMasterGetByDate?OrderDate=" + Orderdate).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlYearMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return objreturn;
        }

        public returndbmlCurrencyMaster CurrencyMasterGetByCurrencyName(string strCurrencyName)
        {

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlCurrencyMaster objreturn = new returndbmlCurrencyMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "CurrencyMasterGetByCurrencyName?strCurrencyName=" + strCurrencyName).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlCurrencyMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return objreturn;
        }

        [ValidateAntiForgeryToken]
        public ActionResult OrderHeaderGetByOrderId(int OrderId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempOrderHistory objreturn = new returndbmlTempOrderHistory();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OrderHeaderGetByOrderId?OrderHeaderId=" + OrderId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderHistory>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;
                SendMail(objreturn.objdbmlTempOrderHistory[0].OrderNo);
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempOrderHistory, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public int SendMail(string OrderNo)
        {
            int StatusId = 99;
            string strStatus = "Invalid";
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                string mailbody = "";
                string FromEmailId = System.Configuration.ConfigurationManager.AppSettings["Emaild"];
                string FromPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
                string EmailId = Convert.ToString(Session["EmailId"]);
                mail.Subject = "Dealer Portal";
                mailbody += "<div class='wrapper' style='width:40%; margin:50px auto;text-align:center'>";
                mailbody += "<img src='http://103.87.174.226:8086/Template/images/logo.png'/>";
                mailbody += "<div class='thank' style='width:100%; background: #fff; box-shadow:0 1px 2px #888;  margin-top:20px;'>";
                mailbody += "<div class='top-head' style='background:#23b67f;text-align:center;padding:10px 0;'>";
                mailbody += "<img src='http://103.87.174.226:8086/Template/images/thumbs-up.png' />";
                mailbody += "</div>";
                mailbody += "<div class='middle' style='padding:30px; '>";
                mailbody += "<h1 style='margin:0; color: #23b67f;' > THANK YOU!</h1>";
                mailbody += "<p style ='margin: 0; text-transform:uppercase; font-weight:600;'><b> Your Order Has been received</b></p>";
                mailbody += "<div class='order' style='background:#f2f2f2; margin:20px 0; padding:10px;'>";
                mailbody += "<b style='color:#23b67f;'> " + OrderNo + "</b>";
                mailbody += "</div>";
                mailbody += "<span> For more details<a target='_blank' href= 'http://183.182.84.210/DealerPortal' > click here</a></span>";
                mailbody += "</div>";
                mailbody += "</div>";
                mailbody += "</div>";
                mail.To.Add(EmailId);
                mail.From = new MailAddress(FromEmailId, "Dealer Portal", System.Text.Encoding.UTF8);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = mailbody;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(FromEmailId, FromPassword);
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Send(mail);
                StatusId = 1;
                strStatus = "Send Mail";

            }
            catch (Exception ex)
            {
                StatusId = 99;
                strStatus = ex.Message.ToString() + " " + ex.StackTrace.ToString();
            }
            return StatusId;
        }


        public int SendMailAfterPlaceOrder(string OrderNo, string OrderHeaderId)
        {
            int StatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client1 = new HttpClient();
                HttpClient client2 = new HttpClient();
                HttpClient client3 = new HttpClient();
                var response = client1.GetAsync(apiUrl + "GetMailServerDetailsByOrderId?MailTypeID=1&SuperStockistID=" + Session["PartyId"] + "&OrderID=" + OrderHeaderId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                returndbmlMailServerDetails objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlMailServerDetails>(responseBody);

                if (objreturn.objdbmlMailServerDetails.Count > 0)
                {
                    string strMailServer = objreturn.objdbmlMailServerDetails[0].MailServer;
                    string strMailUser = objreturn.objdbmlMailServerDetails[0].MailUser;
                    string strPassword = objreturn.objdbmlMailServerDetails[0].Password;
                    string strFromEmailId = objreturn.objdbmlMailServerDetails[0].FromID;
                    string strToEmailId = objreturn.objdbmlMailServerDetails[0].ToEmailID;
                    string SuperStockistEmailId = Convert.ToString(Session["EmailId"]);
                    string strCCEmail = objreturn.objdbmlMailServerDetails[0].CCEmail != "" ? objreturn.objdbmlMailServerDetails[0].CCEmail + "," + SuperStockistEmailId : SuperStockistEmailId;
                    string strBCCEmail = objreturn.objdbmlMailServerDetails[0].BCCEmail;
                    string strPortNo = objreturn.objdbmlMailServerDetails[0].PortNo.ToString();
                    bool strSSL = Boolean.Parse(objreturn.objdbmlMailServerDetails[0].SSL.ToString());
                    string strSubject = "Sales Order - " + Session["PartyName"].ToString() + "- at " + DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();


                    //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    //string mailbody = "";

                    //string FromPassword = strPassword;
                    //mail.Subject = strSubject;
                    //mailbody += "<div class='wrapper' style='width:40%; margin:50px auto;text-align:center'>";
                    //mailbody += "<img src='https://dealerportal.unomaxworld.com/Template/images/logo.png'/>";
                    //mailbody += "<div class='thank' style='width:100%; background: #fff; box-shadow:0 1px 2px #888;  margin-top:20px;'>";
                    //mailbody += "<div class='top-head' style='background:#23b67f;text-align:center;padding:10px 0;'>";
                    //mailbody += "<img src='https://dealerportal.unomaxworld.com/Template/images/thumbs-up.png' />";
                    //mailbody += "</div>";
                    //mailbody += "<div class='middle' style='padding:30px; '>";
                    ////mailbody += "<h1 style='margin:0; color: #23b67f;' > THANK YOU!</h1>";
                    //mailbody += "<p style ='margin: 0; text-transform:uppercase; font-weight:600;'><b> Order Has been placed</b></p>";
                    //mailbody += "<div class='order' style='background:#f2f2f2; margin:20px 0; padding:10px;'>";
                    //mailbody += "<b style='color:#23b67f;'> " + OrderNo + "</b>";
                    //mailbody += "</div>";
                    ////mailbody += "<span> For more details<a target='_blank' href= 'https://dealerportal.unomaxworld.com/' > click here</a></span>";
                    //mailbody += "</div>";
                    //mailbody += "</div>";
                    //mailbody += "</div>";
                    //mail.To.Add(EmailId);
                    //mail.From = new MailAddress(strFromEmailId, "Place Order", System.Text.Encoding.UTF8);
                    //mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    //mail.Body = mailbody;
                    //mail.BodyEncoding = System.Text.Encoding.UTF8;
                    //mail.IsBodyHtml = true;
                    //mail.Priority = MailPriority.High;
                    //SmtpClient client = new SmtpClient();
                    //client.Credentials = new System.Net.NetworkCredential(strFromEmailId, strPassword);
                    //client.Port = int.Parse(strPortNo);
                    //client.Host = strMailServer;
                    //client.EnableSsl = strSSL;
                    //client.Send(mail);
                    var response2 = client3.GetAsync(apiUrl + "OrderGetByOrderId?OrderHeaderId=" + OrderHeaderId + "").Result;
                    string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                    returndbmlTempOrderHeaderId objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderHeaderId>(responseBody2);

                    var response1 = client2.GetAsync(apiUrl + "OrderDetailGetByOrderId?OrderHeaderId=" + OrderHeaderId + "").Result;
                    string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                    returndbmlTempOrderDetail objreturn1 = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderDetail>(responseBody1);
                    string Emailbody = "";
                    if (objreturn1.objdbmlTempOrderDetail.Count > 0)
                    {
                        Emailbody = "<table border='1'>";
                        Emailbody += "<tr><td colspan='5' align='left'><b>Order No: </b>" + OrderNo + "<span style='float:right'><b>Date:  </b>" + DateTime.Now.ToString("dd/MMM/yyyy") + "</span></td></tr>";
                        Emailbody += "<tr><td width='100px'><b>Code</b></td><td width=400px'><b>Product Name</b></td><td width='100px' align='right'><b>PackSize</b></td><td width='100px' align='right'><b>Cartons</b></td><td width='100px' align='right'><b>Quantity</b></td></tr>";
                        decimal PackSizeTotal = 0;
                        decimal OrderCartonTotal = 0;
                        decimal OrderUnitsTotal = 0;
                        for (int x = 0; x < objreturn1.objdbmlTempOrderDetail.Count; x++)
                        {
                            Emailbody += "<tr>";
                            Emailbody += "<td>" + objreturn1.objdbmlTempOrderDetail[x].ItemCode + "</td>";
                            Emailbody += "<td>" + objreturn1.objdbmlTempOrderDetail[x].ItemName + "</td>";
                            Emailbody += "<td align='right'>" + String.Format("{0:0.00}", objreturn1.objdbmlTempOrderDetail[x].PackSize) + "</td>";
                            Emailbody += "<td align='right'>" + String.Format("{0:0.00}", objreturn1.objdbmlTempOrderDetail[x].OrderCarton) + "</td>";
                            Emailbody += "<td align='right'>" + String.Format("{0:0.00}", objreturn1.objdbmlTempOrderDetail[x].OrderUnits) + "</td>";
                            Emailbody += "</tr>";
                            PackSizeTotal += objreturn1.objdbmlTempOrderDetail[x].PackSize;
                            OrderCartonTotal += objreturn1.objdbmlTempOrderDetail[x].OrderCarton;
                            OrderUnitsTotal += objreturn1.objdbmlTempOrderDetail[x].OrderUnits;
                        }
                        Emailbody += "<tr>";
                        Emailbody += "<td>&nbsp;</td>";
                        Emailbody += "<td align='right'><b>Total</b>&nbsp;</td>";
                        Emailbody += "<td align='right'><b>" + Convert.ToDecimal(PackSizeTotal) + "</b></td>";
                        Emailbody += "<td align='right'><b>" + Convert.ToDecimal(OrderCartonTotal) + "</b></td>";
                        Emailbody += "<td align='right'><b>" + Convert.ToDecimal(OrderUnitsTotal) + "</b></td>";
                        Emailbody += "</tr>"; ;
                        Emailbody += "</table>";
                    }

                    if (objreturn2.objdbmlTempOrderHeaderDetailId.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(objreturn2.objdbmlTempOrderHeaderId[0].remark))
                        {
                            Emailbody += "<br><b>Remarks:</b> " + objreturn2.objdbmlTempOrderHeaderId[0].remark.Replace(Environment.NewLine, "<br>");
                        }
                        else
                        {
                            Emailbody += "<br><b>Remarks:</b> ";
                        }
                    }
                    else
                    {
                        Emailbody += "<br><b>Remarks:</b> ";
                    }

                    MailMessage message = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    MailAddress fromAddress = new MailAddress(strFromEmailId);
                    message.From = fromAddress;
                    message.To.Add(strToEmailId);
                    message.CC.Add(strCCEmail);
                    message.Bcc.Add(strBCCEmail);
                    message.Subject = strSubject;
                    message.IsBodyHtml = true;
                    message.Body = Emailbody;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(strMailUser, strPassword);
                    smtpClient.Host = strMailServer;
                    smtpClient.Port = int.Parse(strPortNo);
                    smtpClient.EnableSsl = strSSL;
                    smtpClient.Send(message);
                    StatusId = 1;
                    strStatus = "Send Mail";
                }
            }
            catch (Exception ex)
            {
                StatusId = 99;
                strStatus = ex.Message.ToString() + " " + ex.StackTrace.ToString();
            }
            return StatusId;
        }


        #endregion

        #region  Check out
        public ActionResult Checkout()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #endregion

        #region  Order History

        public ActionResult OrderHistory()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DateTime NowDate = DateTime.Now;
            DateTime FromDate = DateTime.Now.AddMonths(-1);

            int FDay = FromDate.Day;
            int FMonth = FromDate.Month;
            int FYear = FromDate.Year;

            if (FDay < 10 && FMonth < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else if (FDay < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + FMonth + "/" + FYear;
            }
            else if (FMonth < 10)
            {
                ViewBag.ZZFromDate = FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else
            {
                ViewBag.ZZFromDate = FDay + "/" + FMonth + "/" + FYear;
            }

            int NDay = NowDate.Day;
            int NMonth = NowDate.Month;
            int NYear = NowDate.Year;

            if (NDay < 10 && NMonth < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else if (NDay < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + NMonth + "/" + NYear;
            }
            else if (NMonth < 10)
            {
                ViewBag.ZZNowDate = NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else
            {
                ViewBag.ZZNowDate = NDay + "/" + NMonth + "/" + NYear;
            }

            return View();
        }

        public ActionResult OrderHistoryGetByFromDateToDatePartyId(string FromDate, string ToDate)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempOrderHistory objreturn = new returndbmlTempOrderHistory();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OrderHistoryGetByFromDateToDatePartyId?FromDate=" + FromDate + "&ToDate=" + ToDate + "&PartyId=" + Session["PartyId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderHistory>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempOrderHistory, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDetailGetByOrderId(string OrderId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempOrderDetail objreturn = new returndbmlTempOrderDetail();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OrderDetailGetByOrderId?OrderHeaderId=" + OrderId + "").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempOrderDetail>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempOrderDetail, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Pending Purchase Invoice

        public ActionResult PendingPurchaseInvoice()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            DateTime NowDate = DateTime.Now;
            DateTime FromDate = DateTime.Now.AddMonths(-1);

            int FDay = FromDate.Day;
            int FMonth = FromDate.Month;
            int FYear = FromDate.Year;

            if (FDay < 10 && FMonth < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else if (FDay < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + FMonth + "/" + FYear;
            }
            else if (FMonth < 10)
            {
                ViewBag.ZZFromDate = FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else
            {
                ViewBag.ZZFromDate = FDay + "/" + FMonth + "/" + FYear;
            }

            int NDay = NowDate.Day;
            int NMonth = NowDate.Month;
            int NYear = NowDate.Year;

            if (NDay < 10 && NMonth < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else if (NDay < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + NMonth + "/" + NYear;
            }
            else if (NMonth < 10)
            {
                ViewBag.ZZNowDate = NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else
            {
                ViewBag.ZZNowDate = NDay + "/" + NMonth + "/" + NYear;
            }

            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            string responseBody = "";
            returndbmlParameter objreturn = new returndbmlParameter();

            var response = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=6").Result; // DPStatus
            responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.DPStatusList = objreturn.objdbmlParameter.ToList();
            return View();
        }

        public ActionResult SalesChallanGetByFromDateToDateDPStatus(string FromDate, string ToDate, string DPStatusId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";

            ReturndbmlTempSalesChallan objreturn = new ReturndbmlTempSalesChallan();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "SalesChallanGetByFromDateToDateDPStatus?FromDate=" + FromDate + "&ToDate=" + ToDate + "&DPStatusId=" + DPStatusId + "&UserId=" + Convert.ToInt32(Session["PartyId"])).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempSalesChallan>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempSalesChallan, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesChallanUpdateDPStatus(ObservableCollection<dbmlSalesChallanUpdtReq> ObjSalesChallanList, dbmlCommon ObjdbmlCommon)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ReqSalesChallanUpdt objReqSalesChallanUpdt = new ReqSalesChallanUpdt();
            returndbmlStatus objreturndbmlStatus = new returndbmlStatus();
            ReturndbmlTempSalesChallan objreturn = new ReturndbmlTempSalesChallan();
            RequestUserInsert objinput = new RequestUserInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var obj in ObjSalesChallanList)
                {
                    objReqSalesChallanUpdt.objdbmlSalesChallanUpdtReq.Add(obj);
                }
                string inputJson = (new JavaScriptSerializer()).Serialize(objReqSalesChallanUpdt);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "SalesChallanUpdateDPStatus", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturndbmlStatus = (new JavaScriptSerializer()).Deserialize<returndbmlStatus>(responseBody);
                intStatusId = objreturndbmlStatus.objdbmlStatus.StatusId;
                strStatus = objreturndbmlStatus.objdbmlStatus.Status;
                if (intStatusId == 1)
                {

                    var response1 = client.GetAsync(apiUrl + "SalesChallanGetByFromDateToDateDPStatus?FromDate=" + ObjdbmlCommon.StringOne + "&ToDate=" + ObjdbmlCommon.StringTwo + "&DPStatusId=" + ObjdbmlCommon.StringFive).Result;
                    string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                    objreturn = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempSalesChallan>(responseBody1);
                    intStatusId = objreturn.objdbmlStatus.StatusId;
                    strStatus = objreturn.objdbmlStatus.Status;
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempSalesChallan, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesChallanGetBySalesChallanId(int SalesChallanId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";

            ReturnSalesChallanDetail objreturn = new ReturnSalesChallanDetail();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "SalesChallanGetBySalesChallanId?SalesChallanId=" + SalesChallanId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<ReturnSalesChallanDetail>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlSalesChallanDetailView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region  Thank You
        public ActionResult Thankyou()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlParameter objreturn = new returndbmlParameter();
            string responseBody = "";


            var response1 = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=5").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterList = objreturn.objdbmlParameter.ToList();

            return View();
        }

        #endregion

        #region  Master

        #region Item Master

        public ActionResult ItemMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ItemMasterGetAll()
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
                var response = client.GetAsync(apiUrl + "ItemMasterGetAll").Result;
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
        public ActionResult ItemMasterGetAllBySuperStockist(string SuperStockistID)
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
                var response = client.GetAsync(apiUrl + "ItemMasterGetAllBySuperStockist?SuperStockistID=" + SuperStockistID).Result;
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

        #endregion

        #region Party Master

        public ActionResult PartyMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult PartyMasterGetAll()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyMasterGetAll").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlPartyMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult PartyMasterGetForIdentity()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempPartyMasterIdentity objreturn = new returndbmlTempPartyMasterIdentity();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyMasterGetForIdentity").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempPartyMasterIdentity>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempPartyMasterIdentity, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Parameter

        public ActionResult Parameter()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ParameterGetAll()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlParameter objreturn = new returndbmlParameter();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ParameterGetAll").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlParameter, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Party ItemLink

        public ActionResult PartyItemLink()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ItemPartyLinkGetByPartyId(int strPartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            ReturndbmlTempIPL objreturn = new ReturndbmlTempIPL();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemPartyLinkGetByPartyId?strPartyId=" + strPartyId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempIPL>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { ResultLinked = objreturn.objdbmlTempIPLLinked, ResultNotLinked = objreturn.objdbmlTempIPLNotLinked, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemPartyLinkInsert(ObservableCollection<dbmlItemPartyLinkReq> ObjAvailableList)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ReturndbmlTempIPL objReturndbmlTempIPL = new ReturndbmlTempIPL();
            RequestdbmlItemPartyLinkReq objinput = new RequestdbmlItemPartyLinkReq();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in ObjAvailableList)
                {
                    item.UserId = Convert.ToInt32(Session["UserId"]);

                }

                objinput.objdbmlItemPartyLinkReq = ObjAvailableList;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "ItemPartyLinkInsert", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objReturndbmlTempIPL = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempIPL>(responseBody);
                intStatusId = objReturndbmlTempIPL.objdbmlStatus.StatusId;
                strStatus = objReturndbmlTempIPL.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultLinked = objReturndbmlTempIPL.objdbmlTempIPLLinked, ResultNotLinked = objReturndbmlTempIPL.objdbmlTempIPLNotLinked, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemPartyLinkDelete(ObservableCollection<dbmlItemPartyLinkReq> ObjLinkList)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ReturndbmlTempIPL objReturndbmlTempIPL = new ReturndbmlTempIPL();
            RequestdbmlItemPartyLinkReq objinput = new RequestdbmlItemPartyLinkReq();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in ObjLinkList)
                {
                    item.UserId = Convert.ToInt32(Session["UserId"]);

                }

                objinput.objdbmlItemPartyLinkReq = ObjLinkList;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "ItemPartyLinkDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objReturndbmlTempIPL = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempIPL>(responseBody);
                intStatusId = objReturndbmlTempIPL.objdbmlStatus.StatusId;
                strStatus = objReturndbmlTempIPL.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultLinked = objReturndbmlTempIPL.objdbmlTempIPLLinked, ResultNotLinked = objReturndbmlTempIPL.objdbmlTempIPLNotLinked, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemPartyLinkCopyPaste(dbmlCommon objinput)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ReturndbmlTempIPL objReturndbmlTempIPL = new ReturndbmlTempIPL();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objinput.StringThree = Convert.ToString(Session["UserId"]); ;
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "ItemPartyLinkCopyPaste", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objReturndbmlTempIPL = (new JavaScriptSerializer()).Deserialize<ReturndbmlTempIPL>(responseBody);
                intStatusId = objReturndbmlTempIPL.objdbmlStatus.StatusId;
                strStatus = objReturndbmlTempIPL.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultLinked = objReturndbmlTempIPL.objdbmlTempIPLLinked, ResultNotLinked = objReturndbmlTempIPL.objdbmlTempIPLNotLinked, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Option List 

        public ActionResult OptionList()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlParameter objreturn = new returndbmlParameter();
            string responseBody = "";

            var response1 = client.GetAsync(apiUrl + "ParameterGetAllForOptionList").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterDataList = objreturn.objdbmlParameter.ToList();

            return View();
        }

        public ActionResult OptionListUpsert(ObservableCollection<dbmlOptionList> objdbmlOptionList)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlOptionList objreturndbmlOptionList = new returndbmlOptionList();
            returndbmlOptionList objinput = new returndbmlOptionList();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in objdbmlOptionList)
                {
                    if (item.OptionListId == 0)
                    {
                        item.CreateId = Convert.ToInt32(Session["UserId"]);
                        item.CreateDate = System.DateTime.Now;
                        item.UpdateId = Convert.ToInt32(Session["UserId"]);
                        item.UpdateDate = System.DateTime.Now;
                    }
                    else
                    {
                        item.UpdateId = Convert.ToInt32(Session["UserId"]);
                        item.UpdateDate = System.DateTime.Now;
                    }
                }

                objinput.objdbmlOptionList = objdbmlOptionList;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "OptionListUpsert", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturndbmlOptionList = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody);
                intStatusId = objreturndbmlOptionList.objdbmlStatus.StatusId;
                strStatus = objreturndbmlOptionList.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objreturndbmlOptionList.objdbmlOptionList, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult OptionListDataGetByParameterId(string ParameterId)
        {

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlOptionList objreturn = new returndbmlOptionList();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "OptionListGetByParameterId?ParameterId=" + ParameterId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlOptionList, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult OptionListDelete(int intOLId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlOptionList objreturn = new returndbmlOptionList();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intOLId);
                objinput.StringTwo = Convert.ToString(Session["UserId"]);
                // objinput.StringThree = Convert.ToString(Session["PartyId"]);
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "OptionListDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlOptionList, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #endregion

        #region  User Master
        public ActionResult UserMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlParameter objreturn = new returndbmlParameter();
            string responseBody = "";

            var response = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=3").Result;
            responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterListUserType = objreturn.objdbmlParameter.ToList();


            var response1 = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=2").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterListSalutation = objreturn.objdbmlParameter.ToList();

            return View();
        }

        public ActionResult UserMasterInsertUpdate(ObservableCollection<dbmlUserMaster> objdbmlUser)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlUserMaster objdbmlUserMasterView = new returndbmlUserMaster();
            RequestUserInsert objinput = new RequestUserInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objinput.objdbmlUser = objdbmlUser;

                if (objinput.objdbmlUser.FirstOrDefault().UserId > 0)
                {
                    RequestUserUpdate objUpdate = new RequestUserUpdate();
                    dbmlUserMasterUpdateReq obj = new dbmlUserMasterUpdateReq();
                    obj.UserId = objinput.objdbmlUser.FirstOrDefault().UserId;
                    obj.UserTypeParaId = objinput.objdbmlUser.FirstOrDefault().UserTypeParaId;
                    obj.FirstName = objinput.objdbmlUser.FirstOrDefault().FirstName;
                    obj.MiddleName = objinput.objdbmlUser.FirstOrDefault().MiddleName;
                    obj.LastName = objinput.objdbmlUser.FirstOrDefault().LastName;
                    obj.IPAddress = objinput.objdbmlUser.FirstOrDefault().IPAddress;
                    obj.Mobile = objinput.objdbmlUser.FirstOrDefault().Mobile;
                    obj.PasswordExpiryDate = objinput.objdbmlUser.FirstOrDefault().PasswordExpiryDate;
                    obj.SalutationParaId = objinput.objdbmlUser.FirstOrDefault().SalutationParaId;
                    obj.PermittedSuperstockistIds = objinput.objdbmlUser.FirstOrDefault().PermittedSuperstockistIds;
                    objUpdate.objdbmlUserMasterUpdateReq = obj;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objUpdate);

                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                    var response = client.PostAsync(apiUrl + "UserMasterUpdate", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlUserMaster>(responseBody);
                    intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                    strStatus = objdbmlUserMasterView.objdbmlStatus.Status;
                }
                else
                {

                    string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                    var response = client.PostAsync(apiUrl + "UserMasterInsert", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlUserMaster>(responseBody);
                    intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                    strStatus = objdbmlUserMasterView.objdbmlStatus.Status;
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objdbmlUserMasterView.objdbmlUserMasterView, UserList = objdbmlUserMasterView.objdbmlUserMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserMasterStatusUpdate(ObservableCollection<dbmlUserMaster> objdbmlUser)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlUserMaster objdbmlUserMasterView = new returndbmlUserMaster();
            RequestUserInsert objinput = new RequestUserInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objinput.objdbmlUser = objdbmlUser;
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterStatusUpdate", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlUserMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlUserMaster>(responseBody);
                intStatusId = objdbmlUserMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlUserMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objdbmlUserMasterView.objdbmlUserMasterView, UserList = objdbmlUserMasterView.objdbmlUserMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UserMasterGetAll()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlUserMaster objreturn = new returndbmlUserMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "UserMasterGetAll").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlUserMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlUserMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UserMasterDelete(int intUserId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlUserMaster objreturn = new returndbmlUserMaster();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intUserId);
                objinput.StringTwo = Convert.ToString(Session["UserId"]);
                objinput.StringThree = Convert.ToString(Session["PartyId"]);
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "UserMasterDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlUserMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlUserMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Country/State/City

        public ActionResult CountryStateCity()
        {
            return View();
        }

        #endregion

        #region CustomerMaster
        public ActionResult SaveCustomer(string PartyID, string PartyTypeID, string PartyTypeName, string PartyName, string PartyCode, string PriceGroup, string PanNumber,
            string GstNo, string PanEftDt, string GSTEftDt, string isRegstrGST, string TransName, string TransGST, int Freight, int CreditDays, CustomerAddressMaster[] addressArr)
        {
            DealerPortal_devEntities db = new DealerPortal_devEntities();

            string result = "";
            try
            {
                if (PartyTypeID != null && PartyTypeName != null && PartyCode != null)
                {

                    CustomerMaster model = new CustomerMaster();

                    model.PartyID = Convert.ToInt32(PartyID);
                    model.PartyCode = PartyCode.ToString();
                    model.SuperStockistID = Convert.ToInt32(Session["UserId"].ToString());
                    model.PartyGroupID = 0;
                    model.Party = PartyName;
                    model.ShortName = "";
                    model.PartyType = PartyTypeName;
                    //model.PartyCode = PartyCode;
                    model.VendorCode = "";
                    model.PriceGroupID = Convert.ToInt32(PriceGroup);
                    model.PANNO = PanNumber;
                    model.GSTNo = GstNo;
                    model.PANNoEffectiveDate = Convert.ToDateTime(PanEftDt);
                    //model.GSTNo = GstNo;
                    model.GSTEffectiveDate = Convert.ToDateTime(GSTEftDt);
                    model.IsUnregisteredforGST = Convert.ToBoolean(isRegstrGST);
                    model.Createdby = Convert.ToInt32(Session["UserId"].ToString());
                    model.CreatedDateTime = DateTime.Now;
                    model.Transporter = TransName;
                    model.TransporterGST = TransGST;
                    model.FreightId = Freight;
                    model.CreditDays = CreditDays;

                    db.CustomerMasters.Add(model);
                    db.SaveChanges();
                    int intPartyID = model.PartyID;
                    foreach (var item in addressArr)
                    {
                        CustomerAddressMaster CustAdrss = new CustomerAddressMaster();
                        CustAdrss.PartyID = intPartyID;
                        CustAdrss.AddressTypeID = Convert.ToInt32(item.AddressTypeID);
                        CustAdrss.Address = item.Address;
                        CustAdrss.City = item.City;
                        CustAdrss.State = item.State;
                        CustAdrss.Pin = item.Pin;
                        CustAdrss.Email = item.Email;
                        CustAdrss.Mobile = item.Mobile;
                        CustAdrss.CountryID = Convert.ToInt32(item.CountryID);
                        CustAdrss.StateID = Convert.ToInt32(item.StateID);

                        db.CustomerAddressMasters.Add(CustAdrss);
                    }
                    db.SaveChanges();
                    result = "Party Details updated successfully";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCity(string strSearch)
        {
            DealerPortal_devEntities db = new DealerPortal_devEntities();
            List<City> ObjList = new List<City>();
            //Note : you can bind same list from database  
            try
            {
                var studentList = db.CityMasters.Where(s => s.CityName.ToLower().Contains(strSearch)).ToList();
                //if (studentList.Count == 0)
                //{
                //    studentList.Add(new CityMaster { CityId = 0, CityName = "No Record Found" });
                //}
                if (studentList.Count > 0)
                {
                    foreach (CityMaster item in studentList)
                    {
                        ObjList.Add(new City() { CityId = Convert.ToInt32(item.CityId), CityName = item.CityName });
                    }
                }
                else
                {
                    ObjList.Add(new City() { CityId = 0, CityName = "No Record Found" });
                }
            }
            catch (Exception ex)
            {
                ObjList.Add(new City() { CityId = 0, CityName = "No Record Found" });
            }

            return Json(ObjList, JsonRequestBehavior.AllowGet);

        }
        public ActionResult FillSuperStockist()
        {
            List<dbmlTempPartyMaster> ObjList = new List<dbmlTempPartyMaster>();
            ObjList.Add(new dbmlTempPartyMaster() { PartyID = Convert.ToInt32(Session["PartyId"]), Party = Session["PartyName"].ToString() });
            return Json(ObjList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPartyType()
        {
            DealerPortal_devEntities db = new DealerPortal_devEntities();
            return Json(db.PartyTypes.Select(x => new
            {
                PartyTypeID = x.PartyTypeID,
                PartyTypeName = x.PartyType1
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getAddressType()
        {

            DealerPortal_devEntities db = new DealerPortal_devEntities();
            return Json(db.AddressTypeMasters.Select(x => new
            {
                AddressTypeID = x.AddressTypeID,
                AddressTypeName = x.AddressType
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountry()
        {

            DealerPortal_devEntities db = new DealerPortal_devEntities();
            return Json(db.CountryMasters.Select(x => new
            {
                CountryID = x.CountryID,
                Country = x.Country
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetState()
        {

            DealerPortal_devEntities db = new DealerPortal_devEntities();
            return Json(db.StateMasters.Select(x => new
            {
                StateID = x.StateID,
                CountryID = x.CountryID,
                State = x.State
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerMasterInsertUpdate(string PartyID, string PartyTypeID, string PartyTypeName, string PartyName, string PartyCode, string PriceGroup, string PanNumber,
            string GstNo, string PanEftDt, string GSTEftDt, string isRegstrGST, CustomerAddressMaster[] addressArr, string SalesPerson, string TransName, string TransGST, string Freight, string CreditDays)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlTempCustomer objdbmlTempCustomer = new returndbmlTempCustomer();
            returndbmlTempCustomerAddress objdbmlTempCustomerAddress = new returndbmlTempCustomerAddress();
            //TempCustomerMaster objInvUpd = new TempCustomerMaster();
            //RequestUserInsert objinput = new RequestUserInsert();
            int intStatusId = 99;
            int intPartyId = 0;
            string strStatus = "Invalid";
            try
            {
                RequestCustomerUpdate objUpdate = new RequestCustomerUpdate();
                dbmlCustomerUpdateReq obj = new dbmlCustomerUpdateReq();

                obj.PartyID = Convert.ToInt32(PartyID);
                obj.PartyCode = PartyCode.ToString();
                obj.SuperStockistID = Convert.ToInt32(Session["PartyId"].ToString());
                obj.PartyGroupID = 0;
                obj.Party = PartyName;
                obj.ShortName = "";
                obj.PartyType = PartyTypeName;
                obj.VendorCode = "";
                obj.PriceGroupID = Convert.ToInt32(PriceGroup);
                obj.PANNO = PanNumber;
                obj.GSTNo = GstNo;
                obj.PANNoEffectiveDate = (string.IsNullOrEmpty(PanEftDt)) ? DateTime.Now : Convert.ToDateTime(PanEftDt);
                obj.GSTEffectiveDate = (string.IsNullOrEmpty(GSTEftDt)) ? DateTime.Now : Convert.ToDateTime(GSTEftDt);
                obj.IsUnregisteredforGST = Convert.ToBoolean(isRegstrGST);
                obj.Createdby = Convert.ToInt32(Session["UserId"].ToString());
                obj.CreatedDateTime = DateTime.Now;

                obj.ZZPANNoEffectiveDate = (string.IsNullOrEmpty(PanEftDt)) ? "" : (PanEftDt);
                obj.ZZGSTEffectiveDate = (string.IsNullOrEmpty(GSTEftDt)) ? "" : (GSTEftDt);
                obj.SalesPerson = SalesPerson;
                obj.Transporter = TransName;
                obj.TransporterGST = TransGST;
                obj.FreightId = (string.IsNullOrEmpty(Freight)) ? 0 : Convert.ToInt32(Freight);
                obj.CreditDays = (string.IsNullOrEmpty(CreditDays)) ? 0 : Convert.ToInt32(CreditDays);

                objUpdate.objdbmlCustomerUpdateReq = obj;
                string inputJson = (new JavaScriptSerializer()).Serialize(objUpdate);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "CustomerMasterInsertUpdate", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlTempCustomer = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
                intStatusId = objdbmlTempCustomer.objdbmlStatus.StatusId;
                intPartyId = objdbmlTempCustomer.objdbmlStatus.StatusId;
                strStatus = objdbmlTempCustomer.objdbmlStatus.Status;
                if (intPartyId > 0 && addressArr != null)
                {
                    foreach (var item in addressArr)
                    {
                        RequestCustomerAddressUpdate objAUpdate = new RequestCustomerAddressUpdate();
                        dbmlCustomerAddressUpdateReq Aobj = new dbmlCustomerAddressUpdateReq();
                        Aobj.PartyAddressID = Convert.ToInt32(item.PartyAddressID);
                        Aobj.PartyID = intPartyId;
                        Aobj.AddressTypeID = Convert.ToInt32(item.AddressTypeID);
                        Aobj.Address = item.Address;
                        Aobj.City = item.City;
                        Aobj.State = item.State;
                        Aobj.Pin = item.Pin;
                        Aobj.Email = item.Email;
                        Aobj.Mobile = item.Mobile;
                        Aobj.CountryID = Convert.ToInt32(item.CountryID);
                        Aobj.StateID = Convert.ToInt32(item.StateID);

                        Aobj.Country = "";
                        Aobj.AddressType = "";
                        Aobj.ContactPersons = "";
                        Aobj.Tel = "0";
                        Aobj.Fax = "0";
                        Aobj.IsMain = false;
                        Aobj.Website = "";
                        Aobj.Cityid = item.Cityid == null ? 0 : Convert.ToInt32(item.Cityid);
                        Aobj.RealAddress = "";

                        objAUpdate.objdbmlCustomerAddressUpdateReq = Aobj;
                        string AinputJson = (new JavaScriptSerializer()).Serialize(objAUpdate);

                        HttpClient Aclient = new HttpClient();
                        HttpContent AinputContent = new StringContent(AinputJson, Encoding.UTF8, "application/json");
                        string AapiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                        var Aresponse = client.PostAsync(AapiUrl + "CustomerAddressMasterInsertUpdate", AinputContent).Result;
                        string AresponseBody = response.Content.ReadAsStringAsync().Result;
                        objdbmlTempCustomerAddress = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomerAddress>(responseBody);

                    }
                }

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
                strStatus = ex.Message + " " + ex.StackTrace;
            }
            return Json(new { Result = objdbmlTempCustomer.objdbmlTempCustomer, CustomerList = objdbmlTempCustomer.objdbmlTempCustomer, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult GetCustmerByUserId()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetCustmerByUserId?UserId=" + Session["UserId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCustomer, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerForCheck()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetCustomerForCheck?PartyId=" + Session["PartyId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCustomer, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GetCustmerAddress(int intPartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCustomerAddress objreturn = new returndbmlTempCustomerAddress();

            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetCustmerAddress?PartyId=" + intPartyId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomerAddress>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCustomerAddress, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCustomerMaster(int intPartyId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intPartyId);
                objinput.StringTwo = Convert.ToString(Session["UserId"]);
                objinput.StringThree = Convert.ToString(Session["PartyId"]);
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "DeleteCustomerMaster", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempCustomer, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerForAutoComplete(string strSearch)
        {

            returndbmlTempCustomer objreturn = new returndbmlTempCustomer();
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "GetCustmerBySuperStockistId?PartyId=" + Session["PartyId"]).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody);
            var CustomerList = objreturn.objdbmlTempCustomer.Where(s => s.Party.ToLower().Contains(strSearch) || s.PartyCode.ToLower().Contains(strSearch)).ToList();
            List<TempCustomerMaster> ObjList = new List<TempCustomerMaster>();
            //Note : you can bind same list from database  
            try
            {

                if (CustomerList.Count > 0)
                {
                    foreach (TempCustomerMaster item in CustomerList)
                    {
                        ObjList.Add(new TempCustomerMaster() { PartyID = Convert.ToInt32(item.PartyID), Party = item.Party, PartyCode = item.PartyCode });
                    }
                }
                else
                {
                    ObjList.Add(new TempCustomerMaster() { PartyID = 0, Party = "No Record Found", PartyCode = "No Record Found" });
                }
            }
            catch (Exception ex)
            {
                ObjList.Add(new TempCustomerMaster() { PartyID = 0, Party = "No Record Found", PartyCode = "No Record Found" });
            }


            return Json(ObjList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemForAutoComplete(string strSearch)
        {

            returnTempCustomerDiscountItemView objOL = new returnTempCustomerDiscountItemView();
            string apiUrl1 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client1 = new HttpClient();
            var response1 = client1.GetAsync(apiUrl1 + "GetItemsbySuperStockistId?PartyId=" + Session["PartyId"]).Result;
            string responseBody1 = response1.Content.ReadAsStringAsync().Result;
            objOL = (new JavaScriptSerializer()).Deserialize<returnTempCustomerDiscountItemView>(responseBody1);
            var ItemList = objOL.objTempCustomerDiscountItemView.Where(s => s.ZZProduct.ToLower().Contains(strSearch) || s.ZZItemCode.ToLower().Contains(strSearch)).ToList();
            List<TempCustomerDiscountItemView> ObjList = new List<TempCustomerDiscountItemView>();
            //Note : you can bind same list from database  
            try
            {

                if (ItemList.Count > 0)
                {
                    foreach (TempCustomerDiscountItemView item in ItemList)
                    {
                        ObjList.Add(new TempCustomerDiscountItemView() { ItemID = Convert.ToInt32(item.ItemID), ZZProduct = item.ZZProduct, ZZItemCode = item.ZZItemCode });
                    }
                }
                else
                {
                    ObjList.Add(new TempCustomerDiscountItemView() { ItemID = 0, ZZProduct = "No Record Found", ZZItemCode = "No Record Found" });
                }
            }
            catch (Exception ex)
            {
                ObjList.Add(new TempCustomerDiscountItemView() { ItemID = 0, ZZProduct = "No Record Found", ZZItemCode = "No Record Found" });
            }


            return Json(ObjList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadDiscountMaster(string SuperStockistId, string CustomerId, string ItemId, string PartyTypeId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returnTempDealerSalesDiscountMaster objreturn = new returnTempDealerSalesDiscountMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ReadDiscountMaster?SuperStockistId=" + SuperStockistId + "&CustomerId=" + CustomerId + "&ItemId=" + ItemId + "&PartyTypeId=" + PartyTypeId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returnTempDealerSalesDiscountMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }


            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objTempDealerSalesDiscountMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDiscountMaster(ObservableCollection<TempDealerSalesDiscountMaster> objdbmlDetail)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returnTempDealerSalesDiscountMaster objReturndbmlTempIA = new returnTempDealerSalesDiscountMaster();
            requestTempDealerSalesDiscountMaster objinput = new requestTempDealerSalesDiscountMaster();
            //int intItemAdjustmentId = objdbmlHeader.FirstOrDefault().ItemAdjustmentId;
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {

                foreach (var item in objdbmlDetail)
                {
                    item.CreatedBy = Convert.ToInt32(Session["UserId"]);
                    item.ZZValidTillDate = (item.ZZValidTillDate == null || item.ZZValidTillDate == "") ? "" : item.ZZValidTillDate;
                }

                objinput.objTempDealerSalesDiscountMasterReq = objdbmlDetail;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "SaveDiscountMaster", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objReturndbmlTempIA = (new JavaScriptSerializer()).Deserialize<returnTempDealerSalesDiscountMaster>(responseBody);
                intStatusId = objReturndbmlTempIA.objdbmlStatus.StatusId;
                strStatus = objReturndbmlTempIA.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultHeader = objReturndbmlTempIA.objTempDealerSalesDiscountMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #region Stock Adjustment

        public ActionResult StockAdjustment()
        {
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "ItemMasterGetAllForStockAdjustment?PartyID=" + Convert.ToString(Session["PartyId"])).Result;

            string responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemMaster>(responseBody);
            ViewBag.ItemListSA = objreturn.objdbmlItemMasterView.ToList();

            returndbmlOptionList objOL = new returndbmlOptionList();
            string apiUrl1 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client1 = new HttpClient();
            //var response1 = client1.GetAsync(apiUrl1 + "OptionListGetByParameterId?ParameterId=5").Result;
            var response1 = client1.GetAsync(apiUrl1 + "OptionListByStockAdjustmentType?ParameterId=5&AdType=3").Result;
            string responseBody1 = response1.Content.ReadAsStringAsync().Result;
            objOL = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody1);
            ViewBag.OptionListSA = objOL.objdbmlOptionList.ToList();

            returndbmlTempCustomer objreturn2 = new returndbmlTempCustomer();
            string apiUrl2 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client2 = new HttpClient();
            var response2 = client2.GetAsync(apiUrl + "GetCustomerForCheck?PartyId=" + Session["PartyId"]).Result;
            string responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlTempCustomer.ToList();
            ViewBag.FormDisplayTitle = "Stock Adjustment";
            return View();
        }

        public ActionResult SalesReturn()
        {
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "ItemMasterGetAllForStockAdjustment?PartyID=" + Convert.ToString(Session["PartyId"])).Result;

            string responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemMaster>(responseBody);
            ViewBag.ItemListSA = objreturn.objdbmlItemMasterView.ToList();

            returndbmlOptionList objOL = new returndbmlOptionList();
            string apiUrl1 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client1 = new HttpClient();
            var response1 = client1.GetAsync(apiUrl1 + "OptionListByStockAdjustmentType?ParameterId=5&AdType=1").Result;
            string responseBody1 = response1.Content.ReadAsStringAsync().Result;
            objOL = (new JavaScriptSerializer()).Deserialize<returndbmlOptionList>(responseBody1);
            ViewBag.OptionListSA = objOL.objdbmlOptionList.ToList();

            returndbmlTempCustomer objreturn2 = new returndbmlTempCustomer();
            string apiUrl2 = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client2 = new HttpClient();
            var response2 = client2.GetAsync(apiUrl + "GetCustomerForCheck?PartyId=" + Session["PartyId"]).Result;
            string responseBody2 = response2.Content.ReadAsStringAsync().Result;
            objreturn2 = (new JavaScriptSerializer()).Deserialize<returndbmlTempCustomer>(responseBody2);
            ViewBag.CustomerList = objreturn2.objdbmlTempCustomer.ToList();
            ViewBag.FormDisplayTitle = "Sales Return";
            return View("~/Views/Home/StockAdjustment.cshtml");
        }

        public ActionResult ItemAdjustmentUpsert(ObservableCollection<dbmlItemAdjustment> objdbmlHeader, ObservableCollection<dbmlItemAdjustmentDetail> objdbmlDetail)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlItemAdjustment objReturndbmlTempIA = new returndbmlItemAdjustment();
            requestdbmlItemAdjustment objinput = new requestdbmlItemAdjustment();
            int intItemAdjustmentId = objdbmlHeader.FirstOrDefault().ItemAdjustmentId;
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                foreach (var item in objdbmlHeader)
                {
                    if (intItemAdjustmentId == 0)
                    {
                        item.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        item.CreatedDate = DateTime.Now;
                        item.PartyId = Convert.ToInt32(Session["PartyId"]);
                    }
                    item.EditedBy = Convert.ToInt32(Session["UserId"]);
                    item.EditedDate = DateTime.Now;

                }
                foreach (var item in objdbmlDetail)
                {
                    if (intItemAdjustmentId == 0)
                    {
                        item.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        item.CreatedDate = DateTime.Now;
                    }
                    item.EditedBy = Convert.ToInt32(Session["UserId"]);
                    item.EditedDate = DateTime.Now;

                }

                objinput.objdbmlItemAdjustment = objdbmlHeader;
                objinput.objdbmlItemAdjustmentDetail = objdbmlDetail;

                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];

                if (intItemAdjustmentId == 0)
                {
                    var response = client.PostAsync(apiUrl + "ItemAdjustmentInsert", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objReturndbmlTempIA = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                    intStatusId = objReturndbmlTempIA.objdbmlStatus.StatusId;
                    strStatus = objReturndbmlTempIA.objdbmlStatus.Status;
                }
                else
                {
                    var response = client.PostAsync(apiUrl + "ItemAdjustmentEdit", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objReturndbmlTempIA = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                    intStatusId = objReturndbmlTempIA.objdbmlStatus.StatusId;
                    strStatus = objReturndbmlTempIA.objdbmlStatus.Status;
                }


            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultHeader = objReturndbmlTempIA.objdbmlItemAdjustment, ResultDetail = objReturndbmlTempIA.objdbmlItemAdjustmentDetail, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemAdjustmentGetByFromDateToDate(string FromDate, string ToDate, string AdjustTypeList)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemAdjustmentDateWIse objreturn = new returndbmlItemAdjustmentDateWIse();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemAdjustmentGetByFromDateToDate?FromDate=" + FromDate + "&ToDate=" + ToDate + "&SuperStockistID=" + Convert.ToInt32(Session["PartyId"]) + "&AdjustTypeList=" + AdjustTypeList).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustmentDateWIse>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }


            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlItemAdjustment, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemAdjustmentGetById(string ItemAdjustmentId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemAdjustment objreturn = new returndbmlItemAdjustment();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "ItemAdjustmentGetById?ItemAdjustmentId=" + ItemAdjustmentId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { ResultHeader = objreturn.objdbmlItemAdjustment, ResultDetail = objreturn.objdbmlItemAdjustmentDetail, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemAdjustmentDelete(int intItemAdjustmentId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemAdjustment objreturn = new returndbmlItemAdjustment();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intItemAdjustmentId);
                //objinput.StringTwo = Convert.ToString(Session["UserId"]);
                //objinput.StringThree = Convert.ToString(Session["PartyId"]);  
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "ItemAdjustmentDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemAdjustmentDetailDelete(int intItemAdjustmentDetailId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemAdjustment objreturn = new returndbmlItemAdjustment();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intItemAdjustmentDetailId);
                //objinput.StringTwo = Convert.ToString(Session["UserId"]);
                //objinput.StringThree = Convert.ToString(Session["PartyId"]);  
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "ItemAdjustmentDetailDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SalesInvoice
        public ActionResult SalesInvoice()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ViewData["FreightTypeList"] = GetFreightTypeMaster();
            ViewData["ChargeList"] = GetChargeMaster();
            ViewData["TCSChargeList"] = GetTCSChargeMaster();
            return View();
        }

        private object GetFreightTypeMaster()
        {
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlFreightTypeMaster objreturn = new returndbmlFreightTypeMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetFreightTypeMaster").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlFreightTypeMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlFreightTypeMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBuyersbyPartyID(string PartyID)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlCustomerMaster objreturn = new returndbmlCustomerMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetBuyersbyPartyID?PartyID=" + (Session["UserTypeId"].ToString() == "22" || Session["UserTypeId"].ToString() == "23" ? PartyID : Session["PartyId"])).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlCustomerMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlCustomerMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBuyersAddressbyPartyID(string PartyID, Char Type)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlCustomerAddressMaster objreturn = new returndbmlCustomerAddressMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetBuyersAddressbyPartyID?PartyID=" + PartyID).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlCustomerAddressMaster>(responseBody);

                if (Type == 'B')
                {
                    if (objreturn.objdbmlCustomerAddressMaster.Where(i => i.AddressTypeID == 1).Count() > 0)
                    {
                        intStatusId = objreturn.objdbmlStatus.StatusId;
                        strStatus = objreturn.objdbmlStatus.Status;
                    }
                    else
                    {
                        intStatusId = 99;
                        strStatus = "Billing address required";
                    }
                }
                else if (Type == 'C')
                {
                    if (objreturn.objdbmlCustomerAddressMaster.Where(i => i.AddressTypeID == 2).Count() > 0)
                    {
                        intStatusId = objreturn.objdbmlStatus.StatusId;
                        strStatus = objreturn.objdbmlStatus.Status;
                    }
                    else
                    {
                        intStatusId = 99;
                        strStatus = "Shipping address required";
                    }
                }

                //if (objreturn.objdbmlCustomerAddressMaster.Where(i => i.AddressTypeID == 1).Count() > 0 && objreturn.objdbmlCustomerAddressMaster.Where(i => i.AddressTypeID == 2).Count() > 0)
                //{
                //    intStatusId = objreturn.objdbmlStatus.StatusId;
                //    strStatus = objreturn.objdbmlStatus.Status;
                //}
                //else
                //{
                //    intStatusId = 99;
                //    strStatus = "Billing & Shipping address required";
                //}
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlCustomerAddressMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemsbyPartyID(string BuyerID, string InvoiceDate, string SuperStockist)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetItemsbyPartyID?PartyID=" + (Session["UserTypeId"].ToString() == "22" ? SuperStockist : Session["PartyId"]) + "&BuyerID=" + BuyerID + "&InvoiceDate=" + InvoiceDate).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempItemMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DealerSalesChallanInsert(dbmlDealerSalesChallan objdbmlHeader, ObservableCollection<dbmlDealerSalesChallanDetail> objdbmlDetail)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlDealerSalesChallan objReturn = new returndbmlDealerSalesChallan();
            requestdbmlDealerSalesChallan objinput = new requestdbmlDealerSalesChallan();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                //Session["IsInvoiceAuto"].ToString() code added on 21 dec2021 for Invoice No auto generation
                if (objdbmlHeader.InvoiceNo == "" && Session["IsInvoiceAuto"].ToString() == "False")
                    strStatus = "Invoice No. is required";
                //else if (objdbmlHeader.InvoiceNo.Length < 15)
                //    strStatus = "Invoice No. length should be 15";
                else if (objdbmlHeader.ZZInvoiceDate == null)
                    strStatus = "Invoice Date is required";
                else if (objdbmlHeader.CustomerID == 0)
                    strStatus = "Buyer is required";
                else
                {
                    if (objdbmlHeader.DealerSalesChallanID == 0)
                    {
                        objdbmlHeader.CreatedBy = Convert.ToInt32(Session["UserId"]);
                        objdbmlHeader.CreatedDate = DateTime.Now;
                        if (Session["UserTypeId"].ToString() != "22")
                            objdbmlHeader.SuperStockistID = Convert.ToInt32(Session["PartyId"]);
                    }
                    objdbmlHeader.EditedBy = Convert.ToInt32(Session["UserId"]);
                    objdbmlHeader.EditedDate = DateTime.Now;
                    objdbmlHeader.NetAmount = objdbmlDetail.Sum(i => i.NetValue);
                    objdbmlHeader.OtherCharges = 0;
                    objdbmlHeader.TaxAmount = objdbmlDetail.Sum(i => i.CGST + i.SGSTORUTGST + i.IGST);
                    objdbmlHeader.TotalAmount = objdbmlDetail.Sum(i => i.GrossValue);

                    if (objdbmlHeader.IsRoundOff == true)
                    {
                        objdbmlHeader.RoundedTotalAmount = Math.Round(Convert.ToDecimal(objdbmlHeader.TotalAmount), 0, MidpointRounding.AwayFromZero);
                        objdbmlHeader.RoundOff = objdbmlHeader.TotalAmount - objdbmlHeader.RoundedTotalAmount;
                    }
                    else
                    {
                        objdbmlHeader.RoundOff = null;
                        objdbmlHeader.RoundedTotalAmount = Convert.ToDecimal(objdbmlHeader.TotalAmount);
                    }
                    //Code Added For TCS
                    //objdbmlHeader
                    //Code Added For TCS

                    objinput.objdbmlDealerSalesChallan = objdbmlHeader;
                    objinput.objdbmlDealerSalesChallanDetail = objdbmlDetail;

                    string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                    //commented if condition since Add & Update isdone in the same SP
                    //...Else condition was already commented
                    //if (objdbmlHeader.DealerSalesChallanID == 0)
                    //{
                    var response = client.PostAsync(apiUrl + "DealerSalesChallanInsert", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    //objReturn = (new JavaScriptSerializer()).Deserialize<returndbmlDealerSalesChallan>(responseBody);
                    objReturn = (serializer).Deserialize<returndbmlDealerSalesChallan>(responseBody);
                    intStatusId = objReturn.objdbmlStatus.StatusId;
                    strStatus = objReturn.objdbmlStatus.Status;
                    //}
                    //else
                    //{
                    //    var response = client.PostAsync(apiUrl + "ItemAdjustmentEdit", inputContent).Result;
                    //    string responseBody = response.Content.ReadAsStringAsync().Result;
                    //    objReturndbmlTempIA = (new JavaScriptSerializer()).Deserialize<returndbmlItemAdjustment>(responseBody);
                    //    intStatusId = objReturndbmlTempIA.objdbmlStatus.StatusId;
                    //    strStatus = objReturndbmlTempIA.objdbmlStatus.Status;
                    //}
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { ResultHeader = objReturn.objdbmlDealerSalesChallan, ResultDetail = objReturn.objdbmlDealerSalesChallanDetail, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        private object GetChargeMaster()
        {
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlChargeMaster objreturn = new returndbmlChargeMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetChargeMaster").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlChargeMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlChargeMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DealerSalesChallanGetByFromDateToDate(string FromDate, string ToDate, string InvoiceNo, string SuperStockist)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returnDealerSalesChallanGetByFromDateToDateResult objreturn = new returnDealerSalesChallanGetByFromDateToDateResult();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "DealerSalesChallanGetByFromDateToDate?PartyID=" + (Session["UserTypeId"].ToString() == "22" ? SuperStockist : Session["PartyId"]) + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&InvoiceNo=" + InvoiceNo).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returnDealerSalesChallanGetByFromDateToDateResult>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objDealerSalesChallanGetByFromDateToDateResult, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DealerSalesChallanGetById(string DealerSalesChallanID)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlDealerSalesChallan objreturn = new returndbmlDealerSalesChallan();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "DealerSalesChallanGetById?DealerSalesChallanID=" + DealerSalesChallanID).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlDealerSalesChallan>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { ResultHeader = objreturn.objdbmlDealerSalesChallan, ResultDetail = objreturn.objdbmlDealerSalesChallanDetail, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalesInvoicePrint(string DealerSalesChallanID)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returnGetSalesInvoicePrintResult objreturn = new returnGetSalesInvoicePrintResult();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetSalesInvoicePrint?DealerSalesChallanID=" + DealerSalesChallanID).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returnGetSalesInvoicePrintResult>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { ResultHeader = objreturn.dtDealerSalesChallan.Replace('\\', '/').Replace("\r\n", "").Replace("\t", ""), ResultDetail = objreturn.dtDealerSalesChallanDetails.Replace('\\', '/').Replace("\r\n", "").Replace("\t", ""), ResultGST = objreturn.dtDealerSalesChallanGST.Replace('\\', '/').Replace("\r\n", "").Replace("\t", ""), Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);//ResultHeader = objreturn.objdbmlDealerSalesChallan, ResultDetail = objreturn.objdbmlDealerSalesChallanDetail, 
        }
        #endregion


        public ActionResult ViewNewsMnoaster()
        {
            DealerPortal_devEntities db = new DealerPortal_devEntities();
            var data = db.News.OrderByDescending(x => x.CreatedDate).Take(15).ToList().ToList();
            //                .ToList()
            //                .Select(x => Util.GetNews(x)).ToList();
            //var data = db.spGetNews_Result.OrderByDescending(x => x.CreatedDate)
            //                .Take(15)
            //                .ToList()
            //                .Select(x => Util.GetNews(x)).ToList();

            return View(data);
        }

        #region  News Master
        public ActionResult NewsMaster()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
            HttpClient client = new HttpClient();
            returndbmlParameter objreturn = new returndbmlParameter();
            string responseBody = "";

            var response = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=3").Result;
            responseBody = response.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterListNewsType = objreturn.objdbmlParameter.ToList();


            var response1 = client.GetAsync(apiUrl + "ParameterGetbyParameterTypeId?ParameterTypeId=2").Result;
            responseBody = response1.Content.ReadAsStringAsync().Result;
            objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlParameter>(responseBody);
            ViewBag.ParameterListSalutation = objreturn.objdbmlParameter.ToList();

            return View();
        }

        public ActionResult NewsMasterInsertUpdate(ObservableCollection<dbmlNewsMaster> objdbmlNews)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlNewsMaster objdbmlNewsMasterView = new returndbmlNewsMaster();
            RequestNewsInsert objinput = new RequestNewsInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objinput.objdbmlNews = objdbmlNews;

                if (objinput.objdbmlNews.FirstOrDefault().NewsId > 0)
                {
                    RequestNewsUpdate objUpdate = new RequestNewsUpdate();
                    dbmlNewsMasterUpdateReq obj = new dbmlNewsMasterUpdateReq();
                    obj.NewsId = objinput.objdbmlNews.FirstOrDefault().NewsId;
                    obj.NewsTitle = objinput.objdbmlNews.FirstOrDefault().NewsTitle;
                    obj.NewsDate = objinput.objdbmlNews.FirstOrDefault().NewsDate;
                    obj.Active = objinput.objdbmlNews.FirstOrDefault().Active;
                    objUpdate.objdbmlNewsMasterUpdateReq = obj;
                    string inputJson = (new JavaScriptSerializer()).Serialize(objUpdate);

                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                    var response = client.PostAsync(apiUrl + "NewsMasterUpdate", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objdbmlNewsMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlNewsMaster>(responseBody);
                    intStatusId = objdbmlNewsMasterView.objdbmlStatus.StatusId;
                    strStatus = objdbmlNewsMasterView.objdbmlStatus.Status;
                }
                else
                {

                    string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                    var response = client.PostAsync(apiUrl + "NewsMasterInsert", inputContent).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    objdbmlNewsMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlNewsMaster>(responseBody);
                    intStatusId = objdbmlNewsMasterView.objdbmlStatus.StatusId;
                    strStatus = objdbmlNewsMasterView.objdbmlStatus.Status;
                }
            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objdbmlNewsMasterView.objdbmlNewsMasterView, NewsList = objdbmlNewsMasterView.objdbmlNewsMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewsMasterStatusUpdate(ObservableCollection<dbmlNewsMaster> objdbmlNews)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            returndbmlNewsMaster objdbmlNewsMasterView = new returndbmlNewsMaster();
            RequestNewsInsert objinput = new RequestNewsInsert();
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                objinput.objdbmlNews = objdbmlNews;
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);

                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "NewsMasterStatusUpdate", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objdbmlNewsMasterView = (new JavaScriptSerializer()).Deserialize<returndbmlNewsMaster>(responseBody);
                intStatusId = objdbmlNewsMasterView.objdbmlStatus.StatusId;
                strStatus = objdbmlNewsMasterView.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                ViewData["MSG"] = ex.Message;
            }
            return Json(new { Result = objdbmlNewsMasterView.objdbmlNewsMasterView, NewsList = objdbmlNewsMasterView.objdbmlNewsMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult NewsMasterGetAll()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlNewsMaster objreturn = new returndbmlNewsMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "NewsMasterGetAll").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlNewsMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlNewsMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        public ActionResult NewsMasterDelete(int intNewsId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlNewsMaster objreturn = new returndbmlNewsMaster();
            dbmlCommon objinput = new dbmlCommon();
            try
            {
                objinput.StringOne = Convert.ToString(intNewsId);
                objinput.StringTwo = Convert.ToString(Session["UserId"]);
                objinput.StringThree = Convert.ToString(Session["PartyId"]);
                string inputJson = (new JavaScriptSerializer()).Serialize(objinput);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "NewsMasterDelete", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlNewsMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlNewsMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult SaveNewsData(string txtNewsTitle, string txtNewsDate, string chkActive)
        {
            //DealerPortal_devEntities db = new DealerPortal_devEntities();
            //var data = db.News.OrderByDescending(x => x.CreatedDate)
            //                .Take(4)
            //                .ToList()
            //                .Select(x => Util.GetNews(x)).ToList();

            return Content("");
        }

        private object GetTCSChargeMaster()
        {
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlChargeMaster objreturn = new returndbmlChargeMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetTCSChargeMaster").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlChargeMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlChargeMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        private object GetUOMMaster()
        {
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlUOMMaster objreturn = new returndbmlUOMMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetUOMMaster").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlUOMMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlUOMList, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddEditItemMaster()
        {
            ViewData["UOMList"] = GetUOMMaster();
            ViewData["HSNCodeList"] = GetHSNCodeMaster();
            return View();
        }


        private object GetHSNCodeMaster()
        {
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlHSNCodeMaster objreturn = new returndbmlHSNCodeMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetHSNCodeMaster").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlHSNCodeMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlHSNCodeList, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ItemMasterGetById(string ItemId)
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
                var response = client.GetAsync(apiUrl + "ItemMasterGetById?ItemId=" + ItemId).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { ResultDetail = objreturn.objdbmlItemMasterView, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        public ActionResult AddEditItemMasterForm(int intItemId, string ItemName, string ItemCode, int UOM, int HSNCode, double PackSize)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlItemMaster objreturn = new returndbmlItemMaster();
            dbmlTempItemInsertUpdateNew objinput = new dbmlTempItemInsertUpdateNew();
            requestdbmlTempItemInsertUpdateNew objReq = new requestdbmlTempItemInsertUpdateNew();
            try
            {
                objinput.ItemId = intItemId;
                objinput.ItemName = ItemName;
                objinput.OurName = ItemName;
                objinput.ItemCode = ItemCode;
                objinput.OurCode = ItemCode;
                objinput.HSNCodeId = Convert.ToInt32(HSNCode);
                objinput.PackingSize = Convert.ToDecimal(PackSize);
                objinput.UOMId = UOM;
                objinput.RetailCategoryId = 1446;
                objinput.Status = 2;
                objinput.IsActive = true;
                objinput.SuperStockistID = Convert.ToInt32(Session["PartyId"]);
                objinput.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                objReq.objdbmlTempItemInsertUpdateNew = objinput;

                //objinput.StringThree = Convert.ToString(Session["PartyId"]);  
                string inputJson = (new JavaScriptSerializer()).Serialize(objReq);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                var response = client.PostAsync(apiUrl + "AddEditItemMasterForm", inputContent).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSuperStockistLoginSessionDetails(string SuperStockist)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempLoginMaster objreturn = new returndbmlTempLoginMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetSuperStockistLoginSessionDetails?SuperStockist=" + SuperStockist).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempLoginMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;


            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempLoginMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        public ActionResult PartyMasterGetAllByUserId()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlPartyMaster objreturn = new returndbmlPartyMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "PartyMasterGetAllByUserId?UserId=" + Session["UserId"].ToString()).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlPartyMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlPartyMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        //Added on 08 March 2022 for multiple DPRate

        //[ValidateAntiForgeryToken]
        public ActionResult GetAllItemsDPRate(string VoucherDate, string ItemId)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetAllItemsDPRate?PartyID=" + Session["PartyId"] + "&ItemId=" + ItemId + "&VoucherDate=" + VoucherDate).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempItemMaster, ResultDP = objreturn.objdbmlTempItemMasterDPRate, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetItemsbyPartyIDMultipleDPRate(string BuyerID, string InvoiceDate, string SuperStockist)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempItemMaster objreturn = new returndbmlTempItemMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetItemsbyPartyIDMultipleDPRate?PartyID=" + (Session["UserTypeId"].ToString() == "22" ? SuperStockist : Session["PartyId"]) + "&BuyerID=" + BuyerID + "&InvoiceDate=" + InvoiceDate).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempItemMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            //return Json(new { Result = objreturn.objdbmlTempItemMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
            return Json(new { Result = objreturn.objdbmlTempItemMaster, ResultDP = objreturn.objdbmlTempItemMasterDPRate, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #region EInvoice
        public ActionResult EInvoice()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            DateTime NowDate = DateTime.Now;
            DateTime FromDate = DateTime.Now.AddMonths(-1);

            int FDay = FromDate.Day;
            int FMonth = FromDate.Month;
            int FYear = FromDate.Year;

            if (FDay < 10 && FMonth < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else if (FDay < 10)
            {
                ViewBag.ZZFromDate = "0" + FDay + "/" + FMonth + "/" + FYear;
            }
            else if (FMonth < 10)
            {
                ViewBag.ZZFromDate = FDay + "/" + "0" + FMonth + "/" + FYear;
            }
            else
            {
                ViewBag.ZZFromDate = FDay + "/" + FMonth + "/" + FYear;
            }

            int NDay = NowDate.Day;
            int NMonth = NowDate.Month;
            int NYear = NowDate.Year;

            if (NDay < 10 && NMonth < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else if (NDay < 10)
            {
                ViewBag.ZZNowDate = "0" + NDay + "/" + NMonth + "/" + NYear;
            }
            else if (NMonth < 10)
            {
                ViewBag.ZZNowDate = NDay + "/" + "0" + NMonth + "/" + NYear;
            }
            else
            {
                ViewBag.ZZNowDate = NDay + "/" + NMonth + "/" + NYear;
            }

            return View();
        }

        public ActionResult GetDocumentListForEnvoice(int SuperStockistID, string DocDate)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempEInvoice objreturn = new returndbmlTempEInvoice();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetDocumentListForEnvoice?DocDate=" + DocDate + "&PartyId=" + SuperStockistID).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempEInvoice>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlTempEInvoice, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAPIAuthDetailsForEInv()
        {
            returndbmlTempAPIAuthDetails objdbmlTempAPIAuthDetails = new returndbmlTempAPIAuthDetails();
            dbmlCommon objdbmlCommon = new dbmlCommon();
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlTempAPIAuthDetails objreturn = new returndbmlTempAPIAuthDetails();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetAPIAuthDetailsForEInv?PartyId=" + Session["PartyId"]).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlTempAPIAuthDetails>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;
                if (objreturn.objdbmlStatus.StatusId == 1)
                {
                    Session["APIAuthUrl"] = objreturn.objdbmlTempAPIAuthDetails[0].AuthUrl;
                    Session["APIBaseUrl"] = objreturn.objdbmlTempAPIAuthDetails[0].BaseURL;
                    Session["APIEwbByIRN"] = objreturn.objdbmlTempAPIAuthDetails[0].EwbByIRN;
                    Session["APICancelEwbUrl"] = objreturn.objdbmlTempAPIAuthDetails[0].CancelEwbUrl;
                    Session["APICancelIRNURL"] = objreturn.objdbmlTempAPIAuthDetails[0].CancelIRNURL;
                    Session["APIGetIRNDetails"] = objreturn.objdbmlTempAPIAuthDetails[0].GetIRNDetails;
                    Session["APIASPUserid"] = objreturn.objdbmlTempAPIAuthDetails[0].ASPUserid;
                    Session["APIASPPassword"] = objreturn.objdbmlTempAPIAuthDetails[0].ASPPassword;
                    Session["APIEWBGSTIN"] = objreturn.objdbmlTempAPIAuthDetails[0].EWBGSTIN;
                    Session["APIEInvUserId"] = objreturn.objdbmlTempAPIAuthDetails[0].EInvUserId;
                    Session["APIEInvPassword"] = objreturn.objdbmlTempAPIAuthDetails[0].EInvPassword;
                    Session["APIGSTTINNo"] = objreturn.objdbmlTempAPIAuthDetails[0].GSTTINNo;
                    Session["APIAuthToken"] = objreturn.objdbmlTempAPIAuthDetails[0].AuthToken;
                    Session["APIEInvoiceTokenExp"] = objreturn.objdbmlTempAPIAuthDetails[0].EWBAuthTokenValidityDateTime;
                    Session["APIKey"] = objreturn.objdbmlTempAPIAuthDetails[0].APIKey;
                    Session["SekKey"] = objreturn.objdbmlTempAPIAuthDetails[0].SekKey;
                }
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }

            return Json(new { Result = objreturn.objdbmlTempAPIAuthDetails, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAuthToken()
        {
            returndbmlAPIAuthResponse objdbmlAPIAuthResponse = new returndbmlAPIAuthResponse();
            returndbmlTempAPIAuthDetails objdbmlTempAPIAuthDetails = new returndbmlTempAPIAuthDetails();

            dbmlCommon objdbmlCommon = new dbmlCommon();
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlAPIAuthResponse objreturn = new returndbmlAPIAuthResponse();
            returndbmlTempAPIAuthDetails objreturn1 = new returndbmlTempAPIAuthDetails();

            try
            {
                HttpClient client = new HttpClient();
                string authAPIURL = Session["APIAuthUrl"] + "aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&user_name=" + Session["APIEInvUserId"] + "&eInvPwd=" + Session["APIEInvPassword"] + "";
                var response = client.GetAsync(authAPIURL).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic resJson = JObject.Parse(responseBody);

                int StatusId = Convert.ToInt32(resJson["Status"].Value);
                string ClientId = resJson["Data"].ClientId.Value;
                string UserName = resJson["Data"].UserName.Value;
                string AuthToken = resJson["Data"].AuthToken.Value;
                string Sek = resJson["Data"].Sek.Value;
                string TokenExpiry = resJson["Data"].TokenExpiry.Value;
                string ErrorDetails = resJson["ErrorDetails"].Value;
                string InfoDtls = resJson["InfoDtls"].Value;
                //string value2 = entity["key2"];
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlAPIAuthResponse>(responseBody);
                


                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client1 = new HttpClient();
                
                var response1 = client1.GetAsync(apiUrl + "UpdateEInvAuthTokenDetails?PartyId=" + Session["PartyId"] + "&GSTNo=" + Session["APIEWBGSTIN"] + "&AuthToken=" + AuthToken + "&Apikey=" + Session["APIKey"] + "&Sekey=" + Session["SekKey"] + "&Expdate=" + TokenExpiry).Result;
                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                objreturn1 = (new JavaScriptSerializer()).Deserialize<returndbmlTempAPIAuthDetails>(responseBody1);
                intStatusId = objreturn1.objdbmlStatus.StatusId;
                strStatus = objreturn1.objdbmlStatus.Status;
            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }

            return Json(new { Result = objreturn.objdbmlAPIAuthResponse, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetIRNForEnvoice(int SuperStockistID, int DSCId)
        {
            RespPlGenIRN objreturn2 = new RespPlGenIRN();
            ReqPlGenIRN jsonResResponsePI = new ReqPlGenIRN();
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            //returndbmlTempEInvoice objreturn = new returndbmlTempEInvoice();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetIRNForEnvoice?PartyId=" + SuperStockistID + "&DSChallanId=" + DSCId ).Result;
                string retResponse = response.Content.ReadAsStringAsync().Result;
                jsonResResponsePI = (new JavaScriptSerializer()).Deserialize<ReqPlGenIRN>(retResponse);
                //string responseBody = System.IO.File.ReadAllText(@"C:\Users\sajjan.SMARTECH\Desktop\EinvoiceSample.txt");
                //dynamic resJson = JObject.Parse(responseBody);
                intStatusId = 1;//resJson.StatusId;
                strStatus = "";//resJson.Status;
                string responseBody = JsonConvert.SerializeObject(jsonResResponsePI);
                if (intStatusId == 1)
                {
                    HttpClient client1 = new HttpClient();
                    //StringContent httpContent = new StringContent(resJson, Encoding.UTF8, "application/json");
                    HttpContent inputContent = new StringContent(responseBody, Encoding.UTF8, "application/json");
                    //HttpContent httpContent=new HttpContent(responseBody) 
                    var response1 = client1.PostAsync(Session["APIBaseUrl"] + "aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&AuthToken=" + Session["APIAuthToken"] + "&user_name=" + Session["APIEInvUserId"] + "&QrCodeSize=250", inputContent).Result;
                    
                    string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                    dynamic resJson1 = JObject.Parse(responseBody1);
                    //intStatusId = Convert.ToInt32(resJson1.status_cd.Value);
                    //strStatus = resJson1["error"].message.Value;

                    dynamic Errdata = resJson1["ErrorDetails"];
                    if (Errdata != null)
                    {
                        strStatus = resJson1.ErrorDetails[0].ErrorMessage.Value;
                    }
                    if (Convert.ToInt32(resJson1["Status"].Value) == 1)
                    {
                        //JArray jsonArray = JArray.Parse(responseBody1);
                        dynamic data = JObject.Parse(resJson1.Data.Value);
                        //var userObj = JObject.Parse(responseBody1);
                        //var userGuid = Convert.ToString(userObj["Data"]["AckNo"]);
                        //RespGenIRNInvData rt = JsonConvert.DeserializeObject<RespGenIRNInvData>(responseBody1);
                        //string ac = rt.AckNo.ToString();
                        int StatusId = Convert.ToInt32(resJson1["Status"].Value);
                        //string ErrorCode = resJson1["ErrorDetails"].ErrorCode.Value;
                        //string ErrorMessage = resJson1["ErrorDetails"].ErrorMessage.Value;
                        //string InfCd = resJson1["InfoDtls"].InfCd.Value;
                        string AckNo = data.AckNo.Value;
                        string AckDt = data.AckDt.Value;
                        string Irn = data.Irn.Value;
                        string JwtIssuer = data.JwtIssuer;
                        string QrCodeImage = data.QrCodeImage;
                        string SignQRCode = data.SignedQRCode;
                        //System.Drawing.Image QRimg = stringToImage(data.QrCodeImage);
                        //byte[] vByteimage = Encoding.Unicode.GetBytes(SignQRCode);
                        //MemoryStream ms = new MemoryStream(vByteimage);
                        //MemoryStream ms1 = new MemoryStream();

                        //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true, true);
                        byte[] vByteimage = Jwt2ByteArray(SignQRCode);
                        //image.Save(ms1, System.Drawing.Imaging.ImageFormat.Png);

                        string QRCodeBase64Text = Convert.ToBase64String(vByteimage);
                        Session["QRCodeBase64Text"] = Convert.ToBase64String(vByteimage);
                        //TempData["QRCodeBase64Text"] = Convert.ToBase64String(vByteimage);

                        Exception exobj = new Exception();
                        LogException(exobj, DSCId.ToString(), AckNo, AckDt, Irn, JwtIssuer, "", "", "", "Gen IRN", SignQRCode, "INV", QRCodeBase64Text);


                        QrCodeModel qr = new QrCodeModel();
                        qr.QrCodeBase64String= Convert.ToBase64String(vByteimage);
                        qr.DSCId = DSCId;
                        qr.AckDt = AckDt;
                        qr.AckNo = AckNo;
                        qr.Irn = Irn;
                        qr.vByteimage = vByteimage;
                        qr.SignQRCode = SignQRCode;
                        qr.DocType = "INV";

                        string inputJson = (new JavaScriptSerializer()).Serialize(qr);
                        HttpClient client2 = new HttpClient();
                        HttpContent inputContent2 = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        var response2 = client2.PostAsync(apiUrl + "UpdateEINVIRNDetails", inputContent2).Result;
                        string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                        objreturn2 = (new JavaScriptSerializer()).Deserialize<RespPlGenIRN>(responseBody2);
                        intStatusId = objreturn2.StatusId;
                        strStatus = objreturn2.Status;

                        //HttpClient client2 = new HttpClient();
                        //var response2 = client2.GetAsync(apiUrl + "UpdateEINVIRNDetails?DSCId=" + DSCId.ToString() + "&AckNo=" + AckNo + "&AckDt=" + AckDt + "&Irn=" + Irn + "&vByteimage=" + vByteimage + "&SignQRCode=" + SignQRCode + "&DocType=INV").Result;
                        //string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                        //objreturn2 = (new JavaScriptSerializer()).Deserialize<RespPlGenIRN>(responseBody2);
                        //intStatusId = objreturn2.StatusId;
                        //strStatus = objreturn2.Status;
                    }
                    else
                    {
                        intStatusId = 99;
                        //strStatus = Errdata.ErrorMessage.Value;
                    }
                }

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelIRN(int SuperStockistID, int DSCId, string IRNNo, string UserId)
        {
            RespPlCancelIRN objreturn = new RespPlCancelIRN();
            ReqPlCancelIRN objIRN = new ReqPlCancelIRN();
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                //string responseBody = @"{"Irn":'" + IRNNo + "','CnlRsn':'1','CnlRem':'Wrong entry'}";
                objIRN.Irn = IRNNo;
                objIRN.CnlRem = "Wrong entry";
                objIRN.CnlRsn = "1";

                string inputJson = (new JavaScriptSerializer()).Serialize(objIRN);
                intStatusId = 1; // resJson.StatusId;
                strStatus = ""; // resJson.Status;
                HttpClient client1 = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                var response1 = client1.PostAsync(Session["APICancelIRNURL"] + "aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&AuthToken=" + Session["APIAuthToken"] + "&user_name=" + Session["APIEInvUserId"], inputContent).Result;

                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                dynamic resJson1 = JObject.Parse(responseBody1);

                dynamic Errdata = resJson1["ErrorDetails"];
                if (Errdata != null)
                {
                    strStatus = resJson1.ErrorDetails[0].ErrorMessage.Value;
                }
                if (Convert.ToInt32(resJson1["Status"].Value) == 1)
                {
                    dynamic data = JObject.Parse(resJson1.Data.Value);
                    int StatusId = Convert.ToInt32(resJson1["Status"].Value);
                    string Irn = data.Irn.Value;
                    string CancelDate = data.CancelDate.Value;

                    Exception exobj = new Exception();
                    LogExceptioCancellation(exobj, DSCId.ToString(), IRNNo, "", CancelDate, "IRN", "INV");


                    RespPlCancelIRN ObjCanINR = new RespPlCancelIRN();
                    ObjCanINR.Irn = Irn;
                    ObjCanINR.CancelDate = CancelDate;
                    ObjCanINR.DSCId = DSCId.ToString();
                    ObjCanINR.DocType = "INV";
                    ObjCanINR.CancelReason = "Wrong Entry";
                    ObjCanINR.UserId = UserId;

                    string reqJson = (new JavaScriptSerializer()).Serialize(ObjCanINR);
                    HttpClient client2 = new HttpClient();
                    HttpContent inputContent2 = new StringContent(reqJson, Encoding.UTF8, "application/json");
                    var response2 = client2.PostAsync(apiUrl + "UpdateIRNOREwayCancellationDetails", inputContent2).Result;
                    string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                    objreturn = (new JavaScriptSerializer()).Deserialize<RespPlCancelIRN>(responseBody2);
                    intStatusId = objreturn.StatusId;
                    strStatus = objreturn.Status;
                }
                else
                {
                    intStatusId = 99;
                    //strStatus = Errdata.ErrorMessage.Value;
                }

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEWBForEnvoice(int SuperStockistID, int DSCId, string IRNNo)
        {
            RespPlGetEWBByIRN objreturn2 = new RespPlGetEWBByIRN();
            ReqPlGenEwbByIRN reqPlGenEwbByIRN = new ReqPlGenEwbByIRN();

            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            //returndbmlTempEInvoice objreturn = new returndbmlTempEInvoice();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetEWBForEnvoice?PartyId=" + SuperStockistID + "&DSChallanId=" + DSCId + "&IRNNo=" + IRNNo).Result;
                string retResponse = response.Content.ReadAsStringAsync().Result;
                reqPlGenEwbByIRN = (new JavaScriptSerializer()).Deserialize<ReqPlGenEwbByIRN>(retResponse);
                //string responseBody = System.IO.File.ReadAllText(@"C:\Users\sajjan.SMARTECH\Desktop\SampleJsonGenEwbByIRN.txt");
                //dynamic resJson = JObject.Parse(responseBody);
                intStatusId = 1;//resJson.StatusId;
                strStatus = "";//resJson.Status;
                string responseBody = JsonConvert.SerializeObject(reqPlGenEwbByIRN);
                if (intStatusId == 1)
                {
                    HttpClient client1 = new HttpClient();
                    //StringContent httpContent = new StringContent(resJson, Encoding.UTF8, "application/json");
                    HttpContent inputContent = new StringContent(responseBody, Encoding.UTF8, "application/json");
                    //HttpContent httpContent=new HttpContent(responseBody) 
                    var response1 = client1.PostAsync(Session["APIEwbByIRN"] + "aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&AuthToken=" + Session["APIAuthToken"] + "&user_name=" + Session["APIEInvUserId"], inputContent).Result;

                    string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                    dynamic resJson1 = JObject.Parse(responseBody1);
                    //intStatusId = Convert.ToInt32(resJson1.status_cd.Value);
                    //strStatus = resJson1["error"].message.Value;

                    dynamic Errdata = resJson1["ErrorDetails"];
                    if (Errdata != null)
                    {
                        strStatus = resJson1.ErrorDetails[0].ErrorMessage.Value;
                    }
                    if (Convert.ToInt32(resJson1["Status"].Value) == 1)
                    {
                        dynamic data = JObject.Parse(resJson1.Data.Value);
                        int StatusId = Convert.ToInt32(resJson1["Status"].Value);
                        string EwbNo = Convert.ToString(data.EwbNo.Value);
                        string EwbDt = data.EwbDt.Value;
                        string EwbValidTill = data.EwbValidTill.Value;

                        Exception exobj = new Exception();
                        LogException(exobj, DSCId.ToString(), "", "", IRNNo, "", EwbNo, EwbDt, EwbValidTill, "Gen EWB by IRN", "", "INV","");

                        RespPlGetEWBByIRN ObjEWB = new RespPlGetEWBByIRN();
                        ObjEWB.DSCId = DSCId;
                        ObjEWB.EwbNo = EwbNo;
                        ObjEWB.EwbDt = EwbDt;
                        ObjEWB.EwbValidTill = EwbValidTill;
                        ObjEWB.DocType = "INV";

                        string inputJson = (new JavaScriptSerializer()).Serialize(ObjEWB);
                        HttpClient client2 = new HttpClient();
                        HttpContent inputContent2 = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        var response2 = client2.PostAsync(apiUrl + "UpdateEINVEWBDetails", inputContent2).Result;
                        string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                        objreturn2 = (new JavaScriptSerializer()).Deserialize<RespPlGetEWBByIRN>(responseBody2);
                        intStatusId = objreturn2.StatusId;
                        strStatus = objreturn2.Status;
                    }
                    else
                    {
                        intStatusId = 99;
                        //strStatus = Errdata.ErrorMessage.Value;
                    }
                }

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CancelEWB(int SuperStockistID, int DSCId, string EWBNo, string UserId)
        {
            RespPlCancelEWB objreturn = new RespPlCancelEWB();
            ReqPlCancelEWB objEWB = new ReqPlCancelEWB();
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                //string responseBody = @"{"EWB":'" + EWBNo + "','CnlRsn':'1','CnlRem':'Wrong entry'}";
                objEWB.ewbNo = Convert.ToInt64(EWBNo);
                objEWB.cancelRmrk = "Cancelled the order";
                objEWB.cancelRsnCode = 2;

                string inputJson = (new JavaScriptSerializer()).Serialize(objEWB);
                intStatusId = 1; // resJson.StatusId;
                strStatus = ""; // resJson.Status;
                HttpClient client1 = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                var response1 = client1.PostAsync(Session["APICancelEWBURL"] + "action=CANEWB&aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&AuthToken=" + Session["APIAuthToken"] + "&user_name=" + Session["APIEInvUserId"], inputContent).Result;

                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                dynamic resJson1 = JObject.Parse(responseBody1);
                intStatusId = Convert.ToInt32(resJson1.status_cd.Value);
                strStatus = resJson1.error.message.Value;

                dynamic Errdata = resJson1["ErrorDetails"];
                if (Errdata != null)
                {
                    strStatus = resJson1.ErrorDetails[0].ErrorMessage.Value;
                }
                if (intStatusId == 1)
                {
                    dynamic data = JObject.Parse(resJson1.Data.Value);
                    int StatusId = Convert.ToInt32(resJson1["Status"].Value);
                    string EWB = data.EWB.Value;
                    string CancelDate = data.CancelDate.Value;

                    Exception exobj = new Exception();
                    LogExceptioCancellation(exobj, DSCId.ToString(), "", EWBNo, CancelDate, "EWB", "INV");


                    RespPlCancelEWB ObjCanEWB = new RespPlCancelEWB();
                    ObjCanEWB.ewayBillNo = EWB;
                    ObjCanEWB.cancelDate = CancelDate;
                    ObjCanEWB.DSCId = DSCId.ToString();
                    ObjCanEWB.DocType = "INV";
                    ObjCanEWB.CancelReason = "Cancelled the order";
                    ObjCanEWB.UserId = UserId;

                    string reqJson = (new JavaScriptSerializer()).Serialize(ObjCanEWB);
                    HttpClient client2 = new HttpClient();
                    HttpContent inputContent2 = new StringContent(reqJson, Encoding.UTF8, "application/json");
                    var response2 = client2.PostAsync(apiUrl + "UpdateEWBOREwayCancellationDetailsEWB", inputContent2).Result;
                    string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                    objreturn = (new JavaScriptSerializer()).Deserialize<RespPlCancelEWB>(responseBody2);
                    intStatusId = objreturn.StatusId;
                    strStatus = objreturn.Status;
                }
                else
                {
                    intStatusId = 99;
                    //strStatus = Errdata.ErrorMessage.Value;
                }

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateIRNForEnvoice(int SuperStockistID, int DSCId, string IRNNo)
        {
            RespPlGenIRN objreturn2 = new RespPlGenIRN();
            RespPlGenIRN objIRN = new RespPlGenIRN();
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                string AckNo = string.Empty; ;
                string AckDt = string.Empty;
                string Irn = string.Empty;
                string JwtIssuer = string.Empty;
                string QrCodeImage = string.Empty;
                string SignQRCode = string.Empty;

                
                HttpClient client1 = new HttpClient();
                var response1 = client1.GetAsync(Session["APIGetIRNDetails"] + IRNNo + "?aspid=" + Session["APIASPUserid"] + "&password=" + Session["APIASPPassword"] + "&Gstin=" + Session["APIEWBGSTIN"] + "&AuthToken=" + Session["APIAuthToken"] + "&user_name=" + Session["APIEInvUserId"] + "&ParseIrnResp=0").Result;

                string responseBody1 = response1.Content.ReadAsStringAsync().Result;
                dynamic resJson1 = JObject.Parse(responseBody1);

                dynamic Errdata = resJson1["ErrorDetails"];
                if (Errdata != null)
                {
                    strStatus = resJson1.ErrorDetails[0].ErrorMessage.Value;
                }
                if (Convert.ToInt32(resJson1["Status"].Value) == 1)
                {
                    dynamic data = JObject.Parse(resJson1.Data.Value);
                    int StatusId = Convert.ToInt32(resJson1["Status"].Value);
                    AckNo = Convert.ToString(data.AckNo.Value);
                    AckDt = data.AckDt.Value;
                    Irn = data.Irn.Value;
                    JwtIssuer = data.JwtIssuer;
                    QrCodeImage = data.QrCodeImage;
                    SignQRCode = data.SignedQRCode;
                    byte[] vByteimage = Jwt2ByteArray(SignQRCode);

                    string QRCodeBase64Text = Convert.ToBase64String(vByteimage);
                    Session["QRCodeBase64Text"] = Convert.ToBase64String(vByteimage);

                    Exception exobj = new Exception();
                    LogException(exobj, DSCId.ToString(), AckNo, AckDt, Irn, JwtIssuer, "", "", "", "Gen IRN", SignQRCode, "INV", QRCodeBase64Text);


                    QrCodeModel qr = new QrCodeModel();
                    qr.QrCodeBase64String = Convert.ToBase64String(vByteimage);
                    qr.DSCId = DSCId;
                    qr.AckDt = AckDt;
                    qr.AckNo = AckNo;
                    qr.Irn = Irn;
                    qr.vByteimage = vByteimage;
                    qr.SignQRCode = SignQRCode;
                    qr.DocType = "INV";

                    string inputJson2 = (new JavaScriptSerializer()).Serialize(qr);
                    HttpClient client2 = new HttpClient();
                    HttpContent inputContent2 = new StringContent(inputJson2, Encoding.UTF8, "application/json");
                    var response2 = client2.PostAsync(apiUrl + "UpdateEINVIRNDetails", inputContent2).Result;
                    string responseBody2 = response2.Content.ReadAsStringAsync().Result;
                    objreturn2 = (new JavaScriptSerializer()).Deserialize<RespPlGenIRN>(responseBody2);
                    intStatusId = objreturn2.StatusId;
                    strStatus = objreturn2.Status;
                }
                else
                {
                    intStatusId = 99;
                    //strStatus = Errdata.ErrorMessage.Value;
                }

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        public static byte[] Jwt2ByteArray(string JWTQRCode)
        {
            //var QrcodeContent = context.AllAttributes["content"].Value.ToString();
            //var alt = context.AllAttributes["alt"].Value.ToString();
            var width = 250; // width of the Qr Code    
            var height = 250; // height of the Qr Code    
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(JWTQRCode);
            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference    
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB    
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image    
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG    
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //output.TagName = "img";
                //output.Attributes.Clear();
                //output.Attributes.Add("width", width);
                //output.Attributes.Add("height", height);
                //output.Attributes.Add("alt", alt);
                //output.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
                byte[] vByteimage = ToByteArray(bitmap, System.Drawing.Imaging.ImageFormat.Png);

                return vByteimage;
            }


        }
        public static byte[] ToByteArray(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }


        #endregion


        #region For Generate Error Log File
        public static void LogException(Exception ex, string saleschallanid, string AcNo, string AcDt, string IRN, string JWTissuer, string billno, string billdate, string validupto, string Type, string QRCodeText, string DocType, string qrcodebas64text)
        {
            string dir = "";
            StreamWriter logfile;
            dir = "C:\\ErrorLogFile\\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            logfile = new StreamWriter(dir + "\\EINVErrorFile" + DateTime.Now.ToString("ddMMMyyyy") + ".txt", true);
            try
            {


                if (Directory.Exists(dir))
                {
                    logfile.WriteLine("---------------------------------------------------------");
                    logfile.WriteLine(DateTime.Now.ToString());

                    logfile.WriteLine("Type = " + Type + " - DocType" + DocType);
                    logfile.WriteLine();
                    if (DocType == "INV")
                        logfile.WriteLine("SaleschallanId = " + saleschallanid);
                    if (DocType == "DBN")
                        logfile.WriteLine("DebitNoteID = " + saleschallanid);
                    if (DocType == "CDN")
                        logfile.WriteLine("CreditNoteID = " + saleschallanid);
                    logfile.WriteLine();
                    logfile.WriteLine("AckNo = " + AcNo);
                    logfile.WriteLine();
                    logfile.WriteLine("AckDt = " + AcDt);
                    logfile.WriteLine();
                    logfile.WriteLine("IRN = " + IRN);
                    logfile.WriteLine();
                    logfile.WriteLine("QRCodeText = " + QRCodeText);
                    logfile.WriteLine();

                    logfile.WriteLine("EwayBillNo = " + billno);
                    logfile.WriteLine();
                    logfile.WriteLine("EwayBillDate = " + billdate);
                    logfile.WriteLine();
                    logfile.WriteLine("ValidUpto = " + validupto);
                    logfile.WriteLine();
                    logfile.WriteLine("QRCodeBase64Text = " + qrcodebas64text);
                    logfile.WriteLine();

                    logfile.WriteLine();
                    logfile.WriteLine(ex.Message);
                    logfile.WriteLine();
                    if (!string.IsNullOrEmpty(ex.Source))
                    {
                        logfile.WriteLine(ex.Source.ToString());
                        logfile.WriteLine(ex.GetBaseException().ToString());
                    }
                    logfile.WriteLine("---------------------------------------------------------");
                }
                logfile.Close();
            }
            catch (Exception e)
            {
                string str = e.ToString();
                logfile.Close();
            }
        }

        public static void LogExceptioCancellation(Exception ex, string saleschallanid, string IrnNo, string EWB, string CancelDate, string Type, string DocType)
        {
            string dir = "";
            StreamWriter logfile;
            dir = "C:\\ErrorLogFile\\";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            logfile = new StreamWriter(dir + "\\EINVCancellationErrorFile" + DateTime.Now.ToString("ddMMMyyyy") + ".txt", true);
            try
            {


                if (Directory.Exists(dir))
                {
                    logfile.WriteLine("---------------------------------------------------------");
                    logfile.WriteLine(DateTime.Now.ToString());
                    logfile.WriteLine("Cancellation Type = " + Type + " - DocType" + DocType);
                    logfile.WriteLine();
                    if (DocType == "INV")
                        logfile.WriteLine("SaleschallanId = " + saleschallanid);
                    if (DocType == "DBN")
                        logfile.WriteLine("DebitNoteID = " + saleschallanid);
                    if (DocType == "CDN")
                        logfile.WriteLine("CreditNoteID = " + saleschallanid);
                    logfile.WriteLine();
                    logfile.WriteLine("EWB No = " + EWB);
                    logfile.WriteLine();
                    logfile.WriteLine("IRN = " + IrnNo);
                    logfile.WriteLine();

                    logfile.WriteLine("Cancelattiondate = " + CancelDate);
                    logfile.WriteLine();
                    logfile.WriteLine();
                    logfile.WriteLine(ex.Message);
                    logfile.WriteLine();
                    if (!string.IsNullOrEmpty(ex.Source))
                    {
                        logfile.WriteLine(ex.Source.ToString());
                        logfile.WriteLine(ex.GetBaseException().ToString());
                    }
                    logfile.WriteLine("---------------------------------------------------------");
                }
                logfile.Close();
            }
            catch (Exception e)
            {
                string str = e.ToString();
                logfile.Close();
            }
        }
        #endregion

        #region PurchaseReturn
        public ActionResult PurchaseReturn()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            ViewData["FreightTypeList"] = GetFreightTypeMaster();
            ViewData["ChargeList"] = GetChargeMaster();
            ViewData["TCSChargeList"] = GetTCSChargeMaster();
            return View();
        }


        public ActionResult GetPurchaseReturnParty()
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returndbmlCustomerMaster objreturn = new returndbmlCustomerMaster();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "GetPurchaseReturnParty").Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returndbmlCustomerMaster>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objdbmlCustomerMaster, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DealerSalesChallanPurchaseReturnGetByFromDateToDate(string FromDate, string ToDate, string InvoiceNo, string SuperStockist)
        {
            if (Session["UserId"] == null)
            {
                return Json(new { StatusId = 99, Status = "Your session has been expired, please login again." }, JsonRequestBehavior.AllowGet);
            }
            int intStatusId = 99;
            string strStatus = "Invalid";
            returnDealerSalesChallanGetByFromDateToDateResult objreturn = new returnDealerSalesChallanGetByFromDateToDateResult();
            try
            {
                string apiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"];
                HttpClient client = new HttpClient();
                var response = client.GetAsync(apiUrl + "DealerSalesChallanPurchaseReturnGetByFromDateToDate?PartyID=" + (Session["UserTypeId"].ToString() == "22" ? SuperStockist : Session["PartyId"]) + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&InvoiceNo=" + InvoiceNo).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                objreturn = (new JavaScriptSerializer()).Deserialize<returnDealerSalesChallanGetByFromDateToDateResult>(responseBody);
                intStatusId = objreturn.objdbmlStatus.StatusId;
                strStatus = objreturn.objdbmlStatus.Status;

            }
            catch (Exception ex)
            {
                strStatus = ex.Message;
            }
            return Json(new { Result = objreturn.objDealerSalesChallanGetByFromDateToDateResult, Status = strStatus, StatusId = intStatusId }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

