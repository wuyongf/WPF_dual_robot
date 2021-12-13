using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PageDRUserFrameMotionSetup.xaml
    /// </summary>
    public partial class PageDRUserFrameMotionSetup : Page
    {
        private DualRobot dr = new DualRobot();
        private Transformation tf = new Transformation();

        // for lock/mutex lock
        // reference: https://stackoverflow.com/questions/5754879/usage-of-mutex-in-c-sharp
        //
        private readonly object syncLock = new object();

        public PageDRUserFrameMotionSetup(ref DualRobot dr_)
        {
            InitializeComponent();

            InitParams();

            dr = dr_;
        }

        private void InitParams()
        {
            TextBoxMeasureCircleRadius.Text = 80.ToString();
            TextBoxMeasureCircleArc.Text = 120.ToString();
            TextBoxMeasureCircleStepAngle.Text = 1.ToString();
        }


        private static void thread_uf_motion_circle(object syncLock, ref DualRobot dr, ref Transformation tf)
        {
            lock (syncLock)
            {
                if (dr.Model == "Fanuc CR-7iA/L")
                {
                    uf_motion_circle_v03(ref dr, ref tf);
                }

                if (dr.Model == "Fanuc CR15-iA")
                {
                    uf_motion_circle(ref dr, ref tf);
                }
            }
        }

        private static void uf_motion_circle(ref DualRobot dr, ref Transformation tf)
        {
            // check List Size
            var via_points_no = tf.via_points.Count;

            MessageBox.Show("via_point number: " + via_points_no);

            object Register1 = 0;
            object Register2 = 0;

            // loop
            for (int i = 0; i < via_points_no; i++)
            {
                // // wait for R[1], R[2] = 0
                var res1 = dr.getRegisterInt(1, ref Register1);
                var res2 = dr.getRegisterInt(2, ref Register2);

                while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                {
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);
                }

                // config the next via_point
                short UF = 1;
                short UT = 1;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(tf.via_points[i][0].ToString());
                PosArray[1] = float.Parse(tf.via_points[i][1].ToString());
                PosArray[2] = float.Parse(tf.via_points[i][2].ToString());
                PosArray[3] = float.Parse(tf.via_points[i][3].ToString());
                PosArray[4] = float.Parse(tf.via_points[i][4].ToString());
                PosArray[5] = float.Parse(tf.via_points[i][5].ToString());

                var res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                // Move
                dr.setRegister(1, 4, 1);
                dr.setRegister(2, 2, 1);

                // Count
                dr.setRegister(3, i + 1, 1);
            }
        }

        private static void uf_motion_circle_v02(ref DualRobot dr, ref Transformation tf)
        {
            // check List Size
            tf.via_orbit_points_part2 = tf.get_uf_orbit_points_part2_v01(0, 180, 90);

            var via_orbit_points_part2_no = tf.via_orbit_points_part2.Count;

            MessageBox.Show("via_point_part_2 number: " + via_orbit_points_part2_no);

            object Register1 = 0;
            object Register2 = 0;

            // loop -- part 2
            for (int i = 0; i < via_orbit_points_part2_no; i++)
            {
                // // wait for R[1], R[2] = 0
                var res1 = dr.getRegisterInt(1, ref Register1);
                var res2 = dr.getRegisterInt(2, ref Register2);

                while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                {
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);
                }

                // config the next via_point
                short UF = 1;
                short UT = 1;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(tf.via_orbit_points_part2[i][0].ToString());
                PosArray[1] = float.Parse(tf.via_orbit_points_part2[i][1].ToString());
                PosArray[2] = float.Parse(tf.via_orbit_points_part2[i][2].ToString());
                PosArray[3] = float.Parse(tf.via_orbit_points_part2[i][3].ToString());
                PosArray[4] = float.Parse(tf.via_orbit_points_part2[i][4].ToString());
                PosArray[5] = float.Parse(tf.via_orbit_points_part2[i][5].ToString());

                var res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                // Move
                dr.setRegister(1, 4, 1);
                dr.setRegister(2, 2, 1);

                // loop -- part 1
                tf.via_orbit_points_part1 = tf.get_uf_orbit_points_part1_v02(180, 1, PosArray);

                var via_orbit_points_part1_no = tf.via_orbit_points_part1.Count;

                for (int j = 0; j < via_orbit_points_part1_no; j++)
                {
                    // // wait for R[1], R[2] = 0
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);

                    while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                    {
                        res1 = dr.getRegisterInt(1, ref Register1);
                        res2 = dr.getRegisterInt(2, ref Register2);
                    }

                    // config the next via_point
                    UF = 1;
                    UT = 1;

                    PosArray = new float[6];
                    ConfigArray = new short[6];

                    PosArray[0] = float.Parse(tf.via_orbit_points_part1[j][0].ToString());
                    PosArray[1] = float.Parse(tf.via_orbit_points_part1[j][1].ToString());
                    PosArray[2] = float.Parse(tf.via_orbit_points_part1[j][2].ToString());
                    PosArray[3] = float.Parse(tf.via_orbit_points_part1[j][3].ToString());
                    PosArray[4] = float.Parse(tf.via_orbit_points_part1[j][4].ToString());
                    PosArray[5] = float.Parse(tf.via_orbit_points_part1[j][5].ToString());

                    res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                    // Move
                    dr.setRegister(1, 4, 1);
                    dr.setRegister(2, 2, 1);
                }

            }
        }

        private static void uf_motion_circle_v03(ref DualRobot dr, ref Transformation tf)
        {
            // check List Size
            tf.via_orbit_points_part2 = tf.get_uf_orbit_points_part2_v01(0, 180, 45);

            var via_orbit_points_part2_no = tf.via_orbit_points_part2.Count;

            MessageBox.Show("via_point_part_2 number: " + via_orbit_points_part2_no);

            object Register1 = 0;
            object Register2 = 0;

            // loop -- part 2
            for (int i = 0; i < via_orbit_points_part2_no; i++)
            {
                // // wait for R[1], R[2] = 0
                var res1 = dr.getRegisterInt(1, ref Register1);
                var res2 = dr.getRegisterInt(2, ref Register2);

                while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                {
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);
                }

                // config the next via_point
                short UF = 1;
                short UT = 2;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(tf.via_orbit_points_part2[i][0].ToString());
                PosArray[1] = float.Parse(tf.via_orbit_points_part2[i][1].ToString());
                PosArray[2] = float.Parse(tf.via_orbit_points_part2[i][2].ToString());
                PosArray[3] = float.Parse(tf.via_orbit_points_part2[i][3].ToString());
                PosArray[4] = float.Parse(tf.via_orbit_points_part2[i][4].ToString());
                PosArray[5] = float.Parse(tf.via_orbit_points_part2[i][5].ToString());

                var res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                // Move
                dr.setRegister(1, 4, 1);
                dr.setRegister(2, 2, 1);

                // wait for R[1], R[2] = 0
                while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                {
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);
                }

                // // 1. change to offset_tcp_temp
                var alpha = -PosArray[5];
                var rpy = tf.GetTempRPY(dr.offset_tcp, alpha);

                var res_offset = dr.SetOffsetTCPTemp(rpy);

                // 2. loop -- part 1 -- swing
                // var via_points = tf.get_uf_orbit_points_part1_v02(180, 1, PosArray);
                tf.via_orbit_points_part1 = tf.get_uf_orbit_points_part1_v03(180, 1, PosArray, rpy);
                // tf.via_orbit_points_part1 = tf.get_uf_orbit_points_part1_v04(180, 1, PosArray, dr.offset_tcp);

                var via_orbit_points_part1_no = tf.via_orbit_points_part1.Count;

                for (int j = 0; j < via_orbit_points_part1_no; j++)
                {
                    // // wait for R[1], R[2] = 0
                    res1 = dr.getRegisterInt(1, ref Register1);
                    res2 = dr.getRegisterInt(2, ref Register2);

                    while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
                    {
                        res1 = dr.getRegisterInt(1, ref Register1);
                        res2 = dr.getRegisterInt(2, ref Register2);
                    }

                    // config the next via_point
                    UF = 1;
                    UT = 2;

                    PosArray = new float[6];
                    ConfigArray = new short[6];

                    PosArray[0] = float.Parse(tf.via_orbit_points_part1[j][0].ToString());
                    PosArray[1] = float.Parse(tf.via_orbit_points_part1[j][1].ToString());
                    PosArray[2] = float.Parse(tf.via_orbit_points_part1[j][2].ToString());
                    PosArray[3] = float.Parse(tf.via_orbit_points_part1[j][3].ToString());
                    PosArray[4] = float.Parse(tf.via_orbit_points_part1[j][4].ToString());
                    PosArray[5] = float.Parse(tf.via_orbit_points_part1[j][5].ToString());

                    res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                    // Move
                    dr.setRegister(1, 4, 1);
                    dr.setRegister(2, 2, 1);
                }

                // // 3. change to offset_tcp
                dr.SetOffsetTCP();
            }
        }

        private void ButtonDefineUserFrame_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. Switch to Robot Base.
            dr.setRegister(1, 2, 1);
            dr.setRegister(2, 2, 1);

            // 2. Record the current pos to PR[98]
            var resRBCurPos = dr.getRBCurPos();

            float[] curPosArray = new float[6];
            short UF = 0;
            short UT = 0;

            if (resRBCurPos)
            {
                curPosArray[0] = float.Parse(dr.strRBCurPosX);
                curPosArray[1] = float.Parse(dr.strRBCurPosY);
                curPosArray[2] = float.Parse(dr.strRBCurPosZ);
                curPosArray[3] = float.Parse(dr.strRBCurPosW);
                curPosArray[4] = float.Parse(dr.strRBCurPosP);
                curPosArray[5] = float.Parse(dr.strRBCurPosR);
            }

            var res = dr.setRegisterPos(98, curPosArray, dr.config, UF, UT);

            // 3. Define User Frame
            dr.setRegister(1, 2, 1);
            dr.setRegister(2, 2, 1);

            // 4. Record the user frame origin.
            dr.arrayUFOrigin = curPosArray;

        }

        private void ButtonUpdateUFCurPos_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. get cur pos
            // 2. get cur config
            var res = dr.getUFCurPos();

            if (res)
            {
                // info the user
                TextBoxCurPosX.Text = dr.strUFCurPosX;
                TextBoxCurPosY.Text = dr.strUFCurPosY;
                TextBoxCurPosZ.Text = dr.strUFCurPosZ;
                TextBoxCurPosW.Text = dr.strUFCurPosW;
                TextBoxCurPosP.Text = dr.strUFCurPosP;
                TextBoxCurPosR.Text = dr.strUFCurPosR;
            }
        }

        private void ButtonApplyRegPos_OnClick(object sender, RoutedEventArgs e)
        {
            short UF = 1;
            short UT = 1;

            float[] PosArray = new float[6];

            var strTextBoxRegPosX = TextBoxCurPosX.Text;
            var strTextBoxRegPosY = TextBoxCurPosY.Text;
            var strTextBoxRegPosZ = TextBoxCurPosZ.Text;
            var strTextBoxRegPosW = TextBoxCurPosW.Text;
            var strTextBoxRegPosP = TextBoxCurPosP.Text;
            var strTextBoxRegPosR = TextBoxCurPosR.Text;

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

            var res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);
        }

        private void ButtonMove1_OnClick(object sender, RoutedEventArgs e)
        {
            // 0. Initialization
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
            dr.setRegister(3, 0, 1);

            // 1. Move to uf_via_point
            dr.setRegister(1, 4, 1);
            dr.setRegister(2, 2, 1);
        }

        private void ButtonApplyOrbitCircleParam_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCircleRadius = TextBoxCircleRadius.Text;

            var strTextBoxCircleStepRadian = TextBoxCircleStepDegree.Text;

            double param_radius, param_radian;

            bool isNumericParamRadian = double.TryParse(strTextBoxCircleStepRadian, out param_radian);

            bool isNumericParamRadius = double.TryParse(strTextBoxCircleRadius, out param_radius);


            bool isNumeric = isNumericParamRadius && isNumericParamRadian;

            if (isNumeric)
            {
                // assign the param: rb_measure_radius, rb_measure_step_radian
                tf.uf_orbit_radius = param_radius;
                tf.uf_orbit_step_angle = param_radian;

                // Notice User
                CardCircleMotion.Background = Brushes.Cornsilk;
                CardCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonMoveOrbitCircle_OnClick(object sender, RoutedEventArgs e)
        {
            // 0. Initialization
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
            dr.setRegister(3, 0, 1);

            // 1. define via_points
            tf.via_points = tf.get_uf_orbit_points(tf.uf_orbit_radius, tf.uf_orbit_step_angle);
            // 2. move
            Thread th = new Thread(() => thread_uf_motion_circle(syncLock, ref dr, ref tf));
            th.Start();
        }

        private void ButtonApplyMeasureCircleParam_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCircleRadius = TextBoxMeasureCircleRadius.Text;
            var strTextBoxMeasureCircleArc = TextBoxMeasureCircleArc.Text;
            var strTextBoxMeasureCircleStepAngle = TextBoxMeasureCircleStepAngle.Text;

            double param_radius, param_arc, param_step_angle;

            bool isNumericParamRadius = double.TryParse(strTextBoxCircleRadius, out param_radius);
            bool isNumericParamArc = double.TryParse(strTextBoxMeasureCircleArc, out param_arc);
            bool isNumericParamStepAngle = double.TryParse(strTextBoxMeasureCircleStepAngle, out param_step_angle);

            bool isNumeric = isNumericParamRadius && isNumericParamArc && isNumericParamStepAngle;

            if (isNumeric)
            {
                // assign the param
                tf.uf_measure_radius = param_radius;
                tf.uf_measure_arc = param_arc;
                tf.uf_measure_step_angle = param_step_angle;

                // Notice User
                CardMeasureCircleMotion.Background = Brushes.Cornsilk;
                CardMeasureCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonMoveMeasureCircle_OnClick(object sender, RoutedEventArgs e)
        {
            // 0. Initialization
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
            dr.setRegister(3, 0, 1);

            // 1. define via_points
            // tf.via_points = tf.get_uf_measure_points_v03(tf.uf_measure_radius, tf.uf_measure_arc, tf.uf_measure_step_angle);

            // 2.1 orbit_point_part_1
            // tf.via_points = tf.get_uf_orbit_points_part1_v01(tf.uf_measure_arc, tf.uf_measure_step_angle);

            // 2.2 orbit_point_part_2
            // tf.via_points = tf.get_uf_orbit_points_part2_v01(tf.uf_measure_radius, tf.uf_measure_arc, tf.uf_measure_step_angle);

            //// 2.3 simulation
            // var orbit_points_part1 = tf.get_uf_orbit_points_part1_v01(tf.uf_measure_arc, tf.uf_measure_step_angle);
            // var orbit_points_part2 = tf.get_uf_orbit_points_part2_v01(65, 180, 60);

            if (dr.Model == "Fanuc CR-7iA/L")
            {
                
            }

            if (dr.Model == "Fanuc CR15-iA")
            {
                tf.via_points = tf.get_uf_measure_points_v03(tf.uf_measure_radius, tf.uf_measure_arc, tf.uf_measure_step_angle);
            }



            // 2. move
            Thread th = new Thread(() => thread_uf_motion_circle(syncLock, ref dr, ref tf));
            th.Start();
        }
    }
}
