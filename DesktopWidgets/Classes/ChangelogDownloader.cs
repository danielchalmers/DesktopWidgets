﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using DesktopWidgets.ApiClasses;
using DesktopWidgets.Properties;
using Newtonsoft.Json;

namespace DesktopWidgets.Classes
{
    public class ChangelogDownloader
    {
        private static readonly List<string> ChangelogBlacklist = new List<string>
        {
            "refactor",
            "cleanup",
            "clean up"
        };

        private string _updateText;
        private Action<string> _updateTextAction;

        private bool _useCache;

        public void GetChangelog(Action<string> updateTextAction, bool useCache = true)
        {
            _useCache = useCache;
            _updateTextAction = updateTextAction;
            var worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_Completed;
            worker.RunWorkerAsync();
            _updateTextAction("Downloading changelog...");
        }

        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            _updateTextAction(_updateText);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if (Settings.Default.DisableChangelog)
            {
                _updateText = "Changelog is disabled.";
                return;
            }
            try
            {
                _updateText = GetFormattedChangelog();
            }
            catch
            {
                _updateText = "Changelog failed to download.";
            }
        }

        private string GetFormattedChangelog()
        {
            List<Changelog> changelogData = null;
            if (_useCache && !string.IsNullOrWhiteSpace(Settings.Default.ChangelogCache))
            {
                changelogData = JsonConvert.DeserializeObject<List<Changelog>>(Settings.Default.ChangelogCache);
            }
            if (!(changelogData != null && changelogData.Any(x => x.Version == AssemblyInfo.Version)))
            {
                changelogData = ParseChangelogJson(DownloadAllChangelogPages()).ToList();
                Settings.Default.ChangelogCache = JsonConvert.SerializeObject(changelogData);
            }

            var stringBuilder = new StringBuilder();
            foreach (var changelog in changelogData.OrderByDescending(x => x.PublishDate))
            {
                stringBuilder.Append($"{changelog.Version} ({changelog.PublishDate.ToString("yyyy-MM-dd")})");
                foreach (
                    var change in
                        changelog.History.OrderBy(x => x)
                            .Where(c => !ChangelogBlacklist.Any(x => c.ToLower().Contains(x))))
                {
                    stringBuilder.Append($"{Environment.NewLine} {change}");
                }
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            stringBuilder.Append($"View the full changelog at {Resources.GitHubCommits}");
            return stringBuilder.ToString();
        }

        private static IEnumerable<Changelog> ParseChangelogJson(IEnumerable<GitHubApiCommitsRootObject> json)
        {
            var history = new List<string>();
            foreach (var j in json.Reverse())
            {
                var fullCommit = j.commit.message?.Split('\n');
                var commit = fullCommit != null && fullCommit.Length > 0 ? fullCommit[0] : string.Empty;
                Version version;
                if (Version.TryParse(commit, out version))
                {
                    yield return new Changelog(
                        version,
                        DateTime.Parse(j.commit.committer.date),
                        history.ToList());
                    history.Clear();
                }
                else
                {
                    history.Add(commit);
                }
            }
        }

        private IEnumerable<GitHubApiCommitsRootObject> DownloadAllChangelogPages()
        {
            var returnData = new List<GitHubApiCommitsRootObject>();
            for (var i = 1; i <= Settings.Default.ChangelogDownloadPages; i++)
            {
                var i1 = i;
                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        _updateTextAction(
                            $"Downloading changelog ({i1} of {Settings.Default.ChangelogDownloadPages})...");
                    });
                returnData.AddRange(
                    JsonConvert.DeserializeObject<IEnumerable<GitHubApiCommitsRootObject>>(DownloadChangelogJson(i1)));
            }
            return returnData;
        }

        private static string DownloadChangelogJson(int page = 1)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("User-Agent: Other");
                var json = webClient.DownloadString($"{Resources.GitHubApiCommits}?page={page}");
                return json;
            }
        }
    }

    public class Changelog
    {
        public Changelog(Version version, DateTime publishDate, List<string> history)
        {
            Version = version;
            PublishDate = publishDate;
            History = history;
        }

        public Version Version { get; }
        public DateTime PublishDate { get; }
        public List<string> History { get; }
    }
}