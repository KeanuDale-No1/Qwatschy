using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

internal partial class AddServerDialogViewModel : DialogViewModelBase
{
    public override string Title { get; init; } = "Neuen Server hinzufügen";
    [ObservableProperty] private string serverAdress;

    public AddServerDialogViewModel(IDialogService dialogService) : base(dialogService)
    {
    }

    [RelayCommand]
    protected async Task close()
    {
        if (ServerAdress != null)
        {
            dialogService.Close(false, serverAdress);
        }
    }
}
