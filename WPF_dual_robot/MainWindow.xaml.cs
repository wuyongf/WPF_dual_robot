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
            TextBoxIpAddress1.Text = "127.0.0.1";
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
                tfCR15.uf_orbit_step_angle = param_step_angle;

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

            // cout the result.
        }

        /// <summary>
        /// Scenario A
        /// </summary>
        private readonly object syncLock_scen1A_CR7 = new object();
        private readonly object syncLock_scen1A_CR15 = new object();

        private int move_flag = 0;

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

            drCR15.setRegister(1, 0, 1);
            drCR15.setRegister(2, 0, 1);
            drCR15.setRegister(3, 0, 1);
            drCR15.setRegister(4, 0, 1);
            drCR15.setRegister(5, 0, 1);
            drCR15.setRegister(6, 0, 1);
            drCR15.setRegister(7, 0, 1);
        }

        private void thread_scen1A_CR7(object syncLock, ref DualRobot dr, ref Transformation tf)
        {
            lock (syncLock)
            {
                Scen1A_CR7();
            }
        }

        private void thread_scen1A_CR15(object syncLock, ref DualRobot dr, ref Transformation tf)
        {
            lock (syncLock)
            {
                Scen1A_CR15();
            }
        }

        // input: via_points
        private void Scen1A_CR7()
        {
            // // 1. Init
            // tfCR7.rb_orbit_radius;
            // tfCR7.rb_orbit_step_angle;
            // tfCR7.via_points;
            // tfCR7.status_orbit_points

            // 2. CR7: for loop. go through each point.
            for (int n = 1; n < tfCR7.uf_measure_points_no; n++)
            {
                // Check if everything is ok
                if (CheckRobotStatus())
                {
                    // a. wait for move_flag(1)
                    // b. check robot_status first.
                    while (move_flag != 1)
                    {
                        if (CheckRobotStatus())
                        {
                            // do nothing, keep checking.

                            // // TIME
                            // Thread.Sleep(500);
                        }
                        else
                        {
                            Console.WriteLine("[ScenarioA]: CR7_2:Something Wrong! Please Check!");
                            break;
                        }
                    }

                    if (move_flag == 1)
                    {
                        // b.1 update move_status(2).
                        tfCR7.status_orbit_points[n - 1][7] = 2;

                        // b.2 configure & move to next point
                        drCR7.move_uf(tfCR7.via_points[n - 1]);

                        // c. check status, update move_status(3 or 5). 


                        // d. based on move_status, set move_flag & measure_flag
                    }
                }
                else
                {
                    Console.WriteLine("[ScenarioA]: CR7_1:Something Wrong! Please Check!");
                    break;
                }
            }

            // 3. check status 
        }

        private void Scen1A_CR15()
        {
            // 1.1 Init - param
            var measure_radius = tfCR15.uf_measure_radius;
            var measure_arce = tfCR15.uf_measure_arc;
            var measure_step_angle = tfCR15.uf_measure_step_angle;

            // 1.2 Init - set move_flag
            move_flag = 1;

            // 2. CR15: for loop. go through each point.
            for (int n = 1; n < tfCR7.uf_measure_points_no; n++)
            {
                // Check if everything is ok
                if (CheckRobotStatus())
                {
                    // a. wait for move_flag(2)
                    // b. check robot_status first.

                    // b. move to next point, update move_status(2). 
                    // c. check status, update move_status(3/5). 
                    // d. based on move_status, set move_flag & measure_flag
                }
                else
                {
                    Console.WriteLine("[ScenarioA]: Something Wrong! Please Check!");
                    break;
                }
            }

            // 3. check status 
        }

        private bool CheckRobotStatus()
        {
            // 1. check CR7
            // 2. check CR15
            // 3. &&
            return false;
        }
    }
}
