using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class StudyCardViewModel
    {
        public CardItem Card { get; set; }
        public bool IsRight { get; set; }
        public int CardNumber { get; set; }
        public int TotalCardNumber { get; set; }
        public string DeckName { get; set; }
        public int DeckId { get; set; }
    }
}