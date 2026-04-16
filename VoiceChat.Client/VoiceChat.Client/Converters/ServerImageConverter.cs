using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace VoiceChat.Client.Converters;

public class ServerImageConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            return null;

        var serverImage = values[0] as string;
        var fallback = values[1] as string;

        if (!string.IsNullOrEmpty(serverImage))
        {
            try
            {
                var base64Data = serverImage.Contains(',') 
                    ? serverImage.Split(',')[1] 
                    : serverImage;
                var binaryData = System.Convert.FromBase64String(base64Data);
                using var stream = new MemoryStream(binaryData);
                var bitmap = new Avalonia.Media.Imaging.Bitmap(stream);

                if (targetType == typeof(IBrush) || targetType.IsAssignableTo(typeof(IBrush)))
                    return new ImageBrush { Source = bitmap, Stretch = Stretch.UniformToFill };

                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        return fallback ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
