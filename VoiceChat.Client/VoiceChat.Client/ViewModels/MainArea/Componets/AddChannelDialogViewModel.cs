using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;

internal partial class AddChannelDialogViewModel : DialogViewModelBase
{
    public override string Title { get; init; } = "Neuen Kanal hinzufügen";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Ein Name ist erforderlich")]
    [NotifyCanExecuteChangedFor(nameof(CloseCommand))]
    private string? channelName;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CloseCommand))]
    private string? description;

    public AddChannelDialogViewModel(IDialogService dialogService) : base(dialogService)
    {
    }

    [RelayCommand(CanExecute = nameof(CanClose))]
    protected async Task Close()
    {
        var result = (ChannelName, Description);
        dialogService.Close(false, result);
    }

    private bool CanClose() => !HasErrors && !string.IsNullOrWhiteSpace(ChannelName);
}