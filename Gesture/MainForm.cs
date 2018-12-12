﻿using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

using nuitrack;
using nuitrack.issues;

namespace Gesture
{
    public partial class MainForm : Form
    {
        private DirectBitmap _bitmap;                   // To store color image which can be showed in from
        private bool _visualizeColorImage = true;       // true: color model, false: depth model

        private DepthSensor       _depthSensor;
        private ColorSensor       _colorSensor;
        private UserTracker       _userTracker;
        private SkeletonTracker   _skeletonTracker;
        private HandTracker       _handTracker;
        private GestureRecognizer _gestureRecognizer;

        private DepthFrame _depthFrame;                 // depth frame

        private SkeletonData    _skeletonData;          // person body skeleton data
        private HandTrackerData _handTrackerData;       // person hands data
        private IssuesData      _issuesData = null;

        // Labels
        static readonly string[] m_LABELNAMES = {"Stand", "Sit", "Walking", "StandUp", "SitDown", "TurnBack", "Others"};

        // Global flag
        bool m_IS_GRAB       = false;       // Grab the images?
        bool m_IS_WRITE_DATA = false;       // Write skeleton data?
        bool m_IS_AUTO       = false;       // Recognize the gesture automatically

        // Running time
        static readonly int m_MAX_TIME_RECORD = 100;
        List<double> m_runningTimeList = new List<double>();

        // Frame counter
        int m_frameCounter = 0;

        // Define handle
        FileHandle m_fileHandle = new FileHandle(m_LABELNAMES);     // Output infos
        ZpHandle m_zpHandle     = new ZpHandle();                   // Some useful functions

        // Data variables
        List<double> m_skeletonDataList = new List<double>();
        string[] m_skeletonDataLines;

        public MainForm()
        {
            InitializeComponent();

            // Set timer interval
            numericUpDownFPS_ValueChanged(null, null);

			// Enable double buffering to prevent flicker
			this.DoubleBuffered = true;   
        }

        ~MainForm()
		{
			_bitmap.Dispose();
		}

        // ---------------------------------------------------------------------------------------------------- //
        // ------------------------------- Important form control events: Grab -------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// timerGrab, doing everything
        ///     Updata the _skeletonTracker
        ///     Get and write data
        ///     Recognize the gesture
        ///     Disp result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerGrab_Tick(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;

