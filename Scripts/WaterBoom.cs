using System;
using System.Drawing;
using Crazy_Arcade.Helper;
using System.Windows.Forms;

namespace Crazy_Arcade.Managers
{
    enum BoomState
    {
        None,
        Idle,
        Boom,

    }

    class WaterBoom
    {

        #region SPR_ANIMATION

        readonly Image[] bubleLeft ={
            Properties.Resources.boomLeft_1,
            Properties.Resources.boomLeft_2,
            Properties.Resources.boomLeft_3,
            Properties.Resources.boomLeft_4,
            Properties.Resources.boomLeft_5,
        };

        readonly Image[] bubleRight ={
            Properties.Resources.boomRight_1,
            Properties.Resources.boomRight_2,
            Properties.Resources.boomRight_3,
            Properties.Resources.boomRight_4,
            Properties.Resources.boomRight_5,
        };

        readonly Image[] bubleUp ={
            Properties.Resources.boomUp_1,
            Properties.Resources.boomUp_2,
            Properties.Resources.boomUp_3,
            Properties.Resources.boomUp_4,
            Properties.Resources.boomUp_5,
        };

        readonly Image[] bubleDown ={
            Properties.Resources.boomDown_1,
            Properties.Resources.boomDown_2,
            Properties.Resources.boomDown_3,
            Properties.Resources.boomDown_4,
            Properties.Resources.boomDown_5,
        };

        readonly Image[] bublesIdle ={
            Properties.Resources.colorbubble1,
            Properties.Resources.colorbubble2,
            Properties.Resources.colorbubble3,
            Properties.Resources.colorbubble2,
        };
        #endregion SPR_ANIMATION

        public bool active = false;
        public bool isTrigger = true;

        int index = 0;
        int waterIndex = 0;

        int animDelay = 0;
        int boomDelay = 0;
        int waterDelay = 0;

        int power;

        public Rectangle collide;
        public Point position;

        public Charactor player;

        
        public bool isShoot = false ;
        public int[] curPower;

        
        Image[][] animations;

        public static int[,] BoomArray = new int[Map.SIZEY,Map.SIZEX];

        public WaterBoom(Charactor player ,Rectangle collide , Point poosition , int power)
        {

            this.player     = player;
            this.collide    = collide;
            this.position   = poosition;
            this.power      = power;

            animations      = new Image[4][];
            animations[0]   = bubleRight;
            animations[1]   = bubleLeft;
            animations[2]   = bubleDown;
            animations[3]   = bubleUp;

            
            curPower = new int[4] { power, power, power, power };
        }


        public void Update(Graphics e , int deltatime )
        {

            if (active == false)
                return;
            if (waterIndex > 4)
            {
                active = false;
                WaterBoom.BoomArray[position.Y, position.X] = 0;
            }
            if ( isTrigger )
                if (MathHelper.CollideDistance(player.collider, collide) > 25)
                    isTrigger = false;

            animDelay += deltatime;
            boomDelay += deltatime;

            if (animDelay > 6)
            {
                ++index;
                animDelay = 0;
            }

            if (index > 3)
                index = 0;

            if ( Manager.DrawCollider )
                e.DrawRectangle(new Pen(Color.White,2), collide);

            if ( isShoot == false )
                e.DrawImage(bublesIdle[index], collide);

            //Boom
            if (boomDelay >= 150 )
            {

                //       Right , Left , Up , Down
                int[] nx = { 1, -1, 0, 0 };
                int[] ny = { 0, 0, 1, -1 };

                isTrigger = true;
                isShoot = true;
               
                waterDelay += deltatime;

                if (waterDelay > 3)
                {
                    ++waterIndex;
                    waterDelay = 0;
                }
                
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < curPower[i]; j++)
                    {
                        int nnx = nx[i] * j + position.X;
                        int nny = ny[i] * j + position.Y;

                        if (nny >= Map.SIZEY || nny < 0 || nnx >= Map.SIZEX || nnx < 0)
                            break;

                        //캐릭터와 충돌하는가?
                        if ( MathHelper.IsCollide(player.map.BlockCollides[nny, nnx] ,  player.collider ) )
                        {
                            //Die
                            //MessageBox.Show("죽었습니다!.");

                        }


                        if (waterIndex > 4)
                            break;
                        if (player.map.iBlocks[nny, nnx] == (int)BLOCK.RAW || player.map.iBlocks[nny, nnx] == (int)BLOCK.WALL)
                            break;

                        Image bublleSprite = animations[i][waterIndex];
                        e.DrawRectangle(new Pen(Color.Aqua , 2), player.map.BlockCollides[nny, nnx]);
                        e.DrawImage(bublleSprite, player.map.BlockCollides[nny, nnx]);

                        if (player.map.iBlocks[nny, nnx] != (int)BLOCK.NONE )
                        {
                            player.map.iBlocks[nny, nnx] = (int)BLOCK.NONE;
                            curPower[i] = j+1;
                            break;
                        }

                    }
                }


            }

        }
    }
}
