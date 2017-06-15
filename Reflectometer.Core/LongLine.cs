using System;
using System.Numerics;

namespace Reflectometer.Core
{
    public class LongLine
    {

        public LongLine(double value, KindOfValue kind)
        {
            switch (kind)
            {
                case KindOfValue.ReflectionCoeff:
                    SetByReflectionCoefficient(value);
                    break;
                case KindOfValue.ReturnLoss:
                    SetByReturnLoss(value);
                    break;
                case KindOfValue.MismatchLoss:
                    SetByMissmatchLoss(value);
                    break;
                case KindOfValue.SWR:
                    SetBySWR(value);
                    break;
                case KindOfValue.LoadResistance:
                    SetByLoadResistance(value);
                    break;
            }
            CulcFuncs();
        }

        public enum KindOfValue
        {
            ReflectionCoeff,
            ReturnLoss,
            MismatchLoss,
            SWR,
            LoadResistance
        }

        public const double Z0 = 50;
        public double XMax = 2;
        public double Loss = 0;
        double time = 0;

        private double betha = 2 * Math.PI;

        public Func<double, double> IncFn;
        public Func<double, double> RefFn;
        public Func<double, double> PassFn;
        public Func<double, double> UpHelpFn;
        public Func<double, double> DownHelpFn;
        public Func<double, double> SummFn;

        public Complex ReflectionCoeff { get; private set; } //?
        public double ReturnLoss { get; private set; }
        public double MissmatchLoss { get; private set; }
        public double StandingWaveRatio { get; private set; }
        public double LoadResistance { get; private set; }

        private void SetByReflectionCoefficient(double reflectionCoeff)
        {
            ReflectionCoeff = reflectionCoeff;
            ReturnLoss = -20 * Math.Log10(Math.Abs(reflectionCoeff));
            MissmatchLoss = -10 * Math.Log10(1 - Math.Pow(reflectionCoeff, 2));
            StandingWaveRatio = (1 + Math.Abs(reflectionCoeff)) / (1 - Math.Abs(reflectionCoeff));
            LoadResistance = -((reflectionCoeff + 1) * Z0 / (reflectionCoeff - 1));
        }
        private void SetByReflectionCoefficient()
        {
            ReturnLoss = -20 * Math.Log10(Math.Abs(ReflectionCoeff.Real));
            MissmatchLoss = -10 * Math.Log10(1 - Math.Pow(ReflectionCoeff.Real, 2));
            StandingWaveRatio = (1 + Math.Abs(ReflectionCoeff.Real)) / (1 - Math.Abs(ReflectionCoeff.Real));
            LoadResistance = -((ReflectionCoeff.Real + 1) * Z0 / (ReflectionCoeff.Real - 1));
        }
        private void SetByReturnLoss(double returnLoss)
        {
            ReflectionCoeff = Math.Pow(10, -returnLoss / 20);
            SetByReflectionCoefficient();
        }
        private void SetByMissmatchLoss(double missmatchLoss)
        {
            ReflectionCoeff = Math.Sqrt(1 - Math.Pow(10, missmatchLoss / -10));
            SetByReflectionCoefficient();
        }
        private void SetBySWR(double swr)
        {
            ReflectionCoeff = (swr - 1) / (swr + 1);
            SetByReflectionCoefficient();
        }
        private void SetByLoadResistance(double loadResistance)
        {
            ReflectionCoeff = (loadResistance - Z0) / (loadResistance + Z0);
            SetByReflectionCoefficient();
        }

        public void CulcFuncs()
        {
            double pZ;

            if (Z0 >= LoadResistance)
            {
                pZ = Math.PI / 2 + ReflectionCoeff.Phase / 2;
            }
            else
            {
                pZ = -Math.PI / 2 + ReflectionCoeff.Phase / 2;
            }

            if (time >= 3)
            {
                time = 0;
            }

            IncFn = (x) => Math.Exp(Loss*(-x)) * Math.Cos(-betha * x + 2 * Math.PI * time);
            RefFn = (x) => (Math.Exp(-2 * Loss * XMax) * ReflectionCoeff.Real) * Math.Exp(Loss * x) * Math.Cos(betha * x + 2 * Math.PI * time + ReflectionCoeff.Phase);

            PassFn = (x) => (Math.Exp(-Loss * XMax) * (1 - ReflectionCoeff.Real)) * Math.Cos(-betha * x + 2 * Math.PI * time + ReflectionCoeff.Phase);

            SummFn = (x) => Math.Exp(Loss * (-x)) * Math.Cos(-betha * x + 2 * Math.PI * time) + (Math.Exp(-2 * Loss * XMax) * ReflectionCoeff.Real) * Math.Exp(Loss * x) * Math.Cos(betha * x + 2 * Math.PI + ReflectionCoeff.Phase);

            //UpHelpFn = (x) => Math.Abs(Math.Exp(-Loss * x) * Math.Exp(Math.Sqrt(-1) * betha * x) * Math.Exp(-Math.Sqrt(-1) * pZ) - (Math.Exp(-2 * Loss * XMax) * ReflectionCoeff.Real) * Math.Exp(Loss * x) * Math.Exp(Math.Sqrt(-1) * betha * x) * Math.Exp(Math.Sqrt(-1) * pZ));
            //DownHelpFn = (x) => -Math.Abs(Math.Exp(-Loss * x) * Math.Exp(Math.Sqrt(-1) * betha * x) * Math.Exp(-Math.Sqrt(-1) * pZ) - (Math.Exp(-2 * Loss * XMax) * ReflectionCoeff.Real) * Math.Exp(Loss * x) * Math.Exp(Math.Sqrt(-1) * betha * x) * Math.Exp(Math.Sqrt(-1) * pZ));

            time = time + 0.02;
        }

    }




}