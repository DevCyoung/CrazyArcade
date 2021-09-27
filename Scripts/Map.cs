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
    public enum BLOCK
    {
        NONE = 0,
        SAND = 1,
        RAW  = 2,
        WOOD = 3,
        WALL = 4,
    }

    public enum ITEM
    {
        POWER,
        SPEED,

    }


    class Map
    {
       
        public const int SIZEX = 15;
        public const int SIZEY = 13;

        public const int TILE_INTER  = 39;
        public const int BLOCK_INTER = 40;

        public int[,] iBlocks;
        public int[,] iItems;

        public Rectangle[,] BlockCollides;
        public Rectangle[,] ItemCollides;

        public Rectangle[,] Rects;

    

        public  Map()
        {
           
            BlockCollides  = new Rectangle[SIZEY, SIZEX];
            Rects = new Rectangle[SIZEY, SIZEX];

            //Block Designe
            iBlocks  = new int[SIZEY, SIZEX]
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

            
            for (int y = 0; y < SIZEY; y++)
            {
                for (int x = 0; x < SIZEX; x++)
                {

                    // 블럭이 존재할때만 체크합니다.
                    if (iBlocks[y, x] == (int)BLOCK.NONE)
                    {
                        BlockCollides[y, x] = new Rectangle(99999999, 999999999, BLOCK_INTER, BLOCK_INTER);
                    }
                    else
                    {
                        BlockCollides[y, x] = new Rectangle(x * BLOCK_INTER, y * BLOCK_INTER, BLOCK_INTER, BLOCK_INTER);
                    }

                    Rects[y, x] = new Rectangle(x * BLOCK_INTER, y * BLOCK_INTER, BLOCK_INTER, BLOCK_INTER);
                }
            }


            //아이템을 만든다.
            //랜덤으로 섞는다 
            

            

        }


        public void Draw(Graphics e)
        {
            int cnt = 0;

            // Draw Tiles
            for (int y = 0; y < SIZEY; y++)
            {
                for (int x = 0; x < SIZEX; x++)
                {
                    if (cnt % 2 == 0 )
                        e.DrawImage(Properties.Resources.Tile2, new Point(x * TILE_INTER, y * TILE_INTER));
                    else
                        e.DrawImage(Properties.Resources.Tile4, new Point(x * TILE_INTER, y * TILE_INTER));
                    ++cnt;
                }
            }

            //Draw Blocks
            for (int y = 0; y < SIZEY; y++)
            {
                for (int x = 0; x < SIZEX; x++)
                {

                    Image blockSprite = null;
                    switch ( (BLOCK)iBlocks[y,x] )
                    {
                        case BLOCK.WOOD:
                            blockSprite = Properties.Resources.Box_02;
                            break;
                        case BLOCK.SAND:
                            blockSprite = Properties.Resources.Box_01;
                            break;
                        case BLOCK.RAW:
                            blockSprite = Properties.Resources.Raw;
                            break;
                    }
                    
                    //Draw Block
                    if ( blockSprite != null )
                        e.DrawImage(blockSprite, BlockCollides[y, x]);

                    //Draw Rect
                    if (iBlocks[y, x] != (int)BLOCK.NONE && Manager.DrawCollider )
                        e.DrawRectangle(new Pen(Color.Green,2), BlockCollides[y,x] );
                }
            }

            //Draw Item
            for (int y = 0; y < SIZEY; y++)
            {
                for (int x = 0; x < SIZEX; x++)
                {

                   

                }
            }



        }

      

    }

    

}
