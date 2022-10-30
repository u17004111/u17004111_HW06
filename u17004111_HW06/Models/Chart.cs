using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u17004111_HW06.Models
{
    public class Chart
    {
        public string[] labels { get; set; } = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public List<Datasets> datasets { get; set; }
    }
    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public int[] data { get; set; }
    }
}
