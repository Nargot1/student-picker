using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace student_picker.Models
{
    public partial class School : ObservableObject
    {
        public ObservableCollection<SchoolClass> Classes { get; } = new();

        private static string DataDirectory =>
            Path.Combine(FileSystem.AppDataDirectory, "classes");

        public School()
        {
            LoadAll();
        }

        public SchoolClass AddClass(string name)
        {
            var sc = new SchoolClass(name);
            Classes.Add(sc);
            SaveClass(sc);
            return sc;
        }

        public void RemoveClass(SchoolClass sc)
        {
            Classes.Remove(sc);
            var path = GetFilePath(sc.Name);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void SaveClass(SchoolClass sc)
        {
            Directory.CreateDirectory(DataDirectory);
            var path = GetFilePath(sc.Name);
            var lines = new List<string>();
            lines.Add($"LuckyNumber:{sc.LuckyNumber}");
            foreach (var student in sc.Students)
            {
                lines.Add($"{student.Id};{student.Name};{student.LastName};{student.IsPresent};{student.PickCooldown}");
            }
            File.WriteAllLines(path, lines);
        }

        public void SaveAll()
        {
            foreach (var sc in Classes)
                SaveClass(sc);
        }

        public void LoadAll()
        {
            Classes.Clear();
            Directory.CreateDirectory(DataDirectory);
            foreach (var file in Directory.GetFiles(DataDirectory, "*.txt"))
            {
                var sc = LoadClass(file);
                if (sc != null)
                    Classes.Add(sc);
            }
        }

        private static SchoolClass? LoadClass(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return null;

            var className = Path.GetFileNameWithoutExtension(filePath);
            var sc = new SchoolClass(className);

            foreach (var line in lines)
            {
                if (line.StartsWith("LuckyNumber:"))
                {
                    if (int.TryParse(line.Substring("LuckyNumber:".Length), out int ln))
                        sc.LuckyNumber = ln;
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length >= 5 &&
                    int.TryParse(parts[0], out int id) &&
                    bool.TryParse(parts[3], out bool present) &&
                    int.TryParse(parts[4], out int cooldown))
                {
                    sc.Students.Add(new Student(id, parts[1], parts[2], present, cooldown));
                }
            }

            return sc;
        }

        public SchoolClass? GetClassByName(string name) =>
            Classes.FirstOrDefault(c => c.Name == name);

        private static string GetFilePath(string className) =>
            Path.Combine(DataDirectory, $"{className}.txt");
    }
}
