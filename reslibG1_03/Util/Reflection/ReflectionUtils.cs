using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Reflection
{
    public class ReflectionUtils
    {
        internal static int max = 4;
        internal static int current = 0;


        public static List<Properties> GetValuesFromObj<T>(T obj)
        {
            //if (obj is null)
            //    return null;

            current = 0;

            List<Properties> properties = GetValuesRecursion(obj, null, false);

            current = 0;

            return properties;
        }

        public static List<Properties> GetValuesFromObj<T>(T obj, string objName)
        {
            current = 0;

            List<Properties> properties = GetValuesRecursion(obj, objName, false);

            current = 0;

            return properties;
        }


        internal static List<Properties> GetValuesRecursion<T>(T obj, string name, bool allAllowed, Type[] allowedTypes = null, int index = 0)
        {
            List<Properties> propList = new();
            Type t = obj.GetType();

            if (index > max)
                return propList;

            if (allowedTypes is null)
                allowedTypes = new Type[0];


            if (t.IsPrimitive || t == typeof(string))
            {
                propList.Add(new(PropType.Instance, t, name, obj, index));
            }
            else if (t.IsArray)
            {
                propList.Add(new(PropType.Instance, t, "Array of type " + t.Name, null, index));

                foreach (var item in obj as IEnumerable)
                {
                    propList.AddRange(GetValuesRecursion(item, name, allAllowed, allowedTypes, index + 1));
                }
            }
            else if (t.IsGenericType)
            {
                var sub = t.GetGenericTypeDefinition();
                var listT = t.GetGenericArguments().FirstOrDefault();

                if (sub == typeof(List<>) || sub == typeof(ObservableCollection<>))
                {
                    propList.Add(new(PropType.Instance, listT, "List of type " + listT.Name, null, index));

                    foreach (var item in obj as IEnumerable)
                    {
                        propList.AddRange(GetValuesRecursion(item, name, allAllowed, allowedTypes, index + 1));
                    }
                }
            }
            else if (allowedTypes.Contains(t) || allAllowed)
            {
                var props = t.GetProperties();
                var fields = t.GetFields();

                foreach (var prop in props)
                {
                    var propT = prop.PropertyType;
                    var value = obj is not null ? prop.GetValue(obj) : null;

                    propList.AddRange(GetValuesRecursion(value, prop.Name, allAllowed, allowedTypes, index));
                }

                foreach (var field in fields)
                {
                    var fieldT = field.FieldType;
                    var value = obj != null ? field.GetValue(obj) : null;

                    propList.AddRange(GetValuesRecursion(value, field.Name, allAllowed, allowedTypes, index));
                }
            }
            else
            {
                propList.Add(new(PropType.Instance, t, name, obj, index));
            }

            return propList;
        }
    }
}
