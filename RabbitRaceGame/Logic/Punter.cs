using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitRaceGame.Logic
{
    public abstract class Punter
    {
        public int Cash { get; set; }

        public Bet PunterBet { get; set; }

        public RadioButton PunterRadioButton { get; set; }

        public string Name { get; set; }

        public TextBox PunterTextBox { get; set; }

        public bool Busted()
        {
            return Cash <= 0;
        }

        public void UpdateBetText()
        {
            PunterTextBox.Text = PunterBet.Description();
        }

        public void PlaceBet(int amount, Rabbit rabbit)
        {
            PunterBet = new Bet() { Amount = amount,Rabbit = rabbit,Bettor = this };
            UpdateBetText();
        }

        public void ResetBet()
        {
            PunterBet = null;
        }

        public bool CheckBetRabbit(Rabbit rabbit)
        {
            return PunterBet != null && PunterBet.Rabbit == rabbit;
        }

        public bool CheckBetStatus()
        {
            return PunterBet != null;
        }

        public bool PunterRadionCheckStatus()
        {
            return PunterRadioButton.Checked;
        }

        public void CheckAndUpdateWinning(Rabbit winner)
        {
            string text = "";
            int amount = PunterBet.GameAmount(winner);
            Cash += amount;
            if (PunterBet.Winner(winner))
            {
                text = string.Format("{0} won the Race and Now, has ${1} Amount For Bet", Name, Cash);
            }
            else if (Cash == 0)
            {
                text = "Punter Lost all Amount So BUSTED";
                PunterRadioButton.Enabled = false;
            }
            else
            {
                text = string.Format("{0} Lost ${1} Amount in the Race and Now, has ${1} Amount in Hand", Name, PunterBet.Amount, Cash);
            }

            PunterTextBox.Text = text;
        }
    }
    public class Luka:Punter
    {
        public Luka()
        {
            Name = "Luka";
            Cash = 50;
        }
    }
    public class Zion:Punter
    {
        public Zion()
        {
            Name = "Zion";
            Cash = 50;
        }
    }
    public class Wyatt:Punter
    {
        public Wyatt()
        {
            Name = "Wyatt";
            Cash = 50;
        }
    }
}
