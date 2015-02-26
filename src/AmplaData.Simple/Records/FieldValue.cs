using System;

namespace AmplaData.Records
{
    public class FieldValue : IComparable<FieldValue>
    {
        public FieldValue(string name, string value, int? id = null)
        {
            Name = name;
            Value = value;
            Id = id;
        }

        public int? Id { get; set; }

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

        public sealed override bool Equals(object obj)
        {
            string thisValue = ToString();
            string otherValue = obj != null ? obj.ToString() : string.Empty;
            return thisValue == otherValue;
        }

        public sealed override int GetHashCode()
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
            if (Id.HasValue)
            {
                return string.Format("[{0}] = {1} ({2})", Name, Value, Id);
            }
            return string.Format("[{0}] = {1}", Name, Value);
        }

        public FieldValue Clone()
        {
            return new FieldValue(Name, Value, Id) ;
        }

        public void SetValue<T>(T value)
        {
            Value = PersistenceHelper.ConvertToString(value);
        }

        public void SetIdValue<T>(T value, int id)
        {
            Value = PersistenceHelper.ConvertToString(value);
            Id = id;
        }

        public T GetValue<T>()
        {
            return PersistenceHelper.ConvertFromString<T>(Value);
        }

        public string ResolveValue(bool resolveId)
        {
            return resolveId ? Value : Id.HasValue ? Convert.ToString(Id) : Value;
        }
    }
}