using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomAPI.ViewModels
{
    public class ClassRoomInfoViewModels
    {
        public static async Task<List<ClassRoomInfoData>> GetBuildingNames()
        {
            return await ClassRoomAPIService.ParseBuildingClassData.GetClassNamesAsync();
        }

        public static async Task<List<ClassRoomStatueData>> GetBuildingInfoByName(string Name="四教")
        {
            var _OriginData = await ClassRoomAPIService.ParseBuildingClassData.GetListAllBuildingInfoAsync();
            var _ReturnData = new List<ClassRoomStatueData>();

            foreach (List<ClassRoomStatueData> item in _OriginData.ListClassRoomStatue)
            {
                if(item[0].BuildingName==Name)
                {
                    _ReturnData = item;
                }
            }

            return _ReturnData;
        }
    }
}
