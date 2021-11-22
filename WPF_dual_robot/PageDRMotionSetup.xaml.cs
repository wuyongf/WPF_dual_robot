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

namespace WPF_dual_robot
{
    /// <summary>
    /// Interaction logic for PageDRMotionSetup.xaml
    /// </summary>
    public partial class PageDRMotionSetup : Page
    {
        public PageDRMotionSetup()
        {
            InitializeComponent();
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

            var res = Globals.setRegisterPos(100, PosArray, ConfigArray, UF, UT);
        }

        private void ButtonMove_OnClick(object sender, RoutedEventArgs e)
        {
            Globals.setRegister(1, 1, 1);
            Globals.setRegister(2, 2, 1);
        }

        private void ButtonResetMoveRegister_OnClick(object sender, RoutedEventArgs e)
        {
            Globals.setRegister(1, 0, 1);
            Globals.setRegister(2, 0, 1);
        }

        private void ButtonUpdateCurPos_OnClick(object sender, RoutedEventArgs e)
        {
            var res = Globals.getCurPos();

            if (res)
            {
                // info the user
                TextBoxRegPosX.Text = Globals.strCurPosX;
                TextBoxRegPosY.Text = Globals.strCurPosY;
                TextBoxRegPosZ.Text = Globals.strCurPosZ;
                TextBoxRegPosW.Text = Globals.strCurPosW;
                TextBoxRegPosP.Text = Globals.strCurPosP;
                TextBoxRegPosR.Text = Globals.strCurPosR;

                // calculate the rotation matrix
                Transformation.RMat = Transformation.rpy2R(double.Parse(Globals.strCurPosW),
                    double.Parse(Globals.strCurPosP), double.Parse(Globals.strCurPosR));

                // assign the center point
                Transformation.center_point[0] = double.Parse(Globals.strCurPosX);
                Transformation.center_point[1] = double.Parse(Globals.strCurPosY);
                Transformation.center_point[2] = double.Parse(Globals.strCurPosZ);
                Transformation.center_point[3] = double.Parse(Globals.strCurPosW);
                Transformation.center_point[4] = double.Parse(Globals.strCurPosP);
                Transformation.center_point[5] = double.Parse(Globals.strCurPosR);
            }
        }


        private void ButtonApplyCircleRadius_OnClick(object sender, RoutedEventArgs e)
        {
            var strTextBoxCircleRadius = TextBoxCircleRadius.Text;

            double radius;

            bool isNumeric = double.TryParse(strTextBoxCircleRadius, out radius);

            if (isNumeric)
            {
                // assign the radius
                Transformation.radius = radius;

                // Notice User
                CardCircleMotion.Background = Brushes.Cornsilk;
                CardCircleMotion.Foreground = Brushes.Black;
            }
            else
            {
                MessageBox.Show("Please Input Float Number!");
            }
        }

        private void ButtonCircleMotion_OnClick(object sender, RoutedEventArgs e)
        {
            Transformation.get_via_points(Transformation.center_point, Transformation.RMat,Transformation.radius, 0.1 );

            // check List Size
            var via_points_no = Transformation.via_points.Count;

            object Register1 = 0;
            object Register2 = 0;

            // loop
            for (int i = 0; i < via_points_no; i++)
            {
                // // wait for R[1], R[2] = 0
                var res1 = Globals.getRegisterInt(1, ref Register1);
                var res2 = Globals.getRegisterInt(2, ref Register1);

                while (Register1.ToString() != 0.ToString() && Register2.ToString() != 0.ToString())
                {
                    res1 = Globals.getRegisterInt(1, ref Register1);
                    res2 = Globals.getRegisterInt(2, ref Register1);
                }

                // config the next via_point
                short UF = 0;
                short UT = 0;

                float[] PosArray = new float[6];
                short[] ConfigArray = new short[6];

                PosArray[0] = float.Parse(Transformation.via_points[i][0].ToString());
                PosArray[1] = float.Parse(Transformation.via_points[i][1].ToString());
                PosArray[2] = float.Parse(Transformation.via_points[i][2].ToString());
                PosArray[3] = float.Parse(Transformation.via_points[i][3].ToString());
                PosArray[4] = float.Parse(Transformation.via_points[i][4].ToString());
                PosArray[5] = float.Parse(Transformation.via_points[i][5].ToString());

                ConfigArray[0] = 0;
                ConfigArray[1] = 0;
                ConfigArray[2] = 1;
                ConfigArray[3] = 1;
                ConfigArray[4] = 0;
                ConfigArray[5] = 0;


                var res = Globals.setRegisterPos(100, PosArray, ConfigArray, UF, UT);


                // Move
                Globals.setRegister(1, 1, 1);
                Globals.setRegister(2, 2, 1);

            }
            

            // move

            
        }
    }
}
