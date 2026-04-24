using System.Windows;
using ShiBoo.Data;

namespace ShiBoo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using (var db = new ShiBooDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}