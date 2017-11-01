using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.Models
{
    public class ClassRoomInfoData
    {
        public string PositionName;
        public string DetailUri;
    }
    public class ClassRoomStatueData
    {
        public string BuildingName;
        public string Date;
        public string ClassRoomName;

        //6节课1-6
        public List<string> ListClassStatus;

    }
    public class ClassBuildingInfo
    {
        public string Date;
        public List<List<ClassRoomStatueData>> ListClassRoomStatue;
    }


}
