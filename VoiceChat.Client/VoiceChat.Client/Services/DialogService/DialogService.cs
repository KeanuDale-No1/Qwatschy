using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace VoiceChat.Client.Services.DialogService;

public class DialogService(IServiceProvider serviceProvider, IApplicationLifetime applicationLifetime, DialogServiceViewSetterService dialogServiceViewSetterService) : IDialogService
{

    private TaskCompletionSource<DialogResult>? _dialogCompletionSource;
    public async Task<DialogResult> ShowDialog<DialogViewModelBase>()
    {
        var viewModel = serviceProvider.GetRequiredService<DialogViewModelBase>();
        if (viewModel == null) { throw new ArgumentException("ViewModel is null");}
        dialogServiceViewSetterService.SetDialogView(viewModel);


        _dialogCompletionSource = new TaskCompletionSource<DialogResult>();

        var result = await _dialogCompletionSource.Task;
        return result;
    }

    public void Close(bool isCancel, object data)
    {
        if (_dialogCompletionSource != null && !_dialogCompletionSource.Task.IsCompleted)
        {
            dialogServiceViewSetterService.ClearDialogView();
            // Setze das Ergebnis und beende die Task
            _dialogCompletionSource.SetResult(new DialogResult {  IsCanceled = isCancel, Data = data });
        }
    }
}
