using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using FRRJIf;
using FRRobotIFLib;

namespace WPF_dual_robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// @input:
    /// 1. hostname (ip address)
    /// 2. port number (for simulation)
    /// </summary>
    public partial class MainWindow : Window
    {
        private DualRobot drCR7 = new DualRobot();
        private Transformation tfCR7 = new Transformation();
        private DualRobot drCR15 = new DualRobot();
        private Transformation tfCR15 = new Transformation();

        public MainWindow()
        {
            InitializeComponent();

            InitParam();
        }

        private void InitParam()
        {

            // cr7
            TextBoxIpAddress1.Text = "192.168.3.124";
            
            // cr15
            TextBoxIpAddress2.Text = "127.0.0.1";

            TextBoxSimPortNumber1.Text = 60008.ToString();
            TextBoxSimPortNumber2.Text = 9021.ToString();
        }

        private void ButtonSimConnect1_OnClick(object sender, RoutedEventArgs e)
        {
            string strIPAddress = TextBoxIpAddress1.Text;
            string strRobotModel = TextBlockRobot1.Text;

            var strPort = TextBoxSimPortNumber1.Text;
            int PortNumber;
            bool isNumeric = int.TryParse(strPort, out PortNumber);

            if (isNumeric)
            {
                //  string RobotModel, string IPAddress,int Port

                // Init Robot
                drCR7.Model = strRobotModel;
                drCR7.HostName = strIPAddress;
                drCR7.PortNumber = PortNumber;

                // TryConnect
                if (drCR7.Init())
                {
                    WindowFrameDualRobot windowFrameDualRobot = new WindowFrameDualRobot(ref drCR7);
                    windowFrameDualRobot.Show();
                }
                else
                {
                    MessageBox.Show("Connection Failed! Please Check!");
                }
            }
            else
            {
                MessageBox.Show("Port: Please Input Integer Number!");
            }
        }

        private void ButtonSimConnect2_OnClick(object sender, RoutedEventArgs e)
        {
            string strIPAddress = TextBoxIpAddress2.Text;
            string strRobotModel = TextBlockRobot2.Text;

            var strPort = TextBoxSimPortNumber2.Text;
            int PortNumber;
            bool isNumeric = int.TryParse(strPort, out PortNumber);

            if (isNumeric)
            {
                //  string RobotModel, string IPAddress,int Port

                // Init Robot
                drCR15.Model = strRobotModel;
                drCR15.HostName = strIPAddress;
                drCR15.PortNumber = PortNumber;

                // TryConnect
                if (drCR15.Init())
                {
                    WindowFrameDualRobot windowFrameDualRobot = new WindowFrameDualRobot(ref drCR15);
                    windowFrameDualRobot.Show();
                }
                else
                {
                    MessageBox.Show("Connection Failed! Please Check!");
                }
            }
            else
            {
                MessageBox.Show("Port: Please Input Integer Number!");
            }
        }

        private void ButtonResetOrbitCircleParam_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxOrbitCircleRadius.Clear();
            TextBoxOrbitCircleStepAngle.Clear();
        }

        private void ButtonApplyOrbitCircleParam_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCircleRadius = TextBoxOrbitCircleRadius.Text;
            var strTextBoxCircleStepAngle = TextBoxOrbitCircleStepAngle.Text;

            double param_radius, param_step_angle;

            bool isNumericParamRadian = double.TryParse(strTextBoxCircleStepAngle, out param_step_angle);
            bool isNumericParamRadius = double.TryParse(strTextBoxCircleRadius, out param_radius);

            bool isNumeric = isNumericParamRadius && isNumericParamRadian;

            if (isNumeric)
            {
                // assign the param: rb_measure_radius, rb_measure_step_radian
                tfCR7.uf_orbit_radius = param_radius;
                tfCR7.uf_orbit_step_angle = param_step_angle;

                // Notice User
                CardOrbitCircleMotion.Background = Brushes.Cornsilk;
                CardOrbitCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonResetMeasureCircleParam_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxMeasureCircleRadius.Clear(); 
            TextBoxMeasureCircleArc.Clear();
            TextBoxMeasureCircleStepAngle.Clear();
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
                tfCR15.uf_measure_radius = param_radius;
                tfCR15.uf_measure_arc = param_arc;
                tfCR15.uf_measure_step_angle = param_step_angle;

                // Notice User
                CardMeasureCircleMotion.Background = Brushes.Cornsilk;
                CardMeasureCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonStartScenario1A_OnClick(object sender, RoutedEventArgs e)
        {
            // get orbit_points

            // get measurement_points

            // init
            Init_scen1A();

            // move, 2 thread.Start()
            Thread th = new Thread(() => thread_Scene1A(syncLock_scen1A));
            th.Start();
            // cout the result.
        }

        /// <summary>
        /// Scenario A
        /// </summary>
        private readonly object syncLock_scen1A_CR7 = new object();
        private readonly object syncLock_scen1A_CR15 = new object();
        private readonly object syncLock_scen1A = new object();

        private int move_flag = 1;

        private void Init_scen1A()
        {
            // 1. prepare the status_points
            //todo:

            // 2. clear register
            drCR7.setRegister(1, 0, 1);
            drCR7.setRegister(2, 0, 1);
            drCR7.setRegister(3, 0, 1);
            drCR7.setRegister(4, 0, 1);
            drCR7.setRegister(5, 0, 1);
            drCR7.setRegister(6, 0, 1);
            drCR7.setRegister(7, 0, 1);

            drCR7.setRegister(102, 0, 1);

            drCR15.setRegister(1, 0, 1);
            drCR15.setRegister(2, 0, 1);
            drCR15.setRegister(3, 0, 1);
            drCR15.setRegister(4, 0, 1);
            drCR15.setRegister(5, 0, 1);
            drCR15.setRegister(6, 0, 1);
            drCR15.setRegister(7, 0, 1);

            drCR15.setRegister(102, 0, 1);
        }

        private void thread_Scene1A(object syncLock)
        {
            lock (syncLock)
            {
                Scene1A();
            }
        }

        private void Scene1A()
        {
            // CR7:  192.168.3.124
            // CR15: 192.168.3.125

            // CR7:  140 +-70
            // CR15: 130 +-65 // 180

            // tfCR7.rb_orbit_radius;
            // tfCR7.rb_orbit_step_angle;
            // tfCR7.via_points;
            // tfCR7.status_orbit_points

            // 0. Initialization
            drCR7.setRegister(1, 0, 1);
            drCR7.setRegister(2, 0, 1);
            drCR7.setRegister(3, 0, 1);
            drCR15.setRegister(1, 0, 1);
            drCR15.setRegister(2, 0, 1);
            drCR15.setRegister(3, 0, 1);

            // 1. CR15 Path
            tfCR15.via_points = tfCR15.get_uf_measure_points_v03(0, 130, 0.5);

            // 2. CR7 Path
            // check List Size
            tfCR7.via_orbit_points_part2 = tfCR7.get_uf_orbit_points_part2_v01(0, 180, 45);

            var via_orbit_points_part2_no = tfCR7.via_orbit_points_part2.Count;

            // MessageBox.Show("via_point_part_2 number: " + via_orbit_points_part2_no);


            // loop -- part 2

            drCR7.WaitForReady();
            drCR7.Wait(100);

            for (int i = 0; i < via_orbit_points_part2_no; i++)
            {
                // config the next via_point
                short UF = 1;
                short UT = 2;

                float[] PosArray_part2 = new float[6];
                short[] ConfigArray = new short[6];

                PosArray_part2[0] = float.Parse(tfCR7.via_orbit_points_part2[i][0].ToString());
                PosArray_part2[1] = float.Parse(tfCR7.via_orbit_points_part2[i][1].ToString());
                PosArray_part2[2] = float.Parse(tfCR7.via_orbit_points_part2[i][2].ToString());
                PosArray_part2[3] = float.Parse(tfCR7.via_orbit_points_part2[i][3].ToString());
                PosArray_part2[4] = float.Parse(tfCR7.via_orbit_points_part2[i][4].ToString());
                PosArray_part2[5] = float.Parse(tfCR7.via_orbit_points_part2[i][5].ToString());

                var res = drCR7.setRegisterPos(99, PosArray_part2, drCR7.config, UF, UT);

                // Move
                drCR7.setRegister(1, 4, 1);
                drCR7.setRegister(2, 2, 1);

                drCR7.WaitForReady();
                drCR7.Wait(100);

                // (1). change to offset_tcp_temp
                var alpha = -PosArray_part2[5];

                // MessageBox.Show("drCR7.offset_tcp:" + drCR7.offset_tcp[3] + "," + drCR7.offset_tcp[4] + "," + drCR7.offset_tcp[5]);
                
                var rpy = tfCR7.GetTempRPY(drCR7.offset_tcp, alpha);
                
                var res_offset = drCR7.SetOffsetTCPTemp(rpy);
                
                // (2). loop -- part 1 -- swing
                // var via_points = tf.get_uf_orbit_points_part1_v02(180, 1, PosArray);
                tfCR7.via_orbit_points_part1 = tfCR7.get_uf_orbit_points_part1_v03(180, 90, PosArray_part2, rpy);
                // tf.via_orbit_points_part1 = tf.get_uf_orbit_points_part1_v04(180, 1, PosArray, dr.offset_tcp);
                
                var via_orbit_points_part1_no = tfCR7.via_orbit_points_part1.Count;
                
                drCR7.WaitForReady();
                drCR7.Wait(100);
                for (int j = 0; j < via_orbit_points_part1_no; j++)
                {
                    // config the next via_point
                    UF = 1;
                    UT = 2;
                
                    float[] PosArray_part1 = new float[6];
                    ConfigArray = new short[6];
                
                    PosArray_part1[0] = float.Parse(tfCR7.via_orbit_points_part1[j][0].ToString());
                    PosArray_part1[1] = float.Parse(tfCR7.via_orbit_points_part1[j][1].ToString());
                    PosArray_part1[2] = float.Parse(tfCR7.via_orbit_points_part1[j][2].ToString());
                    PosArray_part1[3] = float.Parse(tfCR7.via_orbit_points_part1[j][3].ToString());
                    PosArray_part1[4] = float.Parse(tfCR7.via_orbit_points_part1[j][4].ToString());
                    PosArray_part1[5] = float.Parse(tfCR7.via_orbit_points_part1[j][5].ToString());
                
                    res = drCR7.setRegisterPos(99, PosArray_part1, drCR7.config, UF, UT);

                    MessageBox.Show("CR7: Move!");

                    // Move
                    drCR7.setRegister(1, 4, 1);
                    drCR7.setRegister(2, 2, 1);
                
                    drCR7.WaitForReady();
                    drCR7.Wait(100);

                    /// CR15
                    CR15_Motion(ref drCR15, ref tfCR15);
                }

                // (3). change to offset_tcp
                drCR7.WaitForReady();
                drCR7.Wait(100);
                drCR7.SetOffsetTCP();
                drCR7.WaitForReady();
                drCR7.Wait(100);

                // /// CR15
                // CR15_Motion(ref drCR15, ref tfCR15);

                // CR7 returns to via_position of part2.
                drCR7.SetOrigin();
                drCR7.MovetoOrigin();
                drCR7.WaitForReady();
                drCR7.Wait(100);
            }
        }
        
        private static void CR15_Motion(ref DualRobot dr, ref Transformation tf)
        {
            // check List Size
            var via_points_no = tf.via_points.Count;

            // MessageBox.Show("CR15: via_point number: " + via_points_no);

            bool res;
            short UF, UT;
            // loop
            dr.WaitForReady();

            for (int i = 0; i < via_points_no; i++)
            {
                // config the next via_point
                UF = UT = 1;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(tf.via_points[i][0].ToString());
                PosArray[1] = float.Parse(tf.via_points[i][1].ToString());
                PosArray[2] = float.Parse(tf.via_points[i][2].ToString());
                PosArray[3] = float.Parse(tf.via_points[i][3].ToString());
                PosArray[4] = float.Parse(tf.via_points[i][4].ToString());
                PosArray[5] = float.Parse(tf.via_points[i][5].ToString());                          

                res = dr.setRegisterPos(99, PosArray, dr.config, UF, UT);

                // Move
                dr.setRegister(1, 4, 1);
                dr.setRegister(2, 2, 1);

                if (i == 0)
                {
                    MessageBox.Show("CR15: Move!");
                }

                // if (i == via_points_no)
                // {
                //     MessageBox.Show("CR15 Arrived!");
                // }

                // Count
                dr.setRegister(3, i + 1, 1);
            }

            UF = UT = 1;
            float[] PosArray_default = new float[6];
            PosArray_default[0] = PosArray_default[1] = PosArray_default[2] = PosArray_default[3] = PosArray_default[4] = PosArray_default[5] = 0;

            res = dr.setRegisterPos(99, PosArray_default, dr.config, UF, UT);

            // Move
            dr.setRegister(1, 4, 1);
            dr.setRegister(2, 2, 1);

            dr.WaitForReady();
        }

        private void thread_Scene2A(object syncLock)
        {
            lock (syncLock)
            {
                Scene2A();
            }
        }

        private void Scene2A()
        {
            // CR7:  192.168.3.124
            // CR15: 192.168.3.125

            // CR7:  [15,90]
            // CR15: [-10,90]

            // 0. Initialization
            drCR7.setRegister(1, 0, 1);
            drCR7.setRegister(2, 0, 1);
            drCR7.setRegister(3, 0, 1);
            drCR15.setRegister(1, 0, 1);
            drCR15.setRegister(2, 0, 1);
            drCR15.setRegister(3, 0, 1);

            drCR7.setRegister(102, 0, 1);
            drCR15.setRegister(102, 0, 1);

            // 1. CR15 Path
            tfCR15.via_points = tfCR15.get_2A_motion_measurement_points(0, 100, 1);

            // 2. CR7 Path
            // check List Size
            tfCR7.via_points = tfCR7.get_2A_motion_points(0, 75, 15);
            var via_points_no = tfCR7.via_points.Count;

            // loop -- part 2
            for (int i = 0; i < via_points_no; i++)
            {
                // config the next via_point
                short UF = 1;
                short UT = 2;

                float[] PosArray_part2 = new float[6];
                short[] ConfigArray = new short[6];

                PosArray_part2[0] = float.Parse(tfCR7.via_points[i][0].ToString());
                PosArray_part2[1] = float.Parse(tfCR7.via_points[i][1].ToString());
                PosArray_part2[2] = float.Parse(tfCR7.via_points[i][2].ToString());
                PosArray_part2[3] = float.Parse(tfCR7.via_points[i][3].ToString());
                PosArray_part2[4] = float.Parse(tfCR7.via_points[i][4].ToString());
                PosArray_part2[5] = float.Parse(tfCR7.via_points[i][5].ToString());

                var res = drCR7.setRegisterPos(99, PosArray_part2, drCR7.config, UF, UT);

                // Move
                drCR7.setRegister(1, 4, 1);
                drCR7.setRegister(2, 2, 1);

                drCR7.WaitForReady();

                /// CR15
                CR15_Motion(ref drCR15, ref tfCR15);

                drCR15.WaitForReady();

            }

            // CR7 returns to via_position of part2.
            drCR7.SetOrigin();
            drCR7.MovetoOrigin();
            drCR7.WaitForReady();
        }

        private void ButtonStartScenario2A_OnClick(object sender, RoutedEventArgs e)
        {
            Init_scen1A();

            // move, 2 thread.Start()
            Thread th = new Thread(() => thread_Scene2A(syncLock_scen1A));
            th.Start();
            // cout the result.
        }
    }
}
