using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Input;
using VoiceChat.Client.Services;
using VoiceChat.Client.ViewModels;

namespace VoiceChat.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        AppState appState;

        private PageViewModelBase currentPage;
        public PageViewModelBase CurrentPage
        {
            get { return currentPage; }
            private set { this.RaiseAndSetIfChanged(ref currentPage, value); }
        }
        public ICommand NavigateNextCommand { get; }
        public ICommand NavigatePreviousCommand { get; }



        private readonly PageViewModelBase[] Pages;

        public MainWindowViewModel(AppState appState, ConnectViewModel connectViewModel)
        {
            this.appState = appState;
            CurrentPage = connectViewModel;
            var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentPage.CanNavigatePrevious);
            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
            
        }


        public void NavigateNext()
        {
            // get the current index and add 1
            var index = Pages.IndexOf(CurrentPage) + 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];
        }
        private void NavigatePrevious()
        {
            // get the current index and subtract 1
            var index = Pages.IndexOf(CurrentPage) - 1;

            //  /!\ Be aware that we have no check if the index is valid. You may want to add it on your own. /!\
            CurrentPage = Pages[index];
        }

    }
}
