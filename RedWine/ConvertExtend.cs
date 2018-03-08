using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RedWine
{
    public static class ConvertExtend
    {
        public static async Task<DataTable> ToDataTable<T>(this IEnumerable<T> collection)
        {
            var t = Task.Run(() =>
            {
                var props = typeof(T).GetProperties();
                var dt = new DataTable();
                dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
                if (collection.Count() > 0)
                {
                    for (int i = 0; i < collection.Count(); i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in props)
                        {
                            object obj = pi.GetValue(collection.ElementAt(i), null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        dt.LoadDataRow(array, true);
                    }
                }
                foreach (var p in props)
                {
                    var ms = (DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute));
                    dt.Columns[p.Name].ColumnName = ms == null ? p.Name : ms.Description;
                }
                return dt;
            });
            return await t;
        }
        public static DateTime GetStartTime(this DateTime datetime)
        {
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }
        public static int NumWeek(this DateTime time)
        {
            var date = time;
            var FirstOfMonth = DateTime.Parse(date.Year + "-" + date.Month + "-" + "1");
            int i = (int)FirstOfMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }
            return (date.Day + i - 2) / 7;
        }
        public static string ODtToStr(this string o)
        {
            return o ?? o.Replace(" 0:00:00", "");
        }
        public static decimal To2Point(this decimal o, int length = 2)
        {
            return Math.Round(o, length, MidpointRounding.AwayFromZero);
        }
        public static string ToRound(this decimal o)
        {
            string r = o.ToString();
            if (r.IndexOf(".") != -1)
            {
                var l = r.Split('.')[1].Length;
                if (l > 2)
                    return r.Substring(0, r.IndexOf(".") + 3);
                if (l == 1)
                    return r + "0";
                else
                    return r;
            }
            return o.ToString() + ".00";
        }
        public static decimal ToDecimal(this object o)
        {
            decimal result = 0;
            var r = o == null ? "0" : o.ToString();
            decimal.TryParse(r, out result);
            return result;
        }
        public static int ToInt(this object o)
        {
            int result = 0;
            var r = o == null ? "0" : o.ToString();
            Int32.TryParse(r, out result);
            return result;
        }
        public static string DtToStr(this DateTime? o)
        {
            return o.HasValue ? o.Value.ToString("yyyy-MM-dd") : "";
        }
        public static DateTime StrToDt(this string o)
        {
            return DateTime.Parse(o);
        }
        public static string DtStrToStr(this string o)
        {
            return DateTime.Parse(o).ToString("yyyy-MM-dd");
        }
    }
}
