using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Web.Models;

namespace Capstone.Web
{
    public class MockDatabaseSvc : IDatabaseSvc
    {

        //simulate database
        private static Dictionary<string, UserItem> _userItems = new Dictionary<string, UserItem>();
        private static Dictionary<int,CardItem> _cards = new Dictionary<int,CardItem>();
        private static Dictionary<string,DeckItem> _decks = new Dictionary<string,DeckItem>();
        private static Dictionary<string, TagItem> _tags = new Dictionary<string, TagItem>();

        // Member variables to simulate primary keys for different tables
        private static int _userId = 1;
        private static int _cardId = 1;
        private static int _deckId = 1;
        private static int _tagId = 1;

        //put fake data in mock DB
        public MockDatabaseSvc()
        {
            if ( _userItems.Count == 0)
            {
                DbManager.PopulateDatabase(this);
            }
        }

        //replicating adding data to mock DB
        public int AddUserItem(UserItem item)
        {
            item.Id = _userId++;
            _userItems.Add(item.UserName, item);
            return item.Id;
        }
        

        /// <summary>
        /// Add a card item to mock database
        /// </summary>
        /// <param name="card">CardItem Object without Id</param>
        /// <returns>CardItem Object with Id</returns>
        public CardItem AddCard(CardItem card)
        {
            int cardCountOld = _cards.Count();
            
            //Add to dictionary
            card.Id = _cardId++;
            _cards.Add(card.Id, card);
            //Check that card was successfully added
            int cardCountNew = _cards.Count();
            if (cardCountNew - cardCountOld != 1)
            {
                throw new Exception("DATABASE ERROR: Card was not added");
            }
            return card;
        }

        /// <summary>
        /// Add a deck item to mock database
        /// </summary>
        /// <param name="deck">DeckItem object to be added without id</param>
        /// <returns>DeckItem object with Id</returns>
        public DeckItem AddDeck(DeckItem deck)
        {
            int deckCountOld = _decks.Count();
            //Add to Dictionary
            deck.Id = _deckId++;
            _decks.Add(deck.Name, deck);
            //Check that card was successfully added
            int deckCountNew = _decks.Count();
            if (deckCountNew - deckCountOld != 1)
            {
                throw new Exception("DATABASE ERROR: Deck was not added");
            }
            return deck;
        }

        /// <summary>
        /// Add a tag item to mock database
        /// </summary>
        /// <param name="tag">tag without id</param>
        /// <returns>tag with id</returns>
        public TagItem AddTag(TagItem tag)
        {
            int tagCountOld = _tags.Count();
            //Add to Dictionary
            tag.Id = _tagId++;
            _tags.Add(tag.TagName, tag);
            //Check that card was successfully added
            int tagCountNew = _tags.Count();
            if (tagCountNew - tagCountOld != 1)
            {
                throw new Exception("DATABASE ERROR: Tag was not added");
            }
            return tag;
        }


        //replicating getting data from database

        public UserItem GetUserItem(string username)
        {
            UserItem item = null;

            if (_userItems.ContainsKey(username))
            {
                item = _userItems[username];
            }
            else
            {
                throw new Exception("Item does not exist.");
            }

            return item.Clone();

        }

        /// <summary>
        /// Get card by id
        /// </summary>
        /// <param name="id">card id</param>
        /// <returns></returns>
        public CardItem GetCard(int id)
        {
            CardItem card = _cards[id];
            return card;
        }

        /// <summary>
        /// Search Mock Database for tag or term that mach searchword
        /// </summary>
        /// <param name="searchWord">word to search tags for</param>
        /// <returns></returns>
        public List<CardItem> SearchCards(string searchWord)
        {
            List<CardItem> resultCards = new List<CardItem>();
            foreach(KeyValuePair<int, CardItem> item in _cards)
            {
                // See if search word matches a term
                if (item.Value.Term.ToLower() == searchWord.ToLower())
                {
                    resultCards.Add(item.Value);
                }
                // See if any tag names match search word. Sorry for the double foreach loop :( -Jon
                else
                {
                    foreach (TagItem tag in item.Value.Tags)
                    {
                        if (tag.TagName.ToLower() == searchWord.ToLower())
                        {
                            resultCards.Add(item.Value);
                        }
                    }
                }
            }
            return resultCards;
        }

