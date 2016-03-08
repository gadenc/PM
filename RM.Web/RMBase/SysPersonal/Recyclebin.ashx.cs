using RM.Busines.DAL;
using RM.Busines.IDAO;
using System;
using System.Web;
using System.Web.SessionState;
namespace RM.Web.RMBase.SysPersonal
{
	public class Recyclebin : IHttpHandler, IRequiresSessionState
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Buffer = true;
			context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
			context.Response.AddHeader("pragma", "no-cache");
			context.Response.AddHeader("cache-control", "");
			context.Response.CacheControl = "no-cache";
			string Action = context.Request["action"];
			string pkVal = context.Request["pkVal"];
			RM_System_IDAO sys_idao = new RM_System_Dal();
			string text = Action;
			if (text != null)
			{
				if (!(text == "restore_Data"))
				{
					if (text == "restore_Empty")
					{
						int Return = sys_idao.Recyclebin_Empty(pkVal.Split(new char[]
						{
							','
						}));
						context.Response.Write(Return.ToString());
					}
				}
				else
				{
					int Return = sys_idao.Recyclebin_Restore(pkVal.Split(new char[]
					{
						','
					}));
					context.Response.Write(Return.ToString());
				}
			}
		}
	}
}
