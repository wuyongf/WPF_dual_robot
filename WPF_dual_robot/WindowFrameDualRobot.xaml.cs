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
using System.Windows.Shapes;

namespace WPF_dual_robot
{
    /// <summary>
    /// Interaction logic for WindowFrameDualRobot.xaml
    /// </summary>
    public partial class WindowFrameDualRobot : Window
    {
        private string strRobotModel;
        private string strIPAddress;
        private int intPort;

        public WindowFrameDualRobot(string RobotModel, string IPAddress,int Port)
        {
            InitializeComponent();

            // Init
            strRobotModel = RobotModel;
            strIPAddress = IPAddress;
            intPort = Port;

            // Init -  UI
            TextBlockRobotModel.Text = strRobotModel;

            // Motion Page
            FrameDualRobot.Content = new PageDRMotionSetup(strRobotModel, strIPAddress, intPort);
        }

        private void ButtonPageDRMotionSetup_OnClick(object sender, RoutedEventArgs e)
        {
            FrameDualRobot.Content = new PageDRMotionSetup(strRobotModel, strIPAddress, intPort);
        }

        private void ButtonPageDRInitSetup_OnClick(object sender, RoutedEventArgs e)
        {
            //todo: need to modify.
            FrameDualRobot.Content = new PageDRInitSetup();
        }
    }
}