        /// <summary>
        /// Get Deck by name of deck
        /// </summary>
        /// <param name="deckName">name of deck</param>
        /// <returns>deck with name</returns>
        public DeckItem GetDeck(string deckName, int userId)
        {
            DeckItem result = _decks[deckName];
            return result;
        }

        /// <summary>
        /// Get deck by deck id
        /// </summary>
        /// <param name="deckId">id for deck</param>
        /// <returns>deck with id requested</returns>
        public DeckItem GetDeck(int deckId)
        {
            DeckItem result = new DeckItem();
            foreach(KeyValuePair<string, DeckItem> item in _decks)
            {
                if (item.Value.Id == deckId)
                {
                    result = item.Value;
                }
            }
            return result;
        }

        /// <summary>
        /// Get all decks for a user
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>All decks for user</returns>
        public List<DeckItem> GetAllDecksForUser(int userId)
        {
            List<DeckItem> userDecks = new List<DeckItem>();
            foreach(KeyValuePair<string, DeckItem> item in _decks)
            {
                if (item.Value.UserID == userId)
                {
                    userDecks.Add(item.Value);
                }
            }
            return userDecks;
        }

        /// <summary>
        /// Get all tags associated with a card
        /// </summary>
        /// <param name="cardId">card for which to get tags</param>
        /// <returns></returns>
        public List<TagItem> GetAllTagsForCard(int cardId)
        {
            List<TagItem> tags = _cards[cardId].Tags;
            return tags;
        }

        /// <summary>
        /// Search for tag by tag name
        /// </summary>
        /// <param name="tagName">tag name</param>
        /// <returns></returns>
        public TagItem GetTag(string tagName)
        {
            TagItem result = new TagItem();
            if (_tags.ContainsKey(tagName))
            {
                result = _tags[tagName];
            }
            //else
            //{
            //    throw new Exception("Tag does not exist in database");
            //}
            return result;
        }

        /// <summary>
        /// Return tag from database based on tag id
        /// </summary>
        /// <param name="tagId">id for tag to be found</param>
        /// <returns>TagItem object with selected id</returns>
        public TagItem GetTag(int tagId)
        {
            TagItem result = new TagItem();
            foreach(KeyValuePair<string, TagItem> item in _tags)
            {
                if (item.Value.Id == tagId)
                {
                    result = item.Value;
                }
            }
            if (result.TagName == null)
            {
                throw new Exception("Tag does not exist in database");
            }
            return result;
        }

        //Updates
        
        /// <summary>
        /// Update a card
        /// </summary>
        /// <param name="card">Card Item with data to update</param>
        /// <param name="cardId">Id of Card to be updated</param>
        /// <returns>Updated Card</returns>
        public bool UpdateCard(CardItem card, int cardId)
        {
            card.Id = cardId;
            _cards[cardId] = card;
            //return GetCard(cardId);

            return true;
        }

        /// <summary>
        /// Update deck in database
        /// </summary>
        /// <param name="deck">Updated Deck Object</param>
        /// <param name="deckId">id of deck in database to be updated</param>
        /// <returns>updated deck from database</returns>
        public bool UpdateDeck(DeckItem deck, int deckId)
        {
            DeckItem deckOld = GetDeck(deckId);
            deck.Name = deckOld.Name;
            deck.Id = deckId;
            _decks[deck.Name] = deck;
            //return deck;

            return true;
        }

        //Functionality

        /// <summary>
        /// Add a card to a deck
        /// </summary>
        /// <param name="cardId">id of card to add</param>
        /// <param name="deckId">id of deck to be added to</param>
        /// <returns>updated deck with card added</returns>
        public DeckItem AddCardToDeck(int cardId, int deckId)
        {
            //Get card and deck from mock database
            CardItem card = GetCard(cardId);
            DeckItem deck = GetDeck(deckId);

            if(deck.Cards == null)
            {
                deck.Cards = new List<CardItem>();
            }
            deck.Cards.Add(card);
            _decks[deck.Name] = deck;

            //return updated deck to make sure card was added
            deck = GetDeck(deckId);
            return deck;
        }

