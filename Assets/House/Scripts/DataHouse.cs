using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class HouseTextOnShop //дание для кнопок в магазине
{
    [HideInInspector] public DataHouseChangeOnText dataHouseChangeOnText;
    public int MaxCountBuild;
    public TimeBuild TimeNeedBuildStart;
    [HideInInspector] public ButtonChange buttonChange;
}
[Serializable]
public class TimeBuild
{
    public int days, hours, minutes, seconds;
    public TimeBuild(int _days, int _hours, int _minutes, int _seconds)
    {
        days = _days;

        if (_hours > 23) _hours = 23;
        if (_hours < 0) _hours = 0;
        hours = _hours;

        if (_minutes > 59) _minutes = 59;
        if (_minutes < 0) _minutes = 0;
        minutes = _minutes;

        if (_seconds > 59) _seconds = 59;
        if (_seconds < 0) _seconds = 0;
        seconds = _seconds;
    }
    public static string ToString(TimeBuild timeBuild)
    {
        string day = timeBuild.days != 0 ? timeBuild.days.ToString() + "d" : null;
        string hour = timeBuild.hours != 0 ? timeBuild.hours.ToString() + "h" : null;
        string minute = timeBuild.minutes != 0 ? timeBuild.minutes.ToString() + "m" : null;
        string second = timeBuild.seconds != 0 ? timeBuild.seconds.ToString() + "s" : null;
        return day + hour + minute + second;
    }
    public static int InSeconds(TimeBuild timeBuild)
    {
        int _seconds = 0;
        _seconds += timeBuild.seconds;
        _seconds += timeBuild.minutes * 60;
        _seconds += timeBuild.hours * 3600;
        _seconds += timeBuild.days * 24 * 3600;
        return _seconds;
    }

}
[Serializable]
public class DataTextOnHouse
{
    public int priceImprove;
    public TimeBuild timeImprove;
    public TypeHouse typeHouse;
    public Info info;
}

[Serializable]
public class Info
{
    public Sprite spriteHouse;
}






















[Serializable]
public class AllData
{
    public List<AllDataHouse> allDataHouses;
}

[Serializable]
public class AllDataHouse
{
    public DataHouse dataHouse;
    public DataHouseChangeOnText dataHouseChangeOnText;
}

[Serializable]
public class DataHouse
{
    public NameHouse NameThisHouse; //**** коли ставлю
    [HideInInspector] public Posit posit;
    public DataAnimBuildHouse dataAnimBuildHouse;
    [HideInInspector] public int myIndexOnSave;  //**** коли ставлю
    public int levelHouse;
}
[Serializable]
public class DataAnimBuildHouse
{
    public TimeDateTime timeEndBuild;
}
[Serializable]
public class TimeDateTime
{
    public int years, month, days, hours, minutes, seconds;
    public TimeDateTime(int _years, int _month, int _days, int _hours, int _minutes, int _seconds)
    {
        years = _years;
        month = _month;
        days = _days;
        hours = _hours;
        minutes = _minutes;
        seconds = _seconds;
    }
    public static string ToString(TimeDateTime timeDateTime)
    {
        string day = timeDateTime.days.ToString() + ".";
        string month = timeDateTime.month.ToString() + ".";
        string years = timeDateTime.years.ToString() + " ";
        string hour = timeDateTime.hours.ToString() + ":";
        string minute = timeDateTime.minutes.ToString() + ":";
        string second = timeDateTime.seconds.ToString();
        return day + month + years + hour + minute + second;
    }
    public static TimeDateTime ToTimeDateTime(string timeDateTime)
    {
        string[] stringAll = timeDateTime.Split(' ');

        string[] stringDMY = stringAll[0].Split('.');
        string[] stringHMS = stringAll[1].Split(':');
        int _seconds = Int32.Parse(stringHMS[2]);
        int _minutes = Int32.Parse(stringHMS[1]);
        int _hours = Int32.Parse(stringHMS[0]);

        int _day = Int32.Parse(stringDMY[0]);
        int _month = Int32.Parse(stringDMY[1]);
        int _years = Int32.Parse(stringDMY[2]);
        return new TimeDateTime(_years, _month, _day, _hours, _minutes, _seconds);
    }
    public static TimeDateTime TimeMinusTime(TimeDateTime time1, TimeDateTime time2)
    {
        bool PlusOrMinus = CompareTimeDateTime(time1, time2);
        if (PlusOrMinus)
        {
            return InitValues(time1, time2);
        }
        else
        {
            Debug.Log("мінус");
            return InitValues(time2, time1);
        }

        TimeDateTime InitValues(TimeDateTime _time1, TimeDateTime _time2)
        {
            int _seconds = _time1.seconds - _time2.seconds;
            int deltaMinutes = 0;
            if (_seconds < 0)
            {
                _seconds = 60 + _seconds;
                deltaMinutes = -1;
            }

            int _minutes = _time1.minutes - _time2.minutes + deltaMinutes;
            int deltaHours = 0;
            if (_minutes < 0)
            {
                _minutes = 60 + _minutes;
                deltaHours = -1;
            }

            int _hours = _time1.hours - _time2.hours + deltaHours;
            int deltaDays = 0;
            if (_hours < 0)
            {
                _hours = 24 + _hours;
                deltaDays = -1;
            }

            int _month = _time1.month - _time2.month;
            int deltaYears = 0;
            if (_month < 0)
            {
                _month = 12 + _month;
                deltaYears = -1;
            }

            int _years = _time1.years - _time2.years + deltaYears;

            int _day = _time1.days - _time2.days + deltaDays;
            if (_day < 0)
            {
                --_month;
                switch (_time2.month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        _day = 31 + _day;

                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        _day = 30 + _day;
                        break;
                    case 2:
                        if (_years % 4 == 0)
                        {
                            _day = 29 + _day;
                        }
                        else
                        {
                            _day = 28 + _day;
                        }
                        break;
                }
            }
            if (PlusOrMinus)
            {
                return new TimeDateTime(_years, _month, _day, _hours, _minutes, _seconds);
            }
            else
            {
                Debug.Log(_minutes + " " + _seconds);
                return new TimeDateTime(_years*(-1), _month * (-1), _day * (-1), _hours * (-1), _minutes * (-1), _seconds * (-1));
            }
        }
    }
    public static TimeDateTime GetSum(TimeDateTime timeDate, TimeBuild timeBuild)
    {
        int _seconds = timeDate.seconds + timeBuild.seconds;
        int deltaMinutes = 0;
        if (_seconds > 59)
        {
            _seconds -= 60;
            deltaMinutes = 1;
        }

        int _minutes = timeDate.minutes + timeBuild.minutes + deltaMinutes;
        int deltaHours = 0;
        if (_minutes > 59)
        {
            _minutes -= 60;
            deltaHours = 1;
        }

        int _hours = timeDate.hours + timeBuild.hours + deltaHours;
        int deltaDays = 0;
        if (_hours > 23)
        {
            _hours -= 24;
            deltaDays = 1;
        }

        int _month = timeDate.month;
        int deltaYears = 0;
        if (_month > 12)
        {
            _month -= 12;
            deltaYears = 1;
        }

        int _years = timeDate.years + deltaYears;

        int _day = timeDate.days + timeBuild.days + deltaDays;
        switch (_month)
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                if (_day > 31)
                {
                    _day -= 31;
                    _month++;
                }
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                if (_day > 30)
                {
                    _day -= 30;
                    _month++;
                }
                break;
            case 2:
                if (_years % 4 == 0)
                {
                    if (_day > 29)
                    {
                        _day -= 29;
                        _month++;
                    }
                }
                else
                {
                    if (_day > 28)
                    {
                        _day -= 28;
                        _month++;
                    }
                }
                break;
        }

