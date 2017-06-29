using System;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using MyClassLibrary;

namespace TrainingXMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ApiService apiService = new ApiService();
        MemoryService memService = new MemoryService();
        private HttpResponseMessage res;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginContext
            {
                TitleLabel = "Training Master",
                NameLabel = "Email:",
                PasswordLabel = "Password:"
            };
        }


        private async Task LoginButton_Clicked(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = false;
            LoginIndicator.IsRunning = true;

            res = await apiService.Login(name.Text, password.Text);

            if (res.IsSuccessStatusCode)
            {
                var contents = await res.Content.ReadAsStringAsync();
                TokenObject tokenObject = JsonConvert.DeserializeObject<TokenObject>(contents);
                //memService.SaveString("trainingmaster_token", tokenObject.access_token);
                Application.Current.Properties["trainingmaster_token"] = tokenObject.access_token;
                res = await apiService.GetUserInfo();

                if (res.IsSuccessStatusCode)
                {
                    contents = await res.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(contents);
                    if (user == null)
                    {
                        await Error("User not found");
                    }
                    else
                    {
                        memService.SerializeUser(user);
                        //Application.Current.MainPage = new MainPage();
                        SetMainPage();
                    }
                }
                else
                {
                    await Error("Something went wrong : " + res.StatusCode);
                }
            }
            else
            {
                await Error("Something went wrong : " + res.StatusCode);
            }
            LoginButton.IsEnabled = true;
            LoginIndicator.IsRunning = false;

        }

        private async Task Error(string errorMessages)
        {
            await DisplayAlert("Error", errorMessages, "ok");
        }

        public class LoginContext
        {
            public string TitleLabel { get; set; }
            public string NameLabel { get; set; }
            public string PasswordLabel { get; set; }
        }


        public static void SetMainPage()
        {
            var tp = new MainTabbedPage() { BarBackgroundColor =  Color.FromHex("#474544") };
            tp.Children.Add(new FoodPage { });
            tp.Children.Add(new TrainingPage { });
            Application.Current.MainPage = new NavigationPage(tp);
        }

    }
}