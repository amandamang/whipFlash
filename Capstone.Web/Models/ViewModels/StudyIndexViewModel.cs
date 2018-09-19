using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models.ViewModels
{
    public class StudyIndexViewModel
    {
        public StudyDeck StudyDeck { get; set; }
        public bool SessionStarted { get; set; }
    }
}