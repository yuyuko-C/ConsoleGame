using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Map : IGameObjectDraw
    {
        public Brick[] bricks;

        public Map()
        {
            int length = Game.width + (Game.height - 3) * 2;
            bricks = new Brick[length];

            int index = 0, lastIndex = length - 1;
            for (int i = 0; i < Game.width; i += 2)
            {
                bricks[index++] = new Brick(i, 0);
                bricks[lastIndex--] = new Brick(i, Game.height - 2);
            }
            for (int i = 1; i < Game.height - 2; i++)
            {
                bricks[index++] = new Brick(0, i);
                bricks[lastIndex--] = new Brick(Game.width - 2, i);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i] != null)
                { bricks[i].Draw(); }
            }
        }


        public bool IsPositionInMap(int x, int y)
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i].position.x == x && bricks[i].position.y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPositionInMap(Position pos)
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i].position == pos)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
