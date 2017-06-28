using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OPS.Models;
using OPS.Models.OPSContext;
using OPS.Repostiories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class OrdersImportService
    {

        private readonly OrdersImportRepository _repository = null;

        public OrdersImportRepository Repository
        {
            get { return _repository ?? new OrdersImportRepository(); }
        }

        public string CheckImportFileAndGetNewPath(HttpPostedFileBase file, int fileSize, string fileType, ref string filePath)
        {
            string msg = string.Empty;

            //在此加入檢查檔案邏輯
            //...
            //...

            //儲存檔案並回傳路徑
            filePath = SaveFileReturnPath(file);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                msg = "儲存檔案錯誤!";
                return msg;
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

        /// <summary>
        /// 匯入訂單到DB的動作
        /// </summary>
        /// <param name="shopees"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public string ImportOrders(List<ImportShopeeTW> shopees)
        {
            string msg = string.Empty;

            try
            {
                DateTime nowDate = DateTime.Now;
                List<Order> ODs = new List<Order>();

                List<string> existOrderNos = new List<string>();//存在的訂單編號
                List<string> successOrderNos = new List<string>();//成功匯入的訂單編號
                List<string> confirmOrders = new List<string>();//訂單狀態為 待確認

                //只處理待出貨狀態
                shopees = shopees.Where(x => x.OrderType.Contains("待出貨")).ToList();

                //處理主檔
                foreach (var shopee in shopees)
                {
                    #region 產生主檔
                    Order od = new Order();

                    od.OrderId = new Guid(); //訂單ID
                    od.SourceOrderId = shopee.OrderNo;//來源訂單編號
                    od.OrderDateTime = nowDate;
                    od.OrderStatus = "1";
                    od.DeliveryWay = shopee.SendType;//資料庫欄位可能要改nvarchar
                    od.Distributor = "蝦皮";//資料庫欄位可能要改nvarchar
                    od.RecieveName = shopee.ConsigneeName;
                    od.RecievePhone = shopee.ConsigneeTel;
                    od.RecieveZipCode = string.Empty;
                    od.RecieveAddress = shopee.ConsigneeAddress;
                    od.OrderPrice = shopee.SubTotal;
                    od.Feight = shopee.Freight;
                    od.Payment = shopee.Amount;
                    od.ConvenienceStoreNo = shopee.ConvenienceStoreNo;
                    od.ConvenienceStoreName = shopee.ConvenienceStoreName;

                    #endregion

                    #region 處理明細
                    foreach (var detail in shopee.OrderShopeeDetails)
                    {
                        Product PD = Repository.GetPD(detail.ProductNo);

                        OrderDetail odd = new OrderDetail();
                        odd.OrderDetailId = new Guid(); //訂單明細編號
                        odd.ProductName = PD.ProductName;
                        odd.ProductId = PD.ProductId;
                        odd.Spec = string.Empty;
                        odd.Quantity = detail.ProductQuantity;
                        odd.UnitPrice = detail.ProductPrice;
                        odd.TotalPrice = detail.ProductPrice * detail.ProductQuantity;//單價 * 數量
                        odd.OrderId = od.OrderId;
                        odd.Order = od;
          
                        od.OrderDetails.Add(odd);
                    }
                    #endregion

                    Repository.SaveOrder(od);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return msg;
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}