using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;


namespace DealerPortal.Models
{
    public class returndbmlStatus
    {
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #region Retail Category

    public class returndbmlRetailCategoryMaster
    {
        public ObservableCollection<dbmlRetailCategoryMaster> objdbmlRetailCategoryMaster = new ObservableCollection<dbmlRetailCategoryMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region MRP Category

    public class returndbmlMRPCategoryMaster
    {
        public ObservableCollection<dbmlMRPCategoryMaster> objdbmlMRPCategoryMaster = new ObservableCollection<dbmlMRPCategoryMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region Item

    public class returndbmlItemDetailView
    {

        public ObservableCollection<dbmlTempItemMaster> objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>();
        public ObservableCollection<dbmlTempCartView> objdbmlTempCartView = new ObservableCollection<dbmlTempCartView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlItemMaster
    {
        public ObservableCollection<dbmlItemMasterView> objdbmlItemMasterView = new ObservableCollection<dbmlItemMasterView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class requestdbmlTempItemInsertUpdate
    {
        public ObservableCollection<dbmlTempItemInsertUpdate> objdbmlTempItemInsertUpdate = new ObservableCollection<dbmlTempItemInsertUpdate>();
    }

    public class requestdbmlTempItemInsertUpdateNew
    {
        public dbmlTempItemInsertUpdateNew objdbmlTempItemInsertUpdateNew = new dbmlTempItemInsertUpdateNew();
    }
    #endregion

    #region Cart

    public class RequestCartInsert
    {
        public ObservableCollection<dbmlCart> objdbmlCart = new ObservableCollection<dbmlCart>();
        public dbmlCommon objdbmlCommon = new dbmlCommon();

    }

    public class returndbmlTempCartMaster
    {
        public ObservableCollection<dbmlTempCartMaster> objdbmlTempCartMaster = new ObservableCollection<dbmlTempCartMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returndbmlTempCustomer
    {
       public ObservableCollection<TempCustomerMaster> objdbmlTempCustomer = new ObservableCollection<TempCustomerMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returndbmlTempCustomerAddress
    {
        public ObservableCollection<TempCustomerAddressMaster> objdbmlTempCustomerAddress = new ObservableCollection<TempCustomerAddressMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class RequestCustomerUpdate
    {
        public dbmlCustomerUpdateReq objdbmlCustomerUpdateReq = new dbmlCustomerUpdateReq();
    }
    public class RequestCustomerAddressUpdate
    {
        public dbmlCustomerAddressUpdateReq objdbmlCustomerAddressUpdateReq = new dbmlCustomerAddressUpdateReq();
    }
    public class returnTempCustomerDiscountItemView
    {
        public ObservableCollection<TempCustomerDiscountItemView> objTempCustomerDiscountItemView = new ObservableCollection<TempCustomerDiscountItemView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returnTempDealerSalesDiscountMaster
    {
        public ObservableCollection<TempDealerSalesDiscountMaster> objTempDealerSalesDiscountMaster = new ObservableCollection<TempDealerSalesDiscountMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class requestTempDealerSalesDiscountMaster
    {
        public ObservableCollection<TempDealerSalesDiscountMaster> objTempDealerSalesDiscountMasterReq = new ObservableCollection<TempDealerSalesDiscountMaster>();
        public ObservableCollection<dbmlItemAdjustmentDetail> objdbmlItemAdjustmentDetail = new ObservableCollection<dbmlItemAdjustmentDetail>();
    }
    #endregion

    #region Order

    public class returndbmlTempOrderHeaderId
    {
        public ObservableCollection<dbmlTempOrderHeaderId> objdbmlTempOrderHeaderId = new ObservableCollection<dbmlTempOrderHeaderId>();
        public ObservableCollection<dbmlTempOrderHeaderDetailId> objdbmlTempOrderHeaderDetailId = new ObservableCollection<dbmlTempOrderHeaderDetailId>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlTempOrderHistory
    {
        public ObservableCollection<dbmlTempOrderHistory> objdbmlTempOrderHistory = new ObservableCollection<dbmlTempOrderHistory>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlTempOrderDetail
    {
        public ObservableCollection<dbmlTempOrderDetail> objdbmlTempOrderDetail = new ObservableCollection<dbmlTempOrderDetail>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class RequestOrderInsert
    {
        public ObservableCollection<dbmlTempCartMaster> objdbmlTempCartMaster = new ObservableCollection<dbmlTempCartMaster>();
        public dbmlCommon objdbmlCommon = new dbmlCommon();
    }

    #endregion

    #region Login

    public class returndbmlTempLoginMaster
    {
        public ObservableCollection<dbmlTempLoginMaster> objdbmlTempLoginMaster = new ObservableCollection<dbmlTempLoginMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region User 

    public class returndbmlUserMaster
    {
        public ObservableCollection<dbmlUserMasterView> objdbmlUserMasterView = new ObservableCollection<dbmlUserMasterView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class RequestUserInsert
    {
        public ObservableCollection<dbmlUserMaster> objdbmlUser = new ObservableCollection<dbmlUserMaster>();
    }

    public class RequestUserUpdate
    {
        public dbmlUserMasterUpdateReq objdbmlUserMasterUpdateReq = new dbmlUserMasterUpdateReq();
    }

    #endregion

    #region Parameter

    public class returndbmlParameter
    {
        public ObservableCollection<dbmlParameter> objdbmlParameter = new ObservableCollection<dbmlParameter>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region Party

    public class requestdbmlTempPartyMaster
    {
        public ObservableCollection<dbmlTempPartyMaster> objdbmlTempPartyMaster = new ObservableCollection<dbmlTempPartyMaster>();
    }

    public class returndbmlTempPartyMasterIdentity
    {
        public ObservableCollection<dbmlTempPartyMasterIdentity> objdbmlTempPartyMasterIdentity = new ObservableCollection<dbmlTempPartyMasterIdentity>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlPartyMaster
    {
        public ObservableCollection<dbmlTempPartyMaster> objdbmlPartyMaster = new ObservableCollection<dbmlTempPartyMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region PartyType

    public class returndbmlPartyType
    {
        public ObservableCollection<dbmlPartyType> objdbmlPartyMaster = new ObservableCollection<dbmlPartyType>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region AddressType

    public class returndbmlAddressType
    {
        public ObservableCollection<dbmlAddressTypeMaster> objdbmlAddressType = new ObservableCollection<dbmlAddressTypeMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region State 

    public class requestdbmlTempStateMaster
    {
        public ObservableCollection<dbmlTempStateMaster> objdbmlTempStateMaster = new ObservableCollection<dbmlTempStateMaster>();
    }

    #endregion

    #region City 

    public class requestdbmlTempCityMaster
    {
        public ObservableCollection<dbmlTempCityMaster> objdbmlTempCityMaster = new ObservableCollection<dbmlTempCityMaster>();
    }

    #endregion

    #region Country 

    public class requestdbmlTempCountryMaster
    {
        public ObservableCollection<dbmlTempCountryMaster> objdbmlTempCountryMaster = new ObservableCollection<dbmlTempCountryMaster>();
    }

    #endregion

    #region Sales Order

    public class requestdbmlSalesOrder
    {
        public ObservableCollection<dbmlTempSalesOrder> objdbmlTempSalesOrder = new ObservableCollection<dbmlTempSalesOrder>();
        public ObservableCollection<dbmlTempSalesOrderItem> objdbmlTempSalesOrderItem = new ObservableCollection<dbmlTempSalesOrderItem>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class ReturndbmlTempOrderNotInserted
    {
        public ObservableCollection<dbmlTempOrderNotInserted> objdbmlTempOrderNotInserted = new ObservableCollection<dbmlTempOrderNotInserted>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region Sales Challan

    public class requestdbmlsalesChallan
    {
        public ObservableCollection<dbmlSalesChallan> objdbmlSalesChallan = new ObservableCollection<dbmlSalesChallan>();
        public ObservableCollection<dbmlSalesChallanDetail> objdbmlSalesChallanDetail = new ObservableCollection<dbmlSalesChallanDetail>();
    }

    public class ReturndbmlTempSalesChallan
    {
        public ObservableCollection<dbmlTempSalesChallan> objdbmlTempSalesChallan = new ObservableCollection<dbmlTempSalesChallan>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class ReqSalesChallanUpdt
    {
        public ObservableCollection<dbmlSalesChallanUpdtReq> objdbmlSalesChallanUpdtReq = new ObservableCollection<dbmlSalesChallanUpdtReq>();
    }

    public class ReturnSalesChallanDetail
    {
        public ObservableCollection<dbmlSalesChallanDetailView> objdbmlSalesChallanDetailView = new ObservableCollection<dbmlSalesChallanDetailView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region MasterUpdate

    public class returndbmlMasterUpdate
    {
        public ObservableCollection<dbmlMasterUpdate> objdbmlMasterUpdate = new ObservableCollection<dbmlMasterUpdate>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    public class ReturndbmlTempIPL
    {
        public ObservableCollection<dbmlTempIPL> objdbmlTempIPLLinked = new ObservableCollection<dbmlTempIPL>();
        public ObservableCollection<dbmlTempIPL> objdbmlTempIPLNotLinked = new ObservableCollection<dbmlTempIPL>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class RequestdbmlItemPartyLinkReq
    {
        public ObservableCollection<dbmlItemPartyLinkReq> objdbmlItemPartyLinkReq = new ObservableCollection<dbmlItemPartyLinkReq>();
    }

    #region Itemledger

    public class ReturndbmlItemLedger
    {
        public ObservableCollection<dbmlItemLedger> objdbmlItemLedger = new ObservableCollection<dbmlItemLedger>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class RequestItemLedgerInsert
    {
        public ObservableCollection<dbmlItemLedger> objdbmlItemLedgerreq = new ObservableCollection<dbmlItemLedger>();
    }

    #endregion

    #region YearMaster
    public class returndbmlYearMaster
    {
        public ObservableCollection<dbmlTempYearMaster> objdbmlYearMaster = new ObservableCollection<dbmlTempYearMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region CurrencyMaster
    public class returndbmlCurrencyMaster
    {
        public ObservableCollection<dbmlTempCurrencyMaster> objdbmlCurrencyMaster = new ObservableCollection<dbmlTempCurrencyMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region OptionList
    public class returndbmlOptionList
    {
        public ObservableCollection<dbmlOptionList> objdbmlOptionList = new ObservableCollection<dbmlOptionList>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region Stock Adjustment
    public class returndbmlItemAdjustment
    {
        public ObservableCollection<dbmlItemAdjustment> objdbmlItemAdjustment = new ObservableCollection<dbmlItemAdjustment>();
        public ObservableCollection<dbmlItemAdjustmentDetail> objdbmlItemAdjustmentDetail = new ObservableCollection<dbmlItemAdjustmentDetail>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }


    public class requestdbmlItemAdjustment
    {
        public ObservableCollection<dbmlItemAdjustment> objdbmlItemAdjustment = new ObservableCollection<dbmlItemAdjustment>();
        public ObservableCollection<dbmlItemAdjustmentDetail> objdbmlItemAdjustmentDetail = new ObservableCollection<dbmlItemAdjustmentDetail>();
    }

    public class returndbmlItemAdjustmentDateWIse
    {
        public ObservableCollection<dbmlItemAdjustmentDateWIse> objdbmlItemAdjustment = new ObservableCollection<dbmlItemAdjustmentDateWIse>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    public class requestdbmlTempRetailCategoryMaster
    {
        public ObservableCollection<dbmlTempItemTypeMaster> objdbmlTempItemTypeMaster = new ObservableCollection<dbmlTempItemTypeMaster>();
    }

    public class requestdbmlTempHSNCodeMaster
    {
        public ObservableCollection<dbmlTempHSNCodeMaster> objdbmlTempHSNCodeMaster = new ObservableCollection<dbmlTempHSNCodeMaster>();
    }

    public class requestdbmlTempDiscountMaster
    {
        public ObservableCollection<dbmlTempDiscountMaster> objdbmlTempDiscountMaster = new ObservableCollection<dbmlTempDiscountMaster>();
    }

    public class requestdbmlTempPriceListMaster
    {
        public ObservableCollection<dbmlTempPriceListMaster> objdbmlTempPriceListMaster = new ObservableCollection<dbmlTempPriceListMaster>();
    }
    public class requestdbmlTempRegionMaster
    {
        public ObservableCollection<dbmlTempRegionMaster> objdbmlTempRegionMaster = new ObservableCollection<dbmlTempRegionMaster>();
    }
    public class requestdbmlTempSalesZoneMaster
    {
        public ObservableCollection<dbmlTempSalesZoneMaster> objdbmlTempSalesZoneMaster = new ObservableCollection<dbmlTempSalesZoneMaster>();
    }
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }

    }

    #region SalesInvoice
    public class returndbmlCustomerMaster
    {
        public ObservableCollection<dbmlCustomerMaster> objdbmlCustomerMaster = new ObservableCollection<dbmlCustomerMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlFreightTypeMaster
    {
        public ObservableCollection<dbmlFreightTypeMaster> objdbmlFreightTypeMaster = new ObservableCollection<dbmlFreightTypeMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlCustomerAddressMaster
    {
        public ObservableCollection<dbmlCustomerAddressMaster> objdbmlCustomerAddressMaster = new ObservableCollection<dbmlCustomerAddressMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlTempItemMaster
    {
        public ObservableCollection<dbmlTempItemMaster> objdbmlTempItemMaster = new ObservableCollection<dbmlTempItemMaster>();
        public ObservableCollection<dbmlTempItemMasterDPRate> objdbmlTempItemMasterDPRate = new ObservableCollection<dbmlTempItemMasterDPRate>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlDealerSalesChallan
    {
        public dbmlDealerSalesChallan objdbmlDealerSalesChallan = new dbmlDealerSalesChallan();
        public ObservableCollection<dbmlDealerSalesChallanDetail> objdbmlDealerSalesChallanDetail = new ObservableCollection<dbmlDealerSalesChallanDetail>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class requestdbmlDealerSalesChallan
    {
        public dbmlDealerSalesChallan objdbmlDealerSalesChallan = new dbmlDealerSalesChallan();
        public ObservableCollection<dbmlDealerSalesChallanDetail> objdbmlDealerSalesChallanDetail = new ObservableCollection<dbmlDealerSalesChallanDetail>();
    }
    public class returndbmlChargeMaster
    {
        public ObservableCollection<dbmlChargeMaster> objdbmlChargeMaster = new ObservableCollection<dbmlChargeMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returnDealerSalesChallanGetByFromDateToDateResult
    {
        public ObservableCollection<DealerSalesChallanGetByFromDateToDateResult> objDealerSalesChallanGetByFromDateToDateResult = new ObservableCollection<DealerSalesChallanGetByFromDateToDateResult>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returnGetSalesInvoicePrintResult
    {
        public String dtDealerSalesChallan;
        public String dtDealerSalesChallanDetails;
        public String dtDealerSalesChallanGST;
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region  Stock Ledger Report
    public class requestdbmlItemLedgerReport
    {
        public ObservableCollection<dbmlItemLedgerReport> objdbmlItemLedgerReport = new ObservableCollection<dbmlItemLedgerReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class requestdbmlitemLedgerDetails
    {
        public ObservableCollection<dbmlitemLedgerDetails> objdbmlitemLedgerDetails = new ObservableCollection<dbmlitemLedgerDetails>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }


    #endregion


    #region  Stock Ledger Valuation Report
    public class requestdbmlItemLedgerValuationReport
    {
        public ObservableCollection<dbmlItemLedgerValuationReport> objdbmlItemLedgerValuationReport = new ObservableCollection<dbmlItemLedgerValuationReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    //public class requestdbmlitemLedgerValuationDetails
    //{
    //    public ObservableCollection<dbmlitemLedgerValuationDetails> objdbmlitemLedgerDetails = new ObservableCollection<dbmlitemLedgerValuationDetails>();

    //    public dbmlStatus objdbmlStatus = new dbmlStatus();
    //}


    #endregion

    #region News 

    public class returndbmlNewsMaster
    {
        public ObservableCollection<dbmlNewsMasterView> objdbmlNewsMasterView = new ObservableCollection<dbmlNewsMasterView>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class RequestNewsInsert
    {
        public ObservableCollection<dbmlNewsMaster> objdbmlNews = new ObservableCollection<dbmlNewsMaster>();
    }

    public class RequestNewsUpdate
    {
        public dbmlNewsMasterUpdateReq objdbmlNewsMasterUpdateReq = new dbmlNewsMasterUpdateReq();
    }

    #endregion

    #region  Product Wise Summary Report
    public class requestdbmlSalesReport
    {
        public ObservableCollection<dbmlSalesReport> objdbmlSalesReport = new ObservableCollection<dbmlSalesReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion
    // sandeep 9-8-2022
    #region  Invoice Statistics Data Report
    public class requestdbmlStatisticsReport
    {
        public ObservableCollection<dbmlStatisticsReport> objdbmlStatisticsReport = new ObservableCollection<dbmlStatisticsReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion
    // sandeep 9-8-2022
    #region 
    public class requestdbmlReprimarySaleReport
    {
        public ObservableCollection<dbmlReprimarySaleReport> objdbmlReprimarySaleReport = new ObservableCollection<dbmlReprimarySaleReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion


    #region  Download Report
    public class requestdbmlDownloadInvoiceReport
    {
        public ObservableCollection<dbmlDownloadInvoiceReport> objdbmlDownloadInvoiceReport = new ObservableCollection<dbmlDownloadInvoiceReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    #endregion

    #region UnitOfMeasureList
    public class returndbmlUOMMaster
    {
        public ObservableCollection<dbmlUOMMaster> objdbmlUOMList = new ObservableCollection<dbmlUOMMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region HSNCodeMasterList
    public class returndbmlHSNCodeMaster
    {
        public ObservableCollection<dbmlTempHSNCodeMaster> objdbmlHSNCodeList = new ObservableCollection<dbmlTempHSNCodeMaster>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion


    public class returndbmlMailServerDetails
    {
        public ObservableCollection<dbmlMailServerDetails> objdbmlMailServerDetails = new ObservableCollection<dbmlMailServerDetails>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class QrCodeModel
    {
        public string QrCodeBase64String { get; set; }
        public int DSCId { get; set; }
        public string AckNo { get; set; }
        public string AckDt { get; set; }
        public string Irn { get; set; }
        public byte[] vByteimage { get; set; }
        public string SignQRCode { get; set; }
        public string DocType { get; set; }
        
    }

    #region  Pending Web Order Status Report
    public class returndbmPendingWebOrderStatusReport
    {
        public ObservableCollection<dbmlPendingWebOrderStatusReport> objdbmlPendingWebOrderStatusReport = new ObservableCollection<dbmlPendingWebOrderStatusReport>();

        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region EInvoice
    public class returndbmlTempEInvoice
    {
        public ObservableCollection<dbmlTempEInvoice> objdbmlTempEInvoice = new ObservableCollection<dbmlTempEInvoice>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }

    public class returndbmlTempAPIAuthDetails
    {
        public ObservableCollection<dbmlTempAPIAuthDetails> objdbmlTempAPIAuthDetails = new ObservableCollection<dbmlTempAPIAuthDetails>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    public class returndbmlAPIAuthResponse
    {
        public ObservableCollection<dbmlAPIAuthResponse> objdbmlAPIAuthResponse = new ObservableCollection<dbmlAPIAuthResponse>();
        public dbmlStatus objdbmlStatus = new dbmlStatus();
    }
    #endregion

    #region For Create Json file for Einvoice
    public class ReqPlGenIRN
    {
       

        public List<AddlDocument> AddlDocDtls { get; set; }
        public BuyerDetails BuyerDtls { get; set; }
        public DispatchedDetails DispDtls { get; set; }
        public DocSetails DocDtls { get; set; }
        public EwbDetails EwbDtls { get; set; }
        public ExpDetails ExpDtls { get; set; }
        public List<ItmList> ItemList { get; set; }
        public PayDetails PayDtls { get; set; }
        public RefDetails RefDtls { get; set; }
        public SellerDetails SellerDtls { get; set; }
        public ShippedDetails ShipDtls { get; set; }
        public TranDetails TranDtls { get; set; }
        public ValDetails ValDtls { get; set; }
        public string Version { get; set; }

        public class AddlDocument
        {
            

            public string Docs { get; set; }
            public string Info { get; set; }
            public string Url { get; set; }
        }
        public class BuyerDetails
        {
            

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public string Pos { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class DispatchedDetails
        {
            

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public string Nm { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }
        public class DocSetails
        {
           

            public string Dt { get; set; }
            public string No { get; set; }
            public string Typ { get; set; }
        }
        public class EwbDetails
        {
           

            public int Distance { get; set; }
            public string TransDocDt { get; set; }
            public string TransDocNo { get; set; }
            public string TransId { get; set; }
            public string TransMode { get; set; }
            public string TransName { get; set; }
            public string VehNo { get; set; }
            public string VehType { get; set; }
        }
        public class ExpDetails
        {
          
            public string CntCode { get; set; }
            public string ForCur { get; set; }
            public string Port { get; set; }
            public string RefClm { get; set; }
            public string ShipBDt { get; set; }
            public string ShipBNo { get; set; }
        }
        public class ItmList
        {
          

            public double AssAmt { get; set; }
            public List<AttributeDtls> AttribDtls { get; set; }
            public string Barcde { get; set; }
            public BatchDetails BchDtls { get; set; }
            public double CesAmt { get; set; }
            public double CesNonAdvlAmt { get; set; }
            public double CesRt { get; set; }
            public double CgstAmt { get; set; }
            public double Discount { get; set; }
            public double FreeQty { get; set; }
            public double GstRt { get; set; }
            public string HsnCd { get; set; }
            public double IgstAmt { get; set; }
            public string IsServc { get; set; }
            public string OrdLineRef { get; set; }
            public string OrgCntry { get; set; }
            public double OthChrg { get; set; }
            public string PrdDesc { get; set; }
            public string PrdSlNo { get; set; }
            public double PreTaxVal { get; set; }
            public double Qty { get; set; }
            public double SgstAmt { get; set; }
            public string SlNo { get; set; }
            public double StateCesAmt { get; set; }
            public double StateCesNonAdvlAmt { get; set; }
            public double StateCesRt { get; set; }
            public double TotAmt { get; set; }
            public double TotItemVal { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }

            public class AttributeDtls
            {
               

                public string Nm { get; set; }
                public string Val { get; set; }
            }
            public class BatchDetails
            {
               

                public string ExpDt { get; set; }
                public string Nm { get; set; }
                public string WrDt { get; set; }
            }
        }
        public class PayDetails
        {
          

            public string AcctDet { get; set; }
            public int CrDay { get; set; }
            public string CrTrn { get; set; }
            public string DirDr { get; set; }
            public string FinInsBr { get; set; }
            public string Mode { get; set; }
            public string Nm { get; set; }
            public double PaidAmt { get; set; }
            public string PayInstr { get; set; }
            public double PaymtDue { get; set; }
            public string PayTerm { get; set; }
        }
        public class RefDetails
        {
           

            public List<Contract> ContrDtls { get; set; }
            public DocPerdDetails DocPerdDtls { get; set; }
            public string InvRm { get; set; }
            public List<PrecDocument> PrecDocDtls { get; set; }

            public class Contract
            {
               

                public string ContrRefr { get; set; }
                public string ExtRefr { get; set; }
                public string PORefDt { get; set; }
                public string PORefr { get; set; }
                public string ProjRefr { get; set; }
                public string RecAdvDt { get; set; }
                public string RecAdvRefr { get; set; }
                public string TendRefr { get; set; }
            }
            public class DocPerdDetails
            {
               

                public string InvEndDt { get; set; }
                public string InvStDt { get; set; }
            }
            public class PrecDocument
            {
               

                public string InvDt { get; set; }
                public string InvNo { get; set; }
                public string OthRefNo { get; set; }
            }
        }
        public class SellerDetails
        {
          

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class ShippedDetails
        {
           

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class TranDetails
        {
          

            public string EcmGstin { get; set; }
            public string IgstOnIntra { get; set; }
            public string RegRev { get; set; }
            public string SupTyp { get; set; }
            public string TaxSch { get; set; }
        }
        public class ValDetails
        {
           
            public double? AssVal { get; set; }
            public double? CesVal { get; set; }
            public double? CgstVal { get; set; }
            public double? Discount { get; set; }
            public double? IgstVal { get; set; }
            public double? OthChrg { get; set; }
            public double? RndOffAmt { get; set; }
            public double? SgstVal { get; set; }
            public double? StCesVal { get; set; }
            public double? TotInvVal { get; set; }
            public double? TotInvValFc { get; set; }
        }
    }

    public class RespGenIRNInvData
    {

        public string AckDt { get; set; }
        public long AckNo { get; set; }
        public List<AddlDocDetails> AddlDocDtls { get; set; }
        public BuyerDetails BuyerDtls { get; set; }
        public DispatchedDetails DispDtls { get; set; }
        public DocDetails DocDtls { get; set; }
        public EwbDetails EwbDtls { get; set; }
        public ExpDetails ExpDtls { get; set; }
        public string Irn { get; set; }
        public List<ItmList> ItemList { get; set; }
        public PayDetails PayDtls { get; set; }
        public RefDetails RefDtls { get; set; }
        public SellerDetails SellerDtls { get; set; }
        public ShippedDetails ShipDtls { get; set; }
        public TranDetails TranDtls { get; set; }
        public ValDetails ValDtls { get; set; }
        public string Version { get; set; }

        public class AddlDocDetails
        {

            public string Docs { get; set; }
            public string Info { get; set; }
            public string Url { get; set; }
        }
        public class BuyerDetails
        {

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public string Pos { get; set; }
            public int Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class DispatchedDetails
        {

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public string Nm { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }
        public class DocDetails
        {

            public string Dt { get; set; }
            public string No { get; set; }
            public string Typ { get; set; }
        }
        public class EwbDetails
        {

            public int Distance { get; set; }
            public string TransDocDt { get; set; }
            public string TransDocNo { get; set; }
            public string TransId { get; set; }
            public string TransMode { get; set; }
            public string TransName { get; set; }
            public string VehNo { get; set; }
            public string VehType { get; set; }
        }
        public class ExpDetails
        {

            public string CntCode { get; set; }
            public string ForCur { get; set; }
            public double? InvForCur { get; set; }
            public string Port { get; set; }
            public string RefClm { get; set; }
            public string ShipBDt { get; set; }
            public string ShipBNo { get; set; }
        }
        public class ItmList
        {

            public double AssAmt { get; set; }
            public List<AttributeDtls> AttribDtls { get; set; }
            public string Barcde { get; set; }
            public BatchDetails BchDtls { get; set; }
            public double CesAmt { get; set; }
            public double CesNonAdvlAmt { get; set; }
            public double CesRt { get; set; }
            public double CgstAmt { get; set; }
            public double Discount { get; set; }
            public string FreeQty { get; set; }
            public double GstRt { get; set; }
            public string HsnCd { get; set; }
            public double IgstAmt { get; set; }
            public string IsServc { get; set; }
            public int ItemNo { get; set; }
            public string OrdLineRef { get; set; }
            public string OrgCntry { get; set; }
            public double OthChrg { get; set; }
            public string PrdDesc { get; set; }
            public string PrdSlNo { get; set; }
            public double PreTaxVal { get; set; }
            public string Qty { get; set; }
            public double SgstAmt { get; set; }
            public string SlNo { get; set; }
            public double StateCesAmt { get; set; }
            public double StateCesNonAdvlAmt { get; set; }
            public double StateCesRt { get; set; }
            public double TotAmt { get; set; }
            public double TotItemVal { get; set; }
            public string Unit { get; set; }
            public double UnitPrice { get; set; }

            public class AttributeDtls
            {

                public string Nm { get; set; }
                public string Val { get; set; }
            }
            public class BatchDetails
            {

                public string ExpDt { get; set; }
                public string Nm { get; set; }
                public string WrDt { get; set; }
            }
        }
        public class PayDetails
        {

            public string AccDet { get; set; }
            public int CrDay { get; set; }
            public string CrTrn { get; set; }
            public string DirDr { get; set; }
            public string FinInsBr { get; set; }
            public string Mode { get; set; }
            public string Nm { get; set; }
            public double PaidAmt { get; set; }
            public string PayInstr { get; set; }
            public double PaymtDue { get; set; }
            public string PayTerm { get; set; }
        }
        public class RefDetails
        {

            public List<ContractDetails> ContrDtls { get; set; }
            public DocPerdDetails DocPerdDtls { get; set; }
            public string InvRm { get; set; }
            public List<PrecDocDetails> PrecDocDtls { get; set; }

            public class ContractDetails
            {

                public string ContrRefr { get; set; }
                public string ExtRefr { get; set; }
                public string PORefDt { get; set; }
                public string PORefr { get; set; }
                public string ProjRefr { get; set; }
                public string RecAdvDt { get; set; }
                public string RecAdvRefr { get; set; }
                public string TendRefr { get; set; }
            }
            public class DocPerdDetails
            {

                public string InvEndDt { get; set; }
                public string InvStDt { get; set; }
            }
            public class PrecDocDetails
            {

                public string InvDt { get; set; }
                public string InvNo { get; set; }
                public string OthRefNo { get; set; }
            }
        }
        public class SellerDetails
        {

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Em { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public string Ph { get; set; }
            public int Pin { get; set; }
            public int Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class ShippedDetails
        {

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Gstin { get; set; }
            public string LglNm { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
            public string TrdNm { get; set; }
        }
        public class TranDetails
        {

            public string EcmGstin { get; set; }
            public string IgstOnIntra { get; set; }
            public string RegRev { get; set; }
            public string SupTyp { get; set; }
            public string TaxSch { get; set; }
        }
        public class ValDetails
        {

            public double? AssVal { get; set; }
            public double? CesVal { get; set; }
            public double? CgstVal { get; set; }
            public double? Discount { get; set; }
            public double? IgstVal { get; set; }
            public double? OthChrg { get; set; }
            public double? RndOffAmt { get; set; }
            public double? SgstVal { get; set; }
            public double? StCesVal { get; set; }
            public double? TotInvVal { get; set; }
            public double? TotInvValFc { get; set; }
        }
    }

    public class RespGenIRNQrCodeData
    {

        public string BuyerGstin { get; set; }
        public string DocDt { get; set; }
        public string DocNo { get; set; }
        public string DocTyp { get; set; }
        public string Irn { get; set; }
        public string ItemCnt { get; set; }
        public string MainHsnCode { get; set; }
        public string SellerGstin { get; set; }
        public double? TotInvVal { get; set; }
    }
    public class RespPlGenIRN
    {

        public string AckDt { get; set; }
        public string AckNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbNo { get; set; }
        public string EwbValidTill { get; set; }
        public RespGenIRNInvData ExtractedSignedInvoiceData { get; set; }
        public RespGenIRNQrCodeData ExtractedSignedQrCode { get; set; }
        public string Irn { get; set; }
        public string JwtIssuer { get; set; }
        public Bitmap QrCodeImage { get; set; }
        public string SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
    public class ReqPlCancelIRN
    {

        public string CnlRem { get; set; }
        public string CnlRsn { get; set; }
        public string Irn { get; set; }
    }
    public class RespPlCancelIRN
    {
        public string CancelDate { get; set; }
        public string Irn { get; set; }
        public string DSCId { get; set; }
        public string CancelReason { get; set; }
        public string CancelRem { get; set; }
        public string DocType { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
    #endregion

    #region EWB
    public class ReqPlGenEwbByIRN
    {
      
        public DispatchedDetails DispDtls { get; set; }
        public int Distance { get; set; }
        public ExportShipDetails ExpShipDtls { get; set; }
        public string Irn { get; set; }
        public string TransDocDt { get; set; }
        public string TransDocNo { get; set; }
        public string TransId { get; set; }
        public string TransMode { get; set; }
        public string TransName { get; set; }
        public string VehNo { get; set; }
        public string VehType { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }

        public class DispatchedDetails
        {

            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public string Nm { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }
        public class ExportShipDetails
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Loc { get; set; }
            public int Pin { get; set; }
            public string Stcd { get; set; }
        }
    }
    public class RespPlGetEWBByIRN
    {
        public int DSCId { get; set; }
        public string EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string DocType { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }

    }

    public class ReqPlCancelEWB
    {
        public string cancelRmrk { get; set; }
        public int cancelRsnCode { get; set; }
        public long ewbNo { get; set; }
    }
    public class RespPlCancelEWB
    {
        public string cancelDate { get; set; }
        public string ewayBillNo { get; set; }
        public string DocType { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string DSCId { get; set; }
        public string CancelReason { get; set; }
    }
    #endregion

}