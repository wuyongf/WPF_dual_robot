using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_dual_robot
{
    public class DualRobot
    {
        public string HostName = "127.0.0.1";
        public int PortNumber = 9021;

        public string Model;

        public Random rnd = new Random();

        public FRRJIf.Core mobjCore;
        public FRRJIf.DataTable mobjDataTable;
        public FRRJIf.DataTable mobjDataTable2;
        public FRRJIf.DataCurPos mobjCurPos;
        public FRRJIf.DataCurPos mobjCurPosUF;
        public FRRJIf.DataCurPos mobjCurPos2;
        public FRRJIf.DataTask mobjTask;
        public FRRJIf.DataTask mobjTaskIgnoreMacro;
        public FRRJIf.DataTask mobjTaskIgnoreKarel;
        public FRRJIf.DataTask mobjTaskIgnoreMacroKarel;
        public FRRJIf.DataPosReg mobjPosReg;
        public FRRJIf.DataPosReg mobjPosReg2;
        public FRRJIf.DataPosRegXyzwpr mobjPosRegXyzwpr;
        public FRRJIf.DataSysVar mobjSysVarInt;
        public FRRJIf.DataSysVar mobjSysVarInt2;
        public FRRJIf.DataSysVar mobjSysVarReal;
        public FRRJIf.DataSysVar mobjSysVarReal2;
        public FRRJIf.DataSysVar mobjSysVarString;
        public FRRJIf.DataSysVarPos mobjSysVarPos;
        public FRRJIf.DataSysVar[] mobjSysVarIntArray;
        public FRRJIf.DataNumReg mobjNumReg;
        public FRRJIf.DataNumReg mobjNumReg2;
        public FRRJIf.DataNumReg mobjNumReg3;
        public FRRJIf.DataAlarm mobjAlarm;
        public FRRJIf.DataAlarm mobjAlarmCurrent;
        public FRRJIf.DataSysVar mobjVarString;
        public FRRJIf.DataString mobjStrReg;
        public FRRJIf.DataString mobjStrRegComment;

        public Array xyzwpr = new float[9];
        public Array config = new short[7];
        public Array joint = new float[9];
        public Array intSDO = new short[100];
        public Array intSDI = new short[10];

        public string strCurPosX;
        public string strCurPosY;
        public string strCurPosZ;
        public string strCurPosW;
        public string strCurPosP;
        public string strCurPosR;

        // Connection
        public bool Init()
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
                mobjCore.PortNumber = PortNumber;

                //connect
                if (lngTmp > 0)
                    mobjCore.TimeOutValue = lngTmp;
                blnRes = mobjCore.Connect(strHost);

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                System.Environment.Exit(0);

                return false;
            }
        }

        // get the Registers
        public bool getRegisterInt(int index, ref object value)
        {
            return mobjNumReg.GetValue(index, ref value);
        }

        // set the Registers R[1] = 1; R[2] = 2
        public bool setRegister(int index, int value, int count)
        {
            return mobjNumReg.SetValuesInt(index, value, count);
        }

        // set the Register Pos
        public bool setRegisterPos(int index, float[] posArray, short[] configArray, short UF, short UT)
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

            return mobjPosReg.SetValueXyzwpr(index, ref xyzwpr, ref config, intUF, intUT);
        }

        public bool getCurPos()
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
                strTmp = strTmp + this.mstrPos(ref xyzwpr, ref config, ref joint, intValidC, intValidJ, intUF, intUT);

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

        private string mstrPos(ref Array xyzwpr, ref Array config, ref Array joint, short intValidC, short intValidJ, int UF, int UT)
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
