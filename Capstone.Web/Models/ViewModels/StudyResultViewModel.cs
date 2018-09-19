using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class StudyResultViewModel
    {
        public string PercentScore { get; set; }
        public int NumberCorrect { get; set; }
        public int TotalCards { get; set; }
        public string DeckName { get; set; }
        public int DeckId { get; set; }

    }
}