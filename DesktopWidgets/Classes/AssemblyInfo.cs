#region

using System;
using System.Deployment.Application;
using System.Reflection;
using System.Runtime.InteropServices;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Classes
{
    internal static class AssemblyInfo
    {
        public static Version Version { get; } = ApplicationDeployment.IsNetworkDeployed
            ? ApplicationDeployment.CurrentDeployment.CurrentVersion
            : Assembly.GetExecutingAssembly().GetName().Version;

        public static Version ActualVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public static string Copyright { get; } = GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);
        public static string Title { get; } = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title);

        public static string Description { get; } =
            GetAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description);

        public static string CustomDescription { get; } =
            string.Format(Resources.About,
                Title, Version, Copyright,
                Resources.GitHubMainPage,
                Resources.GitHubCommits,
                Resources.GitHubIssues,
                Resources.IconCredit);

        public static string Guid { get; } = GetAssemblyAttribute<GuidAttribute>(a => a.Value);

        private static string GetAssemblyAttribute<T>(Func<T, string> value)
            where T : Attribute
        {
            var attribute = (T) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }
    }
}