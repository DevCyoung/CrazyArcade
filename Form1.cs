using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Crazy_Arcade
{


    public partial class MainForm : Form
    {



        Charactor player;
        Map map;

        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

        }



        private void Form1_Load(object sender, EventArgs e)
        {

            Invalidate();

            map    = new Map();
            player = new Charactor(map);
            

            GameTimer.Enabled = true;
        }










        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            player.InputKeyDown(e);
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            player.InputkeyUp(e);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            BufferedGraphicsContext currentContext;
            BufferedGraphics myBuffer;

            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate( e.Graphics , this.DisplayRectangle);
            ////////////////////////////////////////////////////////////////////////
            //Draw

            map.Draw(myBuffer.Graphics);
            player.Draw(myBuffer.Graphics);

            //Draw
            ////////////////////////////////////////////////////////////////////////
            myBuffer.Render();
            myBuffer.Dispose();
            
        }





        const int deltaTime = 3;
        private void GameTimer_Tick(object sender, EventArgs e)
        {

            Refresh();
            player.Update(deltaTime);

        }

        

       

    }
}
