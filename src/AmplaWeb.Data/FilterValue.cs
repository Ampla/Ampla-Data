namespace AmplaWeb.Data
{
    public class FilterValue
    {
        public FilterValue()
        {
            
        }

        public FilterValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return string.Format("FilterValue ({0} = {1})", Name, Value);
        }
    }
}