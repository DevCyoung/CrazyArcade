using System;
using System.Drawing;

namespace Crazy_Arcade.Helper
{
    class MathHelper
    {
        public static bool IsCollide(Rectangle r1, Rectangle r2)
        {
            bool check = false;
            if (r1.Left < r2.Right && r1.Right > r2.Left && r1.Top < r2.Bottom && r1.Bottom > r2.Top)
                check = true;
            return check;
        }
        public static double CollideDistance(Rectangle r1, Rectangle r2)
        {
            double r1X = r1.X + r1.Width / 2;
            double r1Y = r1.Y - r1.Height / 2;
            double r2X = r2.X + r2.Width / 2;
            double r2Y = r2.Y - r2.Height / 2;
            double distance = Math.Pow(r1X - r2X, 2) + Math.Pow(r1Y - r2Y, 2);
            distance = Math.Sqrt(distance);
            return distance;
        }
    }
}
