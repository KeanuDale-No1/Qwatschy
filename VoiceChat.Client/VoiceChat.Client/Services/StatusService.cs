using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VoiceChat.Client.Services
{
    public class StatusService
    {

        public ObservableCollection<String> Reports { get; set; } = new();


        public void AddReport(string report)
        {
            Reports.Add(report);
        }

    }
}
