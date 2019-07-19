using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectStreams
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        Mode _mode = Mode.Color;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;

        bool _displayBody = false;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Infrared)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                // Draw skeleton.
                                if (_displayBody)
                                {
                                    canvas.DrawSkeleton(body);

                                    Joint head = body.Joints[JointType.Head];
                                    HeadX.Text = head.Position.X.ToString();
                                    HeadY.Text = head.Position.Y.ToString();
                                    HeadZ.Text = head.Position.Z.ToString();

                                    float hx = head.Position.X * 30;
                                    float hy = head.Position.Y * 30;

                                    if(head.Position.X > hx || head.Position.Y > hy){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in Head joint";
                                       Alert.Background = Brushes.Red;
                                    }

                                    /*if(head.Position.Z > 1.5){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in Head joint";
                                       Alert.Background = Brushes.Red;
                                       }*/

                                    Joint Neck = body.Joints[JointType.Head];
                                    NeckX.Text = Neck.Position.X.ToString();
                                    NeckY.Text = Neck.Position.Y.ToString();
                                    NeckZ.Text = Neck.Position.Z.ToString();

                                   /* float nx = Neck.Position.X * 30;
                                    float ny = Neck.Position.Y * 30;

                                    if(Neck.Position.X > nx || Neck.Position.Y > ny){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in Neck joint";
                                       Alert.Background = Brushes.Red;
                                    }*/


                                    Joint SpineMid = body.Joints[JointType.Head];
                                    SpineMidX.Text = SpineMid.Position.X.ToString();
                                    SpineMidY.Text = SpineMid.Position.Y.ToString();
                                    SpineMidZ.Text = SpineMid.Position.Z.ToString();

                                    /* float smx = SpineMid.Position.X * 30;
                                    float smy = SpineMid.Position.Y * 30;

                                    if(SpineMid.Position.X > nx || SpineMid.Position.Y > ny){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in SpineMid joint";
                                       Alert.Background = Brushes.Red;
                                    }*/
                                    
                                    Joint SpineBase = body.Joints[JointType.Head];
                                    SpineBaseX.Text = SpineBase.Position.X.ToString();
                                    SpineBaseY.Text = SpineBase.Position.Y.ToString();
                                    SpineBaseZ.Text = SpineBase.Position.Z.ToString();

                                   /* float sbx = SpineBase.X *30;
                                    float sby = SpineBase.Y *30;

                                    if(SpineBase.Position.X > nx || SpineBase.Position.Y > ny){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in SpineBase joint";
                                       Alert.Background = Brushes.Red;
                                    }*/

                                    Joint ShoulderLeft = body.Joints[JointType.Head];
                                    ShoulderLeftX.Text = ShoulderLeft.Position.X.ToString();
                                    ShoulderLeftY.Text = ShoulderLeft.Position.Y.ToString();
                                    ShoulderLeftZ.Text = ShoulderLeft.Position.Z.ToString();

                                    /* float slx = ShoulderLeft.X *30;
                                    float sly = ShoulderLeft.Y *30;

                                    if(ShoulderLeft.Position.X > nx || ShoulderLeft.Position.Y > ny){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in ShoulderLeft joint";
                                       Alert.Background = Brushes.Red;
                                    }*/

                                    Joint ElbowLeft = body.Joints[JointType.Head];
                                    ElbowLeftX.Text = ElbowLeft.Position.X.ToString();
                                    ElbowLeftY.Text = ElbowLeft.Position.Y.ToString();
                                    ElbowLeftZ.Text = ElbowLeft.Position.Z.ToString();

                                    /* float slx = ShoulderLeft.X *30;
                                    float sly = ShoulderLeft.Y *30;

                                    if(ShoulderLeft.Position.X > nx || ShoulderLeft.Position.Y > ny){
                                       NotificationBlock.FontSize = 25;
                                       NotificationBlock.Content = "Alert !";
                                       NotificationBlock.Background = Brushes.Red;
                                       Alert.Content = "Abnormalisam Detected in ShoulderLeft joint";
                                       Alert.Background = Brushes.Red;
                                    }*/

                                    Joint WristLeft = body.Joints[JointType.Head];
                                    WristLeftX.Text = WristLeft.Position.X.ToString();
                                    WristLeftY.Text = WristLeft.Position.Y.ToString();
                                    WristLeftZ.Text = WristLeft.Position.Z.ToString();

                                    Joint HandLeft = body.Joints[JointType.Head];
                                    HandLeftX.Text = HandLeft.Position.X.ToString();
                                    HandLeftY.Text = HandLeft.Position.Y.ToString();
                                    HandLeftZ.Text = HandLeft.Position.Z.ToString();
                                    




                                    Joint ShoulderRight = body.Joints[JointType.Head];
                                    ShoulderRightX.Text = ShoulderRight.Position.X.ToString();
                                    ShoulderRightY.Text = ShoulderRight.Position.Y.ToString();
                                    ShoulderRightZ.Text = ShoulderRight.Position.Z.ToString();

                                    Joint ElbowRight = body.Joints[JointType.Head];
                                    ElbowRightX.Text = ElbowRight.Position.X.ToString();
                                    ElbowRightY.Text = ElbowRight.Position.Y.ToString();
                                    ElbowRightZ.Text = ElbowRight.Position.Z.ToString();

                                    Joint WristRight = body.Joints[JointType.Head];
                                    WristRightX.Text = WristRight.Position.X.ToString();
                                    WristRightY.Text = WristRight.Position.Y.ToString();
                                    WristRightZ.Text = WristRight.Position.Z.ToString();
                                    
                                    Joint HandRight = body.Joints[JointType.Head];
                                    HandRightX.Text = HandRight.Position.X.ToString();
                                    HandRightY.Text = HandRight.Position.Y.ToString();
                                    HandRightZ.Text = HandRight.Position.Z.ToString();

                                    Joint HipLeft = body.Joints[JointType.Head];
                                    HipLeftX.Text = HipLeft.Position.X.ToString();
                                    HipLeftY.Text = HipLeft.Position.Y.ToString();
                                    HipLeftZ.Text = HipLeft.Position.Z.ToString();

                                    Joint KneeLeft = body.Joints[JointType.Head];
                                    KneeLeftX.Text = KneeLeft.Position.X.ToString();
                                    KneeLeftY.Text = KneeLeft.Position.Y.ToString();
                                    KneeLeftZ.Text = KneeLeft.Position.Z.ToString();

                                    Joint AnkleLeft = body.Joints[JointType.Head];
                                    AnkleLeftX.Text = AnkleLeft.Position.X.ToString();
                                    AnkleLeftY.Text = AnkleLeft.Position.Y.ToString();
                                    AnkleLeftZ.Text = AnkleLeft.Position.Z.ToString();

                                    Joint FootLeft = body.Joints[JointType.Head];
                                    FootLeftX.Text = FootLeft.Position.X.ToString();
                                    FootLeftY.Text = FootLeft.Position.Y.ToString();
                                    FootLeftZ.Text = FootLeft.Position.Z.ToString();





                                    Joint HipRight = body.Joints[JointType.Head];
                                    HipRightX.Text = HipRight.Position.X.ToString();
                                    HipRightY.Text = HipRight.Position.Y.ToString();
                                    HipRightZ.Text = HipRight.Position.Z.ToString();

                                    Joint KneeRight = body.Joints[JointType.Head];
                                    KneeRightX.Text = KneeRight.Position.X.ToString();
                                    KneeRightY.Text = KneeRight.Position.Y.ToString();
                                    KneeRightZ.Text = KneeRight.Position.Z.ToString();

                                    Joint AnkleRight = body.Joints[JointType.Head];
                                    AnkleRightX.Text = AnkleRight.Position.X.ToString();
                                    AnkleRightY.Text = AnkleRight.Position.Y.ToString();
                                    AnkleRightZ.Text = AnkleRight.Position.Z.ToString();
                                    
                                    Joint FootRight = body.Joints[JointType.Head];
                                    FootRightX.Text = FootRight.Position.X.ToString();
                                    FootRightY.Text = FootRight.Position.Y.ToString();
                                    FootRightZ.Text = FootRight.Position.Z.ToString();

                                    Joint SpineShoulder = body.Joints[JointType.Head];
                                    SpineShoulderX.Text = SpineShoulder.Position.X.ToString();
                                    SpineShoulderY.Text = SpineShoulder.Position.Y.ToString();
                                    SpineShoulderZ.Text = SpineShoulder.Position.Z.ToString();

                                    Joint HandTipLeft = body.Joints[JointType.Head];
                                    HandTipLeftX.Text = HandTipLeft.Position.X.ToString();
                                    HandTipLeftY.Text = HandTipLeft.Position.Y.ToString();
                                    HandTipLeftZ.Text = HandTipLeft.Position.Z.ToString();

                                    Joint ThumbLeft = body.Joints[JointType.Head];
                                    ThumbLeftX.Text = ThumbLeft.Position.X.ToString();
                                    ThumbLeftY.Text = ThumbLeft.Position.Y.ToString();
                                    ThumbLeftZ.Text = ThumbLeft.Position.Z.ToString();

                                    Joint HandTipRight = body.Joints[JointType.Head];
                                    HandTipRightX.Text = HandTipRight.Position.X.ToString();
                                    HandTipRightY.Text = HandTipRight.Position.Y.ToString();
                                    HandTipRightZ.Text = HandTipRight.Position.Z.ToString();
                                    

                                    


                                    Joint ThumbRight = body.Joints[JointType.Head];
                                    ThumbRightX.Text = ThumbRight.Position.X.ToString();
                                    ThumbRightY.Text = ThumbRight.Position.Y.ToString();
                                    ThumbRightZ.Text = ThumbRight.Position.Z.ToString();



                                }
                            }
                        }
                    }
                }
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Depth;
        }

        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Infrared;
        }

        private void Body_Click(object sender, RoutedEventArgs e)
        {
            _displayBody = !_displayBody;
        }

         private void Reset_Click(object sender, RoutedEventArgs e)
        {
             NotificationBlock.Content = "No Abnomalisam";
             NotificationBlock.Background = Brushes.YellowGreen;
             Alert.Content = "No Abnormalism Detected";
             Alert.Background = Brushes.White;
             NotificationBlock.FontSize = 12;
        }

        #endregion
    }

    public enum Mode
    {
        Color,
        Depth,
        Infrared
    }

}
