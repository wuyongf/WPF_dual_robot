using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FRRJIf;
using FRRobotIFLib;

namespace WPF_dual_robot
{
    public class Globals
    {
        public static string HostName = "127.0.0.1";

        public static int intPortNum = 9021;

        public static int intPortNumDR1 = 9021;
        public static int intPortNumDR2 = 9021;

        public const string cnstApp = "frrjiftest";
        public const string cnstSection = "setting";

        public Random rnd = new Random();
        
        public static FRRJIf.Core mobjCore;
        public static FRRJIf.DataTable mobjDataTable;
        public static FRRJIf.DataTable mobjDataTable2;
        public static FRRJIf.DataCurPos mobjCurPos;
        public static FRRJIf.DataCurPos mobjCurPosUF;
        public static FRRJIf.DataCurPos mobjCurPos2;
        public static FRRJIf.DataTask mobjTask;
        public static FRRJIf.DataTask mobjTaskIgnoreMacro;
        public static FRRJIf.DataTask mobjTaskIgnoreKarel;
        public static FRRJIf.DataTask mobjTaskIgnoreMacroKarel;
        public static FRRJIf.DataPosReg mobjPosReg;
        public static FRRJIf.DataPosReg mobjPosReg2;
        public static FRRJIf.DataPosRegXyzwpr mobjPosRegXyzwpr;
        public static FRRJIf.DataSysVar mobjSysVarInt;
        public static FRRJIf.DataSysVar mobjSysVarInt2;
        public static FRRJIf.DataSysVar mobjSysVarReal;
        public static FRRJIf.DataSysVar mobjSysVarReal2;
        public static FRRJIf.DataSysVar mobjSysVarString;
        public static FRRJIf.DataSysVarPos mobjSysVarPos;
        public static FRRJIf.DataSysVar[] mobjSysVarIntArray;
        public static FRRJIf.DataNumReg mobjNumReg;
        public static FRRJIf.DataNumReg mobjNumReg2;
        public static FRRJIf.DataNumReg mobjNumReg3;
        public static FRRJIf.DataAlarm mobjAlarm;
        public static FRRJIf.DataAlarm mobjAlarmCurrent;
        public static FRRJIf.DataSysVar mobjVarString;
        public static FRRJIf.DataString mobjStrReg;
        public static FRRJIf.DataString mobjStrRegComment;

        public static Array xyzwpr = new float[9];
        public static Array config = new short[7];
        public static Array joint = new float[9];
        public static Array intSDO = new short[100];
        public static Array intSDI = new short[10];

        public object value;

        public int indexRegister = 8;

        public float floatRegister = 8.88f;

        public static string strCurPosX;
        public static string strCurPosY;
        public static string strCurPosZ;
        public static string strCurPosW;
        public static string strCurPosP;
        public static string strCurPosR;

