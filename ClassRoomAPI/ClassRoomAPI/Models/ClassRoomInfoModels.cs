using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Models
{
    public class ClassRoomData
    {
        public string PositionName;
        public string DetailUri;
    }
    public class ClassBuildingData
    {
        public string BuildingName;
        public string Date;
        public string ClassRoomName;

        //6节课1-6
        public List<string> ListClassStatus;


    }
}
