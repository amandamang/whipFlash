using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;

namespace Capstone.Web.Models
{
    public class DbManager
    {
        public static void PopulateDatabase(IDatabaseSvc db)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                // Add Standard User
                UserItem userItem = new UserItem()
                {
                    FirstName = "Standard",
                    LastName = "User",
                    UserName = "standardUser",
                    Email = "User@email.com",
                    Password = "password",
                    Salt = "saltysalt",
                    IsAdmin = false
                };
                db.AddUserItem(userItem);

                // Add Standard User 2
                userItem = new UserItem()
                {
                    FirstName = "Standard",
                    LastName = "UserTwo",
                    UserName = "secondUser",
                    Email = "User2@email.com",
                    Password = "password",
                    Salt = "saltysalt",
                    IsAdmin = false
                };
                db.AddUserItem(userItem);

                // Add Admin User
                userItem = new UserItem()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "adminUser",
                    Email = "Admin@email.com",
                    Password = "adminPass",
                    Salt = "saltyAdmin",
                    IsAdmin = true
                };
                db.AddUserItem(userItem);

                // Add Cards
                //id 1
                CardItem card = new CardItem()
                {
                    Term = "Polymorphism",
                    Definition = "polymorphism refers to a programming language's ability to process" +
                                 "objects differently depending on their data type or class. More" +
                                 "specifically, it is the ability to redefine methods for derived classes.",
                    UserID = 1
                };
                db.AddCard(card);

                //id2
                card = new CardItem()
                {
                    Term = "Inheritance",
                    Definition = "Inheritance enables new objects to take on the properties of existing objects. " +
                    "A class that is used as the basis for inheritance is called a superclass or base class. " +
                    "A class that inherits from a superclass is called a subclass or derived class.",
                    UserID = 1
                };
                db.AddCard(card);

                //id3
                card = new CardItem()
                {
                    Term = "Encapsulation",
                    Definition = "refers to the bundling of data with the methods that operate on that data." +
                    " Encapsulation is used to hide the values or state of a structured data object inside a class, " +
                    "preventing unauthorized parties' direct access to them.",
                    UserID = 1
                };
                db.AddCard(card);

                //id4
                card = new CardItem()
                {
                    Term = "hola",
                    Definition = "hello",
                    UserID = 1
                };
                db.AddCard(card);

                //id5
                card = new CardItem()
                {
                    Term = "Adios",
                    Definition = "goodbye",
                    UserID = 1
                };
                db.AddCard(card);

                //id6
                card = new CardItem()
                {
                    Term = "si",
                    Definition = "yes",
                    UserID = 1
                };
                db.AddCard(card);

                //id7
                card = new CardItem()
                {
                    Term = "De nada",
                    Definition = "Thank You",
                    UserID = 1
                };
                db.AddCard(card);

                //id8
                card = new CardItem()
                {
                    Term = "polimorfismo",
                    Definition = "polymorphism",
                    UserID = 1
                };
                db.AddCard(card);

                //id9
                card = new CardItem()
                {
                    Term = "Front (term) example",
                    Definition = "Back (definition) example",
                    UserID = 1
                };
                db.AddCard(card);

                //id10
                card = new CardItem()
                {
                    Term = "When did World War I begin and end?",
                    Definition = "July 28, 1914 - November 11, 1918",
                    UserID = 1
                };
                db.AddCard(card);

                //id11
                card = new CardItem()
                {
                    Term = "When did World War II begin and end?",
                    Definition = "September 1, 1939 - September 2, 1945",
                    UserID = 1
                };
                db.AddCard(card);

                //id12
                card = new CardItem()
                {
                    Term = "When did the Korean War begin and end?",
                    Definition = "June 25, 1950 - July 27, 1953",
                    UserID = 1
                };
                db.AddCard(card);

                //id13
                card = new CardItem()
                {
                    Term = "When did the Vietnam War begin and end?",
                    Definition = "November 1, 1955 - April 30, 1975",
                    UserID = 1
                };
                db.AddCard(card);

                //id14
                card = new CardItem()
                {
                    Term = "What are the main components of the nervous system?",
                    Definition = "The nervous system comprises the central nervous " +
                    "system, consisting of the brain and spinal cord, and the " +
                    "peripheral nervous system, consisting of the cranial, spinal, " +
                    "and peripheral nerves, together with their motor and sensory endings.",
                    UserID = 1
                };
                db.AddCard(card);

