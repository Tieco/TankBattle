using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankBattle
{
    public class Game
    {
        private int numPlayers = 0;
        private int numRounds = 0;
        private int currentRound = 0;
        private int windSpeed = 0;
        private int currentPlayer = 0;
        private int startingController = 0;

        private List<AttackEffect> lAttackEffect = null;

        private PlayerTank[] vPlayerTanks = null;
        private TankController[] vTankControllers = null;

        private Map map = null;

        public Game(int numPlayers, int numRounds)
        {
            this.numPlayers = numPlayers;
            this.numRounds = numRounds;
            this.vTankControllers = new TankController[100];
            this.lAttackEffect = new List<AttackEffect>();
        }

        public int TotalPlayers()
        {
            return numPlayers;
        }

        public int CurrentRound()
        {
            return currentRound;
        }

        public int GetTotalRounds()
        {
            return numRounds;
        }

        //Add new player to the array
        //playerNum - 1 because array index start from 0
        public void SetPlayer(int playerNum, TankController player)
        {
            if (playerNum >= 1 && playerNum <= numPlayers)
            {
                vTankControllers[playerNum - 1] = player;
            }
        }

        //Return the player of the respective index
        //playerNum - 1 because array index start from 0
        public TankController Player(int playerNum)
        {
            if (playerNum >= 1 && playerNum <= numPlayers)
            {
                return vTankControllers[playerNum - 1];
            }

            return null;
        }

        //playerNum - 1 because array index start from 0
        public PlayerTank GetBattleTank(int playerNum)
        {
            return vPlayerTanks[playerNum - 1];
        }

        //assign a color at each index of the array
        public static Color PlayerColour(int playerNum)
        {
            return new Color[8]
            {
              Color.Red,
              Color.LightGreen,
              Color.Blue,
              Color.Yellow,
              Color.MediumPurple,
              Color.Gold,
              Color.Gray,
              Color.Teal
            }[playerNum - 1];
        }

        
        public static int[] CalcPlayerLocations(int numPlayers)
        {
            int[] retValue = new int[numPlayers];

            decimal distanceDecimal = 160 / numPlayers;
            int firstDistance = Convert.ToInt32(Math.Round(distanceDecimal, MidpointRounding.AwayFromZero));
            int secondDistance = Convert.ToInt32(Math.Round((distanceDecimal / 2), MidpointRounding.AwayFromZero));

            for (int i = 0; i < numPlayers; i++)
            {
                retValue[i] = secondDistance;
                secondDistance += firstDistance;
            }

            return retValue;
        }

        //I randomize the number in the array with a linq function using a temporary array
        //Then i put the new array values in the one that I need to randomize
        //You cannot shuffle directly the right array, or change all the values assigning it directly the other array
        //because it doesnt work, you have to do it 1 value at time as you see in the for cicle
        public static void RandomReorder(int[] array)
        {
            int[] appArray = new int[array.Length];

            Random random = new Random();
            appArray = array.OrderBy(x => random.Next()).ToArray();

            for(int i = 0; i < array.Length; i++)
            {
                array[i] = appArray[i];
            }

        }

        public void BeginGame()
        {
            currentRound = 1;
            startingController = 0;
            StartRound();
        }

        public void StartRound()
        {
            currentPlayer = startingController;
            map = new Map();
            vPlayerTanks = new PlayerTank[100];
            int[] tankControllerPositions = CalcPlayerLocations(vTankControllers.Count());
            for (int i = 0; i < vTankControllers.Count(); i++)
            {
                if (vTankControllers[i] != null)
                {
                    vTankControllers[i].NewRound();

                    RandomReorder(tankControllerPositions);
                    int horizontalTankPos = tankControllerPositions[i];
                    int verticalTankPos = map.TankPlace(horizontalTankPos);
                    PlayerTank player = new PlayerTank(vTankControllers[i], horizontalTankPos, verticalTankPos, this);
                    vPlayerTanks[i] = player;
                }
                else
                {
                    //when i meet the first null i break the cicle cause that means 
                    //there are not other tankController in the array
                    break; 
                }
            }

            Random nRandom = new Random();
            windSpeed = nRandom.Next(-100, 100);

            GameForm gf = new GameForm(this);
            gf.Show();
        }

        public Map GetArena()
        {
            return map;
        }

        public void DrawPlayers(Graphics graphics, Size displaySize)
        {
            foreach (PlayerTank pt in vPlayerTanks)
            {
                if (pt != null)
                {
                    if (pt.IsAlive())
                    {
                        pt.Render(graphics, displaySize);
                    }
                }
            }
        }

        public PlayerTank CurrentPlayerTank()
        {
            return vPlayerTanks[currentPlayer];
        }

        public void AddWeaponEffect(AttackEffect weaponEffect)
        {
            weaponEffect.RecordCurrentGame(this);
            lAttackEffect.Add(weaponEffect);
        }

        public bool ProcessWeaponEffects()
        {
            foreach (AttackEffect attackEffect in lAttackEffect)
            {
                attackEffect.ProcessTimeEvent();
            }

            return true;
        }

        public void RenderEffects(Graphics graphics, Size displaySize)
        {
            foreach (AttackEffect attackEffect in lAttackEffect)
            {
                attackEffect.Render(graphics, displaySize);
            }
        }

        public void EndEffect(AttackEffect weaponEffect)
        {
            lAttackEffect.Remove(weaponEffect);
        }

        public bool DetectCollision(float projectileX, float projectileY)
        {
            //projectile out of map
            if (projectileX < 0 || projectileY < 0 || projectileX > Map.HEIGHT || projectileY > Map.WIDTH)
            {
                return false;
            }

            //projectile hit terrain
            if (map.TerrainAt((int)projectileX, (int)projectileY))
            {
                return true;
            }

            //projectile hit a tank
            foreach (PlayerTank player in vPlayerTanks)
            {
                if (player != null)
                {
                    if ((projectileX <= (player.X() + Tank.WIDTH / 2) && projectileX >= (player.X() - Tank.WIDTH / 2)) &&
                        (projectileY <= (player.Y() + Tank.HEIGHT / 2) && projectileY >= (player.Y() - Tank.HEIGHT / 2)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DamageArmour(float damageX, float damageY, float explosionDamage, float radius)
        {
            foreach (PlayerTank player in vPlayerTanks)
            {
                if (player != null)
                {
                    if (player.IsAlive())
                    {
                        float tankPosX = player.X() + Tank.WIDTH / 2;
                        float tankPosY = player.Y() + Tank.HEIGHT / 2;
                        float distance = (float)Math.Sqrt(Math.Pow(tankPosX - damageX, 2) + Math.Pow(damageY - damageY, 2));
                        float damageDone = 0;
                        if (distance < radius && distance > radius / 2)
                        {
                            damageDone = (explosionDamage * (radius - distance)) / radius;
                        }
                        else if (distance < radius / 2)
                        {
                            //this is not clear in the specifics "explosionRadius is done" in this case.
                            //I've setted the same damage above, not so relevant
                            damageDone = (explosionDamage * (radius - distance)) / radius;
                        }

                        player.DamageArmour((int)damageDone);
                    }
                }
            }
        }

        public bool GravityStep()
        {
            if (map.GravityStep())
            {
                return true;
            }

            foreach (PlayerTank player in vPlayerTanks)
            {
                if (player != null)
                {
                    if (player.GravityStep())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool FinaliseTurn()
        {
            Random nRandom = new Random();
            int playersAliveCount = 0;
            foreach (PlayerTank pt in vPlayerTanks)
            {
                if (pt.IsAlive())
                {
                    playersAliveCount++;
                }
            }

            if (playersAliveCount >= 2)
            {
                for (int i = 0; i < vPlayerTanks.Count(); i++)
                {
                    if ((currentPlayer + 1) > vPlayerTanks.Count())
                    {
                        currentPlayer = 0;
                    }
                    else
                    {
                        currentPlayer++;
                    }

                    if (vPlayerTanks[currentPlayer].IsAlive())
                    {
                        windSpeed += nRandom.Next(-10, 10);
                        if (windSpeed < -100) windSpeed = -100;
                        if (windSpeed > 100) windSpeed = 100;

                        return true;
                    }
                }
            }

            FindWinner();
            return false;
        }

        public void FindWinner()
        {
            foreach (PlayerTank pt in vPlayerTanks)
            {
                if (pt.IsAlive())
                {
                    pt.Player().Winner();
                }
            }
        }

        public void NextRound()
        {
            if ((currentRound + 1) <= numRounds)
            {
                currentRound++;

                if ((currentPlayer + 1) > numPlayers)
                {
                    currentPlayer = 0;
                }
                else
                {
                    currentPlayer++;
                }

                StartRound();
            }
            else
            {
                TitlescreenForm form = new TitlescreenForm();
                form.Show();
            }

        }
        
        public int Wind()
        {
            return windSpeed;
        }
    }
}
