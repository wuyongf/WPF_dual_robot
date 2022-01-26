using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace WPF_dual_robot
{
    public class DualRobotAPI
    {
        /// <summary>
        /// Initialization 
        /// </summary>

        public bool Connect(string RobotModel, string IPAddress)
        {
            return false;
        }

        public bool Connect(string RobotModel, string IPAddress, string Port)
        {
            return false;
        }

        public bool GetConnectionStatus()
        {
            return false;
        }

        public int GetMissionStatus()
        {
            return 0;
        }

        bool GetToolFrame(int no)
        {
            return false;
        }


        // 1. bool GetConnectionStatus()
        // 2. int GetMissionStatus()

        // 1. bool Connect(string ip,string port)
        // 2. bool ResetMovement()
        // 3. bool SetTCP(float[])
        // 4. float[] GetCurPos();
        // 5. bool SetOriginal(float[])
        // 6. bool SetSinglePoint(float[])
        // 7. bool MoveSinglePoint()
        // 8. bool Scenario1A()
        // 9. bool Scenario2A()

    }
}
