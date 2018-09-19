using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Capstone.Web.Models;

namespace Capstone.Web
{
    public class DatabaseSvc : IDatabaseSvc
    {
        //Member Variables
        private string _connectionString;
        
        //Single Parameter Constructor

        /// <summary>
        /// DatabaseSvc Constructor.  Connectings methods to database with provided connection string
        /// </summary>
        /// <param name="connectionString">Connection string to database for methods</param>
        public DatabaseSvc(string connectionString)
        {
            _connectionString = connectionString;

            //Populate database if emptygit puu
            List<UserItem> users = new List<UserItem>();
            users = GetAllUsers();

            if (users.Count == 0)
            {
                DbManager.PopulateRealDatabase(this);
            }
        }

        //Login Methods

        /// <summary>
        /// Add a user to the database
        /// </summary>
        /// <param name="user">User Object</param>
        /// <returns>Database Id for User Object</returns>
        public int AddUserItem(UserItem user)
        {
            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlReservation = "INSERT INTO [User] ( FirstName, LastName, UserName, Email, Password, Salt, IsAdmin) " +
                                        $"VALUES( @firstName, @lastName, @userName, @email, @password, @salt, 0); " +
                                        "SELECT CAST(scope_identity() as int);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlReservation;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);
                cmd.Parameters.AddWithValue("@userName", user.UserName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@salt", user.Salt);

                //Send command to database
                int? newId = (int?)cmd.ExecuteScalar();

                if(!newId.HasValue)
                {
                    throw new Exception("DATABASE ERROR: User could not be added.");
                }

                int userId = newId.Value ;
                return userId;
            }
        }

