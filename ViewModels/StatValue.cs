using System;
using Google.Cloud.BigQuery.V2;

namespace CFAN.SchoolMap.ViewModels
{
    public class StatValue
    {
        public StatValue(BigQueryRow row, IStatDateDim dim)
        {
            Date = dim.GetValue(row["date"].ToString());
            Attendance = long.Parse(row["Attendance"].ToString());
            Decisions = long.Parse(row["Decisions"].ToString());
            Visit = long.Parse(row["Visit"].ToString());
        }

        public long Visit { get; set; }

        public string VisitV => $"{Visit:## ###}";

        public long Decisions { get; set; }
        public string DecisionsV => $"{Decisions:## ###}";

        public long Attendance { get; set; }
        public string AttendanceV => $"{Attendance:## ###}";

        public string Date { get; set; }
    }
}