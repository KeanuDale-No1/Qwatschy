using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using VoiceChat.Shared.Models;

namespace VoiceChat.Desktop;

public partial class ChannelPage : ContentPage
{
    public ObservableCollection<ChannelViewModel> Channels { get; } = new ObservableCollection<ChannelViewModel>();

    public ObservableCollection<string> SelectedMembers { get; } = new ObservableCollection<string>();

    public ChannelPage()
    {
        InitializeComponent();
        BindingContext = this;

        // sample data with sample members
        Channels.Add(new ChannelViewModel(new Channel { Id = Guid.NewGuid(), Name = "General" }, new[] { "Alice", "Bob" }));
        Channels.Add(new ChannelViewModel(new Channel { Id = Guid.NewGuid(), Name = "Gaming" }, new[] { "Carol" }));
        Channels.Add(new ChannelViewModel(new Channel { Id = Guid.NewGuid(), Name = "Music" }, new[] { "Dave", "Eve", "Frank" }));
    }

    private async void OnCreateChannelClicked(object sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Neuer Channel", "Name des Channels:");
        if (!string.IsNullOrWhiteSpace(name))
        {
            Channels.Add(new ChannelViewModel(new Channel { Id = Guid.NewGuid(), Name = name }, Array.Empty<string>()));
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        var text = MessageEntry?.Text;
        if (!string.IsNullOrWhiteSpace(text))
        {
            await DisplayAlert("Send", $"Nachricht gesendet: {text}", "OK");
            MessageEntry.Text = string.Empty;
        }
    }

    private void ChannelsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedMembers.Clear();

        // update selection flag on all view-models so the Join button visibility updates
        foreach (var c in Channels)
            c.IsSelected = false;

        var vm = e.CurrentSelection.FirstOrDefault() as ChannelViewModel;
        if (vm != null)
        {
            vm.IsSelected = true;
            foreach (var m in vm.Members)
                SelectedMembers.Add(m);
        }
    }

    private async void OnJoinClicked(object? sender, EventArgs e)
    {
        // The TapGestureRecognizer will send the Frame (or the element) as sender.
        ChannelViewModel? vm = null;
        if (sender is Microsoft.Maui.Controls.VisualElement ve)
        {
            vm = ve.BindingContext as ChannelViewModel;
        }

        // fallback: if sender is TapGestureRecognizer (rare), try its parent context via the event args
        if (vm == null && sender is Microsoft.Maui.Controls.TapGestureRecognizer)
        {
            // nothing reliable to extract here; ignore
        }

        if (vm != null)
        {
            // TODO: implement real join logic (connect via websocket / API)
            await DisplayAlert("Join", $"Joining channel {vm.Name}", "OK");
        }
    }
}

public class ChannelViewModel : INotifyPropertyChanged
{
    public Channel Model { get; }
    public ObservableCollection<string> Members { get; } = new ObservableCollection<string>();

    bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public ChannelViewModel(Channel channel, IEnumerable<string> members)
    {
        Model = channel;
        foreach (var m in members) Members.Add(m);
    }

    public Guid Id => Model.Id;
    public string Name => Model.Name;
    public string Description => Model.Description;
    public int MemberCount => Members.Count;
}

