using System.Reflection;

namespace ImpoDoc.Common
{
    public static class Utils
    {
        public static T CloneObject<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }

            MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return inst != null ? (T)inst.Invoke(obj, null) : null;
        }
    }
}
