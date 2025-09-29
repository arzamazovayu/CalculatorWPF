using System;
using System.Globalization;

namespace CalculatorWPF
{
    public class CalcEngine
    {
        public enum Operator : int //операторы вычислений
        {
            eUnknown = 0,
            eAdd = 1,
            eSubtract = 2,
            eMultiply = 3,
            eDivide = 4,
            ePower = 5
        }

        private static string currentInput = "0";
        private static Operator pendingOperation = Operator.eUnknown; //отложенная операция
        private static double firstNumber = 0;
        private static bool shouldResetInput = false; //обновить ввод
        public static string InputNumber(string number) //ввод чисел
        {
            if (shouldResetInput || currentInput == "0" || currentInput == "Error")
            {
                currentInput = number;
                shouldResetInput = false;
            }
            else
            {
                currentInput += number;
            }
            return currentInput;
        }
        public static string InputDecimal() //ввод десятичной точки
        {
            if (shouldResetInput || currentInput == "Error")
            {
                currentInput = "0.";  
                shouldResetInput = false;
            }
            else if (!currentInput.Contains("."))  
            {
                currentInput += ".";  
            }
            return currentInput;
        }
        public static string ChangeSign() //смена знака
        {
            if (currentInput != "0" && currentInput != "Error")
            {
                if (currentInput.StartsWith("-"))
                    currentInput = currentInput.Substring(1);
                else
                    currentInput = "-" + currentInput;
            }
            return currentInput;
        }
        public static void SetOperation(Operator operation)
        {
            // Парсим текущий ввод в число
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number)) 
                //инвариантную культуру пришлось добавить из-за путаницы с запятыми и точками в разделителе 
            {
                // Если уже есть операция и мы не в режиме сброса, вычисляем её
                if (pendingOperation != Operator.eUnknown && !shouldResetInput)
                {
                    double result = PerformCalculation(firstNumber, number, pendingOperation);
                    currentInput = FormatResult(result);
                    firstNumber = result;
                }
                else
                {
                    // Сохраняем текущее число
                    firstNumber = number;
                }
            }
            // Устанавливаем новую операцию и флаг сброса
            pendingOperation = operation;
            shouldResetInput = true;
        }
        //вычисления с первым и вторым числами
        private static double PerformCalculation(double first, double second, Operator op)
        {
            switch (op)
            {
                case Operator.eAdd: return first + second;                              //сложение
                case Operator.eSubtract: return first - second;                         //вычитание
                case Operator.eMultiply: return first * second;                         //умножение
                case Operator.eDivide: return second != 0 ? first / second : double.NaN;//деление не на 0
                case Operator.ePower: return Math.Pow(first, second);                   //возведение в произвольную степень
                default: return second;
            }
        }
        //метод вычисления
        public static string Calculate()
        {
            // Выполняем операцию только если есть pending операция и мы не в режиме сброса
            if (pendingOperation != Operator.eUnknown && !shouldResetInput)
            {
                // Парсим второе число
                if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double secondNumber))
                {
                    double result = PerformCalculation(firstNumber, secondNumber, pendingOperation);
                    currentInput = FormatResult(result);
                }
            }
            //сброс состояний
            pendingOperation = Operator.eUnknown; //сброс отложенной операции
            shouldResetInput = true; //сброс ввода для нового ввода
            return currentInput;
        }
        //форматирование результата
        private static string FormatResult(double result)
        {
            if (double.IsNaN(result) || double.IsInfinity(result))
                return "Error";

            // Для целых чисел
            if (result == Math.Round(result))
                return result.ToString("0", CultureInfo.InvariantCulture);

            // Для дробных чисел используем точку (без замены)
            return result.ToString("0.############", CultureInfo.InvariantCulture);
        }
        //С
        public static void Clear()
        {
            currentInput = "0";
            shouldResetInput = false;
        }
        //СЕ
        public static void ClearAll()
        {
            currentInput = "0";
            firstNumber = 0;
            pendingOperation = Operator.eUnknown;
            shouldResetInput = false;
        }
        public static string GetCurrentDisplay() => currentInput;
        public static string Backspace()
        {
            if (currentInput == "Error" || shouldResetInput)
            {
                currentInput = "0";
                shouldResetInput = false;
            }
            else if (currentInput.Length > 1)
            {
                // Удаляем последний символ
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else
            {
                // Если осталась одна цифра, сбрасываем на 0
                currentInput = "0";
            }
            return currentInput;
        }
        //возведение в любую степень
        public static string Power()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double baseNumber))
            {
                firstNumber = baseNumber;
                pendingOperation = Operator.ePower;
                shouldResetInput = true;
                currentInput = "0";
            }
            return currentInput;
        }
        //возведение в квадрат
        public static string Square()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                double result = number * number;
                currentInput = FormatResult(result);
                shouldResetInput = true;
            }
            return currentInput;
        }
        //квадратный корень
        public static string SquareRoot()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                if (number >= 0)
                {
                    double result = Math.Sqrt(number);
                    currentInput = FormatResult(result);
                }
                else
                {
                    currentInput = "Error";
                }
                shouldResetInput = true;
            }
            return currentInput;
        }
        //1/х
        public static string Reciprocal()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                if (number != 0)
                {
                    double result = 1 / number;
                    currentInput = FormatResult(result);
                }
                else
                {
                    currentInput = "Error";
                }
                shouldResetInput = true;
            }
            return currentInput;
        }
        public static string Factorial()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                // Проверяем, что число неотрицательное и целое
                if (number >= 0 && number == Math.Round(number) && number <= 18) // 18! - максимум, иначе выходит за пределы окна
                {
                    double result = 1;
                    for (int i = 2; i <= (int)number; i++)
                    {
                        result *= i;
                    }
                    currentInput = FormatResult(result);
                }
                else if (number > 18)
                {
                    currentInput = "Too large";
                }
                else
                {
                    currentInput = "Error";
                }
                shouldResetInput = true;
            }
            return currentInput;
        }
        //кубический корень
        public static string CubeRoot()
        {
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                // Кубический корень можно извлекать из отрицательных чисел
                double result = Math.Pow(Math.Abs(number), 1.0 / 3.0);

                // Если исходное число было отрицательным, меняем знак результата
                if (number < 0)
                {
                    result = -result;
                }

                currentInput = FormatResult(result);
                shouldResetInput = true;
            }
            return currentInput;
        }
    }
}