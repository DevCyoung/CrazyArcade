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
       
        public const int SIZEX = 17;
        public const int SIZEY = 15;

        public const int TILE_INTER  = 39;
        public const int BLOCK_INTER = 40;

        public const int PIVOTX = 250;
        public const int PIVOTY = 130;

        public int[,] iBlocks;
        public Rectangle[,] BlockCollides;

        public  Map()
        {
           
            BlockCollides  = new Rectangle[SIZEY, SIZEX];
     
            //Block Designe
            iBlocks  = new int[SIZEY, SIZEX]
            {
                {4,  4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4},      // 0
                {4,  1,  1 , 1 , 1 , 0 , 0 , 0 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 1,  4},      // 1
                {4,  1,  2 , 1 , 0 , 3 , 3 , 3 , 1 , 3 , 3 , 3 , 0 , 1 , 2 , 1,  4},      // 2
                {4,  1,  1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 1,  4},      // 3
                {4,  1,  0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 3 , 0 , 0,  4},      // 4
                {4,  1,  0 , 3 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 0,  4},      // 5
                {4,  1,  0 , 3 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 3 , 0 , 0,  4},      // 6
                {4,  1,  0 , 3 , 1 , 1 , 1 , 4 , 4 , 4 , 1 , 1 , 1 , 3 , 0 , 0,  4},      // 7
                {4,  1,  1 , 0 , 3 , 0 , 1 , 1 , 1 , 1 , 1 , 0 , 3 , 0 , 1 , 1,  4},      // 8
                {4,  1,  1 , 1 , 0 , 3 , 1 , 1 , 1 , 1 , 1 , 3 , 0 , 1 , 1 , 1,  4},      // 9
                {4,  1,  1 , 1 , 1 , 0 , 3 , 0 , 1 , 0 , 3 , 0 , 1 , 1 , 1 , 1,  4},      // 10
                {4,  1,  1 , 0 , 1 , 1 , 0 , 3 , 3 , 3 , 0 , 1 , 1 , 0 , 1 , 1,  4},      // 11
                {4,  1,  2 , 0 , 1 , 1 , 1 , 0 , 0 , 0 , 1 , 1 , 1 , 0 , 2 , 1,  4},      // 12
                {4,  1,  0 , 0 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 1 , 0 , 0 , 1,  4},      // 13
                {4,  4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4 , 4,  4},      // 14
            };

            
            for (int y = 0; y < SIZEY; y++)
                for (int x = 0; x < SIZEX; x++)
                    BlockCollides[y, x] = new Rectangle(PIVOTX + x * BLOCK_INTER, PIVOTY + y * BLOCK_INTER, BLOCK_INTER, BLOCK_INTER);
             
        }


        public void Draw(Graphics e)
        {
            int cnt = 0;

            // Draw Tiles
            for (int y = 1; y < SIZEY - 1; y++)
            {
                for (int x = 1; x < SIZEX - 1; x++)
                {
                    if (cnt % 2 == 0 )
                        e.DrawImage(Properties.Resources.Tile2, new Point(PIVOTX+x * TILE_INTER, PIVOTY+y * TILE_INTER));
                    else
                        e.DrawImage(Properties.Resources.Tile4, new Point(PIVOTX + x * TILE_INTER, PIVOTY + y * TILE_INTER));
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

                    //Draw BlockCollides
                    if (iBlocks[y, x] == (int)BLOCK.WALL || iBlocks[y, x] == (int)BLOCK.RAW  && Manager.DrawCollider)
                        e.DrawRectangle(new Pen(Color.Yellow, 2), BlockCollides[y, x]);
                    else if (iBlocks[y, x] != (int)BLOCK.NONE && Manager.DrawCollider )
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
