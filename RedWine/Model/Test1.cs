using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedWine.Model
{
    [Table("Test1")]
    public class Test1
    {
        public int? Id { get; set; }
        public string Tc1 { get; set; }
        public string Tc2 { get; set; }
    }
    public class TestEpq:EasyuiPagedQuery
    {
        public string Tc1 { get; set; }
        public string Tc2 { get; set; }
    }
}