        // Connection
        public static void subInit()
        {
            bool blnRes = false;
            string strHost = null;
            int lngTmp = 0;

            try
            {
                mobjCore = new FRRJIf.Core();

                // You need to set data table before connecting.
                mobjDataTable = mobjCore.DataTable;

                {
                    mobjAlarm = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_LIST, 5, 0);
                    mobjAlarmCurrent = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_CURRENT, 1, 0);
                    mobjCurPos = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1);
                    mobjCurPosUF = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15);
                    mobjCurPos2 = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 2);
                    mobjTask = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK, 1);
                    mobjTaskIgnoreMacro = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO, 1);
                    mobjTaskIgnoreKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_KAREL, 1);
                    mobjTaskIgnoreMacroKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO_KAREL, 1);
                    mobjPosReg = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 100);
                    mobjPosReg2 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 2, 1, 4);
                    mobjSysVarInt = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$FAST_CLOCK");
                    mobjSysVarInt2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");
                    mobjSysVarReal = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$MOR_GRP[1].$CURRENT_ANG[1]");
                    mobjSysVarReal2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$DUTY_TEMP");
                    mobjSysVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$TIMER[10].$COMMENT");
                    mobjSysVarPos = mobjDataTable.AddSysVarPos(FRRJIf.FRIF_DATA_TYPE.SYSVAR_POS, "$MNUTOOL[1,1]");
                    mobjVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$[HTTPKCL]CMDS[1]");
                    mobjNumReg = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 200);
                    mobjNumReg2 = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 6, 10);
                    mobjPosRegXyzwpr = mobjDataTable.AddPosRegXyzwpr(FRRJIf.FRIF_DATA_TYPE.POSREG_XYZWPR, 1, 1, 10);
                    mobjStrReg = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG, 1, 3);
                    mobjStrRegComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG_COMMENT, 1, 3);
                }

                // 2nd data table.
                // You must not set the first data table.
                mobjDataTable2 = mobjCore.DataTable2;
                mobjNumReg3 = mobjDataTable2.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5);
                mobjSysVarIntArray = new FRRJIf.DataSysVar[10];
                mobjSysVarIntArray[0] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[1].$TIMER_VAL");
                mobjSysVarIntArray[1] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[2].$TIMER_VAL");
                mobjSysVarIntArray[2] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[3].$TIMER_VAL");
                mobjSysVarIntArray[3] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[4].$TIMER_VAL");
                mobjSysVarIntArray[4] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[5].$TIMER_VAL");
                mobjSysVarIntArray[5] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[6].$TIMER_VAL");
                mobjSysVarIntArray[6] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[7].$TIMER_VAL");
                mobjSysVarIntArray[7] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[8].$TIMER_VAL");
                mobjSysVarIntArray[8] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[9].$TIMER_VAL");
                mobjSysVarIntArray[9] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");

                //get host name
                strHost = HostName;

                //get time out value
                lngTmp = 10000;

                //set port number
                mobjCore.PortNumber = intPortNum;

                //connect
                if (lngTmp > 0)
                    mobjCore.TimeOutValue = lngTmp;
                blnRes = mobjCore.Connect(strHost);

                MessageBox.Show("IsConnected?: " + blnRes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0);
            }
        }

        // get the Registers
        public static bool getRegisterInt(int index, ref object value)
        {
            return mobjNumReg.GetValue(index, ref value);
        }

        // set the Registers R[1] = 1; R[2] = 2
        public static bool setRegister(int index, int value, int count)
        {
            return mobjNumReg.SetValuesInt(index, value, count);
        }


        // set the Register Pos
        public static bool setRegisterPos(int index, float[] posArray, short[] configArray, short UF, short UT)
        {
            short intUF = UF;
            short intUT = UT;

            xyzwpr.SetValue(posArray[0], 0);
            xyzwpr.SetValue(posArray[1], 1);
            xyzwpr.SetValue(posArray[2], 2);
            xyzwpr.SetValue(posArray[3], 3);
            xyzwpr.SetValue(posArray[4], 4);
            xyzwpr.SetValue(posArray[5], 5);

            config.SetValue(configArray[0], 0);
            config.SetValue(configArray[1], 1);
            config.SetValue(configArray[2], 2);
            config.SetValue(configArray[3], 3);
            config.SetValue(configArray[4], 4);
            config.SetValue(configArray[5], 5);

            return mobjPosReg.SetValueXyzwpr(index,ref xyzwpr, ref config, intUF, intUT);
        }

        

        public static bool getCurPos()
        {
            string strTmp = null;

            short intUF = 0;
            short intUT = 0;
            short intValidC = 0;
            short intValidJ = 0;
            bool blnDT = false;

            //Refresh data table
            blnDT = mobjDataTable.Refresh();

            if (mobjCurPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
            {
                strTmp = strTmp + "--- CurPos GP1 World ---\r\n";
                strTmp = strTmp + Globals.mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);

                strCurPosX = xyzwpr.GetValue(0).ToString();
                strCurPosY = xyzwpr.GetValue(1).ToString();
                strCurPosZ = xyzwpr.GetValue(2).ToString();
                strCurPosW = xyzwpr.GetValue(3).ToString();
                strCurPosP = xyzwpr.GetValue(4).ToString();
                strCurPosR = xyzwpr.GetValue(5).ToString();

                return true;
            }
            else
            {
                strTmp = strTmp + "CurPos Error!!!\r\n";

                return false;
            }
        }

        private static string mstrPos(ref Array xyzwpr, ref Array config, ref Array joint, short intValidC, short intValidJ, int UF, int UT)
        {
            string tmp = "";
            int ii = 0;

            tmp = tmp + "UF = " + UF + ", ";
            tmp = tmp + "UT = " + UT + "\r\n";
            if (intValidC != 0)
            {
                tmp = tmp + "XYZWPR = ";
                //5
                for (ii = 0; ii <= 8; ii++)
                {
                    tmp = tmp + xyzwpr.GetValue(ii) + " ";
                }

                tmp = tmp + "\r\n" + "CONFIG = ";
                if ((short)config.GetValue(0) != 0)
                {
                    tmp = tmp + "F ";
                }
                else
                {
                    tmp = tmp + "N ";
                }
                if ((short)config.GetValue(1) != 0)
                {
                    tmp = tmp + "L ";
                }
                else
                {
                    tmp = tmp + "R ";
                }
                if ((short)config.GetValue(2) != 0)
                {
                    tmp = tmp + "U ";
                }
                else
                {
                    tmp = tmp + "D ";
                }
                if ((short)config.GetValue(3) != 0)
                {
                    tmp = tmp + "T ";
                }
                else
                {
                    tmp = tmp + "B ";
                }
                tmp = tmp + String.Format("{0}, {1}, {2}\r\n", config.GetValue(4), config.GetValue(5), config.GetValue(6));
            }

            if (intValidJ != 0)
            {
                tmp = tmp + "JOINT = ";
                //5
                for (ii = 0; ii <= 8; ii++)
                {
                    tmp = tmp + joint.GetValue(ii) + " ";
                }
                tmp = tmp + "\r\n";
            }

            return tmp;

        }

    }
}
