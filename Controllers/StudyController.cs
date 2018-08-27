using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class StudyController : Controller
    {
        private IDatabaseSvc _db = null;

        public StudyController(IDatabaseSvc db)
        {
            _db = db;
        }

        // GET: Index
        public ActionResult Index(int? deckId)
        {
            int id = 0;

            //REMOVE OPTIONAL AFTER TESTING - Jon
            if (!deckId.HasValue)
            {
                id = 1;
            }
            else
            {
                id = deckId.Value;
            }
            // Get Data for view model
            DeckItem deck = _db.GetDeck(id);
            deck.Cards = _db.GetAllCardsForDeck(id);
            StudyDeck studyDeck = new StudyDeck
            {
                Deck = deck
            };
            bool sessionStarted = StartStudyDeckSession(studyDeck);

            //Add data to view model
            StudyIndexViewModel IndexView = new StudyIndexViewModel
            {
                StudyDeck = studyDeck,
                SessionStarted = sessionStarted
            };

            return View("Index", IndexView);
        }

        //Get: StudyCard
        public ActionResult StudyCard(int deckId)
        {

            StudyDeck studyDeck = GetActiveStudyDeckSession(deckId);

            StudyCardViewModel viewModel = new StudyCardViewModel
            {
                Card = studyDeck.Deck.Cards[studyDeck.CurrentCard],
                CardNumber = studyDeck.CurrentCard + 1,
                TotalCardNumber = studyDeck.Deck.Cards.Count(),
                DeckName = studyDeck.Deck.Name,
                DeckId = studyDeck.Deck.Id
            };

            return View("StudyCard", viewModel);
        }

        //Post: StudyCard
        [HttpPost]
        public ActionResult StudyCard(int deckId, bool isRight)
        {
            string action = "StudyCard";

            StudyDeck studyDeck = GetActiveStudyDeckSession(deckId);
            if(!studyDeck.CurrentCardIsLast)
            {
                int cardNumber = NextCardInStudyDeckSession(deckId, isRight);
            }
            else
            {
                action = "StudyResult";
                if(isRight)
                {
                    studyDeck.NumberCorrect++;
                }
            }

            return RedirectToAction(action, "Study", new { deckId = deckId } );
        }

        //Get: StudyResult
        public ActionResult StudyResult(int deckId)
        {
            StudyDeck studyDeck = GetActiveStudyDeckSession(deckId);


            StudyResultViewModel viewModel = new StudyResultViewModel
            {
                DeckName = studyDeck.Deck.Name,
                NumberCorrect = studyDeck.NumberCorrect,
                PercentScore = studyDeck.ScoreString,
                TotalCards = studyDeck.Deck.Cards.Count,
                DeckId = deckId
            };
            EndCurrentStudyDeckSession(deckId);

            return View("StudyResult", viewModel);
        }


        #region Session Methods
        /// <summary>
        /// Attempt to start a study session for a deck. 
        /// </summary>
        /// <param name="studyDeck">StudyDeck Object to be studied</param>
        /// <returns>true if there is no existing session for deck, false is session already exists</returns>
        private bool StartStudyDeckSession(StudyDeck studyDeck)
        {
            bool noExistingSessionForDeck = true;
            if (Session["StudyDeck" + studyDeck.Deck.Id] == null)
            {
                Session["StudyDeck" + studyDeck.Deck.Id] = studyDeck;        // <-- saves the Study deck to the session
            }
            else
            {
                Session["StudyDeck" + studyDeck.Deck.Id] = studyDeck;
                //noExistingSessionForDeck = false;
            }
            return noExistingSessionForDeck;
        }

        /// <summary>
        /// Update a studyDeck in the session
        /// </summary>
        /// <param name="studyDeck">StudyDeckObject with update data</param>
        /// <returns>true if session existed, false if there is no session</returns>
        private bool UpdateStudyDeckSession(StudyDeck studyDeck)
        {
            bool sessionExists = true;
            if (Session["StudyDeck" + studyDeck.Deck.Id] == null)
            {
                sessionExists = false;        // <-- no session exists
            }
            else
            {
                Session["StudyDeck" + studyDeck.Deck.Id] = studyDeck;
            }
            return sessionExists;
        }

        /// <summary>
        /// Add 1 to Current Card in the deck Session. Also adds 1 to NumberCorrect if correct.
        /// </summary>
        /// <param name="deckName">Name of Deck in Session</param>
        /// <param name="wasRight">Whether or not question was answered correctly</param>
        /// <returns>new CurrentCard value, returns -1 if no Session exists</returns>
        private int NextCardInStudyDeckSession(int deckId, bool wasRight)
        {
            int nextCardId = -1;
            //Get Deck from session
            StudyDeck currentStudyDeck = GetActiveStudyDeckSession(deckId);
            if (currentStudyDeck.CurrentCard != -1)
            {
                //Add 1 to current card
                currentStudyDeck.CurrentCard++;
                nextCardId = currentStudyDeck.CurrentCard;

                //Add to NumberCorrect if right
                if (wasRight)
                {
                    currentStudyDeck.NumberCorrect++;
                }

                //Update session with current card
                UpdateStudyDeckSession(currentStudyDeck);
            }
            return nextCardId;
        }

        /// <summary>
        /// Returns active study session for a deck.  If no active session, returns CurrentCard property as -1
        /// </summary>
        /// <param name="deckName">Name of deck to be studied</param>
        /// <returns>Session deck if session exists.  Blank StudyDeck with CurrentCard at -1 if session does not exist</returns>
        private StudyDeck GetActiveStudyDeckSession(int deckId)
        {
            StudyDeck result = new StudyDeck();
            if (Session["StudyDeck" + deckId] != null)
            {
                result = Session["StudyDeck" + deckId] as StudyDeck;
            }
            else
            {
                result.CurrentCard = -1;
            }
            return result;
        }

        /// <summary>
        /// Ends Session for a StudyDeck
        /// </summary>
        /// <param name="deckName">Name of Deck being studied</param>
        private void EndCurrentStudyDeckSession(int deckId)
        {
            Session["StudyDeck" + deckId] = null;
        }

        /// <summary>
        /// Get Active user's Id out of session
        /// </summary>
        /// <returns>Id of User in session</returns>
        private int GetActiveUserId()
        {
            UserItem user = null;
            int result = 1;
            if (Session["User"] != null)
            {
                user = Session["User"] as UserItem; // <-- gets the User out of session
                result = user.Id;
            }

            return result;
        }
        #endregion
    }
}