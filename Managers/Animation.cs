using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crazy_Arcade.Managers
{
    class Animation
    {

        int size;
        int curDelay;
        int nextDelay;
        int sprIndex;
        Image[][] animations;
        Image[] animation;

        public Animation(  int size  , int delay , int nextDelay)
        {

            this.curDelay = 3;
            this.size = size;
            this.nextDelay = nextDelay;
            sprIndex = 0;

            animations = new Image[size][];

        }


        public void Update(int deltatime)
        {

            curDelay += deltatime;
            if (curDelay >= nextDelay)
            {
                ++sprIndex;
            }



        }
        
    }
}
