using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Crazy_Arcade.Managers;



enum DIRECTION
{
    LEFT = 0,
    UP,
    RIGHT,
    DOWN,
}

namespace Crazy_Arcade
{


    class Charactor
    {

        #region SPRITE_ANIMS
        readonly Image[] left ={
            Properties.Resources.BazziLeftIdle,
            Properties.Resources.BazziLeftWalk_2,
            Properties.Resources.BazziLeftWalk_1,
            Properties.Resources.BazziLeftWalk_3,
            Properties.Resources.BazziLeftWalk_4
        };
        readonly Image[] up ={
            Properties.Resources.BazziUpIdle,
            Properties.Resources.BazziUpWalk_1,
            Properties.Resources.BazziUpWalk_2,
            Properties.Resources.BazziUpWalk_3,
            Properties.Resources.BazziUpWalk_4
        };
        readonly Image[] right ={
            Properties.Resources.BazziRightIdle,
            Properties.Resources.BazziRightWalk_2,
            Properties.Resources.BazziRightWalk_1,
            Properties.Resources.BazziRightWalk_3,
            Properties.Resources.BazziRightWalk_4
        };
        readonly Image[] down ={
            Properties.Resources.BazziDownIdle_0,
            Properties.Resources.BazziDownWalk_1,
            Properties.Resources.BazziDownWalk_2,
            Properties.Resources.BazziDownWalk_3,
            Properties.Resources.BazziDownWalk_4
        };
        #endregion

        //INFO
        public Map map;


        public Point position;

        //Input
        bool[] inputKeys  = new bool[4];
        bool inputSpace   = false;
        bool anyInput = false;
        DIRECTION playerDir = DIRECTION.DOWN;
        Rectangle dirRect;

        
        //Boom 
        public WaterBoom[] booms = new WaterBoom[100];
        int boomIndex = 0;

        //Charactor SPR
        public Image[][] animations = new Image[4][];
        public Image[] curAnim;
        public int sprIndex = 0;
        public int animDelay = 0;



        public Charactor( Map map)
        {

            
            this.map    = map;

            curAnim     = down;
            dirRect     = new Rectangle();
            position    = new Point(40 * 12, 39 * 12 + 1);

            animations[(int)DIRECTION.LEFT]  = left;
            animations[(int)DIRECTION.UP]    = up;
            animations[(int)DIRECTION.RIGHT] = right;
            animations[(int)DIRECTION.DOWN]  = down;
        }
        public void Update(int deltaTime)
        {
            InputUpdate(deltaTime);
            MoveUpdate(deltaTime);
            AnimUpdate(deltaTime);
        }

        public void Draw(Graphics e)
        {

           //Draw Booms
            for (int i = 0; i < 100; i++)
            {
                if (booms[i] == null)
                    continue;
                booms[i].Update(e, 2);
            }

            //Draw Charactor
            int width = curAnim[sprIndex].Width;
            int height = curAnim[sprIndex].Height;
            e.DrawImage(curAnim[sprIndex] , position.X, position.Y -10 , width, height);

            //Draw CharactorRect
            if (Manager.DrawCollider )
                e.DrawRectangle(new Pen(Color.Red,2), new Rectangle(position.X, position.Y + 15, 40, 38) );

            //Draw dirRect
            if(  Manager.DrawCollider )
                e.DrawRectangle(new Pen(Color.Blue, 2), dirRect );

        }

