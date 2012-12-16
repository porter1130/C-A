using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Exchange;
using SAP.Middleware.Table;

namespace CA.WorkFlow.Common.SAP.Test
{
    class Program
    {
        static string mSapNum = string.Empty;

        static void Main(string[] args)
        {
            InitializeData();

            Console.WriteLine("Strating...");
            Console.WriteLine("");

            //DateTime dt1 = DateTime.Now;
            //Console.WriteLine("第一次启动时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            //DateTime dt2 = DateTime.Now;
            //Console.WriteLine("第一次结束时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"));
            //Console.WriteLine("第一次耗费时间：" + (dt2 - dt1).TotalMilliseconds);

            ////TravelClaim
            //ISapExchange tec1 = SapExchangeFactory.GetTravelClaim();
            //List<object[]> result = tec1.ImportDataToSap(mSapParametersTR);
            //ProcessResult(result);
            //Console.ReadKey();

            //EmployeeClaim
            ISapExchange tec2 = SapExchangeFactory.GetEmployeeClaim();
            List<object[]> result2 = tec2.ImportDataToSap(mSapParametersEC);
            ProcessResult(result2);
            Console.ReadKey();

            ////CashAdvance
            //ISapExchange tec3 = SapExchangeFactory.GetCashAdvance();
            //List<object[]> result3 = tec3.ImportDataToSap(mSapParametersCD);
            //ProcessResult(result3);
            //Console.ReadKey();

            ////CreditCard
            //ISapExchange tec4 = SapExchangeFactory.GetCreditCard();
            //List<object[]> result4 = tec4.ImportDataToSap(mSapParametersCC);
            //ProcessResult(result4);
            //Console.ReadKey();

            ////EmployeeCreditCard
            //ISapExchange tec5 = SapExchangeFactory.GetEmployeeCCClaim();
            //List<object[]> result5 = tec5.ImportDataToSap(mSapParametersECC);
            //ProcessResult(result5);
            //Console.ReadKey();

            //ISapExchange tec6 = SapExchangeFactory.GetPurchaseOrder();
            //List<object[]> result6 = tec6.ImportDataToSap(mSapParametersPO);
            //ProcessResult(result6);
            //Console.ReadKey();

            //ISapExchange tec7 = SapExchangeFactory.GetPurchaseOrderReturn();
            //List<object[]> result7 = tec7.ImportDataToSap(mSapParametersPOR);
            //ProcessResult(result7);
            //Console.ReadKey();

            //StroeRecives
            //ISapExchange tec8 = SapExchangeFactory.GetStoresReceive();
            //List<object[]> result8 = tec8.ImportDataToSap(mSapParametersGR);
            //ProcessResult(result8);
            //Console.ReadKey();

            ////Payment Request
            //ISapExchange tec9 = SapExchangeFactory.GetStoresReceive();
            //List<object[]> result9 = tec9.ImportDataToSap(mSapParametersPR);
            //ProcessResult(result9);
            //Console.ReadKey();

            //Payment Request Query
            //ISapExchange tec10 = SapExchangeFactory.GetPaymentRequestQuery();
            //List<object[]> result10 = tec10.ImportDataToSap(mSapParametersPRQ);
            //for (int i = 0; i < result10.Count; i++)
            //{
            //    if ((bool)result10[i][2])
            //    {
            //        SapResult sr = (SapResult)result10[i][1];
            //        if (sr.OBJ_PRINFO.ZFLAG == "X")
            //        {
            //            Console.WriteLine("Current is paid");
            //        }
            //    }
            //    else
            //    {
            //        if (result10[i][1] is string)
            //        {
            //            Console.WriteLine(result10[i][1].ToString());
            //        }
            //        else
            //        {
            //            string ss = "";
            //            SapResult sr = (SapResult)result10[i][1];
            //            for (int j = 0; j < sr.RETURN_LIST.Count; j++)
            //            {
            //                ss += sr.RETURN_LIST[j].MESSAGE;
            //            }

            //            Console.WriteLine(ss);
            //        }
            //    }
            //}

            //Console.ReadKey();


            //ISapExchange tec9 = SapExchangeFactory.GetPurchaseOrderMod();
            //List<object[]> result9 = tec9.ImportDataToSap(new List<SapParameter>() { new SapParameter() { SapNumber = "8500000828", DocDate = "2011-12-25" } });
            //for (int i = 0; i < result9.Count; i++)
            //{
            //    bool bl = (bool)result9[i][2];
            //    if (bl)
            //    {
            //        SapResult sr = (SapResult)result9[i][1];
            //        if (sr.OBJ_POINFO.STATUS == "Y")
            //        {
            //        }
            //    }
            //    else
            //    {
            //        if (result9[i][1] is string)
            //        {
            //            Console.WriteLine(result9[i][1].ToString());
            //        }
            //        else
            //        {
            //            string ss = "";
            //            SapResult sr = (SapResult)result9[i][1];
            //            for (int j = 0; j < sr.RETURN_LIST.Count; j++)
            //            {
            //                ss += sr.RETURN_LIST[j].MESSAGE;
            //            }

            //            Console.WriteLine(ss);
            //        }
            //    }
            //}

            //Console.ReadKey();



            



        }

