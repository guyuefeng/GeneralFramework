using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing ;
using System.Drawing.Imaging ;
using System.IO ;

namespace GF.Core
{
    public class SiteWatermark
    {
        /// <summary>
        /// 在图片上加入图片版权信息
        /// </summary>
        /// <param name="strFileName">输入图片</param>
        /// <param name="strOutfileName">输出图片</param>
        /// <param name="strCopyRightFile">水印图片</param>
        /// <param name="intPlace">水印位置</param>
        /// <param name="bolFileName">是否删除输入图片</param>
        public void HandleImage(string strFileName, string strOutfileName, string strCopyRightFile, int intPlace, bool bolFileName)
        {
            if (intPlace > 0)
            {
                //开始
                Image MyImage = Image.FromFile(strFileName);
                Image CopyMyImage = Image.FromFile(strCopyRightFile);
                Bitmap OutPut = new Bitmap(MyImage);
                Graphics GImage = Graphics.FromImage(OutPut);
                int IntX = 0;
                int IntY = 0;
                //在左上
                if (intPlace == 1)
                {
                    IntX = 0;
                    IntY = 0;
                }
                //在正上
                else if (intPlace == 2)
                {
                    IntX = (MyImage.Width - CopyMyImage.Width) / 2;
                    IntY = 0;
                }
                //在右上
                else if (intPlace == 3)
                {
                    IntX = MyImage.Width - CopyMyImage.Width;
                    IntY = 0;
                }
                //在正左
                else if (intPlace == 4)
                {
                    IntX = 0;
                    IntY = (MyImage.Height - CopyMyImage.Height) / 2;
                }
                //在中间
                else if (intPlace == 5)
                {
                    IntX = (MyImage.Width - CopyMyImage.Width) / 2;
                    IntY = (MyImage.Height - CopyMyImage.Height) / 2;
                }
                //在正右
                else if (intPlace == 6)
                {
                    IntX = MyImage.Width - CopyMyImage.Width;
                    IntY = (MyImage.Height - CopyMyImage.Height) / 2;
                }
                // 在左下
                else if (intPlace == 7)
                {
                    IntX = 0;
                    IntY = MyImage.Height - CopyMyImage.Height;
                }
                //在正下
                else if (intPlace == 8)
                {
                    IntX = (MyImage.Width - CopyMyImage.Width) / 2;
                    IntY = MyImage.Height - CopyMyImage.Height;
                }
                //在右下
                else if (intPlace == 9)
                {
                    IntX = MyImage.Width - CopyMyImage.Width;
                    IntY = MyImage.Height - CopyMyImage.Height;
                }
                else
                {
                    IntX = MyImage.Width - CopyMyImage.Width - 10;
                    IntY = MyImage.Height - CopyMyImage.Height - 10;
                }
                // 画出水印的位置
                GImage.DrawImage(CopyMyImage, IntX, IntY, CopyMyImage.Width, CopyMyImage.Height);
                string strExtend = strFileName.Substring(strFileName.LastIndexOf(".") + 1).ToLower();
                switch (strExtend)
                {
                    case "bmp":
                        OutPut.Save(strOutfileName, ImageFormat.Bmp);
                        break;
                    case "jpg":
                        OutPut.Save(strOutfileName, ImageFormat.Jpeg);
                        break;
                    case "gif":
                        OutPut.Save(strOutfileName, ImageFormat.Gif);
                        break;
                    case "icon":
                        OutPut.Save(strOutfileName, ImageFormat.Icon);
                        break;
                    case "png":
                        OutPut.Save(strOutfileName, ImageFormat.Png);
                        break;
                    case "tif":
                        OutPut.Save(strOutfileName, ImageFormat.Tiff);
                        break;
                    default:
                        OutPut.Save(strOutfileName, ImageFormat.Jpeg);
                        break;
                }
                GImage.Dispose();
                OutPut.Dispose();
                MyImage.Dispose();
                CopyMyImage.Dispose();
                if (bolFileName) { File.Delete(strFileName); }
            }
        }
    }
}
