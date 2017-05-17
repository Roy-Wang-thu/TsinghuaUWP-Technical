using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TsinghuaUWP.WebLearn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Weblearn : Page
    {
        public List<Course> listcourses;
        private ObservableCollection<Deadline> ddl1;
        private ObservableCollection<Course> courses1;
        private ObservableCollection<Announce> announces;
        private List<Announce> listanc;
        private List<Deadline> listddl;
        public Weblearn()
        {
            this.InitializeComponent();
            courses1 = new ObservableCollection<Course>();
            ddl1 = new ObservableCollection<Deadline>();
            announces = new ObservableCollection<Announce>();
            

        }

        private async void Oninitial(object sender, RoutedEventArgs e)
        {
            try
            {
                listcourses = await DataAccess.getCourses();//temp
                courses1.Clear();
                listcourses.ForEach(p => courses1.Add(p));
            }
            catch
            {

            }
        }

        private async void Coursebuttons_Click(object sender, RoutedEventArgs e)
        {
            PR0.IsActive = true;

            try {

                listcourses = await DataAccess.getCourses(forceRemote: true);//temp
                Debug.WriteLine("[remote refresh]await DataAccess.getCourses(forceRemote: true)");
            }
            catch
            {

                //MessageDialog a = new MessageDialog("Wrong data local try remote");
                //await a.ShowAsync();
                Debug.WriteLine("[remote refresh]Wrong data local try remote");
            }
            try
            {
                await DataAccess.getAllAnnounce(forceRemote: true);
                Debug.WriteLine("[remote refresh] await DataAccess.getAllAnnounce(forceRemote: true);");
            }
            catch
            {
                MessageDialog a = new MessageDialog("Wrong data anc remote");
                await a.ShowAsync();
            }
            try
            {
                await DataAccess.getAllDeadlines(forceRemote: true);
                Debug.WriteLine("[remote refresh]await DataAccess.getAllDeadlines(forceRemote: true);");
            }
            catch
            {
                MessageDialog a = new MessageDialog("Wrong data ddl remote");
                await a.ShowAsync();
            }

            try
            {
                 courses1.Clear();
                listcourses.ForEach(p => courses1.Add(p));
            }
            catch
            {
                MessageDialog a = new MessageDialog("Wrong data add to list");
                await a.ShowAsync();
            }
            PR0.IsActive = false;
        }
        private async void CourseGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tp;
            String coid;
            tp = CourseGrid.SelectedIndex;

            try
            {
                coid = listcourses[tp].id;
                listddl = await DataAccess.getAllDeadlines();
                listddl.ForEach(p => ddl1.Add(p));
                ddl1.Clear();
                var filteredDDLItems = listddl.Where(p => p.courseid == coid).ToList();
                filteredDDLItems.ForEach(p => ddl1.Add(p));

                coid = listcourses[tp].id;
                listanc = await DataAccess.getAllAnnounce();
                listanc.ForEach(q => announces.Add(q));
                announces.Clear();
                var filteredDDLItems1 = listanc.Where(p => p.courseId == coid).ToList();
                filteredDDLItems1.ForEach(p => announces.Add(p));
            }
            catch
            {
               // INFOTB.Visibility = Visibility.Visible;
            }
        }

        private void CoursePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void HWGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private async void AncGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tp;
            tp = AncGrid.SelectedIndex;
            try
            {
                var tp1 = CourseGrid.SelectedIndex;
                bool coid = courses1[tp1].isNew;

                if (!coid)
                {
                    string uri = announces[tp].detail;
                    var dialog = new ContentDialogAnc(uri, false);

                    await dialog.ShowAsync();
                }
                else
                {
                    string uri = announces[tp].detail;


                    var dialog1 = new ContentDialogAnc(uri, true);

                    await dialog1.ShowAsync();

                }


                AncGrid.SelectedItem = null;

            }
            catch
            {
            }
        }
    }
}
