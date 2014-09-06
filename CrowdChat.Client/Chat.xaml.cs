namespace CrowdChat.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using CrowdChat.Models;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using CrowdChat.Data;

    public partial class Chat : Page
    {
        private static MongoDatabase db = new ChatDb().Database;
        private IEnumerable<Message> messages;

        public Chat(User chatUser)
        {
            this.ChatUser = chatUser;
            this.messages = new List<Message>();
            InitializeComponent();
            this.ChangeGreeting();
            this.ShowMessages();
            this.UpdateMessages();
        }

        public User ChatUser { get; private set; }

        public void ChangeGreeting()
        {
            Greeting.Text = "Hello, " + ChatUser.Name + "!";
        }

        private Task<List<Message>> LoadMessages()
        {
            return Task.Run(() =>
                {
                    var messages = db.GetCollection<Message>("messages");
                    var allMessages = messages.FindAll().ToList();
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
                MessageText.IsEnabled = false;
            }
        }

        private void UpdateMessages()
        {
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            var allMessages = await LoadMessages();

            if (allMessages.Count() > this.messages.Count())
            {
                ShowMessages();
            }
        }

        private void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
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
    }
}
