using Microsoft.Maui.Controls.StyleSheets;
using student_picker.Models;

namespace student_picker
{
    public partial class App : Application
    {
        public static School SchoolData { get; } = new School();

        public App()
        {
            InitializeComponent();
            LoadStyleSheet();
        }

        private void LoadStyleSheet()
        {
            using var stream = typeof(App).Assembly.GetManifestResourceStream("student_picker.Resources.Styles.app.css");
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                Resources.Add(StyleSheet.FromString(reader.ReadToEnd()));
            }
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}