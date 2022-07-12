using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Brick : GameObject
    {
        public ConsoleColor color = ConsoleColor.Red;

        public Brick(int x, int y)
        {
            Position = new Position(x, y);
        }
        public Brick(int x, int y, ConsoleColor color) : this(x, y)
        {
            this.color = color;
        }

        public override void Draw()
        {
            if (!(Position.x < 0 || Position.x >= Game.width || Position.y < 0 || Position.y >= Game.height))
            {
                Console.SetCursorPosition(Position.x, Position.y);
                Console.ForegroundColor = color;
                Console.Write("■");
            }
        }
        public override void ClearDraw()
        {
            if (!(position.x < 0 || position.x >= Game.width || position.y < 0 || position.y >= Game.height))
            {
                Console.SetCursorPosition(position.x, position.y);
                Console.Write("  ");
            }
        }
    }
}
