using System;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            if (c == 0 && a != c && b != c)
                return 0;
            else if (a <= 0 || b <= 0 || c < 0)
                return double.NaN;
            var result = (a * a + b * b - c * c) / (2 * a * b);
            return Math.Acos(result);
        }
    }
}

