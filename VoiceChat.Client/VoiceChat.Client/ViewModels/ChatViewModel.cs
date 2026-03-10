using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace VoiceChat.Client.ViewModels
{
    public class ChatViewModel
    {
        public List<string> Messages { get; set; } = new();


        public string MessageInput { get; set; }

        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; }

        public ChatViewModel()
        {
            SendMessageCommand = ReactiveCommand.Create(() => { });
        }
    }
}