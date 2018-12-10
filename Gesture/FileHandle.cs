using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gesture
{
    public class FileHandle
    {
        private string m_outputFolder  = string.Empty;        // 输出文件夹
        private string m_imageFolder = "Images";            // 输出图像文件夹
        private string m_dataFile    = "default.txt";       // 输出数据 txt 文件名


        public FileHandle()
        {
            SetFolder("Output//Default//");
        }

        public FileHandle(string filefolder)
        {
            SetFolder(filefolder);
        }

        public void SetFolder(string outputFolder)
        {
            // Set the output root folder
            m_outputFolder = outputFolder;
            if (!Directory.Exists(m_outputFolder))
            {
                Directory.CreateDirectory(m_outputFolder);
            }

            // Set the image folder
            m_imageFolder  = Path.Combine(m_outputFolder, "Images");
            if (!Directory.Exists(m_imageFolder))
            {
                Directory.CreateDirectory(m_imageFolder);
            }

            // Set the data txt file
            m_dataFile     = Path.Combine(m_outputFolder, string.Format("Data {0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
        }

        /// <summary>
        /// Read all lines
        /// </summary>
        /// <returns></returns>
        public string[] ReadAllLine()
        {
            if (File.Exists(m_dataFile))
            {
                return (File.ReadAllLines(m_dataFile));
            }
            else
            {
                return (null);
            }
        }

        /// <summary>
        /// Write txt file
        /// </summary>
        /// <param name="msg"></param>
        public void WriteMsg(string msg)
        {
            // Create the file if the file does not exist
            FileMode fileMode = FileMode.Append;
            if (!File.Exists(m_dataFile))
            {
                fileMode = FileMode.Create;
            }

            // Create file stream and writer
            FileStream fs = new FileStream(m_dataFile, fileMode, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            // Writing
            sw.WriteLine(msg);
            sw.Flush();

            // Close
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Write image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="name"></param>
        public void WriteImg(Bitmap img, string name)
        {
            string url = Path.Combine(m_imageFolder, string.Format("{0}.bmp", name));
            img.Save(url, System.Drawing.Imaging.ImageFormat.Bmp);
        }

    }
}
