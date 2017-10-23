using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public class Map
    {
        private Random nRandom‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮ = new Random();
        public const int WIDTH = 160;
        public const int HEIGHT = 120;
        private bool[,] terrain;

        //FUCKING HARD
        //Create a map
        public Map()
        {
            terrain‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌‭⁮⁮‏‪‮ = new bool[120, 160];
            int minValue = 3;
            int num1 = 119;
            int num2 = nRandom‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(minValue, num1 + 1);
            for (int i = 0; i < 160; ++i)
            {
                for (int j = num2; j < 120; ++j)
                    terrain‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌‭⁮⁮‏‪‮[j, i] = true;
                do
                {
                    num2 += nRandom‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮‭‮‪⁪‍⁮‫‬‌‫⁬⁪⁯‌⁭⁫⁪⁪⁭‍⁫‌‭‍‬‮‎‎‭⁮‪‌‌‍⁯‮⁭⁯‬⁪‭‮.Next(-1, 2);
                }
                while (num2 < minValue || num2 > num1);
            }
        }

        //return true if there is terrain in X,Y coordinates
        public bool TerrainAt(int x, int y)
        {
            return terrain‎‫‭‌⁮‌⁬‬⁮‫⁫⁯⁯‫‪⁪‫⁫⁪‫‭‬‬‌⁮⁭⁯‌⁪⁬⁮‭⁪‌‭⁮⁮‏‪‮[y, x];
        }

        //move through the Tank area and return true if it collide with terrain
        //return false if the tank fit correctly in the map
        public bool TankCollisionAt(int tankX, int tankY)
        {
            for (int y = tankY; y < tankY + Tank.HEIGHT; y++)
            {
                for (int x = tankX; x < tankX + Tank.WIDTH; x++)
                {
                    if (TerrainAt(x, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //FUCKING HARD
        //given X coordinate return the correct Y to fit the Tank correctly in the map
        public int TankPlace(int x)
        {
            int lowestValidY = 0;
            for (int y = 0; y <= Map.HEIGHT - Tank.HEIGHT; y++)
            {
                int colTiles = 0;
                for (int iy = 0; iy < Tank.HEIGHT; iy++)
                {
                    for (int ix = 0; ix < Tank.WIDTH; ix++)
                    {
                        if (TerrainAt(x + ix, y + iy))
                        {
                            colTiles++;
                        }
                    }
                }
                if (colTiles == 0)
                {
                    lowestValidY = y;
                }
            }

            return lowestValidY;
        }

        public void TerrainDestruction(float destroyX, float destroyY, float radius)
        {
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(x - destroyX, 2) + Math.Pow(destroyY - destroyY, 2));
                    if (distance < radius)
                    {
                        terrain[y, x] = false;
                    }
                }
            }
        }
        
        //HARD
        //move the terrain following the gravity law
        public bool GravityStep()
        {
            int missingTerrainCount = 0;
            bool retValue = false;

            for (int x = 0; x < Map.WIDTH; x++) //this for because I need to check each column of 2d array
            {
                for (int y = Map.HEIGHT - 1; y > 0; y--) //cicle to move through the single column
                {
                    if (TerrainAt(x, y) == false) //this means I found empty terrain
                    {
                        for (y = y; y > 0; y--) //continue the cicle from empty terrain point
                        {
                            if (TerrainAt(x, y)) //and check if I find another terrain in that column
                            {
                                for (y = y; y > 0; y--) //this means I found another terrain so this means there is a blank space between 2 terrains //continue the cicle to shift and fix the terrain
                                {
                                    if (TerrainAt(x, y)) //each time I find a terrain I need to shit and fix it
                                    {
                                        terrain[y, x] = false; //ex position now is empty
                                        terrain[y + missingTerrainCount, x] = true; //new position is now changed from empty to terrain
                                        retValue = true;
                                    }
                                }
                            }

                            missingTerrainCount++; //count the empty terrain I found in a column. I need it to shift the terrain.
                        }
                    }
                }
            }

            return retValue;
        }
    }
}
