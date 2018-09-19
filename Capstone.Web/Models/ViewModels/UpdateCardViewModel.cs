using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class UpdateCardViewModel
    {
        public int CardId { get; set; }
        public string Term { get; set; }
        public string Definition { get; set; }
        public Dictionary<int,string> DeckIdName { get; set; }
        public Dictionary<int,string> CurrentDeckIdName { get; set; }
        public int? NewDeckId { get; set; }
        public Dictionary<string,bool> TagNamesRemoveTrue { get; set; }
        public List<string> TagNames { get; set; }
        public string AddTag { get; set; }
        public int? DestinationDeckId { get; set; }
        public List<string> UpdateErrors { get; set; }


        public void FilterDeckIdName()
        {
            foreach(KeyValuePair<int,string> item in CurrentDeckIdName)
            {
                if(DeckIdName.ContainsKey(item.Key))
                {
                    DeckIdName.Remove(item.Key);
                }
            }

        }
    }
}