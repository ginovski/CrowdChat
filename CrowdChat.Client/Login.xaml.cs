namespace CrowdChat.Client
{
    using System.Windows;
    using System.Windows.Input;

    using CrowdChat.Models;

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string userName = LoginInput.Text;
            User user = new User(userName);

            this.Content = new Chat(user);
        }

        private void LoginInputKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonClick(sender, e);
            }
        }
    }
}
