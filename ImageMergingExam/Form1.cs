using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace ImageMergingExam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static void SetGraphicsProperty(Graphics graphics)
        {
            //设置   
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //下面这个也设成高质量  
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            //下面这个设成High  
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        private static Image GetImageStream(string imgPath)
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

        private void btnProcess_Click(object sender, EventArgs e)
        {
            var appPath = Environment.CurrentDirectory;
            Image sourceImg = GetImageStream(appPath + "\\images\\n0.jpg");
            Image thumbImg = GetImageStream(appPath + "\\images\\n1.jpg");
            var targetImgPath = appPath + "\\images\\new.jpg";

            Bitmap bmp = new Bitmap(sourceImg.Width, sourceImg.Height);

            Graphics gr = Graphics.FromImage(bmp);
            SetGraphicsProperty(gr);
            //缩略图绘制区域
            Rectangle rectDestination = new Rectangle(0, 0, sourceImg.Width, sourceImg.Height);
            gr.DrawImage(thumbImg, rectDestination, 0, 0, thumbImg.Width, thumbImg.Height, GraphicsUnit.Pixel);

            //从原始图片创建画布，并将 bmp 贴到水印的位置
            Graphics grConver = Graphics.FromImage(sourceImg);
            SetGraphicsProperty(grConver);
            Rectangle coverRect = new Rectangle(212, 367, 420, 61);
            grConver.DrawImage(bmp, coverRect, coverRect, GraphicsUnit.Pixel);

            //保存缩
            sourceImg.Save(targetImgPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            //释放资源
            grConver.Dispose();
            gr.Dispose();
        }
    }
}
