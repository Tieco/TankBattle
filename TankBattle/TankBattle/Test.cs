using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TankBattle;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TankBattleTestSuite
{
    class RequirementException : Exception
    {
        public RequirementException()
        {
        }

        public RequirementException(string message) : base(message)
        {
        }

        public RequirementException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    class Test
    {
        #region Testing Code

        private delegate bool TestCase();

        private static string ErrorDescription = null;

        private static void SetErrorDescription(string desc)
        {
            ErrorDescription = desc;
        }

        private static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01) return true;
            return false;
        }

        private static Dictionary<string, string> unitTestResults = new Dictionary<string, string>();

        private static void Passed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[passed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                throw new Exception("ErrorDescription found for passing test case");
            }
            Console.WriteLine();
        }
        private static void Failed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[failed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                Console.Write("\n{0}", ErrorDescription);
                ErrorDescription = null;
            }
            Console.WriteLine();
        }
        private static void FailedToMeetRequirement(string name, string comment)
        {
            Console.Write("[      ] ");
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("{0}", comment);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        private static void DoTest(TestCase test)
        {
            // Have we already completed this test?
            if (unitTestResults.ContainsKey(test.Method.ToString()))
            {
                return;
            }

            bool passed = false;
            bool metRequirement = true;
            string exception = "";
            try
            {
                passed = test();
            }
            catch (RequirementException e)
            {
                metRequirement = false;
                exception = e.Message;
            }
            catch (Exception e)
            {
                exception = e.GetType().ToString();
            }

            string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
            string fnName = test.Method.ToString().Split('0')[1];

            if (metRequirement)
            {
                if (passed)
                {
                    unitTestResults[test.Method.ToString()] = "Passed";
                    Passed(string.Format("{0}.{1}", className, fnName), exception);
                }
                else
                {
                    unitTestResults[test.Method.ToString()] = "Failed";
                    Failed(string.Format("{0}.{1}", className, fnName), exception);
                }
            }
            else
            {
                unitTestResults[test.Method.ToString()] = "Failed";
                FailedToMeetRequirement(string.Format("{0}.{1}", className, fnName), exception);
            }
            Cleanup();
        }

        private static Stack<string> errorDescriptionStack = new Stack<string>();


        private static void Requires(TestCase test)
        {
            string result;
            bool wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

            if (!wasTested)
            {
                // Push the error description onto the stack (only thing that can change, not that it should)
                errorDescriptionStack.Push(ErrorDescription);

                // Do the test
                DoTest(test);

                // Pop the description off
                ErrorDescription = errorDescriptionStack.Pop();

                // Get the proper result for out
                wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

                if (!wasTested)
                {
                    throw new Exception("This should never happen");
                }
            }

            if (result == "Failed")
            {
                string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
                string fnName = test.Method.ToString().Split('0')[1];

                throw new RequirementException(string.Format("-> {0}.{1}", className, fnName));
            }
            else if (result == "Passed")
            {
                return;
            }
            else
            {
                throw new Exception("This should never happen");
            }

        }

        #endregion

        #region Test Cases
        private static Game InitialiseGame()
        {
            Requires(TestGame0Game);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);
            Requires(TestGame0SetPlayer);

            Game game = new Game(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player1 = new HumanOpponent("player1", tank, Color.Orange);
            TankController player2 = new HumanOpponent("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);
            return game;
        }
        private static void Cleanup()
        {
            while (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Dispose();
            }
        }
        private static bool TestGame0Game()
        {
            Game game = new Game(2, 1);
            return true;
        }
        private static bool TestGame0TotalPlayers()
        {
            Requires(TestGame0Game);

            Game game = new Game(2, 1);
            return game.TotalPlayers() == 2;
        }
        private static bool TestGame0GetTotalRounds()
        {
            Requires(TestGame0Game);

            Game game = new Game(3, 5);
            return game.GetTotalRounds() == 5;
        }
        private static bool TestGame0SetPlayer()
        {
            Requires(TestGame0Game);
            Requires(TestTank0CreateTank);

            Game game = new Game(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player = new HumanOpponent("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return true;
        }
        private static bool TestGame0Player()
        {
            Requires(TestGame0Game);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);

            Game game = new Game(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player = new HumanOpponent("playerName", tank, Color.Orange);
            game.SetPlayer(1, player);
            return game.Player(1) == player;
        }
        private static bool TestGame0PlayerColour()
        {
            Color[] arrayOfColours = new Color[8];
            for (int i = 0; i < 8; i++)
            {
                arrayOfColours[i] = Game.PlayerColour(i + 1);
                for (int j = 0; j < i; j++)
                {
                    if (arrayOfColours[j] == arrayOfColours[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGame0CalcPlayerLocations()
        {
            int[] positions = Game.CalcPlayerLocations(8);
            for (int i = 0; i < 8; i++)
            {
                if (positions[i] < 0) return false;
                if (positions[i] > 160) return false;
                for (int j = 0; j < i; j++)
                {
                    if (positions[j] == positions[i]) return false;
                }
            }
            return true;
        }
        private static bool TestGame0RandomReorder()
        {
            int[] ar = new int[100];
            for (int i = 0; i < 100; i++)
            {
                ar[i] = i;
            }
            Game.RandomReorder(ar);
            for (int i = 0; i < 100; i++)
            {
                if (ar[i] != i)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGame0BeginGame()
        {
            Game game = InitialiseGame();
            game.BeginGame();

            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestGame0GetArena()
        {
            Requires(TestMap0Map);
            Game game = InitialiseGame();
            game.BeginGame();
            Map battlefield = game.GetArena();
            if (battlefield != null) return true;

            return false;
        }
        private static bool TestGame0CurrentPlayerTank()
        {
            Requires(TestGame0Game);
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);
            Requires(TestGame0SetPlayer);
            Requires(TestPlayerTank0Player);

            Game game = new Game(2, 1);
            Tank tank = Tank.CreateTank(1);
            TankController player1 = new HumanOpponent("player1", tank, Color.Orange);
            TankController player2 = new HumanOpponent("player2", tank, Color.Purple);
            game.SetPlayer(1, player1);
            game.SetPlayer(2, player2);

            game.BeginGame();
            PlayerTank ptank = game.CurrentPlayerTank();
            if (ptank.Player() != player1 && ptank.Player() != player2)
            {
                return false;
            }
            if (ptank.CreateTank() != tank)
            {
                return false;
            }

            return true;
        }

        private static bool TestTank0CreateTank()
        {
            Tank tank = Tank.CreateTank(1);
            if (tank != null) return true;
            else return false;
        }
        private static bool TestTank0DrawTankSprite()
        {
            Requires(TestTank0CreateTank);
            Tank tank = Tank.CreateTank(1);

            int[,] tankGraphic = tank.DrawTankSprite(45);
            if (tankGraphic.GetLength(0) != 12) return false;
            if (tankGraphic.GetLength(1) != 16) return false;
            // We don't really care what the tank looks like, but the 45 degree tank
            // should at least look different to the -45 degree tank
            int[,] tankGraphic2 = tank.DrawTankSprite(-45);
            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (tankGraphic2[y, x] != tankGraphic[y, x])
                    {
                        return true;
                    }
                }
            }

            SetErrorDescription("Tank with turret at -45 degrees looks the same as tank with turret at 45 degrees");

            return false;
        }
        private static void DisplayLine(int[,] array)
        {
            string report = "";
            report += "A line drawn from 3,0 to 0,3 on a 4x4 array should look like this:\n";
            report += "0001\n";
            report += "0010\n";
            report += "0100\n";
            report += "1000\n";
            report += "The one produced by Tank.SetLine() looks like this:\n";
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    report += array[y, x] == 1 ? "1" : "0";
                }
                report += "\n";
            }
            SetErrorDescription(report);
        }
        private static bool TestTank0SetLine()
        {
            int[,] ar = new int[,] { { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 } };
            Tank.SetLine(ar, 3, 0, 0, 3);

            // Ideally, the line we want to see here is:
            // 0001
            // 0010
            // 0100
            // 1000

            // However, as we aren't that picky, as long as they have a 1 in every row and column
            // and nothing in the top-left and bottom-right corners

            int[] rows = new int[4];
            int[] cols = new int[4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ar[y, x] == 1)
                    {
                        rows[y] = 1;
                        cols[x] = 1;
                    }
                    else if (ar[y, x] > 1 || ar[y, x] < 0)
                    {
                        // Only values 0 and 1 are permitted
                        SetErrorDescription(string.Format("Somehow the number {0} got into the array.", ar[y, x]));
                        return false;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (rows[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
                if (cols[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
            }
            if (ar[0, 0] == 1)
            {
                DisplayLine(ar);
                return false;
            }
            if (ar[3, 3] == 1)
            {
                DisplayLine(ar);
                return false;
            }

            return true;
        }
        private static bool TestTank0GetArmour()
        {
            Requires(TestTank0CreateTank);
            // As long as it's > 0 we're happy
            Tank tank = Tank.CreateTank(1);
            if (tank.GetArmour() > 0) return true;
            return false;
        }
        private static bool TestTank0ListWeapons()
        {
            Requires(TestTank0CreateTank);
            // As long as there's at least one result and it's not null / a blank string, we're happy
            Tank tank = Tank.CreateTank(1);
            if (tank.ListWeapons().Length == 0) return false;
            if (tank.ListWeapons()[0] == null) return false;
            if (tank.ListWeapons()[0] == "") return false;
            return true;
        }

        private static TankController CreateTestingPlayer()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);

            Tank tank = Tank.CreateTank(1);
            TankController player = new HumanOpponent("player1", tank, Color.Aquamarine);
            return player;
        }

        private static bool TestTankController0HumanOpponent()
        {
            Requires(TestTank0CreateTank);

            Tank tank = Tank.CreateTank(1);
            TankController player = new HumanOpponent("player1", tank, Color.Aquamarine);
            if (player != null) return true;
            return false;
        }
        private static bool TestTankController0CreateTank()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);

            Tank tank = Tank.CreateTank(1);
            TankController p = new HumanOpponent("player1", tank, Color.Aquamarine);
            if (p.CreateTank() == tank) return true;
            return false;
        }
        private static bool TestTankController0Identifier()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);

            const string PLAYER_NAME = "kfdsahskfdajh";
            Tank tank = Tank.CreateTank(1);
            TankController p = new HumanOpponent(PLAYER_NAME, tank, Color.Aquamarine);
            if (p.Identifier() == PLAYER_NAME) return true;
            return false;
        }
        private static bool TestTankController0PlayerColour()
        {
            Requires(TestTank0CreateTank);
            Requires(TestTankController0HumanOpponent);

            Color playerColour = Color.Chartreuse;
            Tank tank = Tank.CreateTank(1);
            TankController p = new HumanOpponent("player1", tank, playerColour);
            if (p.PlayerColour() == playerColour) return true;
            return false;
        }
        private static bool TestTankController0Winner()
        {
            TankController p = CreateTestingPlayer();
            p.Winner();
            return true;
        }
        private static bool TestTankController0GetScore()
        {
            Requires(TestTankController0Winner);

            TankController p = CreateTestingPlayer();
            int wins = p.GetScore();
            p.Winner();
            if (p.GetScore() == wins + 1) return true;
            return false;
        }
        private static bool TestHumanOpponent0NewRound()
        {
            TankController p = CreateTestingPlayer();
            p.NewRound();
            return true;
        }
        private static bool TestHumanOpponent0StartTurn()
        {
            Requires(TestGame0BeginGame);
            Requires(TestGame0Player);
            Game game = InitialiseGame();

            game.BeginGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Game.BeginGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that NewTurn enables it
            controlPanel.Enabled = false;

            game.Player(1).StartTurn(gameplayForm, game);

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after HumanPlayer.NewTurn()");
                return false;
            }
            return true;

        }
        private static bool TestHumanOpponent0ProjectileHit()
        {
            TankController p = CreateTestingPlayer();
            p.ProjectileHit(0, 0);
            return true;
        }

        private static bool TestPlayerTank0PlayerTank()
        {
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            return true;
        }
        private static bool TestPlayerTank0Player()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (playerTank.Player() == p) return true;
            return false;
        }
        private static bool TestPlayerTank0CreateTank()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestTankController0CreateTank);
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (playerTank.CreateTank() == playerTank.Player().CreateTank()) return true;
            return false;
        }
        private static bool TestPlayerTank0GetTankAngle()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            float angle = playerTank.GetTankAngle();
            if (angle >= -90 && angle <= 90) return true;
            return false;
        }
        private static bool TestPlayerTank0SetAimingAngle()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetTankAngle);
            float angle = 75;
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.SetAimingAngle(angle);
            if (FloatEquals(playerTank.GetTankAngle(), angle)) return true;
            return false;
        }
        private static bool TestPlayerTank0GetTankPower()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);

            playerTank.GetTankPower();
            return true;
        }
        private static bool TestPlayerTank0SetPower()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetTankPower);
            int power = 65;
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.SetPower(power);
            if (playerTank.GetTankPower() == power) return true;
            return false;
        }
        private static bool TestPlayerTank0GetCurrentWeapon()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);

            playerTank.GetCurrentWeapon();
            return true;
        }
        private static bool TestPlayerTank0ChangeWeapon()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0GetCurrentWeapon);
            int weapon = 3;
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.ChangeWeapon(weapon);
            if (playerTank.GetCurrentWeapon() == weapon) return true;
            return false;
        }
        private static bool TestPlayerTank0Render()
        {
            Requires(TestPlayerTank0PlayerTank);
            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.Render(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestPlayerTank0X()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, x, y, game);
            if (playerTank.X() == x) return true;
            return false;
        }
        private static bool TestPlayerTank0Y()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, x, y, game);
            if (playerTank.Y() == y) return true;
            return false;
        }
        private static bool TestPlayerTank0Launch()
        {
            Requires(TestPlayerTank0PlayerTank);

            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.Launch();
            return true;
        }
        private static bool TestPlayerTank0DamageArmour()
        {
            Requires(TestPlayerTank0PlayerTank);
            TankController p = CreateTestingPlayer();

            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            playerTank.DamageArmour(10);
            return true;
        }
        private static bool TestPlayerTank0IsAlive()
        {
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0DamageArmour);

            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            PlayerTank playerTank = new PlayerTank(p, 32, 32, game);
            if (!playerTank.IsAlive()) return false;
            playerTank.DamageArmour(playerTank.CreateTank().GetArmour());
            if (playerTank.IsAlive()) return false;
            return true;
        }
        private static bool TestPlayerTank0GravityStep()
        {
            Requires(TestGame0GetArena);
            Requires(TestMap0TerrainDestruction);
            Requires(TestPlayerTank0PlayerTank);
            Requires(TestPlayerTank0DamageArmour);
            Requires(TestPlayerTank0IsAlive);
            Requires(TestPlayerTank0CreateTank);
            Requires(TestTank0GetArmour);

            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.BeginGame();
            // Unfortunately we need to rely on DestroyTerrain() to get rid of any terrain that may be in the way
            game.GetArena().TerrainDestruction(Map.WIDTH / 2.0f, Map.HEIGHT / 2.0f, 20);
            PlayerTank playerTank = new PlayerTank(p, Map.WIDTH / 2, Map.HEIGHT / 2, game);
            int oldX = playerTank.X();
            int oldY = playerTank.Y();

            playerTank.GravityStep();

            if (playerTank.X() != oldX)
            {
                SetErrorDescription("Caused X coordinate to change.");
                return false;
            }
            if (playerTank.Y() != oldY + 1)
            {
                SetErrorDescription("Did not cause Y coordinate to increase by 1.");
                return false;
            }

            int initialArmour = playerTank.CreateTank().GetArmour();
            // The tank should have lost 1 armour from falling 1 tile already, so do
            // (initialArmour - 2) damage to the tank then drop it again. That should kill it.

            if (!playerTank.IsAlive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.DamageArmour(initialArmour - 2);
            if (!playerTank.IsAlive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.GravityStep();
            if (playerTank.IsAlive())
            {
                SetErrorDescription("Tank survived despite taking enough falling damage to destroy it");
                return false;
            }

            return true;
        }
        private static bool TestMap0Map()
        {
            Map battlefield = new Map();
            return true;
        }
        private static bool TestMap0TerrainAt()
        {
            Requires(TestMap0Map);

            bool foundTrue = false;
            bool foundFalse = false;
            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        foundTrue = true;
                    }
                    else
                    {
                        foundFalse = true;
                    }
                }
            }

            if (!foundTrue)
            {
                SetErrorDescription("IsTileAt() did not return true for any tile.");
                return false;
            }

            if (!foundFalse)
            {
                SetErrorDescription("IsTileAt() did not return false for any tile.");
                return false;
            }

            return true;
        }
        private static bool TestMap0TankCollisionAt()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int y = 0; y <= Map.HEIGHT - Tank.HEIGHT; y++)
            {
                for (int x = 0; x <= Map.WIDTH - Tank.WIDTH; x++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Tank.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Tank.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        if (battlefield.TankCollisionAt(x, y))
                        {
                            SetErrorDescription("Found collision where there shouldn't be one");
                            return false;
                        }
                    }
                    else
                    {
                        if (!battlefield.TankCollisionAt(x, y))
                        {
                            SetErrorDescription("Didn't find collision where there should be one");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private static bool TestMap0TankPlace()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int x = 0; x <= Map.WIDTH - Tank.WIDTH; x++)
            {
                int lowestValid = 0;
                for (int y = 0; y <= Map.HEIGHT - Tank.HEIGHT; y++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Tank.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Tank.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        lowestValid = y;
                    }
                }

                int placedY = battlefield.TankPlace(x);
                if (placedY != lowestValid)
                {
                    SetErrorDescription(string.Format("Tank was placed at {0},{1} when it should have been placed at {0},{2}", x, placedY, lowestValid));
                    return false;
                }
            }
            return true;
        }
        private static bool TestMap0TerrainDestruction()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        battlefield.TerrainDestruction(x, y, 0.5f);
                        if (battlefield.TerrainAt(x, y))
                        {
                            SetErrorDescription("Attempted to destroy terrain but it still exists");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            SetErrorDescription("Did not find any terrain to destroy");
            return false;
        }
        private static bool TestMap0GravityStep()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);
            Requires(TestMap0TerrainDestruction);

            Map battlefield = new Map();
            for (int x = 0; x < Map.WIDTH; x++)
            {
                if (battlefield.TerrainAt(x, Map.HEIGHT - 1))
                {
                    if (battlefield.TerrainAt(x, Map.HEIGHT - 2))
                    {
                        // Seek up and find the first non-set tile
                        for (int y = Map.HEIGHT - 2; y >= 0; y--)
                        {
                            if (!battlefield.TerrainAt(x, y))
                            {
                                // Do a gravity step and make sure it doesn't slip down
                                battlefield.GravityStep();
                                if (!battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Moved down terrain even though there was no room");
                                    return false;
                                }

                                // Destroy the bottom-most tile
                                battlefield.TerrainDestruction(x, Map.HEIGHT - 1, 0.5f);

                                // Do a gravity step and make sure it does slip down
                                battlefield.GravityStep();

                                if (battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Terrain didn't fall");
                                    return false;
                                }

                                // Otherwise this seems to have worked
                                return true;
                            }
                        }


                    }
                }
            }
            SetErrorDescription("Did not find any appropriate terrain to test");
            return false;
        }
        private static bool TestAttackEffect0RecordCurrentGame()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestGame0Game);

            AttackEffect weaponEffect = new Explosion(1, 1, 1);
            Game game = new Game(2, 1);
            weaponEffect.RecordCurrentGame(game);
            return true;
        }
        private static bool TestProjectile0Projectile()
        {
            Requires(TestExplosion0Explosion);
            TankController player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);
            Projectile projectile = new Projectile(25, 25, 45, 30, 0.02f, explosion, player);
            return true;
        }
        private static bool TestProjectile0ProcessTimeEvent()
        {
            Requires(TestGame0BeginGame);
            Requires(TestExplosion0Explosion);
            Requires(TestProjectile0Projectile);
            Requires(TestAttackEffect0RecordCurrentGame);
            Game game = InitialiseGame();
            game.BeginGame();
            TankController player = game.Player(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Projectile projectile = new Projectile(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.RecordCurrentGame(game);
            projectile.ProcessTimeEvent();

            // We can't really test this one without a substantial framework,
            // so we just call it and hope that everything works out

            return true;
        }
        private static bool TestProjectile0Render()
        {
            Requires(TestGame0BeginGame);
            Requires(TestGame0Player);
            Requires(TestExplosion0Explosion);
            Requires(TestProjectile0Projectile);
            Requires(TestAttackEffect0RecordCurrentGame);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the projectile
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.BeginGame();
            TankController player = game.Player(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Projectile projectile = new Projectile(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.RecordCurrentGame(game);
            projectile.Render(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestExplosion0Explosion()
        {
            TankController player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);

            return true;
        }
        private static bool TestExplosion0Detonate()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0Player);
            Requires(TestGame0BeginGame);

            Game game = InitialiseGame();
            game.BeginGame();
            TankController player = game.Player(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.RecordCurrentGame(game);
            explosion.Detonate(25, 25);

            return true;
        }
        private static bool TestExplosion0ProcessTimeEvent()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0Player);
            Requires(TestGame0BeginGame);
            Requires(TestExplosion0Detonate);

            Game game = InitialiseGame();
            game.BeginGame();
            TankController player = game.Player(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.RecordCurrentGame(game);
            explosion.Detonate(25, 25);
            explosion.ProcessTimeEvent();

            // Again, we can't really test this one without a full framework

            return true;
        }
        private static bool TestExplosion0Render()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestAttackEffect0RecordCurrentGame);
            Requires(TestGame0Player);
            Requires(TestGame0BeginGame);
            Requires(TestExplosion0Detonate);
            Requires(TestExplosion0ProcessTimeEvent);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the explosion
            TankController p = CreateTestingPlayer();
            Game game = InitialiseGame();
            game.BeginGame();
            TankController player = game.Player(1);
            Explosion explosion = new Explosion(10, 10, 10);
            explosion.RecordCurrentGame(game);
            explosion.Detonate(25, 25);
            // Step it for a bit so we can be sure the explosion is visible
            for (int i = 0; i < 10; i++)
            {
                explosion.ProcessTimeEvent();
            }
            explosion.Render(graphics, bitmapSize);

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }

        private static GameForm InitialiseGameForm(out NumericUpDown angleCtrl, out TrackBar powerCtrl, out Button fireCtrl, out Panel controlPanel, out ListBox weaponSelect)
        {
            Requires(TestGame0BeginGame);

            Game game = InitialiseGame();

            angleCtrl = null;
            powerCtrl = null;
            fireCtrl = null;
            controlPanel = null;
            weaponSelect = null;

            game.BeginGame();
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Game.BeginGame() did not create a GameForm and that is the only way GameForm can be tested");
                return null;
            }

            bool foundDisplayPanel = false;
            bool foundControlPanel = false;

            foreach (Control c in gameplayForm.Controls)
            {
                // The only controls should be 2 panels
                if (c is Panel)
                {
                    // Is this the control panel or the display panel?
                    Panel p = c as Panel;

                    // The display panel will have 0 controls.
                    // The control panel will have separate, of which only a few are mandatory
                    int controlsFound = 0;
                    bool foundFire = false;
                    bool foundAngle = false;
                    bool foundAngleLabel = false;
                    bool foundPower = false;
                    bool foundPowerLabel = false;


                    foreach (Control pc in p.Controls)
                    {
                        controlsFound++;

                        // Mandatory controls for the control panel are:
                        // A 'Fire!' button
                        // A NumericUpDown for controlling the angle
                        // A TrackBar for controlling the power
                        // "Power:" and "Angle:" labels

                        if (pc is Label)
                        {
                            Label lbl = pc as Label;
                            if (lbl.Text.ToLower().Contains("angle"))
                            {
                                foundAngleLabel = true;
                            }
                            else
                            if (lbl.Text.ToLower().Contains("power"))
                            {
                                foundPowerLabel = true;
                            }
                        }
                        else
                        if (pc is Button)
                        {
                            Button btn = pc as Button;
                            if (btn.Text.ToLower().Contains("fire"))
                            {
                                foundFire = true;
                                fireCtrl = btn;
                            }
                        }
                        else
                        if (pc is TrackBar)
                        {
                            foundPower = true;
                            powerCtrl = pc as TrackBar;
                        }
                        else
                        if (pc is NumericUpDown)
                        {
                            foundAngle = true;
                            angleCtrl = pc as NumericUpDown;
                        }
                        else
                        if (pc is ListBox)
                        {
                            weaponSelect = pc as ListBox;
                        }
                    }

                    if (controlsFound == 0)
                    {
                        foundDisplayPanel = true;
                    }
                    else
                    {
                        if (!foundFire)
                        {
                            SetErrorDescription("Control panel lacks a \"Fire!\" button OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngle)
                        {
                            SetErrorDescription("Control panel lacks an angle NumericUpDown OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPower)
                        {
                            SetErrorDescription("Control panel lacks a power TrackBar OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngleLabel)
                        {
                            SetErrorDescription("Control panel lacks an \"Angle:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPowerLabel)
                        {
                            SetErrorDescription("Control panel lacks a \"Power:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }

                        foundControlPanel = true;
                        controlPanel = p;
                    }

                }
                else
                {
                    SetErrorDescription(string.Format("Unexpected control ({0}) named \"{1}\" found in GameForm", c.GetType().FullName, c.Name));
                    return null;
                }
            }

            if (!foundDisplayPanel)
            {
                SetErrorDescription("No display panel found");
                return null;
            }
            if (!foundControlPanel)
            {
                SetErrorDescription("No control panel found");
                return null;
            }
            return gameplayForm;
        }

        private static bool TestGameForm0GameForm()
        {
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            return true;
        }
        private static bool TestGameForm0EnableControlPanel()
        {
            Requires(TestGameForm0GameForm);
            Game game = InitialiseGame();
            game.BeginGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Game.BeginGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that EnableControlPanel enables it
            controlPanel.Enabled = false;

            gameplayForm.EnableControlPanel();

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after GameForm.EnableControlPanel()");
                return false;
            }
            return true;

        }
        private static bool TestGameForm0SetAimingAngle()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            float testAngle = 27;

            gameplayForm.SetAimingAngle(testAngle);
            if (FloatEquals((float)angle.Value, testAngle)) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set angle to {0} but angle is {1}", testAngle, (float)angle.Value));
                return false;
            }
        }
        private static bool TestGameForm0SetPower()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            int testPower = 71;

            gameplayForm.SetPower(testPower);
            if (power.Value == testPower) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set power to {0} but power is {1}", testPower, power.Value));
                return false;
            }
        }
        private static bool TestGameForm0ChangeWeapon()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            gameplayForm.ChangeWeapon(0);

            // WeaponSelect is optional behaviour, so it's okay if it's not implemented here, as long as the method works.
            return true;
        }
        private static bool TestGameForm0Launch()
        {
            Requires(TestGameForm0GameForm);
            // This is something we can't really test properly without a proper framework, so for now we'll just click
            // the button and make sure it disables the control panel
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            controlPanel.Enabled = true;
            fire.PerformClick();
            if (controlPanel.Enabled)
            {
                SetErrorDescription("Control panel still enabled immediately after clicking fire button");
                return false;
            }

            return true;
        }
        private static void UnitTests()
        {
            DoTest(TestGame0Game);
            DoTest(TestGame0TotalPlayers);
            DoTest(TestGame0GetTotalRounds);
            DoTest(TestGame0SetPlayer);
            DoTest(TestGame0Player);
            DoTest(TestGame0PlayerColour);
            DoTest(TestGame0CalcPlayerLocations);
            DoTest(TestGame0RandomReorder);
            DoTest(TestGame0BeginGame);
            DoTest(TestGame0GetArena);
            DoTest(TestGame0CurrentPlayerTank);
            DoTest(TestTank0CreateTank);
            DoTest(TestTank0DrawTankSprite);
            DoTest(TestTank0SetLine);
            DoTest(TestTank0GetArmour);
            DoTest(TestTank0ListWeapons);
            DoTest(TestTankController0HumanOpponent);
            DoTest(TestTankController0CreateTank);
            DoTest(TestTankController0Identifier);
            DoTest(TestTankController0PlayerColour);
            DoTest(TestTankController0Winner);
            DoTest(TestTankController0GetScore);
            DoTest(TestHumanOpponent0NewRound);
            DoTest(TestHumanOpponent0StartTurn);
            DoTest(TestHumanOpponent0ProjectileHit);
            DoTest(TestPlayerTank0PlayerTank);
            DoTest(TestPlayerTank0Player);
            DoTest(TestPlayerTank0CreateTank);
            DoTest(TestPlayerTank0GetTankAngle);
            DoTest(TestPlayerTank0SetAimingAngle);
            DoTest(TestPlayerTank0GetTankPower);
            DoTest(TestPlayerTank0SetPower);
            DoTest(TestPlayerTank0GetCurrentWeapon);
            DoTest(TestPlayerTank0ChangeWeapon);
            DoTest(TestPlayerTank0Render);
            DoTest(TestPlayerTank0X);
            DoTest(TestPlayerTank0Y);
            DoTest(TestPlayerTank0Launch);
            DoTest(TestPlayerTank0DamageArmour);
            DoTest(TestPlayerTank0IsAlive);
            DoTest(TestPlayerTank0GravityStep);
            DoTest(TestMap0Map);
            DoTest(TestMap0TerrainAt);
            DoTest(TestMap0TankCollisionAt);
            DoTest(TestMap0TankPlace);
            DoTest(TestMap0TerrainDestruction);
            DoTest(TestMap0GravityStep);
            DoTest(TestAttackEffect0RecordCurrentGame);
            DoTest(TestProjectile0Projectile);
            DoTest(TestProjectile0ProcessTimeEvent);
            DoTest(TestProjectile0Render);
            DoTest(TestExplosion0Explosion);
            DoTest(TestExplosion0Detonate);
            DoTest(TestExplosion0ProcessTimeEvent);
            DoTest(TestExplosion0Render);
            DoTest(TestGameForm0GameForm);
            DoTest(TestGameForm0EnableControlPanel);
            DoTest(TestGameForm0SetAimingAngle);
            DoTest(TestGameForm0SetPower);
            DoTest(TestGameForm0ChangeWeapon);
            DoTest(TestGameForm0Launch);
        }
        
        #endregion
        
        #region CheckClasses

        private static bool CheckClasses()
        {
            string[] classNames = new string[] { "Program", "ComputerPlayer", "Map", "Explosion", "GameForm", "Game", "HumanOpponent", "Projectile", "TankController", "PlayerTank", "Tank", "AttackEffect" };
            string[][] classFields = new string[][] {
                new string[] { "Main" }, // Program
                new string[] { }, // ComputerPlayer
                new string[] { "TerrainAt","TankCollisionAt","TankPlace","TerrainDestruction","GravityStep","WIDTH","HEIGHT"}, // Map
                new string[] { "Detonate" }, // Explosion
                new string[] { "EnableControlPanel","SetAimingAngle","SetPower","ChangeWeapon","Launch","InitRenderBuffer"}, // GameForm
                new string[] { "TotalPlayers","CurrentRound","GetTotalRounds","SetPlayer","Player","GetBattleTank","PlayerColour","CalcPlayerLocations","RandomReorder","BeginGame","StartRound","GetArena","DrawPlayers","CurrentPlayerTank","AddWeaponEffect","ProcessWeaponEffects","RenderEffects","EndEffect","DetectCollision","DamageArmour","GravityStep","FinaliseTurn","FindWinner","NextRound","Wind"}, // Game
                new string[] { }, // HumanOpponent
                new string[] { }, // Projectile
                new string[] { "CreateTank","Identifier","PlayerColour","Winner","GetScore","NewRound","StartTurn","ProjectileHit"}, // TankController
                new string[] { "Player","CreateTank","GetTankAngle","SetAimingAngle","GetTankPower","SetPower","GetCurrentWeapon","ChangeWeapon","Render","X","Y","Launch","DamageArmour","IsAlive","GravityStep"}, // PlayerTank
                new string[] { "DrawTankSprite","SetLine","CreateTankBitmap","GetArmour","ListWeapons","FireWeapon","CreateTank","WIDTH","HEIGHT","NUM_TANKS"}, // Tank
                new string[] { "RecordCurrentGame","ProcessTimeEvent","Render"} // AttackEffect
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("Checking classes for public methods...");
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic)
                {
                    if (type.Namespace != "TankBattle")
                    {
                        Console.WriteLine("Public type {0} is not in the TankBattle namespace.", type.FullName);
                        return false;
                    }
                    else
                    {
                        int typeIdx = -1;
                        for (int i = 0; i < classNames.Length; i++)
                        {
                            if (type.Name == classNames[i])
                            {
                                typeIdx = i;
                                classNames[typeIdx] = null;
                                break;
                            }
                        }
                        foreach (MemberInfo memberInfo in type.GetMembers())
                        {
                            string memberName = memberInfo.Name;
                            bool isInherited = false;
                            foreach (MemberInfo parentMemberInfo in type.BaseType.GetMembers())
                            {
                                if (memberInfo.Name == parentMemberInfo.Name)
                                {
                                    isInherited = true;
                                    break;
                                }
                            }
                            if (!isInherited)
                            {
                                if (typeIdx != -1)
                                {
                                    bool fieldFound = false;
                                    if (memberName[0] != '.')
                                    {
                                        foreach (string allowedFields in classFields[typeIdx])
                                        {
                                            if (memberName == allowedFields)
                                            {
                                                fieldFound = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fieldFound = true;
                                    }
                                    if (!fieldFound)
                                    {
                                        Console.WriteLine("The public field \"{0}\" is not one of the authorised fields for the {1} class.\n", memberName, type.Name);
                                        Console.WriteLine("Remove it or change its access level.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //Console.WriteLine("{0} passed.", type.FullName);
                }
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                if (classNames[i] != null)
                {
                    Console.WriteLine("The class \"{0}\" is missing.", classNames[i]);
                    return false;
                }
            }
            Console.WriteLine("All public methods okay.");
            return true;
        }
        
        #endregion

        public static void Main()
        {
            if (CheckClasses())
            {
                UnitTests();

                int passed = 0;
                int failed = 0;
                foreach (string key in unitTestResults.Keys)
                {
                    if (unitTestResults[key] == "Passed")
                    {
                        passed++;
                    }
                    else
                    {
                        failed++;
                    }
                }

                Console.WriteLine("\n{0}/{1} unit tests passed", passed, passed + failed);
                if (failed == 0)
                {
                    Console.WriteLine("Starting up TankBattle...");
                    Program.Main();
                    return;
                }
            }

            Console.WriteLine("\nPress enter to exit.");
            Console.ReadLine();
        }
    }
}
