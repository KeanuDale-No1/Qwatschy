using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels
{
    public partial class StatusBarViewModel: ViewModelBase
    {
        StatusService statusService;
        [ObservableProperty]private string lastmessage ="";
        public StatusBarViewModel(StatusService statusService)
        {
            this.statusService = statusService;
            statusService.Reports.CollectionChanged += Reports_CollectionChanged;
        }

        private void Reports_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Lastmessage= statusService.Reports.Last();
        }
    }
}
