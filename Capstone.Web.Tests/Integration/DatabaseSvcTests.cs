using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Capstone.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Web.Tests.Integration
{
    [TestClass]
    public class DatabaseSvcTests
    {
        //SQL Member Variables
        private TransactionScope _tran;
        private string _connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = FlashCards; trusted_Connection=Yes;";

        UserItem user = new UserItem();
        DeckItem deck1 = new DeckItem();
        DeckItem deck2 = new DeckItem();


        CardItem card1 = new CardItem();
        CardItem card2 = new CardItem();

        TagItem tag1 = new TagItem();
        TagItem tag2 = new TagItem();


        //Test User Member Variables
        private int _userId;
        private const string TestFirstName = "John";
        private const string TestLastName = "Doe";
        private const string TestEmail = "John.Doe@gmail.com";
        private const string TestUserName = "TestUser";
        private const string TestPassword = "TestPassword";
        private const string TestSalt = "SaltyTest";

        //Test Deck Member Variables
        private int _deckId1;
        private int _deckId2;

        private int _deckUserId;
        private const string TestDeckName = "Test Deck";
        private List<CardItem> _testDeckCards = new List<CardItem>();
        private const string TestDeckDescription ="Test Deck For Testing";

        //Test Card Member Variables
        private int _cardId1;
        private const string TestCard1Term = "Term1";
        private const string TestCard1Definition = "Definition1";
        private const string TestCard1Name = "Card1";
        private List<TagItem> _testCard1Tags = new List<TagItem>();

        private int _cardId2;
        private const string TestCard2Term = "Term no. 2";
        private const string TestCard2Definition = "Definition2";
        private const string TestCard2Name = "Card2";
        private List<TagItem> _testCard2Tags = new List<TagItem>();

        //Test Tag Member Variables
        private int _tagId1;
        private int _tagId2;
        private const string TestTag1Name = "Tag1";
        private const string TestTag2Name = "Tag2";


        [TestInitialize]
        public void Initialize()
        {
            _tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                //Insert User and get userId
                cmd = new SqlCommand("INSERT INTO [User] ( FirstName, LastName, UserName, Email, Password, Salt, IsAdmin) " +
                                    $"VALUES('{TestFirstName}', '{TestLastName}', '{TestUserName}', '{TestEmail}', '{TestPassword}', '{TestSalt}', 0); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);

                _userId = (int)cmd.ExecuteScalar();
                user.Id = _userId;

                //Insert Cards and get CardIds
                cmd = new SqlCommand("INSERT INTO [Card] (UserId, Term, Definition, TagId) " +
                                     $"VALUES({_userId}, '{TestCard1Term}', '{TestCard1Definition}', 2); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                _cardId1 = (int)cmd.ExecuteScalar();
                card1.Id = _cardId1;

                cmd = new SqlCommand("INSERT INTO [Card] (UserId, Term, Definition, TagId) " +
                                     $"VALUES({_userId}, '{TestCard2Term}', '{TestCard2Definition}', 2); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                _cardId2 = (int)cmd.ExecuteScalar();
                card2.Id = _cardId2;

                //Insert Tags and get tagIds
                cmd = new SqlCommand("INSERT INTO [TagItem] (CardId, TagName) " +
                                     $"VALUES({_cardId1}, '{TestTag1Name}'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);

                _tagId1 = (int)cmd.ExecuteScalar();
                tag1.Id = _tagId1;

                cmd = new SqlCommand("INSERT INTO [TagItem] (CardId, TagName) " +
                                     $"VALUES({_cardId1}, '{TestTag2Name}'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);

                _tagId2 = (int)cmd.ExecuteScalar();
                tag2.Id = _tagId2;

                //Insert Deck

                cmd = new SqlCommand("INSERT INTO [Deck] (UserId, Name, Description) " +
                                    $"VALUES ({_userId}, '{TestDeckName}', '{TestDeckDescription}'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", conn);

                _deckId1 = (int)cmd.ExecuteScalar();
                deck1.Id = _deckId1;

                //Add card to Deck
                cmd = new SqlCommand("INSERT INTO [Deck_Card] (DeckId, CardId) " +
                                     $"VALUES({_deckId1}, {_cardId1}); ", conn);
                cmd.ExecuteScalar();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _tran.Dispose();
        }

        #region User Method Tests

        [TestMethod]
        public void GetUserItemTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            UserItem user = testDAL.GetUserItem(TestUserName);
            //Assert
            Assert.AreEqual(_userId, user.Id);
            Assert.AreEqual(TestPassword, user.Password);
            Assert.AreEqual(TestSalt, user.Salt);

        }

        [TestMethod]
        public void AddUserItemTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            UserItem testUser = new UserItem
            {
                FirstName = "Jimmi",
                LastName = "Test",
                Email = "Jimmi@Test.com",
                UserName = "JimmiTest",
                Password = "JimmiPassword",
                Salt = "JimmiSalt",
                IsAdmin = false
            };

            //Act
            int testUserId = testDAL.AddUserItem(testUser);
            UserItem confirmTestUser = testDAL.GetUserItem(testUser.UserName);

            //Assert
            int confirmTestUserId = _userId + 1;
            Assert.AreEqual(confirmTestUserId, confirmTestUser.Id);
            Assert.AreEqual(confirmTestUser.Password, testUser.Password);
            Assert.AreEqual(confirmTestUser.Salt, testUser.Salt);
        }

        [TestMethod]
        public void GetAllDecksForUserTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            List<DeckItem> decks = testDAL.GetAllDecksForUser(_userId);

            //Assert
            Assert.AreEqual(1, decks.Count(), "Confirm number of decks");
            Assert.AreEqual(TestDeckName, decks[0].Name, "Confirm deck name");
            Assert.AreEqual(1, decks[0].CardCount, "Confirm card count");
        }
        #endregion

        #region Card Tests

        [TestMethod]
        public void SearchCardsTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);
            string searchTagTest = TestTag1Name;
            string searchTermTest = TestCard2Term;
            string searchDefinitionTest = TestCard2Definition;

            string searchTagLikeTest = "Tag";
            string searchTermLikeTest = "Term";
            string searchDefinitionLikeTest = "Definition";

            //Act
            //Test Exact Term results
            List<CardItem> tagTestResult = testDAL.SearchCards(searchTagTest);
            List<CardItem> termTestResult = testDAL.SearchCards(searchTermTest);
            List<CardItem> definitionTestResult = testDAL.SearchCards(searchDefinitionTest);

            //Test "Like" terms test
            List<CardItem> tagLikeTestResult = testDAL.SearchCards(searchTagLikeTest);
            List<CardItem> termLikeTestResult = testDAL.SearchCards(searchTermLikeTest);
            List<CardItem> definitionLikeTestResult = testDAL.SearchCards(searchDefinitionLikeTest);

            //Get cards from database to compare
            CardItem card1 = testDAL.GetCard(_cardId1);
            CardItem card2 = testDAL.GetCard(_cardId2);

            List<CardItem> allCards = new List<CardItem> { card1, card2};

            //Assert
            Assert.AreEqual(tagTestResult[0].Id, card1.Id, "Exact Tag Test");
            Assert.AreEqual(termTestResult[0].Id, card2.Id, "Exact Term Test");
            Assert.AreEqual(definitionTestResult[0].Id, card2.Id, "Exact Definition Test");

            Assert.AreEqual(tagLikeTestResult[1].Id, card1.Id, "Like Tag Test");
            Assert.AreEqual(termLikeTestResult[3].Id, allCards[0].Id, "Like Term Test id1");
            Assert.AreEqual(termLikeTestResult[4].Id, allCards[1].Id, "Like Term Test id2");
            Assert.AreEqual(5, termLikeTestResult.Count(), "Like Term Count Test");
            Assert.AreEqual(definitionLikeTestResult[1].Id, allCards[0].Id, "Like Definition Test");
            Assert.AreEqual(definitionLikeTestResult[2].Id, allCards[1].Id, "Like Definition Test");
            Assert.AreEqual(3, definitionLikeTestResult.Count(), "Like Definition Count Test");

        }

        [TestMethod]
        public void GetCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            CardItem card = testDAL.GetCard(_cardId1);

            //Assert
            Assert.AreEqual(_cardId1, card.Id, "Confirm card id");
            Assert.AreEqual(TestCard1Term, card.Term, "Confirm term");
            Assert.AreEqual(TestCard1Definition, card.Definition, "confirm definition");
            Assert.AreEqual(2, card.Tags.Count(), "Confirm tags were added");

        }

        [TestMethod]
        public void GetAllDecksForCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            List<DeckItem> decks = testDAL.GetAllDecksForCard(_cardId1, _userId);

            //Assert
            Assert.AreEqual(1, decks.Count(), "Confirm number of Decks");
            Assert.AreEqual(TestDeckName, decks[0].Name, "Confirm Deck Name");
        }

        [TestMethod]
        public void AddCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            CardItem testCardNoId = new CardItem
            {
                Term = "Front",
                Definition = "Back",
                UserID = _userId
            };

            //Act
            CardItem testCardHasId = testDAL.AddCard(testCardNoId);
            CardItem confirmTestCard = testDAL.GetCard(testCardHasId.Id);

            //Assert
            Assert.AreEqual(testCardNoId.Term, confirmTestCard.Term, "Confirm Term");
            Assert.AreEqual(testCardNoId.Definition, confirmTestCard.Definition, "Confirm Definition");
        }

        [TestMethod]
        public void DeleteCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            int testId = _cardId1;

            //Act
            bool cardDeletionSuccess = testDAL.DeleteCard(testId);

            //Assert
            Assert.AreEqual(true, cardDeletionSuccess, "Confirm deletion");
        }

        [TestMethod]
        public void UpdateCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);
            
            CardItem testCard = new CardItem
            {
                Id = _cardId1,
                Term = "Front",
                Definition = "Back",
                UserID = _userId
            };

            //Act
            bool CardUpdated = testDAL.UpdateCard(testCard, _cardId1);
            CardItem confirmTestCard = testDAL.GetCard(_cardId1);

            //Assert
            Assert.AreEqual(testCard.Term, confirmTestCard.Term, "Confirm Term");
            Assert.AreEqual(testCard.Definition, confirmTestCard.Definition, "Confirm Definition");
        }

        #endregion

        #region Deck Tests
        
        [TestMethod]
        public void AddDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            DeckItem testDeckNoId = new DeckItem
            {
                Name = "Test Name",
                Description = "Test Description",
                UserID = _userId
            };

            //Act
            DeckItem testDeckHasId = testDAL.AddDeck(testDeckNoId);
            DeckItem confirmTestDeck = testDAL.GetDeck(testDeckHasId.Id);

            //Assert
            int confirmTestDeckId = _deckId1 + 1;

            Assert.AreEqual(confirmTestDeckId, testDeckHasId.Id, "Confirm new id");
            Assert.AreEqual(testDeckNoId.Name, confirmTestDeck.Name, "Confirm Name");
            Assert.AreEqual(testDeckNoId.Description, confirmTestDeck.Description, "Confirm Description");

        }

        [TestMethod]
        public void GetDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            DeckItem deck = testDAL.GetDeck(_deckId1);

            //Assert
            Assert.AreEqual(_deckId1, deck.Id, "Confirm deck id");
            Assert.AreEqual(TestDeckName, deck.Name, "Confirm deck name");
            Assert.AreEqual(TestDeckDescription, deck.Description, "Confirm deck description");
            Assert.AreEqual(1, deck.CardCount, "Confirm number of cards");

        }

        [TestMethod]
        public void UpdateDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            DeckItem testDeck= new DeckItem
            {
                Id = _deckId1,
                Name = "New Test Deck!",
                Description = "Wow a cool new deck for testing updates!",
                UserID = _userId
            };

            //Act
            bool deckUpdated = testDAL.UpdateDeck(testDeck, _deckId1);
            DeckItem confirmTestDeck = testDAL.GetDeck(_deckId1);

            //Assert
            Assert.AreEqual(true, deckUpdated, "Confirm Bool");
            Assert.AreEqual(testDeck.Name, confirmTestDeck.Name, "Confirm Name");
            Assert.AreEqual(testDeck.Description, confirmTestDeck.Description, "Confirm Description");
        }

        [TestMethod]
        public void GetAllCardsInDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            List<CardItem> cards = testDAL.GetAllCardsForDeck(_deckId1);

            //Assert
            Assert.AreEqual(1, cards.Count(), "Confirm number of tags");
            Assert.AreEqual(TestCard1Term, cards[0].Term);
        }

        [TestMethod]
        public void AddCardToDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            List<CardItem> oldCards = testDAL.GetAllCardsForDeck(_deckId1);
            DeckItem deck= testDAL.AddCardToDeck(_cardId2, _deckId1);
            List<CardItem> newCards = testDAL.GetAllCardsForDeck(_deckId1);

            //Assert
            Assert.AreEqual(1, (newCards.Count() - oldCards.Count()), "Confirm number of cards");
            Assert.AreEqual(TestCard1Term, newCards[0].Term, "Confirm old card term");
            Assert.AreEqual(TestCard2Term, newCards[1].Term, "Confirm new card term");

        }

        [TestMethod]
        public void RemoveCardFromDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            int cardId = _cardId1;
            string tagName = TestTag1Name;

            //Act
            int oldTagCount = testDAL.GetAllTagsForCard(cardId).Count();
            bool tagDeletionSuccess = testDAL.RemoveTagFromCard(tagName, cardId);
            int newTagCount = testDAL.GetAllTagsForCard(cardId).Count();
            bool removedTag = false;

            if (oldTagCount - newTagCount == 1)
            {
                removedTag = true;
            }

            //Assert
            Assert.AreEqual(true, tagDeletionSuccess, "Confirm method success");
            Assert.AreEqual(true, removedTag, "Confirm removal from database");
        }

        [TestMethod]
        public void DeleteDeckTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            int testId = _deckId1;

            //Act
            int oldDeckCount = testDAL.GetAllDecksForUser(_userId).Count();
            bool cardDeletionSuccess = testDAL.DeleteDeck(testId);
            int newDeckCount = testDAL.GetAllDecksForUser(_userId).Count();
            bool removedDeck = false;

            if(oldDeckCount - newDeckCount == 1)
            {
                removedDeck = true;
            }

            //Assert
            Assert.AreEqual(true, cardDeletionSuccess, "Confirm method success");
            Assert.AreEqual(true, removedDeck, "Confirm removal from database");

        }

        #endregion

        #region Tag Tests

        [TestMethod]
        public void DeleteAllTagsOnCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            int testId = _cardId1;

            //Act
            bool testDeletionSuccess = testDAL.DeleteAllTagsOnCard(testId);

            //Assert
            Assert.AreEqual(true, testDeletionSuccess, "Confirm deletion");
        }

        [TestMethod]
        public void GetAllTagsForCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);

            //Act
            List<TagItem> tags = testDAL.GetAllTagsForCard(_cardId1);

            //Assert
            Assert.AreEqual(2, tags.Count(), "Confirm number of tags");
            Assert.AreEqual(TestTag1Name, tags[0].TagName);
            Assert.AreEqual(TestTag2Name, tags[1].TagName);
        }

        [TestMethod]
        public void AddTagToCardTest()
        {
            //Arrange 
            DatabaseSvc testDAL = new DatabaseSvc(_connectionString);


            string tagName = "AddTag Test";
            int cardId = _cardId1;
            

            //Act
            TagItem testTagHasId = testDAL.AddTagToCard(tagName, cardId);
            CardItem confirmCard = testDAL.GetCard(_cardId1);

            //Assert
            bool tagAdded = false;
            foreach( TagItem item in confirmCard.Tags)
            {
                if(item.TagName == tagName)
                {
                    tagAdded = true;
                }

            }

            Assert.AreEqual(true, tagAdded, "Confirm tag was added");
        }

        #endregion
    }
}
