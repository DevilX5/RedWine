using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedWine
{
    public class EasyuiPaged<T>
    {
        public EasyuiPaged(int total, IEnumerable<T> data)
        {
            this.total = total;
            this.rows = data;
        }
        public int total { get; set; }
        public IEnumerable<T> rows { get; set; }
        public IEnumerable<T> footer { get; set; }
    }
    public class EasyuiDgvData<T>
    {
        public int total { get; set; }
        public IEnumerable<T> rows { get; set; }
    }
    public class ExtContent
    {
        public string field { get; set; }
        public object value { get; set; }
    }
    public class EasyuiColumn
    {
        public bool hidden { get; set; }
        public string field { get; set; }
        public string title { get; set; }
        public string align { get; set; }
        public int width { get; set; }
        public int rowspan { get; set; }
        public int colspan { get; set; }
    }

    public class EasyuiPagedQuery
    {
        public EasyuiPagedQuery()
        {
            page = 1;
            rows = 10;
        }
        public int page { get; set; }
        public int rows { get; set; }
        public int start { get { return (page - 1) * rows; } }
        public int end { get { return page * rows; } }
    }
    public class DropJson
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Node
    {
        public Node()
        {
            children = new List<Node>();
        }
        public string id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public string iconCls { get; set; }
        public List<Node> children { get; set; }
    }
}
