using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    abstract public class TankController
    {
        private string name;
        private Color colour;
        private Tank tank;
        private int roundsWon;

        public TankController(string name, Tank tank, Color colour)
        {
            this.colour = colour;
            this.tank = tank;
            this.name = name;
            roundsWon = 0;
        }
        public Tank CreateTank()
        {
            return tank;
        }
        public string Identifier()
        {
            return name;
        }
        public Color PlayerColour()
        {
            return colour;
        }
        public void Winner()
        {
            roundsWon++;
        }
        public int GetScore()
        {
            return roundsWon;
        }

        public abstract void NewRound();

        public abstract void StartTurn(GameForm gameplayForm, Game currentGame);

        public abstract void ProjectileHit(float x, float y);
    }
}
