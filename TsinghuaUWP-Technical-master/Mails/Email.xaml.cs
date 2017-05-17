using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using System.Xml;
using Windows.UI.Popups;




// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TsinghuaUWP.Mails
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Email : Page
    {
        public Email()
        {
            this.InitializeComponent();

        }

        private async void Oninitial()
        {
            HtmlDocument HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml("http://mails.tsinghua.edu.cn/coremail/xphone/index.jsp");
            string lgrs;
         lgrs= await Remote.POST("http://mails.tsinghua.edu.cn/coremail/xphone/main.jsp", "service=PHONE&face=XJS&locale=zh_CN&destURL=%2Fcoremail%2Fxphone%2Fmain.jsp&domain=mails.tsinghua.edu.cn & uid = jw - zhang15 & password = zjw660815 & action % 3Alogin = ");

            

        }

        private async void gg_Click(object sender, RoutedEventArgs e)
        {
            string lgrs;
            lgrs = await Remote.POST("http://mails.tsinghua.edu.cn/coremail/xphone/main.jsp", "service=PHONE&face=XJS&locale=zh_CN&destURL=%2Fcoremail%2Fxphone%2Fmain.jsp&domain=mails.tsinghua.edu.cn&uid=jw-zhang15&password=zjw660815&action%3Alogin=");

            MessageDialog a = new MessageDialog(lgrs);
            await a.ShowAsync();
        }
    }
}
