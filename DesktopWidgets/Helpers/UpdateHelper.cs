using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;
using DesktopWidgets.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace DesktopWidgets.Helpers
{
    internal static class UpdateHelper
    {
        public static bool IsUpdateable => ApplicationDeployment.IsNetworkDeployed;
        private static Version ForgetUpdateVersion => Settings.Default.ForgetUpdateVersion ?? new Version(0, 0, 0, 0);

        public static bool IsUpdateDay
            =>
                Settings.Default.UpdateDays.ToLower() == "all" ||
                Settings.Default.UpdateDays.Contains(DateTime.Today.DayOfWeek.ToString());

        public static void CheckForUpdatesAsync(bool auto)
        {
            if (auto && (App.IsMuted || !IsUpdateDay || FullScreenHelper.DoesAnyMonitorHaveFullscreenApp()))
            {
                return;
            }

            UpdateCheckInfo updateInfo = null;
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    updateInfo = ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate(false);
                }
            };
            bw.RunWorkerCompleted += (sender, args) => CheckForUpdates(updateInfo, auto);
            bw.RunWorkerAsync();
        }

        private static void CheckForUpdates(UpdateCheckInfo info, bool auto)
        {
            try
            {
                if (!ApplicationDeployment.IsNetworkDeployed)
                {
                    if (!auto)
                    {
                        Popup.Show(
                            "This application was not installed via ClickOnce and cannot be updated automatically.",
                            image: MessageBoxImage.Error);
                    }
                    return;
                }

                if (info == null)
                {
                    if (!auto)
                    {
                        Popup.Show(
                            "An error occurred while trying to check for updates.", image: MessageBoxImage.Error);
                    }
                    return;
                }

                Settings.Default.LastUpdateCheck = DateTime.Now;
                if (info.UpdateAvailable && !(Settings.Default.UpdateBranch != UpdateBranch.Beta &&
                                              info.AvailableVersion.Minor == Settings.Default.BetaVersion) &&
                    !(AssemblyInfo.Version.Major != ForgetUpdateVersion.Major &&
                      ForgetUpdateVersion.Major == info.AvailableVersion.Major))
                {
                    if (auto && info.AvailableVersion == ForgetUpdateVersion)
                    {
                        return;
                    }
                    var ad = ApplicationDeployment.CurrentDeployment;
                    ad.UpdateCompleted += delegate
                    {
                        var args = new List<string>();
                        if (auto && Settings.Default.AutoUpdate)
                        {
                            args.Add("updatingsilent");
                        }
                        else
                        {
                            args.Add("updating");
                        }
                        AppHelper.RestartApplication(args);
                    };
                    if (auto && Settings.Default.AutoUpdate)
                    {
                        try
                        {
                            ad.UpdateAsync();
                        }
                        catch
                        {
                            // ignored
                        }
                        return;
                    }

                    if (auto)
                    {
                        if (Settings.Default.UpdateWaiting != info.AvailableVersion)
                        {
                            Settings.Default.UpdateWaiting = info.AvailableVersion;
                            TrayIconHelper.ShowBalloon(
                                $"Update available ({info.AvailableVersion})\nClick to view details",
                                BalloonIcon.Info);
                        }
                    }
                    else
                    {
                        App.UpdateScheduler?.Stop();

                        var updateDialog = new UpdatePrompt(info.AvailableVersion, info.IsUpdateRequired);
                        updateDialog.ShowDialog();
                        updateDialog.Activate();

                        switch (updateDialog.SelectedUpdateMode)
                        {
                            case UpdatePrompt.UpdateMode.RemindNever:
                                Settings.Default.ForgetUpdateVersion = info.AvailableVersion;
                                break;
                            case UpdatePrompt.UpdateMode.UpdateNow:
                                try
                                {
                                    var progressDialog = new UpdateProgress(info.AvailableVersion);
                                    ad.UpdateProgressChanged +=
                                        delegate (object sender, DeploymentProgressChangedEventArgs args)
                                        {
                                            if (args.State == DeploymentProgressState.DownloadingApplicationFiles)
                                            {
                                                progressDialog.CurrentProgress = args.ProgressPercentage;
                                            }
                                        };
                                    progressDialog.Show();
                                    progressDialog.Activate();
                                    ad.UpdateAsync();
                                }
                                catch (DeploymentDownloadException dde)
                                {
                                    Popup.Show(
                                        $"Failed to download the latest version.\nPlease check your network connection, or try again later.\n\nError: {dde}",
                                        image: MessageBoxImage.Error);
                                }
                                break;
                        }

                        App.UpdateScheduler?.Start();
                    }
                }
                else
                {
                    if (!auto)
                    {
                        Popup.Show($"You have the latest version ({AssemblyInfo.Version}).");
                    }
                }
            }
            catch (Exception)
            {
                if (!auto)
                {
                    throw;
                }
            }
        }

        public static void HandleUpdate()
        {
            if (Settings.Default.UpdateWaiting != null)
            {
                Settings.Default.UpdateWaiting = null;
                CheckForUpdatesAsync(false);
            }
        }
    }
}