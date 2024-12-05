using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace Client
{
    public partial class MainWindow : Window
    {
        public static IPAddress IpAdress;
        private Socket _clientSocket;
        public static int Port;
        public static int Id = -1;
        public MainWindow()
        {
            InitializeComponent();
        }
        public static bool CheckCommand(string message)
        {
            bool BCommand = false;
            string[] DataMessage = message.Split(new string[1] { " " }, StringSplitOptions.None);
            if (DataMessage.Length > 0)
            {
                string Command = DataMessage[0];
                if (Command == "connect")
                {
                    if (DataMessage.Length != 3)
                    {
                        MessageBox.Show($"Использование: connect [login] [password]\nПример: connect User1 Asdfg123");
                        BCommand = false;
                    }
                    else
                    {
                        BCommand = true;
                    }
                }
                else if (Command == "cd")
                {
                    BCommand = true;
                }
                else if (Command == "get")
                {
                    if (DataMessage.Length == 1)
                    {
                        MessageBox.Show($"Использование: get [NameFile]\nПример: get Test.txt");
                        BCommand = false;
                    }
                    else BCommand = true;
                }
                else if (Command == "set")
                {
                    if (DataMessage.Length == 1)
                    {
                        MessageBox.Show("Использование: set [NameFile]\nПример: set Test.txt");
                        BCommand = false;
                    }
                    else BCommand = true;
                }
            }
            return BCommand;
        }
    }
}
