#region

using System.ComponentModel;
using System.Windows;

#endregion

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window, INotifyPropertyChanged
    {
        private string _inputData;

        public InputBox(string title, string displayData = null)
        {
            InitializeComponent();
            Title = title;
            InputData = displayData;
            if (!string.IsNullOrEmpty(displayData))
            {
                txtData.IsReadOnly = true;
                txtData.SelectAll();
                btnCancel.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnCancel.Visibility = Visibility.Visible;
            }
            DataContext = this;
            txtData.Focus();
        }

        public bool Cancelled { get; private set; }

        public string InputData
        {
            get { return _inputData; }
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
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            DialogResult = true;
        }
    }
}