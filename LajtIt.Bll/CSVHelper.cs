using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

//namespace LajtIt.Bll
//{
//    public class CSVHelper
//    {
//        public class CsvColumnNameAttribute : Attribute
//        {
//            public bool Export { get; set; }
//            public int Order { get; set; }
//            public string Name { get; set; }

//            public CsvColumnNameAttribute()
//            {
//                Export = true;
//                Order = int.MaxValue; // so unordered columns are at the end
//            }

//        }
//        public string GetCsv<T>(List<T> csvDataObjects)
//        {
//            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
//            var sb = new StringBuilder();
//            sb.AppendLine(GetCsvHeaderSorted(propertyInfos));
//            csvDataObjects.ForEach(d => sb.AppendLine(GetCsvDataRowSorted(d, propertyInfos)));
//            return sb.ToString();
//        }

//        private string GetCsvDataRowSorted<T>(T csvDataObject, PropertyInfo[] propertyInfos)
//        {
//            IEnumerable<string> valuesSorted = propertyInfos
//                .Select(x => new
//                {
//                    Value = x.GetValue(csvDataObject, null),
//                    Attribute = (CsvColumnNameAttribute)Attribute.GetCustomAttribute(x, typeof(CsvColumnNameAttribute), false)
//                })
//                .Where(x => x.Attribute != null && x.Attribute.Export)
//                .OrderBy(x => x.Attribute.Order)
//                .Select(x => GetPropertyValueAsString(x.Value));
//            return String.Join(",", valuesSorted);
//        }

//        private string GetCsvHeaderSorted(PropertyInfo[] propertyInfos)
//        {
//            IEnumerable<string> headersSorted = propertyInfos
//                .Select(x => (CsvColumnNameAttribute)Attribute.GetCustomAttribute(x, typeof(CsvColumnNameAttribute), false))
//                .Where(x => x != null && x.Export)
//                .OrderBy(x => x.Order)
//                .Select(x => x.Name);
//            return String.Join(",", headersSorted);
//        }

//        private string GetPropertyValueAsString(object propertyValue)
//        {
//            string propertyValueString;

//            if (propertyValue == null)
//                propertyValueString = "";
//            else if (propertyValue is DateTime)
//                propertyValueString = ((DateTime)propertyValue).ToString("dd MMM yyyy");
//            else if (propertyValue is int)
//                propertyValueString = propertyValue.ToString();
//            else if (propertyValue is float)
//                propertyValueString = ((float)propertyValue).ToString("#.####"); // format as you need it
//            else if (propertyValue is double)
//                propertyValueString = ((double)propertyValue).ToString("#.####"); // format as you need it
//            else // treat as a string
//                propertyValueString = @"""" + propertyValue.ToString().Replace(@"""", @"""""") + @""""; // quotes with 2 quotes

//            return propertyValueString;
//        }
//    }
//}
