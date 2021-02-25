using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Util.Reflection
{
    public class Properties
    {
        public Properties()
        {

        }

        public Properties(PropType type, Type t, string name, object value, int index = 0)
        {
            PropType = type;
            Type = t.Name;
            Name = name ?? "";
            Value = value?.ToString() ?? "null";
            NestedIndex = index;
        }

        private PropType propType;

        private int nestedIndex;

        private string type;
        private string name;
        private string value;

        public PropType PropType { get => propType; set => propType = value; }

        public int NestedIndex { get => nestedIndex; set => nestedIndex = value; }

        public string Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
    }

    public enum PropType { Instance, Field, Property }
}
