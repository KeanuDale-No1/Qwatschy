using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.DialogService;

namespace VoiceChat.Client.ViewModels.Base;

public partial class DialogViewModelBase : ViewModelBase
{
    protected readonly IDialogService dialogService;
    public virtual string Title { get; init; }

    public DialogViewModelBase(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    [RelayCommand]
    protected virtual void cancel()
    {
        dialogService.Close(true,null);
    }
}
