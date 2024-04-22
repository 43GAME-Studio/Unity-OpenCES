using System;
using UnityEngine;

namespace FSGAMEStudio.Date
{
    /// <summary>
    /// 可序列化时间与日期
    /// </summary>
    [Serializable]
    public struct DateTime
    {
        [Header("日期")]
        public int year;
        [Range(1, 12)] public int month;
        [Range(1, 31)] public int day;

        [Header("时间")]
        [Range(0, 23)] public int hour;
        [Range(0, 59)] public int minute;
        [Range(0, 59)] public int second;
        [Space]
        [NonSerialized] public int secondOfDay;

        public void AddYear() => year++;
        public void AddMonth()
        {
            month++;

            if (month > 12) { month = 1; AddYear(); }
        }
        public void AddDay()
        {
            day++;

            switch (month)
            {
                case 4: case 6: case 9: case 11:
                    if (day == 30) { day = 0; AddMonth(); }
                    break;
                case 2:
                    if (day == 28) { day = 0; AddMonth(); }
                    break;
                default:
                    if (day == 31) { day = 0; AddMonth(); }
                    break;
            }
        }

        public void AddHour()
        {
            hour++;

            if (hour > 23) { hour = 0; AddDay(); }
        }
        public void AddMinute()
        {
            minute++;

            if (minute > 59) { minute = 0; AddHour(); }
        }
        public void AddSecond()
        {
            second++;

            if (second > 59) { second = 0; AddMinute(); }

            UpdateSecondOfDay();
        }

        public void UpdateSecondOfDay()
        {
            secondOfDay = hour * 3600 + minute * 60 + second;
        }

        public static DateTime Default { get; } = new() { month = 1, day = 1};
    }
}