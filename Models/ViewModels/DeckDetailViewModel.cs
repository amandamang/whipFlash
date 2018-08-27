using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class DeckDetailViewModel
    {
        public DeckItem Deck { get; set; } 
        public string UpdateSuccessMessage { get; set; }
    }
}