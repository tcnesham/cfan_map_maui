using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CFAN.SchoolMap.Helpers
{
    public class Param
    {
        public Param(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public static Dictionary<string, string> DecodeValues(IDictionary<string, string> query)
        {
            return query.ToDictionary(i => i.Key, i => HttpUtility.UrlDecode(i.Value));
        }

        public static T Get<T>(Dictionary<string, string> pars, string name)
        {
            if (!pars.ContainsKey(name)) throw new ApplicationException("Chybné jméno parametru " + name);
            if (typeof(T) == typeof(string)) return (T)(object)pars[name];
            throw new NotImplementedException(typeof(T).Name);
        }

        public static T GetOrDefault<T>(Dictionary<string, string> pars, string name)
        {
            if (!pars.ContainsKey(name)) return default(T);
            return Get<T>(pars, name);
        }

        public static string EncodeParameters(params Param[] parameters)
        {
            var routeSb = new StringBuilder();
            if (parameters != null && parameters.Any())
            {
                routeSb.Append("?");
                bool first = true;
                foreach (var p in parameters)
                {
                    if (p.Value == null)
                    {
                        throw new ApplicationException(p.Name);
                    }
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        routeSb.Append("&");
                    }

                    routeSb.Append(p.Name);
                    routeSb.Append("=");
                    routeSb.Append(Uri.EscapeDataString(p.Value));
                }
            }
            return routeSb.ToString();
        }
    }
}
