using System;
using System.Collections.Generic;
using System.Text;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.Services.DialogService
{
    public class DialogServiceViewSetterService
    {

        public Action<object>? DialogViewModelSetted;
        internal void SetDialogView(object dialogViewModelBase)
        {
            DialogViewModelSetted?.Invoke(dialogViewModelBase);
        }
        internal void ClearDialogView()
        {
            DialogViewModelSetted?.Invoke(null);
        }
    }
}