                //id15
                card = new CardItem()
                {
                    Term = "What is the locomotor system?",
                    Definition = "Also known as the human musculoskeletal system, " +
                    "the locomotor system consists of bone, cartilage, joints, muscle" +
                    " and tendons, giving humans the ability to move and provides" +
                    "support, stability, and movement to the body.",
                    UserID = 1
                };
                db.AddCard(card);

                //id16
                card = new CardItem()
                {
                    Term = "What is the vertebral column?",
                    Definition = "The vertebral column usually consists of 33 vertebrae: " +
                    "24 presacral vertebrae (7 cervical, 12 thoracic, and 5 lumbar) followed by " +
                    "the sacrum (5 fused sacral vertebrae) and the coccyx (4 frequently fused " +
                    "coccygeal vertebrae). The 24 presacral vertebrae allow movement and hence " +
                    "render the vertebral column flexible. Stability is provided by ligaments, " +
                    "muscles, and the form of the bones.",
                    UserID = 1
                };
                db.AddCard(card);

                //id17
                card = new CardItem()
                {
                    Term = "Describe the musculature of the arm and the elbow.",
                    Definition = "The muscles of the anterior arm are the biceps, coracobrachialis, " +
                    "and brachialis. They are supplied by the musculocutaneous nerve. The triceps is " +
                    "the muscle of the posterior arm, and it is supplied by the radial nerve. The " +
                    "anterior and posterior muscles are separated from each other by lateral " +
                    "and medial intermuscular septa",
                    UserID = 1
                };
                db.AddCard(card);

                //id18
                card = new CardItem()
                {
                    Term = "Describe the anatomy of the human ankle.",
                    Definition = "The word ankle refers to the angle between the leg and the foot. The foot " +
                    "functions in support and in locomotion, whereas the hand is a tactile and grasping organ. " +
                    "The toes are numbered from one to five, beginning with the great toe, or hallux. Thus, the " +
                    "pre-axial digit in either the hand or the foot is numbered one. The terms abduction and adduction " +
                    "of the toes are used with reference to an axis through the second toe. Thus, abduction of the " +
                    "big toe is a medial movement, away from the second toe. The tendons around the ankle (similar " +
                    "to those at the wrist) are bound down by retinacula",
                    UserID = 1
                };
                db.AddCard(card);

                // Add Decks
                //id 1
                DeckItem deck = new DeckItem()
                {
                    Name = "Principles of OOP",
                    UserID = 1,
                    Description = "Contains definitions of Object-oriented programming concepts."
                };
                db.AddDeck(deck);

                //id 2
                deck = new DeckItem()
                {
                    Name = "Spanish Vocabulary",
                    UserID = 1,
                    Description = "A deck of spanish vocabulary words with English equivalents."
                };
                db.AddDeck(deck);

                //id 3
                deck = new DeckItem()
                {
                    Name = "Military History",
                    UserID = 1,
                    Description = "Contains some historical dates of various wars."
                };
                db.AddDeck(deck);

                //id 4
                deck = new DeckItem()
                {
                    Name = "Test deck - Name field",
                    UserID = 1,
                    Description = "Here's a blank deck for reference - this is the Description field."
                };
                db.AddDeck(deck);

                //id 5
                deck = new DeckItem()
                {
                    Name = "Human Anatomy",
                    UserID = 1,
                    Description = "Human anatomy basics."
                };
                db.AddDeck(deck);

                //Add Tag to Cards
                db.AddTagToCard("OOP", 1);
                db.AddTagToCard("OOP", 2);
                db.AddTagToCard("OOP", 3);
                db.AddTagToCard("Spanish", 4);
                db.AddTagToCard("Spanish", 5);
                db.AddTagToCard("Spanish", 6);
                db.AddTagToCard("Spanish", 7);
                db.AddTagToCard("Spanish", 8);
                db.AddTagToCard("example tag", 9);
                db.AddTagToCard("War History", 10);
                db.AddTagToCard("War History", 11);
                db.AddTagToCard("War History", 12);
                db.AddTagToCard("War History", 13);
                db.AddTagToCard("OOP", 9);
                db.AddTagToCard("misc", 9);
                db.AddTagToCard("another tag", 9);
                db.AddTagToCard("even more tagz", 9);
                db.AddTagToCard("anatomy", 14);
                db.AddTagToCard("anatomy", 15);
                db.AddTagToCard("anatomy", 16);
                db.AddTagToCard("anatomy", 17);
                db.AddTagToCard("anatomy", 18);

