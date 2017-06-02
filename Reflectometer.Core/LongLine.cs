using System;
using System.Numerics;

namespace Reflectometer.Core
{
    public class LongLine
    {
        public enum KindOfValue
        {
            ReflectionCoeff,
            ReturnLoss,
            MismatchLoss,
            SWR,
            LoadResistance
        }

        public double Xmax { get; private set; }

        public const double Z0 = 50;


        public Complex ReflectionCoeff { get; private set; } //?
        public double ReturnLoss { get; private set; }
        public double MissmatchLoss { get; private set; }
        public double StandingWaveRatio { get; private set; }
        public double LoadResistance { get; private set; }


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
        }


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

        /*   private void Something(double xmax, double z0, double rn, double xn, double lambda, double alpha, double f, double a)
           {
               Xmax = xmax;
               Z0 = z0;
               Rn = rn;
               Xn = xn;
               Lambda = lambda;
               Alpha = alpha;
               F = f;
               A = a;

               Zn = new Complex(Rn, Xn);


               if (Rn > Z0)
               {
                   ReflectionCoeff = (Zn - Z0) / (Zn + Z0);
               }
               else
               {
                   ReflectionCoeff = (-Zn + Z0) / (Zn + Z0);
               }

               var s = (1 + ReflectionCoeff.Magnitude) / (1 - ReflectionCoeff.Magnitude);

               if (Z0 >= Zn.Real)
               {
                   Pz = Math.PI / 2 + ReflectionCoeff.Phase / 2;
               }
               else
               {
                   Pz = -Math.PI / 2 + ReflectionCoeff.Phase / 2;
               }

               Betha = 2 * Math.PI / Lambda;

               W = 2 * Math.PI * F;

               B = A * Math.Exp(-2 * Alpha * Xmax) * ReflectionCoeff.Magnitude;
               B1 = A * Math.Exp(-Alpha * Xmax) * (1-ReflectionCoeff.Magnitude);
           }
           */





    }




}