using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Services
{
    public class UseLocalBuildingInfo : Exception
    {
        public new string Message;
        public UseLocalBuildingInfo(string _message)
        {
            Message = _message;
        }
    }
}
