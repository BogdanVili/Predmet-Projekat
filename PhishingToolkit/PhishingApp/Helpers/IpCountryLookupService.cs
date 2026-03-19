using System;
using System.Collections.Generic;
using System.IO;

namespace PhishingApp.Helpers
{
    public class IpCountryLookupService
    {
        private static IpCountryLookupService _instance;

        public static IpCountryLookupService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new IpCountryLookupService();

                return _instance;
            }
        }
        private IpCountryLookupService() { }

        private IpRange[] _ranges;

        private struct IpRange
        {
            public uint Start;
            public uint End;
            public string Country;
        }

        public void Load(string filePath)
        {
            var list = new List<IpRange>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("start_ip"))
                    continue;

                var parts = SplitCsv(line);

                uint start = uint.Parse(parts[0]);
                uint end = uint.Parse(parts[1]);
                string country = parts[2].Trim('"');

                list.Add(new IpRange
                {
                    Start = start,
                    End = end,
                    Country = country
                });
            }

            list.Sort((a, b) => a.Start.CompareTo(b.Start));

            _ranges = list.ToArray();
        }

        public string Lookup(string ip)
        {
            if (_ranges == null)
                throw new InvalidOperationException("Data not loaded.");

            uint ipNum = IpToUint(ip);

            int left = 0;
            int right = _ranges.Length - 1;

            while (left <= right)
            {
                int mid = (left + right) >> 1;
                var range = _ranges[mid];

                if (ipNum < range.Start)
                {
                    right = mid - 1;
                }
                else if (ipNum > range.End)
                {
                    left = mid + 1;
                }
                else
                {
                    return range.Country;
                }
            }

            return null;
        }

        private static uint IpToUint(string ip)
        {
            var parts = ip.Split('.');
            return ((uint)int.Parse(parts[0]) << 24) |
                   ((uint)int.Parse(parts[1]) << 16) |
                   ((uint)int.Parse(parts[2]) << 8) |
                   uint.Parse(parts[3]);
        }

        private static string[] SplitCsv(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            var current = "";

            foreach (char c in line)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            values.Add(current);
            return values.ToArray();
        }
    }
}
