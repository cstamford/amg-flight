// ==================================================================== \\
// File   : Helpers.cs                                                  \\
// Author : Christopher Stamford                                        \\
//                                                                      \\
// Helpers.cs provides common functionality that is script-independent. \\
// ==================================================================== \\

using System;
using System.Linq;
using UnityEngine;

namespace cst.Common
{
    public static class Helpers
    {
        // Wraps a value between positive numbers lowBound and highBound
        public static float wrapValue(float value,  float lowBound, float highBound)
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
        public static float clamp(float value, float lowBound, float highBound)
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

        // Return a normalized angle, from [-180 ... 180]
        // Assume the angle is wrapped between [0 ... 360]
        public static float getNormalizedAngle(float angle)
        {
            return angle > 180.0f ? angle - 360.0f : angle;
        }

        // Quadratic interpolation - easing in
        public static float quadraticInterpIn(float start, float max, float time, float maxTime)
        {
            time /= maxTime;
            return (max * (float)Math.Pow(time, 2.0f)) + start;
        }

        // Quadratic interpolation - easing out
        public static float quadraticInterpOut(float start, float max, float time, float maxTime)
        {
            time /= maxTime;
            return -max * time * (time - 2) + start;     
        }

        // Cubic interpolation - easing in
        public static float cubicInterpIn(float start, float max, float time, float maxTime)
        {
            time /= maxTime;
            return (max * (float)Math.Pow(time, 3.0f)) + start;
        }

        // Returns the nearest ray hit projected with the provided unit vector
        public static RaycastHit? nearestHit(Vector3 position, Vector3 direction, float distance, string tag = null)
        {
            RaycastHit[] hits = Physics.RaycastAll(position, direction, distance);

            const float EPSILON = 0.000001f;
            RaycastHit? nearest = hits
                .Where(hit => tag == null || String.Equals(hit.collider.tag, tag))
                .Cast<RaycastHit?>().
                FirstOrDefault(hit => Math.Abs(hit.Value.distance - hits.Min(dist => dist.distance)) < EPSILON);

            return nearest;
        }

        // Returns the distance to the nearest ray hit projected with the provided unit vector
        public static float? nearestHitDistance(Vector3 position, Vector3 direction, float distance, string tag = null)
        {
            RaycastHit? hit = nearestHit(position, direction, distance, tag);
            return hit.HasValue ? hit.Value.distance : (float?)null;
        }
    }

}