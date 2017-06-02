using Android.App;
using Android.Widget;
using Android.OS;
using Reflectometer.Android.Fragments;
using Android.Text;
using Reflectometer.Core;
using System;
using Android.Icu.Text;

namespace Reflectometer.Android
{
    [Activity(Label = "@string/ApplicationName", Theme="@style/MyTheme.CastomActionBar")]
    public class MainActivity : Activity
    {

        static readonly string Tag = "ActionBarTabsSupport";

        public LongLine LongLine { get; set; }

        Fragment[] _fragments;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            SetContentView(Resource.Layout.Main);

            _fragments = new Fragment[]
             {
                             new HelpFragment(), 
                             new CalculatorFragment(),
                             new VisualizationFragment()
             };

            AddTabToActionBar(Resource.Drawable.information);
            AddTabToActionBar(Resource.Drawable.calculator);
            AddTabToActionBar(Resource.Drawable.chart);

            ActionBar.SetSelectedNavigationItem(1);


            //            ActionBar.SetCustomView(Resource.Layout.ActionBar);
            //            ActionBar.SetDisplayShowCustomEnabled(true);


        }

        void AddTabToActionBar(int iconResourceId)
        {
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetCustomView(Resource.Layout.TabLayout);
            tab.CustomView.FindViewById<ImageView>(Resource.Id.tabIcon).SetImageResource(iconResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab)sender;

            Fragment frag = _fragments[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout, frag);
        }
    }
}

