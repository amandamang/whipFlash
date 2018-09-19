using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models.ViewModels;
using Capstone.Web.Models;

namespace Capstone.Web.Controllers
{
    public class DeckController : Controller
    {
        private IDatabaseSvc _db = null;

        public DeckController(IDatabaseSvc db)
        {
            _db = db;
        }
        
        // GET: Deck/ViewDecks
        public ActionResult ViewDecks()
        {
            DecksViewModel decks = new DecksViewModel();

            int userId = GetActiveUserId();
            decks.Decks = _db.GetAllDecksForUser(userId);

            if(TempData["UpdateSuccess"] != null)
            {
                decks.SuccessMessage = TempData["UpdateSuccess"] as string;
            }
            if(TempData["ErrorMessage"] != null)
            {
                decks.ErrorMessage = TempData["ErrorMessage"] as string;
            }

            return View("ViewDecks", decks);
        }

        // GET: Deck/ViewCards
        public ActionResult ViewCards()
        {
            List<CardItem> usersCards = new List<CardItem>();

            int userId = GetActiveUserId();
            var usersDecks = _db.GetAllDecksForUser(userId);
            foreach (var item in usersDecks)
            {
                usersCards = _db.GetAllCardsForDeck(item.Id);
            }
            
            return View("ViewCards", usersCards);
        }

        // GET: Deck/About
        public ActionResult About()
        {
            return View();
        }

        // GET: Deck/DeckDetail
        //CHANGE PARAMETER TO INT FOR FINAL - JON
        public ActionResult DeckDetail(int? deckId)
        {
            DeckDetailViewModel result = new DeckDetailViewModel();
            if(TempData["UpdateSuccess"] != null)
            {
                result.UpdateSuccessMessage = TempData["UpdateSuccess"] as string;
            }

            //FOR TESTING ONLY:
            if (!deckId.HasValue)
            {
                deckId = TempData["deckId"] as int?;
            }
            int id = deckId.Value;
            DeckItem deck = _db.GetDeck(id);
            List<CardItem> cards = _db.GetAllCardsForDeck(id);
            foreach(CardItem card in cards)
            {
                card.Tags = _db.GetAllTagsForCard(card.Id);
            }
            deck.Cards = cards;
            result.Deck = deck;

            return View("DeckDetail", result);
        }

        // GET: Deck/UpdateCard
        //CHANGE PARAMETER TO INT FOR FINAL - JON
        public ActionResult UpdateCard(int cardId, int? deckId)
        {
            //Get Data for view model
            CardItem card = _db.GetCard(cardId);
            card.Tags = _db.GetAllTagsForCard(cardId);
            int userId = GetActiveUserId();
            List<DeckItem> userDecks = _db.GetAllDecksForUser(userId);
            List<DeckItem> currentCardDecks = _db.GetAllDecksForCard(cardId, userId);
            Dictionary<string, bool> tagNamesRemoveTrue = new Dictionary<string, bool>();
            List<string> tagNames = new List<string>();
            foreach(TagItem tag in card.Tags)
            {
                tagNamesRemoveTrue[tag.TagName] = false;
                tagNames.Add(tag.TagName);
            }
            Dictionary<int, string> deckNameId = new Dictionary<int, string>();
            Dictionary<int, string> currentDeckNameId = new Dictionary<int, string>();
            foreach (DeckItem deck in userDecks)
            {
                deckNameId[deck.Id] = deck.Name;
            }
            foreach (DeckItem deck in currentCardDecks)
            {
                currentDeckNameId[deck.Id] = deck.Name;
            }

            UpdateCardViewModel result = new UpdateCardViewModel
            {
                CardId = cardId,
                Term = card.Term,
                Definition = card.Definition,
                DeckIdName = deckNameId,
                CurrentDeckIdName = currentDeckNameId,
                TagNamesRemoveTrue = tagNamesRemoveTrue,
                TagNames = tagNames
            };
            if (deckId.HasValue)
            {
                result.DestinationDeckId = deckId.Value;
            }
            //remove current decks from decks dictionary
            result.FilterDeckIdName();

            if(TempData["ErrorMessages"] != null)
            {
                result.UpdateErrors = TempData["ErrorMessages"] as List<string>;
            }

            return View("UpdateCard", result);
        }

