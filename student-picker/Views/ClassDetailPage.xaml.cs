using student_picker.Models;

namespace student_picker.Views
{
    [QueryProperty(nameof(ClassName), "className")]
    public partial class ClassDetailPage : ContentPage
    {
        private SchoolClass? _schoolClass;

        public string ClassName
        {
            set
            {
                var decoded = Uri.UnescapeDataString(value ?? string.Empty);
                _schoolClass = App.SchoolData.GetClassByName(decoded);
                if (_schoolClass != null)
                    BindingContext = _schoolClass;
            }
        }

        public ClassDetailPage()
        {
            InitializeComponent();
        }

        private void OnGenerateLuckyNumber(object? sender, EventArgs e)
        {
            _schoolClass?.GenerateLuckyNumber();
            SaveClass();
        }

        private async void OnPickStudent(object? sender, EventArgs e)
        {
            if (_schoolClass == null) return;

            var picked = _schoolClass.PickRandomStudent();
            if (picked == null)
            {
                PickResultLabel.Text = "Brak dostępnych uczniów!";
                await DisplayAlert("Komunikat", "Brak uczniów do losowania!", "OK");
                return;
            }

            PickResultLabel.Text = $"{picked.Name} {picked.LastName} (nr {picked.Id})";
            SaveClass();
        }

        private async void OnAddStudent(object? sender, EventArgs e)
        {
            if (_schoolClass == null) return;

            var firstName = FirstNameEntry.Text?.Trim();
            var lastName = LastNameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                await DisplayAlert("Komunikat", "Wpisz imię i nazwisko!", "OK");
                return;
            }

            _schoolClass.AddStudent(firstName, lastName);
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            SaveClass();
        }

        private async void OnRemoveStudent(object? sender, EventArgs e)
        {
            if (_schoolClass == null || sender is not Button btn || btn.CommandParameter is not Student student)
                return;

            bool confirm = await DisplayAlert("Potwierdzenie", $"Usunąć ucznia {student.Name} {student.LastName}?", "Tak", "Nie");
            if (confirm)
            {
                _schoolClass.RemoveStudent(student);
                SaveClass();
            }
        }

        private async void OnEditStudent(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.CommandParameter is not Student student)
                return;

            var newFirstName = await DisplayPromptAsync("Edycja ucznia", "Imię:", initialValue: student.Name);
            if (newFirstName == null) return;

            var newLastName = await DisplayPromptAsync("Edycja ucznia", "Nazwisko:", initialValue: student.LastName);
            if (newLastName == null) return;

            student.Name = newFirstName.Trim();
            student.LastName = newLastName.Trim();
            SaveClass();
        }

        private void OnSaveClass(object? sender, EventArgs e)
        {
            SaveClass();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SaveClass();
        }

        private void SaveClass()
        {
            if (_schoolClass != null)
                App.SchoolData.SaveClass(_schoolClass);
        }
    }
}
