namespace CFAN.SchoolMap.ViewModels
{
    public interface IStatDateDim
    {
        public string Title { get; }
        public string Column { get; }
        public string ColumnName { get; }
        public string GetValue(string value);

    }
    public class StatDimDay : IStatDateDim
    {
        public string Title => "day";
        public string Column => "YYMMDD";
        public string ColumnName => "Day";
        public string GetValue(string value)
        {
            return "" + value[0] + value[1] + "-" + value[2] + value[3] + "-" + value[4] + value[5];
        }
    }

    public class StatDimWeek : IStatDateDim
    {
        public string Title => "week";
        public string Column => "YYWW";

        public string ColumnName => "Week";
        public string GetValue(string value)
        {
            return "20" + value[0] + value[1] + " week " + value[2] + value[3];
        }
    }

    public class StatDimMonth : IStatDateDim
    {
        public string Title => "month";
        public string Column => "YYMM";
        public string ColumnName => "Month";
        public string GetValue(string value)
        {
            return "20" + value[0] + value[1] + "-" + value[2] + value[3];
        }
    }

    public class StatDimYear : IStatDateDim
    {
        public string Title => "year";
        public string Column => "YY";
        public string ColumnName => "Year";
        public string GetValue(string value)
        {
            return "20" + value[0] + value[1];
        }
    }
}