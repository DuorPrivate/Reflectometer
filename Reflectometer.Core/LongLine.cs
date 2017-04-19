using System;
using System.Numerics;

namespace Reflectometer.Core
{
    public class LongLine
    {
        public double Xmax { get; }

        public double Z0 { get; }

        public double Rn { get; }

        public double Xn { get; }

        public Complex Zn { get; } //?

        public Complex Gamma { get; } //?

        public double Pz { get; } //?

        public double Lambda { get; }

        public double Alpha { get; }
        public double Betha { get; }

        public double F { get; }
        public double W { get; }

        public double A { get; }

        public double B { get; }
        public double B1 { get; }

        public LongLine(double xmax, double z0, double rn, double xn, double lambda, double alpha, double f, double a)
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
                Gamma = (Zn - Z0) / (Zn + Z0);
            }
            else
            {
                Gamma = (-Zn + Z0) / (Zn + Z0);
            }

            var s = (1 + Gamma.Magnitude) / (1 - Gamma.Magnitude);

            if (Z0 >= Zn.Real)
            {
                Pz = Math.PI / 2 + Gamma.Phase / 2;
            }
            else
            {
                Pz = -Math.PI / 2 + Gamma.Phase / 2;
            }

            Betha = 2 * Math.PI / Lambda;

            W = 2 * Math.PI * F;

            B = A * Math.Exp(-2 * Alpha * Xmax) * Gamma.Magnitude;
            B1 = A * Math.Exp(-Alpha * Xmax) * (1-Gamma.Magnitude);
        }

    }




}