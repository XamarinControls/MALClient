using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MALClient.XShared.Delegates;

namespace MALClient.Android
{
    public class ListenableWebClient : WebViewClient
    {
        public EmptyEventHander PageReady;
        public NavigationInterceptPossible NavigationInterceptOpportunity;

        private bool _loading;
        private bool _redirect;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (_loading)
                _redirect = true;

            InterceptUrl(view,url);
            return true;
        }

        private async void InterceptUrl(WebView view, string url)
        {
            var invoke = NavigationInterceptOpportunity?.Invoke(url);
            if (invoke != null)
            {
                var result = await invoke;
                if(!string.IsNullOrEmpty(result))
                    view.LoadUrl(url);
            }
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            _loading = true;
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            if (!_redirect)
            {
                _loading = false;
            }

            if (!_loading && !_redirect)
            {
                PageReady?.Invoke();
            }
            else
            {
                _redirect = false;
            }
            base.OnPageFinished(view, url);
        }
    }
}