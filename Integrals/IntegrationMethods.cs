using System;

namespace Integrals
{
    public abstract class Method
    {
        protected abstract int P();
        public abstract double ComplexCalc(Func<double, double> func, double left, double right, double step);

        public double Error(Func<double, double> func, double left, double right, double step)
        {
            var i05 = ComplexCalc(func, left, right, step / 2);
            var i1 = ComplexCalc(func, left, right, step);
            var two = Math.Pow(2, P());
            var res = two * (i05 - i1) / (two - 1);
            return Math.Abs(res);
        }
    }
    public abstract class SimpleMethod : Method
    {
        public override double ComplexCalc(Func<double, double> func, double left, double right, double step)
        {
            var n = (int)Math.Round((right - left) / step);
            var a = left;
            var b = a + step;
            double result = 0;
            for (var i = 0; i < n; i++)
            {
                result += Calc(func, a, b);
                a = b;
                b += step;
            }
            return result;
        }
        public abstract double Calc(Func<double, double> func, double left, double right);
    }
    class LeftRectangle : SimpleMethod
    {
        protected override int P() => 1;
        public override double Calc(Func<double, double> func, double left, double right) =>
            func(left) * Math.Abs(right - left);

    }
    class RightRectangle : SimpleMethod
    {
        protected override int P() => 1;
        public override double Calc(Func<double, double> func, double left, double right) =>
            func(right) * Math.Abs(right - left);
    }
    class Simpson : SimpleMethod
    {
        protected override int P() => 4;
        public override double Calc(Func<double, double> func, double left, double right) =>
            (right - left) / 6 * (func(left) + 4 * func((left + right) / 2) + func(right));
    }
    class Gregory : Method
    {
        protected override int P() => 4;
        public override double ComplexCalc(Func<double, double> func, double left, double right, double step)
        {
            var n = (int)Math.Round((right - left) / step);
            var a = left;
            var b = a + step;
            double result = 0;
            for (var i = 0; i < n; i++)
            {
                result += func(a) + func(b);
                a = b;
                b += step;
            }
            result *= step / 2;
            result += (step * step / 12) *
                      (DerivativeLeft(func, left, step) -
                       DerivativeRight(func, right, step));
            return result;
        }

        private double DerivativeLeft(Func<double, double> func, double x, double step) =>
            (-3 * func(x) + 4 * func(x + step) - func(x + 2 * step)) / (2 * step);

        private double DerivativeRight(Func<double, double> func, double x, double step) =>
            (3 * func(x) - 4 * func(x - step) + func(x - 2 * step)) / (2 * step);
    }
    public class Trapeze : SimpleMethod
    {
        protected override int P() => 2;
        public override double Calc(Func<double, double> func, double left, double right) =>
            0.5 * (right - left) * (func(left) + func(right));
    }

    public class ThreeEight : SimpleMethod
    {
        protected override int P() => 4;
        public override double Calc(Func<double, double> func, double left, double right)
        {
            var h = right - left;
            return h / 8 * (
                func(left) +
                3 * func(left + h / 3) +
                3 * func(left + h * 2 / 3) +
                func(right));
        }
    }
}
