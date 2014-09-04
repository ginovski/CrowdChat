using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace CrowdChat.Models
{
    public class Message
    {
        private string content;
        public Message(string content, User user)
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.Content = content;
            this.User = user;
            this.DateSended = DateTime.Now;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string Content
        {
            get
            {
                return this.content;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Message text cannot be null");
                }

                this.content = value;
            }
        }

        public DateTime DateSended { get; private set; }

        public User User { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}: {2}", this.DateSended, this.User.Name, this.Content);
        }
    }
}
