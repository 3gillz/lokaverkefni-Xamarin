using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyClassLibrary;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace TrainingXMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage : ContentPage
    {
        ApiService apiService = new ApiService();
        private HttpResponseMessage res;
        public bool trainingprogramLoaded = false;

        public TrainingPage()
        {
            InitializeComponent();

        }
        async void Profile_Clicked(object sender, EventArgs e)
        {
            var user = Application.Current.Properties["user"].ToString();
            var page = new NavigationPage(new ProfilePage() { BindingContext = JsonConvert.DeserializeObject<User>(user) });
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
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
            if (trainingprogramLoaded)
            {
                return;
            }
            LoadingIndicator.IsRunning = true;
            res = await apiService.GetTrainingProgramTPID();

            if (res.IsSuccessStatusCode)
            {
                var contents = await res.Content.ReadAsStringAsync();
                TrainingProgram tp = JsonConvert.DeserializeObject<TrainingProgram>(contents);
                TrainingProgramName.Text = tp.name;
                res = await apiService.GetTrainings(tp.TPID);

                if (res.IsSuccessStatusCode)
                {
                    contents = await res.Content.ReadAsStringAsync();
                    Training[] trainings = JsonConvert.DeserializeObject<Training[]>(contents);
                    SetTrainingList(trainings);
                    trainingprogramLoaded = true;
                }
                else
                {
                    //await Error("Something went wrong : " + res.StatusCode);
                }
            }
            else
            {
                //await Error("Something went wrong : " + res.StatusCode);
            }
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
        public async Task Training_Selected(object sender, EventArgs e)
        {
            Training selected = (trainingListView.SelectedItem as Training);
            Exercise exercise = await apiService.GetExercise(selected.exercise_EID);
            NavigationPage page = new NavigationPage(new TrainingDetailPage() { BindingContext = exercise });
            await Application.Current.MainPage.Navigation.PushModalAsync(page, true);
        }


        public void SetTrainingList(Training[] trainings)
        {
            DateTime dt = DateTime.Now;
            DayOfWeek weekday = dt.DayOfWeek;
            WeekDay.Text = weekday.ToString();

            switch (weekday)
            {
                case DayOfWeek.Sunday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.sunday == true));
                    break;
                case DayOfWeek.Monday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.monday == true));
                    break;
                case DayOfWeek.Tuesday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.tuesday == true));
                    break;
                case DayOfWeek.Wednesday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.wednesday == true));
                    break;
                case DayOfWeek.Thursday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.thursday == true));
                    break;
                case DayOfWeek.Friday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.friday == true));
                    break;
                case DayOfWeek.Saturday:
                    trainingListView.ItemsSource = new ObservableCollection<Training>(trainings.Where((training) => training.saturday == true));
                    break;
            }

        }


    }
}
