using System.Windows.Input;

namespace VoiceChat.Client.ViewModels
{
    public class RightSidebarViewModel : ViewModelBase
    {
        private readonly MainViewModel _main;

        public RightSidebarViewModel(MainViewModel main)
        {
            _main = main;

            //OpenSettingsCommand = new RelayCommand(() => _main.IsSettingsOpen = true);
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public ICommand OpenSettingsCommand { get; }
    }
}