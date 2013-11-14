using System;

namespace AmplaData.WebService
{
    public class WebServiceFactory<T>
    {
        public static Func<T> Factory { private get; set; }

        public static T Create()
        {
            return Factory();
        }
    }
}