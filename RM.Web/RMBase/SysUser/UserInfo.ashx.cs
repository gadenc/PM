using RM.Busines;
using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;
namespace RM.Web.RMBase.SysUser
{
	public class UserInfo : IHttpHandler, IRequiresSessionState
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
			string user_ID = context.Request["user_ID"];
			Hashtable ht = new Hashtable();
			string text = Action;
			if (text != null)
			{
				if (!(text == "accredit"))
				{
					if (text == "lock")
					{
						ht["DeleteMark"] = 2;
						int Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
						context.Response.Write(Return.ToString());
					}
				}
				else
				{
					ht["DeleteMark"] = 1;
					int Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
					context.Response.Write(Return.ToString());
				}
			}
		}
	}
}
