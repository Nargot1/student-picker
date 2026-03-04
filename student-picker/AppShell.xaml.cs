using student_picker.Views;

namespace student_picker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("classdetail", typeof(ClassDetailPage));
        }
    }
}
