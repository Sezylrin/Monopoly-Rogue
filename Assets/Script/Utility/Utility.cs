using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Returns the first equivalent position of the enum within the list of enums for flag type enums
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static int EnumPos<T>(T enumValue) where T : Enum
    {
        int final = -1;
        int n = Convert.ToInt32(enumValue);
        for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
        {
            if((n & 1) == 1) 
            {
                final = i;
                break;
            }
            n >>= 1;        // Right shift to check the next bit
        }
        return final;
    }
    /// <summary>
    /// Returns a list of positions for all detected enums in the given enum value for flag type enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static List<int> EnumPoses<T>(T enumValue) where T : Enum
    {
        List<int> ints = new List<int>();
        int n = Convert.ToInt32(enumValue);
        for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
        {
            if ((n & 1) == 1)
            {
                ints.Add(i);
            }
            n >>= 1;        // Right shift to check the next bit
        }

        return ints;
    }
}
