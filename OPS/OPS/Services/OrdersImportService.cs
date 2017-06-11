using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OPS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class OrdersImportService
    {
        /// <summary>
        /// 檢查檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileSize"></param>
        /// <param name="fileType"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string CheckImportFileAndGetNewPath(HttpPostedFileBase file, int fileSize, string fileType, ref string filePath)
        {
            string msg = string.Empty;

            //檢查是否上傳檔案
            if (file == null)
            {
                msg = "請選擇檔案匯入!";
                return msg;
            }

            //檢查檔案大小與類型
            msg = CheckFile(file, fileSize, fileType);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                return msg;
            }

            //另外存儲檔案並回傳路徑
            filePath = SaveFileReturnPath(file);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                msg = "儲存檔案錯誤!";
                return msg;
            }

            return msg;
        }

        /// <summary>
        /// 檢查EXCEL檔案格式與大小
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string CheckFile(HttpPostedFileBase file, int sizeMB, string fileType)
        {
            string msg = string.Empty;

            //檢查檔案是否存在
            if (file == null)
            {
                msg = "請選擇檔案";
                return msg;
            }

            //檢查檔案大小
            if (file != null && file.ContentLength > (sizeMB * 1024 * 1024))
            {
                msg = $"檔案限{sizeMB}MB以下";
                return msg;
            }

            //檢查檔案類型
            String fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

            switch (fileType)
            {
                case "EXCEL":
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {
                        msg = "請上傳xls檔或xlsx檔!";
                    }
                    break;

                case "CSV":
                    if (fileExtension != ".csv")
                    {
                        msg = "請上傳csv檔!";
                    }
                    break;

                case "xlsx":
                    if (fileExtension != ".xlsx")
                    {
                        msg = "請上傳xlsx檔!";
                    }
                    break;

                default:
                    break;
            }

            return msg;
        }

        /// <summary>
        /// 儲存檔案並回傳路徑
        /// </summary>
        /// <param name="file"></param>
        /// <returns>File Path</returns>
        public static string SaveFileReturnPath(HttpPostedFileBase file)
        {
            //加入亂數，產生的數字不會重覆
            Random rnd = new Random();
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/FileUploads"), rnd.Next() + Path.GetFileName(file.FileName));
            file.SaveAs(path);

            return path;
        }

        /// <summary>
        /// 檢查上傳檔案所需的欄位NPOI
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string CheckImportFileNeedColsNPOI(string needColStr, List<ICell> checkColNames)
        {
            string msg = string.Empty;
            List<string> needCols = new List<string>();
            List<string> lackCols = new List<string>();

            needCols = needColStr.Split(',').ToList();
            lackCols = needCols.Where(x => !checkColNames.Any(y => y.StringCellValue.Equals(x))).ToList();

            if (lackCols.Count() > 0)
            {
                msg = string.Format("您所上傳的檔案缺少以下欄位『{0}』。", String.Join("；", lackCols));
            }

            return msg;
        }

        /// <summary>
        /// 檢查上傳檔案所需的欄位NPOI
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CheckImportFileNeedColsNPOI(List<string> needCols, List<ICell> checkColNames)
        {
            string msg = string.Empty;
            List<string> lackCols = new List<string>();

            lackCols = needCols.Where(x => !checkColNames.Any(y => y.StringCellValue.Equals(x))).ToList();

            if (lackCols.Count() > 0)
            {
                msg = string.Format("您所上傳的檔案缺少以下欄位『{0}』。", String.Join("；", lackCols));
            }

            return msg;
        }

        /// <summary>
        /// 商品資訊轉換成訂單明細
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public List<ImportShopeeTWDetail> ConvertToOrderDetail(string productInfo)
        {

            List<ImportShopeeTWDetail> details = new List<ImportShopeeTWDetail>();
            string[] stringSeparators = new string[] { "\r\n", "\n" };

            //先從商品資訊取出個別的商品資訊
            string[] products = productInfo.Split(stringSeparators, StringSplitOptions.None);

            //個別的商品取出數量、價錢、商品編號
            //[1] 商品名稱:Shoe goo 鞋固膠 防磨膠 修鞋膠 大包裝 透明 NMD Jordan 編織 3.7 fl oz109.4ml; 商品選項名稱:RHHA169C2; 價格: $ 390; 數量: 1; 
            foreach (var product in products)
            {
                ImportShopeeTWDetail detail = new ImportShopeeTWDetail();
                char[] charSeparators = new char[] { ':', ';' };

                string[] items = product.Split(charSeparators);

                detail.ProductName = items[1];
                detail.ProductNo = items[3].Split('-')[0].Trim();
                detail.ProductPrice = decimal.Parse(items[5].Split('$')[1].Trim());
                detail.ProductQuantity = int.Parse(items[7].Trim());

                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 將地址轉換成超商資訊
        /// </summary>
        /// <param name="consigneeAddress"></param>
        /// <returns></returns>
        public List<string> ConvertToConvenienceStoreInfo(string consigneeAddress)
        {

            //去除/t不必要之特殊字元
            consigneeAddress = consigneeAddress.Replace('\t', ' ').Replace('\r', ' ');

            string[] stringSeparators = new string[] { "\r\n", "\n" };
            List<string> CVSInfo = consigneeAddress.Split(stringSeparators, StringSplitOptions.None).ToList();

            //非正常格式(上面多一行空白，需去除掉)
            if (string.IsNullOrWhiteSpace(CVSInfo.First()))
            {
                CVSInfo.RemoveAt(0);
            }

            //門市名稱 
            CVSInfo[0] = !string.IsNullOrWhiteSpace(CVSInfo[0]) ? CVSInfo[0].Trim().Replace("[7-11]", string.Empty).Replace("[全家]", string.Empty)
                                                                : string.Empty;
            //門市電話 
            CVSInfo[1] = !string.IsNullOrWhiteSpace(CVSInfo[1])
                                                      ? CVSInfo[1].Trim()
                                                      : string.Empty;
            //門市住址
            CVSInfo[2] = !string.IsNullOrWhiteSpace(CVSInfo[2])
                                                      ? CVSInfo[2].Trim()
                                                      : string.Empty;
            //門市店號
            CVSInfo[3] = !string.IsNullOrWhiteSpace(CVSInfo[3])
                                                      ? CVSInfo[3].Trim().Replace("門市店號:", string.Empty)
                                                      : string.Empty;

            return CVSInfo;
        }

        ///// <summary>
        ///// 匯入訂單到DB的動作
        ///// </summary>
        ///// <param name="shopees"></param>
        ///// <param name="products"></param>
        ///// <returns></returns>
        //public string ImportOrders(List<ToKantOrderShopeeView> shopees, List<PD> products)
        //{
        //    //訂單狀態: 未付款 / 待出貨 / 運送中 / 已完成 / 已取消 / 退款退貨
        //    //寄送方式: 黑貓宅急便、7-11、全家
        //    //付款方式: 貨到付款、信用卡/VISA金融卡、銀行轉帳 (付款方式:信用卡/銀行轉帳/貨到付款)
        //    string msg = string.Empty;

        //    try
        //    {
        //        DateTime nowDate = DateTime.Now;
        //        int cntOD = 0;
        //        string myMEMD01 = "13"; //蝦皮的代號 康德部分,代碼13-蝦皮拍賣,需開發票隨貨出
        //        string myST00 = "01"; //倉庫代碼
        //        string myYYYYMMDD = nowDate.ToString("yyyyMMdd");
        //        string myHHMMSS = nowDate.ToString("HHmmss");
        //        List<OD> ODs = new List<OD>();

        //        List<string> existOrderNos = new List<string>();//存在的訂單編號
        //        List<string> successOrderNos = new List<string>();//成功匯入的訂單編號
        //        List<string> confirmOrders = new List<string>();//訂單狀態為 待確認

        //        //只處理待出貨狀態
        //        shopees = shopees.Where(x => x.OrderType.Contains("待出貨")).ToList();

        //        #region 取得會員編號
        //        var odMembers = shopees.Select(x => new SearchMemberIDModel()
        //        {
        //            OD50 = x.OrderNo,
        //            ReceiverName = x.ConsigneeName,
        //            ReceiverMobile = "",
        //            ReceiverPhone = x.ConsigneeTel,
        //            BuyerName = "",
        //            BuyerMobile = "",
        //            BuyerPhone = "",
        //        }).ToList();
        //        odMembers = _cosService.SearchOrCreateMemberID(odMembers, "9", "TW").ToList();
        //        #endregion

        //        //處理主檔
        //        foreach (var shopee in shopees)
        //        {
        //            string myOD00 = myMEMD01 + nowDate.ToString("yyMMddHHmmss") + (++cntOD).ToString("000"); //自訂訂單編號(康德資料來源+年月日時分秒)

        //            //檢查訂單編號是否已上傳
        //            bool isExistOrder = (from o in _db.ODs where o.OD50 == shopee.OrderNo && o.MEMD01 == myMEMD01 select o).Any();
        //            if (isExistOrder)
        //            {
        //                existOrderNos.Add(shopee.OrderNo);
        //                continue;
        //            }

        //            #region 產生主檔
        //            OD od = new OD();
        //            od.OD00 = myOD00; //訂單單號
        //            od.OD01 = myYYYYMMDD; //訂單日期(YYYYMMDD)
        //            od.MEM00 = odMembers.Where(x => shopee.OrderNo.Equals(x.OD50)).Select(x => x.MEM00).FirstOrDefault() ?? ""; //會員編號

        //            //數量總計
        //            int quantitys = shopee.ToKantOrderShopeeDetails.Select(x => x.ProductQuantity).Sum();
        //            od.OD03 = quantitys;

        //            od.OD04 = shopee.SubTotal;//使用蝦皮匯入Excel的訂單小計 //金額合計
        //            //od.OD05 = 0; //運費 (2016/09/09蝦皮不跟SHOP86收運費)
        //            od.OD05 = shopee.Freight;//(2016/10/24上線匯入運費)
        //            od.od21 = 0; //折扣金額
        //            od.OD06 = shopee.SubTotal + od.OD05 - od.od21; //應付總計(總價+運費-折扣)

        //            //預設為2待出貨。備註有值(買家備註、備註)，訂單狀態須改為1待確認!
        //            od.OD07 = "2";
        //            if (!string.IsNullOrWhiteSpace(shopee.ConsigneeMemo) || !string.IsNullOrWhiteSpace(shopee.Memo))
        //            {
        //                od.OD07 = "1";
        //                confirmOrders.Add(myOD00);
        //            }

        //            od.OD15 = "N"; //轉大宗單(Y/N)(必填)
        //            od.OD16 = "Y"; //發通知信(Y/N)
        //            od.OD17 = (shopee.ConsigneeMemo != null && shopee.ConsigneeMemo.Length > 300)
        //                ? shopee.ConsigneeMemo.Substring(0, 300)
        //                : shopee.ConsigneeMemo;  //備註(買家備註) (顯示 單一出貨單上) 

        //            string paymentTerms = shopee.PaymentTerms.Trim();
        //            string sendType = shopee.SendType.Trim();

        //            #region 付款方式 Shopee有三種 貨到付款、信用卡/VISA金融卡、銀行轉帳
        //            //if (paymentTerms.Contains("貨到付款") && (sendType.Contains("7-11") || sendType.Contains("全家")))//009 超商取貨付款
        //            //{
        //            //    od.OD18 = "009";
        //            //}
        //            //else if (paymentTerms.Contains("貨到付款") && (sendType.Contains("黑貓宅急便")))//000 貨到付款
        //            //{
        //            //    od.OD18 = "000";
        //            //}
        //            //else if (paymentTerms.Contains("信用卡/VISA金融卡")) //010 線上刷卡
        //            //{
        //            //    od.OD18 = "010";
        //            //}
        //            //else if (paymentTerms.Contains("銀行轉帳")) //008 ATM轉帳
        //            //{
        //            //    od.OD18 = "008";
        //            //}
        //            od.OD18 = "WD";
        //            #endregion

        //            od.OD08 = shopee.ConsigneeName; //收件人姓名(必填)

        //            //收件地址郵遞區號
        //            //依照測試Excel範本，只有寄送方式為黑貓宅急便，需解析郵遞區號。
        //            //ex:247 新北市蘆洲區民族路361號3樓之1
        //            if (!string.IsNullOrWhiteSpace(shopee.ConsigneeAddress) && shopee.SendType.Contains("黑貓宅急便"))
        //            {
        //                string postalCode = shopee.ConsigneeAddress.Trim().Substring(0, 3);
        //                string[] cityAndAreaNo = _commonService.GetCityAreaNoByPostalCode(postalCode).Split(',');

        //                od.OD13 = postalCode;
        //                od.OD11 = cityAndAreaNo[0];
        //                od.OD12 = cityAndAreaNo[1];
        //                od.OD14 = shopee.ConsigneeAddress; //收件地址(必填)
        //            }

        //            #region 寄送方式 Shopee有三種 黑貓宅急便、7-11、全家
        //            if (sendType.Contains("黑貓宅急便"))
        //            {
        //                od.OD19 = "1"; //1 黑貓(國內)
        //                od.OD09 = shopee.ConsigneeTel; //收件人手機(必填)
        //                od.OD10 = shopee.ConsigneeTel; //收件人電話
        //            }
        //            else if (sendType.Contains("7-11") || sendType.Contains("全家"))
        //            {
        //                od.OD19 = sendType.Contains("7-11") ? "S" : "F"; //S 平台7-11店配、F 平台 全家店配

        //                od.OD09 = shopee.ConsigneeTel; //收件人手機(必填)
        //                od.OD10 = shopee.ConsigneeTel; //收件人電話

        //                od.OD31 = shopee.ConsigneeName; //店配_提貨人姓名
        //                od.OD32 = shopee.ConsigneeTel; //店配_提貨人電話
        //                od.OD33 = string.Empty; //店配_提貨人eMail

        //                od.OD28 = shopee.ConvenienceStoreNo; //店配_門市店號
        //                od.OD29 = shopee.ConvenienceStoreName; //店配_門市名稱
        //                od.OD30 = shopee.ConvenienceStoreAddress; //店配_門市地址
        //            }
        //            #endregion

        //            od.MEMD01 = myMEMD01;
        //            od.MEMD02 = string.Empty; //拍賣帳號
        //            od.OD24 = myYYYYMMDD; //訂單確認日期
        //            od.OD25 = myHHMMSS; //訂單確認時間
        //            od.ST00 = myST00; //倉庫代碼(必填)
        //            od.OD36 = (int)od.OD06; //店配_代收金額(7-11店配取貨必填)
        //            od.OD46 = "1"; //宅配_指定送達時段_38
        //            od.OD47 = string.Empty; //出貨日期(YYYYMMDD)(狀態3已出貨必填)
        //            od.OD50 = shopee.OrderNo; //購物通_訂單編號/超級商城交易序號 (非拍派及官網請填入該平台交易序號)
        //            od.OD51 = "01"; //國別(01/02)(必填)
        //            od.OD55 = "N"; //是否列印過檢貨單(N)(必填)
        //            od.OD59 = 0; //使用折抵(購物金)(0)(必填)
        //            od.OD80 = myYYYYMMDD; //買家填單確認日期(YYYYMMDD)(必填)
        //            od.OD81 = myHHMMSS; //買家填單確認時間(hhmmss)(必填)
        //            od.OD85 = string.Empty; //郵局無摺存款_匯款日期
        //            od.OD86 = string.Empty; //郵局無摺存款_匯款人姓名
        //            od.OD87 = string.Empty; //郵局無摺存款_郵局局號
        //            od.OD88 = 0; //郵局無摺存款_匯款金額
        //            od.OD94 = "02"; //買家發票選擇(必填)(01開立託管電子發票02隨貨附) 康德部分,代碼13-蝦皮拍賣,需開發票隨貨出
        //            od.OD0A = "1"; //會員體系
        //            od.OD99 = myOD00; //舊訂單訂單編號(原系統訂單編號)
        //            od.AID = string.Empty; //Y拍代號
        //            od.CRE_DATE = myYYYYMMDD;
        //            od.CRE_TIME = myHHMMSS;
        //            #endregion

        //            #region 處理明細
        //            int cntODD = 0;
        //            foreach (var detail in shopee.ToKantOrderShopeeDetails)
        //            {
        //                ODD odd = new ODD();
        //                odd.ODD00 = myOD00 + (++cntODD).ToString("00"); //訂單明細編號
        //                odd.OD00 = myOD00; //訂單單號
        //                odd.ST00 = myST00; //倉庫代碼

        //                PD pd = (from x in products where x.PD37 == detail.ProductNo select x).First();
        //                odd.PD00 = pd.PD00; //物品編號
        //                odd.PD_SIZE = "F"; //尺寸
        //                odd.PD_COLOR = "FFF"; //顏色
        //                odd.ODD02 = detail.ProductQuantity; //訂單數量
        //                odd.ODD03 = detail.ProductPrice; //單價
        //                odd.ODD05 = (detail.ProductQuantity * detail.ProductPrice); //小計
        //                odd.ODD04 = string.Empty; //備註
        //                odd.ODD06 = "N"; //是否特價品(Y/N)
        //                odd.ODD07 = "N"; //是否贈品(Y/N)
        //                odd.ODD15 = shopee.OrderNo; //購物通產品編號/超級商城訂單編號(超級商城訂單明細編號Y開頭)
        //                odd.ODD16 = string.Empty; //Yahoo拍賣帳號(此欄位寫入會員登入帳號)
        //                odd.ODD17 = string.Empty; //Yahoo拍賣編號(此欄位寫入上架編號)
        //                odd.ODD18 = "N"; //Yahoo評價(是/否)(Y/N)
        //                odd.ODD_DTIME = myYYYYMMDD + myHHMMSS; //訂單日期(yyyymmddhhmmss)
        //                odd.CRE_DATE = myYYYYMMDD;
        //                odd.CRE_TIME = myHHMMSS;

        //                od.ODDs.Add(odd);
        //            }
        //            #endregion

        //            _db.ODs.Add(od);
        //            successOrderNos.Add(myOD00);
        //        }

        //        #region 訊息
        //        //訂單已存在的錯誤訊息
        //        if (existOrderNos.Count() > 0)
        //        {
        //            msg += _commonService.GetExistOrderNoMsg(string.Join(",", existOrderNos));
        //        }

        //        if (!(_db.SaveChanges() > 0))
        //        {
        //            return msg;
        //        }

        //        //成功匯入的錯誤訊息
        //        if (successOrderNos.Count() > 0)
        //        {
        //            msg += _commonService.GetCompleteMsg(successOrderNos.Count(), string.Join(",", successOrderNos));
        //        }

        //        //狀態為待確認訂單
        //        if (confirmOrders.Count() > 0)
        //        {
        //            msg += _commonService.GetConfirmOrdersMsg(string.Join(",", confirmOrders));
        //        }
        //        #endregion

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    return msg;
        //}
    }
}