                //Add Cards to Decks
                db.AddCardToDeck(1, 1);
                db.AddCardToDeck(2, 1);
                db.AddCardToDeck(3, 1);
                db.AddCardToDeck(4, 2);
                db.AddCardToDeck(5, 2);
                db.AddCardToDeck(6, 2);
                db.AddCardToDeck(7, 2);
                db.AddCardToDeck(8, 2);
                db.AddCardToDeck(9, 4);
                db.AddCardToDeck(10, 3);
                db.AddCardToDeck(11, 3);
                db.AddCardToDeck(12, 3);
                db.AddCardToDeck(13, 3);
                db.AddCardToDeck(14, 5);
                db.AddCardToDeck(15, 5);
                db.AddCardToDeck(16, 5);
                db.AddCardToDeck(17, 5);
                db.AddCardToDeck(18, 5);


                scope.Complete();

            }
        }

        public static void PopulateRealDatabase(IDatabaseSvc db)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                // Add Standard User
                UserItem userItem = new UserItem("password")
                {
                    FirstName = "Standard",
                    LastName = "User",
                    UserName = "standardUser",
                    Email = "User@email.com",
                    IsAdmin = false
                };
                int user1Id = db.AddUserItem(userItem);

                // Add Standard User 2
                userItem = new UserItem("password")
                {
                    FirstName = "Standard",
                    LastName = "UserTwo",
                    UserName = "secondUser",
                    Email = "User2@email.com",
                    IsAdmin = false
                };
                int user2Id = db.AddUserItem(userItem);

                // Add Admin User
                userItem = new UserItem()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "adminUser",
                    Email = "Admin@email.com",
                    Password = "adminPass",
                    Salt = "saltyAdmin",
                    IsAdmin = true
                };
                int admin1Id = db.AddUserItem(userItem);

                // Add Cards
                //id 1
                CardItem card = new CardItem()
                {
                    Term = "Polymorphism",
                    Definition = "polymorphism refers to a programming language's ability to process" +
                                 "objects differently depending on their data type or class. More" +
                                 "specifically, it is the ability to redefine methods for derived classes.",
                    UserID = user1Id
                };
                CardItem card1 = db.AddCard(card);

                //id2
                card = new CardItem()
                {
                    Term = "Inheritance",
                    Definition = "Inheritance enables new objects to take on the properties of existing objects. " +
                    "A class that is used as the basis for inheritance is called a superclass or base class. " +
                    "A class that inherits from a superclass is called a subclass or derived class.",
                    UserID = user1Id
                };
                CardItem card2 = db.AddCard(card);


                //id3
                card = new CardItem()
                {
                    Term = "Encapsulation",
                    Definition = "refers to the bundling of data with the methods that operate on that data." +
                    " Encapsulation is used to hide the values or state of a structured data object inside a class, " +
                    "preventing unauthorized parties' direct access to them.",
                    UserID = user1Id
                };
                CardItem card3 = db.AddCard(card);

                //id4
                card = new CardItem()
                {
                    Term = "hola",
                    Definition = "hello",
                    UserID = user1Id
                };
                CardItem card4 = db.AddCard(card);


                //id5
                card = new CardItem()
                {
                    Term = "Adios",
                    Definition = "goodbye",
                    UserID = user1Id
                };
                CardItem card5 = db.AddCard(card);

                //id6
                card = new CardItem()
                {
                    Term = "si",
                    Definition = "yes",
                    UserID = user1Id
                };
                CardItem card6 = db.AddCard(card);

                //id7
                card = new CardItem()
                {
                    Term = "De nada",
                    Definition = "Thank You",
                    UserID = user1Id
                };
                CardItem card7 = db.AddCard(card);

                //id8
                card = new CardItem()
                {
                    Term = "polimorfismo",
                    Definition = "polymorphism",
                    UserID = user1Id
                };
                CardItem card8 = db.AddCard(card);

                //id9
                card = new CardItem()
                {
                    Term = "Front (term) example",
                    Definition = "Back (definition) example",
                    UserID = user1Id
                };
                CardItem card9 = db.AddCard(card);

                //id10
                card = new CardItem()
                {
                    Term = "When did World War I begin and end?",
                    Definition = "July 28, 1914 - November 11, 1918",
                    UserID = user1Id
                };
                CardItem card10 = db.AddCard(card);

                //id11
                card = new CardItem()
                {
                    Term = "When did World War II begin and end?",
                    Definition = "September 1, 1939 - September 2, 1945",
                    UserID = user1Id
                };
                CardItem card11 = db.AddCard(card);

                //id12
                card = new CardItem()
                {
                    Term = "When did the Korean War begin and end?",
                    Definition = "June 25, 1950 - July 27, 1953",
                    UserID = user1Id
                };
                CardItem card12 = db.AddCard(card);

                //id13
                card = new CardItem()
                {
                    Term = "When did the Vietnam War begin and end?",
                    Definition = "November 1, 1955 - April 30, 1975",
                    UserID = user1Id
                };
                CardItem card13 = db.AddCard(card);

                //id14
                card = new CardItem()
                {
                    Term = "What are the main components of the nervous system?",
                    Definition = "The nervous system comprises the central nervous " +
                    "system, consisting of the brain and spinal cord, and the " +
                    "peripheral nervous system, consisting of the cranial, spinal, " +
                    "and peripheral nerves, together with their motor and sensory endings.",
                    UserID = user1Id
                };
                CardItem card14 = db.AddCard(card);

                //id15
                card = new CardItem()
                {
                    Term = "What is the locomotor system?",
                    Definition = "Also known as the human musculoskeletal system, " +
                    "the locomotor system consists of bone, cartilage, joints, muscle" +
                    " and tendons, giving humans the ability to move and provides" +
                    "support, stability, and movement to the body.",
                    UserID = user1Id
                };
                CardItem card15 = db.AddCard(card);

                //id16
                card = new CardItem()
                {
                    Term = "What is the vertebral column?",
                    Definition = "The vertebral column usually consists of 33 vertebrae: " +
                    "24 presacral vertebrae (7 cervical, 12 thoracic, and 5 lumbar) followed by " +
                    "the sacrum (5 fused sacral vertebrae) and the coccyx (4 frequently fused " +
                    "coccygeal vertebrae). The 24 presacral vertebrae allow movement and hence " +
                    "render the vertebral column flexible. Stability is provided by ligaments, " +
                    "muscles, and the form of the bones.",
                    UserID = user1Id
                };
                CardItem card16 = db.AddCard(card);

                //id17
                card = new CardItem()
                {
                    Term = "Describe the musculature of the arm and the elbow.",
                    Definition = "The muscles of the anterior arm are the biceps, coracobrachialis, " +
                    "and brachialis. They are supplied by the musculocutaneous nerve. The triceps is " +
                    "the muscle of the posterior arm, and it is supplied by the radial nerve. The " +
                    "anterior and posterior muscles are separated from each other by lateral " +
                    "and medial intermuscular septa",
                    UserID = user1Id
                };
                CardItem card17 = db.AddCard(card);

                //id18
                card = new CardItem()
                {
                    Term = "Describe the anatomy of the human ankle.",
                    Definition = "The word ankle refers to the angle between the leg and the foot. The foot " +
                    "functions in support and in locomotion, whereas the hand is a tactile and grasping organ. " +
                    "The toes are numbered from one to five, beginning with the great toe, or hallux. Thus, the " +
                    "pre-axial digit in either the hand or the foot is numbered one. The terms abduction and adduction " +
                    "of the toes are used with reference to an axis through the second toe. Thus, abduction of the " +
                    "big toe is a medial movement, away from the second toe. The tendons around the ankle (similar " +
                    "to those at the wrist) are bound down by retinacula",
                    UserID = user1Id
                };
                CardItem card18 = db.AddCard(card);

                // Add Decks
                //id 1
                DeckItem deck = new DeckItem()
                {
                    Name = "Principles of OOP",
                    UserID = user1Id,
                    Description = "Contains definitions of Object-oriented programming concepts."
                };
                DeckItem deck1 = db.AddDeck(deck);

                //id 2
                deck = new DeckItem()
                {
                    Name = "Spanish Vocabulary",
                    UserID = user1Id,
                    Description = "A deck of spanish vocabulary words with English equivalents."
                };
                DeckItem deck2 = db.AddDeck(deck);

                //id 3
                deck = new DeckItem()
                {
                    Name = "War Dates History",
                    UserID = user1Id,
                    Description = "Contains some historical dates of various wars."
                };
                DeckItem deck3 = db.AddDeck(deck);

                //id 4
                deck = new DeckItem()
                {
                    Name = "Test deck - Name field",
                    UserID = user1Id,
                    Description = "Here's a blank deck for reference - this is the Description field."
                };
                DeckItem deck4 = db.AddDeck(deck);

                //id 5
                deck = new DeckItem()
                {
                    Name = "Human Anatomy",
                    UserID = user1Id,
                    Description = "Human anatomy basics."
                };
                DeckItem deck5 = db.AddDeck(deck);

                //Add Tag to Cards
                db.AddTagToCard("OOP", card1.Id);
                db.AddTagToCard("OOP", card2.Id);
                db.AddTagToCard("OOP", card3.Id);
                db.AddTagToCard("Spanish", card4.Id);
                db.AddTagToCard("Spanish", card5.Id);
                db.AddTagToCard("Spanish", card6.Id);
                db.AddTagToCard("Spanish", card7.Id);
                db.AddTagToCard("Spanish", card8.Id);
                db.AddTagToCard("example tag", card9.Id);
                db.AddTagToCard("War History", card10.Id);
                db.AddTagToCard("War History", card11.Id);
                db.AddTagToCard("War History", card12.Id);
                db.AddTagToCard("War History", card13.Id);
                db.AddTagToCard("OOP", card8.Id);
                db.AddTagToCard("misc", card9.Id);
                db.AddTagToCard("another tag", card9.Id);
                db.AddTagToCard("even more tagz", card9.Id);
                db.AddTagToCard("anatomy", card14.Id);
                db.AddTagToCard("anatomy", card15.Id);
                db.AddTagToCard("anatomy", card16.Id);
                db.AddTagToCard("anatomy", card17.Id);
                db.AddTagToCard("anatomy", card18.Id);

                //Add Cards to Decks
                db.AddCardToDeck(card1.Id, deck1.Id);
                db.AddCardToDeck(card2.Id, deck1.Id);
                db.AddCardToDeck(card3.Id, deck1.Id);
                db.AddCardToDeck(card4.Id, deck2.Id);
                db.AddCardToDeck(card5.Id, deck2.Id);
                db.AddCardToDeck(card6.Id, deck2.Id);
                db.AddCardToDeck(card7.Id, deck2.Id);
                db.AddCardToDeck(card8.Id, deck2.Id);
                db.AddCardToDeck(card9.Id, deck4.Id);
                db.AddCardToDeck(card10.Id, deck3.Id);
                db.AddCardToDeck(card11.Id, deck3.Id);
                db.AddCardToDeck(card12.Id, deck3.Id);
                db.AddCardToDeck(card13.Id, deck3.Id);
                db.AddCardToDeck(card14.Id, deck5.Id);
                db.AddCardToDeck(card15.Id, deck5.Id);
                db.AddCardToDeck(card16.Id, deck5.Id);
                db.AddCardToDeck(card17.Id, deck5.Id);
                db.AddCardToDeck(card18.Id, deck5.Id);


                scope.Complete();

            }
        }



        private string _password;
        private static int _workFactor = 20;
        private static int _saltSize = 16;

        public string Salt { get; set; }
        public string Hash { get; set; }

        public DbManager(string password)
        {
            _password = password;
            GenerateSalt();
            GenerateHash();
        }

        public DbManager(string password, string salt)
        {
            _password = password;
            Salt = salt;
            GenerateHash();
        }

        public bool Verify(string hash)
        {
            return Hash == hash;
        }

        #region Private methods

        private void GenerateSalt()
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(_password, _saltSize, _workFactor);
            Salt = GetSalt(rfc);
        }

        private void GenerateHash()
        {
            Rfc2898DeriveBytes rfc = HashPasswordWithPBKDF2(_password, Salt);
            Hash = GetHash(rfc);
        }

        private static Rfc2898DeriveBytes HashPasswordWithPBKDF2(string password, string salt)
        {
            // Creates the crypto service provider and provides the salt - usually used to check for a password match
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), _workFactor);

            return rfc2898DeriveBytes;
        }

        private static string GetHash(Rfc2898DeriveBytes rfc)
        {
            return Convert.ToBase64String(rfc.GetBytes(20));
        }

        private static string GetSalt(Rfc2898DeriveBytes rfc)
        {
            return Convert.ToBase64String(rfc.Salt);
        }
        #endregion

        
        
    }
}