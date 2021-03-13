using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            var assembly = typeof(RegisterPage);
            iconImage.Source = ImageSource.FromResource("TravelRecordApp.Assets.Images.plane.png", assembly);
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (passwordEntry.Text == confirmpasswordEntry.Text)
            {
                //Register the user
                Users user = new Users()
                {
                    Email = emailEntry.Text,
                    Password = passwordEntry.Text
                };

                Users.Register(user);

                //Log user in after registering
                bool canLogin = await Users.Login(emailEntry.Text, passwordEntry.Text);

                if (canLogin)
                    await Navigation.PushAsync(new HomePage());
                else
                    await DisplayAlert("Error", "Try again", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Passwords don't match", "ok");
            }
        }
    }
}