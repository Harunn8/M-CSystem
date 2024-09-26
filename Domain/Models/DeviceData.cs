using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class DeviceData
    {
        public string deviceName { get; set; }
        public string oid { get; set; }
        public string value { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
