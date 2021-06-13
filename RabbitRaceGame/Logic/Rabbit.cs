using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RabbitRaceGame.Logic
{
    public class Rabbit
    {
        public string Name { set; get; }

        public PictureBox RabbitPictureBox { set; get; }

        public int TrackLength { set; get; }

        public void Move(int step)
        {
            RabbitPictureBox.Location = new Point(RabbitPictureBox.Location.X + step, RabbitPictureBox.Location.Y);
        }
        public bool WinTheRace()
        {
            return RabbitPictureBox.Location.X > TrackLength;
        }

        public void ResetPicture()
        {
            RabbitPictureBox.Location = new Point(2, RabbitPictureBox.Location.Y);
        }
    }
}
