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
    public static class Methods
    {
        #region Input/Output
        private static readonly Printer _printer = new();

        public static void Print(params object?[] values) => _printer.Print(values);

        public static void ConfigurePrint(string? sep = null, string? end = null, string? file = null) => _printer.Configure(sep, end, file);

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

        public static T? Input<T>() => Input().To<T>();

        public static T? Input<T>(string prompt) => Input(prompt).To<T>();

        public static T? Input<T>(T defaultValue) => Input().To<T>(defaultValue);

        public static T? Input<T>(string prompt, T defaultValue) => Input(prompt).To<T>(defaultValue);

        public static T? InputLoop<T>()
        {
            while (true)
            {
                try
                {
                    return Input<T>();
                }
                catch (InvalidCastException)
                {
                    continue;
                }
            }
        }

        public static T? InputLoop<T>(string prompt, bool repeatPrompt = true)
        {
            while (true)
            {
                if (!repeatPrompt)
                    Console.Write(prompt);

                try
                {
                    if (repeatPrompt)
                        Console.Write(prompt);

                    return Input<T>();
                }
                catch (InvalidCastException)
                {
                    continue;
                }
            }
        }

        public static T? InputLoop<T>(string prompt, string failureMessage, bool repeatPrompt = true)
        {
            while (true)
            {
                if (!repeatPrompt)
                    Console.Write(prompt);

                try
                {
                    if (repeatPrompt)
                        Console.Write(prompt);

                    return Input<T>();
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine(failureMessage);
                }
            }
        }

        public static void Pause() => Console.ReadKey();

        public static void Pause(string message)
        {
            Console.WriteLine(message);
            Pause();
        }

        public static void ClearScreen() => Console.Clear();
        #endregion

        #region Type casting
        public static T? To<T>(this object value)
        {
            if (value is null)
                return default;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new InvalidCastException($"Cannot convert {value} to {typeof(T)}.");
            }
        }

        public static T? To<T>(this object value, T? defaultValue)
        {
            try
            {
                return value.To<T>();
            }
            catch (InvalidCastException)
            {
                if (defaultValue is not null)
                    return defaultValue;

                return default;
            }
        }
        #endregion

        #region Random
        private static readonly Random _random = new();

        public static double RandomDouble() => _random.NextDouble();

        public static int RandomRange(int stop) => _random.Next(stop);

        public static int RandomRange(int start, int stop) => _random.Next(start, stop);

        public static int RandomRange(int start, int stop, int step)
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

        public static int RandomInt(int a, int b) => _random.Next(a, b + 1);

        public static double RandomUniform(int a, int b) => a + _random.NextDouble() * (b - a);

        public static T RandomChoice<T>(IList<T> collection)
        {
            if (collection is null)
                throw new ArgumentException("collection cannot be null.", nameof(collection));

            int length = collection.Count;
            if (length == 0)
                throw new ArgumentException("collection cannot be empty.", nameof(collection));

            return collection[_random.Next(length)];
        }

        public static void RandomShuffle<T>(IList<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), "collection cannot be null.");

            int length = collection.Count;
            for (int i = 0; i < (length - 1); i++)
            {
                int randomIndex = i + _random.Next(length - i);
                T temporaryElement = collection[randomIndex];
                collection[randomIndex] = collection[i];
                collection[i] = temporaryElement;
            }
        }
        #endregion
    }
}