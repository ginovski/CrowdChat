using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CrowdChat.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
namespace CrowdChat.Client
{
    public partial class Chat : Page
    {
        private IEnumerable<Message> messages;

        public Chat(User chatUser)
        {
            this.ChatUser = chatUser;
            this.messages = new List<Message>();
            InitializeComponent();
            this.ChangeGreeting();
            this.ShowMessages();
        }
        public User ChatUser { get; private set; }

        public void ChangeGreeting()
        {
            Greeting.Text = "Hello, " + ChatUser.Name + "!";
        }

        private Task<MongoCursor<Message>> LoadMessages()
        {
            return Task.Run(() =>
                {
                    var db = this.GetDatabase("mongodb://admin:sshk23@ds033740.mongolab.com:33740/crowdchat", "crowdchat");
                    var messages = db.GetCollection<Message>("messages");
                    var allMessages = messages.FindAll();
                    return allMessages;
                });
        }

        public async void ShowMessages()
        {
            try
            {
                Loading.Text = "Loading...";
                var allMessages = await LoadMessages();
                Loading.Text = "";
                MessageBox.ItemsSource = allMessages;
                if (MessageBox.Items.Count > 0)
                {
                    MessageBox.ScrollIntoView(MessageBox.Items[MessageBox.Items.Count - 1]);
                }
                this.messages = allMessages;
            }
            catch (MongoConnectionException)
            {
                Loading.Text = "No Internet Connection.";
            }
        }

        private MongoDatabase GetDatabase(string connection, string database)
        {
            var url = new MongoUrl(connection);
            var client = new MongoClient(url);
            var server = client.GetServer();
            var db = server.GetDatabase(database);

            return db;
        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            var db = this.GetDatabase("mongodb://admin:sshk23@ds033740.mongolab.com:33740/crowdchat", "crowdchat");
            var messages = db.GetCollection<Message>("messages");

            var message = new Message(MessageText.Text, this.ChatUser);

            messages.Insert(message);
            MessageText.Clear();
            ShowMessages();
        }

        private void MessageSendOnEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.SendMessageButtonClick(sender, e);
            }
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            ShowMessages();
        }
    }
}
