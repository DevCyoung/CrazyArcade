using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Crazy_Arcade
{


    class Map
    {

        public const int sizeX = 15;
        public const int sizeY = 13;

        public const int tileInter  = 39;
        public const int blockInter = 40;

        public int[,] iBlocks;
        public Rectangle[,] RectBlocks;
        public Rectangle[,] Rects;

        int START_W = 0;
        int START_H = 0;
        int w;
        int h;
        int[,] Tiles;
        int[,] Figure;

        MainForm mf;

        public  Map(MainForm mf)
        {
            int cnt = 0;

            Tiles   = new int[sizeY, sizeX];
            RectBlocks  = new Rectangle[sizeY, sizeX];
            Rects = new Rectangle[sizeY, sizeX];

            // Location Block
            iBlocks  = new int[sizeY, sizeX]
            {
                { 1,  1 , 1 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 1},      // 1
                { 1,  2 , 1 , 0 , 3 , 3 , 3 , 1 , 3 , 3 , 3 , 0 , 1 , 2 , 1},      // 2
                { 1,  1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 1},      // 3
                { 1,  0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 3 , 0 , 0},      // 4
                { 1,  0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 0},      // 5
                { 1,  0 , 3 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 3 , 0 , 0},      // 6
                { 1,  0 , 3 , 1 , 1 , 1 , 4 , 4 , 4 , 1 , 1 , 1 , 3 , 0 , 0},      // 7
                { 1,  1 , 0 , 3 , 0 , 1 , 1 , 1 , 1 , 1 , 0 , 3 , 0 , 1 , 1},      // 8
                { 1,  1 , 1 , 0 , 3 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 1 , 1 , 1},      // 9
                { 1,  1 , 1 , 1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 1 , 1 , 1},      // 10
                { 1,  1 , 0 , 1 , 1 , 0 , 3 , 3 , 3 , 0 , 1 , 1 , 0 , 1 , 1},      // 11
                { 1,  2 , 0 , 1 , 1 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 0 , 2 , 1},      // 12
                { 1,  0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1},      // 13
            };

            for ( int y = 0; y < sizeY; y++ )
            {
                for ( int x = 0; x < sizeX; x++ )
                {
                    if (cnt % 2 == 0)
                        Tiles[y, x] = 1;
                    else
                        Tiles[y, x] = 0;

                    ++cnt;
                }
            }

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {

                    // 블럭이 존재할때만 체크합니다.
                    if (iBlocks[y, x] == 0)
                    {
                        //매우밖에 위치를 시켜버립니다.
                        RectBlocks[y,x] = new Rectangle(99999999, 999999999, blockInter, blockInter);
                    }
                    else
                        RectBlocks[y, x] = new Rectangle(x * blockInter , y * blockInter, blockInter, blockInter);




                    Rects[y,x] = new Rectangle(x * blockInter, y * blockInter, blockInter, blockInter);







                }
            }





            this.mf = mf;

            //Sound Play

            
            


        }


        
        // 한번만그린다.
        public void Show(Graphics e)
        {
            //if (one)
            //    return;
            //one = true;

            // Draw Tiles

            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (Tiles[y, x] == 1)
                    {
                        e.DrawImage(Properties.Resources.Tile2, new Point(x * tileInter, y * tileInter));

                        //e.Graphics.DrawImage(Properties.Resources.Tile4, 
                        //    START_W + i * interval, START_H +j * interval, Properties.Resources.Tile4.Width,
                        //    Properties.Resources.Tile4.Height);
                    }
                    else
                    {
                        e.DrawImage(Properties.Resources.Tile4, new Point(x * tileInter, y * tileInter));
                        //e.Graphics.DrawImage(Properties.Resources.Tile2,
                        //    START_W + i * interval, START_H + j * interval, Properties.Resources.Tile2.Width,
                        //    Properties.Resources.Tile4.Height);
                    }
                }
            }

            


            //Draw Blocks
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    if (iBlocks[y, x] == 3)
                    {
                        e.DrawImage(Properties.Resources.Box_02, RectBlocks[y, x]);
                    }
                
                    else if (iBlocks[y, x] == 1)
                    {
                        e.DrawImage(Properties.Resources.Box_01, RectBlocks[y, x]);
                        
                    }

                    else if ( iBlocks[y , x] == 2 )
                    {
                        e.DrawImage(Properties.Resources.Raw, RectBlocks[y, x]);
                    }
                   
                    //draw box
                    if (iBlocks[y, x] != 0 && Manager.DrawCollider )
                        e.DrawRectangle(new Pen(Color.Green,2), RectBlocks[y,x] );



                }
            }

            


        }

      

    }

    

}
