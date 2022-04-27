using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DateManager : MonoBehaviour {
    private static bool created = false;
    private static DateTimeWrapper lastAdWatchedTime;
    private static DateTimeWrapper nextAdTime;
    private static bool adAvailable;
    public static bool AdAvailable { get { return adAvailable; } }

    private void Awake() {
        if(created) {
            DestroyImmediate(gameObject);
        } else {
            created = true;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    public static void VerifyDate() {
        DateTimeWrapper currentTime = new DateTimeWrapper(DateTime.Now);

        if (lastAdWatchedTime == null || nextAdTime == null) {
            adAvailable = true;
            return;
        }
        
        if (nextAdTime.time < DateTime.Now) adAvailable = true;
        else adAvailable = false;
    }

    public static void UpdateTime() {
        lastAdWatchedTime = new DateTimeWrapper(DateTime.Now);
        PlayerPrefs.SetString("AdTime", lastAdWatchedTime.time.ToString());
        CalculateNextAddTime();
    }

    private static void Initialize() {
        if (!PlayerPrefs.HasKey("AdTime")) {
            lastAdWatchedTime = null;
            adAvailable = true;
        } else {
            try {
                lastAdWatchedTime = new DateTimeWrapper(DateTime.Parse(PlayerPrefs.GetString("AdTime")));
                CalculateNextAddTime();
            }catch(FormatException e) {
                lastAdWatchedTime = null;
                adAvailable = true;
            }
        }

    }

    private static void CalculateNextAddTime() {
        string nextDate;
        int nextHour;
        int nextMinute = (lastAdWatchedTime.minute + 6) % 60;

        Debug.Log("Hour: " + lastAdWatchedTime.hour + " Minute: " + lastAdWatchedTime.minute);

        if(lastAdWatchedTime.hour < 23 || (lastAdWatchedTime.minute < 55 && lastAdWatchedTime.hour == 23)) {
            Debug.Log("True 1");
            nextDate = lastAdWatchedTime.date;
            if (lastAdWatchedTime.minute < 55) {
                nextHour = lastAdWatchedTime.hour;
            } else {
                nextHour = lastAdWatchedTime.hour ++;
            }
        } else {
            Debug.Log("True 2");
            nextHour = 0;
            nextDate = lastAdWatchedTime.GetNextDate();
        }

        nextAdTime = new DateTimeWrapper(nextDate, nextHour, nextMinute, 0);

        Debug.Log("Next Ad Time: " + nextAdTime);
    }

    public static int GetTimeLeft() {
        return nextAdTime.GetDifference(new DateTimeWrapper(DateTime.Now));
    }

    class DateTimeWrapper {
        public DateTime time;
        public string date;
        public int hour;
        public int minute;
        public int second;

        public DateTimeWrapper(DateTime time) {
            this.time = time;
            InitializeVariables();
        }

        public DateTimeWrapper(string date, int hour, int minute, int second) {
            this.date = date;
            this.hour = hour;
            this.minute = minute;
            this.second = second;

            time = new DateTime(GetYear(), GetMonth(), GetDay(), hour, minute, second);
        }

        private int GetDay() {
            string[] tokens = date.Split('/');

            return Int32.Parse(tokens[0]);
        }

        private int GetMonth() {
            string[] tokens = date.Split('/');

            return Int32.Parse(tokens[1]);
        }

        private int GetYear() {
            string[] tokens = date.Split('/');

            return Int32.Parse(tokens[2]);
        }

        private void InitializeVariables() {
            date = time.Date.ToString("dd/MM/yyyy");
            hour = time.Hour;
            minute = time.Minute;
            second = time.Second;
        }
        
        public string GetNextDate() {
            DateTime timeCopy = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            timeCopy.AddDays(1);
            DateTimeWrapper wrapper = new DateTimeWrapper(timeCopy);

            return wrapper.date;
        }

        public override string ToString() {
            return time.ToString();
        }

        public int GetDifference(DateTimeWrapper other) { //Note that this is assuming that the max difference is 5 mins
           return (int)(time.Subtract(other.time).TotalSeconds);
        }
    }
}


