using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class DeckItem : BaseItem
    {
        public string Name { get; set; }
        public int UserID { get; set; }
        public List<CardItem> Cards { get; set; } 
        public string Description { get; set; }
        public int CardCount { get; set; }

        public DeckItem()
        {
            Cards = new List<CardItem>();
        }
    }
}