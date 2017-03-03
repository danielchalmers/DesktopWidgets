using System.Text;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    public static class AboutHelper
    {
        public static string AboutText
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"{AssemblyInfo.Title} ({AssemblyInfo.Version})");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"Project: {Resources.GitHubMainPage}");
                stringBuilder.AppendLine($"Changes: {Resources.GitHubCommits}");
                stringBuilder.AppendLine($"Issues: {Resources.GitHubIssues}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"Icon made by {IconCredits}");
                stringBuilder.AppendLine();
                stringBuilder.Append(AssemblyInfo.Copyright);

                return stringBuilder.ToString();
            }
        }

        private static string IconCredits { get; } =
            "Freepik (http://www.freepik.com) from www.flaticon.com" +
            " is licensed under CC BY 3.0 (http://creativecommons.org/licenses/by/3.0/)";
    }
}