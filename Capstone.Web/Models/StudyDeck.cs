using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone.Web.Models
{
    public class StudyDeck
    {
        public DeckItem Deck { get; set; }
        public int NumberCorrect { get; set; }
        public int CurrentCard { get; set; }


        public bool CurrentCardIsLast
        {
            get
            {
                bool result = false;
                if(CurrentCard == Deck.Cards.Count() - 1)
                {
                    result = true;
                }
                return result;
            }
        }

        public double PercentScore
        {
            get
            {
                return NumberCorrect / Deck.Cards.Count();
            }
        }
        public string ScoreString
        {
            get
            {
                int result = (int)((double)NumberCorrect / (double)Deck.Cards.Count() * (double)100);
                return result.ToString() + "%";
            }
        }

        //Constructors
        public StudyDeck()
        {
            CurrentCard = 0;
            NumberCorrect = 0;
        }

        public StudyDeck(DeckItem deck)
        {
            Deck = deck;
            CurrentCard = 0;
            NumberCorrect = 0;
        }
    }
}