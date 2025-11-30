using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMath
{
    public static Vector2 RotateVector(this Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(radians);
        float sinTheta = Mathf.Sin(radians);

        float newX = (v.x * cosTheta) - (v.y * sinTheta);
        float newY = (v.x * sinTheta) + (v.y * cosTheta);

        return new Vector2(newX, newY);
    }
}
