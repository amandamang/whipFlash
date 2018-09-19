using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class EditViewModel
    {
        public string Term { get; set; }
        public int CardId { get; set; }
        public int DeckId { get; set; }
    }
}