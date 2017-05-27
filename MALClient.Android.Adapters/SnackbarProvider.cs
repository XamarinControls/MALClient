﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MALClient.Adapters;

namespace MALClient.Android.Adapters
{
    public class SnackbarProvider : ISnackbarProvider
    {
        public void ShowText(string text)
        {
            var t = Toast.MakeText(Application.Context, text, ToastLength.Short);
            t.Show();
        }
    }
}