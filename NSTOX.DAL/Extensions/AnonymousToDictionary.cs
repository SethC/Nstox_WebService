using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using NSTOX.DAL.Model;

namespace NSTOX.DAL
{
    public static class AnonymousToDictionary
    {
        /// <summary>
        /// This method will try to build a Dictionary from an object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="excludeFlaggedOnInsert">If true, will exclude all the properties that have the UpdateableField and the ExcludeOnInsert attribute property true</param>
        /// <param name="excludeFlaggedOnUpdate">If true, will exclude all the properties that have the UpdateableField and the ExcludeOnUpdate attribute property true</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object data, bool excludeFlaggedOnInsert = false, bool excludeFlaggedOnUpdate = false)
        {
            if (data == null)
                return null;

            bool isAnonymousType = IsAnonymousType(data.GetType());

            BindingFlags attr = BindingFlags.Public | BindingFlags.Instance;
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (PropertyInfo property in data.GetType().GetProperties(attr))
            {
                if (isAnonymousType)
                {
                    dict.Add(property.Name, property.GetValue(data, null));
                }
                else
                {
                    UpdateableField attrib = property.GetCustomAttributes(typeof(UpdateableField), true).FirstOrDefault() as UpdateableField;

                    if (property.CanRead && attrib != null)
                    {
                        if ((excludeFlaggedOnInsert && attrib.ExcludeOnInsert) || (excludeFlaggedOnUpdate && attrib.ExcludeOnUpdate))
                        {
                            continue;
                        }

                        string propName = string.IsNullOrEmpty(attrib.ParamName) ? property.Name : attrib.ParamName;
                        object propValue = property.GetValue(data, null);
                        dict.Add(propName, propValue == null ? DBNull.Value : propValue);
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// This method will flat a dictionary to a string having the following format: key1: value1, key2: value2, etc. 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string ToFormattedString(this Dictionary<string, object> dict)
        {
            if (dict == null)
                return "no params";

            string result = string.Empty;
            foreach (string key in dict.Keys)
            {
                result += string.Format("{0}: {1}, ", key, dict[key] == null ? "NULL" : dict[key]);
            }
            result = result.TrimEnd(",".ToCharArray());
            return result;
        }

        /// <summary>
        /// This method will try to convert an object to string
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the string value of the input, or null</returns>
        public static string SafelyToString(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return field.ToString();
            }
            return null;
        }

        /// <summary>
        /// This method will try to convert an object to string
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the string value of the input, or string.empty</returns>
        public static string SafelyToStringDefaultEmpty(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return field.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// This method will try to convert an object to DateTime
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the DateTime value of the input, or DateTime.MinValue</returns>
        public static DateTime SafelyToDateTime(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToDateTime(field.ToString());
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// This method will try to convert an object to DateTime
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the DateTime value of the input, or DateTime.MinValue</returns>
        public static DateTime? SafelyToNullableDateTime(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToDateTime(field.ToString());
            }
            return null;
        }

        /// <summary>
        /// This method will try to convert an object to int
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the int value of the input, or 0</returns>
        public static int SafelyToInt(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToInt32(field.ToString());
            }
            return 0;
        }

        /// <summary>
        /// This method will try to convert an object to int
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the int value of the input, or 0</returns>
        public static int? SafelyToNullableInt(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToInt32(field.ToString());
            }
            return null;
        }

        /// <summary>
        /// This method will try to convert an object to int
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the int value of the input, or 0</returns>
        public static long SafelyToLong(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToInt64(field.ToString());
            }
            return 0;
        }

        /// <summary>
        /// This method will try to convert an object to int
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the int value of the input, or 0</returns>
        public static bool SafelyToBool(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToBoolean(field.ToString());
            }
            return false;
        }

        /// <summary>
        /// This method will try to convert an object to a given enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <returns>the enum value of the input, or default(enum)</returns>
        public static T SafelyToEnum<T>(this object field) where T : struct
        {
            if (field != DBNull.Value && field != null)
            {
                string value = field.ToString();
                value = Regex.Replace(value, @"\s", "");

                T result;

                if (Enum.TryParse<T>(value, out result))
                {
                    return result;
                }
                return default(T);
            }
            return default(T);
        }

        /// <summary>
        /// This method will try to convert an object to decimal
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the decimal value of the input, or 0</returns>
        public static decimal SafelyToDecimal(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToDecimal(field.ToString());
            }
            return 0;
        }

        /// <summary>
        /// This method will try to convert an object to decimal
        /// </summary>
        /// <param name="field"></param>
        /// <returns>the decimal value of the input, or 0</returns>
        public static double SafelyToDouble(this object field)
        {
            if (field != DBNull.Value && field != null)
            {
                return Convert.ToDouble(field.ToString());
            }
            return 0;
        }

        /// <summary>
        /// This method verifies if a given type is anonymous (created with: new {Name1 = value1, Name2 = value2 })
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Boolean IsAnonymousType(this Type type)
        {
            Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }

}
