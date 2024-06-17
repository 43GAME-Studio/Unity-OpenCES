using FTGAMEStudio.Timer.DeltaTimers;
using System;
using UnityEngine;

namespace FTGAMEStudio.Date
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

        public readonly int SecondOfDay => hour * 3600 + minute * 60 + second;
        public readonly int SecondOfAll => year * 31557600 + hour * 3600 * minute * 60 + second;

        public DateTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public void UpdateMonth()
        {
            int yearOffset = Mathf.FloorToInt(month / 12);
            month -= 12 * yearOffset;
            year += yearOffset;
        }
        public void UpdateDay()
        {
            int maxDay = month switch { 4 => 30, 6 => 30, 9 => 30, 11 => 30, 2 => 28, _ => 31 };
            int monthOffset = Mathf.FloorToInt(day / maxDay);
            day -= maxDay * monthOffset;
            month += monthOffset;
        }
        public void UpdateHour()
        {
            int dayOffset = Mathf.FloorToInt(hour/24);
            hour -= 24 * dayOffset;
            day += dayOffset;
        }
        public void UpdateMinute()
        {
            int hourOffset = Mathf.FloorToInt(minute/60);
            minute -= 60 * hourOffset;
            hour += hourOffset;
        }
        public void UpdateSecond()
        {
            int minuteOffset = Mathf.FloorToInt(second/60);
            second -= 60 * minuteOffset;
            minute += minuteOffset;
        }

        public void Update()
        {
            UpdateSecond();
            UpdateMinute();
            UpdateHour();
            UpdateDay();
            UpdateMonth();
        }

        public void AddYear() => year++;
        public void AddMonth()
        {
            month++;
            Update();
        }
        public void AddDay()
        {
            day++;
            Update();
        }

        public void AddHour()
        {
            hour++;
            Update();
        }
        public void AddMinute()
        {
            minute++;
            Update();
        }
        public void AddSecond()
        {
            second++;
            Update();
        }

        public static DateTime Normal { get; } = new() {year = 1, month = 1, day = 1 };

        public readonly string GetTime(string interval = ": ", bool second = false) => hour.ToString("D2") + interval + minute.ToString("D2") + (second ? interval + this.second.ToString("D2") : "");
        public readonly string GetDate(string interval = ": ") => year.ToString("D4") + interval + month.ToString("D2") + interval + day.ToString("D2");

        public static DateTime operator +(DateTime v1, DateTime v2)
        {
            v1.second += v2.second;
            v1.minute += v2.minute;
            v1.hour += v2.hour;
            v1.day += v2.day;
            v1.month += v2.month;
            v1.year += v2.year;
            v1.Update();
            return v1;
        }

        public static DateTime operator -(DateTime v1, DateTime v2)
        {
            v1.second -= v2.second;
            v1.minute -= v2.minute;
            v1.hour -= v2.hour;
            v1.day -= v2.day;
            v1.month -= v2.month;
            v1.year -= v2.year;
            v1.Update();
            return v1;
        }

        public static DateTime operator *(DateTime v1, float v2)
        {
            DateTime dateTime = Normal;
            dateTime.second = (int)(v1.SecondOfAll * v2);
            dateTime.Update();
            return dateTime;
        }

        public static DateTime operator /(DateTime v1, float v2)
        {
            DateTime dateTime = Normal;
            dateTime.second = (int)(v1.SecondOfAll / v2);
            dateTime.Update();
            return dateTime;
        }

        public static explicit operator System.DateTime(DateTime dateTime) => new(dateTime.year, dateTime.month, dateTime.day, dateTime.hour, dateTime.minute, dateTime.second);
        public static explicit operator DateTime(System.DateTime dateTime) => new(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

    [Serializable]
    public class DateTimer : DeltaTimer
    {
        public Action<DateTime> action;

        public DateTime dateTime = DateTime.Normal;

        public DateTimer(Action<DateTime> action, float time, EventInformation eventInformation, bool repeat = false, string name = null) : base(time, eventInformation, repeat, name)
        {
            this.action = action;
        }

        protected override void Invoke()
        {
            action?.Invoke(dateTime);
            dateTime.AddSecond();
        }
    }
}