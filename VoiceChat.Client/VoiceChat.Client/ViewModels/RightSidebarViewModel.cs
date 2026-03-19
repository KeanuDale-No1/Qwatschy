using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;
using VoiceChat.Client.ViewModels.MainArea;

namespace VoiceChat.Client.ViewModels
{
    public partial class RightSidebarViewModel : ViewModelBase
    {
        private readonly ConnectionService connectionService;
        public RightSidebarViewModel(ConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }




        [RelayCommand]
        public async Task Disconnect()
        {
            await connectionService.ServerDisconnect();
        }



        public ICommand ConnectCommand { get; }

        public ICommand OpenSettingsCommand { get; }
    }
}