            // -------------------- Update Nuitrack data -------------------- //
			try
			{
                // Data will be synchronized with skeleton time stamps.
                // The code will activate event: 
                    // onColorSensorUpdate: Update _bitmap only for color model
                    // onDepthSensorUpdate: Update _depthFrame
                    // onUserTrackerUpdate: Update _bitmap only for depth model
                    // onSkeletonUpdate   : Update _skeletonData
                    // onHandTrackerUpdate: Update _handTrackerData
                    // onIssueDataUpdate  : Update _issuesData
				Nuitrack.Update(_skeletonTracker);

                // Count frame
                m_frameCounter++;
			}
			catch(LicenseNotAcquiredException exception)
			{
                string output = string.Format("LicenseNotAcquired exception.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
			}
			catch(System.Exception exception)
			{
                string output = string.Format("Nuitrack update failed.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
			}

            // -------------------- Get data -------------------- //
            // Clear
            m_skeletonDataList.Clear();

			// Write skeleton joints
			if (_skeletonData != null)
			{
                // Draw joint size
	            const int jointSize = 5;

                // Loop for users 
				foreach (var skeleton in _skeletonData.Skeletons)
				{
                    // Loop for joints
					foreach (var joint in skeleton.Joints)
					{
                        // Update the _bitmap
                        _bitmap.SetCircle((int)(joint.Proj.X * _bitmap.Width), (int)(joint.Proj.Y * _bitmap.Height),
							              jointSize, Color.FromArgb(255, 255, 0, 0));

                        // Add skeleton data
                        m_skeletonDataList.Add((double)joint.Real.X);
                        m_skeletonDataList.Add((double)joint.Real.Y);
                        m_skeletonDataList.Add((double)joint.Real.Z);
					}

                    // Only add user 1
                    break;
				}
			}

            // -------------------- Write data -------------------- //
            // There are 25 points-3d in skeletonDataList for each user.
            if (m_IS_WRITE_DATA)
            {
                string skeletonDataStr = m_zpHandle.GetSkeletonData(m_frameCounter, m_skeletonDataList);
                m_fileHandle.WriteMsg(m_fileHandle.DataFile, skeletonDataStr);
                m_fileHandle.WriteImg(_bitmap.Bitmap, string.Format("{0}.bmp", m_frameCounter.ToString("00000")));
            }

            // -------------------- Disp -------------------- //
            // Disp running time
            m_runningTimeList.Add((DateTime.Now - startTime).TotalMilliseconds);
            m_zpHandle.DispRunningTime(m_frameCounter, m_runningTimeList, m_MAX_TIME_RECORD, ref statusStripGrab);

            // Disp skeleton data
            m_zpHandle.DispSkeletonData(m_skeletonDataList, ref textBoxSkeletonData);

            // -------------------- Refresh main from -------------------- //
            pictureBox.Image = _bitmap.Bitmap;
            pictureBox.Refresh();
        }

        /// <summary>
        /// Start or Stop the camera grab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (!m_IS_GRAB)
            {
                // --------------------- Initialize nuitrack ---------------------- //
			    try
			    {
				    Nuitrack.Init("");
			    }
			    catch(System.Exception exception)
			    {
                    string output = string.Format("Cannot initialize Nuitrack.\nException message: {0}", exception.Message);
                    MessageBox.Show(output);
				    Trace.WriteLine(output);
                    return;
			    }

                // ------------------------ Create modules ------------------------ //
			    try
			    {
				    // Create and setup all required modules
				    _depthSensor       = DepthSensor.Create();
				    _colorSensor       = ColorSensor.Create();
				    _userTracker       = UserTracker.Create();
				    _skeletonTracker   = SkeletonTracker.Create();
				    _handTracker       = HandTracker.Create();
				    _gestureRecognizer = GestureRecognizer.Create();

                    // Config
			        _depthSensor.SetMirror(false);      // Mirror left and right
			    }
			    catch(System.Exception exception)
			    {
                    string output = string.Format("Cannot create Nuitrack module.\nException message: {0}", exception.Message);
                    MessageBox.Show(output);
				    Trace.WriteLine(output);
                    return;
			    }

                // -------------------------- Add events -------------------------- //
			    // Add event handlers for all modules
			    _depthSensor.OnUpdateEvent             += onDepthSensorUpdate;
			    _colorSensor.OnUpdateEvent             += onColorSensorUpdate;
			    _userTracker.OnUpdateEvent             += onUserTrackerUpdate;
			    _skeletonTracker.OnSkeletonUpdateEvent += onSkeletonUpdate;
			    _handTracker.OnUpdateEvent             += onHandTrackerUpdate;
			    _gestureRecognizer.OnNewGesturesEvent  += onNewGestures;

			    // Add an event handler for the IssueUpdate event 
			    Nuitrack.onIssueUpdateEvent += onIssueDataUpdate;

                // ------------------------ Bitmap object ------------------------- //
			    // Create and configure the Bitmap object according to the depth sensor output mode
			    OutputMode mode      = _depthSensor.GetOutputMode();
			    OutputMode colorMode = _colorSensor.GetOutputMode();

			    if (mode.XRes < colorMode.XRes)
				    mode.XRes = colorMode.XRes;
			    if (mode.YRes < colorMode.YRes)
				    mode.YRes = colorMode.YRes;

			    _bitmap = new DirectBitmap(mode.XRes, mode.YRes);
			    for (int y = 0; y < mode.YRes; ++y)
			    {
				    for (int x = 0; x < mode.XRes; ++x)
					    _bitmap.SetPixel(x, y, Color.FromKnownColor(KnownColor.Aqua));
			    }

                // ------------------------- Run Nuitrack ------------------------- //
			    // Run Nuitrack. This starts sensor data processing.
			    try
			    {
				    Nuitrack.Run();
			    }
			    catch(System.Exception exception)
			    {
                    string output = string.Format("Cannot start Nuitrack.\nException message: {0}", exception.Message);
                    MessageBox.Show(output);
				    Trace.WriteLine(output);
                    return;
			    }

                // ------------------------- Update global variables ------------------------- //
                m_IS_GRAB            = true;
                m_frameCounter       = 0;
                timerGrab.Enabled    = true;
                buttonGrab.Text      = "Grab ◼";
                buttonGrab.BackColor = Color.GreenYellow;
            }
            else
            {
                // ------------------------- Release Nuitrack and remove all modules ------------------------- //
			    try
			    {
				    Nuitrack.onIssueUpdateEvent -= onIssueDataUpdate;

				    _depthSensor.OnUpdateEvent             -= onDepthSensorUpdate;
				    _colorSensor.OnUpdateEvent             -= onColorSensorUpdate;
				    _userTracker.OnUpdateEvent             -= onUserTrackerUpdate;
				    _skeletonTracker.OnSkeletonUpdateEvent -= onSkeletonUpdate;
				    _handTracker.OnUpdateEvent             -= onHandTrackerUpdate;
				    _gestureRecognizer.OnNewGesturesEvent  -= onNewGestures;

				    Nuitrack.Release();
			    }
			    catch(System.Exception exception)
			    {
                    string output = string.Format("Nuitrack release failed.\nException message: {0}", exception.Message);
                    MessageBox.Show(output);
				    Trace.WriteLine(output);
                    return;
			    }
                
                // ------------------------- Update global variables ------------------------- //
                m_IS_GRAB            = false;
                timerGrab.Enabled    = false;
                buttonGrab.Text      = "Grab ▶";
                buttonGrab.BackColor = Color.FromArgb(0, 255, 255, 255);
            }
        }

