using System.Windows.Input;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.ViewModels
{
    public class RightSidebarViewModel : ViewModelBase
    {
        private readonly MainAreaViewModel _main;

        public RightSidebarViewModel(MainAreaViewModel main)
        {
            _main = main;

            //OpenSettingsCommand = new RelayCommand(() => _main.IsSettingsOpen = true);
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public ICommand OpenSettingsCommand { get; }
    }
}