using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class Projectile : AttackEffect
    {
        private float x;
        private float y;
        private float xVelocity;
        private float yVelocity;
        private float angle;
        private float power;
        private float gravity;
        private Explosion explosion;
        private TankController player;

        public Projectile(float x, float y, float angle, float power, float gravity, Explosion explosion, TankController player)
        {
            this.angle = angle;
            this.power = power;
            this.gravity = gravity;
            this.explosion = explosion;
            this.player = player;
            this.x = x;
            this.y = y;

            float angleRadiant = (90 - angle) * (float)Math.PI / 180;
            float magnitude = power / 50;

            this.xVelocity = (float)Math.Cos(angleRadiant) * magnitude;
            this.yVelocity = (float)Math.Sin(angleRadiant) * magnitude;
        }

        //calculate the path of the projectile and eventually the hit and explosion
        public override void ProcessTimeEvent()
        {
            for (int i = 0; i < 10; i++)
            {
                x += xVelocity;
                y += yVelocity;
                x += game.Wind() / 1000.0f;

                if (x > Map.HEIGHT || y > Map.WIDTH)
                {
                    game.EndEffect(this);
                }
                else if (game.DetectCollision(x, y))
                {
                    player.ProjectileHit(x, y);
                    explosion.Detonate(x, y);
                    game.AddWeaponEffect(explosion);
                    game.EndEffect(this);
                    return;
                }

                yVelocity += gravity;
            }
        }

        public override void Render(Graphics graphics, Size size)
        {
            float x = (float)this.x * size.Width / Map.WIDTH;
            float y = (float)this.y * size.Height / Map.HEIGHT;
            float s = size.Width / Map.WIDTH;

            RectangleF r = new RectangleF(x - s / 2.0f, y - s / 2.0f, s, s);
            Brush b = new SolidBrush(Color.WhiteSmoke);

            graphics.FillEllipse(b, r);
        }
    }
}
