using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Web.Models;

namespace Capstone.Web
{
    public interface IDatabaseSvc
    {
        //User
        int AddUserItem(UserItem item);
        UserItem GetUserItem(string username);
        bool GetUsername(string username, string password);
        List<UserItem> GetAllUsers();

        //Card
        CardItem AddCard(CardItem card);
        CardItem GetCard(int id);
        List<CardItem> SearchCards(string searchWord);
        List<CardItem> GetAllCardsForDeck(int deckId);
        bool UpdateCard(CardItem card, int cardId);
        TagItem AddTagToCard(string tagName, int cardId);
        bool RemoveTagFromCard(string tagName, int cardId);
        bool DeleteCard(int cardId);
        List<CardItem> GetAllUnassignedCards();
        List<DeckItem> GetAllDecksForCard(int cardId, int userId);

        //Deck
        DeckItem AddDeck(DeckItem deck);
        DeckItem GetDeck(int deckId);
        List<DeckItem> GetAllDecksForUser(int userId);
        bool UpdateDeck(DeckItem deck, int deckId);
        DeckItem AddCardToDeck(int cardId, int deckId);
        bool RemoveCardFromDeck(int cardId, int deckId);
        bool DeleteDeck(int deckId);

        //Tags
        List<TagItem> GetAllTagsForCard(int cardId);
        bool DeleteAllTagsOnCard(int cardId);

    }
}
