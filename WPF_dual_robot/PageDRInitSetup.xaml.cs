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

namespace WPF_dual_robot
{
    /// <summary>
    /// Interaction logic for PageDRInitSetup.xaml
    /// </summary>
    public partial class PageDRInitSetup : Page
    {
        public PageDRInitSetup()
        {
            InitializeComponent();
        }

        private void ButtonApplySimPortNumber_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCardDualRobotSimPortNumber = TextBoxCardDualRobotSimPortNumber.Text;

            int PortNumber;

            bool isNumeric = int.TryParse(strTextBoxCardDualRobotSimPortNumber, out PortNumber);

            if (isNumeric)
            {
                Globals.intPortNum = PortNumber;

                // Notice User
                CardDualRobotSimPort.Background = Brushes.Cornsilk;
                CardDualRobotSimPort.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Integer Number!");
            }
        }


        private void ButtonConnect_OnClick(object sender, RoutedEventArgs e)
        {
            Globals.subInit();
        }
    }
}
