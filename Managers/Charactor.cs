using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Crazy_Arcade.Managers;



enum Direction
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

        //INFO
        MainForm form;
        public Point position;
        public Map map;

        //Input
        bool[] inputKeys = new bool[4];
        
        bool anyInput = false;
        Direction playerDir = Direction.DOWN;
        Rectangle dirRect;

        
        //Boom 
        WaterBoom[] booms = new WaterBoom[100];
        int boomIndex = 0;

        //Charactor SPR
        public Image[][] animations = new Image[4][];
        public Image[] curAnim;
        public int sprIndex = 0;
        public int animDelay = 0;

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


      
        public Charactor(MainForm form , Map map)
        {

            this.form   = form;
            this.map    = map;

            curAnim     = down;
            dirRect     = new Rectangle();
            position    = new Point(40 * 12, 39 * 12 + 1);

            animations[(int)Direction.LEFT]  = left;
            animations[(int)Direction.UP]    = up;
            animations[(int)Direction.RIGHT] = right;
            animations[(int)Direction.DOWN]  = down;
        }

        public void InputKeyDown(KeyEventArgs e)
        {
            
            
            switch ( e.KeyCode )
            {

                case Keys.Left:
                    playerDir = Direction.LEFT;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Up:
                    playerDir = Direction.UP;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Right:
                    playerDir = Direction.RIGHT;
                    inputKeys[(int)playerDir] = true;
                    break;
                case Keys.Down:
                    playerDir = Direction.DOWN;
                    inputKeys[(int)playerDir] = true;
                    break;
            }
            


            if ( Keys.Space == e.KeyCode )
            {
                WaterBoom boom = new WaterBoom(this);
                boom.active = true;

                double result = double.MaxValue;
                Rectangle dest = new Rectangle();

                Rectangle boomrect = new Rectangle(position.X, position.Y + 15, 46, 42);
                Rectangle[,] Rects = map.Rects;
                Point TilePos = new Point(0, 0);

                for (int y = 0; y < Map.sizeY; y++)
                {
                    for (int x = 0; x < Map.sizeX; x++)
                    {
                        Rectangle rect = Rects[y, x];
                        double TilepointX = rect.X + Map.blockInter / 2;
                        double TilepointY = rect.Y + Map.blockInter / 2;

                        double boompointX = boomrect.X + Map.blockInter / 2;
                        double boompointY = boomrect.Y + Map.blockInter / 2;

                        double distance = Math.Pow(TilepointX - boompointX, 2) + Math.Pow(TilepointY - boompointY, 2);
                        distance = Math.Sqrt(distance);

                        if (distance < result)
                        {
                            result = distance;
                            dest = rect;
                            TilePos = new Point(x, y);
                        }

                    }
                }
                boom.rect = dest;
                boom.tilePos = TilePos;
                booms[boomIndex] = boom;
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
                    inputKeys[(int)Direction.LEFT] = false;
                    break;
                case Keys.Up:
                    inputKeys[(int)Direction.UP]    = false;
                    break;
                case Keys.Right:
                    inputKeys[(int)Direction.RIGHT] = false;
                    break;
                case Keys.Down:
                    inputKeys[(int)Direction.DOWN] = false;
                    break;
            }

        }

        public void Draw(Graphics e)
        {

           
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

        public void Update(int deltaTime)
        {

            anyInput = false;
            for (int i = 0; i < 4; i++)
            {
                if (inputKeys[i])
                    anyInput = true;
            }


            InputUpdate(deltaTime);

            MoveUpdate(deltaTime);
            AnimationUpdate(deltaTime);



        }


        
        public void AnimationUpdate(int deltaTime)
        {

            animDelay += deltaTime;
            curAnim = animations[(int)playerDir];

            if (anyInput == false)
            {
                sprIndex = 0;
                return;
            }

            if (animDelay < 4)
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
            for (int y = 0; y < Map.sizeY; y++)
            {
                for (int x = 0; x < Map.sizeX; x++)
                {

                    if (map.iBlocks[y, x] == 0)
                        continue;

                    if ( CollideCheck(dirRect, map.RectBlocks[y, x]) )
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


            if (isCollide)
                return;

            for (int i = 0; i < 4; i++)
            {
                if (playerDir == (Direction)i && inputKeys[i])
                {
                    position.X += tickMove * nx[i];
                    position.Y += tickMove * ny[i];
                    break;
                }
            }

          
        }
        
        public void InputUpdate(int deltatime)
        {

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

