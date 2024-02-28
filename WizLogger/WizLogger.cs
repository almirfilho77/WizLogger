using System.Runtime.CompilerServices;

namespace WizLog;

public class WizLogger : IDisposable
{
    public enum LogLevel : int 
    { 
        DEBUG = 0, INFO = 1, WARNING = 2, ERROR = 3, FATAL = 4 
    };

    public static void Initialize(string filepath, LogLevel log_level)
    {
        if (_Instance == null)
        {
            try
            {
                _Instance = new Lazy<WizLogger>(() => new WizLogger(filepath, log_level));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public static WizLogger GetLogger()
    {
        if (_Instance == null)
        {
            try
            {
                _Instance = new Lazy<WizLogger>( () => new WizLogger("log.txt", LogLevel.INFO) );
            }
            catch (Exception)
            {
                throw;
            }
        }
        return _Instance.Value;
    }

    public static void Debug(string message,
        [CallerFilePath] string filepath = "",
        [CallerMemberName] string functionName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (_Instance != null)
        {
            _Instance.Value.WriteDebug($"{message}", Path.GetFileName(filepath), functionName, lineNumber);
        }
    }

    public static void Info(string message,
        [CallerFilePath] string filepath = "",
        [CallerMemberName] string functionName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (_Instance != null)
        {
            _Instance.Value.WriteInfo($"{message}", Path.GetFileName(filepath), functionName, lineNumber);
        }
    }

    public static void Warning(string message,
        [CallerFilePath] string filepath = "",
        [CallerMemberName] string functionName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (_Instance != null)
        {
            _Instance.Value.WriteWarning($"{message}", Path.GetFileName(filepath), functionName, lineNumber);
        }
    }

    public static void Error(string message,
        [CallerFilePath] string filepath = "",
        [CallerMemberName] string functionName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (_Instance != null)
        {
            _Instance.Value.WriteError($"{message}", Path.GetFileName(filepath), functionName, lineNumber);
        }
    }

    public static void Fatal(string message,
        [CallerFilePath] string filepath = "",
        [CallerMemberName] string functionName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (_Instance != null)
        {
            _Instance.Value.WriteFatal($"{message}", Path.GetFileName(filepath), functionName, lineNumber);
        }
    }

    private static Lazy<WizLogger>? _Instance = null;
    private readonly StreamWriter? _FileWriter = null;
    private bool _disposed = false;
    
    // Logger related
    private string _filepath = "log.txt";

#if DEBUG
    private LogLevel _logLevel = LogLevel.DEBUG;
#else
    private LogLevel _logLevel = LogLevel.ERROR;
#endif
    private WizLogger(string filepath, LogLevel logLevel)
    {
        _filepath = filepath;
        _logLevel = logLevel;
        _FileWriter = new StreamWriter(_filepath);
        AppDomain.CurrentDomain.ProcessExit += TerminateWithProgram;
    }

    //TODO: Add error handling
    private void WriteDebug(string message, string filename, string functionName, int lineNumber)
    {
        if (_logLevel <= LogLevel.DEBUG)
        {
            _FileWriter?.WriteLine($"[DEBUG] {filename} : {functionName}({lineNumber}) > {message}");
        }
    }

    //TODO: Add error handling
    private void WriteInfo(string message, string filename, string functionName, int lineNumber)
    {
        if (_logLevel <= LogLevel.INFO)
        {
            _FileWriter?.WriteLine($"[INFO] {filename} : {functionName}({lineNumber}) > {message}");
        }
    }

    //TODO: Add error handling
    private void WriteWarning(string message, string filename, string functionName, int lineNumber)
    {
        if (_logLevel <= LogLevel.WARNING)
        {
            _FileWriter?.WriteLine($"[WARNING] {filename} : {functionName}({lineNumber}) > {message}");
        }
    }

    //TODO: Add error handling
    private void WriteError(string message, string filename, string functionName, int lineNumber)
    {
        if (_logLevel <= LogLevel.ERROR)
        {
            _FileWriter?.WriteLine($"[ERROR] {filename} : {functionName}({lineNumber}) > {message}");
        }
    }

    //TODO: Add error handling
    private void WriteFatal(string message, string filename, string functionName, int lineNumber)
    {
        if (_logLevel <= LogLevel.FATAL)
        {
            _FileWriter?.WriteLine($"[FATAL] {filename} : {functionName}({lineNumber}) > {message}");
        }
    }

    ~WizLogger()
    {
        Dispose();
    }

    private void TerminateWithProgram(object? sender, EventArgs e)
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_Instance != null && !_disposed)
        {
            Console.WriteLine("Disposing logger");
            if (_FileWriter != null)
            {
                _FileWriter.Flush();
                _FileWriter.Close();
                _FileWriter.Dispose();
            }
            _disposed = true;
        }
    }
}
