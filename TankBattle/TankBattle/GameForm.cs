using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class GameForm : Form
    {
        private Color landscapeColour;
        private Random rng = new Random();
        private Image backgroundImage = null;
        private int levelWidth = 160;
        private int levelHeight = 120;
        private Game currentGame;

        private BufferedGraphics backgroundGraphics;
        private BufferedGraphics gameplayGraphics;

        public GameForm(Game game)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            currentGame = game;

            string[] imageFileNames = { "Images\\background1.jpg",
                                        "Images\\background2.jpg",
                                        "Images\\background3.jpg",
                                        "Images\\background4.jpg"};

            Color[] landscapeColours = { Color.FromArgb(255, 0, 0, 0),
                                        Color.FromArgb(255, 0, 0, 0),
                                        Color.FromArgb(255, 0, 0, 0),
                                        Color.FromArgb(255, 0, 0, 0)};

            int nRandom = rng.Next(0, 3);

            backgroundImage = Image.FromFile(imageFileNames[nRandom]);
            landscapeColour = landscapeColours[nRandom];

            InitializeComponent();

            backgroundGraphics = InitRenderBuffer();
            gameplayGraphics = InitRenderBuffer();
            DrawBackground();
            DrawGameplay();
            NewTurn();
        }

        // From https://stackoverflow.com/questions/13999781/tearing-in-my-animation-on-winforms-c-sharp
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public void EnableControlPanel()
        {
            controlPanel.Enabled = true;
        }

        public void SetAimingAngle(float angle)
        {
            numUpDownAngle.Value = (decimal)angle;
        }

        public void SetPower(int power)
        {
            tbPower.Value = power;
        }
        public void ChangeWeapon(int weapon)
        {
            cmbWeapon.SelectedItem = weapon;
        }

        public void Launch()
        {
            PlayerTank player = currentGame.CurrentPlayerTank();
            player.Launch();
            controlPanel.Enabled = false;
            timer.Enabled = true;
        }

        private void DrawBackground()
        {
            Graphics graphics = backgroundGraphics.Graphics;
            Image background = backgroundImage;
            graphics.DrawImage(backgroundImage, new Rectangle(0, 0, displayPanel.Width, displayPanel.Height));

            Map battlefield = currentGame.GetArena();
            Brush brush = new SolidBrush(landscapeColour);

            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        int drawX1 = displayPanel.Width * x / levelWidth;
                        int drawY1 = displayPanel.Height * y / levelHeight;
                        int drawX2 = displayPanel.Width * (x + 1) / levelWidth;
                        int drawY2 = displayPanel.Height * (y + 1) / levelHeight;
                        graphics.FillRectangle(brush, drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1);
                    }
                }
            }
        }

        public BufferedGraphics InitRenderBuffer()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            Graphics graphics = displayPanel.CreateGraphics();
            Rectangle dimensions = new Rectangle(0, 0, displayPanel.Width, displayPanel.Height);
            BufferedGraphics bufferedGraphics = context.Allocate(graphics, dimensions);
            return bufferedGraphics;
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = displayPanel.CreateGraphics();
            gameplayGraphics.Render(graphics);
        }

        private void DrawGameplay()
        {
            backgroundGraphics.Render(gameplayGraphics.Graphics);
            currentGame.DrawPlayers(gameplayGraphics.Graphics, displayPanel.Size);
            currentGame.RenderEffects(gameplayGraphics.Graphics, displayPanel.Size);
        }

        private void NewTurn()
        {
            PlayerTank player = currentGame.CurrentPlayerTank();
            TankController tankController = player.Player();
            this.Text = "Tank Battle - Round " + currentGame.CurrentRound() +"of " + currentGame.GetTotalRounds();
            BackColor = tankController.PlayerColour();
            lblPlayerName.Text = tankController.Identifier();
            SetAimingAngle(player.GetTankAngle());
            SetPower(player.GetTankPower());
            if (currentGame.Wind() > 0)
            {
                lblWindValue.Text = currentGame.Wind() + " E";
            }
            else
            {
                lblWindValue.Text = currentGame.Wind() * -1 + " W";
            }
            cmbWeapon.Items.Clear();
            Tank tank = player.CreateTank();
            String[] lWeaponsAvailable = tank.ListWeapons();
            cmbWeapon.Items.AddRange(lWeaponsAvailable);
            ChangeWeapon(player.GetCurrentWeapon());
            tankController.StartTurn(this, currentGame);
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            Launch();
        }

        private void cmbWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeWeapon(cmbWeapon.SelectedIndex);
        }

        private void numUpDownAngle_ValueChanged(object sender, EventArgs e)
        {
            SetAimingAngle((float)numUpDownAngle.Value);
            DrawGameplay();
            displayPanel.Invalidate();
        }

        private void tbPower_ValueChanged(object sender, EventArgs e)
        {
            int pwr = tbPower.Value;
            SetPower(pwr);
            lblPowerValue.Text = pwr.ToString();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (currentGame.ProcessWeaponEffects())
            {
                DrawGameplay();
                displayPanel.Invalidate();
            }
            else
            {
                currentGame.GravityStep();
                DrawBackground();
                DrawGameplay();
                displayPanel.Invalidate();
                if (currentGame.GravityStep())
                {
                    return;
                }
                else
                {
                    timer.Enabled = false;
                    if (currentGame.FinaliseTurn())
                    {
                        NewTurn();
                    }
                    Dispose();
                    currentGame.NextRound();
                    return;
                }
            }
        }
    }
}
