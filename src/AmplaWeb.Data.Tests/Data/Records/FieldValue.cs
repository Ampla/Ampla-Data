using System;

namespace AmplaWeb.Data.Records
{
    public class FieldValue : IComparable<FieldValue>
    {
        public FieldValue()
            : this("Field", "Value")
        {
        }

        public FieldValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Value
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            string thisValue = ToString();
            string otherValue = obj != null ? obj.ToString() : string.Empty;
            return thisValue == otherValue;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(FieldValue other)
        {
            int compare = 0;

            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(Name, other.Name);
            }
            if (compare == 0)
            {
                compare = StringComparer.InvariantCulture.Compare(Value, other.Value);
            }
            return compare;
        }

        public override string ToString()
        {
            const bool isRequired = false;
            const bool isReadOnly = false;
            return string.Format("[{0}] = {1}{2}{3}", Name, Value, isRequired ? " {Required}" : "", isReadOnly ? " {ReadOnly}" : "");
        }

        public virtual FieldValue Clone()
        {
            return new FieldValue(Name, Value) ;
        }

        public void SetValue<T>(T value)
        {
            Value = PersistenceHelper.ConvertToString(value);
        }

        public T GetValue<T>()
        {
            return PersistenceHelper.ConvertFromString<T>(Value);
        }
    }
}