using StatefulModel.EventListeners;
using System;
using System.ComponentModel;

namespace Clapton.Extensions
{
    public static class PropertyChangedExtensions
    {

        public static IDisposable Subscribe(this INotifyPropertyChanged obj, PropertyChangedEventHandler handler)
        {
            return new PropertyChangedEventListener(obj, handler);
        }

        public static IDisposable Subscribe(this INotifyPropertyChanged obj, Action<string> action)
        {
            return new PropertyChangedEventListener(obj, (sender, args) => action(args.PropertyName));
        }

        public static IDisposable Subscribe(this INotifyPropertyChanged obj, string propertyName, Action action, bool runNow = false)
        {
            if (runNow) action();
            PropertyChangedEventListener listener = new PropertyChangedEventListener(obj);
            listener.Add(propertyName, (sender, args) => action());
            return listener;
        }

    }
}