        //POST: Deck/Update Card
        [HttpPost]
        public ActionResult UpdateCard(UpdateCardViewModel model)
        {

            ActionResult result = RedirectToAction("ViewDecks");
            if(model.DestinationDeckId.HasValue)
            {
                int deckId = model.DestinationDeckId.Value;
                result = RedirectToAction("DeckDetail", deckId);
                TempData["deckId"] = model.DestinationDeckId.Value;

            }
            List<string> errorMessages = new List<string>();

            //Update Card to Database
            int userId = GetActiveUserId();
            CardItem card = new CardItem
            {
                Id = model.CardId,
                Term = model.Term,
                Definition = model.Definition,
                UserID = userId
            };

            bool updateSuccess = _db.UpdateCard(card, model.CardId);
            if(!updateSuccess)
            {
                errorMessages.Add("DATABASE ERROR: Card not updated");
            }

            //Add Card to Decks
            bool deckAddedSuccess = true;
            if (model.NewDeckId.HasValue)
            {
                try
                {
                    _db.AddCardToDeck(model.CardId, model.NewDeckId.Value);
                }
                catch(Exception e)
                {
                    deckAddedSuccess = false;
                    errorMessages.Add(e.Message);
                }
            }

            //Add Tags
            bool tagAddedSuccess = true;
            if(model.AddTag != null)
            {
                try
                {
                    _db.AddTagToCard(model.AddTag, model.CardId);
                }
                catch(Exception e)
                {
                    tagAddedSuccess = false;
                    errorMessages.Add(e.Message);
                }
            }

            //Remove Tags
            bool tagRemovedSuccess = true;
            
            foreach(KeyValuePair<string,bool> item in model.TagNamesRemoveTrue)
            {
                if (item.Value)
                {
                    tagRemovedSuccess = _db.RemoveTagFromCard(item.Key, model.CardId);
                }
            }
            if(!tagRemovedSuccess)
            {
                errorMessages.Add("DATABASE ERROR: Tag not removed");
            }

            if(!updateSuccess || !deckAddedSuccess || !tagAddedSuccess || !tagRemovedSuccess)
            {
                result = RedirectToAction("UpdateCard", model.DestinationDeckId.Value);
                TempData["ErrorMessages"] = errorMessages;
            }
            else
            {
                TempData["UpdateSuccess"] = $"{model.Term} card edit successful";
            }

            return result;
        }
 

        // GET: Deck/Edit
        // allows the user to confirm/abort the deletion of a card
        public ActionResult Edit(int cardId, int deckId)
        {
            CardItem card = _db.GetCard(cardId);
            EditViewModel result = new EditViewModel
            {
                CardId = cardId,
                Term = card.Term,
                DeckId = deckId
            };


            return View("Edit", result);
        }

        //POST: Deck/Edit
        [HttpPost]
        public ActionResult Edit(EditViewModel model)
        {
            bool cardRemoved = _db.RemoveCardFromDeck(model.CardId, model.DeckId);
            List<string> errorMessages = new List<string>();
            errorMessages.Add($"DATABASE ERROR: {model.Term} card deletion failed");
            if(cardRemoved)
            {
                TempData["ErrorMessages"] = errorMessages;
            }
            else
            {
                TempData["UpdateSuccess"] = $"{model.Term} card deletion successful";
            }

            return RedirectToAction("DeckDetail", model.DeckId);
        }

        // GET: Deck/NewCard
        public ActionResult NewCard(int? deckId)
        {
            int userId = GetActiveUserId();
            List<DeckItem> userDecks = _db.GetAllDecksForUser(userId);
            string deckName;
            Dictionary<int, string> deckNameId = new Dictionary<int, string>();
            foreach (DeckItem deck in userDecks)
            {
                deckNameId[deck.Id] = deck.Name;
            }

            if(deckId.HasValue)
            {
                int deckIdFinal = deckId.Value;
                deckName = _db.GetDeck(deckIdFinal).Name;
                if(deckNameId.ContainsKey(deckIdFinal))
                {
                    deckNameId.Remove(deckIdFinal);
                }
            }

            CardViewModel card = new CardViewModel
            {
                DeckIdName = deckNameId
            };

            if (TempData["UpdateSuccess"] != null)
            {
                card.UpdateSuccessMessage = TempData["UpdateSuccess"] as string;
            }
            if (deckId.HasValue)
            {
                card.DeckId = deckId.Value;
                card.DeckName = _db.GetDeck(deckId.Value).Name;
            }


            return View("NewCard", card);
        }


