using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Gesture
{
    public partial class LabelMaker : UserControl
    {
        private string m_labelname;
        private string[] m_skeletonDataLines;
        private NumericUpDown m_numericUpDownCombineData;
        private ListBox m_listBox;
        private FileHandle m_fileHandle;

        private ZpHandle m_zpHandle = new ZpHandle();

        public LabelMaker()
        {
            InitializeComponent();
        }

        public void SetMaker(string labelname, ref string[] skeletonDataLines, ref NumericUpDown numericUpDownCombineData, 
                                               ref ListBox listBox, ref FileHandle fileHandle)
        {
            m_labelname                = labelname;
            m_skeletonDataLines        = skeletonDataLines;
            m_numericUpDownCombineData = numericUpDownCombineData;
            m_listBox                  = listBox;
            m_fileHandle               = fileHandle;
        }

        private void buttonLabel_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_listBox.SelectedItems.Count == m_numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, m_listBox, m_fileHandle, m_labelname);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_labelname, ref buttonCnt, ref numericUpDownIndex);
                }
                else
                {
                    MessageBox.Show("The number of selected datas is invalid. ");
                    return;
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("LabelMaker.buttonLabel_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
        }

        private void buttonCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_labelname);
            if (Directory.Exists(folderUrl))
            {
                System.Diagnostics.Process.Start(folderUrl);
            }
        }

        private void numericUpDownIndex_ValueChanged(object sender, EventArgs e)
        {

        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        public string ButtonLabelText
        {
            get
            {
                return buttonLabel.Text;
            }
            set
            {
                buttonLabel.Text = value;
            }
        }

        public string ButtonCntText
        {
            get
            {
                return buttonCnt.Text;
            }
            set
            {
                buttonCnt.Text = value;
            }
        }

        public decimal numericUpDownIndexValue
        {
            get
            {
                return numericUpDownIndex.Value;
            }
            set
            {
                numericUpDownIndex.Value = value;
            }
        }

        public decimal numericUpDownIndexMax
        {
            get
            {
                return numericUpDownIndex.Maximum;
            }
            set
            {
                numericUpDownIndex.Maximum = value;
            }
        }

        public decimal numericUpDownIndexMin
        {
            get
            {
                return numericUpDownIndex.Minimum;
            }
            set
            {
                numericUpDownIndex.Minimum = value;
            }
        }
    }
}
