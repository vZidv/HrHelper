using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace HrHelper.Windows
{
    public partial class Authorization_win : Window
    {
        public Authorization_win()
        {
            InitializeComponent();

            CheckDatabase();
            mainFrame.Content = new Pages.Authorization_page() { authorization_win = this };
        }

        void CheckDatabase()
        {
            try
            {
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    if (db.Database.CanConnect())
                        return;
                    db.Database.EnsureCreated();

                    #region Костыли

                    //Тип 
                    string[] types = new string[] { "admin", "user" };
                    List<UserType> userTypes = new List<UserType>();
                    foreach (string type in types)
                        userTypes.Add(new UserType() { Type = type });

                    //Стаутс 
                    string[] statuses = new string[] { "Приглашен", "Отказ", "Принят", "Без Статуса" };
                    List<SummaryStatus> summaryStatus = new List<SummaryStatus>();
                    foreach (string status in statuses)
                        summaryStatus.Add(new SummaryStatus() { Status = status });

                    //Занятость 
                    string[] busynnes = new string[] { "Полная занятость", "Частичная занятость", "Проектная работа", "Волонтерство", "Стажировка" };
                    List<Busyness> busynness = new List<Busyness>();
                    foreach (string busynn in busynnes)
                        busynness.Add(new Busyness() { Type = busynn });

                    //Оброзование
                    string[] educations = new string[] { "Cреднее общее образование", "Cреднее профессиональное образование", "Высшее образование" };
                    List<Education> educationss = new List<Education>();
                    foreach (string education in educations)
                        educationss.Add(new Education() { EducationName = education });

                    foreach (Education education in educationss)
                        db.Educations.Add(education);
                    foreach (UserType userType in userTypes)
                        db.UserTypes.Add(userType);
                    foreach (SummaryStatus status in summaryStatus)
                        db.SummaryStatuses.Add(status);
                    foreach (Busyness busynne in busynness)
                        db.Busynesses.Add(busynne);
                    #endregion

                    db.SaveChanges();

                    AuthorizationUser user = new AuthorizationUser()
                    {
                        Login = "dima",
                        Password = "123",
                        UserTypeId = 1
                    };
                    db.AuthorizationUsers.Add(user);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Classes.MyMessageBox.Show("Ошибка", ex.Message);
            };
        }

    }
}
