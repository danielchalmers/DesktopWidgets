using System;
using System.IO;
using DesktopWidgets.Properties;
using Newtonsoft.Json;

namespace DesktopWidgets.Helpers
{
    public static class ExceptionHelper
    {
        private static readonly string ExceptionDumpPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.AppName);

        public static void SaveException(Exception ex)
        {
            try
            {
                if (!Settings.Default.DumpUnhandledErrors)
                {
                    return;
                }
                var serialised = JsonConvert.SerializeObject(ex, SettingsHelper.JsonSerializerSettingsAllTypeHandling);
                var path = Path.Combine(ExceptionDumpPath, $"error-{Guid.NewGuid()}.json");
                FileSystemHelper.WriteTextToFile(path, serialised);
            }
            catch
            {
                // ignored
            }
        }
    }
}