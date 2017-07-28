using System.ComponentModel;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Actions
{
    public class SpeakTextAction : ActionBase
    {
        public FilePath FilePath { get; set; } = new FilePath();

        [DisplayName("Text")]
        public string Text { get; set; } = "";

        [DisplayName("Input Mode")]
        public InputMode InputMode { get; set; } = InputMode.Text;

        public SpeechSettings SpeechSettings { get; set; } = new SpeechSettings();

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            var synthesizer = new SpeechSynthesizer
            {
                Rate = SpeechSettings.Rate,
                Volume = SpeechSettings.Volume
            };

            var input = string.Empty;
            switch (InputMode)
            {
                case InputMode.Clipboard:
                    input = Clipboard.GetText();
                    break;
                case InputMode.File:
                    input = File.ReadAllText(FilePath.Path);
                    break;
                case InputMode.Text:
                    input = Text;
                    break;
            }
            synthesizer.SpeakAsync(input);
        }
    }
}