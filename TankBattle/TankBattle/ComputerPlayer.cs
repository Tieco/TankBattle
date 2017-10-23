using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class ComputerPlayer : TankController
    {
        private Tank tank;
        private string name;
        private Color colour;

        public ComputerPlayer(string name, Tank tank, Color colour) : base(name, tank, colour)
        {
            this.tank = tank;
            this.name = name;
            this.colour = colour;
        }

        //No specifics for this method
        public override void NewRound()
        {
            
        }

        public override void StartTurn(GameForm gameplayForm, Game currentGame)
        {
            gameplayForm.EnableControlPanel();
        }

        //No specifics for this method
        public override void ProjectileHit(float x, float y)
        {
            
        }
    }
}
