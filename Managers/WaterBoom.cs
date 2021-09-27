using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        public bool active = false;
        public bool isTrigger = false;

        int index = 0;
        int waterIndex = 0;

        int animDelay = 0;
        int boomDelay = 0;
        int waterDelay = 0;
        int power = 5;

        public Rectangle rect;
        public Point tilePos;
        public Charactor ch;

        public bool playerExit = false;
        public bool oneShot = false ;


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

        Image curImage;
        Image[][] animations;
        public WaterBoom(Charactor ch)
        {
            curImage = bublesIdle[index];
            rect = new Rectangle();

            this.ch = ch;
            animations = new Image[4][];

            animations[0] = bubleRight;
            animations[1] = bubleLeft;
            animations[2] = bubleDown;
            animations[3] = bubleUp;



        }

        public void Update(Graphics e , int deltatime )
        {

            

            if (active == false)
                return;

            //밖으로 나갔는지 검사 수정
            if (playerExit == false )
            {

                Rectangle pr = new Rectangle(ch.position.X, ch.position.Y + 15, 40, 40);
                Rectangle br = rect;

                double TilepointX = pr.X + Map.blockInter / 2;
                double TilepointY = pr.Y + Map.blockInter / 2;

                double boompointX = br.X + Map.blockInter / 2;
                double boompointY = br.Y + Map.blockInter / 2;

                double distance = Math.Pow(TilepointX - boompointX, 2) + Math.Pow(TilepointY - boompointY, 2);
                distance = Math.Sqrt(distance);

                if ( distance > 25)
                    playerExit = true;

            }

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
                e.DrawRectangle(new Pen(Color.White), rect);

            if ( oneShot == false )
                e.DrawImage(bublesIdle[index], rect);


            // 폭발!
            if (boomDelay >= 150 && boomDelay <= 180 )
            {
                isTrigger = true;
              
                // Right , Left , Up , Down
                int[] nx = { 1, -1, 0, 0 };
                int[] ny = { 0, 0, 1, -1 };
                waterDelay += deltatime;

                if (waterDelay > 3)
                {
                    ++waterIndex;
                    waterDelay = 0;
                }




                bool[] checking = new bool[4];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < power; j++)
                    {
                        //if (checking[i] == true)
                        //    break;

                        int nnx = nx[i] * j + tilePos.X;
                        int nny = ny[i] * j + tilePos.Y;

                        if (nny >= 13 || nny < 0 || nnx >= 15 || nnx < 0)
                            continue;
                        if (waterIndex > 4)
                            continue;
                        Image image = animations[i][waterIndex];

                        if (ch.map.iBlocks[nny, nnx] != 0)
                        {
                            checking[i] = true;
                            break;
                        }
                        

                        e.DrawRectangle(new Pen(Color.Violet , 2),  ch.map.Rects[nny , nnx ]);
                        e.DrawImage(image, ch.map.Rects[nny, nnx]);
                    }


                }

                checking = new bool[4];
                if (oneShot == false)
                {
                    oneShot = true;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < power; j++)
                        {

                            if (checking[i] == true)
                                break;

                            int nnx = nx[i] * j + tilePos.X;
                            int nny = ny[i] * j + tilePos.Y;

                            if (nny >= 13 || nny < 0 || nnx >= 15 || nnx < 0)
                                continue;



                            if (ch.map.iBlocks[nny, nnx] != 0)
                            {
                                ch.map.iBlocks[nny, nnx] = 0;
                                checking[i] = true;
                            }



                        }


                    }


                }

                
            }
            


            if (boomDelay >= 152)
            {
                isTrigger = true;
            }




        }

        int animationDelay = 0;
        int BoomDelay = 0;
        public enum BoomState
        {
            NONE,
            IDLE,
            BOOM,
            BOOMING,
        }

        public BoomState state = BoomState.NONE;
        public void Draw(Graphics e, int deltatime )
        {

            if ( state == BoomState.NONE )
                return;

            //밖으로 나갔는지 검사 수정
            if (playerExit == false)
            {

                Rectangle pr = new Rectangle(ch.position.X, ch.position.Y + 15, 40, 40);
                Rectangle br = rect;

                double TilepointX = pr.X + Map.blockInter / 2;
                double TilepointY = pr.Y + Map.blockInter / 2;

                double boompointX = br.X + Map.blockInter / 2;
                double boompointY = br.Y + Map.blockInter / 2;

                double distance = Math.Pow(TilepointX - boompointX, 2) + Math.Pow(TilepointY - boompointY, 2);
                distance = Math.Sqrt(distance);

                if (distance > 25)
                    playerExit = true;

            }

            animDelay += deltatime;
            boomDelay += deltatime;

            if (animDelay > 6)
            {
                ++index;
                animDelay = 0;
            }

            if (index > 3)
                index = 0;

            if (Manager.DrawCollider)
                e.DrawRectangle(new Pen(Color.White), rect);

            if (state == BoomState.IDLE)
            {
                e.DrawImage(bublesIdle[index], rect);
            }
            else if (state == BoomState.BOOM)
            {
                state = BoomState.BOOMING;
                isTrigger = true;
           
            }
            else if ( state == BoomState.BOOMING)
            {

                int[] nx = { 1, -1, 0, 0 };
                int[] ny = { 0, 0, 1, -1 };
                bool[] check = new bool[4];
                waterDelay += deltatime;

                if (waterDelay > 3)
                {
                    ++waterIndex;
                    waterDelay = 0;
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < power; j++)
                    {
                        int nnx = nx[i] * j + tilePos.X;
                        int nny = ny[i] * j + tilePos.Y;
                        

                        if (nny >= 13 || nny < 0 || nnx >= 15 || nnx < 0)
                            continue;
                        if (waterIndex > 4)
                            continue;

                        e.DrawRectangle(new Pen(Color.Violet, 2), ch.map.Rects[nny, nnx]);
                        e.DrawImage(animations[i][waterIndex], ch.map.Rects[nny, nnx]);

                        // 여기까지 물폭탄
                        if (ch.map.iBlocks[nny, nnx] != 0)
                        {
                            ch.map.iBlocks[nny, nnx] = 0;
                            check[i] = true;
                            break;
                        }

                        

                    }


                }

                
                

            }
        }



            

        








    }
}
