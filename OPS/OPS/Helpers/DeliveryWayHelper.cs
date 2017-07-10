using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OPS.Helpers
{
    public static class DeliveryWayHelper
    {
        public static MvcHtmlString DeliveryWayHelperFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            string value = System.Web.Mvc.Html.ValueExtensions.ValueFor(html, expression).ToString();

            List<SelectListItem> ddlList = new List<SelectListItem>();
            ddlList.Add(new SelectListItem() { Text = "請選擇", Value = "" });

            string filePath = HttpContext.Current.Server.MapPath("/Config/DeliveryWay.xml");
            XDocument doc = XDocument.Load(filePath);
            var configuration = from conf in doc.Descendants("add")
                                select new
                                {
                                    Key = conf.Attribute("key").Value,
                                    Value = conf.Attribute("value").Value
                                };

            foreach (var item in configuration)
            {
                if (item.Value == value)
                {
                    ddlList.Add(new SelectListItem() { Text = item.Value, Value = item.Value, Selected = true });
                }
                else
                {
                    ddlList.Add(new SelectListItem() { Text = item.Value, Value = item.Value });
                }
            }

            /*** Add htmlAttributes ***/
            MvcHtmlString input = (htmlAttributes != null) ? System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, ddlList, htmlAttributes) :
                System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, ddlList, new { @class = "form-control" });

            StringBuilder retorno = new StringBuilder();
            retorno.Append(input);

            return MvcHtmlString.Create(retorno.ToString());
        }
    }
}