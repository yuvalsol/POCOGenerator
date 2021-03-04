using System;

namespace POCOGeneratorUI.Filtering
{
    public enum FilterType
    {
        Equals,
        Contains,
        Does_Not_Contain
    }

    public class FilterSetting
    {
        public FilterType FilterType { get; set; }
        public string Filter { get; set; }

        public FilterSetting()
        {
            FilterType = FilterType.Contains;
            Filter = null;
        }

        public bool IsEnabled
        {
            get { return string.IsNullOrEmpty(Filter) == false; }
        }
    }

    public class FilterSettings
    {
        public FilterSetting FilterName { get; set; }
        public FilterSetting FilterSchema { get; set; }

        public FilterSettings()
        {
            FilterName = new FilterSetting();
            FilterSchema = new FilterSetting();
        }

        public bool IsEnabled
        {
            get { return FilterName.IsEnabled || FilterSchema.IsEnabled; }
        }
    }
}
