using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OPS.Models;
using OPS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class OrdersImportController : Controller
    {
        private readonly OrdersImportService _service = null;

        public OrdersImportService Service
        {
            get { return _service ?? new OrdersImportService(); }
        }

        // GET: OrderImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportShopeeTW(HttpPostedFileBase file)
        {
            string msg = string.Empty;
            string filePath = string.Empty;

            try
            {
                //CheckFile
                msg = Service.CheckImportFileAndGetNewPath(file, 10, "EXCEL", ref filePath);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    TempData["message"] = msg;
                    return View("Index");
                }

                HSSFWorkbook workbook;
                //讀取excel
                using (FileStream filex = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(filex);
                }

                //檢查所需之欄位
                string needColsStr = "訂單編號,訂單狀態,訂單小計 (TWD),買家支付的運費,訂單總金額,商品資訊,收件地址,收件者姓名,電話號碼,寄送方式,付款方式,買家備註,備註";
                List<ICell> checkColsNames = workbook.GetSheetAt(0).GetRow(0).Cells;    // 取得標題列
                msg = Service.CheckImportFileNeedColsNPOI(needColsStr, checkColsNames);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    TempData["message"] = msg;
                    return View("Index");
                }

                //Mapping的Excel轉成自訂的Model，並取到欄位資料
                List<ImportShopeeTW> shopees = new List<ImportShopeeTW>();

                //讀取Sheet1 工作表
                var sheet = workbook.GetSheet("orders");

                for (int row = 0; row <= sheet.LastRowNum; row++)
                {
                    //標頭列
                    if (row == 0)
                    {
                        continue;
                    }

                    if (sheet.GetRow(row) != null)
                    {
                        ImportShopeeTW shopee = new ImportShopeeTW();
                        int columnCnt = 0;

                        foreach (var c in sheet.GetRow(row).Cells)
                        {
                            string columnStr = string.Empty;

                            //如果是數字型 就要取 NumericCellValue  這屬性的值
                            if (c.CellType == CellType.Numeric)
                            {
                                columnStr = c.NumericCellValue.ToString();
                            }

                            //如果是字串型 就要取 StringCellValue  這屬性的值
                            if (c.CellType == CellType.String)
                            {
                                columnStr = c.StringCellValue;
                            }

                            if (columnCnt == 0)//訂單編號
                            {
                                shopee.OrderNo = columnStr;
                            }
                            else if (columnCnt == 1)//訂單狀態
                            {
                                shopee.OrderType = columnStr;
                            }
                            else if (columnCnt == 6)//訂單小計 (TWD)6
                            {
                                shopee.SubTotal = decimal.Parse(columnStr);
                            }
                            else if (columnCnt == 7)//買家支付的運費7
                            {
                                shopee.Freight = decimal.Parse(columnStr);
                            }
                            else if (columnCnt == 8)//訂單總金額8
                            {
                                shopee.Amount = decimal.Parse(columnStr);
                            }
                            else if (columnCnt == 9)//商品資訊9
                            {
                                shopee.ProductInfo = columnStr;
                            }
                            else if (columnCnt == 10)//收件地址10
                            {
                                shopee.ConsigneeAddress = columnStr;
                            }
                            else if (columnCnt == 15)//收件者姓名15
                            {
                                shopee.ConsigneeName = columnStr;
                            }
                            else if (columnCnt == 16)//電話號碼16
                            {
                                shopee.ConsigneeTel = columnStr;
                            }
                            else if (columnCnt == 17)//寄送方式17
                            {
                                shopee.SendType = columnStr;
                            }
                            else if (columnCnt == 18)//付款方式18
                            {
                                shopee.PaymentTerms = columnStr;
                            }
                            else if (columnCnt == 23)//買家備註23
                            {
                                shopee.ConsigneeMemo = columnStr;
                            }
                            else if (columnCnt == 24)//備註24
                            {
                                shopee.Memo = columnStr;
                            }

                            columnCnt++;
                        }

                        shopees.Add(shopee);
                    }
                }

                #region 解析 商品資訊 與 收件住址
                //蝦皮將訂單明細都放到商品資訊，固定對商品資訊解析出商品明細。
                List<string> checkProducts = new List<string>();
                foreach (var shopee in shopees)
                {
                    List<ImportShopeeTWDetail> details = Service.ConvertToOrderDetail(shopee.ProductInfo);

                    int cnt = (from x in details
                               where string.IsNullOrWhiteSpace(x.ProductNo)
                               select x).Count();

                    if (cnt > 0)
                    {
                        msg = string.Format("訂單編號 {0} 的欄位 商品資訊，請檢查 商品選項名稱 是否有包含商品編號!", shopee.OrderNo);
                        TempData["message"] = msg;
                        return View("Index");
                    }

                    checkProducts.AddRange(details.Select(x => x.ProductNo));
                    shopee.OrderShopeeDetails = details;
                }

                //根據寄送方式 7-11與全家 需另外解析住址
                foreach (var shopee in shopees)
                {
                    string sendType = shopee.SendType;

                    if (sendType.Contains("7-11") || sendType.Contains("全家"))
                    {
                        List<string> CVSInfo = Service.ConvertToConvenienceStoreInfo(shopee.ConsigneeAddress);
                        //門市名稱
                        shopee.ConvenienceStoreName = CVSInfo[0];
                        //門市電話
                        shopee.ConvenienceStoreTel = CVSInfo[1];
                        //門市住址
                        shopee.ConvenienceStoreAddress = CVSInfo[2];
                        //門市店號
                        shopee.ConvenienceStoreNo = CVSInfo[3];
                    }
                }
                #endregion

                //寫入資料至主表與明細
                msg = Service.ImportOrders(shopees);
            }
            catch (Exception ex)
            {
                throw;
            }

            TempData["message"] = msg;
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}