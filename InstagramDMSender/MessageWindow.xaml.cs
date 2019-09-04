using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace InstagramDMSender
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        const string DEFAULT_TEXT = "Start typing...";

        private long senderId;
        private List<User> recievers;
        private IApiWrapper instaApi;
        public MessageWindow(List<User> recievers, long senderId, IApiWrapper instaApi)
        {
            this.senderId = senderId;
            this.recievers = recievers;
            this.instaApi = instaApi;

            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == DEFAULT_TEXT)
                MessageBox.Text = String.Empty;
            return;
        }

        private void MessageBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Text == string.Empty)
                MessageBox.Text = DEFAULT_TEXT;
            return;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Text;
            await instaApi.SendMessage(recievers.Select(r => r.UserId).ToList(), message, senderId);

            this.Close();
        }
    }
}
