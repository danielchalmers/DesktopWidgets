using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Helpers;
using Microsoft.Win32;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window, INotifyPropertyChanged
    {
        private readonly bool _allowEmptyData;
        private string _inputData;

        public InputBox(string title, string displayData = null, bool allowEmptyData = false)
        {
            InitializeComponent();
            Title = title;
            InputData = displayData;
            _allowEmptyData = allowEmptyData;
            IsDisplayData = !string.IsNullOrEmpty(displayData);
            if (IsDisplayData)
            {
                txtData.SelectAll();
            }
            DataContext = this;
        }

        public bool Cancelled { get; private set; }
        public bool IsDisplayData { get; }

        public string InputData
        {
            get => _inputData;
            set
            {
                if (_inputData != value)
                {
                    _inputData = value;
                    RaisePropertyChanged(nameof(InputData));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!_allowEmptyData && string.IsNullOrWhiteSpace(InputData))
            {
                Cancelled = true;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            DialogResult = true;
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                FileSystemHelper.WriteTextToFile(dialog.FileName, InputData);
            }
        }

        private void btnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(InputData);
        }
    }
}