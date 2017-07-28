using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for UpdatePrompt.xaml
    /// </summary>
    public partial class UpdatePrompt : Window, INotifyPropertyChanged
    {
        public enum UpdateMode
        {
            UpdateNow,
            RemindLater,
            RemindNever
        }

        private string _changelogText;

        private bool _updateIsRequired;

        private string _updateSubText;

        private Version _updateVersion;

        public UpdatePrompt(Version updateVersion, bool isRequired)
        {
            InitializeComponent();

            UpdateIsRequired = isRequired;

            UpdateText =
                (UpdateIsRequired ? "An important update, " : "") +
                $"{AssemblyInfo.Title} {updateVersion} is out!";

            DataContext = this;

            new ChangelogDownloader().GetChangelog(x => { ChangelogText = x; }, false);
        }

        public bool UpdateIsRequired
        {
            get { return _updateIsRequired; }
            set
            {
                if (_updateIsRequired != value)
                {
                    _updateIsRequired = value;
                    RaisePropertyChanged(nameof(UpdateIsRequired));
                }
            }
        }

        public Version UpdateVersion
        {
            get { return _updateVersion; }
            set
            {
                if (_updateVersion != value)
                {
                    _updateVersion = value;
                    RaisePropertyChanged(nameof(UpdateVersion));
                }
            }
        }

        public string UpdateText
        {
            get { return _updateSubText; }
            set
            {
                if (_updateSubText != value)
                {
                    _updateSubText = value;
                    RaisePropertyChanged(nameof(UpdateText));
                }
            }
        }

        public string ChangelogText
        {
            get { return _changelogText; }
            set
            {
                if (_changelogText != value)
                {
                    _changelogText = value;
                    RaisePropertyChanged(nameof(ChangelogText));
                }
            }
        }

        public UpdateMode SelectedUpdateMode { get; private set; } = UpdateMode.RemindLater;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void btnRemindLater_Click(object sender, RoutedEventArgs e)
        {
            SelectedUpdateMode = UpdateMode.RemindLater;
            DialogResult = true;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            SelectedUpdateMode = UpdateMode.RemindNever;
            DialogResult = true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SelectedUpdateMode = UpdateMode.UpdateNow;
            DialogResult = true;
        }
    }
}