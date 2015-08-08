using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using GF.Core;

namespace GF.Logic.Service
{
    /// <summary>
    /// 验证码处理类
    /// </summary>
    public class ServiceVerifyCode
    {
        ///<summary>显示验证码控件</summary>
        public void Display()
        {
            CreateCheckCodeImage(GenerateCheckCode());
        }

        private string GenerateCheckCode()
        {
            int number;
            string strCode = string.Empty;
            //随机数种子
            Random random = new Random();
            number = random.Next();
            strCode = number.ToString().Substring(0, 3);
            //在Cookie中保存校验码
            SiteDat.SetVerifyCode(strCode.ToLower());
            return strCode;
        }

        ///<summary>
        ///根据校验码输出图片
        ///</summary>
        ///<param name="checkCode">产生的随机校验码</param>
        private void CreateCheckCodeImage(string checkCode)
        {
            //若校验码为空，则直接返回
            if (checkCode == null || checkCode.Trim() == String.Empty) { return; }
            //根据校验码的长度确定输出图片的长度
            Bitmap image = new Bitmap((int)Math.Ceiling((double)(checkCode.Length * 12)), 20);
            //创建Graphics对象
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机数种子
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪音线 10条
                for (int i = 0; i < 10; i++)
                {
                    //噪音线起点坐标(x1,y1),终点坐标(x2,y2)
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    //用银色画出噪音线
                    g.DrawLine(new Pen(Color.White), x1, y1, x2, y2);
                }
                //输出图片中校验码的字体: 12号Arial,粗斜体
                Font font = new Font("Verdana", 9, (FontStyle.Bold | FontStyle.Italic));
                //线性渐变画刷
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Red, Color.Blue, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 0);
                //画图片的前景噪音点 50个
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Transparent), 0, 0, image.Width - 1, image.Height - 1);
                //创建内存流用于输出图片
                using (MemoryStream ms = new MemoryStream())
                {
                    //图片格式指定为png
                    image.Save(ms, ImageFormat.Png);
                    //清除缓冲区流中的所有输出
                    HttpContext.Current.Response.ClearContent();
                    //输出流的HTTP MIME类型设置为"image/Png"
                    HttpContext.Current.Response.ContentType = "Image/PNG";
                    //输出图片的二进制流
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                }
            }
            finally
            {
                //释放Bitmap对象和Graphics对象
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
