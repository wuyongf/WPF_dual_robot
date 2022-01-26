using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MathNet.Numerics.LinearAlgebra;

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

        public string strRBCurPosX;
        public string strRBCurPosY;
        public string strRBCurPosZ;
        public string strRBCurPosW;
        public string strRBCurPosP;
        public string strRBCurPosR;

        public string strUFCurPosX;
        public string strUFCurPosY;
        public string strUFCurPosZ;
        public string strUFCurPosW;
        public string strUFCurPosP;
        public string strUFCurPosR;

        //{0,0,0,0,0,0}
        public float[] arrayUFOrigin = new float[6];

        /// register pos
        /// 1. get method
        public Array RpParamXyzwpr = new float[9];
        public Array RpParamConfig = new short[7];
        public Array RpParamJoint = new float[9];
        public short RpParamUF, RpParamUT, RpParamValidC, RpParamValidJ;

        /// 2. offset_tcp
        public float[] offset_tcp = new float[6];

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
            //Refresh data table
            mobjDataTable.Refresh();

            return mobjNumReg.GetValue(index, ref value);
        }

        // set the Registers R[1] = 1; R[2] = 2
        public bool setRegister(int index, int value, int count)
        {
            return mobjNumReg.SetValuesInt(index, value, count);
        }

        // get the Register Pos
        public bool getRegisterPos(int index)
        {
            //Refresh data table
            mobjDataTable.Refresh();

            return mobjPosReg.GetValue(index, ref RpParamXyzwpr, ref RpParamConfig, ref RpParamJoint, ref RpParamUF, ref RpParamUT, ref RpParamValidC, ref RpParamValidJ);
        }

        // set the Register Pos
        public bool setRegisterPos(int index, float[] posArray, System.Array configArray, short UF, short UT)
        {
            short intUF = UF;
            short intUT = UT;

            xyzwpr.SetValue(posArray[0], 0);
            xyzwpr.SetValue(posArray[1], 1);
            xyzwpr.SetValue(posArray[2], 2);
            xyzwpr.SetValue(posArray[3], 3);
            xyzwpr.SetValue(posArray[4], 4);
            xyzwpr.SetValue(posArray[5], 5);

            return mobjPosReg.SetValueXyzwpr(index, ref xyzwpr, ref config, intUF, intUT);
        }

        public bool getRBCurPos()
        {
            // 1. cur_pos: xyzwpr
            // 2. cur_config: config

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
                strRBCurPosX = xyzwpr.GetValue(0).ToString();
                strRBCurPosY = xyzwpr.GetValue(1).ToString();
                strRBCurPosZ = xyzwpr.GetValue(2).ToString();
                strRBCurPosW = xyzwpr.GetValue(3).ToString();
                strRBCurPosP = xyzwpr.GetValue(4).ToString();
                strRBCurPosR = xyzwpr.GetValue(5).ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool getUFCurPos()
        {
            // 1. cur_pos: xyzwpr
            // 2. cur_config: config

            string strTmp = null;

            short intUF = 1;
            short intUT = 1;
            short intValidC = 0;
            short intValidJ = 0;
            bool blnDT = false;

            //Refresh data table
            blnDT = mobjDataTable.Refresh();

            if (mobjCurPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
            {
                strUFCurPosX = (float.Parse(xyzwpr.GetValue(0).ToString()) - arrayUFOrigin[0]).ToString();
                strUFCurPosY = (float.Parse(xyzwpr.GetValue(1).ToString()) - arrayUFOrigin[1]).ToString();
                strUFCurPosZ = (float.Parse(xyzwpr.GetValue(2).ToString()) - arrayUFOrigin[2]).ToString();
                strUFCurPosW = (float.Parse(xyzwpr.GetValue(3).ToString()) - arrayUFOrigin[3]).ToString();
                strUFCurPosP = (float.Parse(xyzwpr.GetValue(4).ToString()) - arrayUFOrigin[4]).ToString();
                strUFCurPosR = (float.Parse(xyzwpr.GetValue(5).ToString()) - arrayUFOrigin[5]).ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void move_uf(double[] via_point)
        {
            // config the next via_point
            short UF = 1;
            short UT = 1;

            float[] PosArray = new float[6];
            short[] ConfigArray = new short[6];

            PosArray[0] = float.Parse(via_point[0].ToString());
            PosArray[1] = float.Parse(via_point[1].ToString());
            PosArray[2] = float.Parse(via_point[2].ToString());
            PosArray[3] = float.Parse(via_point[3].ToString());
            PosArray[4] = float.Parse(via_point[4].ToString());
            PosArray[5] = float.Parse(via_point[5].ToString());

            var res = this.setRegisterPos(99, PosArray, this.config, UF, UT);

            // Move
            this.setRegister(1, 4, 1);
            this.setRegister(2, 2, 1);
        }

        public void GetOffsetTCP()
        {
            getRegisterPos(97);

            offset_tcp[0] = float.Parse(RpParamXyzwpr.GetValue(0).ToString());
            offset_tcp[1] = float.Parse(RpParamXyzwpr.GetValue(1).ToString());
            offset_tcp[2] = float.Parse(RpParamXyzwpr.GetValue(2).ToString());
            offset_tcp[3] = float.Parse(RpParamXyzwpr.GetValue(3).ToString());
            offset_tcp[4] = float.Parse(RpParamXyzwpr.GetValue(4).ToString());
            offset_tcp[5] = float.Parse(RpParamXyzwpr.GetValue(5).ToString());
        }

        public void SetOffsetTCP()
        {
            // 1. assign the tcp
            short UF = 0;
            short UT = 0;

            float[] PosArray = new float[6];

            PosArray = offset_tcp;

            var res = this.setRegisterPos(97, PosArray, this.config, this.RpParamUF, this.RpParamUT);

            // 2. station machine: set tool frame
            this.setRegister(1, 5, 1);
            this.setRegister(2, 2, 1);
        }

        public bool SetOffsetTCPTemp(Matrix<double> rpy)
        {
            // 1. assign the tcp temp
            short UF = 1;
            short UT = 2;

            float[] PosArray = new float[6];

            PosArray[0] = offset_tcp[0];
            PosArray[1] = offset_tcp[1];
            PosArray[2] = offset_tcp[2];
            PosArray[3] = (float)rpy[0, 0];
            PosArray[4] = (float)rpy[1, 0];
            PosArray[5] = (float)rpy[2, 0];

            var res = this.setRegisterPos(96, PosArray, this.config, UF, UT);

            Thread.Sleep(3000);

            // 1. assign offset_tcp
            if (res)
            {
                // 2. station machine: set tool frame temp
                this.setRegister(1, 6, 1);
                this.setRegister(2, 2, 1);

                return true;
            }
            else
            {
                MessageBox.Show("Cannot Apply TCP! Please Check!");

                return false;
            }
        }

        public bool WaitForReady()
        {
            object Register1 = -1;
            object Register2 = -1;

            bool res1, res2;

            // wait for R[1], R[2] = 0
            res1 = this.getRegisterInt(1, ref Register1);
            res2 = this.getRegisterInt(2, ref Register2);

            while (Register1.ToString() != 0.ToString() || Register2.ToString() != 0.ToString())
            {
                res1 = this.getRegisterInt(1, ref Register1);
                res2 = this.getRegisterInt(2, ref Register2);
            }

            if (res1 && res2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Wait(int ms)
        {
            Thread.Sleep(ms);
        }

        public void SetOrigin()
        {
            short UF = 1;
            short UT = 2;

            float[] PosArray = new float[6];

            PosArray[0] = 0;
            PosArray[1] = 0;
            PosArray[2] = 0;
            PosArray[3] = 0;
            PosArray[4] = 0;
            PosArray[5] = 0;

            var res = this.setRegisterPos(99, PosArray, this.config, UF, UT);
        }

        public void MovetoOrigin()
        {
            // 0. Initialization
            this.setRegister(1, 0, 1);
            this.setRegister(2, 0, 1);

            // 1. Move to uf_via_point
            this.setRegister(1, 4, 1);
            this.setRegister(2, 2, 1);
        }
    }
}
