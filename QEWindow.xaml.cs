using System;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    public partial class QEWindow : Window
    {
        public QEWindow()
        {
            InitializeComponent();
        }

        // Класс вычислений (взято из лабы по C#)
        private static class Operations
        {
            // Метод вычисления дискриминанта
            public static int Discriminant(double a, double b, double c, out double D)
            {
                D = Math.Pow(b, 2) - 4 * a * c; // формула дискриминанта
                if (D < 0)
                {
                    return -1;
                }
                else if (D > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            public static bool Root(double a, double b, double c, out double x1, out double x2)
            {
                x1 = 0; // инициализация х1 и х2
                x2 = 0;

                double D;
                int d = Discriminant(a, b, c, out D); // вызов D по ссылке

                if (d == -1)
                {
                    return false; // корней нет
                }
                else if (d == 0)
                {
                    x1 = -b / (2 * a);
                    x2 = x1; // один корень
                    return true;
                }
                else // (d == 1)
                {   // два корня
                    x1 = (-b + Math.Sqrt(D)) / (2 * a);
                    x2 = (-b - Math.Sqrt(D)) / (2 * a);
                    return true;
                }
            }
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем корректность ввода коэффициентов
                if (!double.TryParse(A.Text, out double a))
                {
                    Result.Text = "Ошибка: Введите корректное число для коэффициента a";
                    return;
                }

                if (!double.TryParse(B.Text, out double b))
                {
                    Result.Text = "Ошибка: Введите корректное число для коэффициента b";
                    return;
                }

                if (!double.TryParse(C.Text, out double c))
                {
                    Result.Text = "Ошибка: Введите корректное число для коэффициента c";
                    return;
                }

                // Проверяем, что a не равно 0
                if (a == 0)
                {
                    Result.Text = "Ошибка: Коэффициент a не может быть равен 0!";
                    return;
                }

                // Вычисляем корни уравнения
                double x1, x2;
                bool hasRoots = Operations.Root(a, b, c, out x1, out x2);

                string result;
                if (!hasRoots) // если Roots == false
                {
                    result = $"Корней уравнения с коэффициентами a = {a}, b = {b}, c = {c} нет.";
                }
                else if (x1 == x2) // если корень один
                {
                    result = $"Корень: x = {FormatNumber(x1)}";
                }
                else
                {
                    result = $"Корни: x₁ = {FormatNumber(x1)}, x₂ = {FormatNumber(x2)}";
                }

                Result.Text = result;
            }
            catch (Exception ex)
            {
                Result.Text = $"Ошибка: {ex.Message}";
            }
        }
        private string FormatNumber(double number)
        {
            // Округляем до 6 знаков после запятой
            if (Math.Abs(number) < 1e-10) return "0";
            if (Math.Abs(number - Math.Round(number)) < 1e-10)
                return Math.Round(number).ToString();
            return Math.Round(number, 6).ToString();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Обработчик для проверки ввода (только числа)
        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[-]?[0-9]*[.,]?[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}