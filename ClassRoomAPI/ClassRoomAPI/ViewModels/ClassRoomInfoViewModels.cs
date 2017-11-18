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
        public static async Task<List<ClassRoomInfoData>> GetBuildingNames(bool RemoteMode=true)
        {
            var _ReturnData = new List<ClassRoomInfoData>();

            _ReturnData = await ClassRoomAPIService.ParseBuildingClassData.GetClassNamesAsync(RemoteMode);
            return _ReturnData;
        }

        public static async Task<List<ClassRoomStatueData>> GetBuildingInfoByName(string Name="四教", bool RemoteMode = true)
        {
            var _OriginData = await ClassRoomAPIService.ParseBuildingClassData.GetListAllBuildingInfoAsync(RemoteMode);
            var _ReturnData = new List<ClassRoomStatueData>();
            if(_OriginData!=null)
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
