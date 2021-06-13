using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitRaceGame.Logic
{
    public class Bet
    {
        public int Amount { get; set; }
        
        public Rabbit Rabbit { get; set; }
        
        public Punter Bettor { get; set; }

        public string Description()
        {
            string description = Bettor.Name + " bet $" + Amount + " on " + Rabbit.Name;
            return description;
        }

        public int GameAmount(Rabbit winner)
        {
            if(winner != Rabbit)
            {
                return -1 * Amount;
            }
            return Amount;
        }

        public bool Winner(Rabbit winner)
        {
            return winner == Rabbit;
        }
    }
}
