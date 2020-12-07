using System.ComponentModel;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToString(this Object obj, string format)
        {
            Type t = obj.GetType();
            Reflection.MethodInfo tostringMethod = t.GetMethod("ToString", Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
            if (tostringMethod == null)
            {
                return obj.ToString();
            }
            else
            {
                //Reflection.ParameterInfo[] paramsInfo = tostringMethod.GetParameters();//得到指定方法的参数列表 
                return tostringMethod.Invoke(obj, new object[] { format }).ToString();
            }
        }

        public static string GetDisplayName(this Object obj)
        {
            Type t = obj.GetType();
            DisplayNameAttribute[] attributes = null;

            if (t == typeof(Enum))
            {
                FieldInfo fi = t.GetField(obj.ToString());
                attributes = (DisplayNameAttribute[])fi.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            }
            else
            {
                attributes = (DisplayNameAttribute[])t.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            }

            if (attributes != null && attributes.Length > 0)
                return attributes[0].DisplayName;
            else
                return null;
        }

        public static string GetDescription(this Object obj)
        {
            Type t = obj.GetType();
            DescriptionAttribute[] attributes = null;

            if (t == typeof(Enum))
            {
                FieldInfo fi = t.GetField(obj.ToString());
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            else
            {
                attributes = (DescriptionAttribute[])t.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return null;
        }
    }
}
