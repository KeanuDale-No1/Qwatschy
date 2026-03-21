using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace VoiceChat.Client.Converters;

public class IsSelectedConverter : IMultiValueConverter
{
    private static readonly IBrush DefaultBrush = new SolidColorBrush(Colors.Transparent);
    private static readonly IBrush SelectedBrush = new SolidColorBrush(Color.Parse("#b8245f"));

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            return DefaultBrush;

        var channelId = values[0] as Guid?;
        var selectedChannel = values[1];

        if (channelId == null || selectedChannel == null)
            return DefaultBrush;

        bool isSelected = false;

        if (selectedChannel is Guid selectedId)
            isSelected = channelId == selectedId;
        else
        {
            var selectedIdProperty = selectedChannel.GetType().GetProperty("Id");
            if (selectedIdProperty != null)
            {
                var selectedIdValue = selectedIdProperty.GetValue(selectedChannel) as Guid?;
                isSelected = channelId == selectedIdValue;
            }
        }

        return isSelected ? SelectedBrush : DefaultBrush;
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            return DefaultBrush;

        return Convert((IList<object?>)values, targetType, parameter, culture);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}