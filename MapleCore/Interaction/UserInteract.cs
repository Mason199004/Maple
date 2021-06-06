using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MapleCore.Interaction
{
    public class UserInteract
    {
        public static string PromptUser(string Message, ICollection<string> options)
        {
            Console.Write(Message + $": ({string.Join('/', options)}): ");
            return Console.ReadLine();
        }
    }
}