using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatchTracker.Models.Data
{
    public class DataItem {
        public string Key { get; set; }

        public object Value { get; set; }

        public DataItem()
        { }

        public DataItem(string key, object value)
        {
            Key = key.ToLower();
            Value = value;
        }
    }
}