        /// <summary>
        /// 
        /// </summary>
        private static List<SapParameter> mSapParametersTR = new List<SapParameter>();
        private static List<SapParameter> mSapParametersEC = new List<SapParameter>();
        private static List<SapParameter> mSapParametersCD = new List<SapParameter>();
        private static List<SapParameter> mSapParametersCC = new List<SapParameter>();
        private static List<SapParameter> mSapParametersECC = new List<SapParameter>();
        private static List<SapParameter> mSapParametersPO = new List<SapParameter>();
        private static List<SapParameter> mSapParametersPOR = new List<SapParameter>();
        private static List<SapParameter> mSapParametersGR = new List<SapParameter>();
        private static List<SapParameter> mSapParametersPR = new List<SapParameter>();
        private static List<SapParameter> mSapParametersPRQ = new List<SapParameter>();

        /// <summary>
        /// 初始化数据
        /// </summary>
        static void InitializeData()
        {
            //Travel Request Claim 数据
            SapParameter mSapParameters1 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                EmployeeID = "0039000001",  //
                EmployeeName = "TEST",        //
                ExchRate = 1,
                Header = "Travel Expense Claim",
                RefDocNo = "CA" + DateTime.Now.ToString("yyyyMMddHHmmss"),// 
                UserName = "ewf",
                CashAmount = 100,
                RefDocNo1 = "",
                PaidByCC = 100
            };

            List<ExpenceDetail> expenceDetails1 = new List<ExpenceDetail>();
            expenceDetails1.Add(new ExpenceDetail() { Amount = 11.0478m });
            //expenceDetails1.Add(new ExpenceDetail() { Amount = 100 });
            //expenceDetails1.Add(new ExpenceDetail() { Amount = 100 });
            //expenceDetails1.Add(new ExpenceDetail() { Amount = 100 });
            //expenceDetails1.Add(new ExpenceDetail() { RefKey = "201112", Amount = 100, CompanyStd = 50, IsApproved = false, IsPaidByCC = true });
            //expenceDetails1.Add(new ExpenceDetail() { Amount = 100 });

            List<ExpenceDetail> groupExpenceDetails1 = new List<ExpenceDetail>();
            groupExpenceDetails1.Add(new ExpenceDetail() { AccountGL = "0011020100", Amount = 11.0478m, CostCenter = "D001", ItemText = "EricZheng Travel_hotel" });
            //groupExpenceDetails1.Add(new ExpenceDetail() { AccountGL = "0011020100", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Travel_meal" });
            //groupExpenceDetails1.Add(new ExpenceDetail() { AccountGL = "0015511004", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Travel_local_transportation" });
            //groupExpenceDetails1.Add(new ExpenceDetail() { AccountGL = "0015511003", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Travel_others" });
            //groupExpenceDetails1.Add(new ExpenceDetail() { AccountGL = "0015511003", Amount = 150, CostCenter = "D001", ItemText = "EricZheng Sample_purchase" });

            mSapParameters1.GroupExpenceDetails = groupExpenceDetails1;
            mSapParameters1.ExpenceDetails = expenceDetails1;
            mSapParametersTR.Add(mSapParameters1);


            string ewfNo = DateTime.Now.ToString("HHmmss");
            //Employee Claim 数据
            SapParameter mSapParameters2 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                //EmployeeID = "0000008047",  //申请人ID
                EmployeeID = "0099999999",  //申请人ID
                EmployeeName = "TEST",      //申请人名字
                Header = "Payment Request", //工作流名字
                RefDocNo = ewfNo, //工作流ID
                UserName = "ewf",
            };

           // Console.WriteLine(ewfNo);

