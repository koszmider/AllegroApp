using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Bll
{
    /// <summary>
    /// This class will be used to
    /// store  ThreadResult in a
    /// HashTable which has been
    /// declared as static.
    /// </summary>
    public class ThreadResult
    {
        private static System.Collections.Hashtable
   ThreadsList = new System.Collections.Hashtable();

        public static void Add(string key, object value)
        {
            ThreadsList.Add(key, value);
        }

        public static object Get(string key)
        {
            return ThreadsList[key];
        }

        public static void Remove(string key)
        {
            ThreadsList.Remove(key);
        }

        public static bool Contains(string key)
        {
            return ThreadsList.ContainsKey(key);
        }

    }
}
