using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessConsole
{
    public class ProcessImage
    {

        internal static void Process(string sourceSrc, string thumbSrc, string targetSrc, string temp, List<dynamic> templatePositions)
        {
            Image sourceImg = GetImageStream(sourceSrc);
            Image thumbImg = GetImageStream(thumbSrc);

            Bitmap bmp = new Bitmap(sourceImg.Width, sourceImg.Height);

            Graphics gr = Graphics.FromImage(bmp);
            SetGraphicsProperty(gr);
            //缩略图绘制区域
            Rectangle rectDestination = new Rectangle(0, 0, sourceImg.Width, sourceImg.Height);

            gr.DrawImage(thumbImg, rectDestination, 0, 0, thumbImg.Width, thumbImg.Height, GraphicsUnit.Pixel);
            bmp.Save(temp);

            #region xx
            Bitmap bmp_new = new Bitmap(sourceImg.Width, sourceImg.Height);
            var gr_new = Graphics.FromImage(bmp_new);
            SetGraphicsProperty(gr_new);
            gr_new.DrawImage(sourceImg, rectDestination, 0, 0, sourceImg.Width, sourceImg.Height, GraphicsUnit.Pixel);
            #endregion

            //获取bmp指定坐标的颜色集合
            //List<Color> colors = new List<Color>();
            foreach (var item in templatePositions)
            {
                //colors.Add(bmp.GetPixel(item.x, item.y));
                bmp_new.SetPixel(item.x, item.y, bmp.GetPixel(item.x, item.y));
            }
            bmp_new.Save(targetSrc);
            gr_new.Dispose();



            //从原始图片创建画布，并将 bmp 贴到水印的位置
            //Graphics grConver = Graphics.FromImage(sourceImg);
            //SetGraphicsProperty(grConver);

            //Rectangle coverRect = new Rectangle(214, 374, 371, 52);
            //grConver.DrawImage(bmp, coverRect, coverRect, GraphicsUnit.Pixel);



            //保存缩
            //sourceImg.Save(targetSrc, System.Drawing.Imaging.ImageFormat.Jpeg);

            //释放资源
            //grConver.Dispose();
            gr.Dispose();
        }

        internal static List<dynamic> GetTempletePositions(string templateSrc)
        {
            Image template = GetImageStream(templateSrc);

            Bitmap bmp = new Bitmap(template.Width, template.Height);

            Graphics gr = Graphics.FromImage(bmp);
            Rectangle rectDestination = new Rectangle(0, 0, template.Width, template.Height);
            gr.DrawImage(template, rectDestination, 0, 0, template.Width, template.Height, GraphicsUnit.Pixel);

            Color color;
            List<dynamic> position = new List<dynamic>();


            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    color = bmp.GetPixel(x, y);
                    if (color != Color.FromArgb(255, 255, 255, 255))
                    {
                        position.Add(new { x, y });
                    }
                }
            }

            return position;
        }

        internal static void SetGraphicsProperty(Graphics graphics)
        {

            //设置   
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //下面这个也设成高质量  
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            //下面这个设成High  
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        internal static Image GetImageStream(string imgPath)
        {
            FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);

            int byteLength = (int)fileStream.Length;
            byte[] fileBytes = new byte[byteLength];
            fileStream.Read(fileBytes, 0, byteLength);

            //文件流关閉,文件解除锁定
            fileStream.Close();
            var imgStream = Image.FromStream(new MemoryStream(fileBytes));
            return imgStream;
        }
    }
}
