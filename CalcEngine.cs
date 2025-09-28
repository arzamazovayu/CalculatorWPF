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

        // Для парсинга используем инвариантную культуру (с точкой)
        private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;

        public static string InputNumber(string number)
        {
            System.Diagnostics.Debug.WriteLine($"InputNumber: currentInput='{currentInput}', shouldResetInput={shouldResetInput}, number={number}");

            if (shouldResetInput || currentInput == "0" || currentInput == "Error")
            {
                currentInput = number;
                shouldResetInput = false;
                System.Diagnostics.Debug.WriteLine($"InputNumber: RESET input to '{currentInput}'");
            }
            else
            {
                currentInput += number;
                System.Diagnostics.Debug.WriteLine($"InputNumber: APPENDED to '{currentInput}'");
            }

            return currentInput;
        }

        public static string InputDecimal()
        {
            System.Diagnostics.Debug.WriteLine($"InputDecimal: currentInput='{currentInput}', shouldResetInput={shouldResetInput}");

            if (shouldResetInput || currentInput == "Error")
            {
                currentInput = "0,";
                shouldResetInput = false;
                System.Diagnostics.Debug.WriteLine($"InputDecimal: RESET to '{currentInput}'");
            }
            else if (!currentInput.Contains(","))
            {
                currentInput += ",";
                System.Diagnostics.Debug.WriteLine($"InputDecimal: ADDED decimal to '{currentInput}'");
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
            System.Diagnostics.Debug.WriteLine($"SetOperation START: currentInput='{currentInput}', shouldResetInput={shouldResetInput}, pendingOperation={pendingOperation}, newOperation={operation}");

            // Заменяем запятую на точку для парсинга
            string inputForParsing = currentInput.Replace(",", ".");

            if (double.TryParse(inputForParsing, NumberStyles.Any, invariantCulture, out double number))
            {
                // Если уже есть операция и мы не в режиме сброса, вычисляем её
                if (pendingOperation != Operator.eUnknown && !shouldResetInput)
                {
                    System.Diagnostics.Debug.WriteLine($"SetOperation: Performing calculation {firstNumber} {pendingOperation} {number}");
                    double result = PerformCalculation(firstNumber, number, pendingOperation);
                    currentInput = FormatResult(result);
                    firstNumber = result;
                    System.Diagnostics.Debug.WriteLine($"SetOperation: Calculated result={result}, currentInput='{currentInput}'");
                }
                else
                {
                    // Сохраняем текущее число
                    firstNumber = number;
                    System.Diagnostics.Debug.WriteLine($"SetOperation: Saved firstNumber={firstNumber}");
                }
            }

            // Устанавливаем новую операцию и флаг сброса
            pendingOperation = operation;
            shouldResetInput = true;

            System.Diagnostics.Debug.WriteLine($"SetOperation END: firstNumber={firstNumber}, pendingOperation={pendingOperation}, shouldResetInput={shouldResetInput}");
        }

        private static double PerformCalculation(double first, double second, Operator op)
        {
            System.Diagnostics.Debug.WriteLine($"PerformCalculation: {first} {op} {second}");

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
            System.Diagnostics.Debug.WriteLine($"Calculate START: currentInput='{currentInput}', pendingOperation={pendingOperation}, firstNumber={firstNumber}, shouldResetInput={shouldResetInput}");

            // Выполняем операцию только если есть pending операция и мы не в режиме сброса
            if (pendingOperation != Operator.eUnknown && !shouldResetInput)
            {
                // Заменяем запятую на точку для парсинга
                string inputForParsing = currentInput.Replace(",", ".");

                if (double.TryParse(inputForParsing, NumberStyles.Any, invariantCulture, out double secondNumber))
                {
                    double result = PerformCalculation(firstNumber, secondNumber, pendingOperation);
                    currentInput = FormatResult(result);
                    System.Diagnostics.Debug.WriteLine($"Calculate: {firstNumber} {pendingOperation} {secondNumber} = {result}");
                }
            }

            pendingOperation = Operator.eUnknown;
            shouldResetInput = true;

            System.Diagnostics.Debug.WriteLine($"Calculate END: currentInput='{currentInput}', shouldResetInput={shouldResetInput}");
            return currentInput;
        }

        private static string FormatResult(double result)
        {
            if (double.IsNaN(result) || double.IsInfinity(result))
                return "Error";

            // Для целых чисел
            if (result == Math.Round(result))
                return result.ToString("0");

            // Для дробных чисел форматируем с точкой, затем заменяем на запятую
            string formatted = result.ToString("0.############", invariantCulture);
            return formatted.Replace(".", ",");
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