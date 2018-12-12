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
    public class ZpHandle
    {
        /// <summary>
        /// Disp frame counter and running time
        /// </summary>
        /// <param name="frameCounter"></param>
        /// <param name="timeList"></param>
        /// <param name="maxRecord"></param>
        /// <param name="statusStrip"></param>
        public void DispRunningTime(int frameCounter, List<double> timeList, int maxRecord, ref StatusStrip statusStrip)
        {
            // Remove the old time
            if (timeList.Count > maxRecord)
            {
                timeList.RemoveRange(0, timeList.Count - maxRecord);
            }

            // Frame counter
            statusStrip.Items["toolStripStatusLabelFrameCounter"].Text = string.Format("{0}", frameCounter.ToString("00000"));

            // Cur time
            statusStrip.Items["toolStripStatusLabelCurTime"].Text      = string.Format("Cur: {0} ms", timeList[timeList.Count - 1].ToString("00.00"));

            // Avg time
            statusStrip.Items["toolStripStatusLabelAvgTime"].Text      = string.Format("Avg: {0} ms", timeList.Average().ToString("00.00"));

            // Max time
            statusStrip.Items["toolStripStatusLabelMaxTime"].Text      = string.Format("Max: {0} ms", timeList.Max().ToString("00.00"));
        }

        /// <summary>
        /// Disp skeleton data
        /// </summary>
        /// <param name="skeletonData"></param>
        /// <param name="textBox"></param>
        public void DispSkeletonData(List<double> skeletonData, ref TextBox textBox)
        {
            string disp_str = "          X           Y           Z   ";
            for (int i = 0; i < skeletonData.Count; i+=3)
            {
                disp_str += Environment.NewLine;
                disp_str += string.Format("{0, 2}   {1, 9}   {2, 9}   {3, 9}", (i/3+1).ToString("00"), 
                                                                               skeletonData[i+0].ToString("0.000"), 
                                                                               skeletonData[i+1].ToString("0.000"), 
                                                                               skeletonData[i+2].ToString("0.000"));
            }
            textBox.Text = disp_str;
        }

        /// <summary>
        /// Disp skeleton data list
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="listBox"></param>
        public void DispSkeletonDataList(string[] dataList, ref ListBox listBox, ref StatusStrip statusStrip)
        {
            // Get the data count
            statusStrip.Items["toolStripStatusLabelAllData"].Text = (dataList.Length - 1).ToString("00000");

            // Refresh the textBox
            listBox.Items.Clear();
            string itemStr = string.Empty;
            for (int i = 1; i < dataList.Length; i++)
            {
                string line   = dataList[i];
                string index  = line.Split(':')[0];
                string[] data = line.Split(':')[1].Split(',');

                itemStr = string.Format("{0}   {1}", index, data.Length == 75 ? "⚑" : " ");
                listBox.Items.Add(itemStr);
            }
        }

        /// <summary>
        /// Get the string variable of skeleton data
        /// </summary>
        /// <param name="index"></param>
        /// <param name="skeletonData"></param>
        /// <returns></returns>
        public string GetSkeletonData(int index, List<double> skeletonData)
        {
            string str = string.Format("{0}: ", index.ToString("00000"));
            for (int i = 0; i < skeletonData.Count; i++)
            {
                str += string.Format("{0, 9}", skeletonData[i].ToString("0.000"));
                if (i < skeletonData.Count - 1)
                {
                    str += ", ";
                }
            }

            return (str);
        }

        /// <summary>
        /// Write label data
        /// </summary>
        /// <param name="skeletonDataLines"></param>
        /// <param name="listBox"></param>
        /// <param name="fileHandle"></param>
        /// <param name="labelname"></param>
        public void WriteLabelData(string[] skeletonDataLines, ListBox listBox, FileHandle fileHandle, string labelname)
        {
            // TXT file to write the skeleton data
            // MD file to write the image url
            string txtName = string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            string mdName  = string.Format("{0}.md" , DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

            // Create a sample
            string dataStr = string.Empty;
            string imgStr  = string.Empty;
            foreach (var selectedItem in listBox.SelectedItems)
            {
                // Make sure all the datas are valid
                string[] itemSplit = selectedItem.ToString().Split(' ');
                if (string.IsNullOrWhiteSpace(itemSplit[itemSplit.Length - 1]))
                {
                    MessageBox.Show("Have empty data in the select datas. ");
                    return;
                }

                // add data string
                string dataline = skeletonDataLines[Convert.ToInt32(itemSplit[0])].Split(':')[1];
                string[] datas = dataline.Split(',');
                for (int j = 0; j < datas.Length; j++)
                {
                    dataStr += string.Format("{0}\n", datas[j].Trim());
                }
                        
                // add image string
                string imgPath = Path.Combine(fileHandle.ImgaeFolder, itemSplit[0] + ".bmp");
                imgStr += string.Format("![{0}]({1})\n\n", itemSplit[0], imgPath);
            }

            // Write
            fileHandle.WriteMsg(Path.Combine(fileHandle.LabelFolder, labelname, txtName), dataStr);
            fileHandle.WriteMsg(Path.Combine(fileHandle.LabelFolder, labelname, mdName) , imgStr);
        }

        // ---------- Disp label Count

        /// <summary>
        /// Disp label data count
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="labelname"></param>
        /// <param name="button"></param>
        public void DispLabelDataCnt(string folder, string labelname, ref Button button, ref NumericUpDown numericUpDown)
        {
            // Define the variables to store txt file url and md file url
            List<List<string>> fileUrls = new List<List<string>>();

            // Get all the file url
            string[] getFiles = Directory.GetFiles(Path.Combine(folder, labelname));

            // Assign
            for (int i = 0; i < getFiles.Length; i += 2)
            {
                List<string> fileUrl = new List<string> { getFiles[i+1], getFiles[i] };
                fileUrls.Add(fileUrl);
            }
            numericUpDown.Tag = fileUrls;
            numericUpDown.Enabled = fileUrls.Count > 0;
            numericUpDown.Maximum = fileUrls.Count;

            // Disp
            button.Text = fileUrls.Count.ToString();
        }

        /// <summary>
        /// Disp sample
        /// </summary>
        /// <param name="numericUpDown"></param>
        /// <param name="pictureBox"></param>
        public void DispSample(NumericUpDown numericUpDown, int dispDelay, ref PictureBox pictureBox)
        {
            // Get md file
            List<List<string>> fileUrls = (List<List<string>>)numericUpDown.Tag;
            string mdFile = fileUrls[Convert.ToInt32(numericUpDown.Value) - 1][1];

            // Read md file
            string[] readLines;
            if (File.Exists(mdFile))
            {
                readLines = File.ReadAllLines(mdFile);
            }
            else
            {
                MessageBox.Show("Cannot find the markdown file. ");
                return;
            }

            // Disp
            foreach (string line in readLines)
            {
                // Continue if line is empty
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Read bitmap
                string[] lineSplit = line.Split('(');
                string imgUrl = lineSplit[1].Split(')')[0];
                Bitmap bitmap  = new Bitmap(imgUrl);

                // Draw the image
                pictureBox.Image = bitmap;
                pictureBox.Refresh();

                // Delay
                System.Threading.Thread.Sleep(dispDelay);
            }

        }
    }
}
