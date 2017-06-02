
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using Reflectometer.Core;
using System;
using static Android.Widget.TextView;

namespace Reflectometer.Android.Fragments
{
    public class CalculatorFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.CalculatorFragment,container,false);

            SetupEvents();

            ma = (MainActivity)Activity;

            return view;
        }

        private void SetupEvents()
        {
            var reflectionCoeffField = view.FindViewById<EditText>(Resource.Id.reflectionCoeffField);
            reflectionCoeffField.EditorAction += EditorAction;

            var returnLossField = view.FindViewById<EditText>(Resource.Id.returnLossField);
            returnLossField.EditorAction += EditorAction;

            var mismatchLossField = view.FindViewById<EditText>(Resource.Id.mismatchLossField);
            mismatchLossField.EditorAction += EditorAction;

            var swrField = view.FindViewById<EditText>(Resource.Id.swrField);
            swrField.EditorAction += EditorAction;

            var loadResistanceField = view.FindViewById<EditText>(Resource.Id.loadResistanceField);
            loadResistanceField.EditorAction += EditorAction;
        }

        void EditorAction(object sender, EditorActionEventArgs e)
        {
            if (sender == view.FindViewById<EditText>(Resource.Id.reflectionCoeffField))
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.reflectionCoeffField).Text);
                ma.LongLine = new LongLine(value, LongLine.KindOfValue.ReflectionCoeff);
            }
            if (sender == view.FindViewById<EditText>(Resource.Id.returnLossField))
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.returnLossField).Text);
                ma.LongLine = new LongLine(value, LongLine.KindOfValue.ReturnLoss);
            }
            if (sender == view.FindViewById<EditText>(Resource.Id.mismatchLossField))
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.mismatchLossField).Text);
                ma.LongLine = new LongLine(value, LongLine.KindOfValue.MismatchLoss);
            }
            if (sender == view.FindViewById<EditText>(Resource.Id.swrField))
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.swrField).Text);
                ma.LongLine = new LongLine(value, LongLine.KindOfValue.SWR);
            }
            if (sender == view.FindViewById<EditText>(Resource.Id.loadResistanceField))
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.loadResistanceField).Text);
                ma.LongLine = new LongLine(value, LongLine.KindOfValue.LoadResistance);
            }

            Update();
        }

        private MainActivity ma;
        private View view;

        private void Update()
        {
            view.FindViewById<EditText>(Resource.Id.reflectionCoeffField).Text = Convert.ToString(ma.LongLine.ReflectionCoeff.Real);
            view.FindViewById<EditText>(Resource.Id.returnLossField).Text = Convert.ToString(ma.LongLine.ReturnLoss);
            view.FindViewById<EditText>(Resource.Id.mismatchLossField).Text = Convert.ToString(ma.LongLine.MissmatchLoss);
            view.FindViewById<EditText>(Resource.Id.swrField).Text = Convert.ToString(ma.LongLine.StandingWaveRatio);
            view.FindViewById<EditText>(Resource.Id.loadResistanceField).Text = Convert.ToString(ma.LongLine.LoadResistance);
        }
    }

    public class HelpFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.HelpFragment, container, false);
        }
    }

    public class VisualizationFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.VisualizationFragment, container, false);
        }
    }
}