using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WPF_dual_robot
{
    /// <summary>
    /// Interaction logic for PageDRMotionSetup.xaml
    /// </summary>
    public partial class PageDRMotionSetup : Page
    {
        private DualRobot dr = new DualRobot();
        private Transformation tf = new Transformation();
        

        // for lock/mutex lock
        // reference: https://stackoverflow.com/questions/5754879/usage-of-mutex-in-c-sharp
        //
        private readonly object syncLock = new object();
        private readonly object syncLock1 = new object();

        public PageDRMotionSetup(string RobotModel, string IPAddress, int Port)
        {
            InitializeComponent();

            // Init Robot
            dr.Model = RobotModel;
            dr.HostName = IPAddress;
            dr.PortNumber = Port;

            // Init UI
            InitParam();

            // TryConnect
            if (dr.Init())
            {
                TextBoxConnectionStatus.Text = "Connected";
            }
            else
            {
                TextBoxConnectionStatus.Text = "Disconnected";
            }
        }

        private void InitParam()
        {
            TextBoxCircleRadius.Text = tf.radius.ToString();
            TextBoxCircleStepRadian.Text = tf.step_radian.ToString();

            TextBoxOrbitRadius.Text = tf.orbit_radius.ToString();
            TextBoxOrbitStepRadian.Text = tf.orbit_step_radian.ToString();
        }

        private static void thread_motion_multi_circles(object syncLock1, object syncLock, ref DualRobot dr, ref Transformation tf, ref int orbit_points_no)
        {
            lock (syncLock1)
            {
                // 1. define via_points
                tf.via_points  = tf.get_orbit_points(tf.center_point, tf.RMat, tf.orbit_radius, 0.01);
                // 2. move
                lock (syncLock)
                {
                    motion_circle(ref dr, ref tf);
                }


                //loop
                for (int j = 0; j < orbit_points_no; j++)
                {
                    // Initialization
                    dr.setRegister(1, 0, 1);
                    dr.setRegister(2, 0, 1);
                    dr.setRegister(3, 0, 1);

                    // update the orbit point.
                    tf.get_orbit_points(tf.center_point, tf.RMat, tf.orbit_radius, tf.orbit_step_radian);

                    var orbit_point = tf.orbit_points[j];

                    // 1. define via_points
                    tf.via_points = tf.get_measure_points(orbit_point, tf.RMat, tf.radius, tf.step_radian);
                    // 2. move
                    lock (syncLock)
                    {
                        motion_circle(ref dr, ref tf);
                    }
                }
            }
        }


        // tf: measure_points
        private static void thread_motion_circle(object syncLock, ref DualRobot dr, ref Transformation tf)
        {
            lock (syncLock)
            {
                motion_circle(ref dr,ref tf);
            }
        }

        private static void motion_circle(ref DualRobot dr, ref Transformation tf)
        {
            // check List Size
            var via_points_no = tf.via_points.Count;

            // MessageBox.Show("via_point number: " + via_points_no);

            object Register1 = 0;
            object Register2 = 0;

            // loop
            for (int i = 0; i < via_points_no; i++)
            {
                // // wait for R[1], R[2] = 0
                var res1 = dr.getRegisterInt(1, ref Register1);
                var res2 = dr.getRegisterInt(2, ref Register1);

                while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                {
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register1);
                }

                // config the next via_point
                short UF = 0;
                short UT = 0;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(tf.via_points[i][0].ToString());
                PosArray[1] = float.Parse(tf.via_points[i][1].ToString());
                PosArray[2] = float.Parse(tf.via_points[i][2].ToString());
                PosArray[3] = float.Parse(tf.via_points[i][3].ToString());
                PosArray[4] = float.Parse(tf.via_points[i][4].ToString());
                PosArray[5] = float.Parse(tf.via_points[i][5].ToString());

                ConfigArray[0] = 0;
                ConfigArray[1] = 0;
                ConfigArray[2] = 1;
                ConfigArray[3] = 1;
                ConfigArray[4] = 0;
                ConfigArray[5] = 0;

                var res = dr.setRegisterPos(100, PosArray, ConfigArray, UF, UT);

                // Move
                dr.setRegister(1, 1, 1);
                dr.setRegister(2, 2, 1);

                // Count
                dr.setRegister(3, i + 1, 1);
            }
        }


        private void ButtonApplyRegPos_OnClick(object sender, RoutedEventArgs e)
        {
            short UF = 0;
            short UT = 0;

            float[] PosArray = new float[6];
            short[] ConfigArray = new short[6];

            var strTextBoxRegPosX = TextBoxRegPosX.Text;
            var strTextBoxRegPosY = TextBoxRegPosY.Text;
            var strTextBoxRegPosZ = TextBoxRegPosZ.Text;
            var strTextBoxRegPosW = TextBoxRegPosW.Text;
            var strTextBoxRegPosP = TextBoxRegPosP.Text;
            var strTextBoxRegPosR = TextBoxRegPosR.Text;

            float floatPosX, floatPosY, floatPosZ, floatPosW, floatPosP, floatPosR;

            bool isPosXNumeric = float.TryParse(strTextBoxRegPosX, out floatPosX);
            bool isPosYNumeric = float.TryParse(strTextBoxRegPosY, out floatPosY);
            bool isPosZNumeric = float.TryParse(strTextBoxRegPosZ, out floatPosZ);
            bool isPosWNumeric = float.TryParse(strTextBoxRegPosW, out floatPosW);
            bool isPosPNumeric = float.TryParse(strTextBoxRegPosP, out floatPosP);
            bool isPosRNumeric = float.TryParse(strTextBoxRegPosR, out floatPosR);

            bool isNumeric = isPosXNumeric && isPosYNumeric && isPosZNumeric && isPosWNumeric && isPosPNumeric &&
                             isPosRNumeric;

            if (isNumeric)
            {
                PosArray[0] = floatPosX;
                PosArray[1] = floatPosY;
                PosArray[2] = floatPosZ;
                PosArray[3] = floatPosW;
                PosArray[4] = floatPosP;
                PosArray[5] = floatPosR;

                ConfigArray[0] = 0;
                ConfigArray[1] = 0;
                ConfigArray[2] = 1;
                ConfigArray[3] = 1;
                ConfigArray[4] = 0;
                ConfigArray[5] = 0;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }

            var res = dr.setRegisterPos(100, PosArray, ConfigArray, UF, UT);
        }

        private void ButtonMove1_OnClick(object sender, RoutedEventArgs e)
        {
            dr.setRegister(1, 1, 1);
            dr.setRegister(2, 2, 1);
        }

        private void ButtonResetMoveRegister_OnClick(object sender, RoutedEventArgs e)
        {
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
        }

        private void ButtonUpdateCurPos_OnClick(object sender, RoutedEventArgs e)
        {
            var res = dr.getCurPos();

            if (res)
            {
                // info the user
                TextBoxRegPosX.Text = dr.strCurPosX;
                TextBoxRegPosY.Text = dr.strCurPosY;
                TextBoxRegPosZ.Text = dr.strCurPosZ;
                TextBoxRegPosW.Text = dr.strCurPosW;
                TextBoxRegPosP.Text = dr.strCurPosP;
                TextBoxRegPosR.Text = dr.strCurPosR;

                // calculate the rotation matrix
                tf.RMat = tf.rpy2R(double.Parse(dr.strCurPosW),
                    double.Parse(dr.strCurPosP), double.Parse(dr.strCurPosR));

                // assign the center point
                tf.center_point[0] = double.Parse(dr.strCurPosX);
                tf.center_point[1] = double.Parse(dr.strCurPosY);
                tf.center_point[2] = double.Parse(dr.strCurPosZ);
                tf.center_point[3] = double.Parse(dr.strCurPosW);
                tf.center_point[4] = double.Parse(dr.strCurPosP);
                tf.center_point[5] = double.Parse(dr.strCurPosR);
            }
        }


        private void ButtonApplyCircleMotionParam_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCircleRadius = TextBoxCircleRadius.Text;

            var strTextBoxCircleStepRadian = TextBoxCircleStepRadian.Text;

            double param_radius, param_radian;

            bool isNumericParamRadian = double.TryParse(strTextBoxCircleStepRadian, out param_radian);

            bool isNumericParamRadius = double.TryParse(strTextBoxCircleRadius, out param_radius);


            bool isNumeric = isNumericParamRadius && isNumericParamRadian;

            if (isNumeric)
            {
                // assign the param: radius, step_radian
                tf.radius = param_radius;
                tf.step_radian = param_radian;

                // Notice User
                CardCircleMotion.Background = Brushes.Cornsilk;
                CardCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonMove2_OnClick(object sender, RoutedEventArgs e)
        {
            // Initialization
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
            dr.setRegister(3, 0, 1);

            // 1. get via points
            tf.via_points = tf.get_measure_points(tf.center_point, tf.RMat, tf.radius, tf.step_radian);
            // 2. move
            Thread th = new Thread(()=> thread_motion_circle(syncLock, ref dr, ref tf));
            th.Start();

        }

        private void ButtonApplyCircleMotionParam3_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxOrbitRadius = TextBoxOrbitRadius.Text;

            var strTextBoxOrbitStepRadian = TextBoxOrbitStepRadian.Text;

            double param_radius, param_radian;

            bool isNumericParamRadian = double.TryParse(strTextBoxOrbitStepRadian, out param_radian);

            bool isNumericParamRadius = double.TryParse(strTextBoxOrbitRadius, out param_radius);


            bool isNumeric = isNumericParamRadius && isNumericParamRadian;

            if (isNumeric)
            {
                // assign the param: radius, step_radian
                tf.orbit_radius = param_radius;
                tf.orbit_step_radian = param_radian;

                // Notice User
                CardMotionMultiCircles.Background = Brushes.Cornsilk;
                CardMotionMultiCircles.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonMove3_OnClick(object sender, RoutedEventArgs e)
        {
            tf.get_orbit_points(tf.center_point, tf.RMat, tf.orbit_radius, tf.orbit_step_radian);

            // check List Size
            var orbit_points_no = tf.orbit_points.Count;

            MessageBox.Show("orbit_point number: " + orbit_points_no);

            // Creating thread
            Thread th = new Thread(() => thread_motion_multi_circles(syncLock1, syncLock, ref dr, ref tf, ref orbit_points_no));
            th.Start();
        }

        private void ButtonMovePresetPos1_OnClick(object sender, RoutedEventArgs e)
        {
            // get from PR

            // write Pos

            // move
            dr.setRegister(1, 1, 1);
            dr.setRegister(2, 2, 1);
        }
    }
}
