using System;
using UnityEngine;

namespace Timeline
{
    public static class Timer
    {
        public static string ValToAnimTime(float normVal, float length)
        {
            int seconds = Mathf.FloorToInt(normVal * length);
            int minutes = Mathf.FloorToInt(seconds / 60);

            string time = minutes.ToString("00") + ":" + (seconds - (minutes * 60)).ToString("00");

            return time;
        }

        public static DateTime ValToAnimDate(DateTime myDt, float normVal, float length)
        {
            int seconds = Mathf.FloorToInt(normVal * length);
            DateTime _myDt = myDt;

            var date = _myDt.AddDays(seconds);

                return date;
        }
    }
}