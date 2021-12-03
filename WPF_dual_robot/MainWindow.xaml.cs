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
            Thread th = new Thread(() => thread_scen1A(syncLock_scen1A));
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

            drCR15.setRegister(1, 0, 1);
            drCR15.setRegister(2, 0, 1);
            drCR15.setRegister(3, 0, 1);
            drCR15.setRegister(4, 0, 1);
            drCR15.setRegister(5, 0, 1);
            drCR15.setRegister(6, 0, 1);
            drCR15.setRegister(7, 0, 1);
        }


        private void thread_scen1A(object syncLock)
        {
            lock (syncLock)
            {
                Scen1A();
            }
        }

        // input: via_points


        private void Scen1A()
        {
            // tfCR7.rb_orbit_radius;
            // tfCR7.rb_orbit_step_angle;
            // tfCR7.via_points;
            // tfCR7.status_orbit_points

            // 1. get all orbit points.
            
            tfCR7.uf_orbit_points = tfCR7.get_uf_orbit_points(tfCR7.uf_orbit_radius, tfCR7.uf_orbit_step_angle);

            tfCR7.uf_orbit_points_no = tfCR7.uf_orbit_points.Count;

            tfCR15.uf_measure_points.Clear();

            for (int i = 0; i < tfCR7.uf_orbit_points_no; i++)
            {
                var sub_measure_points = tfCR15.get_uf_measure_points(tfCR7.uf_orbit_points[i],tfCR15.uf_measure_radius, 
                                            tfCR15.uf_measure_arc, tfCR15.uf_measure_step_angle);

                tfCR15.uf_measure_points.Add(sub_measure_points);
            }


            // 2. CR7: for loop. go through each point.
            for (int n = 0; n < tfCR7.uf_orbit_points_no; n++)
            {
                // move to next orbit point.
                drCR7.move_uf(tfCR7.uf_orbit_points[n]);

                // drCR15.move_uf(tfCR15.uf_measure_points[n][0]);

                // MessageBox.Show("cr15 measure_point[" + n + "]: " + tfCR15.uf_measure_points[n][0][1].ToString());

                for (int m = 0; m < tfCR15.uf_measure_points[n].Count; m++)
                {
                    // // wait for move_flag
                    // while (move_flag != 1)
                    // {
                    //     if (CheckRobotStatus())
                    //     {
                    //         // do nothing, keep checking.
                    //         Console.WriteLine("[Scenario1A]: Keep Checking!");
                    //     }
                    //     else
                    //     {
                    //         Console.WriteLine("[Scenario1A]: Something Wrong! Please Check!");
                    //         break;
                    //     }
                    // }
                
                    // move to next measure point
                    drCR15.move_uf(tfCR15.uf_measure_points[n][m]);
                
                    // reset move_flag
                    // move_flag = 0;
                }
            }
        }

        private bool CheckRobotStatus()
        {
            // 1. check CR7
            // 2. check CR15
            // 3. &&
            return true;
        }
    }
}
