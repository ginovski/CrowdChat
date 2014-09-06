namespace CrowdChat.Data
{
    using MongoDB.Driver;

    public class ChatDb
    {
        private MongoDatabase database;

        public ChatDb()
        {
            string connectionString = CrowdChatSettings.Default.ConnectionString;
            string dbName = CrowdChatSettings.Default.DbName;
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            this.Database = server.GetDatabase(dbName);
        }

        public MongoDatabase Database
        {
            get 
            {
                return this.database;
            }
            private set
            {
                this.database = value;
            }
        }
    }
}
