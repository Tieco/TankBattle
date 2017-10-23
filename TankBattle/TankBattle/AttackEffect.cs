using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public abstract class AttackEffect
    {
        //I set this variabile protected here because I need to call it from 
        //the class that inherits from this abstract class
        protected Game game;

        public void RecordCurrentGame(Game game)
        {
            this.game = game;
        }

        public abstract void ProcessTimeEvent();
        public abstract void Render(Graphics graphics, Size displaySize);
    }
}
