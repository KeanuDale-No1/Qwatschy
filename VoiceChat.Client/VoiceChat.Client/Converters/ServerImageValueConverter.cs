using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace VoiceChat.Client.Converters;

public class ServerImageValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var serverImage = value as string;

        if (string.IsNullOrEmpty(serverImage))
            return null;

        try
        {
            var base64Data = serverImage.Contains(',') 
                ? serverImage.Split(',')[1] 
                : serverImage;
            var binaryData = System.Convert.FromBase64String(base64Data);
            using var stream = new MemoryStream(binaryData);
            return new Bitmap(stream);
        }
        catch
        {
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
