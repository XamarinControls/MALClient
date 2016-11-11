using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MALClient.Android
{
    public static class AndroidUtilities
    {
        public static T ClassCast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }

        public static T EnumCast<T>(this Java.Lang.Object obj)
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? default(T) : (T)propertyInfo.GetValue(obj, null);
        }

        private static readonly Dictionary<INotifyPropertyChanged, Tuple<string, Action, PropertyChangedEventHandler>> RegisteredActions =
            new Dictionary<INotifyPropertyChanged, Tuple<string, Action, PropertyChangedEventHandler>>();

        public static void RegisterOneTimeOnPropertyChangedAction(this INotifyPropertyChanged viewModel, string property, Action action)
        {
            var dlgt = new PropertyChangedEventHandler(OnPropertyChangedHandler);
            RegisteredActions.Add(viewModel, new Tuple<string, Action, PropertyChangedEventHandler>(property, action, dlgt));
            viewModel.PropertyChanged += dlgt;
        }
        private static void OnPropertyChangedHandler(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var viewModel = sender as INotifyPropertyChanged;
            if (viewModel == null)
                return;
            Tuple<string, Action, PropertyChangedEventHandler> data;
            if (RegisteredActions.TryGetValue(viewModel, out data))
            {
                if (propertyChangedEventArgs.PropertyName == data.Item1)
                {
                    data.Item2.Invoke();
                    viewModel.PropertyChanged -= data.Item3;
                    RegisteredActions.Remove(viewModel);
                }
            }
        }
    }
}