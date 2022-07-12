using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    struct Position
    {
        public int x;
        public int y;


        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
            {
                return false;
            }

            return true;
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.x + p2.x, p1.y + p2.y);
        }
    }
}
