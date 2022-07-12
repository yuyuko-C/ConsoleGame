using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    abstract class GameObject : IGameObjectDraw
    {
        public Position position;

        public abstract void Draw();

        public void SetPosition(int x, int y)
        {
            ClearDraw();
            position.x = x; position.y = y;
        }

        public void ClearDraw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write("  ");
        }
    }
}
