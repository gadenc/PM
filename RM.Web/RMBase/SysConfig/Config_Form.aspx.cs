using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;
using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace RM.Web.RMBase.SysConfig
{
    public class Config_Form : PageBase
    {
        protected HtmlForm form1;
        protected HtmlInputHidden AppendProperty_value;
        public StringBuilder str_OutputHtml = new StringBuilder();
        private string Property_Function = "系统配置信息";
        private RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.str_OutputHtml.Append(this.systemidao.AppendProperty_Html(this.Property_Function));
            this.GetValue();
        }
        public void GetValue()
        {
            string returnValue = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(base.Server.MapPath("/App_Code/Config.xml"));
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//appSettings/add");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                string text = returnValue;
                returnValue = string.Concat(new string[]
				{
					text,
					xmlNode.Attributes["key"].Value,
					"∫",
					xmlNode.Attributes["value"].Value,
					"∮"
				});
            }
            this.AppendProperty_value.Value = returnValue;
        }
    }
}