        return new TimeDateTime(_years, _month, _day, _hours, _minutes, _seconds);
    }
    public static int InSeconds(TimeDateTime timeDate) // для секунд в разнице времени, там нет месяцев
    {
        TimeDateTime time = new TimeDateTime(timeDate.years, timeDate.month, timeDate.days, timeDate.hours, timeDate.minutes, timeDate.seconds); // щоб перевірилось
        //Debug.Log("startCheckData");
        //Debug.Log(time.seconds);
        //Debug.Log(time.minutes);
        //Debug.Log(time.hours);
        //Debug.Log(time.days);

        int _seconds = 0;
        _seconds += time.seconds;
        _seconds += time.minutes * 60;
        _seconds += time.hours * 3600;
        _seconds += time.days * 24 * 3600;
        return _seconds;
    }
    public static bool CompareTimeDateTime(TimeDateTime time1, TimeDateTime time2) //true якщо перший більший
    {
        if(time1.years > time2.years) { return true; }
        else if (time1.years < time2.years) { return false; }

        if (time1.month > time2.month) { return true; }
        else if (time1.month < time2.month) { return false; }

        if (time1.days > time2.days) { return true; }
        else if (time1.days < time2.days) { return false; }

        if (time1.hours > time2.hours) { return true; }
        else if (time1.hours < time2.hours) { return false; }

        if (time1.minutes > time2.minutes) { return true; }
        else if (time1.minutes < time2.minutes) { return false; }

        if (time1.seconds > time2.seconds) { return true; }
        else if (time1.seconds < time2.seconds) { return false; }

        return false;
    }
}

[Serializable]
public class DataHouseChangeOnText
{
    public int currentBuildThisHouse;  //**** коли ставлю
}
[Serializable]
public class Posit
{
    public int x, z;
    public Posit(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
    public static Vector2 InitInPosit(int x,int z,House house)
    {
        float X = (x + (float)house.NeParniX / 2f + MyTerrain.xMin) * MyTerrain.sizeOneCell;
        float Z = (z + (float)house.NeParniZ / 2f + MyTerrain.zMin) * MyTerrain.sizeOneCell;
        return new Vector2(X,Z);
    }
    public static Vector2Int DesWithPosit(float x, float z, House house)
    {
        int X = Mathf.RoundToInt( x / MyTerrain.sizeOneCell - MyTerrain.xMin - (float)house.NeParniX / 2f);
        int Z = Mathf.RoundToInt(z / MyTerrain.sizeOneCell - MyTerrain.xMin - (float)house.NeParniZ / 2f);
        return new Vector2Int(X, Z);
    }
}