        // POST: Deck/NewCard
        [HttpPost]
        public ActionResult NewCard(CardViewModel model)
        {
            ActionResult result = RedirectToAction("NewCard", model.DeckId);

            List<string> errorMessages = new List<string>();

            //Add Card to Database
            int userId = GetActiveUserId();
            CardItem card = new CardItem
            {
                Term = model.Term,
                Definition = model.Definition,
                UserID = userId
            };

            bool cardAddedSuccess = true;
            card = _db.AddCard(card);
            if (card.Id < 0)
            {
                cardAddedSuccess = false;
            }

            //Add Card to Decks
            bool deckAddedSuccess = true;
            if (model.NewDeckId.HasValue)
            {
                try
                {
                    _db.AddCardToDeck(card.Id, model.NewDeckId.Value);
                }
                catch (Exception e)
                {
                    deckAddedSuccess = false;
                    errorMessages.Add(e.Message);
                }
            }
            if (model.DeckId.HasValue)
            {
                try
                {
                    _db.AddCardToDeck(card.Id, model.DeckId.Value);
                }
                catch (Exception e)
                {
                    deckAddedSuccess = false;
                    errorMessages.Add(e.Message);
                }
                TempData["deckId"] = model.DeckId;
            }

            //Add Tags
            bool tagAddedSuccess = true;
            if (model.TagNames.Count() > 0)
            {
                foreach (string tagName in model.TagNames)
                {
                    try
                    {
                        _db.AddTagToCard(tagName, card.Id);
                    }
                    catch (Exception e)
                    {
                        tagAddedSuccess = false;
                        errorMessages.Add(e.Message);
                    }
                }
            }

            if (!cardAddedSuccess || !deckAddedSuccess || !tagAddedSuccess)
            {
                result = RedirectToAction("AddCard", model.DeckId);
                TempData["ErrorMessages"] = errorMessages;
            }
            else
            {
                if (model.DeckId.HasValue)
                {
                    result = RedirectToAction("DeckDetail", model.DeckId);
                }
                else
                {
                    result = RedirectToAction("NewCard", model.DeckId);
                }
                TempData["UpdateSuccess"] = $"{model.Term} card creation successful";
            }

            return result;
        }

        // GET: Deck/NewDeck
        public ActionResult NewDeck()
        {
            NewDeckViewModel deck = new NewDeckViewModel();

            return View("NewDeck", deck);
        }

        // POST: Deck/NewDeck
        [HttpPost]
        public ActionResult NewDeck(NewDeckViewModel model)
        {
            ActionResult result;

            //Validate the model before proceeding
            if (!ModelState.IsValid)
            {
                result = View("NewDeck", model);
            }
            else
            {
                //make deckitem
                int userId = GetActiveUserId();
                DeckItem deck = new DeckItem
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserID = userId
                };

                //Attempt add to database
                string errorMessage;
                bool addDeckSuccess = true;
                try
                {
                    deck = _db.AddDeck(deck);
                }
                catch(Exception e)
                {
                    errorMessage = e.Message;
                    addDeckSuccess = false;
                }

                //send success/error message
                if(addDeckSuccess)
                {
                    TempData["UpdateSuccess"] = $"{deck.Name} deck creation successful";
                }
                else
                {
                    TempData["ErrorMessage"] = $"DATABASE ERROR: {deck.Name} deck creation failed";
                }


                result = RedirectToAction("ViewDecks");
            }
            return result;
        }

