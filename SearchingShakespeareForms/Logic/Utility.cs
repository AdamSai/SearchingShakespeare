using System;

namespace SearchingShakespeare
{
    public static class Utility
    {
        public static int MathClamp(int value, int min, int max)
        {
            if (min > max)
            {
                throw new Exception("min must be smaller than max");
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            
            return value;
        }

    }
}