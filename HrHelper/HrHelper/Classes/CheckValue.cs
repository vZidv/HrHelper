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

            foreach (Control control in controls)
            {
                if (control.GetType().GetProperty("Text") != null)
                {
                    string textValue = control.GetType().GetProperty("Text").GetValue(control)?.ToString();

                    if (string.IsNullOrEmpty(textValue))
                    {
                        control.BorderBrush = (SolidColorBrush)Application.Current.Resources["Dnd"];
                        hasError = true;
                    }
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
