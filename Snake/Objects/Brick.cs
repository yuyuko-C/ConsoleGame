using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Brick : GameObject
    {
        public Brick(int x, int y)
        {
            position = new Position(x, y);
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("■");
        }
    }
}
