using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class TagItem : BaseItem
    {
        public string TagName { get; set; }
        public int CardId { get; set; }
    }
}