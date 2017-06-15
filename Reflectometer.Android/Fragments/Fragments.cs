
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;
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

            var loss = view.FindViewById<EditText>(Resource.Id.lossField);
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

            if (view.FindViewById<EditText>(Resource.Id.lossField).Text != null)
            {
                double value = Convert.ToDouble(view.FindViewById<EditText>(Resource.Id.lossField).Text);
                ma.LongLine.Loss = value;
            }

            Update();
        }

        private MainActivity ma;
        private View view;

        private void Update()
        {
            view.FindViewById<EditText>(Resource.Id.reflectionCoeffField).Text = Convert.ToString(Math.Round((ma.LongLine.ReflectionCoeff.Real),4));
            view.FindViewById<EditText>(Resource.Id.returnLossField).Text = Convert.ToString(Math.Round((ma.LongLine.ReturnLoss), 4));
            view.FindViewById<EditText>(Resource.Id.mismatchLossField).Text = Convert.ToString(Math.Round((ma.LongLine.MissmatchLoss), 4));
            view.FindViewById<EditText>(Resource.Id.swrField).Text = Convert.ToString(Math.Round((ma.LongLine.StandingWaveRatio), 4));
            view.FindViewById<EditText>(Resource.Id.loadResistanceField).Text = Convert.ToString(Math.Round((ma.LongLine.LoadResistance), 4));
        }
    }

    public class HelpFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.HelpFragment, container, false);

            var imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
            imageView.SetImageResource(Resource.Drawable.LL);

            return view;
        }
    }

    public class VisualizationFragment : Fragment
    {
        private bool runing = false;
        private PlotView sumPlot;
        private PlotModel SumPlotModel;
        private PlotView passPlot;
        private PlotView incAndRefPlot;

        private View view;

        private MainActivity ma;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ma = (MainActivity)Activity;

            view = inflater.Inflate(Resource.Layout.VisualizationFragment, container, false);

            var startButton = view.FindViewById<Button>(Resource.Id.button1);
            startButton.Click += HandleTouchUpInside;

            var imageView = view.FindViewById<ImageView>(Resource.Id.imageView1);
            imageView.SetImageResource(Resource.Drawable.reflectometer);

            incAndRefPlot = view.FindViewById<PlotView>(Resource.Id.plotView1);
            SetIncAndRefPlot(incAndRefPlot);

            passPlot = view.FindViewById<PlotView>(Resource.Id.plotView2);
            SetPassPlot(passPlot);

            sumPlot = view.FindViewById<PlotView>(Resource.Id.plotView3);
            SetSumPlot(sumPlot);


            return view;
        }

        private void HandleTouchUpInside(object sender, EventArgs e)
        {

            ma.LongLine.CulcFuncs();

            SetIncAndRefPlot(incAndRefPlot);
            SetSumPlot(sumPlot);
            SetPassPlot(passPlot);

            //runing = !runing;
        }


        private void SetSumPlot(PlotView sumPlot)
        {
            SumPlotModel = new PlotModel
            {
                Title = "Результирующая",
                TitleFontSize = 10
            };
            SumPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Maximum = 2,
                Minimum = -2,
            });
            //            plotModel.Series.Add(new FunctionSeries(ma.LongLine.UpHelpFn, 0, ma.LongLine.XMax, 0.1) { Color = OxyColors.Blue });
            //            plotModel.Series.Add(new FunctionSeries(ma.LongLine.DownHelpFn, 0, ma.LongLine.XMax, 0.1) { Color = OxyColors.Blue });
            SumPlotModel.Series.Add(new FunctionSeries(ma.LongLine.SummFn, 0, ma.LongLine.XMax, 0.01) { Color = OxyColors.Red });
            sumPlot.Model = SumPlotModel;
        }

        private void SetIncAndRefPlot(PlotView incAndRefPlot)
        {
            var plotModel = new PlotModel
            {
                Title = "Падающая и отраженные волны",
                TitleFontSize = 10
            };
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Maximum = 2,
                Minimum = -2,
            });
            plotModel.Series.Add(new FunctionSeries(ma.LongLine.IncFn, 0, ma.LongLine.XMax, 0.01) { Color = OxyColors.Red });
            plotModel.Series.Add(new FunctionSeries(ma.LongLine.RefFn, 0, ma.LongLine.XMax, 0.01) { Color = OxyColors.Yellow });
            incAndRefPlot.Model = plotModel;
        }

        private void SetPassPlot(PlotView passPlot)
        {
            var plotModel = new PlotModel
            {
                Title = "Прошедшая волна",
                TitleFontSize = 10
            };
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Maximum = 2,
                Minimum = -2,
            });
            plotModel.Series.Add(new FunctionSeries(ma.LongLine.PassFn, 0, ma.LongLine.XMax, 0.01) { Color = OxyColors.Red });
            passPlot.Model = plotModel;
        }
    }
}