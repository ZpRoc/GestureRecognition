using System;
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

        // Running time
        const int MAX_TIME_RECORD = 100;
        List<double> m_runningTimeList = new List<double>();

        // Frame counter
        int m_frameCounter = 0;

        // Write data
        List<double> m_skeletonDataList = new List<double>();

        public MainForm()
        {
            InitializeComponent();

            // ---------------------------------------------------------------- //
            // --------------------- Initialize nuitrack ---------------------- //
            // ---------------------------------------------------------------- //
			try
			{
				Nuitrack.Init("");
			}
			catch(System.Exception exception)
			{
				Trace.WriteLine("Cannot initialize Nuitrack.");
				throw exception;
			}

            // ---------------------------------------------------------------- //
            // ------------------------ Create modules ------------------------ //
            // ---------------------------------------------------------------- //
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
				Trace.WriteLine("Cannot create Nuitrack module.");
				throw exception;
			}

            // ---------------------------------------------------------------- //
            // -------------------------- Add events -------------------------- //
            // ---------------------------------------------------------------- //
			// Add event handlers for all modules
			_depthSensor.OnUpdateEvent             += onDepthSensorUpdate;
			_colorSensor.OnUpdateEvent             += onColorSensorUpdate;
			_userTracker.OnUpdateEvent             += onUserTrackerUpdate;
			_skeletonTracker.OnSkeletonUpdateEvent += onSkeletonUpdate;
			_handTracker.OnUpdateEvent             += onHandTrackerUpdate;
			_gestureRecognizer.OnNewGesturesEvent  += onNewGestures;

			// Add an event handler for the IssueUpdate event 
			Nuitrack.onIssueUpdateEvent += onIssueDataUpdate;

            // ---------------------------------------------------------------- //
            // ------------------------ Bitmap object ------------------------- //
            // ---------------------------------------------------------------- //
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

            // ---------------------------------------------------------------- //
            // ------------------------- Run Nuitrack ------------------------- //
            // ---------------------------------------------------------------- //
			// Run Nuitrack. This starts sensor data processing.
			try
			{
				Nuitrack.Run();
			}
			catch(System.Exception exception)
			{
				Trace.WriteLine("Cannot start Nuitrack.");
				throw exception;
			}

            // ---------------------------------------------------------------- //
            // ----------------------- Config MainForm ------------------------ //
            // ---------------------------------------------------------------- //
			// Enable double buffering to prevent flicker
			this.DoubleBuffered = true;

            
        }

        ~MainForm()
		{
			_bitmap.Dispose();
		}

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

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
			}
			catch(LicenseNotAcquiredException exception)
			{
				Trace.WriteLine("LicenseNotAcquired exception. Exception: " + exception.Message);
				throw exception;
			}
			catch(System.Exception exception)
			{
				Trace.WriteLine("Nuitrack update failed. Exception: " + exception.Message);
			}

            // -------------------- Get data -------------------- //
            // Clear
             m_skeletonDataList.Clear();

			// Write skeleton joints
			if (_skeletonData != null)
			{
                // Loop for users 
				foreach (var skeleton in _skeletonData.Skeletons)
				{
                    // Loop for joints
					foreach (var joint in skeleton.Joints)
					{
                        // Add skeleton data
                        m_skeletonDataList.Add((double)joint.Real.X);
                        m_skeletonDataList.Add((double)joint.Real.Y);
                        m_skeletonDataList.Add((double)joint.Real.Z);
					}
				}
			}

            // -------------------- Write data -------------------- //
            // There are 25 points-3d in skeletonDataList for each user.
            if (m_skeletonDataList.Count != 0)
            {
                // Disp skeleton data
                DispSkeletonData(m_skeletonDataList);
                
            }

            // -------------------- Refresh main from -------------------- //
            this.Refresh();

            // -------------------- Output running time -------------------- //
            m_runningTimeList.Add((DateTime.Now - startTime).TotalMilliseconds);
            DispRunningTime(m_runningTimeList, MAX_TIME_RECORD);
        }

        /// <summary>
        /// Start or Stop the camera grab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStartGrab_Click(object sender, EventArgs e)
        {
            if (timerGrab.Enabled == false)
            {
                timerGrab.Enabled = true;
                buttonGrab.Text = "Stop Grab";
                buttonGrab.BackColor = new SystemColors()
            }
            else
            {
                timerGrab.Enabled = false;
                buttonGrab.Text = "Start Grab";
            }
        }

        /// <summary>
        /// Disp running time
        /// </summary>
        /// <param name="timeList"></param>
        /// <param name="maxRecord"></param>
        private void DispRunningTime(List<double> timeList, int maxRecord)
        {
            // Remove the old one
            if (timeList.Count > maxRecord)
            {
                timeList.RemoveRange(0, timeList.Count - maxRecord);
            }

            // Cur time
            toolStripStatusLabelCurTime.Text = string.Format("Cur: {0} ms", timeList[timeList.Count - 1].ToString("00.00"));

            // Avg time
            toolStripStatusLabelAvgTime.Text = string.Format("Avg: {0} ms", timeList.Average().ToString("00.00"));

            // Max time
            toolStripStatusLabelMaxTime.Text = string.Format("Max: {0} ms", timeList.Max().ToString("00.00"));
        }

        /// <summary>
        /// Disp skeleton data
        /// </summary>
        /// <param name="skeletonData"></param>
        private void DispSkeletonData(List<double> skeletonData)
        {
            string disp_str = "          X           Y           Z   ";
            for (int i = 0; i < skeletonData.Count; i+=3)
            {
                disp_str += Environment.NewLine;
                disp_str += string.Format("{0, 2}   {1, 9}   {2, 9}   {3, 9}", (i/3+1).ToString("00"), skeletonData[i+0].ToString("0.000"), 
                                            skeletonData[i+1].ToString("0.000"), skeletonData[i+2].ToString("0.000"));
            }
            textBoxSkeletonData.Text = disp_str;
        }

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

		/// <summary>
        /// Switch visualization mode on a mouse click
        /// </summary>
        /// <param name="args"></param>
		protected override void OnClick(EventArgs args)
		{
			base.OnClick(args);

			_visualizeColorImage = !_visualizeColorImage;
		}

        /// <summary>
        /// Event handler for the Paint event
        /// </summary>
        /// <param name="args"></param>
		protected override void OnPaint(PaintEventArgs args)
		{
            base.OnPaint(args);

			// Draw a bitmap (color model or depth model)
			args.Graphics.DrawImage(_bitmap.Bitmap, new Point(0, 0));

			// Draw skeleton joints
			if (_skeletonData != null)
			{
                // Draw joint size
				const int jointSize = 10;

                // Loop for users 
				foreach (var skeleton in _skeletonData.Skeletons)
				{
                    // Define brush
					SolidBrush brush = new SolidBrush(Color.FromArgb(255 - 40 * skeleton.ID, 0, 0));

                    // Loop for joints
					foreach (var joint in skeleton.Joints)
					{
                        // Draw
						args.Graphics.FillEllipse(brush, joint.Proj.X * _bitmap.Width - jointSize / 2,
												  joint.Proj.Y * _bitmap.Height - jointSize / 2, jointSize, jointSize);
					}
				}
			}

			// Draw hand pointers
			if (_handTrackerData != null)
			{
				foreach (var userHands in _handTrackerData.UsersHands)
				{
					if (userHands.LeftHand != null)
					{
                        // Get left hand value
						HandContent hand = userHands.LeftHand.Value;
						int size = hand.Click ? 20 : 30;

                        // Define brush
						Brush brush = new SolidBrush(Color.Aquamarine);

                        // Draw left hand
						args.Graphics.FillEllipse(brush, hand.X * _bitmap.Width - size/2, hand.Y * _bitmap.Height - size / 2, size, size);
                    }

					if (userHands.RightHand != null)
					{
                        // Get right hand value
						HandContent hand = userHands.RightHand.Value;
						int size = hand.Click ? 20 : 30;

                        // Define brush
						Brush brush = new SolidBrush(Color.DarkBlue);

                        // Draw right hand
						args.Graphics.FillEllipse(brush, hand.X * _bitmap.Width - size/2, hand.Y * _bitmap.Height - size / 2, size, size);
					}
				}
			}
		}

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
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
        // ---------------------------------------------------------------------------------------------------- //
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

        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// Event handler for the FormClosing event
        /// </summary>
        /// <param name="e"></param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			// Release Nuitrack and remove all modules
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
				Trace.WriteLine("Nuitrack release failed.");
				throw exception;
			}
		}

        
    }
}
