using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using VoiceChat.Client.ViewModels.Base;

namespace VoiceChat.Client.ViewModels.MainArea.Componets;


public record ServerConnectionInfo(string ServerAdress, string ServerName, Bitmap Image);

public partial class ServerConnectionsViewModel : ViewModelBase
{
    public ObservableCollection<ServerConnectionInfo> Servers { get; } = new ObservableCollection<ServerConnectionInfo>()
    {
        new ServerConnectionInfo("Test Server 1", "", null),
        new ServerConnectionInfo("Test Server 2", "", null)
    }; 
    
    
    
}
