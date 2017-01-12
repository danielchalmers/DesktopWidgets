using System;
using System.IO;
using DesktopWidgets.Properties;
using Newtonsoft.Json;

namespace DesktopWidgets.Helpers
{
    public static class ExceptionHelper
    {
        private static readonly string ExceptionDumpPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.AppName, "Errors");

        public static void SaveException(Exception ex)
        {
            try
            {
                if (!Settings.Default.DumpUnhandledErrors)
                {
                    return;
                }
                var serialised = JsonConvert.SerializeObject(ex, SettingsHelper.JsonSerializerSettingsAllTypeHandling);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var path = Path.Combine(ExceptionDumpPath, $"error-{timestamp}.json");
                if (!Directory.Exists(ExceptionDumpPath))
                {
                    Directory.CreateDirectory(ExceptionDumpPath);
                }
                File.WriteAllText(path, serialised);
            }
            catch
            {
                // ignored
            }
        }
    }
}