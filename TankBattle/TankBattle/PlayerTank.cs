using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class PlayerTank
    {
        private Tank tank = null;
        private TankController tankController = null;
        private Game game;
        private Bitmap tankBmp;
        private int tankX;
        private int tankY;
        private int health;
        private float angle;
        private int power;
        private int weapon;

        public PlayerTank(TankController player, int tankX, int tankY, Game game)
        {
            this.tankX = tankX;
            this.tankY = tankY;
            this.game = game;
            this.tankController = player;

            angle = 0;
            power = 25;
            weapon = 0;

            tank = tankController.CreateTank();
            health = tank.GetArmour();
            tankBmp = tank.CreateTankBitmap(tankController.PlayerColour(), angle);
        }

        public TankController Player()
        {
            return tankController;
        }
        public Tank CreateTank()
        {
            return tankController.CreateTank();
        }

        public float GetTankAngle()
        {
            return angle;
        }

        public void SetAimingAngle(float angle)
        {
            this.angle = angle;
        }

        public int GetTankPower()
        {
            return power;
        }

        public void SetPower(int power)
        {
            this.power = power;
        }

        public int GetCurrentWeapon()
        {
            return weapon;
        }
        public void ChangeWeapon(int newWeapon)
        {
            this.weapon = newWeapon;
        }

        public void Render(Graphics graphics, Size displaySize)
        {
            int drawX1 = displaySize.Width * tankX / Map.WIDTH;
            int drawY1 = displaySize.Height * tankY / Map.HEIGHT;
            int drawX2 = displaySize.Width * (tankX + Tank.WIDTH) / Map.WIDTH;
            int drawY2 = displaySize.Height * (tankY + Tank.HEIGHT) / Map.HEIGHT;

            graphics.DrawImage(tankBmp, new Rectangle(drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1));

            int drawY3 = displaySize.Height * (tankY - Tank.HEIGHT) / Map.HEIGHT;

            Font font = new Font("Arial", 8);
            Brush brush = new SolidBrush(Color.White);

            int pet = health * 100 / tank.GetArmour();
            if (pet < 100)
            {
                graphics.DrawString(pet + "%", font, brush, new Point(drawX1, drawY3));
            }
        }

        public int X()
        {
            return tankX;
        }
        public int Y()
        {
            return tankY;
        }

        public void Launch()
        {
            Tank tank = CreateTank();
            tank.FireWeapon(weapon, this, game);
        }

        public void DamageArmour(int damageAmount)
        {
            health -= damageAmount;
        }

        public bool IsAlive()
        {
            if (health > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GravityStep()
        {
            Map map = game.GetArena();

            if (IsAlive() == false)
            {
                return false;
            }

            if (map.TankCollisionAt(tankX, tankY + 1))
            {
                return false;
            }

            tankY++;
            health--;

            if (tankY == Map.HEIGHT - Tank.HEIGHT)
            {
                health = 0;
            }

            return true;
        }
    }
}
