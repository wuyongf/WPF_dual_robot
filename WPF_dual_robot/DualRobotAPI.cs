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
    }

}