        //Get Deck/EditDeck
        public ActionResult UpdateDeck(int deckId)
        {
            DeckItem oldDeck = _db.GetDeck(deckId);
            UpdateDeckViewModel model = new UpdateDeckViewModel
            {
                DeckId = deckId,
                Name = oldDeck.Name,
                Description = oldDeck.Description
            };

            return View("UpdateDeck", model);
        }

        //Post Deck/EditDeck
        [HttpPost]
        public ActionResult UpdateDeck(UpdateDeckViewModel model)
        {
            ActionResult result;
            bool updateDeckSuccess = true;

            //Validate the model before proceeding
            if (!ModelState.IsValid)
            {
                result = View("ViewDecks");
            }
            else
            {
                //make deckitem
                int userId = GetActiveUserId();
                DeckItem deck = new DeckItem
                {
                    Id = model.DeckId,
                    Name = model.Name,
                    Description = model.Description,
                    UserID = userId
                };

                //Attempt add to database
                
                updateDeckSuccess = _db.UpdateDeck(deck, deck.Id);

                //send success/error message
                if (updateDeckSuccess)
                {
                    TempData["UpdateSuccess"] = $"{deck.Name} deck edit successful";
                }
                else
                {
                    TempData["ErrorMessage"] = $"DATABASE ERROR: {deck.Name} deck edit failed";
                }


                result = RedirectToAction("ViewDecks");
            }
            return result;
        }


        /// <summary>
        /// Get Active user out of session
        /// </summary>
        /// <returns>User in session</returns>
        private UserItem GetActiveUser()
        {
            UserItem user = null;

            user = Session["User"] as UserItem; // <-- gets the User out of session
            if (Session["User"] == null)
            {
                user = new UserItem();
                Session["User"] = user;        // <-- saves the Temp Helper into session
            }
            else
            {
                user = Session["User"] as UserItem; // <-- gets the Temp Helper out of session
            }

            return user;
        }

        #region Session Methods
        /// <summary>
        /// Attempt to start a study session for a deck. 
        /// </summary>
        /// <param name="studyDeck">StudyDeck Object to be studied</param>
        /// <returns>true if there is no existing session for deck, false is session already exists</returns>
        private bool StartStudyDeckSession(StudyDeck studyDeck)
        {
            bool noExistingSessionForDeck = true; ;
            if (Session["StudyDeck" + studyDeck.Deck.Name] == null)
            {
                Session["StudyDeck" + studyDeck.Deck.Name] = studyDeck;        // <-- saves the Study deck to the session
            }
            else
            {
                noExistingSessionForDeck = false;
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
            if (Session["StudyDeck" + studyDeck.Deck.Name] == null)
            {
                sessionExists = false;        // <-- no session exists
            }
            else
            {
                Session["StudyDeck" + studyDeck.Deck.Name] = false;
            }
            return sessionExists;
        }

        /// <summary>
        /// Add 1 to Current Card in the deck Session. Also adds 1 to NumberCorrect if correct.
        /// </summary>
        /// <param name="deckName">Name of Deck in Session</param>
        /// <param name="wasRight">Whether or not question was answered correctly</param>
        /// <returns>new CurrentCard value, returns -1 if no Session exists</returns>
        private int NextCardInStudyDeckSession(string deckName, bool wasRight)
        {
            int nextCardId = -1;
            //Get Deck from session
            StudyDeck currentStudyDeck = GetActiveStudyDeckSession(deckName);
            if (currentStudyDeck.CurrentCard != -1)
            {
                //Add 1 to current card
                currentStudyDeck.CurrentCard++;
                nextCardId = currentStudyDeck.CurrentCard;

                //Add to NumberCorrect if right
                if(wasRight)
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
        private StudyDeck GetActiveStudyDeckSession(string deckName)
        {
            StudyDeck result = new StudyDeck();
            if (Session["StudyDeck" + deckName] != null)
            {
                result = Session["StudyDeck" + deckName] as StudyDeck;
                result.CurrentCard = -1;
            }
            return result;
        }

        /// <summary>
        /// Ends Session for a StudyDeck
        /// </summary>
        /// <param name="deckName">Name of Deck being studied</param>
        private void EndCurrentStudyDeckSession(string deckName)
        {
            Session["StudyDeck" + deckName] = null;
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