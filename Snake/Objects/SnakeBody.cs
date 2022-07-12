using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    enum E_BodyType
    {
        Head, Body
    }

    class SnakeBody : GameObject
    {
        private E_BodyType bodyType;

        public SnakeBody(int x, int y, E_BodyType bodyType)
        {
            position = new Position(x, y);
            this.bodyType = bodyType;
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = bodyType == E_BodyType.Head ? ConsoleColor.Blue : ConsoleColor.Cyan;
            Console.Write(bodyType == E_BodyType.Head ? "●" : "◎");
        }
    }
}
