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
        private DualRobot dr = new DualRobot();

        public PageDRInitSetup(ref DualRobot dr_)
        {
            InitializeComponent();

            dr = dr_;

            // Init Param
            InitParam();
        }

        private void InitParam()
        {
            TextBoxTCPX.Text = 0.ToString();
            TextBoxTCPY.Text = 0.ToString();
            TextBoxTCPZ.Text = 415.ToString();
            TextBoxTCPW.Text = (-180).ToString();
            TextBoxTCPP.Text = 0.ToString();
            TextBoxTCPR.Text = 0.ToString();
        }

        private void ButtonResetMoveRegister_OnClick(object sender, RoutedEventArgs e)
        {
            dr.setRegister(1, 0, 1);
            dr.setRegister(2, 0, 1);
        }

        private void ButtonUpdateTCP_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. Retrieve data from Tool Frame
            if (dr.getRegisterPos(97))
            {
                // 2. show user
                TextBoxTCPX.Text = dr.RpParamXyzwpr.GetValue(0).ToString(); 
                TextBoxTCPY.Text = dr.RpParamXyzwpr.GetValue(1).ToString();
                TextBoxTCPZ.Text = dr.RpParamXyzwpr.GetValue(2).ToString();
                TextBoxTCPW.Text = dr.RpParamXyzwpr.GetValue(3).ToString();
                TextBoxTCPP.Text = dr.RpParamXyzwpr.GetValue(4).ToString();
                TextBoxTCPR.Text = dr.RpParamXyzwpr.GetValue(5).ToString();
            }
        }

        private void ButtonResetTCP_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxTCPX.Clear();
            TextBoxTCPY.Clear();
            TextBoxTCPZ.Clear();
            TextBoxTCPW.Clear();
            TextBoxTCPP.Clear();
            TextBoxTCPR.Clear();
        }

        private void ButtonApplyTCP_OnClick(object sender, RoutedEventArgs e)
        {
            // 1. assign offset_tcp
            if (AssignOffsetTCP())
            {
                // 1. station machine: set tool frame
                dr.setRegister(1, 5, 1);
                dr.setRegister(2, 2, 1);
            }
            else
            {
                MessageBox.Show("Cannot Apply TCP! Please Check!");
            }
        }

        private bool AssignOffsetTCP()
        {
            short UF = 0;
            short UT = 1;

            float[] PosArray = new float[6];

            var strTextBoxRegPosX = TextBoxTCPX.Text;
            var strTextBoxRegPosY = TextBoxTCPY.Text;
            var strTextBoxRegPosZ = TextBoxTCPZ.Text;
            var strTextBoxRegPosW = TextBoxTCPW.Text;
            var strTextBoxRegPosP = TextBoxTCPP.Text;
            var strTextBoxRegPosR = TextBoxTCPR.Text;

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

                return false;
            }

            var res = dr.setRegisterPos(97, PosArray, dr.config, UF, UT);

            return res;
        }
    }
}
