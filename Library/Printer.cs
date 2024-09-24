using System.Text;

namespace Library
{
    public class Printer
    {
        // _nullRepresentation will Print instead of "" when null is passed
        private static readonly string _nullRepresentation = "null";

        // If File == _consoleRepresentation, uses Console.Out instead
        private static readonly string _consoleRepresentation = "Console";

        // Sep is the string that joins the values together
        public string Sep;
        private string _defaultSep;
        public string DefaultSep
        {
            get => _defaultSep;
            set
            {
                _defaultSep = value;
                Sep = value; // Setting DefaultSep will update Sep too
            }
        }

        // End is the final string of the output
        public string End;
        private string _defaultEnd;
        public string DefaultEnd
        {
            get => _defaultEnd;
            set
            {
                _defaultEnd = value;
                End = value; // Setting DefaultEnd will update End too
            }
        }

        // File is the file path that the output is printed to
        public string File;
        private string _defaultFile;
        public string DefaultFile
        {
            get => _defaultFile;
            set
            {
                _defaultFile = value;
                File = value; // Setting DefaultFile will update File too
                SetWriter(); // New _writer upon updating DefaultFile
            }
        }

        /* _stringBuilder is only for GetOutput method, 
         * but is an attribute to avoid constant instantiation */
        private readonly StringBuilder _stringBuilder = new();

        /* _writer is used to write to files when the default file is
         * not Console.Out to stop repeated file opening
         * Is not used when the file is changed temporarily */
        private StreamWriter? _writer;

        public Printer(string? sep = null, string? end = null, string? file = null)
        {
            /* Define default arguments here because:
             * Environment.NewLine and _consoleRepresentation
             * cannot be used as a default argument */
            string defaultArgumentSep = " ";
            string defaultArgumentEnd = Environment.NewLine;
            string defaultArgumentFile = _consoleRepresentation;

            Sep = sep ?? defaultArgumentSep;
            _defaultSep = Sep;

            End = end ?? defaultArgumentEnd;
            _defaultEnd = End;

            File = file ?? defaultArgumentFile;
            _defaultFile = File;

            SetWriter();
        }

        private void AddToStringBuilder(object? value)
        {
            if (value is not null)
                _stringBuilder.Append(value);
            else
                _stringBuilder.Append(_nullRepresentation);

            _stringBuilder.Append(Sep);
        }

        private void EndStringBuilder()
        {
            _stringBuilder.Length -= Sep.Length; // Removes the last Sep
            _stringBuilder.Append(End);
        }

        private string GetStringBuilderResult()
        {
            string result = _stringBuilder.ToString();

            // Clear the StringBuilder after every new output
            _stringBuilder.Clear();

            return result;
        }

        private string GetOutput(object?[] values)
        {
            if (values is null) // Sent null only
                return _nullRepresentation + End;

            if (values.Length == 0) // Sent nothing
                return End;

            foreach (object? value in values)
            {
                AddToStringBuilder(value);
            }

            EndStringBuilder();

            return GetStringBuilderResult();
        }

        private void ResetAttributes()
        {
            Sep = _defaultSep;
            End = _defaultEnd;
            File = _defaultFile;
        }

        private void SetWriter()
        {
            _writer?.Dispose(); // Delete previous writer if exists

            Stream stream;
            if (File == _consoleRepresentation)
                stream = Console.OpenStandardOutput();
            else
                stream = new FileStream(_defaultFile, FileMode.Append, FileAccess.Write, FileShare.Read);

            _writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };
        }

        private void Write(string value)
        {
            // Use the _writer if the File is the same as the DefaultFile
            if (File == _defaultFile)
            {
                _writer.Write(value);
                return;
            }

            // Write to the console if the File is set to it
            if (File == _consoleRepresentation)
            {
                Console.Write(value);
                return;
            }

            // Write to the set File otherwise
            System.IO.File.AppendAllText(File, value);
        }

        public void Print(params object?[] values)
        {
            string output = GetOutput(values);
            Write(output);
            ResetAttributes();
        }
    }
}