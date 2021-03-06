﻿//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using System.Windows.Controls;
    using SkeletonBasics.Exercises;
    //using SkeletonBasics.HelperFunctions;
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const float RenderWidth = 640.0f;
        private const float RenderHeight = 480.0f;
        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;
        private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);       
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        private KinectSensor sensor;
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;

        // code for the csv data file
        private int moveNumber = 0;
        private static string fileBase = @"C:\Users\Megan\Documents\GitHub\CS279_FinalProject_MQ_VB\Code\SkeletonPerformanceTool\Data\";
        private static string folderName = HelperFunctions.GetFolderName(DateTime.Now);
        private static string columnHeadings = "TimeStamp,FootLeft_x,FootRight_x,FootLeft_y,FootRight_y,WristLeft_x,WristRight_x,WristLeft_z,WristRight_z,KneeLeft_x,KneeRight_x,KneeLeft_y,KneeRight_y,ElbowLeft_x,ElbowRight_x,ElbowLeft_y,ElbowRight_y,Head_y,AnkleLeft_y,AnkleRight_y\n";
        private string fileName;
        // globals to keep track of progress in routine (starts out as rest)
        //private Move currentMove = new Move(0, 60);
        private Boolean currentlyTracking = false;

        // variables to count the sets
        private int numJumpingJacks = 0;
        private int numArmCircles = 0;
        private int numKneeElbow = 0;
        private int numSquatJumps = 0;
        private int numSideToSide = 0;
        private int numHighMarches = 0;
        private int numTuckJumps = 0;

        /// Initializes a new instance of the MainWindow class.
        public MainWindow()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            //currentlyTracking = true;
            if (moveNumber == 0)
            {
                moveNumber = 1;
                System.IO.Directory.CreateDirectory(fileBase + folderName);
                //fileName = fileBase + folderName + @"\move" + moveNumber.ToString() + ".csv";
                //Console.WriteLine(fileName);
                //File.AppendAllText(fileName, columnHeadings);
                return;
            }
            else return;
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = false;
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            moveNumber++;
            fileName = fileBase + folderName + @"\move" + moveNumber.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void JumpingJackClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numJumpingJacks++;
            fileName = fileBase + folderName + @"\jumpingjack" + numJumpingJacks.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void ArmCirclesClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numArmCircles++;
            fileName = fileBase + folderName + @"\armcircles" + numArmCircles.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void KneeToElbowClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numKneeElbow++;
            fileName = fileBase + folderName + @"\kneetoelbow" + numKneeElbow.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void SquatJumpsClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numSquatJumps++;
            fileName = fileBase + folderName + @"\squatjumps" + numSquatJumps.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void SideToSideClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numSideToSide++;
            fileName = fileBase + folderName + @"\sidetoside" + numSideToSide.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void HighMarchesClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numHighMarches++;
            fileName = fileBase + folderName + @"\highmarches" + numHighMarches.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        private void TuckJumpsClick(object sender, RoutedEventArgs e)
        {
            currentlyTracking = true;
            numTuckJumps++;
            fileName = fileBase + folderName + @"\tuckjumps" + numTuckJumps.ToString() + ".csv";
            File.AppendAllText(fileName, columnHeadings);
        }

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // create directory where we will store data
            
            //File.AppendAllText(file, "Data\n");
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                //this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
                       
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // check to see if should be tracking data
            if (currentlyTracking)
            {
                StringBuilder sb = new StringBuilder();
                // add content to string builder
                sb.Append(HelperFunctions.GetMSeconds(DateTime.Now) + ", ");
                sb.Append(skeleton.Joints[JointType.FootLeft].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.FootRight].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.FootLeft].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.FootRight].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.WristLeft].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.WristRight].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.WristLeft].Position.Z.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.WristRight].Position.Z.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.KneeLeft].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.KneeRight].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.KneeLeft].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.KneeRight].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.ElbowLeft].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.ElbowRight].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.ElbowLeft].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.ElbowRight].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.Head].Position.X.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.AnkleLeft].Position.Y.ToString() + ", ");
                sb.Append(skeleton.Joints[JointType.AnkleRight].Position.Y.ToString() + "\n ");

                if (fileName != null)
                {
                    File.AppendAllText(fileName, sb.ToString());
                }
            }
            
            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);
                   
            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
 
            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;                    
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;                    
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        /// <summary>
        /// Handles the checking or unchecking of the seated mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
       
    }
}