        public void InputKeyDown(KeyEventArgs e)
        {
            
            
            switch ( e.KeyCode )
            {

                case Keys.Left:
                    playerDir = DIRECTION.LEFT;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Up:
                    playerDir = DIRECTION.UP;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Right:
                    playerDir = DIRECTION.RIGHT;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Down:
                    playerDir = DIRECTION.DOWN;
                    inputKeys[(int)playerDir] = true;
                    break;
            }
            


            if ( Keys.Space == e.KeyCode && inputSpace == false )
            {
                inputSpace = true;
                Rectangle[,] mapRects = map.Rects;
                Rectangle destRect = new Rectangle();
                Rectangle boomrect = new Rectangle(position.X, position.Y + 15, 46, 42);
                Point TilePos = new Point(0, 0);

                WaterBoom curBoom = new WaterBoom(this);
                double result = double.MaxValue;
                curBoom.active = true;

                for (int y = 0; y < Map.SIZEY; y++)
                {
                    for (int x = 0; x < Map.SIZEX; x++)
                    {
                        Rectangle rect = mapRects[y, x];
                        double TilepointX = rect.X + Map.BLOCK_INTER / 2;
                        double TilepointY = rect.Y + Map.BLOCK_INTER / 2;

                        double boompointX = boomrect.X + Map.BLOCK_INTER / 2;
                        double boompointY = boomrect.Y + Map.BLOCK_INTER / 2;

                        double distance = Math.Pow(TilepointX - boompointX, 2) + Math.Pow(TilepointY - boompointY, 2);
                        distance = Math.Sqrt(distance);

                        if (distance < result)
                        {
                            result = distance;
                            destRect = rect;
                            TilePos = new Point(x, y);
                        }

                    }
                }

                curBoom.rect = destRect;
                curBoom.tilePos = TilePos;
                booms[boomIndex] = curBoom;
                ++boomIndex;
                if (boomIndex > 99)
                    boomIndex = 0;

            }


        }
        public void InputkeyUp(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Left:
                    inputKeys[(int)DIRECTION.LEFT] = false;
                    break;
                case Keys.Up:
                    inputKeys[(int)DIRECTION.UP]    = false;
                    break;
                case Keys.Right:
                    inputKeys[(int)DIRECTION.RIGHT] = false;
                    break;
                case Keys.Down:
                    inputKeys[(int)DIRECTION.DOWN] = false;
                    break;
            }

            if ( e.KeyCode == Keys.Space )
                inputSpace = false;
            

        }

        public void AnimUpdate(int deltaTime)
        {

            if (anyInput == false)
            {
                sprIndex = 0;
                return;
            }

            animDelay += deltaTime;
            curAnim = animations[(int)playerDir];

            if (animDelay < 5)
                return;

            animDelay = 0;
            ++sprIndex;

            if (sprIndex > 4)
                sprIndex = 0;

        }
        public void MoveUpdate(int deltaTime)
        {


            int[] nx    = {-1 ,  0 , 1 , 0};
            int[] ny    = { 0 , -1 , 0 , 1};
            int[] dirX  = { 0, 15, 25, 15 };
            int[] dirY  = { 25, 15, 25, 40 };

            int tickMove = 8;
            bool isCollide = false;

            dirRect = new Rectangle(position.X + dirX[(int)playerDir] + nx[(int)playerDir] * 5,
                                    position.Y + dirY[(int)playerDir] + ny[(int)playerDir] * 5,
                                    15, 15);



            //Box 충돌 체크 
            for (int y = 0; y < Map.SIZEY; y++)
            {
                for (int x = 0; x < Map.SIZEX; x++)
                {

                    if (map.iBlocks[y, x] == 0)
                        continue;

                    if ( CollideCheck(dirRect, map.BlockCollides[y, x]) )
                        isCollide = true;  
                    
                }
            }

            //Boom 충돌 체크
            for (int i = 0; i < 100; i++)
            {

                if (booms[i] == null)
                    break;

                if (booms[i].playerExit == false || booms[i].active == false || booms[i].isTrigger == true)
                    continue;

                if (CollideCheck(dirRect, booms[i].rect))
                    isCollide = true;

            }

            //물폭탄 터진 충돌 체크


            if (isCollide)
                return;

            for (int i = 0; i < 4; i++)
            {
                if (playerDir == (DIRECTION)i && inputKeys[i])
                {
                    position.X += tickMove * nx[i];
                    position.Y += tickMove * ny[i];
                    break;
                }
            }

          
        }
        
        public void InputUpdate(int deltatime = 1)
        {
            anyInput = false;
            for (int i = 0; i < 4; i++)
            {
                if (inputKeys[i])
                    anyInput = true;
            }
        }

        public bool CollideCheck(Rectangle r1 , Rectangle r2 )
        {
            bool check = false;

            if (r1.Left < r2.Right && r1.Right > r2.Left && r1.Top < r2.Bottom && r1.Bottom > r2.Top)
                check = true;
            return check;
        }
    

    }




    
}

