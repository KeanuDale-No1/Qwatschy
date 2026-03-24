using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

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
        public async Task Connect()
        {
        }

        [RelayCommand]
        public async Task Disconnect()
        {
            await connectionService.ServerDisconnect();
        }

        [RelayCommand]
        public void OpenSettings()
        {
        }
    }
}