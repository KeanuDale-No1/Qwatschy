using System.Threading.Tasks;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.Services;

public interface INavigationService
{
    Task NavigateTo<T>() where T : ViewModelBase;
}
