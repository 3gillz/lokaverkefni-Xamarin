using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingXMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingDetailPage : ContentPage
    {
        public TrainingDetailPage()
        {
            InitializeComponent();
        }

        public void PlayVideo_Clicked(Button sender)
        {
           string link = sender.CommandParameter.ToString() + "?autoplay=1&cc_load_policy=1.";
           Device.OpenUri(new Uri(link));          
        }

        async void Back_Clicked()
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}
