using System;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    public partial class EngiWindow : Window
    {
        public EngiWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            EnterField.Text = CalcEngine.GetCurrentDisplay();
        }
        private void SimpleMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Left = this.Left;
            this.Owner.Top = this.Top;
            this.Owner.Show();
            this.Close();
        }
        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        //метод обработки клика по сетке
        private void myGrid_Click_1(object sender, RoutedEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (element == null) return;

            //try
            //{
                switch (element.Name)
                {
                    // Цифры
                    case "zero": CalcEngine.InputNumber("0"); break;
                    case "one": CalcEngine.InputNumber("1"); break;
                    case "two": CalcEngine.InputNumber("2"); break;
                    case "three": CalcEngine.InputNumber("3"); break;
                    case "four": CalcEngine.InputNumber("4"); break;
                    case "five": CalcEngine.InputNumber("5"); break;
                    case "six": CalcEngine.InputNumber("6"); break;
                    case "seven": CalcEngine.InputNumber("7"); break;
                    case "eight": CalcEngine.InputNumber("8"); break;
                    case "nine": CalcEngine.InputNumber("9"); break;

                    // Операции
                    case "sum": CalcEngine.SetOperation(CalcEngine.Operator.eAdd); break;
                    case "substract": CalcEngine.SetOperation(CalcEngine.Operator.eSubtract); break;
                    case "times": CalcEngine.SetOperation(CalcEngine.Operator.eMultiply); break;
                    case "divide": CalcEngine.SetOperation(CalcEngine.Operator.eDivide); break;

                    // Функции
                    case "equal": CalcEngine.Calculate(); break;
                    case "comma": CalcEngine.InputDecimal(); break;
                    case "invert": CalcEngine.ChangeSign(); break;
                    case "c": CalcEngine.Clear(); break;
                    case "ce": CalcEngine.ClearAll(); break;
                    case "backspace": CalcEngine.Backspace(); break;

                    //Инженерные Функции
                    case "power": CalcEngine.Power(); break;
                    case "square": CalcEngine.Square(); break;
                    case "sqrt": CalcEngine.SquareRoot(); break;
                    case "reciprocal": CalcEngine.Reciprocal(); break;
                    case "cubesqrt": CalcEngine.CubeRoot(); break;
                    case "factor": CalcEngine.Factorial(); break;
                }
                UpdateDisplay();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
            //    if (EnterField != null)
            //        EnterField.Text = "Error";
            //}
            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            QEWindow qeWindow = new QEWindow();
            qeWindow.Owner = this;
            qeWindow.Show(); 
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.Show();
        }
    }
}
