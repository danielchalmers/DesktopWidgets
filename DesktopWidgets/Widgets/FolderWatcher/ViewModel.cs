using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly DispatcherTimer _resumeTimer;
        private FileInfo _currentFile;
        private string _currentFileContent;
        private BitmapImage _currentImage;
        private DirectoryWatcher _directoryWatcher;
        private FileType _fileType = FileType.None;
        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
            {
                return;
            }

            switch (Settings.ResumeOnStartMode)
            {
                case ResumeOnStartMode.Auto:
                    if (Settings.ResumeOnNextStart)
                    {
                        Settings.Paused = false;
                        Settings.ResumeOnNextStart = false;
                    }
                    break;
                case ResumeOnStartMode.Resume:
                    Settings.Paused = false;
                    break;
            }
            IsPaused = Settings.Paused;

            if (Settings.CurrentFile != null && !Settings.FileHistory.Contains(Settings.CurrentFile))
            {
                Settings.FileHistory.Add(Settings.CurrentFile);
            }
            CurrentFile = Settings.CurrentFile;

            OpenFile = new RelayCommand(OpenFileExecute);
            TogglePlayPause = new RelayCommand(TogglePlayPauseExecute);
            Next = new RelayCommand(NextExecute);
            Previous = new RelayCommand(PreviousExecute);

            _resumeTimer = new DispatcherTimer();
            _resumeTimer.Tick += (sender, args) => { Unpause(); };

            _directoryWatcher = new DirectoryWatcher(Settings.DirectoryWatcherSettings, AddToFileQueue);
            _directoryWatcher.Start();

            CheckFile(false);
        }

        public ICommand OpenFile { get; set; }
        public ICommand TogglePlayPause { get; set; }
        public ICommand Next { get; set; }
        public ICommand Previous { get; set; }

        public Settings Settings { get; }

        public FileInfo CurrentFile
        {
            get { return _currentFile; }
            set
            {
                _currentFile = value;
                Settings.CurrentFile = value;
                RaisePropertyChanged();
                UpdateNextPrevious();
            }
        }

        public string CurrentFileContent
        {
            get { return _currentFileContent; }
            set
            {
                _currentFileContent = value;
                RaisePropertyChanged();
            }
        }

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (!Equals(_currentImage, value))
                {
                    _currentImage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public FileType FileType
        {
            get { return _fileType; }
            set
            {
                if (_fileType != value)
                {
                    _fileType = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPaused
        {
            get { return Settings.Paused; }
            set
            {
                Settings.Paused = value;
                RaisePropertyChanged();
            }
        }

        public bool PreviousEnabled => HistoryIndex > 0;
        public bool NextEnabled => HistoryIndex < Settings.FileHistory.Count - 1;

        private int HistoryIndex => Settings.FileHistory.IndexOf(CurrentFile);

        private void UpdateNextPrevious()
        {
            RaisePropertyChanged(nameof(PreviousEnabled));
            RaisePropertyChanged(nameof(NextEnabled));
        }

        private void AddToFileQueue(List<FileInfo> paths, DirectoryChange change)
        {
            Settings.FileHistory.AddRange(paths);
            if (Settings.FileHistoryMax > 0 && Settings.FileHistory.Count > Settings.FileHistoryMax)
            {
                Settings.FileHistory.RemoveRange(0, Settings.FileHistory.Count - Settings.FileHistoryMax);
            }
            RaisePropertyChanged(nameof(Settings.FileHistory));
            UpdateNextPrevious();
            if (!Settings.QueueFiles || !_isShowing)
            {
                HandleDirectoryChange();
            }
        }

        private void CheckFile(bool playMedia = true)
        {
            FileType = FileType.None;
            CurrentFileContent = string.Empty;
            CurrentImage = null;
            if (Settings.CurrentFile == null)
            {
                return;
            }
            if (!File.Exists(Settings.CurrentFile.FullName))
            {
                FileType = FileType.Warning;
                CurrentFileContent = "File is missing";
            }
            else if (Settings.CurrentFile.Length != new FileInfo(Settings.CurrentFile.FullName).Length)
            {
                FileType = FileType.Warning;
                CurrentFileContent = "File has been modified";
            }
            else if (HandleFileImage())
            {
            }
            else if (HandleFileMedia(playMedia))
            {
                FileType = FileType.Audio;
            }
            else if (HandleFileContent())
            {
            }
            else
            {
                FileType = FileType.Other;
            }
        }

        private void HandleDirectoryChange()
        {
            if (IsPaused)
            {
                return;
            }
            if (HistoryIndex == Settings.FileHistory.Count - 1)
            {
                View?.HideUi();
                return;
            }
            _isShowing = true;
            CurrentFile = Settings.FileHistory[HistoryIndex + 1];
            CurrentFileContent = "";

            CheckFile();

            OnSpecialEvent();
        }

        private bool HandleFileImage()
        {
            if (Settings.ShowImages && ImageHelper.IsSupported(CurrentFile.Extension))
            {
                FileType = FileType.Warning;
                CurrentFileContent = "Loading...";
                Task.Run(() =>
                {
                    var path = CurrentFile.FullName;
                    var image = ImageHelper.LoadBitmapImageFromPath(path);
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            if (CurrentFile.FullName != path)
                            {
                                return;
                            }
                            CurrentImage = image;
                            FileType = FileType.Image;
                        }));
                });
                return true;
            }
            return false;
        }

        private bool HandleFileContent()
        {
            if (Settings.ShowTextContentWhitelist != null && Settings.ShowTextContentWhitelist.Count > 0 &&
                Settings.ShowTextContentWhitelist.Any(
                    x => x.EndsWith(CurrentFile.Extension, StringComparison.OrdinalIgnoreCase)) &&
                (Settings.ShowContentMaxSize <= 0 || CurrentFile.Length <= Settings.ShowContentMaxSize))
            {
                FileType = FileType.Warning;
                CurrentFileContent = "Loading...";
                Task.Run(() =>
                {
                    var path = CurrentFile.FullName;
                    var content = File.ReadAllText(CurrentFile.FullName);
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            if (CurrentFile.FullName != path)
                            {
                                return;
                            }
                            CurrentFileContent = content;
                            FileType = FileType.Text;
                        }));
                });
                return true;
            }
            return false;
        }

        private bool HandleFileMedia(bool play)
        {
            if (play && Settings.PlayMedia && MediaPlayerHelper.IsSupported(CurrentFile.Extension))
            {
                MediaPlayerStore.PlaySoundAsync(CurrentFile.FullName, Settings.PlayMediaVolume);
                return true;
            }
            return false;
        }

        private void OpenFileExecute()
        {
            if (File.Exists(CurrentFile.FullName) || Directory.Exists(CurrentFile.FullName))
            {
                ProcessHelper.Launch(CurrentFile.FullName);
            }
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                View?.Dismiss();
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            _directoryWatcher.Stop();
            _directoryWatcher = null;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _directoryWatcher.SetSettings(Settings.DirectoryWatcherSettings);
            _resumeTimer.Interval = Settings.ResumeWaitDuration;
        }

        private void TogglePlayPauseExecute()
        {
            if (IsPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            _resumeTimer?.Stop();
            Settings.ResumeOnNextStart = false;
            IsPaused = true;
        }

        private void Unpause()
        {
            _resumeTimer?.Stop();
            Settings.ResumeOnNextStart = false;
            IsPaused = false;
        }

        private void PauseAndAutoResume()
        {
            if (Settings.ResumeWaitDuration.TotalSeconds > 0)
            {
                _resumeTimer?.Stop();
                _resumeTimer?.Start();
                if (!IsPaused)
                {
                    Settings.ResumeOnNextStart = true;
                }
                IsPaused = true;
            }
        }

        private void NextExecute()
        {
            PauseAndAutoResume();
            if (NextEnabled)
            {
                CurrentFile = Settings.FileHistory[HistoryIndex + 1];
                CheckFile();
            }
        }

        private void PreviousExecute()
        {
            PauseAndAutoResume();
            if (PreviousEnabled)
            {
                CurrentFile = Settings.FileHistory[HistoryIndex - 1];
                CheckFile();
            }
        }

        public override void ExecuteSpecialAction()
        {
            base.ExecuteSpecialAction();
            _isShowing = false;
            HandleDirectoryChange();
        }

        public override void PreviewKeyDownExecute(KeyEventArgs e)
        {
            KeyDownExecute(e);
            switch (e.Key)
            {
                case Key.Left:
                    PreviousExecute();
                    e.Handled = true;
                    break;
                case Key.Right:
                    NextExecute();
                    e.Handled = true;
                    break;
                case Key.Space:
                    TogglePlayPauseExecute();
                    e.Handled = true;
                    break;
            }
        }
    }
}