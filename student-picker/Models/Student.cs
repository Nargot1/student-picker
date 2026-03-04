using CommunityToolkit.Mvvm.ComponentModel;

namespace student_picker.Models
{
    public partial class Student : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private bool _isPresent = true;

        [ObservableProperty]
        private int _pickCooldown;

        public bool IsAbsent => !IsPresent;
        public bool HasCooldown => PickCooldown > 0;
        public bool ShowPause => IsPresent && HasCooldown;

        public Student(int id, string name, string lastName, bool isPresent = true, int pickCooldown = 0)
        {
            _id = id;
            _name = name;
            _lastName = lastName;
            _isPresent = isPresent;
            _pickCooldown = pickCooldown;
        }

        partial void OnIsPresentChanged(bool value)
        {
            OnPropertyChanged(nameof(IsAbsent));
            OnPropertyChanged(nameof(ShowPause));
        }

        partial void OnPickCooldownChanged(int value)
        {
            OnPropertyChanged(nameof(HasCooldown));
            OnPropertyChanged(nameof(ShowPause));
        }
    }
}