        /// <summary>
        /// Remove a card from a deck
        /// </summary>
        /// <param name="cardId">id of card to be removed</param>
        /// <param name="deckId">id of deck for card to be removed from</param>
        /// <returns>Updated deck</returns>
        public bool RemoveCardFromDeck(int cardId, int deckId)
        {
            //Get card and deck from mock database
            DeckItem deck = GetDeck(deckId);

            foreach(CardItem card in deck.Cards)
            {
                if (card.Id == cardId)
                {
                    deck.Cards.Remove(card);
                }
            }

            //return updated deck to make sure card was added
            deck = GetDeck(deckId);
            return true;
        }

        /// <summary>
        /// Add a tag to a card
        /// </summary>
        /// <param name="tagName">Name of tag to be added</param>
        /// <param name="cardId">Id of card to add tag to</param>
        /// <returns>updated card</returns>
        public TagItem AddTagToCard(string tagName, int cardId)
        {
            TagItem tag = GetTag(tagName);
            CardItem card = GetCard(cardId);

            if(card.Tags == null)
            {
                card.Tags = new List<TagItem>();
            }
            card.Tags.Add(tag);
            UpdateCard(card, cardId);
            return tag;
        }

        /// <summary>
        /// Remove a tag from a card
        /// </summary>
        /// <param name="tagName">name of tag to be removed</param>
        /// <param name="cardId">id of card from which to remove card</param>
        /// <returns></returns>
        public bool RemoveTagFromCard(string tagName, int cardId)
        {
            TagItem tag = GetTag(tagName);
            CardItem card = GetCard(cardId);

            card.Tags.Remove(tag);
            UpdateCard(card, cardId);
            return true;
        }

        //Deletion

        /// <summary>
        /// Delete card from all decks and mock database
        /// </summary>
        /// <param name="cardId">id of card to be deleted</param>
        public bool DeleteCard(int cardId)
        {
            bool itWorked = true;
            int oldCount = _cards.Count();
            //Remove card from decks
            foreach(KeyValuePair<string, DeckItem> item in _decks)
            {
                RemoveCardFromDeck(cardId, item.Value.Id);
            }

            //Remove card from database
            _cards.Remove(cardId);

            //confirm deletion
            int newCount = _cards.Count();
            if(!(oldCount - newCount == 1))
            {
                itWorked = false;
            }
            return itWorked;
        }

        /// <summary>
        /// Delete deck from mock database
        /// </summary>
        /// <param name="deckId">id of deck to be deleted</param>
        public bool DeleteDeck(int deckId)
        {
            bool itWorked = true;
            int oldCount = _decks.Count();

            //Get deckname and remove
            DeckItem deck = GetDeck(deckId);
            _decks.Remove(deck.Name);

            //Confirm deletion
            int newCount = _decks.Count();
            if (!(oldCount - newCount == 1))
            {
                itWorked = false;
            }
            return itWorked;
        }

        public bool DeleteAllTagsOnCard(int cardId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a Tag from all cards and mock database
        /// </summary>
        /// <param name="tagName">name of tag to be deleted</param>
        public bool DeleteTag(string tagName)
        {
            bool itWorked = true;
            int oldCount = _tags.Count();

            //Remove Tag from all cards
            foreach (KeyValuePair<string, DeckItem> item in _decks)
            {
                foreach (CardItem card in item.Value.Cards)
                {
                    RemoveTagFromCard(tagName, card.Id);
                }
            }

            //Remove Tag from mock database
            _tags.Remove(tagName);

            //Confirm deletion
            int newCount = _tags.Count();
            if (!(oldCount - newCount == 1))
            {
                itWorked = false;
            }
            return itWorked;
        }

        public List<CardItem> GetAllCardsForDeck(int deckId)
        {
            DeckItem deck = GetDeck(deckId);
            return deck.Cards;
        }

        public List<CardItem> GetAllUnassignedCards()
        {
            List<CardItem> cards = new List<CardItem>();
            return cards;
        }

        public bool GetUsername(string username, string password)
        {
            throw new NotImplementedException();
        }

        public List<UserItem> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public List<DeckItem> GetAllDecksForCard(int cardId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}