            List<ExpenceDetail> expenceDetails2 = new List<ExpenceDetail>();
            expenceDetails2.Add(new ExpenceDetail()
            {
                AccountGL = "15171500",
                ExchRate = 1m,
                Amount = 1200m,
                CostCenter = "S008",
                ItemText = "Payment Request"
            });
            expenceDetails2.Add(new ExpenceDetail()
            {
                AccountGL = "15171500",
                ExchRate = 1m,
                Amount = 1300m,
                CostCenter = "S008",
                ItemText = "Payment Request"
            });
            expenceDetails2.Add(new ExpenceDetail()
            {
                AccountGL = "12210102",
                ExchRate = 1m,
                Amount = 425m,
                CostCenter = "",
                ItemText = "Payment Request"
            });

            //  expenceDetails2.Add(new ExpenceDetail() { AccountGL = "15511200", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Travel_local_transportation" });
            //  expenceDetails2.Add(new ExpenceDetail() { AccountGL = "15511003", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Travel_others" });
            // expenceDetails2.Add(new ExpenceDetail() { AccountGL = "15511406", Amount = 100, CostCenter = "D001", ItemText = "EricZheng Sample_purchase" });
            mSapParameters2.VendorInfo = new Vendor()
            {
                BankAcct = "234567893456789345678",
                BankCity = "CN",
                BankNo = "104290003033",
                City = "上海",
                Country = "CN",
                Name = "上海文思创新科技有限公司-上海文思创新科技有限公司"
            };
            mSapParameters2.ExpenceDetails = expenceDetails2;
            mSapParametersEC.Add(mSapParameters2);


