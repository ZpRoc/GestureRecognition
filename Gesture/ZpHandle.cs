using System;
using System.Collections.Generic;
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
        /// <param name="statusStripTime"></param>
        public void DispRunningTime(int frameCounter, List<double> timeList, int maxRecord, ref StatusStrip statusStripTime)
        {
            // Remove the old time
            if (timeList.Count > maxRecord)
            {
                timeList.RemoveRange(0, timeList.Count - maxRecord);
            }

            // Frame counter
            statusStripTime.Items["toolStripStatusLabelFrameCounter"].Text = string.Format("{0}", frameCounter.ToString("00000"));

            // Cur time
            statusStripTime.Items["toolStripStatusLabelCurTime"].Text      = string.Format("Cur: {0} ms", timeList[timeList.Count - 1].ToString("00.00"));

            // Avg time
            statusStripTime.Items["toolStripStatusLabelAvgTime"].Text      = string.Format("Avg: {0} ms", timeList.Average().ToString("00.00"));

            // Max time
            statusStripTime.Items["toolStripStatusLabelMaxTime"].Text      = string.Format("Max: {0} ms", timeList.Max().ToString("00.00"));
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




    }
}
