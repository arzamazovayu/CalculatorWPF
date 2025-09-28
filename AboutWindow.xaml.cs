using System.Windows;

namespace CalculatorWPF
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            txtAbout.Text = "Калькулятор\n" +
                           "Версия 1.0\n"+
                           "Калькулятор с функцией инженерного и опцией вычисления корней квадратного уравнения\n" +
                           "Арзамазов А.Ю. 2025";
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}