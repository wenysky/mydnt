using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;

namespace Discuz.EntLib.ToolKit
{
    public class PerfCounterFactory
    {
        static PerfCounterFactory _instance = new PerfCounterFactory();
        Dictionary<string, PerformanceCounter> _counters = new Dictionary<string, PerformanceCounter>();
        public static PerfCounterFactory Instance
        {
            get { return _instance; }
        }
        public PerformanceCounter GetCounter(string key)
        {
            PerformanceCounter counter = null;
            lock (_counters)
            {
                if (!_counters.TryGetValue(key, out counter))
                {
                    string[] strs = key.Split('|');
                    if (strs.Length == 2)
                    {
                        counter = new PerformanceCounter(strs[0], strs[1]);
                    }
                    else if (strs.Length == 3)
                    {
                        counter = new PerformanceCounter(strs[0], strs[1], strs[2]);
                    }
                    else if (strs.Length == 4)
                    {
                        counter = new PerformanceCounter(strs[0], strs[1], strs[2], strs[3]);
                    }
                    else
                    {
                        throw new ArgumentException(key);
                    }
                    _counters[key] = counter;
                }
            }
            return counter;
        }
    }
}
