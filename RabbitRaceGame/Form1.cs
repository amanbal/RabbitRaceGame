using RabbitRaceGame.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RabbitRaceGame
{
    public partial class Form1 : Form
    {

        Rabbit[] rabbits;
        Punter[] punters;
        Timer[] timers;
        Rabbit winnerRabbit;

        public Form1()
        {
            InitializeComponent();
            PrepareAllGamePart();
        }

        private void PrepareAllGamePart()
        {
            PictureBox[] pictures = { picture1, picture2, picture3, picture4 };
            rabbits = new Rabbit[pictures.Length];
            for (int index = 0; index < rabbits.Length; index++)
            {
                rabbits[index] = new Rabbit();
                rabbits[index].Name = "Rabbit " + (index + 1);
                rabbits[index].RabbitPictureBox = pictures[index];
                rabbits[index].TrackLength = 690;
            }

            TextBox[] texts = { text1, text2, text3 };
            RadioButton[] radioButtons = { radio1, radio2, radio3 };

            punters = new Punter[texts.Length];
            for (int index = 0; index < punters.Length; index++)
            {
                punters[index] = Factory.GetAPunter(index);
                punters[index].PunterRadioButton = radioButtons[index];
                punters[index].PunterRadioButton.Text = punters[index].Name;
                punters[index].PunterTextBox = texts[index];
            }

            numericRabbit.Minimum = 1;
            numericRabbit.Maximum = rabbits.Length;
            numericRabbit.Value = 1;
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            if (btnOperation.Text.Contains("Place"))
            {
                PlaceTheBet();
            }
            else if(btnOperation.Text.Contains("Start"))
            {
                StartRace();
            }
        }

        private void PlaceTheBet()
        {
            int active = 0;
            int totalbet = 0;
            for (int index = 0; index < punters.Length; index++)
            {
                if (!punters[index].Busted())
                {
                    active++;
                    if (punters[index].PunterRadionCheckStatus())
                    {
                        string message = "";
                        if (punters[index].CheckBetStatus())
                        {
                            message = string.Format(" {0} is Already Placed Bet For Race Game...", punters[index].Name);
                        }
                        else
                        {
                            int number = (int)numericRabbit.Value;
                            int amount = (int)numericBet.Value;
                            bool picked = false;
                            for (int i = 0; i < punters.Length; i++)
                            {
                                if(punters[i].CheckBetRabbit(rabbits[number-1]))
                                {
                                    picked = true;
                                    break;
                                }
                            }
                            if (picked)
                            {
                                message = string.Format("Rabbit {0} is Picked By Another Punter", number);
                            }
                            else
                            {
                                punters[index].PlaceBet(amount, rabbits[number - 1]);
                            }
                        }
                        if (message.Length != 0)
                        {
                            MessageBox.Show(message);
                        }
                    }

                    if (punters[index].PunterBet != null)
                    {
                        totalbet++;
                    }
                }
            }
            SetupPunter();
            if (totalbet == active)
            {
                btnOperation.Text = "Start Race...";
                panelBet.Enabled = false;
            }
        }

        private void StartRace()
        {
            timers = new Timer[rabbits.Length];
            for (int index = 0; index < rabbits.Length; index++)
            {
                timers[index] = new Timer();
                timers[index].Interval = 12;
                timers[index].Tick += Ticking_Event;
            }
            foreach (Timer timer in timers)
            {
                timer.Start();
            }
            btnOperation.Enabled = false;
        }

        private void Ticking_Event(object sender, EventArgs e)
        {
            Timer timer = (Timer)sender;
            int index = -1;
            for (int i = 0; i < timers.Length; i++)
            {
                if (timer == timers[i])
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                if (rabbits[index].WinTheRace())
                {
                    
                    if (winnerRabbit == null)
                    {
                        winnerRabbit = rabbits[index];
                    }
                    foreach (Timer tim in timers)
                    {
                        tim.Stop();
                    }
                }
                else
                {
                    int step = new Random().Next(1, 25);
                    rabbits[index].Move(step);
                }
            }
            
            if (winnerRabbit != null)
            {
                MessageBox.Show(string.Format("{0} is won the Race!!!", winnerRabbit.Name));
                SetupPunter();
                for ( index = 0; index < punters.Length; index++)
                {
                    if (punters[index].CheckBetStatus())
                    {
                        punters[index].CheckAndUpdateWinning(winnerRabbit);
                    }
                }

                winnerRabbit = null;
                timers = null;
                int inactive = 0;
                for ( index = 0; index < punters.Length; index++)
                {
                    if (punters[index].Busted())
                    {
                        inactive++;
                    }
                    else
                    {
                        RadioButton radio = punters[index].PunterRadioButton;
                        if (radio.Enabled && radio.Checked)
                        {
                            lblMax.Text = string.Format("{0} Max Bet Amount Limit is ${1}", punters[index].Name, punters[index].Cash);
                            btnOperation.Text = string.Format("Place A BET For Player {0}", punters[index].Name);
                            lblBet.Text = string.Format("Bet Amount of {0} is $", punters[index].Name);
                            lblFox.Text = string.Format("{0} Place Bet on Rabbit", punters[index].Name);
                            numericBet.Maximum = punters[index].Cash;
                            numericBet.Minimum = 1;
                        }
                    }
                    punters[index].ResetBet();
                }                
                if (inactive == punters.Length)
                {
                    MessageBox.Show("ALL PUNTER ARE BUSTED.... GAME OVER!!!!");
                    Application.Exit();
                }
                else
                {
                    panelBet.Enabled = true;
                    btnOperation.Enabled = true;                    
                    SetupPunter();
                }
                
                for ( index = 0; index < rabbits.Length; index++)
                {
                    rabbits[index].ResetPicture();
                }
            }
        }

        private void radio_changed(object sender, EventArgs e)
        {
            SetupPunter();
        }

        private void SetupPunter()
        {
            for (int index = 0; index < punters.Length; index++)
            {
                Punter punter = punters[index];
                string message = "";
                if (punter.Busted())
                {
                    message = "Punter BUSTED. No Cash.";
                }
                else
                {
                    if (punter.PunterBet == null)
                    {
                        message = string.Format("{0} hasn't placed a Bet", punter.Name);
                    }
                    else
                    {
                        message = string.Format("{0} placed Bet Amount ${1} on {2}", punter.Name, punter.PunterBet.Amount, punter.PunterBet.Rabbit.Name);
                    }
                    if (punter.PunterRadioButton.Checked)
                    {
                        lblMax.Text = string.Format("{0} Max Bet Amount Limit is ${1}", punter.Name, punter.Cash);
                        btnOperation.Text = string.Format("Place BET For Player {0}", punter.Name);
                        lblBet.Text = string.Format("Bet Amount of {0} is $", punter.Name);
                        lblFox.Text = string.Format("{0} Place Bet on Rabbit", punter.Name);
                        numericBet.Minimum = 1;
                        numericBet.Maximum = punter.Cash;
                        numericBet.Value = 1;
                    }
                }
                punter.PunterTextBox.Text = message;
            }
        }
    }
}
