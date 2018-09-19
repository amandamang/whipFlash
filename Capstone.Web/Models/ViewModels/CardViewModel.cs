using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class CardViewModel
    {
        public string DeckName { get; set; }
        public int? DeckId { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
        public List<string> TagNames { get; set; }
        public Dictionary<int, string> DeckIdName { get; set; }
        public int? NewDeckId { get; set; }
        public List<string> UpdateErrors { get; set; }
        public string UpdateSuccessMessage { get; set; }


        public CardViewModel ()
        {
        }
    }
}