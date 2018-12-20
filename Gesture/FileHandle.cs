using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gesture
{
    public class FileHandle
    {
        private string[] m_labelnames;

        private string m_IMAGE_FOLDER_NAME = "Images";      // The image folder name
        private string m_LABEL_FOLDER_NAME = "Labels";      // The label folder name
        private string m_DATA_FILE_NAME    = "Data.txt";    // The data file name

        private string m_outputFolder  = string.Empty;      // The output folder
        private string m_imageFolder   = "Images";          // The image folder
        private string m_labelFolder   = "Labels";          // The image folder
        private string m_dataFile      = "default.txt";     // The data file

        public FileHandle(string[] label_names)
        {
            m_labelnames = label_names;
            SetFolder("Output\\All");
        }

        public FileHandle(string[] label_names, string filefolder)
        {
            m_labelnames = label_names;
            SetFolder(filefolder);
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Set the output folder and create sub folders and files
        /// </summary>
        /// <param name="outputFolder"></param>
        public void SetFolder(string outputFolder)
        {
            // Set the output root folder
            m_outputFolder = outputFolder;
            if (!Directory.Exists(m_outputFolder))
            {
                Directory.CreateDirectory(m_outputFolder);
            }

            // Set the image folder
            m_imageFolder = Path.Combine(m_outputFolder, m_IMAGE_FOLDER_NAME);
            if (!Directory.Exists(m_imageFolder))
            {
                Directory.CreateDirectory(m_imageFolder);
            }

            // Set the data txt file
            m_dataFile = Path.Combine(m_outputFolder, m_DATA_FILE_NAME);

            // Set label folder
            m_labelFolder = Path.Combine(m_outputFolder, m_LABEL_FOLDER_NAME);
            if (!Directory.Exists(m_labelFolder))
            {
                Directory.CreateDirectory(m_labelFolder);
            }

            for (int i = 0; i < m_labelnames.Length; i++)
            {
                string tmpFolder = Path.Combine(m_labelFolder, m_labelnames[i]);
                if (!Directory.Exists(tmpFolder))
                {
                    Directory.CreateDirectory(tmpFolder);
                }
            }
        }

        /// <summary>
        /// Does the given folder has all the sub folders and files
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public bool IsValidFolder(string outputFolder)
        {
            // Is the output root folder exist
            if (!Directory.Exists(outputFolder))
            {
                return (false);
            }

            // Is the image folder exist
            string imageFolder = Path.Combine(outputFolder, m_IMAGE_FOLDER_NAME);
            if (!Directory.Exists(imageFolder))
            {
                return (false);
            }

            // Is the data txt file exist
            string dataFile = Path.Combine(outputFolder, m_DATA_FILE_NAME);
            if (!File.Exists(dataFile))
            {
                return (false);
            }

            return (true);
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Read all lines
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string[] ReadAllLine(string url)
        {
            if (File.Exists(url))
            {
                return (File.ReadAllLines(url));
            }
            else
            {
                return (null);
            }
        }

        /// <summary>
        /// Write txt file
        /// </summary>
        /// <param name="url"></param>
        /// <param name="msg"></param>
        public void WriteMsg(string url, string msg)
        {
            // Create the file if the file does not exist
            FileMode fileMode = FileMode.Append;
            if (!File.Exists(url))
            {
                fileMode = FileMode.Create;
            }

            // Create file stream and writer
            FileStream fs = new FileStream(url, fileMode, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            // Writing
            sw.WriteLine(msg);
            sw.Flush();

            // Close
            sw.Close();
            fs.Close();
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Read image
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public Bitmap ReadImg(string imgPath)
        {
            if (File.Exists(imgPath))
            {
                Bitmap bitmap = new Bitmap(imgPath);
                return (bitmap);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Write image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imgName"></param>
        public void WriteImg(Bitmap img, string imgName)
        {
            string url = Path.Combine(m_imageFolder, imgName);
            img.Save(url, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        public string IMAGE_FOLDER_NAME
        {
            get
            {
                return m_IMAGE_FOLDER_NAME;
            }
        }

        public string LABEL_FOLDER_NAME
        {
            get
            {
                return m_LABEL_FOLDER_NAME;
            }
        }

        public string DATA_FILE_NAME
        {
            get
            {
                return m_DATA_FILE_NAME;
            }
        }

        public string OutputFolder
        {
            get
            {
                return m_outputFolder;
            }
        }

        public string ImgaeFolder
        {
            get
            {
                return m_imageFolder;
            }
        }

        public string LabelFolder
        {
            get
            {
                return m_labelFolder;
            }
        }

        public string DataFile
        {
            get
            {
                return m_dataFile;
            }
        }
    }
}
