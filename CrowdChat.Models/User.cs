namespace CrowdChat.Models
{
    using System;

    public class User
    {
        private string name;

        public User(string name)
        {
            this.Name = name;
            this.DateRegistered = DateTime.Now;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Name cannot be null or empty");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("Name cannot be less than 3 symbols");
                }

                this.name = value;
            }
        }

        public DateTime DateRegistered { get; private set; }
    }
}
