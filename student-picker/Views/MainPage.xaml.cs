using student_picker.Models;

namespace student_picker.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ClassList.ItemsSource = App.SchoolData.Classes;
        }

        private async void OnAddClassClicked(object? sender, EventArgs e)
        {
            var name = NewClassNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Komunikat", "Wpisz nazwę klasy!", "OK");
                return;
            }

            if (App.SchoolData.GetClassByName(name) != null)
            {
                await DisplayAlert("Komunikat", "Klasa o tej nazwie już istnieje!", "OK");
                return;
            }

            App.SchoolData.AddClass(name);
            NewClassNameEntry.Text = string.Empty;
        }

        private async void OnOpenClassClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is SchoolClass sc)
            {
                await Shell.Current.GoToAsync($"classdetail?className={Uri.EscapeDataString(sc.Name)}");
            }
        }

        private async void OnDeleteClassClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is SchoolClass sc)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", $"Usunąć klasę {sc.Name}?", "Tak", "Nie");
                if (confirm)
                    App.SchoolData.RemoveClass(sc);
            }
        }
    }
}
