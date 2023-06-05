using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HrHelper.Classes
{
    /// <summary>
    /// Статический класс, содержащий методы для проверки и подсветки обязательных полей.
    /// </summary>
    static class CheckValue
    {
        /// <summary>
        /// Проверяет, заполнены ли обязательные поля, и подсвечивает соотствующие поля, если они не заполнены.
        /// </summary>
        /// <param name="controls">Список полей, которые нужно проверить.</param>
        /// <returns>True, если хотя бы одно обязное поле не заполнено, иначе - false.</returns>
        public static bool CheckElementNullValue(List<Control> controls)
        {
            bool hasError = false;

            // Проверка каждого элемента управления в списке
            foreach (Control control in controls)
            {
                // Проверка, имеет элемент управления свойство Text
                if (control.GetType().GetProperty("Text") != null)
                {
                    // Получение значения свойства Text элемента управления
                    string textValue = control.GetType().GetProperty("Text").GetValue(control)?.ToString();

                    // Если значение свойства Text пустое или null, то установка границы элемента управления красным цветом и установка флага ошибки
                    if (string.IsNullOrEmpty(textValue))
                    {
                        control.BorderBrush = (SolidColorBrush)Application.Current.Resources["Dnd"];
                        hasError = true;
                    }
                    // Иначе - очистка границы элемента управления
                    else
                    {
                        control.ClearValue(Border.BorderBrushProperty);
                    }
                }
            }

            return hasError;
        }
    }

    
}
