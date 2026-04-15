using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.Validation;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

internal partial class AddServerDialogViewModel : DialogViewModelBase
{
    public override string Title { get; init; } = "Neuen Server hinzufügen";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Url(ErrorMessage = "Bitte geben Sie eine gültige Adresse ein.")]
    [ServerAddressNotDuplicate(ErrorMessage = "Serveradresse existiert bereits.")]
    [NotifyCanExecuteChangedFor(nameof(CloseCommand))]
    private string? serverAdress;

    public AddServerDialogViewModel(IDialogService dialogService) : base(dialogService)
    {
    }

    [RelayCommand(CanExecute = nameof(CanClose))]
    protected async Task Close()
    {
        if (ServerAdress != null)
        {
            dialogService.Close(false, ServerAdress);
        }
    }

    private bool CanClose() => !HasErrors && !string.IsNullOrWhiteSpace(ServerAdress);
}
