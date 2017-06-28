using MyClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace TrainingXMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodPage : ContentPage
    {
        ApiService apiService = new ApiService();
        private HttpResponseMessage res;
        public bool foodprogramLoaded = false;

        public FoodPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (ToolbarItems.Count == 0)
            {
                ToolbarItem delToolbar = new ToolbarItem();
                delToolbar = new ToolbarItem
                {
                    //Order = ToolbarItemOrder.Primary,
                    Text = "Profile",
                    Command = new Command(async () =>
                    {
                        ToolbarItems.Remove(delToolbar);
                        var user = Application.Current.Properties["user"].ToString();
                        var page = new NavigationPage(new ProfilePage() { BindingContext = JsonConvert.DeserializeObject<User>(user) });
                        await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
                    })
                };
                ToolbarItems.Add(delToolbar);
            }
            if (foodprogramLoaded)
            {
                return;
            }
            LoadingIndicator.IsRunning = true;
            res = await apiService.GetFoodProgramFPMID();
            if (res.IsSuccessStatusCode)
            {
                var contents = await res.Content.ReadAsStringAsync();
                FoodProgram fp = JsonConvert.DeserializeObject<FoodProgram>(contents);
                FoodProgramName.Text = fp.name;

                res = await apiService.GetFoodPortions(fp.FPMID);

                if (res.IsSuccessStatusCode)
                {
                    contents = await res.Content.ReadAsStringAsync();
                    FoodPortion[] portions = JsonConvert.DeserializeObject<FoodPortion[]>(contents);
                    SetFoodList(portions);
                    foodprogramLoaded = true;
                    
                }

            }
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }

        public void SetFoodList(FoodPortion[] trainings)
        {
            DateTime dt = DateTime.Now;
            DayOfWeek weekday = dt.DayOfWeek;
            WeekDay.Text = weekday.ToString();
            switch (weekday)
            {
                case DayOfWeek.Sunday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.sunday == true));
                    break;
                case DayOfWeek.Monday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.monday == true));
                    break;
                case DayOfWeek.Tuesday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.tuesday == true));
                    break;
                case DayOfWeek.Wednesday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.wednesday == true));
                    break;
                case DayOfWeek.Thursday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.thursday == true));
                    break;
                case DayOfWeek.Friday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.friday == true));
                    break;
                case DayOfWeek.Saturday:
                    foodListView.ItemsSource = new ObservableCollection<FoodPortion>(trainings.Where((training) => training.saturday == true));
                    break;
            }

        }

    }
}
