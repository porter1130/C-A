using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using SAP.Middleware.Table;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 操作SAP的方法集合，用来创建通用的全局变量（如：IRfcFunction 对象）和定义SAP交互的逻辑结构
    /// </summary>
    internal class SapFunctions
    {
        private string mCurrentDestinationStr = SapDestinationNames.NCO_TESTS_WITHOUT_POOL;
        /// <summary>
        /// 连接SAP的参数配置，从配置文件获取
        /// </summary>
        protected string CurrentDestinationStr
        {
            set { mCurrentDestinationStr = value; }
        }
        private string mCurrentFunctionStr = SapFunctionNames.Z_EWF_CREATE;
        /// <summary>
        /// 调用SAP的方法名，不同的工作流需要执行不同的SAP方法操作
        /// </summary>
        protected string CurrentFunctionStr
        {
            set { mCurrentFunctionStr = value; }
        }
       
        /// <summary>
        /// SAP进行交互的对象
        /// </summary>
        private IRfcFunction mCurrentRfcFunction = null;
        private RfcDestination mCurrentDestination = null;
        
        /// <summary>
        /// 填充 DOCUMENTHEADER 表数据
        /// </summary>
        /// <param name="docHeader">DOCUMENTHEADER 对象</param>
        protected void InsertDataDocumentHeader(DOCUMENTHEADER docHeader)
        {
            IRfcStructure rfcStructDocHeader = mCurrentRfcFunction.GetStructure("DOCUMENTHEADER");
            rfcStructDocHeader.SetValue("OBJ_TYPE", docHeader.OBJ_TYPE);
            rfcStructDocHeader.SetValue("OBJ_KEY", docHeader.OBJ_KEY);
            rfcStructDocHeader.SetValue("OBJ_SYS", docHeader.OBJ_SYS);
            rfcStructDocHeader.SetValue("BUS_ACT", docHeader.BUS_ACT);
            rfcStructDocHeader.SetValue("USERNAME", docHeader.USERNAME);
            rfcStructDocHeader.SetValue("HEADER_TXT", docHeader.HEADER_TXT);
            rfcStructDocHeader.SetValue("COMP_CODE", docHeader.COMP_CODE);
            rfcStructDocHeader.SetValue("DOC_DATE", docHeader.DOC_DATE);
            rfcStructDocHeader.SetValue("PSTNG_DATE", docHeader.PSTNG_DATE);
            rfcStructDocHeader.SetValue("TRANS_DATE", docHeader.TRANS_DATE);
            rfcStructDocHeader.SetValue("FISC_YEAR", docHeader.FISC_YEAR);
            rfcStructDocHeader.SetValue("FIS_PERIOD", docHeader.FIS_PERIOD);
            rfcStructDocHeader.SetValue("DOC_TYPE", docHeader.DOC_TYPE);
            rfcStructDocHeader.SetValue("REF_DOC_NO", docHeader.REF_DOC_NO);
            rfcStructDocHeader.SetValue("AC_DOC_NO", docHeader.AC_DOC_NO);
            rfcStructDocHeader.SetValue("OBJ_KEY_R", docHeader.OBJ_KEY_R);
            rfcStructDocHeader.SetValue("REASON_REV", docHeader.REASON_REV);
            rfcStructDocHeader.SetValue("COMPO_ACC", docHeader.COMPO_ACC);
            rfcStructDocHeader.SetValue("REF_DOC_NO_LONG", docHeader.REF_DOC_NO_LONG);
            rfcStructDocHeader.SetValue("ACC_PRINCIPLE", docHeader.ACC_PRINCIPLE);
            rfcStructDocHeader.SetValue("NEG_POSTNG", docHeader.NEG_POSTNG);
            rfcStructDocHeader.SetValue("OBJ_KEY_INV", docHeader.OBJ_KEY_INV);
            rfcStructDocHeader.SetValue("BILL_CATEGORY", docHeader.BILL_CATEGORY);
            rfcStructDocHeader.SetValue("VATDATE", docHeader.VATDATE);
        }
        /// <summary>
        /// 插入数据到SAP表：AccountGL
        /// </summary>
        /// <param name="accountGL"></param>
        protected void InsertDataAccountGL(ACCOUNTGL accountGL)
        {
            IRfcTable rfcTableAccountGL = mCurrentRfcFunction.GetTable("ACCOUNTGL");
            rfcTableAccountGL.Insert();
            rfcTableAccountGL.CurrentRow.SetValue("ITEMNO_ACC", accountGL.ITEMNO_ACC);
            rfcTableAccountGL.CurrentRow.SetValue("GL_ACCOUNT", accountGL.GL_ACCOUNT);
            rfcTableAccountGL.CurrentRow.SetValue("ITEM_TEXT", accountGL.ITEM_TEXT);
            rfcTableAccountGL.CurrentRow.SetValue("STAT_CON", accountGL.STAT_CON);
            rfcTableAccountGL.CurrentRow.SetValue("LOG_PROC", accountGL.LOG_PROC);
            rfcTableAccountGL.CurrentRow.SetValue("AC_DOC_NO", accountGL.AC_DOC_NO);
            rfcTableAccountGL.CurrentRow.SetValue("REF_KEY_1", accountGL.REF_KEY_1);
            rfcTableAccountGL.CurrentRow.SetValue("REF_KEY_2", accountGL.REF_KEY_2);
            rfcTableAccountGL.CurrentRow.SetValue("REF_KEY_3", accountGL.REF_KEY_3);
            rfcTableAccountGL.CurrentRow.SetValue("ACCT_KEY", accountGL.ACCT_KEY);
            rfcTableAccountGL.CurrentRow.SetValue("ACCT_TYPE", accountGL.ACCT_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("DOC_TYPE", accountGL.DOC_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("COMP_CODE", accountGL.COMP_CODE);
            rfcTableAccountGL.CurrentRow.SetValue("BUS_AREA", accountGL.BUS_AREA);
            rfcTableAccountGL.CurrentRow.SetValue("FUNC_AREA", accountGL.FUNC_AREA);
            rfcTableAccountGL.CurrentRow.SetValue("PLANT", accountGL.PLANT);
            rfcTableAccountGL.CurrentRow.SetValue("FIS_PERIOD", accountGL.FIS_PERIOD);
            rfcTableAccountGL.CurrentRow.SetValue("FISC_YEAR", accountGL.FISC_YEAR);
            rfcTableAccountGL.CurrentRow.SetValue("PSTNG_DATE", accountGL.PSTNG_DATE);
            rfcTableAccountGL.CurrentRow.SetValue("VALUE_DATE", accountGL.VALUE_DATE);
            rfcTableAccountGL.CurrentRow.SetValue("FM_AREA", accountGL.FM_AREA);
            rfcTableAccountGL.CurrentRow.SetValue("CUSTOMER", accountGL.CUSTOMER);
            rfcTableAccountGL.CurrentRow.SetValue("CSHDIS_IND", accountGL.CSHDIS_IND);
            rfcTableAccountGL.CurrentRow.SetValue("VENDOR_NO", accountGL.VENDOR_NO);
            rfcTableAccountGL.CurrentRow.SetValue("ALLOC_NMBR", accountGL.ALLOC_NMBR);
            rfcTableAccountGL.CurrentRow.SetValue("TAX_CODE", accountGL.TAX_CODE);
            rfcTableAccountGL.CurrentRow.SetValue("TAXJURCODE", accountGL.TAXJURCODE);
            rfcTableAccountGL.CurrentRow.SetValue("EXT_OBJECT_ID", accountGL.EXT_OBJECT_ID);
            rfcTableAccountGL.CurrentRow.SetValue("BUS_SCENARIO", accountGL.BUS_SCENARIO);
            rfcTableAccountGL.CurrentRow.SetValue("COSTOBJECT", accountGL.COSTOBJECT);
            rfcTableAccountGL.CurrentRow.SetValue("COSTCENTER", accountGL.COSTCENTER);
            rfcTableAccountGL.CurrentRow.SetValue("ACTTYPE", accountGL.ACTTYPE);
            rfcTableAccountGL.CurrentRow.SetValue("PROFIT_CTR", accountGL.PROFIT_CTR);
            rfcTableAccountGL.CurrentRow.SetValue("PART_PRCTR", accountGL.PART_PRCTR);
            rfcTableAccountGL.CurrentRow.SetValue("NETWORK", accountGL.NETWORK);
            rfcTableAccountGL.CurrentRow.SetValue("WBS_ELEMENT", accountGL.WBS_ELEMENT);
            rfcTableAccountGL.CurrentRow.SetValue("ORDERID", accountGL.ORDERID);
            rfcTableAccountGL.CurrentRow.SetValue("ORDER_ITNO", accountGL.ORDER_ITNO);
            rfcTableAccountGL.CurrentRow.SetValue("ROUTING_NO", accountGL.ROUTING_NO);
            rfcTableAccountGL.CurrentRow.SetValue("ACTIVITY", accountGL.ACTIVITY);
            rfcTableAccountGL.CurrentRow.SetValue("COND_TYPE", accountGL.COND_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("COND_COUNT", accountGL.COND_COUNT);
            rfcTableAccountGL.CurrentRow.SetValue("COND_ST_NO", accountGL.COND_ST_NO);
            rfcTableAccountGL.CurrentRow.SetValue("FUND", accountGL.FUND);
            rfcTableAccountGL.CurrentRow.SetValue("FUNDS_CTR", accountGL.FUNDS_CTR);
            rfcTableAccountGL.CurrentRow.SetValue("CMMT_ITEM", accountGL.CMMT_ITEM);
            rfcTableAccountGL.CurrentRow.SetValue("CO_BUSPROC", accountGL.CO_BUSPROC);
            rfcTableAccountGL.CurrentRow.SetValue("ASSET_NO", accountGL.ASSET_NO);
            rfcTableAccountGL.CurrentRow.SetValue("SUB_NUMBER", accountGL.SUB_NUMBER);
            rfcTableAccountGL.CurrentRow.SetValue("BILL_TYPE", accountGL.BILL_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("SALES_ORD", accountGL.SALES_ORD);
            rfcTableAccountGL.CurrentRow.SetValue("S_ORD_ITEM", accountGL.S_ORD_ITEM);
            rfcTableAccountGL.CurrentRow.SetValue("DISTR_CHAN", accountGL.DISTR_CHAN);
            rfcTableAccountGL.CurrentRow.SetValue("DIVISION", accountGL.DIVISION);
            rfcTableAccountGL.CurrentRow.SetValue("SALESORG", accountGL.SALESORG);
            rfcTableAccountGL.CurrentRow.SetValue("SALES_GRP", accountGL.SALES_GRP);
            rfcTableAccountGL.CurrentRow.SetValue("SALES_OFF", accountGL.SALES_OFF);
            rfcTableAccountGL.CurrentRow.SetValue("SOLD_TO", accountGL.SOLD_TO);
            rfcTableAccountGL.CurrentRow.SetValue("DE_CRE_IND", accountGL.DE_CRE_IND);
            rfcTableAccountGL.CurrentRow.SetValue("P_EL_PRCTR", accountGL.P_EL_PRCTR);
            rfcTableAccountGL.CurrentRow.SetValue("XMFRW", accountGL.XMFRW);
            rfcTableAccountGL.CurrentRow.SetValue("QUANTITY", accountGL.QUANTITY);
            rfcTableAccountGL.CurrentRow.SetValue("BASE_UOM", accountGL.BASE_UOM);
            rfcTableAccountGL.CurrentRow.SetValue("BASE_UOM_ISO", accountGL.BASE_UOM_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("INV_QTY", accountGL.INV_QTY);
            rfcTableAccountGL.CurrentRow.SetValue("INV_QTY_SU", accountGL.INV_QTY_SU);
            rfcTableAccountGL.CurrentRow.SetValue("SALES_UNIT", accountGL.SALES_UNIT);
            rfcTableAccountGL.CurrentRow.SetValue("SALES_UNIT_ISO", accountGL.SALES_UNIT_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("PO_PR_QNT", accountGL.PO_PR_QNT);
            rfcTableAccountGL.CurrentRow.SetValue("PO_PR_UOM", accountGL.PO_PR_UOM);
            rfcTableAccountGL.CurrentRow.SetValue("PO_PR_UOM_ISO", accountGL.PO_PR_UOM_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("ENTRY_QNT", accountGL.ENTRY_QNT);
            rfcTableAccountGL.CurrentRow.SetValue("ENTRY_UOM", accountGL.ENTRY_UOM);
            rfcTableAccountGL.CurrentRow.SetValue("ENTRY_UOM_ISO", accountGL.ENTRY_UOM_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("VOLUME", accountGL.VOLUME);
            rfcTableAccountGL.CurrentRow.SetValue("VOLUMEUNIT", accountGL.VOLUMEUNIT);
            rfcTableAccountGL.CurrentRow.SetValue("VOLUMEUNIT_ISO", accountGL.VOLUMEUNIT_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("GROSS_WT", accountGL.GROSS_WT);
            rfcTableAccountGL.CurrentRow.SetValue("NET_WEIGHT", accountGL.NET_WEIGHT);
            rfcTableAccountGL.CurrentRow.SetValue("UNIT_OF_WT", accountGL.UNIT_OF_WT);
            rfcTableAccountGL.CurrentRow.SetValue("UNIT_OF_WT_ISO", accountGL.UNIT_OF_WT_ISO);
            rfcTableAccountGL.CurrentRow.SetValue("ITEM_CAT", accountGL.ITEM_CAT);
            rfcTableAccountGL.CurrentRow.SetValue("MATERIAL", accountGL.MATERIAL);
            rfcTableAccountGL.CurrentRow.SetValue("MATL_TYPE", accountGL.MATL_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("MVT_IND", accountGL.MVT_IND);
            rfcTableAccountGL.CurrentRow.SetValue("REVAL_IND", accountGL.REVAL_IND);
            rfcTableAccountGL.CurrentRow.SetValue("ORIG_GROUP", accountGL.ORIG_GROUP);
            rfcTableAccountGL.CurrentRow.SetValue("ORIG_MAT", accountGL.ORIG_MAT);
            rfcTableAccountGL.CurrentRow.SetValue("SERIAL_NO", accountGL.SERIAL_NO);
            rfcTableAccountGL.CurrentRow.SetValue("PART_ACCT", accountGL.PART_ACCT);
            rfcTableAccountGL.CurrentRow.SetValue("TR_PART_BA", accountGL.TR_PART_BA);
            rfcTableAccountGL.CurrentRow.SetValue("TRADE_ID", accountGL.TRADE_ID);
            rfcTableAccountGL.CurrentRow.SetValue("VAL_AREA", accountGL.VAL_AREA);
            rfcTableAccountGL.CurrentRow.SetValue("VAL_TYPE", accountGL.VAL_TYPE);
            rfcTableAccountGL.CurrentRow.SetValue("ASVAL_DATE", accountGL.ASVAL_DATE);
            rfcTableAccountGL.CurrentRow.SetValue("PO_NUMBER", accountGL.PO_NUMBER);
            rfcTableAccountGL.CurrentRow.SetValue("PO_ITEM", accountGL.PO_ITEM);
            rfcTableAccountGL.CurrentRow.SetValue("ITM_NUMBER", accountGL.ITM_NUMBER);
            rfcTableAccountGL.CurrentRow.SetValue("COND_CATEGORY", accountGL.COND_CATEGORY);
            rfcTableAccountGL.CurrentRow.SetValue("FUNC_AREA_LONG", accountGL.FUNC_AREA_LONG);
            rfcTableAccountGL.CurrentRow.SetValue("CMMT_ITEM_LONG", accountGL.CMMT_ITEM_LONG);
            rfcTableAccountGL.CurrentRow.SetValue("GRANT_NBR", accountGL.GRANT_NBR);
            rfcTableAccountGL.CurrentRow.SetValue("CS_TRANS_T", accountGL.CS_TRANS_T);
            rfcTableAccountGL.CurrentRow.SetValue("MEASURE", accountGL.MEASURE);
            rfcTableAccountGL.CurrentRow.SetValue("SEGMENT", accountGL.SEGMENT);
            rfcTableAccountGL.CurrentRow.SetValue("PARTNER_SEGMENT", accountGL.PARTNER_SEGMENT);
            rfcTableAccountGL.CurrentRow.SetValue("RES_DOC", accountGL.RES_DOC);
            rfcTableAccountGL.CurrentRow.SetValue("RES_ITEM", accountGL.RES_ITEM);
        }

        /// <summary>
        /// 插入数据到SAP表：AccountPlayble
        /// </summary>
        /// <param name="accountPlayble"></param>
        protected void InsertDataAccountPlayble(ACCOUNTPAYABLE accountPlayble)
        {
            IRfcTable rfcTableAccountPlayble = mCurrentRfcFunction.GetTable("ACCOUNTPAYABLE");
            rfcTableAccountPlayble.Insert();
            rfcTableAccountPlayble.CurrentRow.SetValue("ITEMNO_ACC", accountPlayble.ITEMNO_ACC);
            rfcTableAccountPlayble.CurrentRow.SetValue("VENDOR_NO", accountPlayble.VENDOR_NO);
            rfcTableAccountPlayble.CurrentRow.SetValue("GL_ACCOUNT", accountPlayble.GL_ACCOUNT);
            rfcTableAccountPlayble.CurrentRow.SetValue("REF_KEY_1", accountPlayble.REF_KEY_1);
            rfcTableAccountPlayble.CurrentRow.SetValue("REF_KEY_2", accountPlayble.REF_KEY_2);
            rfcTableAccountPlayble.CurrentRow.SetValue("REF_KEY_3", accountPlayble.REF_KEY_3);
            rfcTableAccountPlayble.CurrentRow.SetValue("COMP_CODE", accountPlayble.COMP_CODE);
            rfcTableAccountPlayble.CurrentRow.SetValue("BUS_AREA", accountPlayble.BUS_AREA);
            rfcTableAccountPlayble.CurrentRow.SetValue("PMNTTRMS", accountPlayble.PMNTTRMS);
            rfcTableAccountPlayble.CurrentRow.SetValue("BLINE_DATE", accountPlayble.BLINE_DATE);
            rfcTableAccountPlayble.CurrentRow.SetValue("DSCT_DAYS1", accountPlayble.DSCT_DAYS1);
            rfcTableAccountPlayble.CurrentRow.SetValue("DSCT_DAYS2", accountPlayble.DSCT_DAYS2);
            rfcTableAccountPlayble.CurrentRow.SetValue("NETTERMS", accountPlayble.NETTERMS);
            rfcTableAccountPlayble.CurrentRow.SetValue("DSCT_PCT1", accountPlayble.DSCT_PCT1);
            rfcTableAccountPlayble.CurrentRow.SetValue("DSCT_PCT2", accountPlayble.DSCT_PCT2);
            rfcTableAccountPlayble.CurrentRow.SetValue("PYMT_METH", accountPlayble.PYMT_METH);
            rfcTableAccountPlayble.CurrentRow.SetValue("PMTMTHSUPL", accountPlayble.PMTMTHSUPL);
            rfcTableAccountPlayble.CurrentRow.SetValue("PMNT_BLOCK", accountPlayble.PMNT_BLOCK);
            rfcTableAccountPlayble.CurrentRow.SetValue("SCBANK_IND", accountPlayble.SCBANK_IND);
            rfcTableAccountPlayble.CurrentRow.SetValue("SUPCOUNTRY", accountPlayble.SUPCOUNTRY);
            rfcTableAccountPlayble.CurrentRow.SetValue("SUPCOUNTRY_ISO", accountPlayble.SUPCOUNTRY_ISO);
            rfcTableAccountPlayble.CurrentRow.SetValue("BLLSRV_IND", accountPlayble.BLLSRV_IND);
            rfcTableAccountPlayble.CurrentRow.SetValue("ALLOC_NMBR", accountPlayble.ALLOC_NMBR);
            rfcTableAccountPlayble.CurrentRow.SetValue("ITEM_TEXT", accountPlayble.ITEM_TEXT);
            rfcTableAccountPlayble.CurrentRow.SetValue("PO_SUB_NO", accountPlayble.PO_SUB_NO);
            rfcTableAccountPlayble.CurrentRow.SetValue("PO_CHECKDG", accountPlayble.PO_CHECKDG);
            rfcTableAccountPlayble.CurrentRow.SetValue("PO_REF_NO", accountPlayble.PO_REF_NO);
            rfcTableAccountPlayble.CurrentRow.SetValue("W_TAX_CODE", accountPlayble.W_TAX_CODE);
            rfcTableAccountPlayble.CurrentRow.SetValue("BUSINESSPLACE", accountPlayble.BUSINESSPLACE);
            rfcTableAccountPlayble.CurrentRow.SetValue("SECTIONCODE", accountPlayble.SECTIONCODE);
            rfcTableAccountPlayble.CurrentRow.SetValue("INSTR1", accountPlayble.INSTR1);
            rfcTableAccountPlayble.CurrentRow.SetValue("INSTR2", accountPlayble.INSTR2);
            rfcTableAccountPlayble.CurrentRow.SetValue("INSTR3", accountPlayble.INSTR3);
            rfcTableAccountPlayble.CurrentRow.SetValue("INSTR4", accountPlayble.INSTR4);
            rfcTableAccountPlayble.CurrentRow.SetValue("BRANCH", accountPlayble.BRANCH);
            rfcTableAccountPlayble.CurrentRow.SetValue("PYMT_CUR", accountPlayble.PYMT_CUR);
            rfcTableAccountPlayble.CurrentRow.SetValue("PYMT_AMT", accountPlayble.PYMT_AMT);
            rfcTableAccountPlayble.CurrentRow.SetValue("PYMT_CUR_ISO", accountPlayble.PYMT_CUR_ISO);
            rfcTableAccountPlayble.CurrentRow.SetValue("SP_GL_IND", accountPlayble.SP_GL_IND);
            rfcTableAccountPlayble.CurrentRow.SetValue("TAX_CODE", accountPlayble.TAX_CODE);
            rfcTableAccountPlayble.CurrentRow.SetValue("TAX_DATE", accountPlayble.TAX_DATE);
            rfcTableAccountPlayble.CurrentRow.SetValue("TAXJURCODE", accountPlayble.TAXJURCODE);
            rfcTableAccountPlayble.CurrentRow.SetValue("ALT_PAYEE", accountPlayble.ALT_PAYEE);
            rfcTableAccountPlayble.CurrentRow.SetValue("ALT_PAYEE_BANK", accountPlayble.ALT_PAYEE_BANK);
            rfcTableAccountPlayble.CurrentRow.SetValue("PARTNER_BK", accountPlayble.PARTNER_BK);
            rfcTableAccountPlayble.CurrentRow.SetValue("BANK_ID", accountPlayble.BANK_ID);
            rfcTableAccountPlayble.CurrentRow.SetValue("PARTNER_GUID", accountPlayble.PARTNER_GUID);
            rfcTableAccountPlayble.CurrentRow.SetValue("PROFIT_CTR", accountPlayble.PROFIT_CTR);
            rfcTableAccountPlayble.CurrentRow.SetValue("FUND", accountPlayble.FUND);
            rfcTableAccountPlayble.CurrentRow.SetValue("GRANT_NBR", accountPlayble.GRANT_NBR);
            rfcTableAccountPlayble.CurrentRow.SetValue("MEASURE", accountPlayble.MEASURE);
            rfcTableAccountPlayble.CurrentRow.SetValue("HOUSEBANKACCTID", accountPlayble.HOUSEBANKACCTID);
        }

        /// <summary>
        /// 插入数据到SAP表：CurrencyAmount
        /// </summary>
        /// <param name="currencyAmount"></param>
        protected void InsertDataCurrencyAmount(CURRENCYAMOUNT currencyAmount, decimal taxValue)
        {
            IRfcTable rfcTableCurrencyAmount = mCurrentRfcFunction.GetTable("CURRENCYAMOUNT");
            rfcTableCurrencyAmount.Insert();
            rfcTableCurrencyAmount.CurrentRow.SetValue("ITEMNO_ACC", currencyAmount.ITEMNO_ACC);
            rfcTableCurrencyAmount.CurrentRow.SetValue("CURR_TYPE", currencyAmount.CURR_TYPE);
            rfcTableCurrencyAmount.CurrentRow.SetValue("CURRENCY", currencyAmount.CURRENCY);
            rfcTableCurrencyAmount.CurrentRow.SetValue("CURRENCY_ISO", currencyAmount.CURRENCY_ISO);
            rfcTableCurrencyAmount.CurrentRow.SetValue("AMT_DOCCUR", currencyAmount.AMT_DOCCUR);
            rfcTableCurrencyAmount.CurrentRow.SetValue("EXCH_RATE", currencyAmount.EXCH_RATE);
            rfcTableCurrencyAmount.CurrentRow.SetValue("EXCH_RATE_V", currencyAmount.EXCH_RATE_V);
            rfcTableCurrencyAmount.CurrentRow.SetValue("AMT_BASE", currencyAmount.AMT_BASE);
            rfcTableCurrencyAmount.CurrentRow.SetValue("DISC_BASE", currencyAmount.DISC_BASE);
            rfcTableCurrencyAmount.CurrentRow.SetValue("DISC_AMT", currencyAmount.DISC_AMT);
            //当taxValue为0时，表示是税额，需要插入如下两个字段,taxValue为所有Items总额
            if (taxValue != 0){
                rfcTableCurrencyAmount.CurrentRow.SetValue("TAX_AMT", currencyAmount.TAX_AMT);
                rfcTableCurrencyAmount.CurrentRow.SetValue("AMT_BASE", taxValue);
            }
        }

        /// <summary>
        /// 插入数据到SAP表：
        /// </summary>
        /// <param name="currencyAmount"></param>
        protected void InsertDataAccountTax(ACCOUNTTAX currencyAmount)
        {
            IRfcTable rfcTableCurrencyAmount = mCurrentRfcFunction.GetTable("ACCOUNTTAX");
            rfcTableCurrencyAmount.Insert();
            rfcTableCurrencyAmount.CurrentRow.SetValue("ITEMNO_ACC", currencyAmount.ITEMNO_ACC);
            rfcTableCurrencyAmount.CurrentRow.SetValue("TAX_CODE", "J1");
        }

        /// <summary>
        /// 填充 表数据
        /// </summary>
        /// <param name="docHeader">DOCUMENTHEADER 对象</param>
        protected void InsertVendor(VENDOR vendor)
        {
            IRfcStructure rfcStructVendor = mCurrentRfcFunction.GetStructure("CUSTOMERCPD");
            rfcStructVendor.SetValue("NAME", vendor.NAME);  //供应商名字，最长34位
            rfcStructVendor.SetValue("NAME_2", vendor.NAME_2);//多余的供应商名字
            rfcStructVendor.SetValue("CITY", vendor.CITY);//供应商所在地
            rfcStructVendor.SetValue("COUNTRY", vendor.COUNTRY);//供应商所在地
            rfcStructVendor.SetValue("BANK_ACCT", vendor.BANK_ACCT);//银行账号，最长18位，
            rfcStructVendor.SetValue("BKREF", vendor.BKREF);//多余的银行账号
            rfcStructVendor.SetValue("BANK_NO", vendor.BANK_NO);//银行KEY,银行代码，必须在SAP存在
            rfcStructVendor.SetValue("BANK_CTRY", vendor.BANK_CTRY);//银行所在国，默认CN
        }


        /// <summary>
        /// 填充 DOCUMENTHEADER 表数据
        /// </summary>
        /// <param name="docHeader">DOCUMENTHEADER 对象</param>
        protected void InsertDataHeaderData(DOCUMENTHEADER docHeader)
        {
            IRfcStructure rfcStructDocHeader = mCurrentRfcFunction.GetStructure("HEADERDATA");
            rfcStructDocHeader.SetValue("INVOICE_IND", "X");
            rfcStructDocHeader.SetValue("DOC_DATE", "2012-03-15");
            rfcStructDocHeader.SetValue("PSTNG_DATE", "2012-03-15");
            rfcStructDocHeader.SetValue("REF_DOC_NO", "IV100002");
            rfcStructDocHeader.SetValue("COMP_CODE", "CA10");
            rfcStructDocHeader.SetValue("CURRENCY", "RMB");
            rfcStructDocHeader.SetValue("GROSS_AMOUNT", "100");
            rfcStructDocHeader.SetValue("CALC_TAX_IND", "X");
            rfcStructDocHeader.SetValue("HEADER_TXT", "IV1000010002");
            rfcStructDocHeader.SetValue("ITEM_TEXT", "TEST");
            rfcStructDocHeader.SetValue("BUS_AREA", "0001");
        }

        /// <summary>
        /// 插入数据到SAP表：AccountGL
        /// </summary>
        /// <param name="accountGL"></param>
        protected void InsertDataIVItemData(ACCOUNTGL accountGL)
        {
            IRfcTable rfcTableAccountGL = mCurrentRfcFunction.GetTable("ITEMDATA");
            rfcTableAccountGL.Insert();
            rfcTableAccountGL.CurrentRow.SetValue("INVOICE_DOC_ITEM", "1");
            rfcTableAccountGL.CurrentRow.SetValue("PO_NUMBER", "6500001604");
            rfcTableAccountGL.CurrentRow.SetValue("PO_ITEM", "10");
            rfcTableAccountGL.CurrentRow.SetValue("TAX_CODE", "J1");
            rfcTableAccountGL.CurrentRow.SetValue("ITEM_AMOUNT", "10");
            rfcTableAccountGL.CurrentRow.SetValue("QUANTITY", "1");
            rfcTableAccountGL.CurrentRow.SetValue("PO_UNIT", "PCS");
        }

        /// <summary>
        ///  Purchase Order POST SAP of Header
        /// </summary>
        /// <param name="poHeader"></param>
        protected void InsertDataPOHeader(POHEADER poHeader)
        {
            IRfcStructure rfcStructBAPIMEPOHEADER = mCurrentRfcFunction.GetStructure("POHEADER");
            rfcStructBAPIMEPOHEADER.SetValue("COMP_CODE", poHeader.COMP_CODE);
            rfcStructBAPIMEPOHEADER.SetValue("DOC_TYPE", poHeader.DOC_TYPE);
            rfcStructBAPIMEPOHEADER.SetValue("VENDOR", poHeader.VENDOR);
            rfcStructBAPIMEPOHEADER.SetValue("PURCH_ORG", poHeader.PURCH_ORG);
            rfcStructBAPIMEPOHEADER.SetValue("PUR_GROUP", poHeader.PUR_GROUP);
            rfcStructBAPIMEPOHEADER.SetValue("DOC_DATE", poHeader.DOC_DATE);
            rfcStructBAPIMEPOHEADER.SetValue("PMNTTRMS", poHeader.PMNTTRMS);
            rfcStructBAPIMEPOHEADER.SetValue("CURRENCY", poHeader.Currency);
            rfcStructBAPIMEPOHEADER.SetValue("CREATED_BY", poHeader.CREATED_BY);

            IRfcStructure rfcStructBAPIMEPOHEADERX = mCurrentRfcFunction.GetStructure("POHEADERX");
            rfcStructBAPIMEPOHEADERX.SetValue("COMP_CODE", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("DOC_TYPE", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("VENDOR", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("PURCH_ORG", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("PUR_GROUP", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("DOC_DATE", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("PMNTTRMS", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("CURRENCY", "X");
            rfcStructBAPIMEPOHEADERX.SetValue("CREATED_BY", "X");
        }

        /// <summary>
        /// PO单退货时需要插入SAP的头数据
        /// </summary>
        protected void InsertDataPOReturnHeader(POHEADER poHeader)
        {
            IRfcStructure rfcStructBAPIMEPOHEADER = mCurrentRfcFunction.GetStructure("POHEADER");
            rfcStructBAPIMEPOHEADER.SetValue("PO_NUMBER", poHeader.PO_NUMBER);

            IRfcStructure rfcStructBAPIMEPOHEADERX = mCurrentRfcFunction.GetStructure("POHEADERX");
            rfcStructBAPIMEPOHEADERX.SetValue("PO_NUMBER", "X");

            mCurrentRfcFunction.SetValue("PURCHASEORDER", poHeader.PO_NUMBER);
        }

        /// <summary>
        /// PO单需要插入SAP的Items数据
        /// </summary>
        /// <param name="poItem"></param>
        protected void InsertDataPOItem(POITEM poItem)
        {
            IRfcTable rfcTableBAPIMEPOITEM = mCurrentRfcFunction.GetTable("POITEM");
            rfcTableBAPIMEPOITEM.Insert();
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PO_ITEM", poItem.PO_ITEM);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("SHORT_TEXT", poItem.SHORT_TEXT);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PLANT", poItem.PLANT);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("MATL_GROUP", poItem.MATL_GROUP);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("QUANTITY", poItem.QUANTITY);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PO_UNIT", poItem.PO_UNIT);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("ACCTASSCAT", poItem.ACCTASSCAT);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("TAX_CODE", poItem.TAX_CODE);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PERIOD_IND_EXPIRATION_DATE", "D");
            //if (poItem.IsPriceZero)//Item里有价格为零
            //{
            //    rfcTableBAPIMEPOITEM.CurrentRow.SetValue("FREE_ITEM", "X");
            //}

            IRfcTable rfcTableBAPIMEPOITEMX = mCurrentRfcFunction.GetTable("POITEMX");
            rfcTableBAPIMEPOITEMX.Insert();
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("PO_ITEM", poItem.PO_ITEM);
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("SHORT_TEXT", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("PLANT", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("MATL_GROUP", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("QUANTITY", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("PO_UNIT", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("ACCTASSCAT", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("TAX_CODE", "X");
            if (poItem.IsPriceZero)//Item里有价格为零
            {
                rfcTableBAPIMEPOITEM.CurrentRow.SetValue("FREE_ITEM", "X");//1
                rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("FREE_ITEM", "X");//2
            }

        }

        /// <summary>
        /// PO单退货时需要插入SAP的Items数据
        /// </summary>
        /// <param name="poItem"></param>
        protected void InsertDataPOReturnItem(POITEM poItem)
        {
            IRfcTable rfcTableBAPIMEPOITEM = mCurrentRfcFunction.GetTable("POITEM");
            rfcTableBAPIMEPOITEM.Insert();
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PO_ITEM", poItem.PO_ITEM);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("QUANTITY", poItem.QUANTITY);
            rfcTableBAPIMEPOITEM.CurrentRow.SetValue("PO_UNIT", poItem.PO_UNIT);

            IRfcTable rfcTableBAPIMEPOITEMX = mCurrentRfcFunction.GetTable("POITEMX");
            rfcTableBAPIMEPOITEMX.Insert();
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("PO_ITEM", poItem.PO_ITEM);
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("QUANTITY", "X");
            rfcTableBAPIMEPOITEMX.CurrentRow.SetValue("PO_UNIT", "X");
        }

        /// <summary>
        /// PO Account
        /// </summary>
        /// <param name="poAccount"></param>
        protected void InsertDataPOAccount(POACCOUNT poAccount)
        {
            IRfcTable rfcTableBAPIMEPOACCOUNT = mCurrentRfcFunction.GetTable("POACCOUNT");
            rfcTableBAPIMEPOACCOUNT.Insert();
            rfcTableBAPIMEPOACCOUNT.CurrentRow.SetValue("PO_ITEM", poAccount.PO_ITEM);
            rfcTableBAPIMEPOACCOUNT.CurrentRow.SetValue("COSTCENTER", poAccount.COSTCENTER);
            rfcTableBAPIMEPOACCOUNT.CurrentRow.SetValue("ASSET_NO", poAccount.ASSET_NO);

            IRfcTable rfcTableBAPIMEPOACCOUNTX = mCurrentRfcFunction.GetTable("POACCOUNTX");
            rfcTableBAPIMEPOACCOUNTX.Insert();
            rfcTableBAPIMEPOACCOUNTX.CurrentRow.SetValue("PO_ITEM", poAccount.PO_ITEM);
            rfcTableBAPIMEPOACCOUNTX.CurrentRow.SetValue("COSTCENTER", "X");
            rfcTableBAPIMEPOACCOUNTX.CurrentRow.SetValue("ASSET_NO", "X");
        }

        /// <summary>
        /// PO Cond
        /// </summary>
        /// <param name="poCond"></param>
        protected void InsertDataPOCond(POCOND poCond)
        {
            IRfcTable rfcTableACCOUNTPAYABLE = mCurrentRfcFunction.GetTable("POCOND");
            rfcTableACCOUNTPAYABLE.Insert();
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("ITM_NUMBER", poCond.ITM_NUMBER);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("COND_TYPE", poCond.COND_TYPE);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("COND_VALUE", poCond.COND_VALUE);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("CHANGE_ID", poCond.CHANGE_ID);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("CURRENCY", poCond.CURRENCY);

            IRfcTable rfcTableACCOUNTPAYABLEX = mCurrentRfcFunction.GetTable("POCONDX");
            rfcTableACCOUNTPAYABLEX.Insert();
            rfcTableACCOUNTPAYABLEX.CurrentRow.SetValue("ITM_NUMBER", poCond.ITM_NUMBER);
            rfcTableACCOUNTPAYABLEX.CurrentRow.SetValue("COND_TYPE", "X");
            rfcTableACCOUNTPAYABLEX.CurrentRow.SetValue("COND_VALUE", "X");
            rfcTableACCOUNTPAYABLEX.CurrentRow.SetValue("CHANGE_ID", "X");

        }

        /// <summary>
        /// PO Text Header
        /// </summary>
        /// <param name="poTextHeader"></param>
        protected void InsertDataPOTextHeader(POTEXTHEADER poTextHeader)
        {
            IRfcTable rfcTableACCOUNTPAYABLE = mCurrentRfcFunction.GetTable("POTEXTHEADER");
            rfcTableACCOUNTPAYABLE.Insert();
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("TEXT_ID", poTextHeader.TEXT_ID);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("TEXT_FORM", poTextHeader.TEXT_FORM);
            rfcTableACCOUNTPAYABLE.CurrentRow.SetValue("TEXT_LINE", poTextHeader.TEXT_LINE);
        }

        /// <summary>
        /// System GR Header
        /// </summary>
        /// <param name="sapNum"></param>
        protected void InsertDataGRHeader(GRHEADER grHeader)
        {
            IRfcStructure rfcStructGRHeader = mCurrentRfcFunction.GetStructure("GOODSMVT_HEADER");
            rfcStructGRHeader.SetValue("PSTNG_DATE", grHeader.PSTNG_DATE);
            rfcStructGRHeader.SetValue("DOC_DATE", grHeader.DOC_DATE);
            rfcStructGRHeader.SetValue("REF_DOC_NO", grHeader.REF_DOC_NO);
            rfcStructGRHeader.SetValue("HEADER_TXT", grHeader.HEADER_TXT);
            rfcStructGRHeader.SetValue("PR_UNAME", grHeader.PR_UNAME);
        }

        /// <summary>
        /// System GR Code
        /// </summary>
        protected void InsertDataGRCode(GRCODE grCode)
        {
            IRfcStructure rfcStructGRCode = mCurrentRfcFunction.GetStructure("GOODSMVT_CODE");
            rfcStructGRCode.SetValue("GM_CODE", grCode.GM_CODE);
        }

        /// <summary>
        /// System GR Items
        /// </summary>
        /// <param name="sapNum"></param>
        /// <param name="itemCode"></param>
        protected void InsertDataGRItem(GRITEM grItem)
        {
            IRfcTable rfcTableGRItem = mCurrentRfcFunction.GetTable("GOODSMVT_ITEM");
            rfcTableGRItem.Insert();
            rfcTableGRItem.CurrentRow.SetValue("PLANT", grItem.PLANT);
            rfcTableGRItem.CurrentRow.SetValue("MOVE_TYPE", grItem.MOVE_TYPE);
            rfcTableGRItem.CurrentRow.SetValue("ENTRY_QNT", grItem.ENTRY_QNT);
            rfcTableGRItem.CurrentRow.SetValue("ENTRY_UOM", grItem.ENTRY_UOM);
            rfcTableGRItem.CurrentRow.SetValue("PO_NUMBER", grItem.PO_NUMBER);
            rfcTableGRItem.CurrentRow.SetValue("PO_ITEM", grItem.PO_ITEM);
            rfcTableGRItem.CurrentRow.SetValue("MVT_IND", grItem.MVT_IND);
            rfcTableGRItem.CurrentRow.SetValue("ITEM_TEXT", grItem.ITEM_TEXT);
        }

        /// <summary>
        /// 修改采购订单的收货时间
        /// </summary>
        /// <param name="po"></param>
        protected void UpdateDataPODate(PO po)
        {
            mCurrentRfcFunction.SetValue("PURCHASEORDER", po.PONo);
            mCurrentRfcFunction.SetValue("DATE", po.Date);
        }

        /// <summary>
        /// 查询采购订单的收获状态
        /// </summary>
        /// <param name="po"></param>
        protected void SelectDataPODate(PO po)
        {
            mCurrentRfcFunction.SetValue("PURCHASEORDER", po.PONo);
        }

        /// <summary>
        /// 关闭SAP的连接
        /// 完成数据插入，需要调用 Invoke() 方法，相当于告知服务器完成一次数据插入
        /// </summary>
        /// <returns></returns>
        protected object[] CloseDestination()
        {
            try
            {
                mCurrentRfcFunction.Invoke(mCurrentDestination);
            }
            catch (RfcBaseException e)
            {
                StringBuilder sb = new StringBuilder();
                Exception ex = e;
                while (ex != null)
                {
                    sb.Append(" { " + ex.Message.ToString() + " } ");
                    ex = ex.InnerException;
                }
                return new object[] { false, sb.ToString() };
            }

            return new object[] { true, "" };
        }

        /// <summary>
        /// 初始化环境，创建连接SAP的连接对象
        /// </summary>
        /// <returns>创建连接SAP的连接对象是否成功</returns>
        protected virtual object[] InitializeEnvironment()
        {
            try
            {
                mCurrentDestination = RfcDestinationManager.GetDestination(mCurrentDestinationStr);
                mCurrentRfcFunction = mCurrentDestination.Repository.CreateFunction(mCurrentFunctionStr);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                Exception ex = e;
                while (ex != null)
                {
                    sb.Append(" { " + ex.Message.ToString() + " } ");
                    ex = ex.InnerException;
                }

                return new object[] { false, sb.ToString() };
            }

            return new object[] { true };
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnObjType
        {
            get { return mCurrentRfcFunction.GetString("OBJ_TYPE"); }
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnObjKey
        {
            get { return mCurrentRfcFunction.GetString("OBJ_KEY"); }
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnObjSys
        {
            get { return mCurrentRfcFunction.GetString("OBJ_SYS"); }
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnExpPurchaseOrder
        {
            get { return mCurrentRfcFunction.GetString("EXPPURCHASEORDER"); }
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnMatDocumentYear
        {
            get { return mCurrentRfcFunction.GetString("MATDOCUMENTYEAR"); }
        }

        /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnMatErialDocument
        {
            get { return mCurrentRfcFunction.GetString("MATERIALDOCUMENT"); }
        }

         /// <summary>
        /// SAP返回的参数值，具体调用位置在Completed(ref SapResult sapResult)方法中
        /// </summary>
        protected string SapReturnStatus
        {
            get { return mCurrentRfcFunction.GetString("STATUS"); }
        }

        /// <summary>
        /// 返回SAP 结果对象
        /// </summary>
        protected SapResult SapReturnResult
        {
            get { return GetSapResult(); }
        }

        /// <summary>
        /// 返回PO单的相关数据信息
        /// </summary>
        /// <returns></returns>
        protected POINFO GetPoInfo()
        {
            POINFO poInfo = new POINFO();
            poInfo.STATUS = mCurrentRfcFunction.GetString("STATUS");   
            poInfo.DATE = mCurrentRfcFunction.GetString("DATE");
            poInfo.ATWRT = mCurrentRfcFunction.GetString("ATWRT");
            poInfo.YEARP = mCurrentRfcFunction.GetString("YEAR_P");
            poInfo.WEEKP = mCurrentRfcFunction.GetString("WEEK_P");
            poInfo.YEARS = mCurrentRfcFunction.GetString("YEAR_S");
            poInfo.WEEKS = mCurrentRfcFunction.GetString("WEEK_S");
            poInfo.DMBTROSP= mCurrentRfcFunction.GetString("DMBTR_OSP");
            poInfo.DMBTROMU = mCurrentRfcFunction.GetString("DMBTR_OMU");
            poInfo.NAME = mCurrentRfcFunction.GetString("NAME");
            poInfo.MESAGE = mCurrentRfcFunction.GetString("MESSAGE");
            poInfo.POQTY = mCurrentRfcFunction.GetString("PO_QTY");
            IRfcTable irt=  mCurrentRfcFunction.GetTable("STYLE_NUMBER");
            object obj = irt.GetValue("STYLE_NUMBER");
            poInfo.STYLENUMBER = obj == null ? string.Empty : obj.ToString();
            return poInfo;
        }

        /// <summary>
        /// 返回结果值，通过 GetTable("RETURN") 方法获取结果对象
        /// </summary>
        protected virtual SapResult GetSapResult()
        {
            List<RETURN> returnList = new List<RETURN>();
            IRfcTable retTable = mCurrentRfcFunction.GetTable("RETURN");
            for (int i = 0; i < retTable.RowCount; i++)
            {
                //set the current row - the row used for Get*/Set* operations
                retTable.CurrentIndex = i;
                //Read COMP_CODE and COMP_NAME from the current row in the table
                returnList.Add(new RETURN()
                {
                    TYPE = retTable.GetString("TYPE"),
                    ID = retTable.GetString("ID"),
                    NUMBER = retTable.GetString("NUMBER"),
                    MESSAGE = retTable.GetString("MESSAGE"),
                    LOG_NO = retTable.GetString("LOG_NO"),
                    LOG_MSG_NO = retTable.GetString("LOG_MSG_NO"),
                    MESSAGE_V1 = retTable.GetString("MESSAGE_V1"),
                    MESSAGE_V2 = retTable.GetString("MESSAGE_V2"),
                    MESSAGE_V3 = retTable.GetString("MESSAGE_V3"),
                    MESSAGE_V4 = retTable.GetString("MESSAGE_V4"),
                    PARAMETER = retTable.GetString("PARAMETER"),
                    ROW = retTable.GetString("ROW"),
                    FIELD = retTable.GetString("FIELD"),
                    SYSTEM = retTable.GetString("SYSTEM")
                });
            }

            return new SapResult() { RETURN_LIST = returnList };
        }

        /// <summary>
        /// 发票验证
        /// </summary>
        /// <returns></returns>
        protected string GetVionceInfo()
        {
            string ss1 = mCurrentRfcFunction.GetString("INVOICEDOCNUMBER");
            string ss2 = mCurrentRfcFunction.GetString("FISCALYEAR");

            return ss1 + ss2;
        }

        #region OSP

        /// <summary>
        /// 设置得到由Style No.得到OSP信息方法的参数
        /// </summary>
        /// <param name="osparg"></param>
        protected void SetOSPSearchArg(OSPArg osparg)
        {
            mCurrentRfcFunction.SetValue("STYLE", osparg.StyleNO);
        }

        /// <summary>
        /// 返回OSP的相关数据信息
        /// </summary>
        /// <returns></returns>
        protected OSPInfo GetOSPInfo()
        {
            OSPInfo ospInfo = new OSPInfo();
            ospInfo.SUB_DIV = mCurrentRfcFunction.GetString("SUB_DIV");
            ospInfo.CLASS = mCurrentRfcFunction.GetString("CLASS");
            ospInfo.PO = mCurrentRfcFunction.GetString("PO");
            ospInfo.QTY = mCurrentRfcFunction.GetString("QTY");
            ospInfo.ORIGINAL_OSP = mCurrentRfcFunction.GetString("ORIGINAL_OSP");
            ospInfo.CURRENT_OMU = mCurrentRfcFunction.GetString("CURRENT_OMU");
            ospInfo.CREATED_BY = mCurrentRfcFunction.GetString("CREATED_BY");
            ospInfo.PAD = mCurrentRfcFunction.GetString("PAD");
            ospInfo.SAD = mCurrentRfcFunction.GetString("SAD");
            ospInfo.GR = mCurrentRfcFunction.GetString("GR");
            ospInfo.ALLOCATED_DATE = mCurrentRfcFunction.GetString("ALLOCATED_DATE");
            ospInfo.COST = mCurrentRfcFunction.GetString("COST");
            ospInfo.MESSAGE = mCurrentRfcFunction.GetString("MESSAGE");
            return ospInfo;
        }


        /// <summary>
        /// 设置修改OSP的参数
        /// </summary>
        /// <param name="po"></param>
        protected void SetUpdateOSPPars(OSPArg osparg)
        {
            mCurrentRfcFunction.SetValue("STYLE", osparg.StyleNO);
            mCurrentRfcFunction.SetValue("NEW_OSP", osparg.Price);
        }
        #endregion
        #region POTypeChange
        /// <summary>
        /// 设置得到由Style No.得到OSP信息方法的参数
        /// </summary>
        /// <param name="osparg"></param>
        protected void SetPOTypeChangeArg(POTypeChangeArp POTCarg)
        {
            mCurrentRfcFunction.SetValue("PURCHASEORDER", POTCarg.Number);
        }


        /// <summary>
        /// 返回OSP的相关数据信息
        /// </summary>
        /// <returns></returns>
        protected POTypeChangeInfo GetPOTypeChangeInfo()
        {
            POTypeChangeInfo POTypeChangeInfo = new POTypeChangeInfo();
            POTypeChangeInfo.PAD = mCurrentRfcFunction.GetString("PAD");
            POTypeChangeInfo.SAD = mCurrentRfcFunction.GetString("SAD");
            POTypeChangeInfo.OMU = mCurrentRfcFunction.GetString("OMU");
            POTypeChangeInfo.Qty = mCurrentRfcFunction.GetString("QUANTITY");
            POTypeChangeInfo.IsAllocated = mCurrentRfcFunction.GetString("IS_ALLOCATED");
            POTypeChangeInfo.SMessage = mCurrentRfcFunction.GetString("MESSAGE");
            return POTypeChangeInfo;
        }


        /// <summary>
        /// 设置修改OSP的参数
        /// </summary>
        /// <param name="po"></param>
        protected void SetPOTypeChangePars(POTypeChangeArp POTCarg)
        {
            mCurrentRfcFunction.SetValue("PURCHASEORDER", POTCarg.Number);
            mCurrentRfcFunction.SetValue("STORAGE_LOCATION", POTCarg.NewType);
        }
        #endregion

    }
}
