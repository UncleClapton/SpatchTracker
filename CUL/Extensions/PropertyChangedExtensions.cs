using StatefulModel.EventListeners;
using System;
using System.ComponentModel;

namespace Clapton.Extensions
{
    public static class PropertyChangedExtensions
    {
        public static IDisposable Subscribe(this INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            return new PropertyChangedEventListener(source, handler);
        }

        public static IDisposable Subscribe(this INotifyPropertyChanged source, Action<string> action)
        {
            return new PropertyChangedEventListener(source, (sender, args) => action(args.PropertyName));
        }
    }
}
