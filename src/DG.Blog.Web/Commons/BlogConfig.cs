using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Blog.Web.Commons
{
    public class BlogConfig
    {
        public string SiteName { get; set; }
        public string Domain { get; set; }
        public string BaseAddress { get; set; }
        public Menus[] Menus { get; set; }
    }

    public class Menus
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Style { get; set; }
    }
}