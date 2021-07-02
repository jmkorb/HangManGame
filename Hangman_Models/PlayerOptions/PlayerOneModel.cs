using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman_Models.PlayerOptions
{
    public class PlayerOneModel
    {
        public string Player1Input { get; set; }
        public string Player1Name { get; set; }

        public void PlayerOne() { }
        public void PlayerOne(string player1Name)
        {
            Player1Name = player1Name;
        }
    }
}
