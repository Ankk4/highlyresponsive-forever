using System;
using UnityEngine;

public class Save
{
    public enum Save_Keys
    {
        Current_Level,
        Highscore,
        Current_Score,
        Max_Combo,
        Current_Combo,
    };

    public static string[] Keys = Enum.GetNames(typeof(Save_Keys));

    public static string GetKey(Save_Keys type)
    {
        return Keys[(int)type];
    }

    public static int? GetPlayerPrefInt(Save_Keys type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {

            return null;

            //throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetInt(type.ToString());
    }

    public static string GetPlayerPrefString(Save_Keys type, string extra = "")
    {
        if (!PlayerPrefs.HasKey(type.ToString() + extra))
        {
            return null;
            //throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetString(type.ToString() + extra);
    }

    public static float? GetPlayerPrefFloat(Save_Keys type)
    {
        if (!PlayerPrefs.HasKey(type.ToString()))
        {
            return null;
            //throw new UnityException("Key '" + type + "' does not exist!");
        }

        return PlayerPrefs.GetFloat(type.ToString());
    }
}