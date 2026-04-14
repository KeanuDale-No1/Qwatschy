using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VoiceChat.Client.Services.DialogService;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.Dialog;

public partial class DialogViewModel:ViewModelBase
{

    [ObservableProperty] DialogViewModelBase dialogViewModelBase;
    [ObservableProperty] bool show;


    DialogServiceViewSetterService dialogServiceViewSetterService;
    public DialogViewModel(DialogServiceViewSetterService dialogServiceViewSetterService)
    {
       this.dialogServiceViewSetterService = dialogServiceViewSetterService;
        this.dialogServiceViewSetterService.DialogViewModelSetted += OnDialogViewModelSetted;
    }

    private void OnDialogViewModelSetted(object obj)
    {
        if (obj == null)
            Show = false;
        else
            Show = true;
        DialogViewModelBase = (DialogViewModelBase)obj;
    }
}
