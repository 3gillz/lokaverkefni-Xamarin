﻿using MyClassLibrary;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainingXMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        MemoryService memoryService = new MemoryService();
        ApiService apiService = new ApiService();

        public ProfilePage()
        {
            InitializeComponent();
        }

        async void Close_Profile_Clicked()
        {
            GC.Collect();
            await Navigation.PopModalAsync();
        }

        public void Logout_Clicked()
        {
            Application.Current.Properties.Clear();
            Application.Current.Properties["trainingmaster_token"] = null;
            Application.Current.MainPage = new LoginPage();
            //bool re = await apiService.Logout();
            //if (re)
            //{
            //    Application.Current.MainPage = new LoginPage();
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
