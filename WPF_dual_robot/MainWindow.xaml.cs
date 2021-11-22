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
        public MainWindow()
        {
            InitializeComponent();

            InitParam();

            // subInit();

            // DualRobot dr1 = new DualRobot();
            // DualRobot dr2 = new DualRobot();
            //
            // dr1.PortNumber = 60008;
            // dr2.PortNumber = 9021;
            //
            // dr1.Init();
            // dr2.Init();
            //
            // dr1.setRegister(1, 1111, 1);
            // dr2.setRegister(1, 2222, 1);
            //
            // dr1.getCurPos();
            // MessageBox.Show("dr1 current position: " + dr1.strCurPosX + ", " + dr1.strCurPosY + ", " + dr1.strCurPosZ);
            //
            // dr2.getCurPos();
            // MessageBox.Show("dr2 current position: " + dr2.strCurPosX + ", " + dr2.strCurPosY + ", " + dr2.strCurPosZ);
        }




        // private void subInit()
        // {
        //     bool blnRes = false;
        //     string strHost = null;
        //     int lngTmp = 0;
        //     
        //
        //     try
        //     {
        //         mobjCore = new FRRJIf.Core();
        //
        //         // You need to set data table before connecting.
        //         mobjDataTable = mobjCore.DataTable;
        //
        //         {
        //             mobjAlarm = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_LIST, 5, 0);
        //             mobjAlarmCurrent = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_CURRENT, 1, 0);
        //             mobjCurPos = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1);
        //             mobjCurPosUF = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15);
        //             mobjCurPos2 = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 2);
        //             mobjTask = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK, 1);
        //             mobjTaskIgnoreMacro = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO, 1);
        //             mobjTaskIgnoreKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_KAREL, 1);
        //             mobjTaskIgnoreMacroKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO_KAREL, 1);
        //             mobjPosReg = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 10);
        //             mobjPosReg2 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 2, 1, 4);
        //             mobjSysVarInt = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$FAST_CLOCK");
        //             mobjSysVarInt2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");
        //             mobjSysVarReal = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$MOR_GRP[1].$CURRENT_ANG[1]");
        //             mobjSysVarReal2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$DUTY_TEMP");
        //             mobjSysVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$TIMER[10].$COMMENT");
        //             mobjSysVarPos = mobjDataTable.AddSysVarPos(FRRJIf.FRIF_DATA_TYPE.SYSVAR_POS, "$MNUTOOL[1,1]");
        //             mobjVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$[HTTPKCL]CMDS[1]");
        //             mobjNumReg = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 200);
        //             mobjNumReg2 = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 6, 10);
        //             mobjPosRegXyzwpr = mobjDataTable.AddPosRegXyzwpr(FRRJIf.FRIF_DATA_TYPE.POSREG_XYZWPR, 1, 1, 10);
        //             mobjStrReg = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG, 1, 3);
        //             mobjStrRegComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG_COMMENT, 1, 3);
        //         }
        //
        //         // 2nd data table.
        //         // You must not set the first data table.
        //         mobjDataTable2 = mobjCore.DataTable2;
        //         mobjNumReg3 = mobjDataTable2.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5);
        //         mobjSysVarIntArray = new FRRJIf.DataSysVar[10];
        //         mobjSysVarIntArray[0] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[1].$TIMER_VAL");
        //         mobjSysVarIntArray[1] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[2].$TIMER_VAL");
        //         mobjSysVarIntArray[2] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[3].$TIMER_VAL");
        //         mobjSysVarIntArray[3] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[4].$TIMER_VAL");
        //         mobjSysVarIntArray[4] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[5].$TIMER_VAL");
        //         mobjSysVarIntArray[5] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[6].$TIMER_VAL");
        //         mobjSysVarIntArray[6] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[7].$TIMER_VAL");
        //         mobjSysVarIntArray[7] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[8].$TIMER_VAL");
        //         mobjSysVarIntArray[8] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[9].$TIMER_VAL");
        //         mobjSysVarIntArray[9] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");
        //
        //         //get host name
        //         strHost = HostName;
        //
        //         //get time out value
        //         lngTmp = 10000;
        //
        //         //set port number
        //         mobjCore.PortNumber = intPortNum;
        //
        //         //connect
        //         if (lngTmp > 0)
        //             mobjCore.TimeOutValue = lngTmp;
        //         blnRes = mobjCore.Connect(strHost);
        //
        //         getXYZ_Click();
        //
        //         MessageBox.Show("current port number: " + mobjCore.PortNumber);
        //     }
        //     catch (Exception ex)
        //     {
        //         MessageBox.Show(ex.Message);
        //         System.Environment.Exit(0);
        //     }
        // }
        //
        // private void getXYZ_Click()
        // {
        //     string strTmp = null;
        //
        //     short intUF = 0;
        //     short intUT = 0;
        //     short intValidC = 0;
        //     short intValidJ = 0;
        //     bool blnDT = false;
        //     bool blnSDO = false;
        //     bool blnSDI = false;
        //
        //     //check
        //     if (mobjCore == null)
        //     {
        //         return;
        //     }
        //
        //     //Refresh data table
        //     blnDT = mobjDataTable.Refresh();
        //     if (blnDT == false)
        //     {
        //         return;
        //     }
        //
        //     //read SDO
        //     blnSDO = mobjCore.ReadSDO(1, ref intSDO, 100);
        //     if (blnSDO == false)
        //     {
        //         return;
        //     }
        //     //read SDI
        //     blnSDI = mobjCore.ReadSDI(1, ref intSDI, 10);
        //     if (blnSDI == false)
        //     {
        //         return;
        //     }
        //
        //     {
        //         if (mobjCurPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
        //         {
        //             strTmp = strTmp + "--- CurPos GP1 World ---\r\n";
        //             strTmp = strTmp + mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);
        //         }
        //         else
        //         {
        //             strTmp = strTmp + "CurPos Error!!!\r\n";
        //         }
        //     }
        //
        //     var pos_x = xyzwpr.GetValue(0).ToString();
        //     var pos_y = xyzwpr.GetValue(1).ToString();
        //     var pos_z = xyzwpr.GetValue(2).ToString();
        //
        //
        //     MessageBox.Show("POS(x,y,z): " + pos_x + ", " + pos_y + ", " + pos_z);
        //
        //     // Read the Registers
        //     var resReadReg = mobjNumReg.GetValue(2, ref value);
        //
        //     // Write the Registers
        //     var resWriteReg = mobjNumReg.SetValuesInt(1, indexRegister, 1);
        //
        //     // Write the RP
        //     var resWriteRegRos = setRegisterPos();
        //
        // }
        //
        // private string mstrPos(ref Array xyzwpr, ref Array config, ref Array joint, short intValidC, short intValidJ, int UF, int UT)
        // {
        //     string tmp = "";
        //     int ii = 0;
        //
        //     tmp = tmp + "UF = " + UF + ", ";
        //     tmp = tmp + "UT = " + UT + "\r\n";
        //     if (intValidC != 0)
        //     {
        //         tmp = tmp + "XYZWPR = ";
        //         //5
        //         for (ii = 0; ii <= 8; ii++)
        //         {
        //             tmp = tmp + xyzwpr.GetValue(ii) + " ";
        //         }
        //
        //         tmp = tmp + "\r\n" + "CONFIG = ";
        //         if ((short)config.GetValue(0) != 0)
        //         {
        //             tmp = tmp + "F ";
        //         }
        //         else
        //         {
        //             tmp = tmp + "N ";
        //         }
        //         if ((short)config.GetValue(1) != 0)
        //         {
        //             tmp = tmp + "L ";
        //         }
        //         else
        //         {
        //             tmp = tmp + "R ";
        //         }
        //         if ((short)config.GetValue(2) != 0)
        //         {
        //             tmp = tmp + "U ";
        //         }
        //         else
        //         {
        //             tmp = tmp + "D ";
        //         }
        //         if ((short)config.GetValue(3) != 0)
        //         {
        //             tmp = tmp + "T ";
        //         }
        //         else
        //         {
        //             tmp = tmp + "B ";
        //         }
        //         tmp = tmp + String.Format("{0}, {1}, {2}\r\n", config.GetValue(4), config.GetValue(5), config.GetValue(6));
        //     }
        //
        //     if (intValidJ != 0)
        //     {
        //         tmp = tmp + "JOINT = ";
        //         //5
        //         for (ii = 0; ii <= 8; ii++)
        //         {
        //             tmp = tmp + joint.GetValue(ii) + " ";
        //         }
        //         tmp = tmp + "\r\n";
        //     }
        //
        //     return tmp;
        //
        // }
        //
        // private bool setRegisterPos()
        // {
        //     short intUF = 0;
        //     short intUT = 0;
        //
        //     xyzwpr.SetValue(-625.408f, 0);
        //     xyzwpr.SetValue(-743.895, 1);
        //     xyzwpr.SetValue(45.834, 2);
        //     xyzwpr.SetValue(5.082, 3);
        //     xyzwpr.SetValue(-4.774, 4);
        //     xyzwpr.SetValue(-153.740, 5);
        //
        //     return mobjPosReg.SetValueXyzwpr(100, ref xyzwpr, ref config, intUF, intUT);
        // }

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
                WindowFrameDualRobot windowFrameDualRobot = new WindowFrameDualRobot(strRobotModel, strIPAddress, PortNumber);
                windowFrameDualRobot.Show();
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
                WindowFrameDualRobot windowFrameDualRobot = new WindowFrameDualRobot(strRobotModel, strIPAddress, PortNumber);
                windowFrameDualRobot.Show();
            }
            else
            {
                MessageBox.Show("Port: Please Input Integer Number!");
            }
        }
    }
}
