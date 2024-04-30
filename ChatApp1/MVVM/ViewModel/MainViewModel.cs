using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    { 
        public ObservableCollection <UserModel> Users {get; set;} //to tracks the users joined 

        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

       

        public string Username { get; set; } //creating a property for the username 
        

       public string Message { get; set; } //property for message

        private Server _server;
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages= new ObservableCollection<String>();
            _server = new Server();
            _server.connectedEvent += Userconnected; //creating a method for connectedevent 
            _server.msgRecievedEvent += MessageReceived; //creating a method for msgRecieved
            _server.userDisconnectEvent += RemoveUser; //creating a method for userDisconnectEvent
            
            ConnectToServerCommand = new RelayCommand( o => _server.ConnectToServer(Username), o =>  !string.IsNullOrEmpty(Username)); //Creating the server and the object goes into the server , which will be received when connecting to the server.
                                                                                                                       ////that is why we are using a relaycommand to disable anyone coming in without entering his/her name


            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message)); //this activates the send button when something is written //know more about annoyoyms coding 
        }

        private void RemoveUser() // get the uid and find the user based on the id so it can be removed
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault(); //find the user based on the id 
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user)); //remove the user
        }

        private void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();//read the data that got sent 
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg)); //add it to the message collection 
        } 

        private void Userconnected()
        { 
            var user = new UserModel //once the user is connected we can to read that 
            { 
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
            
        }

    }
}

    


 