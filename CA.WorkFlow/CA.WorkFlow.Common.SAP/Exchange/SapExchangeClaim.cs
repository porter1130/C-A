using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Table;
using SAP.Middleware.Connector;
using System.IO;

namespace SAP.Middleware.Exchange
{
    /// <summary>
    /// 报销的抽象类，应用“模板方法”设计模式
    /// 包括：出差申请报销、员工报销、员工信用卡报销 会继承此类，实现类中的抽象方法
    /// 数据需要插入到SAP中的三个表
    /// 第一个表：费用类型的明细
    /// 第二个表：费用支付给谁
    /// 第三个表：费用额
    /// </summary>
    internal abstract class SapExchangeClaim : SapExchange
    {

        private int mGlobalCount = 0;
        /// <summary>
        /// 第一个表和第二个表插入的数据记录标识
        /// </summary>
        protected int GlobalCount
        {
            set
            {
                mGlobalCount = value;
            }
            get
            {
                return mGlobalCount;
            }
        }

        private List<object[]> mFristTable = new List<object[]>();
        /// <summary>
        /// 存储SAP第一张表的相关数据，第一个表：ACCOUNTGL
        /// </summary>
        protected List<object[]> FristTable
        {
            set
            {
                mFristTable = value;
            }
            get
            {
                return mFristTable;
            }
        }
        private List<object[]> mSecondTable = new List<object[]>();
        /// <summary>
        /// 存储SAP第二张表的相关数据，第二个表：ACCOUNTPAYABLE
        /// </summary>
        protected List<object[]> SecondTable
        {
            set
            {
                mSecondTable = value;
            }
            get
            {
                return mSecondTable;
            }
        }
        private List<object[]> mThirdTable = new List<object[]>();
        /// <summary>
        /// 存储SAP第三张表的相关数据，第三个表：CURRENCYAMOUNT
        /// </summary>
        protected List<object[]> ThirdTable
        {
            set
            {
                mThirdTable = value;
            }
            get
            {
                return mThirdTable;
            }
        }

