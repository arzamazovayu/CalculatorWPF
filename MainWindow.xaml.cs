using System;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            if (EnterField != null)
                EnterField.Text = CalcEngine.GetCurrentDisplay();
        }
        private void myGrid_Click_1(object sender, RoutedEventArgs e)
        {
            var element = e.Source as FrameworkElement;
            if (element == null) return;
            // Отладочная информация
            string debugInfo = $"Кнопка: {element.Name}, Текущий ввод: {CalcEngine.GetCurrentDisplay()}";
            System.Diagnostics.Debug.WriteLine(debugInfo);
            try
            {
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
                    case "close": Application.Current.Shutdown(); break;
                }

                UpdateDisplay();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка: {ex.Message}");
                if (EnterField != null)
                    EnterField.Text = "Error";
            }

            e.Handled = true;
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Простой калькулятор\nБазовые операции: +, -, ×, ÷", "О программе");
        }

    }
}