        /// <summary>
        /// Enabled or disabled write skeleton data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWrite_Click(object sender, EventArgs e)
        {
            if (!m_IS_WRITE_DATA)
            {
                // Set the global flag
                m_IS_WRITE_DATA = true;

                // Update the button control format
                buttonWrite.Text      = "Write ◼";
                buttonWrite.BackColor = Color.GreenYellow;

                // Reset
                m_frameCounter = 0;

                // Set the output file path
                m_fileHandle.SetFolder(string.Format("Output//{0}", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")));
                m_fileHandle.WriteMsg(m_fileHandle.DataFile, "Skeleton data (X, Y, Z) * 25 points. ");
            }
            else
            {
                // Set the global flag
                m_IS_WRITE_DATA       = false;

                // Update the button control format
                buttonWrite.Text      = "Write ▶";
                buttonWrite.BackColor = Color.FromArgb(0, 255, 255, 255);
            }
        }

        /// <summary>
        /// Enabled or disabled recognize the gesture automatically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAuto_Click(object sender, EventArgs e)
        {
            if (!m_IS_AUTO)
            {
                // Set the global flag
                m_IS_AUTO            = true;

                // Update the button control format
                buttonAuto.Text      = "Auto ◼";
                buttonAuto.BackColor = Color.GreenYellow;
            }
            else
            {
                // Set the global flag
                m_IS_AUTO            = false;

                // Update the button control format
                buttonAuto.Text      = "Auto ▶";
                buttonAuto.BackColor = Color.FromArgb(0, 255, 255, 255);
            }
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ------------------------------- Important form control events: Label ------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Load skeleton data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            // Selete a data file
            string dataFile = string.Empty;
            OpenFileDialog openFileDialog   = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "Output");
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title            = "Please select the data file. ";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataFile = openFileDialog.FileName;
            }
            else
            {
                return;
            }

            // Set the m_fileHandle
            string dataFolder = Path.GetDirectoryName(dataFile);
            if (m_fileHandle.IsValidFolder(dataFolder))
            {
                m_fileHandle.SetFolder(dataFolder);

                // Disp label count
                m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[0], ref buttonStandCnt);
                m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[1], ref buttonWalkingCnt);
                m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[2], ref buttonStandUpCnt);
                m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[3], ref buttonSitDownCnt);
                m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[4], ref buttonTurnBackCnt);

                // Enable the open folder button
                buttonStandCnt.Enabled    = true;
                buttonWalkingCnt.Enabled  = true;
                buttonStandUpCnt.Enabled  = true;
                buttonSitDownCnt.Enabled  = true;
                buttonTurnBackCnt.Enabled = true;
            }
            else
            {
                MessageBox.Show("Invalid folder. ");
                return;
            }

            // Read the data file
            m_skeletonDataLines = m_fileHandle.ReadAllLine(m_fileHandle.DataFile);

            // Disp the data list
            m_zpHandle.DispSkeletonDataList(m_skeletonDataLines, ref listBoxData, ref statusStripLabel);
        }

        /// <summary>
        /// Select data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disp the select items count
            statusStripLabel.Items["toolStripStatusLabelSelectData"].Text = listBoxData.SelectedItems.Count.ToString("00");

            // Show the image
            if (listBoxData.SelectedItems.Count > 0)
            {
                string item    = listBoxData.SelectedItems[listBoxData.SelectedItems.Count-1].ToString();
                string imgName = item.Split(' ')[0];
                string imgPath = Path.Combine(m_fileHandle.ImgaeFolder, imgName + ".bmp");
                Bitmap bitmap  = m_fileHandle.ReadImg(imgPath);

                // Draw the image
                pictureBox.Image = bitmap;
                pictureBox.Refresh();
            }

            // Enable the button
            // The number of select data must be equal numericUpDownCombineData.value
            if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
            {
                buttonStand.Enabled    = true;
                buttonSit.Enabled      = true; 
                buttonWalking.Enabled  = true;
                buttonStandUp.Enabled  = true;
                buttonSitDown.Enabled  = true;
                buttonTurnBack.Enabled = true;
                buttonOthers.Enabled   = true;
            }
            else
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Disp the select data and image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDispVedio_Click(object sender, EventArgs e)
        {
            // Show the image
            foreach (var selectItem in listBoxData.SelectedItems)
            {
                string item    = selectItem.ToString();
                string imgName = item.Split(' ')[0];
                string imgPath = Path.Combine(m_fileHandle.ImgaeFolder, imgName + ".bmp");
                Bitmap bitmap  = m_fileHandle.ReadImg(imgPath);

                // Draw the image
                pictureBox.Image = bitmap;
                pictureBox.Refresh();

                // Delay
                System.Threading.Thread.Sleep(Convert.ToInt32(numericUpDownDispDelay.Value));
            }
        }

        /// <summary>
        /// Auto select the next CombineData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAutoSelect_Click(object sender, EventArgs e)
        {
            // Must have the first index
            if (listBoxData.SelectedIndices.Count == 0)
            {
                return;
            }

            // Is next batch
            // The number of cur select items larger than CombineData, 
            // we will remove some items and leave the latest CoverData, 
            // then add up to CombineData. 
            bool isNext = false;

            // Calculate the count about select items
            int cntTotal = Convert.ToInt32(numericUpDownCombineData.Value);
            int cntCover = Convert.ToInt32(numericUpDownCoverData.Value);
            int cntCur   = listBoxData.SelectedIndices.Count;

            // Calculate how many data do we need now?
            int cntNeed  = 0;
            if (cntCur >= cntTotal)
            {
                cntNeed = cntTotal - cntCover;
                isNext  = true;
            }
            else
            {
                cntNeed = cntTotal - cntCur;
            }

            // Do we have enough data lfet?
            int indexCur = listBoxData.SelectedIndices[cntCur - 1];
            if (listBoxData.Items.Count - indexCur - 1 < cntNeed)
            {
                MessageBox.Show("Do not have enough datas. ");
                return;
            }

            // if auto next batch
            // Remove all items and add more items: it will refresh the pictureBox when add items.
            // In order to see the whole gesture processing.
            if (isNext)
            {
                listBoxData.SelectedIndices.Clear();
                indexCur = indexCur - cntCover;
                cntNeed  = cntNeed + cntCover;
            }

            // Add items
            for (int i = 0; i < cntNeed; i++)
            {
                listBoxData.SelectedIndices.Add(++indexCur);
            }
        }

        /// <summary>
        /// Write Stand label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStand_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[0]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[0], ref buttonStand);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write Sit label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSit_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[1]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[1], ref buttonSit);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write Walking label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWalking_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[2]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[2], ref buttonWalkingCnt);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write StandUp label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStandUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[3]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[3], ref buttonStandUpCnt);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write SitDown label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSitDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[4]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[4], ref buttonSitDownCnt);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write TurnBack label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTurnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[5]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[5], ref buttonTurnBackCnt);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Write Others label data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOthers_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxData.SelectedItems.Count == numericUpDownCombineData.Value)
                {
                    m_zpHandle.WriteLabelData(m_skeletonDataLines, listBoxData, m_fileHandle, m_LABELNAMES[6]);
                    m_zpHandle.DispLabelDataCnt(m_fileHandle.LabelFolder, m_LABELNAMES[6], ref buttonOthers);
                }
            }
            catch (System.Exception exception)
            {
                string output = string.Format("buttonStand_Click error.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
            }
            finally
            {
                buttonStand.Enabled    = false;
                buttonSit.Enabled      = false; 
                buttonWalking.Enabled  = false;
                buttonStandUp.Enabled  = false;
                buttonSitDown.Enabled  = false;
                buttonTurnBack.Enabled = false;
                buttonOthers.Enabled   = false;
            }
        }

        /// <summary>
        /// Open Stand label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStandCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[0]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open Sit label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSitCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[1]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open Walking label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWalkingCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[2]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open StandUp label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStandUpCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[3]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open SitDown label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSitDownCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[4]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open TurnBack label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTurnbackCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[5]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        /// <summary>
        /// Open Other label folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOthersCnt_Click(object sender, EventArgs e)
        {
            string folderUrl = Path.Combine(m_fileHandle.LabelFolder, m_LABELNAMES[6]);
            System.Diagnostics.Process.Start(folderUrl);
        }

        // ---------------------------------------------------------------------------------------------------- //
        // --------------------------------------- Form control events ---------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

		/// <summary>
        /// Switch visualization mode on a mouse click
        /// </summary>
        /// <param name="args"></param>
		protected override void OnClick(EventArgs args)
		{
			base.OnClick(args);

			//_visualizeColorImage = !_visualizeColorImage;
		}

        /// <summary>
        /// Event handler for the FormClosing event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// Release Nuitrack and remove all modules
			try
			{
                if (m_IS_GRAB)
                {
				    Nuitrack.onIssueUpdateEvent -= onIssueDataUpdate;

				    _depthSensor.OnUpdateEvent             -= onDepthSensorUpdate;
				    _colorSensor.OnUpdateEvent             -= onColorSensorUpdate;
				    _userTracker.OnUpdateEvent             -= onUserTrackerUpdate;
				    _skeletonTracker.OnSkeletonUpdateEvent -= onSkeletonUpdate;
				    _handTracker.OnUpdateEvent             -= onHandTrackerUpdate;
				    _gestureRecognizer.OnNewGesturesEvent  -= onNewGestures;

				    Nuitrack.Release();
                }
			}
			catch(System.Exception exception)
			{
                string output = string.Format("Nuitrack release failed.\nException message: {0}", exception.Message);
                MessageBox.Show(output);
				Trace.WriteLine(output);
                return;
			}
		}

        /// <summary>
        /// Set the timer interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownFPS_ValueChanged(object sender, EventArgs e)
        {
            timerGrab.Interval = Convert.ToInt32(1000.0 / (double)numericUpDownFPS.Value);
        }

        // ---------------------------------------------------------------------------------------------------- //
        // --------------------------------------- Updata camera datas ---------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Event handler for the IssueDataUpdate event: Update _issuesData
        /// </summary>
        /// <param name="issuesData"></param>
		private void onIssueDataUpdate(IssuesData issuesData)
		{
			_issuesData = issuesData;
		}

		/// <summary>
        /// Event handler for the DepthSensorUpdate event: Update _depthFrame
        /// </summary>
        /// <param name="depthFrame"></param>
		private void onDepthSensorUpdate(DepthFrame depthFrame)
		{
			_depthFrame = depthFrame;
		}

		/// <summary>
        /// Event handler for the ColorSensorUpdate event: Update _bitmap
        /// </summary>
        /// <param name="colorFrame"></param>
        /// Notes: Convert the color frame data to bitmap, only sampling directly
		private void onColorSensorUpdate(ColorFrame colorFrame)
		{
            // Only for color model
			if (!_visualizeColorImage)
				return;

            // Step value in width and height
			float wStep = (float)_bitmap.Width / colorFrame.Cols;
			float hStep = (float)_bitmap.Height / colorFrame.Rows;
			
			// color frame data: width x height x 3
            // color frame data format: b, g, r, b, g, r, ...
            // bitmap fromat: argb
			const int elemSizeInBytes = 3;  
			Byte[] data   = colorFrame.Data;
			int colorPtr  = 0;
			int bitmapPtr = 0;

            // Loop for rows
			float nextVerticalBorder = hStep;
			for (int i = 0; i < _bitmap.Height; ++i)
			{
				if (i == (int)nextVerticalBorder)
				{
					colorPtr += colorFrame.Cols * elemSizeInBytes;
					nextVerticalBorder += hStep;
				}

				int offset = 0;
				int argb = data[colorPtr] | (data[colorPtr + 1] << 8) | (data[colorPtr + 2] << 16) | (0xFF << 24);

                // Loop for cols
				float nextHorizontalBorder = wStep;
				for (int j = 0; j < _bitmap.Width; ++j)
				{
					if (j == (int)nextHorizontalBorder)
					{
						offset += elemSizeInBytes;
						argb = data[colorPtr + offset] | (data[colorPtr + offset + 1] << 8) | (data[colorPtr + offset + 2] << 16) | (0xFF << 24);
						nextHorizontalBorder += wStep;
					}

                    // Assign the bitmap
					_bitmap.Bits[bitmapPtr++] = argb;
				}
			}
		}

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------- NUITRICK handlers ----------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

		/// <summary>
        /// Event handler for the UserTrackerUpdate event
        /// </summary>
        /// <param name="userFrame"></param>
		private void onUserTrackerUpdate(UserFrame userFrame)
		{
            // Only for depth model
			if (_visualizeColorImage)
				return;
			if (_depthFrame == null)
				return;

            // labelIssueState???
			const int MAX_LABELS = 7;
			bool[] labelIssueState = new bool[MAX_LABELS];
			for (UInt16 label = 0; label < MAX_LABELS; ++label)
			{
				labelIssueState[label] = false;
				if (_issuesData != null)
				{
					FrameBorderIssue frameBorderIssue = _issuesData.GetUserIssue<FrameBorderIssue>(label);
					labelIssueState[label] = (frameBorderIssue != null);
				}
			}

            // Step value in width and height
			float wStep = (float)_bitmap.Width / _depthFrame.Cols;
			float hStep = (float)_bitmap.Height / _depthFrame.Rows;

            // depth Frame data: width x height x 2
			const int elemSizeInBytes = 2;
			Byte[] dataDepth = _depthFrame.Data;
			Byte[] dataUser = userFrame.Data;
			int dataPtr = 0;
			int bitmapPtr = 0;

            // Loop for rows
			float nextVerticalBorder = hStep;
			for (int i = 0; i < _bitmap.Height; ++i)
			{
				if (i == (int)nextVerticalBorder)
				{
					dataPtr += _depthFrame.Cols * elemSizeInBytes;
					nextVerticalBorder += hStep;
				}

				int offset = 0;
				int argb = 0;
				int label = dataUser[dataPtr] | dataUser[dataPtr + 1] << 8;
				int depth = Math.Min(255, (dataDepth[dataPtr] | dataDepth[dataPtr + 1] << 8) / 32);

                // Loop for cols
				float nextHorizontalBorder = wStep;
				for (int j = 0; j < _bitmap.Width; ++j)
				{
					if (j == (int)nextHorizontalBorder)
					{
						offset += elemSizeInBytes;
						label = dataUser[dataPtr + offset] | dataUser[dataPtr + offset + 1] << 8;
						if (label == 0)
							depth = Math.Min(255, (dataDepth[dataPtr + offset] | dataDepth[dataPtr + offset + 1] << 8) / 32);
						nextHorizontalBorder += wStep;
					}

					if (label > 0)
					{
						int user = label * 40;
						if (!labelIssueState[label])
							user += 40;
						argb = 0 | (user << 8) | (0 << 16) | (0xFF << 24);
					}
					else
					{
						argb = depth | (depth << 8) | (depth << 16) | (0xFF << 24);
					}

                    // Assign the bitmap
					_bitmap.Bits[bitmapPtr++] = argb;
				}
			}
		}

		/// <summary>
        /// Event handler for the SkeletonUpdate event: Update _skeletonData
        /// </summary>
        /// <param name="skeletonData"></param>
		private void onSkeletonUpdate(SkeletonData skeletonData)
		{
			_skeletonData = skeletonData;
		}

		/// <summary>
        /// Event handler for the HandTrackerUpdate event: Update _handTrackerData
        /// </summary>
        /// <param name="handTrackerData"></param>
		private void onHandTrackerUpdate(HandTrackerData handTrackerData)
		{
			_handTrackerData = handTrackerData;
		}

		/// <summary>
        /// Event handler for the gesture detection event: Disp gestures in the console
        /// </summary>
        /// <param name="gestureData"></param>
		private void onNewGestures(GestureData gestureData)
		{
			// Display the information about detected gestures in the console
			foreach (var gesture in gestureData.Gestures)
				Trace.WriteLine(string.Format("Recognized {0} from user {1}", gesture.Type.ToString(), gesture.UserID));
		}

        
    }
}
