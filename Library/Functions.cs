using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Library
{
    public static class Functions
    {
        // Attributes
        private static readonly Printer _printer = new();
        private static readonly Random _random = new();

        // Print functions
        public static void Print(params object?[] values) => _printer.Print(values);

        public static void ConfigurePrint(string? sep = null, string? end = null, string? file = null)
        {
            if (sep is not null)
                _printer.Sep = sep;

            if (end is not null)
                _printer.End = end;

            if (file is not null)
                _printer.File = file;
        }

        // Console functions
        public static string Input()
        {
            string? output = Console.ReadLine();

            if (output is not null)
                return output;

            return String.Empty;
        }

        public static string Input(string prompt)
        {
            Console.Write(prompt);
            return Input();
        }

        public static void Pause() => Console.ReadKey();

        public static void Pause(string message)
        {
            Console.WriteLine(message);
            Pause();
        }

        public static void ClearScreen() => Console.Clear();

        // StringTo functions
        public static int StringToInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;

            throw new ArgumentException($"StringToInt(\"{value}\") cannot convert \"{value}\" to int");
        }

        public static int StringToInt(string value, int failsafe)
        {
            if (int.TryParse(value, out int result))
                return result;

            return failsafe;
        }

        public static float StringToFloat(string value)
        {
            if (float.TryParse(value, out float result))
                return result;

            throw new ArgumentException($"StringToFloat(\"{value}\") cannot convert \"{value}\" to float");
        }

        public static float StringToFloat(string value, float failsafe)
        {
            if (float.TryParse(value, out float result))
                return result;

            return failsafe;
        }

        public static double StringToDouble(string value)
        {
            if (double.TryParse(value, out double result))
                return result;

            throw new ArgumentException($"StringToDouble(\"{value}\") cannot convert \"{value}\" to double");
        }

        public static double StringToDouble(string value, double failsafe)
        {
            if (double.TryParse(value, out double result))
                return result;

            return failsafe;
        }

        // Random functions
        public static double Random() => _random.NextDouble();

        public static int RandRange(int stop) => _random.Next(stop);

        public static int RandRange(int start, int stop) => _random.Next(start, stop);

        public static int RandRange(int start, int stop, int step)
        {
            if (step <= 0)
                throw new ArgumentOutOfRangeException(nameof(step), $"Step must be greater than 0, not {step}");

            if (start >= stop)
                throw new ArgumentOutOfRangeException(nameof(start), nameof(step),
                    $"start must be less than stop, but {start} was {((start == stop) ? "equal to" : "greater than")} {stop}"
                );

            int rangeSize = (stop - start) / step;
            if (rangeSize <= 0)
                throw new ArgumentException($"The range is too small for the step, as ({stop} - {start}) / {step} == {rangeSize}");

            int stepIndex = _random.Next(rangeSize + 1);
            int result = start + (stepIndex * step);

            return result;
        }

        public static int RandInt(int a, int b) => _random.Next(a, b + 1);

        public static double Uniform(int a, int b) => a + _random.NextDouble() * (b - a);

        public static T Choice<T>(T[] array)
        {
            if (array is null)
                throw new ArgumentException("Array cannot be null", nameof(array));

            if (array.Length == 0)
                throw new ArgumentException("Array cannot be empty", nameof(array));

            return array[_random.Next(array.Length)];
        }
    }
}