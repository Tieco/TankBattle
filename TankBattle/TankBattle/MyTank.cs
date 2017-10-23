using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    //Tank is the GENERAL class for the Tank
    //I create MyTank and inerhit from Tank because we need at least 1 type of tank 
    //(Tank class is abstract, for this reason you cant create an istance of it and you need this class)
    class MyTank : Tank
    {
        //Draw the base and the cannon of this type of tank
        //Move also the cannon of the tank in the angle where you are shooting
        public override int[,] DrawTankSprite(float angle)
        {
            //int[12,16]
            int[,] tankShape = { 
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0},
            { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

            if (angle > - 22.5 && angle < 22.5)
            {
                SetLine(tankShape, 7, 6, 7, 1); //point to up
            }
            else if (angle > -67.5 && angle < -22.5)
            {
                SetLine(tankShape, 7, 6, 3, 2); //point to middle left
            }
            else if (angle < -67.5)
            {
                SetLine(tankShape, 7, 6, 2, 6); //point to low left
            }
            else if (angle > 22.5 && angle < 67.5)
            {
                SetLine(tankShape, 7, 6, 1, 2); //point to middle right
            }
            else if (angle > 67.5)
            {
                SetLine(tankShape, 7, 6, 12, 6); //point to low right
            }

            return tankShape;
        }

        public override void FireWeapon(int weapon, PlayerTank playerTank, Game currentGame)
        {
            int x = playerTank.X();
            int y = playerTank.Y();
            float xPos = (float)x + (Tank.HEIGHT / 2);
            float yPos = (float)y + (Tank.WIDTH / 2);
            TankController player = playerTank.Player();
            Explosion explosion = new Explosion(100,4,4);
            Projectile projectile = new Projectile(xPos, yPos, playerTank.GetTankAngle(), playerTank.GetTankPower(), 0.01f, explosion, player);
            currentGame.AddWeaponEffect(projectile);
        }

        //You can change here the health of this type of tank
        public override int GetArmour()
        {
            return 100;
        }

        //You can add here other weapon for this type of tank
        public override string[] ListWeapons()
        {
            return new string[] { "Standard shell" };
        }
    }
}
