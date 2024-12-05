﻿using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
   
    public class Program
    {
        public static List<User> Users = new List<User>();
        public static IPAddress IAddress;
        public static int Port;
        public static bool AutorizationUser(string login,string password)
        {
            User user = null;
            user = Users.Find(x=>x.login == login && x.password == password);
            return user != null;
        }
        public static List<string> GetDirectory(string src)
        {
            List<string> FolderFiles = new List<string>();
            if (Directory.Exists(src))
            {
                string[] dirs = Directory.GetDirectories(src);
                foreach (string dir in dirs)
                {
                    string NameDirectory = dir.Replace(src, "");
                    FolderFiles.Add(NameDirectory + "/");
                }
                string[] files = Directory.GetFiles(src);
                foreach (string file in files)
                {
                    string NameFile = file.Replace(src, "");
                    FolderFiles.Add(NameFile);
                }
            }
            return FolderFiles;
        }
        public static void StartServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress, Port);
            Socket sListener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            sListener.Bind(endPoint);
            sListener.Listen(10);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Сервер запущен.");
            while (true)
            {
                try
                {
                    Socket Handler = sListener.Accept();
                    string Data = null;
                    byte[] Bytes = new byte[10485760];
                    int BytesRec = Handler.Receive(Bytes);
                    Data += Encoding.UTF8.GetString(Bytes, 0, BytesRec);
                    Console.Write("Сообщение от пользователя: " + Data + "\n");
                    string Reply = "";
                    ViewModelSend ViewModelSend = JsonConvert.DeserializeObject<ViewModelSend>(Data);
                    if (ViewModelSend != null)
                    {
                        ViewModelMessage viewModelMessage;
                        string[] DataCommand = ViewModelSend.Message.Split(new string[1] { " " }, StringSplitOptions.None);
                        List<string> FoldersFiles = new List<string>();
                        if (dataMessage.Length == 1)
                        {
                            Users[ViewModelSend.Id].temp_src = Users[ViewModelSend.Id].src;
                            FoldersFiles = GetDirectory(Users[ViewModelSend.Id].src);
                        }
                        else
                        {
                            string cdFolder = "";
                            for (int i = 1; i < dataMessage.Length; i++)
                                if (cdFolder == "")
                                    cdFolder += dataMessage[i];
                                else cdFolder += " " + dataMessage[i];
                            Users[ViewModelSend.Id].temp_src = Users[ViewModelSend.Id].temp_src + cdFolder;
                            FoldersFiles = GetDirectory(Users[ViewModelSend.Id].temp_src);
                        }
                        if (FoldersFiles.Count == 0)
                            ViewModelMessage = new ViewModelMessage("message", "Директория пуста или не существует.");
                        else ViewModelMessage = new ViewModelMessage("cd", JsonConvert.SerializeObject(FoldersFiles));

                    }
                    else
                        viewModelMessage = new ViewModelMessage("message", "Необходимо авторизоваться");
                    Reply = JsonConvert.SerializeObject(viewModelMessage);
                    byte[] message = Encoding.UTF8.GetBytes(Reply);
                    Handler.Send(message);
                }
                else if (DataCommad[0] == "get")
                {
                    if (viewModelSend.Id != -1)
                    {
                        string[] dataMessage = viewModelSend.Message.Split(new string[1] { " " }, StringSplitOptions.None);
                        string getFile = "";
                        for (int i = 1; i < dataMessage.Length; i++)
                            if (getFile == "")
                                getFile += dataMessage[i];
                            else
                                getFile += " " + dataMessage[i];
                        byte[] byteFile = File.ReadAllBytes(Users[viewModelSend.Id].temp_src + getFile);
                        viewModelMessage = new ViewModelMessage("file", JsonConvert.SerializeObject(byteFile));
                    }
                    else
                    {
                        viewModelMessage = new ViewModelMessage("message", "Необходимо авторизоваться");
                    }
                    Reply = JsonConvert.SerializeObject(viewModelMessage);
                    byte[] message = Encoding.UTF8.GetBytes(Reply);
                    Handler.Send(message);
                }

            }
        }
               
            }
        static void Main(string[] args)
        {
        }
    }
}
