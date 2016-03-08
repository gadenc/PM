using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace RM.Web.Ajax
{
    /// <summary>
    /// Verify_code 的摘要说明
    /// </summary>
    public class Verify_code : IHttpHandler, IRequiresSessionState
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
            int codeW = 80;
            int codeH = 22;
            int fontSize = 16;
            string chkCode = string.Empty;
            Color[] color = new Color[]
			{
				Color.Black,
				Color.Red,
				Color.Blue,
				Color.Green,
				Color.Orange,
				Color.Brown,
				Color.Brown,
				Color.DarkBlue
			};
            string[] font = new string[]
			{
				"Times New Roman",
				"Verdana",
				"Arial",
				"Gungsuh",
				"Impact"
			};
            char[] character = new char[]
			{
				'2',
				'3',
				'4',
				'5',
				'6',
				'8',
				'9',
				'a',
				'b',
				'd',
				'e',
				'f',
				'h',
				'k',
				'm',
				'n',
				'r',
				'x',
				'y',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F',
				'G',
				'H',
				'J',
				'K',
				'L',
				'M',
				'N',
				'P',
				'R',
				'S',
				'T',
				'W',
				'X',
				'Y'
			};
            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            context.Session["dt_session_code"] = chkCode;
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            for (int i = 0; i < 1; i++)
            {
                int x = rnd.Next(codeW);
                int y = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x, y, x2, y2);
            }
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, (float)fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18f + 2f, 0f);
            }
            for (int i = 0; i < 100; i++)
            {
                int x3 = rnd.Next(bmp.Width);
                int y3 = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x3, y3, clr);
            }
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddMilliseconds(0.0);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.AppendHeader("Pragma", "No-Cache");
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Png";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                bmp.Dispose();
                g.Dispose();
            }
        }
    }
}