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
    /// Interaction logic for PageDRRobotBaseMotion.xaml
    /// </summary>
    public partial class PageDRRobotBaseMotion : Page
    {
        private DualRobot dr = new DualRobot();
        private Transformation tf = new Transformation();
        

        // for lock/mutex lock
        // reference: https://stackoverflow.com/questions/5754879/usage-of-mutex-in-c-sharp
        //
        private readonly object syncLock = new object();
        private readonly object syncLock1 = new object();

        public PageDRRobotBaseMotion(ref DualRobot dr_)
        {
            InitializeComponent();

            InitParam();

            dr = dr_;
        }

        private void InitParam()
        {
            TextBoxCircleRadius.Text = tf.rb_measure_radius.ToString();
            TextBoxCircleStepRadian.Text = tf.rb_measure_step_radian.ToString();

            TextBoxOrbitRadius.Text = tf.rb_orbit_radius.ToString();
            TextBoxOrbitStepRadian.Text = tf.rb_orbit_step_angle.ToString();
        }

        private static void thread_motion_multi_circles(object syncLock1, object syncLock, ref DualRobot dr, ref Transformation tf, ref int orbit_points_no)
        {
            lock (syncLock1)
            {
                // 1. define via_points
                tf.via_points  = tf.get_rb_orbit_points(tf.rb_center_point, tf.RMat, tf.rb_orbit_radius, 0.01);
                // 2. move
                lock (syncLock)
                {
                    rb_motion_circle(ref dr, ref tf);
                }


                //loop
                for (int j = 0; j < orbit_points_no; j++)
                {
                    // Initialization
                    dr.setRegister(1, 0, 1);
                    dr.setRegister(2, 0, 1);
                    dr.setRegister(3, 0, 1);

                    // update the orbit point.
                    tf.get_rb_orbit_points(tf.rb_center_point, tf.RMat, tf.rb_orbit_radius, tf.rb_orbit_step_angle);

                    var orbit_point = tf.rb_orbit_points[j];

                    // 1. define via_points
                    tf.via_points = tf.get_rb_measure_points(orbit_point, tf.RMat, tf.rb_measure_radius, tf.rb_measure_step_radian);
                    // 2. move
                    lock (syncLock)
                    {
                        rb_motion_circle(ref dr, ref tf);
                    }
                }
            }
        }


        // tf: rb_measure_points
        private static void thread_rb_motion_circle(object syncLock, ref DualRobot dr, ref Transformation tf)
        {
            lock (syncLock)
            {
                rb_motion_circle(ref dr,ref tf);
            }
        }

        private static void rb_motion_circle(ref DualRobot dr, ref Transformation tf)
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
                dr.setRegister(1, 3, 1);
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
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }

            var res = dr.setRegisterPos(100, PosArray, dr.config, UF, UT);
        }

        private void ButtonMove1_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. Change to Robot Base
            dr.setRegister(1, 1, 1);
            dr.setRegister(2, 2, 1);

            // 2. Move to rb_via_point
            dr.setRegister(1, 3, 1);
            dr.setRegister(2, 2, 1);
        }

        

        private void ButtonUpdateCurPos_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. get cur pos
            // 2. get cur config
            var res = dr.getRBCurPos();

            if (res)
            {
                // info the user
                TextBoxRegPosX.Text = dr.strRBCurPosX;
                TextBoxRegPosY.Text = dr.strRBCurPosY;
                TextBoxRegPosZ.Text = dr.strRBCurPosZ;
                TextBoxRegPosW.Text = dr.strRBCurPosW;
                TextBoxRegPosP.Text = dr.strRBCurPosP;
                TextBoxRegPosR.Text = dr.strRBCurPosR;

                // calculate the rotation matrix
                tf.RMat = tf.rpy2R(double.Parse(dr.strRBCurPosW),
                    double.Parse(dr.strRBCurPosP), double.Parse(dr.strRBCurPosR));

                // assign the center point
                tf.rb_center_point[0] = double.Parse(dr.strRBCurPosX);
                tf.rb_center_point[1] = double.Parse(dr.strRBCurPosY);
                tf.rb_center_point[2] = double.Parse(dr.strRBCurPosZ);
                tf.rb_center_point[3] = double.Parse(dr.strRBCurPosW);
                tf.rb_center_point[4] = double.Parse(dr.strRBCurPosP);
                tf.rb_center_point[5] = double.Parse(dr.strRBCurPosR);
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
                // assign the param: rb_measure_radius, rb_measure_step_radian
                tf.rb_measure_radius = param_radius;
                tf.rb_measure_step_radian = param_radian;

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
            tf.via_points = tf.get_rb_measure_points(tf.rb_center_point, tf.RMat, tf.rb_measure_radius, tf.rb_measure_step_radian);
            // 2. move
            Thread th = new Thread(()=> thread_rb_motion_circle(syncLock, ref dr, ref tf));
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
                // assign the param: rb_measure_radius, rb_measure_step_radian
                tf.rb_orbit_radius = param_radius;
                tf.rb_orbit_step_angle = param_radian;

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
            tf.get_rb_orbit_points(tf.rb_center_point, tf.RMat, tf.rb_orbit_radius, tf.rb_orbit_step_angle);

            // check List Size
            var orbit_points_no = tf.rb_orbit_points.Count;

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
