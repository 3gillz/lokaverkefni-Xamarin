using TrainingXMaster.Views;
using MyClassLibrary;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrainingXMaster
{
    public partial class App : Application
    {
        MemoryService memService = new MemoryService();
        ApiService apiService = new ApiService();
        public App()
        {
            InitializeComponent();

            MainPage = new IntroPage();
        }

        protected override void OnStart()
        {
            if (!Current.Properties.ContainsKey("trainingmaster_token"))
            {
                Current.Properties["trainingmaster_token"] = "";
            }
            if (!Current.Properties.ContainsKey("user"))
            {
                Current.Properties["user"] = "";
            }
            CheckUser();
        }

        public async void CheckUser()
        {
            string token = Current.Properties["trainingmaster_token"].ToString();
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (await CheckToken(token))
                {
                    SetMainPage();
                }
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        private async Task<bool> CheckToken(string token)
        {
            return await apiService.CheckToken(token);
        }

        public void SetMainPage()
        {
            var tp = new MainTabbedPage();
            tp.Children.Add(new FoodPage { });
            tp.Children.Add(new TrainingPage { });
            MainPage = new NavigationPage(tp);
        }

        public class ExtendedViewCell : ViewCell
        {
            public static readonly BindableProperty SelectedBackgroundColorProperty =
                BindableProperty.Create("SelectedBackgroundColor",
                                        typeof(Color),
                                        typeof(ExtendedViewCell),
                                        Color.Default);

            public Color SelectedBackgroundColor
            {
                get { return (Color)GetValue(SelectedBackgroundColorProperty); }
                set { SetValue(SelectedBackgroundColorProperty, value); }
            }
        }
    }
}