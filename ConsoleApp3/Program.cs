using System;
using System.Collections.Generic;
using System.Globalization;

namespace PhysicalUnitsConverter
{
    public interface ICommand
    {
        void Execute();
    }

    public class AddConversionFactorCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Введіть нову одиницю вимірювання 1:");
            string newUnit1 = Console.ReadLine();

            Console.WriteLine("Введіть нову одиницю вимірювання 2:");
            string newUnit2 = Console.ReadLine();

            double newConversionFactor = ReadConversionFactor();

            if (newConversionFactor != 0)
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

        private double ReadConversionFactor()
        {
            Console.WriteLine("Введіть новий коефіцієнт конвертації (наприклад, для переведення одиниці 1 в одиницю 2 введіть 0.001):");
            double factor;
            while (!double.TryParse(Console.ReadLine().Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out factor))
            {
                Console.WriteLine("Некоректний коефіцієнт. Будь ласка, введіть числове значення.");
            }
            return factor;
        }
    }

    public class ConvertUnitsCommand : ICommand
    {
        public void Execute()
        {
            double inputValue = ReadValue("Введіть значення:");

            string unit1 = ReadUnit("Введіть одиницю вимірювання 1 (наприклад, meter):");
            string unit2 = ReadUnit("Введіть одиницю вимірювання 2 (наприклад, kilometer):");

            if (Converter.TryGetConversionFactor(unit1, unit2, out double conversionFactor))
            {
                double convertedValue = Converter.Convert(inputValue, unit1, unit2);
                Console.WriteLine($"Результат конвертації: {convertedValue} {unit2}");
            }
            else
            {
                Console.WriteLine($"Не визначений коефіцієнт для переведення від {unit1} до {unit2}. Будь ласка, додайте його.");
                new AddConversionFactorCommand().Execute();
            }

            Console.WriteLine("Натисніть Enter для продовження.");
            Console.ReadLine();
        }

        private double ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            double value;
            while (!double.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
            {
                Console.WriteLine("Некоректне значення. Будь ласка, введіть числове значення.");
            }
            return value;
        }

        private string ReadUnit(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }

    public class Converter
    {
        public static Dictionary<string, double> ConversionFactors = new Dictionary<string, double>();

        public static void AddConversionFactor(string unit1, string unit2, double factor)
        {
            string key1 = $"{unit1}_{unit2}";
            string key2 = $"{unit2}_{unit1}";

            if (!ConversionFactors.ContainsKey(key1))
            {
                ConversionFactors[key1] = factor;
            }

            if (!ConversionFactors.ContainsKey(key2))
            {
                ConversionFactors[key2] = 1 / factor;
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
                        new ConvertUnitsCommand().Execute();
                        break;
                    case "2":
                        new AddConversionFactorCommand().Execute();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір опції.");
                        break;
                }
            }
        }
    }
}
