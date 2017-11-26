using ClassRoomAPI.Helpers;
using ClassRoomAPI.Models;
using ClassRoomAPI.Services;
using ClassRoomAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WebLearnTimeTable : Page
    {
        public WebLearnTimeTable()
        {
            this.InitializeComponent();
        }

       

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (WebLearnAPIService.CredentialAbsent())
            {
                //--
            }
            else
            {
                try
                {
                    var _Data = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Local);
                    MainListView.ItemsSource = _Data.ListAppointment;
                    if ((DateTime.Now - _Data.Date).Minutes > 5)
                        throw new Exception("The Data are out-of-date.");
                    else
                        MainListView.ItemsSource = _Data.ListAppointment;
                }
                catch
                {
                    try
                    {
                        var _DataRemote = await WebLearnTimeTableViewModel.GetTimeTableViewModel(ParseDataMode.Remote);
                        MainListView.ItemsSource = _DataRemote.ListAppointment;
                    }
                    catch
                    {
                        //异常处理，前端
                    }

                }
            }
            
        }
    }
}
