using System;
using System.Collections.Generic;
using System.Globalization;

namespace PhysicalUnitsConverter
{
    public class Converter
    {
        public static Dictionary<string, double> ConversionFactors = new Dictionary<string, double>();

        public static void AddConversionFactor(string unit1, string unit2, double factor)
        {
            string key = $"{unit1}_{unit2}";
            if (!ConversionFactors.ContainsKey(key))
            {
                ConversionFactors[key] = factor;
            }
        }

        public static double Convert(double value, string unit1, string unit2)
        {
            string key = $"{unit1}_{unit2}";
            return ConversionFactors.TryGetValue(key, out double conversionFactor) ? value * conversionFactor : value;
        }

        public static bool TryGetConversionFactor(string unit1, string unit2, out double conversionFactor)
        {
            string key1 = $"{unit1}_{unit2}";
            string key2 = $"{unit2}_{unit1}";
            return ConversionFactors.TryGetValue(key1, out conversionFactor) || ConversionFactors.TryGetValue(key2, out conversionFactor);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("   Головне меню");
                Console.WriteLine();
                Console.WriteLine("1. Ввести дані для конвертації");
                Console.WriteLine("2. Додати нову одиницю вимірювання та коефіцієнт");
                Console.WriteLine("3. Вихід");

                Console.Write("Виберіть опцію (1, 2 або 3): ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ConvertUnits();
                        break;
                    case "2":
                        AddNewConversionFactor();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір опції.");
                        break;
                }
            }
        }

        static void ConvertUnits()
        {
            Console.WriteLine("Введіть значення:");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double inputValue))
            {
                Console.WriteLine("Некоректне значення. Будь ласка, введіть числове значення.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Введіть одиницю вимірювання 1 (наприклад, meter):");
            string unit1 = Console.ReadLine();

            Console.WriteLine("Введіть одиницю вимірювання 2 (наприклад, kilometer):");
            string unit2 = Console.ReadLine();

            if (Converter.TryGetConversionFactor(unit1, unit2, out double conversionFactor))
            {
                double convertedValue = Converter.Convert(inputValue, unit1, unit2);
                Console.WriteLine($"Результат конвертації: {convertedValue} {unit2}");
            }
            else
            {
                Console.WriteLine($"Не визначений коефіцієнт для переведення від {unit1} до {unit2}. Будь ласка, додайте його.");
                AddNewConversionFactor();
            }

            Console.WriteLine("Натисніть Enter для продовження.");
            Console.ReadLine();
        }

        static void AddNewConversionFactor()
        {
            Console.WriteLine("Введіть нову одиницю вимірювання 1:");
            string newUnit1 = Console.ReadLine();

            Console.WriteLine("Введіть нову одиницю вимірювання 2:");
            string newUnit2 = Console.ReadLine();

            Console.WriteLine("Введіть новий коефіцієнт конвертації (наприклад, для переведення одиниці 1 в одиницю 2 введіть 0.001):");

            if (double.TryParse(Console.ReadLine().Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double newConversionFactor))
            {
                Converter.AddConversionFactor(newUnit1, newUnit2, newConversionFactor);
                Console.WriteLine($"Додано новий коефіцієнт для конвертації від {newUnit1} до {newUnit2}.");
            }
            else
            {
                Console.WriteLine("Некоректний коефіцієнт. Будь ласка, введіть числове значення.");
            }

            Console.WriteLine("Натисніть Enter для продовження.");
            Console.ReadLine();
        }
    }
}
