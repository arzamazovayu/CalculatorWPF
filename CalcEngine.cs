using System;
using System.Globalization;

namespace CalculatorWPF
{
    public class CalcEngine
    {
        public enum Operator : int
        {
            eUnknown = 0,
            eAdd = 1,
            eSubtract = 2,
            eMultiply = 3,
            eDivide = 4
        }

        private static string currentInput = "0";
        private static Operator pendingOperation = Operator.eUnknown;
        private static double firstNumber = 0;
        private static bool shouldResetInput = false;

        public static string InputNumber(string number)
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

        public static string InputDecimal()
        {
            if (shouldResetInput || currentInput == "Error")
            {
                currentInput = "0.";  // Используем точку вместо запятой
                shouldResetInput = false;
            }
            else if (!currentInput.Contains("."))  // Проверяем точку вместо запятой
            {
                currentInput += ".";  // Добавляем точку вместо запятой
            }
            return currentInput;
        }

        public static string ChangeSign()
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
            // Парсим напрямую - теперь currentInput всегда с точкой
            if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
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

        private static double PerformCalculation(double first, double second, Operator op)
        {
            switch (op)
            {
                case Operator.eAdd: return first + second;
                case Operator.eSubtract: return first - second;
                case Operator.eMultiply: return first * second;
                case Operator.eDivide: return second != 0 ? first / second : double.NaN;
                default: return second;
            }
        }

        public static string Calculate()
        {
            // Выполняем операцию только если есть pending операция и мы не в режиме сброса
            if (pendingOperation != Operator.eUnknown && !shouldResetInput)
            {
                // Парсим напрямую - теперь currentInput всегда с точкой
                if (double.TryParse(currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double secondNumber))
                {
                    double result = PerformCalculation(firstNumber, secondNumber, pendingOperation);
                    currentInput = FormatResult(result);
                }
            }

            pendingOperation = Operator.eUnknown;
            shouldResetInput = true;
            return currentInput;
        }

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

        public static void Clear()
        {
            currentInput = "0";
            shouldResetInput = false;
        }

        public static void ClearAll()
        {
            currentInput = "0";
            firstNumber = 0;
            pendingOperation = Operator.eUnknown;
            shouldResetInput = false;
        }

        public static string GetCurrentDisplay() => currentInput;
    }
}