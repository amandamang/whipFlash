using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class CardItem : BaseItem
    {
        public string Term { get; set; }
        public string Definition { get; set; }
        public int UserID { get; set; }
        public string TagName { get; set; }
        public List<TagItem> Tags { get; set; }
    }
}