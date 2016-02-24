using System;

namespace Integrals
{
    class Program
    {
        private const int N = 20;
        static double Integrand(double x) =>
            Math.Sqrt(x)*Math.Exp(x);

        static Method[] GetMethods() =>
            new Method[]
            {
                new LeftRectangle(),
                new RightRectangle(),
                new Simpson(),
                new Gregory(), 
                new Trapeze(), 
                new ThreeEight(), 
            };

        static void Main(string[] args)
        {
            var left = 0.1*N;
            var right = 0.5 + 0.2*N;
            var steps = new[]{0.1, 0.05, 0.025};
            var methods = GetMethods();
            var format = "{0,-20} {1,-20} {2,-20}";
            Console.WriteLine(format,"Method","Integral value","Error");
            foreach (var step in steps)
            {
                Console.WriteLine(step);
                foreach (var method in methods)
                {
                    Console.WriteLine(format, 
                        method.GetType().Name,
                        method.ComplexCalc(Integrand,left,right,step),
                        method.Error(Integrand, left, right, step));
                }
                Console.WriteLine();
            }
        }
    }
}
