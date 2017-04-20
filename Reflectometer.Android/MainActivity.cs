﻿using Android.App;
using Android.Widget;
using Android.OS;

namespace Reflectometer.Android
{
    [Activity(Label = "@string/ApplicationName")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
        }
    }
}

