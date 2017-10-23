using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Explosion : AttackEffect
    {
        private int explosionDamage;
        private int explosionRadius;
        private int earthDestructionRadius;
        private float detonateX;
        private float detonateY;
        private float explosionLifeSpan;

        public Explosion(int explosionDamage, int explosionRadius, int earthDestructionRadius)
        {
            this.explosionDamage = explosionDamage;
            this.explosionRadius = explosionRadius;
            this.earthDestructionRadius = earthDestructionRadius;    
        }

        public void Detonate(float x, float y)
        {
            detonateX = x;
            detonateY = y;
            explosionLifeSpan = 1.0f;
        }

        public override void ProcessTimeEvent()
        {
            if ((explosionLifeSpan - 0.05f) <= 0)
            {
                game.DamageArmour(detonateX, detonateY, explosionDamage, explosionRadius);
                Map map = game.GetArena();
                map.TerrainDestruction(detonateX, detonateY, earthDestructionRadius);
                game.EndEffect(this);
            }
            else
            {
                explosionLifeSpan -= 0.05f;
            }
        }

        public override void Render(Graphics graphics, Size displaySize)
        {
            float x = (float)this.detonateX * displaySize.Width / Map.WIDTH;
            float y = (float)this.detonateY * displaySize.Height / Map.HEIGHT;
            float radius = displaySize.Width * (float)((1.0 - explosionLifeSpan) * explosionRadius * 3.0 / 2.0) / Map.WIDTH;

            int alpha = 0, red = 0, green = 0, blue = 0;

            if (explosionLifeSpan < 1.0 / 3.0)
            {
                red = 255;
                alpha = (int)(explosionLifeSpan * 3.0 * 255);
            }
            else if (explosionLifeSpan < 2.0 / 3.0)
            {
                red = 255;
                alpha = 255;
                green = (int)((explosionLifeSpan * 3.0 - 1.0) * 255);
            }
            else
            {
                red = 255;
                alpha = 255;
                green = 255;
                blue = (int)((explosionLifeSpan * 3.0 - 2.0) * 255);
            }

            RectangleF rect = new RectangleF(x - radius, y - radius, radius * 2, radius * 2);
            Brush b = new SolidBrush(Color.FromArgb(alpha, red, green, blue));

            graphics.FillEllipse(b, rect);
        }
    }
}
