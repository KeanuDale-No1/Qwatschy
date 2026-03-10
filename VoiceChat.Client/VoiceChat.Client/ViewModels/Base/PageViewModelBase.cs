using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceChat.Client.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        public abstract bool CanNavigateNext { get; protected set; }
        public abstract bool CanNavigatePrevious { get; protected set; }
    }
}
