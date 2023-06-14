using HrHelper.Classes;
using HrHelper.Windows;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HrHelper.Pages
{
    /// <summary>
    /// Interaction logic for User_page.xaml
    /// </summary>
    public partial class UserList_page : Page
    {
        public UserList_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        // Функция для загрузки данных пользователей ву
        private void LoadDataGrid()
        {
            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Загружаем данные пользователей в таблицу, включая типы пользователей
                users_dg.ItemsSource = db.AuthorizationUsers.Include(o => o.UserType).ToArray();
            }

            // Обновляем количество строк в таблице
            RowCountUpdate();
        }

        // Функция для обновления количества строк в таблице
        private void RowCountUpdate() => alluses_tblock.Text = $"Всего - {users_dg.Items.Count}";

        // Обработчик нажатия кнопки "Добавить пользователя"
        private void userAdd_but_Click(object sender, RoutedEventArgs e)
        {
            // Создаем окно для добавления пользователя
            MinWin_win win = new MinWin_win(new UserAdd_page());

            // Отображаем окно и ждем, пока пользователь закроет его
            win.ShowDialog();

            // Обновляем данные в таблице
            LoadDataGrid();
        }

        // Обработчик нажатия кнопки "Удалить пользователя"
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранного пользователя
            AuthorizationUser user = users_dg.SelectedItem as AuthorizationUser;

            // Проверяем, точно ли пользователь хочет удалить выбранного пользователя
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этого пользователя?", MyMessageBoxOptions.YesNo) == false)
                return;

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Удаляем пользователя из базы данных
                db.AuthorizationUsers.Remove(user);
                db.SaveChanges();
            }

            // Выводим сообщение об успешном удалении пользователя
            MyMessageBox.Show("Внимание", "Пользователь успешно удален!");

            // Обновляем данные в таблице
            LoadDataGrid();
        }

        // Обработчик нажатия кнопки "Редактировать пользователя"
        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            // Создаем окно для редактирования пользователя и передаем выбранного пользователя в качестве параметра
            MinWin_win win = new MinWin_win(new UserEdit_page(users_dg.SelectedItem as AuthorizationUser));

            // Отображаем окно и ждем, пока пользователь закроет его
            win.ShowDialog();

            // Обновляем данные в таблице
            LoadDataGrid();
        }

        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Фильтруем данные
                AuthorizationUser[]  authorizationUsers = db.AuthorizationUsers.Where(o =>
                EF.Functions.Like(o.Login, $"%{search_tb.Text}%")).Include(o => o.UserType).ToArray();

                // Обновляем таблицу с данными 
                users_dg.ItemsSource = authorizationUsers;

                // Обновляем количество строк в таблице
                RowCountUpdate();
            }
        }
    }
}