        /// <summary>
        /// Checks if a username and password exists for log in validation
        /// </summary>
        /// <param name="username">Username associated with User to pull from database</param>
        /// <returns></returns>
        public bool GetUsername(string username, string password)
        {
            bool correctUser = true;

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlGetUsername = "SELECT COUNT(UserName, Email, Password, Salt, IsAdmin) " +
                                       "FROM [User] " +
                                       "WHERE UserName = @username " +
                                       "AND Password = @password";
                
                //Pull data off of result set
                //UserItem user = new UserItem();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlGetUsername;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@password", password);

                //Send command to database
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 0)
                {
                    correctUser = false;
                }
            }
            return correctUser;
        }

        /// <summary>
        /// Get all users from database.  Primarily for populate database
        /// </summary>
        /// <returns>List of all Users in database</returns>
        public List<UserItem> GetAllUsers()
        {
            List<UserItem> result = new List<UserItem>();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT Id, FirstName, LastName, UserName, Email, Password, Salt, IsAdmin " +
                                       "FROM [User]; "; 

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    UserItem user = new UserItem();

                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.FirstName = Convert.ToString(reader["FirstName"]);
                    user.LastName = Convert.ToString(reader["LastName"]);
                    user.UserName = Convert.ToString(reader["UserName"]);

                    user.Email = Convert.ToString(reader["Email"]);
                    user.Password = Convert.ToString(reader["Password"]);
                    user.Salt = Convert.ToString(reader["Salt"]);
                    user.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);

                    result.Add(user);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve User Object from database based on id
        /// </summary>
        /// <param name="username">Username associated with User to pull from database</param>
        /// <returns></returns>
        public UserItem GetUserItem(string username)
        {
            UserItem result = new UserItem();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT Id, FirstName, LastName, UserName, Email, Password, Salt, IsAdmin " +
                                       "FROM [User] " +
                                       $"WHERE UserName = @userName ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@userName", username);
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    UserItem user = new UserItem();

                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.FirstName = Convert.ToString(reader["FirstName"]);
                    user.LastName = Convert.ToString(reader["LastName"]);
                    user.UserName = Convert.ToString(reader["UserName"]);

                    user.Email = Convert.ToString(reader["Email"]);
                    user.Password = Convert.ToString(reader["Password"]);
                    user.Salt = Convert.ToString(reader["Salt"]);
                    user.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);

                    result = user;
                }
            }
            return result;
        }

        //Flashcard Methods

        #region Add Methods

        /// <summary>
        /// Adds a flashCard to the database
        /// </summary>
        /// <param name="card">CardItem object</param>
        /// <returns>a new Card item</returns>
        public CardItem AddCard(CardItem card)
        {
            CardItem result = new CardItem();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlInsertCard = "INSERT INTO [Card] ( UserId, Term, Definition) " +
                                        $"VALUES(@userId, @term, @definition); " +
                                        "SELECT CAST(scope_identity() as int);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlInsertCard;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@userId", card.UserID);
                cmd.Parameters.AddWithValue("@term", card.Term);
                cmd.Parameters.AddWithValue("@definition", card.Definition);
                //cmd.Parameters.AddWithValue("@tagId", card.TagId);

                //Send command to database

                int? newId = (int?)cmd.ExecuteScalar();

                if (!newId.HasValue)
                {
                    throw new Exception("DATABASE ERROR: Card could not be added.");
                }

                int cardId = newId.Value;
                card.Id = cardId;
            }
            return card;
        }

        /// <summary>
        /// Add a card to a deck in the database
        /// </summary>
        /// <param name="cardId">id of card to be added to deck</param>
        /// <param name="deckId">id of deck to recieve new card</param>
        /// <returns>Updated deck item</returns>
        public DeckItem AddCardToDeck(int cardId, int deckId)
        {
            DeckItem deckItem = new DeckItem();
            
            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlInsertCard = "INSERT INTO [Deck_Card] (DeckId, CardId) " +
                                       "VALUES(@deckId, @cardId); ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlInsertCard;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@deckId", deckId);
                cmd.Parameters.AddWithValue("@cardId", cardId);

                ////Send command to database

                int rowsAffected = cmd.ExecuteNonQuery();

                
                if (rowsAffected == 0)
                {
                    throw new Exception("DATABASE ERROR: Card could not be added.");
                }
                
                //int cardId = newId.Value;
            }
            return deckItem;

        }

        /// <summary>
        /// Add Deck to Database (Without Any Cards)
        /// </summary>
        /// <param name="deck">DeckItem Object</param>
        /// <returns>DeckItem Object with Id added</returns>
        public DeckItem AddDeck(DeckItem deck)
        {
            DeckItem result = deck;

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlInsertDeck = "INSERT INTO [Deck] (UserId, Name, Description) " +
                                       $"VALUES (@UserId, @Name, @Description); " +
                                       "SELECT CAST(scope_identity() as int); ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlInsertDeck;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@UserId", deck.UserID);
                cmd.Parameters.AddWithValue("@Name", deck.Name);
                cmd.Parameters.AddWithValue("@Description", deck.Description);

                int? newId = (int?)cmd.ExecuteScalar();

                if (!newId.HasValue)
                {
                    throw new Exception("DATABASE ERROR: Deck could not be added.");
                }

                int deckId = newId.Value;
                result.Id = deckId;
            }
            return result;

        }

        /// <summary>
        /// Add a tag to a card in the database
        /// </summary>
        /// <param name="tagName">Name of tag to be added</param>
        /// <param name="cardId">Id of card for tag to be added to</param>
        /// <returns>TagItem object with tagId</returns>
        public TagItem AddTagToCard(string tagName, int cardId)
        {
            TagItem newTag = new TagItem();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlInsertCardItem = "INSERT INTO [TagItem] (TagName, CardId) " +
                                       $"VALUES(@tagName, @cardId); " +
                                       "SELECT CAST(scope_identity() as int); ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlInsertCardItem;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@tagName", tagName);
                cmd.Parameters.AddWithValue("@cardId", cardId);

                int? newId = (int?)cmd.ExecuteScalar();

                if (!newId.HasValue)
                {
                    throw new Exception("DATABASE ERROR: Tag could not be added.");
                }

                int tagId = newId.Value;
                newTag.Id = tagId;
            }
            return newTag;
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Delete A card from the database based on the cardId
        /// </summary>
        /// <param name="cardId">Id of card to be deleted</param>
        public bool DeleteCard(int cardId)
        {
            bool wasSuccessful = true;

            //Delete all tags associated with card to avoid foreign key exception
            DeleteAllTagsOnCard(cardId);

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    //Create sql statement
                    string sqlDeleteCard = "DELETE [Deck_Card] " +
                                            $"WHERE CardId = @cardId; " +
                                            "DELETE [Card] " +
                                            $"WHERE Id = @cardId;";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlDeleteCard;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@cardId", cardId);

                    //Send command to database
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        wasSuccessful = false;
                    }
                }
            }
            return wasSuccessful;
        }
        
        /// <summary>
        /// Delete all the tags associated with one card (Primarily for use within delete card method)
        /// </summary>
        /// <param name="cardId">Id of card where all tags should be deleted</param>
        /// <returns></returns>
        public bool DeleteAllTagsOnCard(int cardId)
        {
            bool wasSuccessful = true;

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    //Create sql statement
                    string sqlDeleteCard = "DELETE [TagItem] " +
                                            $"WHERE CardId = @cardId;";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sqlDeleteCard;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@cardId", cardId);

                    //Send command to database
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        wasSuccessful = false;
                    }
                }
            }
            return wasSuccessful;
        }

        /// <summary>
        /// Remove a Deck and all Deck connections from database
        /// </summary>
        /// <param name="deckId">Id of deck to be deleted</param>
        /// <returns>true if removal was successful</returns>
        public bool DeleteDeck(int deckId)
        {
            bool itWorked = true;
            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlDeleteDeck = "DELETE [Deck_Card] " +
                                       $"WHERE DeckId = @deckId; " +
                                       "DELETE [Deck] " +
                                       $"WHERE Id = @deckId ;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlDeleteDeck;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@deckId", deckId);

                //Send command to database
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    itWorked = false;
                }
            }
            return itWorked;
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Return a list of DeckItem objects assigned to a user ID
        /// </summary>
        /// <param name="userId">ID of user</param>
        /// <returns>List of user's decks</returns>
        public List<DeckItem> GetAllDecksForUser(int userId)
        {
            List<DeckItem> result = new List<DeckItem>();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT deck.Id, deck.UserId, deck.Name, deck.Description, COUNT(Deck_Card.CardId) as CardCount " +
                                       "FROM [Deck] " +
                                       $"LEFT JOIN [Deck_Card] ON Deck.Id = Deck_Card.DeckId " +
                                       $"WHERE deck.UserId = @userId " +
                                        "GROUP BY Deck_Card.DeckId, deck.userId, deck.Id, deck.Name, deck.Description;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@userId", userId);
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    DeckItem deck = new DeckItem();

                    deck.Id = Convert.ToInt32(reader["Id"]);
                    deck.UserID = Convert.ToInt32(reader["UserId"]);
                    deck.Name = Convert.ToString(reader["Name"]);
                    deck.Description = Convert.ToString(reader["Description"]);
                    deck.CardCount = Convert.ToInt32(reader["CardCount"]);
                    
                    result.Add(deck);
                }
            }
            return result;
        }

        /// <summary>
        /// Return a list of CardItem objects assigned to a deck
        /// </summary>
        /// <param name="deckId">Id of deck</param>
        /// <returns>List of Cards in deck</returns>
        public List<CardItem> GetAllCardsForDeck(int deckId)
        {
            List<CardItem> result = new List<CardItem>();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT card.Id, card.UserId, Term, Definition " +
                                       "FROM [Card] " +
                                       "JOIN Deck_Card ON card.Id = Deck_Card.CardId " +
                                       $"WHERE Deck_Card.DeckId = @deckId; ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@deckId", deckId);
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    CardItem card = new CardItem();

                    card.Id = Convert.ToInt32(reader["Id"]);
                    card.UserID = Convert.ToInt32(reader["UserId"]);
                    card.Term = Convert.ToString(reader["Term"]);
                    card.Definition = Convert.ToString(reader["Definition"]);

                    result.Add(card);
                }
            }
            return result;
        }

        public List<DeckItem> GetAllDecksForCard(int cardId, int userId)
        {
            List<DeckItem> result = new List<DeckItem>();
            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT deck.Id, deck.UserId, deck.Description, deck.Name " +
                                       "FROM [Deck] " +
                                       "JOIN Deck_Card ON deck.Id = Deck_Card.DeckId " +
                                       $"WHERE Deck_Card.CardId = @cardId " +
                                       $"AND Deck.UserId = @userId; ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.Parameters.AddWithValue("@userId", userId);

                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    DeckItem deck = new DeckItem();

                    deck.Id = Convert.ToInt32(reader["Id"]);
                    deck.UserID = Convert.ToInt32(reader["UserId"]);
                    deck.Name = Convert.ToString(reader["Name"]);
                    deck.Description = Convert.ToString(reader["Description"]);

                    result.Add(deck);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all tags assigned to a single card (primarily for GetCard Method)
        /// </summary>
        /// <param name="cardId">Card Id tags are assigned to</param>
        /// <returns></returns>
        public List<TagItem> GetAllTagsForCard(int cardId)
        {
            List<TagItem> result = new List<TagItem>();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT Id, CardId, TagName " +
                                       "FROM [TagItem] " +
                                       $"WHERE CardId = @cardId ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@cardId", cardId);
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    TagItem tag = new TagItem();

                    tag.Id = Convert.ToInt32(reader["Id"]);
                    tag.CardId = Convert.ToInt32(reader["CardId"]);
                    tag.TagName = Convert.ToString(reader["TagName"]);
                    

                    result.Add(tag);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets Card information based on card Id.  Runs GetAllTagsForCard method also.
        /// </summary>
        /// <param name="id">card id</param>
        /// <returns>CardItem Object</returns>
        public CardItem GetCard(int id)
        {
            CardItem card = new CardItem();
            const string sql = "SELECT * FROM [Card] WHERE Id = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    CardItem newCard = new CardItem();

                    newCard.Id = Convert.ToInt32(reader["Id"]);
                    newCard.UserID = Convert.ToInt32(reader["UserId"]);
                    newCard.Term = Convert.ToString(reader["Term"]);
                    newCard.Definition = Convert.ToString(reader["Definition"]);

                    card = newCard;
                }
            }
            card.Tags = GetAllTagsForCard(id);
            return card;
        }
        
        /// <summary>
        /// Returns a DeckItem object based on id (Does not currently return card list)
        /// </summary>
        /// <param name="deckId">id of deck</param>
        /// <returns>Deck</returns>
        public DeckItem GetDeck(int deckId)
        {
            DeckItem deckItem = new DeckItem();
            string sql = "SELECT deck.Id, deck.UserId, deck.Name, deck.Description, COUNT(Deck_Card.CardId) as CardCount " +
                        $"FROM [Deck] " +
                        $"LEFT JOIN [Deck_Card] ON Deck.Id = Deck_Card.DeckId " +
                        $"WHERE deck.Id = @Id " +
                        "GROUP BY Deck_Card.DeckId, deck.userId, deck.Id, deck.Name, deck.Description;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", deckId);
                
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    DeckItem newDeck = new DeckItem();

                    newDeck.Id = Convert.ToInt32(reader["Id"]);
                    newDeck.Name = Convert.ToString(reader["Name"]);
                    newDeck.UserID = Convert.ToInt32(reader["UserId"]);
                    newDeck.Description = Convert.ToString(reader["Description"]);
                    newDeck.CardCount = Convert.ToInt32(reader["CardCount"]);

                    deckItem = newDeck;
                }
            }
            return deckItem;
        }

        /// <summary>
        /// Return a list of CardItem objects NOT assigned to any deck
        /// </summary>
        /// <returns>List of Cards in deck</returns>
        public List<CardItem> GetAllUnassignedCards()
        {
            List<CardItem> result = new List<CardItem>();

            //Connect to Database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                string sqlPlayer = "SELECT card.Id, card.UserId, Term, Definition " +
                                       "FROM [Card] " +
                                       $"WHERE card.Id NOT IN (Select CardId from Deck_Card); ";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlPlayer;
                cmd.Connection = conn;
                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                //Pull data off of result set
                while (reader.Read())
                {
                    CardItem card = new CardItem();

                    card.Id = Convert.ToInt32(reader["Id"]);
                    card.UserID = Convert.ToInt32(reader["UserId"]);
                    card.Term = Convert.ToString(reader["Term"]);
                    card.Definition = Convert.ToString(reader["Definition"]);

                    result.Add(card);
                }
            }
            return result;
        }


        #endregion

        #region Remove and Update Methods 

        public bool RemoveCardFromDeck(int cardId, int deckId)
        {
            bool wasSuccessful = true;

            //Connect to database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create SQL statement
                const string sqlRemoveCardFromDeck = "Delete From Deck_Card Where CardId = @cardId AND DeckId = @deckId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlRemoveCardFromDeck;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@cardId", cardId);
                cmd.Parameters.AddWithValue("@deckId", deckId);

                //Send command to database
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                }

            }
            return wasSuccessful;
        }

        public bool RemoveTagFromCard(string tagName, int cardId)
        {
            bool wasSuccessful = true;

            //Connect to database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create SQL statement
                const string sqlRemoveTagFromCard = "Delete From TagItem Where TagName = @tagName AND CardId = @cardId;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlRemoveTagFromCard;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@tagName", tagName);
                cmd.Parameters.AddWithValue("@cardId", cardId);

                //Send command to database
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                }

            }
            return wasSuccessful;
        }

        public List<CardItem> SearchCards(string searchWord)
        {
            List<CardItem> result = new List<CardItem>();

            //Connect to the database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create sql statement
                const string sqlSearchCards = "Select Card.Id, Card.UserId, Card.Term, Card.Definition, TagItem.TagName " +
                                                     "FROM Card " +
                                                     "FULL JOIN TagItem ON Card.Id = TagItem.CardId " +
                                                     "WHERE Card.Term LIKE @searchWord " +
                                                     "OR Card.Definition LIKE @searchWord " +
                                                     "OR TagItem.TagName LIKE @searchWord;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlSearchCards;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@searchWord", "%" + searchWord + "%");

                //Send command to database
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CardItem card = PopulateCardFromReader(reader);
                    bool cardAlreadyAdded = false;
                    foreach(CardItem i in result)
                    {
                        if(i.Id == card.Id)
                        {
                            cardAlreadyAdded = true;
                        }

                    }
                    if (!cardAlreadyAdded)
                    {
                        result.Add(card);
                    }
                }
            }
            return result;
        }

        private CardItem PopulateCardFromReader(SqlDataReader reader)
        {
            CardItem item = new CardItem();

            // Read in the value from the reader
            // Reference by index or by column_name
            item.Id = Convert.ToInt32(reader["Id"]);
            item.UserID = Convert.ToInt32(reader["UserId"]);
            item.Term = Convert.ToString(reader["Term"]);
            item.Definition = Convert.ToString(reader["Definition"]);

            return item;
        }

        public bool UpdateCard(CardItem card, int cardId) //Note: updating tags might be done in views or sql statement can be changed to join tag and card tables (Diana)
        {
            bool wasSuccessful = true;

            //Connect to database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create SQL statement
                const string sqlUpdateCard = "UPDATE [Card] SET Term = @Term, Definition = @Definition, TagId = @TagId WHERE Id = @Id;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlUpdateCard;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Term", card.Term);
                cmd.Parameters.AddWithValue("@Definition", card.Definition);
                cmd.Parameters.AddWithValue("@TagId", 2);
                cmd.Parameters.AddWithValue("@Id", cardId);


                //Send command to database
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                }

            }
            return wasSuccessful;
        }

        public bool UpdateDeck(DeckItem deck,int deckId)
        {
            bool wasSuccessful = true;
            
            //Connect to database
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                //Create SQL statement
                const string sqlUpdateDeck = "UPDATE [Deck] SET Name = @Name, Description = @Description WHERE Id = @Id;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlUpdateDeck;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Name", deck.Name);
                cmd.Parameters.AddWithValue("@Description", deck.Description);
                cmd.Parameters.AddWithValue("@Id", deck.Id);

                //Send command to database
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    wasSuccessful = false;
                }

            }
            return wasSuccessful;
        }
        
        #endregion
    }
}