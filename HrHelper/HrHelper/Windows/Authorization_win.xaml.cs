using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for Authorization_win.xaml
    /// </summary>
    public partial class Authorization_win : Window
    {
        public Authorization_win()
        {
            InitializeComponent();

            CheckDatabase();
            mainFrame.Content = new Pages.Authorization_page() { authorization_win = this};
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void close_but_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minWind_but_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        void CheckDatabase()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {               
                if (db.Database.CanConnect())
                    return;
                db.Database.EnsureCreated();

                #region Костыли

                string[] types = new string[] { "admin", "user" };

                List<UserType> userTypes = new List<UserType>();
                foreach (string type in types)
                 userTypes.Add( new UserType() { Type = type });

                string[] statuses = new string[] { "Приглашен" , "Отказ" , "Принят" , "Без Статуса" };
                List<SummaryStatus> summaryStatus = new List<SummaryStatus>();
                foreach (string status in statuses)
                summaryStatus.Add( new SummaryStatus() { Status = status });

                string[] busynnes = new string[] { "Полная занятость" , "Частичная занятость" , "Проектная работа" , "Волонтерство" , "Стажировка" };
                List<Busyness> busynness = new List<Busyness>();
                foreach (string busynn in busynnes)
                    busynness.Add( new Busyness() { Type = busynn });

                foreach (UserType userType in userTypes)
                {
                    db.UserTypes.Add(userType);
                }
                foreach (SummaryStatus status in summaryStatus)
                {
                    db.SummaryStatuses.Add(status);
                }
                foreach (Busyness busynne in busynness)
                {
                    db.Busynesses.Add(busynne);
                }
                #endregion

                db.SaveChanges();

                AuthorizationUser user = new AuthorizationUser()
                {
                    Login = "dima",
                    Password = "123",
                    Type = 1
                };
                db.AuthorizationUsers.Add(user);

                db.SaveChanges();
            }
        }

    }
}
