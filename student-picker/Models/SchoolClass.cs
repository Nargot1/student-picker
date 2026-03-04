using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace student_picker.Models
{
    public partial class SchoolClass : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _luckyNumber;

        public ObservableCollection<Student> Students { get; } = new();

        public int StudentCount => Students.Count;

        private readonly Random _random = new();

        public SchoolClass(string name)
        {
            _name = name;
            Students.CollectionChanged += (_, _) => OnPropertyChanged(nameof(StudentCount));
        }

        public void AddStudent(string firstName, string lastName)
        {
            int newId = Students.Count > 0 ? Students.Max(s => s.Id) + 1 : 1;
            Students.Add(new Student(newId, firstName, lastName));
        }

        public void RemoveStudent(Student student)
        {
            Students.Remove(student);
            for (int i = 0; i < Students.Count; i++)
                Students[i].Id = i + 1;
            if (LuckyNumber > Students.Count)
                LuckyNumber = 0;
        }

        public void GenerateLuckyNumber()
        {
            if (Students.Count == 0)
            {
                LuckyNumber = 0;
                return;
            }
            LuckyNumber = _random.Next(1, Students.Count + 1);
        }

        public Student? PickRandomStudent()
        {
            foreach (var s in Students.Where(s => s.PickCooldown > 0))
                s.PickCooldown--;

            var available = Students.Where(s =>
                s.IsPresent &&
                s.PickCooldown <= 0 &&
                s.Id != LuckyNumber).ToList();

            if (available.Count == 0)
                return null;

            var picked = available[_random.Next(available.Count)];
            picked.PickCooldown = 3;
            return picked;
        }
    }
}
