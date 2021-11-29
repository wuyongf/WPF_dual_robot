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
        private DualRobot dr = new DualRobot();

        public WindowFrameDualRobot(ref DualRobot dr_)
        {
            InitializeComponent();

            // Init

            dr = dr_;

            // Init -  UI
            TextBlockRobotModel.Text = dr.Model;

            // Show the Init Page.
            FrameDualRobot.Content = new PageDRInitSetup(ref dr);
        }

        private void ButtonPageDRInitSetup_OnClick(object sender, RoutedEventArgs e)
        {
            FrameDualRobot.Content = new PageDRInitSetup(ref dr);
        }

        private void ButtonPageDRRobotBaseMotion_OnClick(object sender, RoutedEventArgs e)
        {
            FrameDualRobot.Content = new PageDRRobotBaseMotion(ref dr);
        }

        private void ButtonPageDRUserFrameMotion_OnClick(object sender, RoutedEventArgs e)
        {
            FrameDualRobot.Content = new PageDRUserFrameMotionSetup(ref dr);
        }
    }
}
