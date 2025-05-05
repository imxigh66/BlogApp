using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class Logger : ILogger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath = "logs.txt")
        {
            _logFilePath = logFilePath;
        }

        public void LogInfo(string message)
        {
            Log($"INFO: {message}");
        }

        public void LogWarning(string message)
        {
            Log($"WARNING: {message}");
        }

        public void LogError(string message, Exception ex = null)
        {
            Log($"ERROR: {message}");
            if (ex != null)
            {
                Log($"EXCEPTION: {ex.Message}");
                Log($"STACK TRACE: {ex.StackTrace}");
            }
        }

        private void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch
            {
                // Если невозможно записать в файл, игнорируем ошибку
            }
        }
    }
}
