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
                        logger.LogDebug(message,source, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Information:
                        logger.LogInformation(message, source, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Warning:
                        logger.LogWarning(message, source, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Error:
                        logger.LogError(message, source, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Critical:
                        logger.LogCritical(message, source, memberName, lineNumber, filePath);
                        break;
                    case AnalogyLogLevel.Analogy:
                    case AnalogyLogLevel.None:
                    case AnalogyLogLevel.Trace:
                    case AnalogyLogLevel.Verbose:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void LogInformation(string message, string source, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Information, source, message, memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogInformation(message, source, memberName, lineNumber, filePath);
            }
        }

        public void LogWarning(string message, string source, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Warning, source, message, memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogWarning(message, source, memberName, lineNumber, filePath);
            }
        }

        public void LogDebug(string message, string source, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Debug, source, message, memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogDebug(message, source, memberName, lineNumber, filePath);
            }
        }

        public void LogError(string message, string source, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Error, source, message, memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogError(message, source, memberName, lineNumber, filePath);
            }
        }

        public void LogCritical(string message, string source, string memberName = "", int lineNumber = 0, string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Critical, source, message, memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogCritical(message, source, memberName, lineNumber, filePath);
            }
        }

        public void LogException(string message,Exception ex, string source , string memberName = "", int lineNumber = 0,
            string filePath = "")
        {
            if (Logger == null)
            {
                PendingMessages.Add((AnalogyLogLevel.Error, source, $"Error: {message.Length }Exception: {ex}", memberName, lineNumber, filePath));
            }
            else
            {
                Logger.LogException(message,ex, source,  memberName, lineNumber, filePath);
            }
        }
    }
}
