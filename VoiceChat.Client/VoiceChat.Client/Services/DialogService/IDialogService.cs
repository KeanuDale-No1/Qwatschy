using System.Threading.Tasks;

namespace VoiceChat.Client.Services.DialogService;

public interface IDialogService
{
    void Close(bool isCancel, object? data);
    Task<DialogResult> ShowDialog<TViewModel>();
}

public class DialogResult
{
    public bool IsCanceled { get; set; }
    public object? Data { get; set; }
}