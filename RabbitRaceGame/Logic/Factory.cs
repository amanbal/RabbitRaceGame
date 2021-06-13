using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitRaceGame.Logic
{
    public static class Factory
    {
        public static Punter GetAPunter(int id)
        {
            switch (id)
            {
                case 0: return new Luka();
                case 1: return new Wyatt();
                case 2: return new Zion();
                default: return null;
            }
        }

    }
}
