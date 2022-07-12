using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    abstract class GameObject : IDraw
    {
        protected Position position;

        public abstract void Draw();
        public abstract void ClearDraw();

        public void SetPosition(int x, int y)
        {
            position.x = x; position.y = y;
        }

        public void SetPosition(Position pos)
        {
            position.x = pos.x; position.y = pos.y;
        }

        public Position Position
        {
            get => position;
            set => position = value;
        }

    }
}
