using System.Collections.Generic;

namespace AmplaData.Logging
{
    public class ListLogger : ILogger
    {
        private readonly List<string> list = new List<string>();
        public void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            list.Add(message);
        }

        public IList<string> Messages
        {
            get
            {
                return list.AsReadOnly();
            }
        }
    }
}