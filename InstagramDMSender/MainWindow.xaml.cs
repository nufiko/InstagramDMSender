using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.ApiWrapper;
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
        private long userId;
        private IApiWrapper instaApi;

        public MainWindow()
        {
            InitializeComponent();
            instaApi = new ApiWrapper.ApiWrapper();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var isLoggedIn = await instaApi.LoginAsync(LoginText.Text, PasswordText.Password);
            
            if(isLoggedIn)
            {
                userId = GetLoggedUserId();
                LoginLabel.Content = $"Logged as {loggedUser.UserName}";
                LoginLabel.Visibility = Visibility.Visible;
                HideLoginItems();
            }
            else
            {
                LoginLabel.Content = $"Error during loging in";
                LoginLabel.Visibility = Visibility.Visible;
            }
        }

        private InstaUserShort GetLoggedUser()
        {
            return instaApi.GetLoggedUser().LoggedInUser;
        }
        private long GetLoggedUserId()
        {
            return GetLoggedUser().Pk;
        }
        private void HideLoginItems()
        {
            LoginButton.Visibility = Visibility.Hidden;
            LoginText.Visibility = Visibility.Hidden;
            PasswordText.Visibility = Visibility.Hidden;
        }
        

        private async Task<IResult<InstaLoginResult>> Login(this IInstaApi instaApi)
        {
            return await instaApi.LoginAsync();
        }

        private async void GetFollowersButton_Click(object sender, RoutedEventArgs e)
        {
            GetFollowersButton.IsEnabled = false;
            var followers = await GetFollowers(userId);

            FollowersList.ItemsSource = followers.Value;
            FollowersList.DisplayMemberPath = "FullName";
            GetFollowersButton.IsEnabled = true;
        }

        private long GetUserId()
        {
            if (FollowersList.SelectedItem == null)
                return userId;
            return ((KeyValuePair<string, long>)FollowersList.SelectedItem).Value;
        }

        private async Task<IResult<InstaUserShortList>> GetFollowers(long userId)
        {
            var paginationParameters = PaginationParameters.Empty;
            var followers = await instaApi.UserProcessor.GetUserFollowersByIdAsync(userId, paginationParameters);

            return followers;
        }
    }
}
