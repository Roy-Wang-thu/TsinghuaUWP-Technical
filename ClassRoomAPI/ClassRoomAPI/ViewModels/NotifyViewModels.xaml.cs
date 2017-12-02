using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using System;

namespace ClassRoomAPI.ViewModels
{
    public sealed partial class NotifyViewModels : UserControl
    {
        private Popup NotifyPopup;

        public NotifyViewModels()
        {
            this.InitializeComponent();
            NotifyPopup = new Popup();
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            NotifyPopup.Child = this;
        }

        public void Show(string Message, TimeSpan MessageDuration)
        {
            NotifySb.Duration = MessageDuration;
            EndKeyFrame.KeyTime = MessageDuration;
            ShowMessage.Text = Message;
            NotifyPopup.IsOpen = true;
            NotifySb.Begin();
        }

        public void Dismiss()
        {
            ShowMessage.Text = "";
            NotifySb.Stop();
            NotifyPopup.IsOpen = false;
        }
    }
}