            //Cash Advance 数据
            SapParameter mSapParameters3 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                EmployeeID = "600011069",  //
                EmployeeName = "TEST",        //
                ExchRate = 1,
                Header = "Cash Advance",
                RefDocNo = "CA" + DateTime.Now.ToString("yyyyMMddHHmmss"),// 
                UserName = "ewf",
                CashAmount = 200,                                      //
                PaidByCC = 100
            };
            mSapParametersCD.Add(mSapParameters3);


            //Credit Card 数据
            SapParameter mSapParameters4_1 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                EmployeeID = "6000000069",  //
                EmployeeName = "Test",     //
                ExchRate = 1,
                Header = "Credit Card", //工作流名字
                RefDocNo = "CACC1234567_1",  //
                UserName = "acnotes",
                BankName = "中国工商银行上海市分行第二营业部"
            };

            List<ExpenceDetail> expenceDetails4_1 = new List<ExpenceDetail>();
            expenceDetails4_1.Add(new ExpenceDetail()
            {
                RefKey = "10011",
                EmpID = "6000000069",
                Amount = 225,
                ItemText = "2342_225"
            });
            expenceDetails4_1.Add(new ExpenceDetail()
            {
                RefKey = "10012",
                EmpID = "6000000069",
                Amount = 100,
                ItemText = "2342_100"
            });
            expenceDetails4_1.Add(new ExpenceDetail()
            {
                RefKey = "10013",
                EmpID = "",
                Amount = -300,
                ItemText = "2342_-300"
            });
            mSapParameters4_1.ExpenceDetails = expenceDetails4_1;

            ////Credit Card 数据
            //SapParameter mSapParameters4_2 = new SapParameter()
            //{
            //    BusAct = "RFBU",
            //    CompCode = "CA10",
            //    DocType = "KR",
            //    BusArea = "0001",
            //    Currency = "USD",
            //    EmployeeID = "6000000069",  //
            //    EmployeeName = "TEST",        //
            //    ExchRate = 0,
            //    Header = "Credit Card", //工作流名字
            //    // RefDocNo = "EEC" + DateTime.Now.ToString("yyyyMMddHHmmss"), //工作流ID
            //    RefDocNo = "CA0000002_2",
            //    BankName = "工商银行美国分行"
            //};

            //List<ExpenceDetail> expenceDetails4_2 = new List<ExpenceDetail>();
            //expenceDetails4_2.Add(new ExpenceDetail() { RefKey = "1002", Amount = -200, ItemText = "迅销（中国）商贸有限公司中山公园店" });

          //  mSapParameters4_2.ExpenceDetails = expenceDetails4_2;
            mSapParametersCC.Add(mSapParameters4_1);
          //  mSapParametersCC.Add(mSapParameters4_2);


            //Emplyee Credit Card 数据
            SapParameter mSapParameters5 = new SapParameter()
            {
                BusAct = "RFBU",
                CompCode = "CA10",
                DocType = "KR",
                BusArea = "0001",
                Currency = "RMB",
                // EmployeeID = "6000000150",  //申请人ID
                EmployeeID = "6000000069",
                EmployeeName = "TEST",      //申请人名字
                ExchRate = 0,
                Header = "Emplyee Credit Card", //工作流名字
                RefDocNo = "EEC" + DateTime.Now.ToString("yyyyMMddHHmmss"), //工作流ID
                UserName = "ewf"
            };

            List<ExpenceDetail> expenceDetails5 = new List<ExpenceDetail>();
            expenceDetails5.Add(new ExpenceDetail()
            {
                RefKey = "10011",
                Currency = "RMB",
                AccountGL = "0015511002",
                Amount = 225,
                CostCenter = "D001",
                ItemText = "EricZheng Travel_hotel"
            });
            expenceDetails5.Add(new ExpenceDetail()
            {
                RefKey = "10012",
                Currency = "RMB",
                AccountGL = "0015511004",
                Amount = 100,
                CostCenter = "D001",
                ItemText = "EricZheng Travel_meal"
            });
            expenceDetails5.Add(new ExpenceDetail()
            {
                RefKey = "10013",
                Currency = "RMB",
                AccountGL = "0015511406",
                Amount = -300,
                CostCenter = "D001",
                ItemText = "EricZheng Travel_local_transportation"
            });
            //expenceDetails5.Add(new ExpenceDetail() { RefKey = "1004", AccountGL = "0015511003", Amount = -100, CostCenter = "D001", ItemText = "EricZheng Travel_others" });
            //expenceDetails5.Add(new ExpenceDetail() { RefKey = "1005", AccountGL = "0015511406", Amount = 600, CostCenter = "D001", ItemText = "EricZheng Sample_purchase" });
            mSapParameters5.ExpenceDetails = expenceDetails5;
            mSapParametersECC.Add(mSapParameters5);


            string wfNumber = "PO" + DateTime.Now.ToString("yyyyMMddHHmm");
            //Purchase Order 数据
            SapParameter mSapParameters6 = new SapParameter()
            {
                DocType = "SA",
                Pmnttrms = "Z002",
                PurGroup = "NT1", //如果是固定资产，PurGroup的值为AST；否则为 NT1
                DocDate = DateTime.Now.ToString("yyyyMMdd"),
                Vendor = "0000008047",
                PaymentCond = "TEST", //分期付款信息
                RefDocNo = wfNumber,
                Currency = "RMB"
            };
          
            List<PurchaseOrderItem> purchaseOrder1 = new List<PurchaseOrderItem>();
            purchaseOrder1.Add(new PurchaseOrderItem()
            {
                ItemNo = 1, //数据中唯一的ID
                MatlGroup = "ZNT_STSTY",  //ZNT_OTHER
                Quantity = 121.11m,
                CondValue = 81,
                AssetNo = "",  //001130000005如果是固定资产，AssetNo的值为实际的AssetNo值；否则为空
                CostCenter = "H1012213",  //如果是固定资产，CostCenter的值为 空；否则为实际的CostCenter值 
                Acctasscat = "K", //如果是固定资产，Acctasscat的值为 A；否则为 K
                TaxCode = "J1",   //如果是固定资产，TaxCode的值为JO(无税)；否则为 J1 (有税)
                Description = "test test",
                Currency = "RMB"
            }
             );

            //purchaseOrder1.Add(new PurchaseOrderItem()
            //{
            //    ItemNo = 1, //数据中唯一的ID
            //    MatlGroup = "ZNT_OTHER", //如果是固定资产，MatlGroup的值为"ZNT_OTHER"固定值；否则AssetNo 关联的MatlGroup值
            //    Quantity = 1,
            //    CondValue = 10,
            //    AssetNo = "001210000001",  //如果是固定资产，AssetNo的值为实际的AssetNo值；否则为空
            //    CostCenter = "",  //如果是固定资产，CostCenter的值为 空；否则为实际的CostCenter值 
            //    Acctasscat = "A", //如果是固定资产，Acctasscat的值为 A；否则为 K
            //    TaxCode = "J0",   //如果是固定资产，TaxCode的值为JO(无税)；否则为 J1 (有税)
            //    Description = "test test",
            //    Currency = "USD"
            //}
            //    );

            mSapParameters6.PurchaseOrderItems = purchaseOrder1;
            mSapParametersPO.Add(mSapParameters6);


            //Purchase Order Return 数据
            SapParameter mSapParameters7 = new SapParameter()
            {
                DocType = "SA",
                SapNumber = "6500000142"
            };

            List<PurchaseOrderItem> purchaseOrder2 = new List<PurchaseOrderItem>();
            purchaseOrder2.Add(new PurchaseOrderItem() { Quantity = 8, ItemNo = 1 });
            mSapParameters7.PurchaseOrderItems = purchaseOrder2;
            mSapParametersPOR.Add(mSapParameters7);

            //Stores Receive 数据
            SapParameter mSapParameters8 = new SapParameter()
            {
                DocDate = "20120203",
                RefDocNo = "6500000201",
                Header = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            List<StoresReceiveItem> storesReceive = new List<StoresReceiveItem>();
            storesReceive.Add(new StoresReceiveItem()
            {
                Quantity = 9,
                SapNumber = "6500000201",
                ItemNo = 1
            });
            storesReceive.Add(new StoresReceiveItem()
            {
                Quantity = 8,
                SapNumber = "6500000201",
                ItemNo = 2
            });
            mSapParameters8.StoresReceiveItems = storesReceive;
            mSapParametersGR.Add(mSapParameters8);


            //Payment Request 数据
            SapParameter mSapParameters9 = new SapParameter()
            {
                DocDate = "20120216",
                RefDocNo = "6500001595",
                Header = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            List<StoresReceiveItem> paymentRequest = new List<StoresReceiveItem>();
            paymentRequest.Add(new StoresReceiveItem()
            {
                Quantity = 1,
                SapNumber = "6500001595",
                ItemNo = 1
            });
            //paymentRequest.Add(new StoresReceiveItem()
            //{
            //    Quantity = 121M,
            //    SapNumber = "6500001547",
            //    ItemNo = 2
            //});
            //paymentRequest.Add(new StoresReceiveItem()
            //{
            //    Quantity = 121M,
            //    SapNumber = "6500001547",
            //    ItemNo = 3
            //});
            mSapParameters9.StoresReceiveItems = paymentRequest;
            mSapParametersPR.Add(mSapParameters9);

            //Purchase Order Return 数据
            SapParameter mSapParameters10 = new SapParameter()
            {
                RefDocNo = "170104",
                Vendor = "6000000069"
            };
            mSapParametersPRQ.Add(mSapParameters10);

        }

        private static void ProcessResult(List<object[]> sapResult)
        {
            for (int i = 0; i < sapResult.Count; i++)
            {
                bool bl = (bool)sapResult[i][2];
                SapParameter sp = (SapParameter)sapResult[i][0];
                Console.WriteLine(sp.RefDocNo + " : " + bl.ToString());
                if (bl)
                {
                    SapResult sr = (SapResult)sapResult[i][1];
                    Console.WriteLine("Sap ID : " + sr.OBJ_KEY);
                    mSapNum = sr.OBJ_KEY;
                }
                else
                {
                    if (sapResult[i][1] is string)
                    {
                        Console.WriteLine(sapResult[i][1].ToString());
                    }
                    else
                    {
                        SapResult sr = (SapResult)sapResult[i][1];
                        ShowResult(sr);
                    }
                }
            }
        }

        /// <summary>
        /// 输出结果到控制台
        /// </summary>
        public static void ShowResult(SapResult sapResult)
        {
            Console.WriteLine("Function finished:");
            foreach (RETURN ret in sapResult.RETURN_LIST)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine(
                    "TYPE: " + ret.TYPE + '\n' +
                    "ID: " + ret.ID + '\n' +
                    "Number: " + ret.NUMBER + '\n' +
                    "LOG_NO: " + ret.LOG_NO + '\n' +
                    "LOG_MSG_NO: " + ret.LOG_MSG_NO + '\n' +
                    "MESSAGE_V1: " + ret.MESSAGE_V1 + '\n' +
                    "MESSAGE_V2: " + ret.MESSAGE_V2 + '\n' +
                    "MESSAGE_V3: " + ret.MESSAGE_V3 + '\n' +
                    "MESSAGE_V4: " + ret.MESSAGE_V4 + '\n' +
                    "PARAMETER: " + ret.PARAMETER + '\n' +
                    "ROW: " + ret.ROW + '\n' +
                    "FIELD: " + ret.FIELD + '\n' +
                    "SYSTEM: " + ret.SYSTEM + '\n' +
                    "MESSAGE: " + ret.MESSAGE
                );
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("OBJ_TYPE: " + sapResult.OBJ_TYPE);
            Console.WriteLine("OBJ_KEY: " + sapResult.OBJ_KEY);
            Console.WriteLine("OBJ_SYS：" + sapResult.OBJ_SYS);

            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
        }
    }
}
      
