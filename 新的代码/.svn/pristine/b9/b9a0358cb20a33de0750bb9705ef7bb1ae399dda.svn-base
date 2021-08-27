using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace KYH_Customer.Filters
{
	// Token: 0x0200000E RID: 14
	public class LocalizationAttribute : ActionFilterAttribute
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.RouteData.Values["lang"] != null && !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
			{
				string lang = filterContext.RouteData.Values["lang"].ToString();
				Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
			}
			else
			{
				HttpCookie cookie = filterContext.HttpContext.Request.Cookies["Language"];
				string langHeader = string.Empty;
				if (cookie != null)
				{
					langHeader = cookie.Value;
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
				}
				else
				{
					langHeader = filterContext.HttpContext.Request.UserLanguages[0];
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
				}
				filterContext.RouteData.Values["lang"] = langHeader;
			}
			HttpCookie _cookie = new HttpCookie("Language", Thread.CurrentThread.CurrentUICulture.Name);
			_cookie.Expires = DateTime.Now.AddMonths(1);
			filterContext.HttpContext.Response.SetCookie(_cookie);
			base.OnActionExecuting(filterContext);
		}
	}
}
