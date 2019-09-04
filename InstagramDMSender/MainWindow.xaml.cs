using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;

namespace InstagramDMSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User loggedUser;
        private IApiWrapper instaApi;

        public MainWindow(IApiWrapperBuilder instaApiBuilder)
        {
            InitializeComponent();
            this.instaApi = instaApiBuilder.CreateApi();
            LoadSession();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            var isLoggedIn = await instaApi.LoginAsync(LoginText.Text, PasswordText.Password);

            if (isLoggedIn)
            {
                if (RememberMe.IsChecked.Value)
                    SaveSession();
                await GetUserData();
                HideLoginItems();
            }
            else
            {
                var dialogBox = new DialogBox($"Error during loging in");
                dialogBox.Show();
            }
            LoginButton.IsEnabled = true;
        }

        private async Task GetUserData()
        {
            loggedUser = await instaApi.GetLoggedInUserAsync();
            ShowLoggedInUsername();
            await LoadFollowersList();
        }

        private async Task LoadFollowersList()
        {
            var followers = await GetFollowers();
            var followersForList = followers.ToList();
            FollowersList.ItemsSource = followersForList;
            FollowersList.DisplayMemberPath = nameof(User.UserName);
            FollowersList.SelectedValuePath = nameof(User.UserId);
        }

        private async Task<IEnumerable<User>> GetFollowers()
        {
            var followers = await instaApi.GetFollowersAsync(loggedUser.UserId);
            var followersForList = FormatFollowers(followers);
            return followersForList;
        }

        private static IEnumerable<User> FormatFollowers(IEnumerable<User> followers)
        {
            return followers.Select(a => new User { UserName = $"{a.UserName} {(a.Private ? "*" : "")}", UserId = a.UserId });
        }

        private async void SaveSession()
        {
            await instaApi.SaveLoginData();
        }

        private async void LoadSession()
        {
            var succesfull = await instaApi.LoadLoginData();
            if (succesfull)
            {
                HideLoginItems();
                await GetUserData();
            }

        }
        private void ShowLoggedInUsername()
        {
            StatusLabel.Content = $"Logged as {loggedUser.UserName}";
            StatusLabel.Visibility = Visibility.Visible;
        }

        private void HideLoginItems()
        {
            LoginGrid.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Visible;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await instaApi.Logout();
            ShowLoginItems();
            FollowersList.ItemsSource = null;
        }

        private void ShowLoginItems()
        {
            LoginGrid.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Hidden;
            StatusLabel.Visibility = Visibility.Hidden;
        }

        private void FollowersList_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var messageRecivers = FollowersList.SelectedItems;
            var messageReciverIds = new List<long>();
            foreach (var reciever in messageRecivers)
            {
                messageReciverIds.Add(((User)reciever).UserId);
            }

            if (messageReciverIds == null || messageReciverIds.Count == 0)
                return;

            var messageWindow = new MessageWindow(messageReciverIds, loggedUser.UserId, instaApi);
            messageWindow.Show();
        }
    }
}
