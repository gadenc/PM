using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace DTcms.Common
{
    /// <summary>
    /// Template为页面模板类.
    /// </summary>
    public abstract class PageTemplate
    {
        public static Regex[] r = new Regex[19];

        static PageTemplate()
        {
            RegexOptions options = RegexOptions.None;
            r[0] = new Regex(@"<%template src=/([^\[\]\{\}\s]+)/%>", options);
            //命名空间
            r[1] = new Regex(@"<%namespace (?:""?)([\s\S]+?)(?:""?)%>", options);
            //C#代码
            r[2] = new Regex(@"<%csharp%>([\s\S]+?)<%/csharp%>", options);
            //loop循环
            //r[3] = new Regex(@"<%loop ((\(([a-zA-Z]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);
            r[3] = new Regex(@"<%loop ((\(([^\[\]\{\}\s]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);
            r[4] = new Regex(@"<%\/loop%>", options);
            //IF语句
            r[5] = new Regex(@"<%if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?)(?:\s*)%>", options);
            r[6] = new Regex(@"<%else(( (?:\s*)if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?))?)(?:\s*)%>", options);
            r[7] = new Regex(@"<%\/if%>", options);
            r[8] = new Regex(@"<%continue%>");
            r[9] = new Regex(@"<%break%>");
            //解析普通变量{}
            r[10] = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", options);
            //截取字符串
            r[11] = new Regex(@"(<%cutstring\(([^\s]+?),(.\d*?)\)%>)", options);
            //url链接
            r[12] = new Regex(@"(<%linkurl\(([^\s]*?)\)%>)", options);
            //set标签
            //r[13] = new Regex(@"<%set ((\(([a-zA-Z]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);
            r[13] = new Regex(@"<%set ((\(([\w\.<>]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);
            ////解析==表达式
            r[14] = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", options);
            //解析普通变量{}
            r[15] = new Regex(@"({([^\[\]/\{\}=:'\s]+)})", options);
            //时间格式转换
            r[16] = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", options);
            //整型转换
            r[17] = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", options);
            //write标签
            r[18] = new Regex(@"<%write (?:\s*)(.*?)(?:\s*)%>", options);
        }

        /// <summary>
        /// 获得模板字符串，从设置中的模板路径来读取模板文件.
        /// </summary>
        /// <param name="sitePath">站点目录</param>
        /// <param name="tempPath">模板目录</param>
        /// <param name="skinName">模板名</param>
        /// <param name="templateName">模板文件的文件名称</param>
        /// <param name="fromPage">源页面名称</param>
        /// <param name="inherit">该页面继承的类</param>
        /// <param name="channelId">该页面所属的频道ID</param>
        /// <param name="nest">嵌套次数</param>
        /// <returns>string值,如果失败则为"",成功则为模板内容的string</returns>
        public static string GetTemplate(string sitePath, string tempPath, string skinName, string templet, string fromPage, string inherit, int channelId, int nest)
        {
            StringBuilder strReturn = new StringBuilder(220000); //返回的字符
            string templetFullPath = Utils.GetMapPath(sitePath + tempPath + "/" + skinName + "/" + templet); //取得模板文件物理路径

            //超过5次嵌套退出
            if (nest < 1)
            {
                nest = 1;
            }
            else if (nest > 5)
            {
                return "";
            }
            string extNamespace = "";
            //检查模板文件是否存在
            if (!File.Exists(templetFullPath))
            {
                return "";
            }

            //开始读写文件
            using (StreamReader objReader = new StreamReader(templetFullPath, Encoding.UTF8))
            {
                StringBuilder textOutput = new StringBuilder(70000);
                textOutput.Append(objReader.ReadToEnd());
                objReader.Close();

                //处理Csharp语句
                foreach (Match m in r[2].Matches(textOutput.ToString()))
                {
                    textOutput.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\t\r"));
                }
                //替换标记
                textOutput.Replace("\r\n", "\r\r\r");
                textOutput.Replace("<%", "\r\r\n<%");
                textOutput.Replace("%>", "%>\r\r\n");
                textOutput.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");

                string[] strlist = Utils.SplitString(textOutput.ToString(), "\r\r\n");
                for (int i = 0; i < strlist.Length; i++)
                {
                    if (strlist[i] == "")
                        continue;
                    //替换标签
                    strReturn.Append(ConvertTags(nest, channelId, sitePath, tempPath, skinName, strlist[i]));
                }

                //写入文件
                if (nest == 1)
                {
                    //命名空间
                    foreach (Match m in r[1].Matches(textOutput.ToString()))
                    {
                        extNamespace += "\r\n<%@ Import namespace=\"" + m.Groups[1] + "\" %>";
                        textOutput.Replace(m.Groups[0].ToString(), string.Empty);
                    }
                    //频道ID
                    string channelStr = string.Empty;
                    if (channelId > 0)
                    {
                        channelStr = "\r\n\tconst int channel_id = " + channelId + ";";
                    }
                    string template = string.Format("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" Inherits=\"{0}\" ValidateRequest=\"false\" %>\r\n<%@ Import namespace=\"System.Collections.Generic\" %>\r\n<%@ Import namespace=\"System.Text\" %>\r\n<%@ Import namespace=\"System.Data\" %>\r\n<%@ Import namespace=\"DTcms.Common\" %>\r\n{1}\r\n<script runat=\"server\">\r\noverride protected void OnInit(EventArgs e)\r\n{{\r\n\r\n\t/* \r\n\t\tThis page was created by DTcms Template Engine at {2}.\r\n\t\t本页面代码由DTcms模板引擎生成于 {2}. \r\n\t*/\r\n\r\n\tbase.OnInit(e);\r\n\tStringBuilder templateBuilder = new StringBuilder({3});{4}\r\n{5}\r\n\tResponse.Write(templateBuilder.ToString());\r\n}}\r\n</script>\r\n", inherit, extNamespace, DateTime.Now, strReturn.Capacity, channelStr, Regex.Replace(strReturn.ToString(), @"\r\n\s*templateBuilder\.Append\(""""\);", ""));
                    string pageDir = Utils.GetMapPath(sitePath + "aspx/");
                    string outputPath = pageDir + fromPage;
                    if (!Directory.Exists(pageDir))
                    {
                        Directory.CreateDirectory(pageDir);
                    }
                    File.WriteAllText(outputPath, template, Encoding.UTF8);
                    //using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    //{
                    //    Byte[] info = Encoding.UTF8.GetBytes(template);
                    //    fs.Write(info, 0, info.Length);
                    //    fs.Close();
                    //}
                }
            }

            return strReturn.ToString();
        }

        /// <summary>
        /// 转换标签
        /// </summary>
        /// <param name="nest">深度</param>
        /// <param name="sitePath">站点目录</param>
        /// <param name="skinName">模板名称</param>
        /// <param name="inputStr">模板内容</param>
        /// <returns></returns>
        private static string ConvertTags(int nest, int channelId, string sitePath, string tempPath, string skinName, string inputStr)
        {
            string strReturn = "";
            string strTemplate;
            strTemplate = inputStr.Replace("\\", "\\\\");
            strTemplate = strTemplate.Replace("\"", "\\\"");
            strTemplate = strTemplate.Replace("</script>", "</\");\r\n\ttemplateBuilder.Append(\"script>");
            bool IsCodeLine = false;

            //解析嵌套标签=========================================================
            foreach (Match m in r[0].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\r\n" + GetTemplate(sitePath, tempPath, skinName, m.Groups[1].ToString(), "", "", channelId, nest + 1) + "\r\n");
            }

            //解析csharpcode===================================================
            foreach (Match m in r[2].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[1].ToString().Replace("\r\t\r", "\r\n\t").Replace("\\\"", "\""));
            }

            //解析loop标签=====================================================
            foreach (Match m in r[3].Matches(strTemplate))
            {
                IsCodeLine = true;
                if (m.Groups[3].ToString() == "")
                {
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        string.Format("\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n", m.Groups[4], m.Groups[5]));
                }
                else
                {
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        string.Format("\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n", m.Groups[3], m.Groups[4], m.Groups[5]));
                }
            }
            foreach (Match m in r[4].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "\r\n\t}\t//end loop\r\n");
            }

            //解译判断IF语句=======================================================
            foreach (Match m in r[5].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "\r\n\tif (" + m.Groups[1].ToString().Replace("\\\"", "\"") + ")\r\n\t{");
            }
            foreach (Match m in r[6].Matches(strTemplate))
            {
                IsCodeLine = true;
                if (m.Groups[1].ToString() == string.Empty)
                {
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "\r\n\t}\r\n\telse\r\n\t{");
                }
                else
                {
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        "\r\n\t}\r\n\telse if (" + m.Groups[3].ToString().Replace("\\\"", "\"") + ")\r\n\t{");
                }
            }
            foreach (Match m in r[7].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "\r\n\t}\t//end if\r\n");
            }

            //解析continue，break==================================================
            foreach (Match m in r[8].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\tcontinue;\r\n");
            }
            foreach (Match m in r[9].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "\tbreak;\r\n");
            }

            //解析{request[a]}=====================================================
            foreach (Match m in r[10].Matches(strTemplate))
            {
                if (IsCodeLine)
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "DTRequest.GetString(\"" + m.Groups[2] + "\")");
                else
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + DTRequest.GetString(\"{0}\") + \"", m.Groups[2]));
            }

            //解析截取字符串=======================================================
            foreach (Match m in r[11].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                              string.Format("\r\n\ttemplateBuilder.Append(Utils.DropHTML({0},{1}));", m.Groups[2], m.Groups[3].ToString().Trim()));
            }

            //解析时间格式转换=====================================================
            foreach (Match m in r[16].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                              string.Format("\ttemplateBuilder.Append(Utils.ObjectToDateTime({0}).ToString(\"{1}\"));", m.Groups[2], m.Groups[3].ToString().Replace("\\\"", string.Empty)));
            }

            //字符串转换整型=======================================================
            foreach (Match m in r[17].Matches(strTemplate))
            {
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    "Utils.StrToInt(" + m.Groups[2] + ", 0)");
            }

            //解析url链接标签======================================================
            foreach (Match m in r[12].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                              string.Format("\r\n\ttemplateBuilder.Append(linkurl({0}));", m.Groups[2]).Replace("\\\"", "\"")
                              );
            }

            //解析set标签==========================================================
            foreach (Match m in r[13].Matches(strTemplate))
            {
                IsCodeLine = true;
                string type = "";
                if (m.Groups[3].ToString() != string.Empty)
                {
                    type = m.Groups[3].ToString();
                }
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    string.Format("\r\n\t{0} {1} = {2};\r\n\t", type, m.Groups[4], m.Groups[5]).Replace("\\\"", "\"")
                    );
            }

            //解析write标签========================================================
            foreach (Match m in r[18].Matches(strTemplate))
            {
                IsCodeLine = true;
                strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                    string.Format("\r\n\ttemplateBuilder.Append({0}{1}.ToString());\r\n\t", m.Groups[1], m.Groups[2]).Replace("\\\"", "\"")
                    );
            }

            //解析{var[a]}=========================================================
            foreach (Match m in r[14].Matches(strTemplate))
            {
                if (IsCodeLine)
                {
                    if (Utils.IsNumeric(m.Groups[3].ToString()))
                        strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "Utils.ObjectToStr(" + m.Groups[2] + "[" + m.Groups[3] + "])");
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                            strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2] + "__loop__id");
                        else
                            strTemplate = strTemplate.Replace(m.Groups[0].ToString(), "Utils.ObjectToStr(" + m.Groups[2] + "[\"" + m.Groups[3] + "\"])");
                    }
                }
                else
                {
                    if (Utils.IsNumeric(m.Groups[3].ToString()))
                        strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + Utils.ObjectToStr({0}[{1}]) + \"", m.Groups[2], m.Groups[3]));
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                            strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + {0}__loop__id.ToString() + \"", m.Groups[2]));
                        else
                            strTemplate = strTemplate.Replace(m.Groups[0].ToString(), string.Format("\" + Utils.ObjectToStr({0}[\"{1}\"]) + \"", m.Groups[2], m.Groups[3]));
                    }
                }
            }

            //解析普通变量{}=======================================================
            foreach (Match m in r[15].Matches(strTemplate))
            {
                if (IsCodeLine)
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(), m.Groups[2].ToString());
                else
                    strTemplate = strTemplate.Replace(m.Groups[0].ToString(),
                        string.Format("\");\r\n\ttemplateBuilder.Append(Utils.ObjectToStr({0}));\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString().Trim()));
            }

            //最后处理=============================================================
            if (IsCodeLine)
            {
                strReturn = strTemplate + "\r\n";
            }
            else
            {
                if (strTemplate.Trim() != "")
                {
                    strReturn = "\r\n\ttemplateBuilder.Append(\"" + strTemplate.Replace("\r\r\r", "\\r\\n") + "\");";
                    strReturn = strReturn.Replace("\\r\\n<?xml", "<?xml");
                    strReturn = strReturn.Replace("\\r\\n<!DOCTYPE", "<!DOCTYPE");
                }
            }
            return strReturn;
        }

    }
}