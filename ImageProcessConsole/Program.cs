using System;
using System.IO;
using System.Linq;

namespace ImageProcessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var appPath = Environment.CurrentDirectory;
            var templateImages = appPath + "\\images\\template\\JD4.jpg";
            var tempFolder = appPath + "\\images\\temp\\";
            var targetFolder = appPath + "\\images\\target\\";

            CreateDirectory(tempFolder, targetFolder);

            var templatePositions = ProcessImage.GetTempletePositions(templateImages);

            if (templatePositions == null || templatePositions.Count == 0)
            {
                Console.WriteLine("***警告：模板文件水印坐标点获取失败！");
                return;
            }

            FileInfo[] files_source = new DirectoryInfo(appPath + "\\images\\source").GetFiles();
            FileInfo[] files_thumb = new DirectoryInfo(appPath + "\\images\\thumb").GetFiles();

            for (int i = 0; i < files_source.Length; i++)
            {
                var item = files_source[i];
                var thumbFile = files_thumb.Where(x => x.Name == item.Name).FirstOrDefault();
                Console.WriteLine("正在处理..{0}/{1}..{2}", i + 1, files_source.Count(), item.Name);

                var sourceSrc = appPath + "\\images\\source\\" + item.Name;
                var thumbSrc = appPath + "\\images\\thumb\\" + thumbFile.Name;
                string targetSrc = targetFolder + item.Name;
                string tempSrc = tempFolder + item.Name;
                ProcessImage.Process(sourceSrc, thumbSrc, targetSrc, tempSrc, templatePositions);
            }


            Console.WriteLine("处理完成……");
            Console.WriteLine("临时路径：{0}", tempFolder);
            Console.WriteLine("目标路径：{0}", targetFolder);
            Console.ReadKey();


        }

        private static void CreateDirectory(string tempFolder, string targetFolder)
        {
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }
        }
    }
}
