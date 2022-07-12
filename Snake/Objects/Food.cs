using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Food : GameObject
    {
        public Food(int x, int y)
        {
            position = new Position(x, y);
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("★");
        }

        public void RandomPosition(Snake snake)
        {
            int x = GameScene.random.Next(2, Game.width / 2 - 1);
            int y = GameScene.random.Next(1, Game.height - 2);

            if (snake.IsPositionInBody(x, y, true))
            {
                RandomPosition(snake);
            }
            else
            {
                position.x = 2 * x; position.y = y;
            }
        }
    }
}
