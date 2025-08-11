using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Pins.States;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Maui.Model
{
    public class CountryData
    {
        public const char PlaceSeparatorCh = '|';

        public string CountryCode { get; set; }
        public byte[] PlacesData { get; set; }
        public int DataSize { get; set; }
        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }
        public string ChangedBy { get; set; }
        public string Version { get; set; }

        public CountryPlaces<TPoint> ToCountryPlaces<TPoint>() where TPoint : BasePoint, new()
        {
            var cp = new CountryPlaces<TPoint> {CountryCode = CountryCode};
            if (PlacesData != null)
            {
                var data = Unzip(PlacesData);
                Deserialize(data, cp);
                //var text = Serialize(cp);
            }
            return cp;
        }

        private static void Deserialize<TPoint>(string data, CountryPlaces<TPoint> cp) where TPoint : BasePoint, new()
        {
            var p = new TPoint();
            var pcb = new StringBuilder(20);
            char s = 's'; //start
            foreach (char c in data)
            {
                switch (s)
                {
                    case 's':
                        p.Type = c;
                        s = (c == PlaceStates.PlacePlanned)
                            ? 't'
                            : 'p';
                        break;
                    case 't':
                        p.TeamChar = c;
                        s = 'p';
                        break;
                    case 'p':
                        if (c == PlaceSeparatorCh)
                        {
                            var pc = pcb.ToString();
                            if (pc.Length == 12)
                            {
                                pc = pc.Substring(1);
                            }
                            if (pc.Length == 10)
                            {
                                pc = p.Team + pc;
                                p.Team = 0;
                            }
                            p.PlusCode = pc;
                            cp.Update(p);
                            p = new TPoint
                            {
                                Team = 0
                            };
                            s = 's';
                            pcb.Clear();
                        }
                        else
                        {
                            pcb.Append(c);
                        }

                        break;
                    default: throw new ApplicationException("unexpected state " + c);
                }
            }
        }


        public static CountryData FromCountryPlaces<TPoint>(CountryPlaces<TPoint> country) where TPoint : BasePoint, new()
        {
            var cd = new CountryData {CountryCode = country.CountryCode};
            var text = Serialize(country);
            cd.PlacesData = Zip(text);
            cd.DataSize = cd.PlacesData.Length;
            return cd;
        }

        private static string Serialize<TPoint>(CountryPlaces<TPoint> country) where TPoint : BasePoint, new()
        {
            var sb = new StringBuilder();
            foreach (var p in country.Places.OrderBy(p => p.PlusCode))
            {
                sb.Append(p.Type);
                if (p.Type == PlaceStates.PlacePlanned)
                {
                    sb.Append(p.TeamChar);
                }

                sb.Append(p.PlusCode);
                sb.Append(PlaceSeparatorCh);
            }

            var text = sb.ToString();
            return text;
        }

        public static byte[] Zip(string str)
        {
            using var output = new MemoryStream();
            using (var zipped = new GZipStream(output, CompressionMode.Compress))
            using (var input = new MemoryStream(Encoding.ASCII.GetBytes(str)))
            {
                input.CopyTo(zipped);
            }
            return output.ToArray();
        }

        public static string Unzip(byte[] bytes)
        {
            using var input = new MemoryStream(bytes);
            using var unzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            unzip.CopyTo(output);
            return Encoding.ASCII.GetString(output.ToArray());
        }
    }
}
