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

            //DoubleBuffered = true;

            

        }



        private void Form1_Load(object sender, EventArgs e)
        {

            Invalidate();
            map    = new Map(this);
            player = new Charactor(this , map);
            GameTimer.Enabled = true;







        }










        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            player.InputKeyDown(e);

            //if ( e.KeyCode == Keys.Enter)
            //    MessageBox.Show($"X : { player.position.X }   Y : { player.position.Y } ");

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

            map.Show(myBuffer.Graphics);
            player.Draw(myBuffer.Graphics);

            
            myBuffer.Render();
            myBuffer.Dispose();
            
            //e.Graphics.DrawImage(Properties.Resources.BazziDownIdle, new Point(500, 500) ) ;

        }

       

  

        // 1초에 80번 깜빡인다.
        int frame = 0;
        private void GameTimer_Tick(object sender, EventArgs e)
        {

            //frame += 1;

            //if ( frame >= 1000 )
            //{
            //    Invalidate();
            //    player.Update(GameTimer.Interval);
            //    //frame = 0;
            //    MessageBox.Show($"{frame/1000} 초 ");
            //}

            Refresh();
            player.Update(3);




        }

        

       

    }
}
