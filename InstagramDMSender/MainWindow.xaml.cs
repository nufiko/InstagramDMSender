using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Models;
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
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var isLoggedIn = await instaApi.LoginAsync(LoginText.Text, PasswordText.Password);
            
            if(isLoggedIn)
            {
                loggedUser = await instaApi.GetLoggedInUserAsync();
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

        private void HideLoginItems()
        {
            LoginButton.Visibility = Visibility.Hidden;
            LoginText.Visibility = Visibility.Hidden;
            PasswordText.Visibility = Visibility.Hidden;
        }

        //private async void GetFollowersButton_Click(object sender, RoutedEventArgs e)
        //{
        //    GetFollowersButton.IsEnabled = false;
        //    var followers = instaApi.GetFollowers(loggedUser.UserId);

        //    FollowersList.ItemsSource = followers.Value;
        //    FollowersList.DisplayMemberPath = "FullName";
        //    GetFollowersButton.IsEnabled = true;
        //}

        //private long GetUserId()
        //{
        //    if (FollowersList.SelectedItem == null)
        //        return userId;
        //    return ((KeyValuePair<string, long>)FollowersList.SelectedItem).Value;
        //}

        //private async Task<IResult<InstaUserShortList>> GetFollowers(long userId)
        //{
        //    var paginationParameters = PaginationParameters.Empty;
        //    //var followers = await instaApi.UserProcessor.GetUserFollowersByIdAsync(userId, paginationParameters);

        //    //return followers;
        //}
    }
}
