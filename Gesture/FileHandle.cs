using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gesture
{
    public class FileHandle
    {
        private string m_filePath = string.Empty;

        public FileHandle()
        {
            m_filePath = "Output//default.txt";
        }

        public FileHandle(string filePath)
        {
            m_filePath = filePath;
        }

        /// <summary>
        /// Read all lines
        /// </summary>
        /// <returns></returns>
        public string[] ReadAllLine()
        {
            if (File.Exists(m_filePath))
            {
                return (File.ReadAllLines(m_filePath));
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
            FileMode fileMode = FileMode.Open;
            if (!File.Exists(m_filePath))
            {
                fileMode = FileMode.Create;
            }

            // Create file stream and writer
            FileStream fs = new FileStream(m_filePath, fileMode, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            // Writing
            sw.WriteLine(msg);
            sw.Flush();

            // Close
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// m_filePath
        /// </summary>
        public string FilePath
        {
            get
            {
                return m_filePath;
            }
            set
            {
                m_filePath = value;
            }
        }
    }
}
