using MyClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        async void Profile_Clicked(object sender, EventArgs e)
        {
            var user = Application.Current.Properties["user"].ToString();
            var page = new NavigationPage(new ProfilePage() {BindingContext = JsonConvert.DeserializeObject<User>(user) });
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
        }

        protected override async void OnAppearing()
        {
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
        }

        public static ObservableCollection<FoodPortion> foodList { private set; get; }
        public void SetFoodList(FoodPortion[] portions)
        {
            DateTime dt = DateTime.Now;
            DayOfWeek weekday = dt.DayOfWeek;
            foodList = new ObservableCollection<FoodPortion>();

            for (int i = 0; i < portions.Length; i++)
            {
                foodList.Add(portions[i]);

                //if (portions[i].wednesday == true)
                //{
                //    if (portions[i].timeOfDay != null)
                //    {
                //        foodList.Insert(0, portions[i]);
                //    }
                //    else
                //    {
                //        foodList.Add(portions[i]);
                //    }
                //}
            }

            foodListView.ItemsSource = foodList;
        }


    }
}