        /// <summary>
        /// 清空全局变量值
        /// </summary>
        protected void InitParameter()
        {
            mFristTable.Clear();
            mSecondTable.Clear();
            mThirdTable.Clear();
            mGlobalCount = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override void OperationForSap()
        {
            InitParameter();
            SetDocumentHeader();
            //当EmployeeID为0099999999时，标示为一次性供应商，需要插入如下信息到SAP
            if (CurrentSapParameter.EmployeeID == "0099999999")
            {
                string name = CurrentSapParameter.VendorInfo.Name;
                string account = CurrentSapParameter.VendorInfo.BankAcct;
                InsertVendor(new VENDOR()
                {
                    BANK_ACCT = CurrentSapParameter.VendorInfo.BankAcct,
                    BANK_NO = CurrentSapParameter.VendorInfo.BankNo,
                    BANK_CTRY = CurrentSapParameter.VendorInfo.BankCity,
                    CITY = CurrentSapParameter.VendorInfo.City,
                    COUNTRY = CurrentSapParameter.VendorInfo.Country,
                    NAME = CurrentSapParameter.VendorInfo.Name,
                    NAME_2 = name.ToCharArray().Length > 18 ? name.Substring(18, name.ToCharArray().Length - 18) : "",
                    BKREF = account.ToCharArray().Length > 18 ? account.Substring(18, account.ToCharArray().Length - 18) : ""
                });
            }
            SetAccountGL();
            SetAccountPayable();
            SetCurrencyAmount();
            Completed();
        }

        /// <summary>
        /// 插入数据到第一个表：AccountGL
        /// </summary>
        protected void SetAccountGL()
        {
            List<ExpenceDetail> expList = GetExpenceDetails();
            if (expList != null)
            {
                decimal totalAmount = 0;
                decimal taxTotalAmount = 0;
                bool isHaveTaxValue = false;

                foreach (var expen in expList)
                {   
                    mGlobalCount++;

                    //当CostCenter为0000000000时，标示为税额
                   if (expen.CostCenter == "0000000000")
                    {
                        isHaveTaxValue = true;
                        taxTotalAmount = expen.Amount;
                    }
                   else
                    {
                        ACCOUNTGL account = new ACCOUNTGL()
                        {
                            BUS_AREA = (string.IsNullOrEmpty(expen.CostCenter) ? expen.BusArea : ""),
                            ITEMNO_ACC = mGlobalCount,
                            GL_ACCOUNT = expen.AccountGL,
                            ITEM_TEXT = expen.ItemText,
                            COSTCENTER = expen.CostCenter
                        };
                        decimal amount = expen.Amount * (expen.ExchRate == 0 ? 1 : expen.ExchRate);
                        //存储object数组值
                        object[] obj = { mGlobalCount, amount, 0 };
                        mFristTable.Add(obj);
                        totalAmount += amount;
                        //插入数据到SAP
                        InsertDataAccountGL(account);
                    }
                }
                
                //当存在税额时，需要单独插入税额数据
                if (isHaveTaxValue == true)
                {
                    ACCOUNTTAX account = new ACCOUNTTAX()
                    {
                        ITEMNO_ACC = mGlobalCount
                    };
                    //插入数据到SAP
                    InsertDataAccountTax(account);
                    //存储object数组值
                    object[] obj = { mGlobalCount, taxTotalAmount, totalAmount };
                    mFristTable.Add(obj);
                }
            }
        }

        /// <summary>
        /// 插入数据到第二个表：AccountPayable
        /// </summary>
        protected void SetAccountPayable()
        {
            decimal cashAmount = GetCashAmount();
            decimal totalAmount = GetTotalAmount();
            decimal paidByCCAmount = GetPaidByCCAmount();

            //   string refDocNo = CurrentSapParameter.RefDocNo;
            // string itemText = CurrentSapParameter.EmployeeName + " Employee Vendor";
            string itemText = CurrentSapParameter.Header;

            //当信用卡和现金支付小于、大于总金额的时候，公司需要支付钱给报销申请人，需要插入多一行数据
            //当信用卡和现金支付等于总金额的时候，公司不需要支付钱给报销申请人，不需要插入多一行数据
            if (cashAmount + paidByCCAmount < totalAmount)
            {
                mSecondTable.Add(new object[] { ++mGlobalCount, -(totalAmount - cashAmount - paidByCCAmount) });
                SetAccountPayable(mGlobalCount, "", CurrentSapParameter.RefDocNo, "", itemText);
            }
            else if (cashAmount + paidByCCAmount > totalAmount)
            {
                mSecondTable.Add(new object[] { ++mGlobalCount, cashAmount + paidByCCAmount - totalAmount });
                SetAccountPayable(mGlobalCount, "V", CurrentSapParameter.RefDocNo, "", itemText);
            }

            //信用卡支付
            List<object[]> paidByCC = GetPaidByCC();
            if (paidByCC != null)
            {
                // itemText = CurrentSapParameter.EmployeeName + " Credit Card";
                foreach (var cc in paidByCC)
                {
                    mSecondTable.Add(new object[] { ++mGlobalCount, (decimal)cc[2] });
                    SetAccountPayable(mGlobalCount, "Y", (string)cc[0], (string)cc[1], itemText);
                }
            }

            //现金支付（借款）
            List<object[]> cashAdvances = GetCashAdvance();
            if (cashAdvances != null)
            {
                // itemText = CurrentSapParameter.EmployeeName + " Cash Advance";
                foreach (var cash in cashAdvances)
                {
                    SecondTable.Add(new object[] { ++GlobalCount, -(decimal)cash[1] });
                    SetAccountPayable(GlobalCount, "V", (string)cash[0], "", itemText);
                }
            }
        }

        /// <summary>
        /// 插入数据到第三个表：CurrencyAmount
        /// </summary>
        protected void SetCurrencyAmount()
        {
            //插入第一个临时表中的数据到SAP中的“CurrencyAmount”
            for (int i = 0; i < FristTable.Count; i++)
            {
                int ITEMNOACC = int.Parse(FristTable[i][0].ToString());
                decimal AMTDOCCUR = decimal.Parse(FristTable[i][1].ToString());
                decimal TAXVALUE = decimal.Parse(FristTable[i][2].ToString());
                SetCurrencyAmount(ITEMNOACC, AMTDOCCUR, TAXVALUE);
            }

            //插入第二个临时表中的数据到SAP中的“CurrencyAmount”
            for (int i = 0; i < SecondTable.Count; i++)
            {
                int ITEMNOACC = int.Parse(SecondTable[i][0].ToString());
                decimal AMTDOCCUR = decimal.Parse(SecondTable[i][1].ToString());
                SetCurrencyAmount(ITEMNOACC, AMTDOCCUR, 0);
            }
        }



        /// <summary>
        /// 插入数据到第二个表：AccountPayable
        /// </summary>
        /// <param name="GlobalCount"></param>
        /// <param name="SPGLIND"></param>
        /// <param name="ITEMTEXT"></param>
        protected virtual void SetAccountPayable(int globalCount, string SPGLIND, string REFKEY, string ALLOCNMBR, string ITEMTEXT)
        {
            ACCOUNTPAYABLE accountPay = new ACCOUNTPAYABLE()
            {
                PMNTTRMS = (string.IsNullOrEmpty(SPGLIND) ? "*" : ""),
                ALLOC_NMBR = ALLOCNMBR,
                SP_GL_IND = SPGLIND,
                ITEM_TEXT = ITEMTEXT,
                REF_KEY_1 = REFKEY,
                ITEMNO_ACC = globalCount,
                VENDOR_NO = CurrentSapParameter.EmployeeID,
                BUS_AREA = CurrentSapParameter.BusArea
            };

            InsertDataAccountPlayble(accountPay);
        }

        /// <summary>
        /// 插入数据到第三个表：CurrencyAmount
        /// </summary>
        /// <param name="ITEMNOACC"></param>
        /// <param name="AMTDOCCUR"></param>
        protected void SetCurrencyAmount(int ITEMNOACC, decimal AMTDOCCUR, decimal TAXVALUE)
        {
            CURRENCYAMOUNT currAmount = new CURRENCYAMOUNT()
            {
                ITEMNO_ACC = ITEMNOACC,
                AMT_DOCCUR = AMTDOCCUR,
                CURRENCY = CurrentSapParameter.Currency,
                EXCH_RATE = CurrentSapParameter.ExchRate
            };

            InsertDataCurrencyAmount(currAmount, TAXVALUE);
        }

        /// <summary>
        /// 插入头数据
        /// </summary>
        protected void SetDocumentHeader()
        {
            DOCUMENTHEADER header = new DOCUMENTHEADER()
            {
                USERNAME = CurrentSapParameter.UserName,
                COMP_CODE = CurrentSapParameter.CompCode,
                DOC_TYPE = CurrentSapParameter.DocType,
                REF_DOC_NO = CurrentSapParameter.RefDocNo,
                HEADER_TXT = CurrentSapParameter.Header,
                BUS_ACT = CurrentSapParameter.BusAct,
                DOC_DATE = Convert.ToDateTime(CurrentSapParameter.DocDate),
                PSTNG_DATE = DateTime.Now
            };

            InsertDataDocumentHeader(header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapResult"></param>
        /// <returns></returns>
        protected override bool Completed(ref SapResult sapResult)
        {
            sapResult = SapReturnResult;
            if (SapReturnObjKey != "$" && string.IsNullOrEmpty(SapReturnObjKey) == false)
            {
                sapResult.OBJ_KEY = SapReturnObjKey;
                sapResult.OBJ_SYS = SapReturnObjSys;
                sapResult.OBJ_TYPE = SapReturnObjType;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 出差报销和员工报销中，计算总金额的方式不一样
        /// </summary>
        /// <returns></returns>
        protected abstract decimal GetTotalAmount();

        /// <summary>
        /// 出差报销和员工报销中 CashAdvance 数据值插入的方式不一样，员工报销中会存在多项 CashAdvance，返回CashAdvance数据项
        /// </summary>
        protected abstract List<object[]> GetCashAdvance();

        /// <summary>
        /// 出差报销和员工报销中 CashAmount 取值方式不一样,返回 CashAmount 计算后的总额
        /// </summary>
        /// <returns></returns>
        protected abstract decimal GetCashAmount();

        /// <summary>
        /// 出差报销和员工报销中 PadiByCC 取值方式不一样,返回 PadiByCC 计算后的总额
        /// </summary>
        /// <returns></returns>
        protected abstract decimal GetPaidByCCAmount();

        /// <summary>
        /// 出差报销和员工报销中，第二个表中插入费用类型的方式不一样
        /// </summary>
        /// <returns></returns>
        protected abstract List<ExpenceDetail> GetExpenceDetails();

        /// <summary>
        /// 出差报销和员工报销中，第二个表中插入信用卡数据的方式不一样
        /// </summary>
        /// <returns></returns>
        protected abstract List<object[]> GetPaidByCC();
    }
}
