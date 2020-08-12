using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.gRPC.Managers
{
    public class LogManager : IAnalogyLogger
    {
        private static Lazy<LogManager> _instance = new Lazy<LogManager>(() => new LogManager());

        private IAnalogyLogger Logger { get; set; }
        public static LogManager Instance { get; } = _instance.Value;
        private List<(AnalogyLogLevel level, string source, string message, string memberName, int lineNumber, string filePath)> PendingMessages { get; set; }
        public LogManager()
        {
            PendingMessages = new List<(AnalogyLogLevel level, string source, string message, string memberName, int lineNumber, string filePath)>();
        }

        public void SetLogger(IAnalogyLogger logger)
        {
            Logger = logger;
            foreach ((AnalogyLogLevel level, string source, string message, string memberName, int lineNumber, string filePath) in PendingMessages)
            {
                switch (level)
                {


                    case AnalogyLogLevel.Debug:
                        logger.LogDebug(source, message, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Event:
                        logger.LogEvent(source, message, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Warning:
                        logger.LogWarning(source, message, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Error:
                        logger.LogError(source, message, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Critical:
                        logger.LogCritical(source, message, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.AnalogyInformation:
                    case AnalogyLogLevel.Disabled:
                    case AnalogyLogLevel.Trace:
                    case AnalogyLogLevel.Verbose:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void LogEvent(string source, string message, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Event, source, message, memberName, lineNumber, filePath));
            }
            else
                Logger.LogEvent(source, message, memberName, lineNumber, filePath);
        }

        public void LogWarning(string source, string message, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Warning, source, message, memberName, lineNumber, filePath));
            }
            else
                Logger.LogWarning(source, message, memberName, lineNumber, filePath);
        }

        public void LogDebug(string source, string message, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Debug, source, message, memberName, lineNumber, filePath));
            }
            else
                Logger.LogDebug(source, message, memberName, lineNumber, filePath);
        }

        public void LogError(string source, string message, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Error, source, message, memberName, lineNumber, filePath));
            }
            else
                Logger.LogError(source, message, memberName, lineNumber, filePath);
        }

        public void LogCritical(string source, string message, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Critical, source, message, memberName, lineNumber, filePath));
            }
            else
                Logger.LogCritical(source, message, memberName, lineNumber, filePath);
        }

        public void LogException(Exception ex, string source, string message, string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Error, source, $"Error: {message.Length }Exception: {ex}", memberName, lineNumber, filePath));
            }
            else
                Logger.LogException(ex, source, message, memberName, lineNumber, filePath);
        }
    }
}
