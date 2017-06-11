using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Models
{
    public class ImportShopeeTW
    {
        #region Mapping EXCEL 所需欄位
        //訂單編號,訂單狀態,訂單小計 (TWD),買家支付的運費,訂單總金額,商品資訊,收件地址,收件者姓名,電話號碼,寄送方式,付款方式,買家備註,備註
        public string OrderNo { get; set; }//訂單編號
        public string OrderType { get; set; }//訂單狀態
        public decimal SubTotal { get; set; }//訂單小計
        public decimal Freight { get; set; }//買家支付的運費
        public decimal Amount { get; set; }//訂單總金額
        public string ProductInfo { get; set; }//商品資訊
        public string ConsigneeAddress { get; set; }//收件地址
        public string ConsigneeName { get; set; }//收件者姓名
        public string ConsigneeTel { get; set; }//電話號碼
        public string SendType { get; set; }//寄送方式
        public string PaymentTerms { get; set; }//付款方式
        public string ConsigneeMemo { get; set; }//買家備註
        public string Memo { get; set; }//備註
        #endregion

        #region 寄送方式為7-11 OR 全家 解析字串所需要的欄位
        public string ConvenienceStoreName { get; set; }
        public string ConvenienceStoreTel { get; set; }
        public string ConvenienceStoreAddress { get; set; }
        public string ConvenienceStoreNo { get; set; }
        #endregion

        public List<ImportShopeeTWDetail> ToKantOrderShopeeDetails { get; set; }
    }
}