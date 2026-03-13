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
        private readonly StatusService statusService;
        public RightSidebarViewModel(ConnectionService connectionService, StatusService statusService)
        {
            this.connectionService = connectionService;
            this.statusService = statusService;

        }




        [RelayCommand]
        public async Task Disconnect()
        {
            try
            {
                await connectionService.ServerDisconnect();

            }
            catch (Exception ex)
            {
                statusService.AddReport($"Error {ex}");
            }
        }



        public ICommand ConnectCommand { get; }

        public ICommand OpenSettingsCommand { get; }
    }
}