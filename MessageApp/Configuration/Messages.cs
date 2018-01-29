using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace MessageApp.Configuration
{
    public class Messages
    {
        public string WelcomeMessage { get; set; }
        public string BodyMessage { get; set; }
        public string ByeMessage { get; set; }
        public Messages()
        {
        }

        public Messages(string welcomeMessage, string bodyMessage, string byeMessage)
        {
            WelcomeMessage = welcomeMessage;
            BodyMessage = bodyMessage;
            ByeMessage = byeMessage;
        }

        #region Static Stuff

        public static Messages Instance { get; set; }

        //private static System.IO.FileSystemWatcher monitor;

        static Messages()
        {
            Instance = new Messages();
        }

        static PhysicalFileProvider _fileProvider ;
        static IChangeToken token;
        public static void Initialize(string configFilePath){
            var fileName = "messageconfig.json";
            var fullPath = System.IO.Path.Combine(configFilePath, fileName);
            _fileProvider = new PhysicalFileProvider(configFilePath);
            loadValues(fullPath);
        }

        static void loadValues(string path){
            
            var loadedValues = JsonConvert.DeserializeObject<Messages>(System.IO.File.ReadAllText(path));
            Instance = loadedValues;
            token = _fileProvider.Watch("messageconfig.json");

            token.RegisterChangeCallback(changeCallBack, path);
        }

        static void changeCallBack(object state){
            loadValues(state.ToString());
        }

        #endregion
    }
}
