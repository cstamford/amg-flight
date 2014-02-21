// Various useful helpers that aren't specific to one script

using System;

namespace cst
{
    public static class Helpers
    {
        // Wraps a value between positive numbers lowBound and highBound
        public static float wrapValue(float value, 
            float lowBound, float highBound)
        {
            if (highBound < lowBound)
                return -1.0f;

            while (value < lowBound)
            {
                value += highBound;
            }

            while (value > highBound)
            {
                value -= highBound;
            }

            return value;
        }

        // Wraps between [0 ... 360]
        public static float wrapAngle(float value)
        {
            return wrapValue(value, 0.0f, 360.0f);
        }

        // Clamp a value between lowBound and highBound;
        public static float clamp(float value,
            float lowBound, float highBound)
        {
            value = high(value, highBound);
            value = low(value, lowBound);

            return value;
        }

        // Returns value, or bound if value is greater than bound
        public static float high(float value, float bound)
        {
            return value > bound ? bound : value;
        }

        // Returns value, or bound if value is less than bound
        public static float low(float value, float bound)
        {
            return value < bound ? bound : value;
        }

        // Returns the difference between two angles between [0 ... 180]
        public static float differenceBetween(float angle1, float angle2)
        {
            return 180.0f - Math.Abs(Math.Abs(angle1 - angle2) - 180.0f);
        }

        // Return a normalized angle, from [-x/2 ... x/2]
        // Assume the angle is wrapped between [0 ... 360]
        public static float getNormalizedAngle(float angle)
        {
            return angle > 180.0f ? angle - 360.0f : angle;
        }
    }

}