using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassRoomAPI.Services
{
    public class ClassRoomAPIService
    {


        public static class ParseShowList
        {
            public static List<ClassRoomInfoData> GetListShow()
            {
                string html = "http://www.hall.tsinghua.edu.cn/columnEx/pwzx_hdap/yc-dy-px-zl-jz/1";
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);
                var htmlNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div[1]/div");
                var InnerTest = htmlNodes[0].InnerHtml;
                Regex.Replace(InnerTest, "::after", "");//Remove after using System.Text.RegularExpressions;
                var doc = new HtmlDocument();
                doc.LoadHtml(InnerTest);
                var ListNodes = doc.DocumentNode.SelectNodes("/div");
                //ParseDataHere

                var Data = new List<ClassRoomInfoData>();
                for (int i = 1; i < ListNodes.Count; i++)
                {
                    string uri = ListNodes[i].ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerText;
                    string PosName = ListNodes[i].ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText;
                    Data.Add(new ClassRoomInfoData
                    {
                        DetailUri = uri,
                        PositionName = PosName
                    }
                    );
                }
                return Data;
            }


        }


        private static DateTime ClassRoomNamesLastLogin = DateTime.MinValue;
        private static int CLASS_ROOM_NAMES_LOGIN_TIMEOUT_MINUTES = 1;
        private static DateTime ClassRoomAllInfoLogin = DateTime.MinValue;
        private static int CLASS_ALL_INFO_LOGIN_TIMEOUT_MINUTES = 1;



        public static class ParseBuildingClassData
        {
            public static async Task<List<ClassRoomInfoData>> GetClassNamesAsync(bool RemoteMode=true)
            {
                if(!RemoteMode)
                {
                    try
                    {
                        var TempData = await CacheHelper.ReadCache("ClassRoomInfoData");
                        var ReturnData = JSONHelper.Parse<List<ClassRoomInfoData>>(TempData);
                        Debug.WriteLine("[GetClassNamesAsync] return local data.");
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetClassNamesAsync] return local data fails.");
                    }
                }
                else
                {
                    Debug.WriteLine("[GetClassNamesAsync] Remote Mode");
                }
                if ((DateTime.Now - ClassRoomNamesLastLogin).TotalMinutes < CLASS_ROOM_NAMES_LOGIN_TIMEOUT_MINUTES)
                {
                    Debug.WriteLine("[GetClassNamesAsync] reuses recent session");
                    var TempData = await CacheHelper.ReadCache("ClassRoomInfoData");
                    var ReturnData = JSONHelper.Parse<List<ClassRoomInfoData>>(TempData);
                    return ReturnData;

                }
                else
                {
                    ClassRoomNamesLastLogin = DateTime.Now;
                }

                var Data = new List<ClassRoomInfoData>();
                try
                {
                    string html = "http://jxgl.cic.tsinghua.edu.cn/jxpg/f/wxjwxs/jsxx?menu=false";
                    HtmlWeb web = new HtmlWeb();
                    var htmlDoc = web.Load(html);
                    var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//html/body/div/div/div[@class='list-block list-class']/div/ul/li");
                    
                    for (int i = 1; i < htmlNodes.Count; i++)
                    {
                        string uri = htmlNodes[i].ChildNodes[1].Attributes["href"].Value;
                        string PosName = htmlNodes[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
                        Data.Add(new ClassRoomInfoData
                        {
                            DetailUri = uri,
                            PositionName = PosName
                        }
                        );
                    }
                    var StringfiedData = JSONHelper.Stringify(Data);
                    await CacheHelper.WriteCache("ClassRoomInfoData", StringfiedData);
                    return Data;
                }
                catch
                {
                    try
                    {
                        var TempData = await CacheHelper.ReadCache("ClassRoomInfoData");
                        var ReturnData = JSONHelper.Parse<List<ClassRoomInfoData>>(TempData);
                        ClassRoomNamesLastLogin = DateTime.MinValue;
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetClassNamesAsync] Get Data from local fails.");
                        ClassRoomNamesLastLogin = DateTime.MinValue;
                        return null;
                    }
                   
                }
  
            }

            public static async Task<List<ClassRoomStatueData>> GetListBuildingInfoAsync(ClassRoomInfoData SourceData,bool RemoteMode=true)
            {
                
                if (!RemoteMode)
                {
                    try
                    {
                        var TempData = await CacheHelper.ReadCache($"ClassBuildingData_{SourceData.PositionName}");
                        var ReturnData = JSONHelper.Parse<List<ClassRoomStatueData>>(TempData);
                        Debug.WriteLine("[GetListBuildingInfoAsync] Return Local");
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListBuildingInfoAsync] 1st session");
                    }
                   
                }
                else
                {
                    Debug.WriteLine("[GetListBuildingInfoAsync] Remote Mode");
                }

                try
                {
                    string html = "http://jxgl.cic.tsinghua.edu.cn/"+SourceData.DetailUri;
                    HtmlWeb web = new HtmlWeb();
                    var htmlDoc = web.Load(html);

                    var _BuildingName = htmlDoc.DocumentNode.SelectNodes("//html/body/div/div/div[1]/div[1]/span/span")[0].InnerText;
                    var _InnerTextDate = htmlDoc.DocumentNode.SelectSingleNode("//html/body/div/div/div[1]/ul/li[1]/div[2]/div/select").InnerHtml;

                    Regex regexObj = new Regex(@"<option selected=""selected\"">(?<mycontent>[\s\S].*?)\r\n    \r\n    <option>");
                    var resultString = regexObj.Match(_InnerTextDate).Groups["mycontent"].Value;
                    var _Date = resultString;

                    var _NodeClassRoom = htmlDoc.DocumentNode.SelectNodes("/html/body/div/div/div[1]/ul/li[2]/div[@class='card-footer no-border']");

                    var Data = new List<ClassRoomStatueData>();
                    for (int i = 0; i < _NodeClassRoom.Count; i++)
                    {
                        var _NodeSpanClassRoom = _NodeClassRoom[i].ChildNodes;
                        var _ClassRoomName = _NodeSpanClassRoom[1].InnerText;

                        var _NodeClassState = _NodeSpanClassRoom[3].SelectNodes("i");

                        var _ListClassState = new List<string>();

                        for (int j = 0; j < _NodeClassState.Count; j++)
                        {
                            var StatueOfClassRoom = _NodeClassState[j].Attributes["class"].Value;
                            if (Regex.IsMatch(StatueOfClassRoom, "ico_zy"))//占用
                            {
                                string item = "占用";
                                _ListClassState.Add(item);
                            }
                            else
                            {
                                string item = "空闲";
                                _ListClassState.Add(item);
                            }

                        }


                        Data.Add(new ClassRoomStatueData
                        {
                            BuildingName = _BuildingName,
                            Date = _Date,
                            ListClassStatus = _ListClassState,
                            ClassRoomName = _ClassRoomName
                        }
                        );
                    }
                    var StringfiedData = JSONHelper.Stringify(Data);
                    await CacheHelper.WriteCache($"ClassBuildingData_{SourceData.PositionName}", StringfiedData);
                    return Data;
                }
                catch
                {
                    if (RemoteMode)
                    {
                        var _Excption = new UseLocalBuildingInfo("无法连接到服务器，正在使用本地数据。");
                        throw _Excption;
                    }
                    try
                    {
                        var TempData = await CacheHelper.ReadCache($"ClassBuildingData_{SourceData.PositionName}");
                        var ReturnData = JSONHelper.Parse<List<ClassRoomStatueData>>(TempData);
                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListBuildingInfoAsync] GetListBuildingInfoAsync fails with no data returned.");
                        var _ListStatue = new List<string>();
                        for (int i=0;i<6;i++)
                        {
                            _ListStatue.Add("N/A");
                        }
                        List<ClassRoomStatueData> _ReturnValue = new List<ClassRoomStatueData>(
                            new[]
                            {
                                new ClassRoomStatueData()
                                {
                                    BuildingName="N/A",
                                    Date="N/A",
                                    ClassRoomName="N/A",

                                    ListClassStatus=_ListStatue,
                                },
                            }
                            );
                        return _ReturnValue;
                    }

                }

                            
            }

            public static async Task<ClassBuildingInfo> GetListAllBuildingInfoAsync(bool RemoteMode=true)
            {
                if(RemoteMode)
                {
                    if ((DateTime.Now - ClassRoomAllInfoLogin).TotalMinutes < CLASS_ALL_INFO_LOGIN_TIMEOUT_MINUTES)
                    {
                        Debug.WriteLine("[ClassAllInfoData] reuses recent session");
                        var TempData = await CacheHelper.ReadCache("AllClassRoomInfoData");
                        var ReturnData = JSONHelper.Parse<ClassBuildingInfo>(TempData);
                        return ReturnData;

                    }
                    else
                    {
                        ClassRoomAllInfoLogin = DateTime.Now;
                    }
                }
                

                var _ClassBuildingInfo = new ClassBuildingInfo();
                _ClassBuildingInfo.ListClassRoomStatue = new List<List<ClassRoomStatueData>>();
                try
                {
                    
                    var _ClassNamesAsync = await GetClassNamesAsync(RemoteMode);
                    _ClassBuildingInfo.ListClassRoomInfo = _ClassNamesAsync;

                    foreach (ClassRoomInfoData item in _ClassNamesAsync)
                    {
                        var _ListBuildingInfo = await GetListBuildingInfoAsync(item,RemoteMode);
                        _ClassBuildingInfo.Date = _ListBuildingInfo[0].Date;
                       
                        _ClassBuildingInfo.ListClassRoomStatue.Add(_ListBuildingInfo);

                    }
                    var StringfiedData = JSONHelper.Stringify(_ClassBuildingInfo);
                    await CacheHelper.WriteCache("AllClassRoomInfoData", StringfiedData);
                    return _ClassBuildingInfo;
                }
                catch
                {
                    if (RemoteMode)
                    {
                        var _Excption = new UseLocalBuildingInfo("无法连接到服务器，正在使用本地数据。");
                        ClassRoomAllInfoLogin = DateTime.MinValue;
                        throw _Excption;
                    }
                    try
                    {
                        var TempData = await CacheHelper.ReadCache("AllClassRoomInfoData");
                        var ReturnData = JSONHelper.Parse<ClassBuildingInfo>(TempData);
                        ClassRoomAllInfoLogin = DateTime.MinValue;

                        return ReturnData;
                    }
                    catch
                    {
                        Debug.WriteLine("[GetListAllBuildingInfoAsync] Get Data from local fails.");
                        ClassRoomAllInfoLogin = DateTime.MinValue;
                       
                        return null;
                    }
                    
                   
                }
               

            }
        }
       
    }
    

}
