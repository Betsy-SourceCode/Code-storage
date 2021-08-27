using System;
using System.Web.Mvc;

namespace KYH_KnowledgeBase.Filters
{
	// Token: 0x0200000D RID: 13
	public class IsSessionExpire : ActionFilterAttribute
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002CB1 File Offset: 0x00000EB1
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002CB9 File Offset: 0x00000EB9
		public bool IsCheck { get; set; }

		// Token: 0x0600004A RID: 74 RVA: 0x00002CC4 File Offset: 0x00000EC4
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session["id"] == null || filterContext.HttpContext.Session["username"] == null || filterContext.HttpContext.Session["DeptID"] == null)
			{
				if (!this.IsCheck)
				{
					return;
				}
				filterContext.HttpContext.Session["id"] = filterContext.HttpContext.Request.Cookies["userid"].Value;
				filterContext.HttpContext.Session["username"] = filterContext.HttpContext.Request.Cookies["username"].Value;
				filterContext.HttpContext.Session["DeptID"] = filterContext.HttpContext.Request.Cookies["deptid"].Value;
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
