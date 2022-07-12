using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    enum E_MoveDir { Up, Down, Left, Right }


    class Snake : IGameObjectDraw
    {
        public int currentLength;
        public E_MoveDir currentDir = E_MoveDir.Right;
        public Position HeadPosition;

        private SnakeBody[] bodys;



        public Snake(int x, int y)
        {
            bodys = new SnakeBody[200];
            bodys[0] = new SnakeBody(x, y, E_BodyType.Head);
            currentLength = 1;
            HeadPosition = new Position(x, y);
        }

        public void Draw()
        {
            for (int i = 0; i < currentLength; i++)
            {
                bodys[i].Draw();
            }
        }

        /// <summary>
        /// 设置蛇头的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetHeadPosition(int x, int y)
        {
            // 设置位置
            for (int i = currentLength - 1; i > 0; i--)
            {
                Position pos = bodys[i - 1].position;
                bodys[i].SetPosition(pos.x, pos.y);
            }

            bodys[0].SetPosition(x, y);
            HeadPosition.x = x;
            HeadPosition.y = y;
        }


        /// <summary>
        /// 检查点是否在蛇身子里
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsPositionInBody(int x, int y, bool includeHead)
        {

            for (int i = includeHead ? 0 : 1; i < currentLength; i++)
            {
                Position pos = bodys[i].position;
                if (pos.x == x && pos.y == y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查点是否在蛇身子里
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsPositionInBody(Position pos, bool includeHead)
        {
            for (int i = includeHead ? 0 : 1; i < currentLength; i++)
            {
                if (bodys[i].position == pos)
                {
                    return true;
                }
            }
            return false;
        }


        public void SetCrawMoveDir(E_MoveDir dir)
        {
            // 互斥方向
            if (currentLength > 1)
            {
                if (currentDir == E_MoveDir.Up && dir == E_MoveDir.Down
                    || currentDir == E_MoveDir.Down && dir == E_MoveDir.Up
                    || currentDir == E_MoveDir.Left && dir == E_MoveDir.Right
                    || currentDir == E_MoveDir.Right && dir == E_MoveDir.Left
                    )
                {
                    return;
                }
            }
            currentDir = dir;
        }


        public void CrawlMove()
        {
            switch (currentDir)
            {
                case E_MoveDir.Up:
                    SetHeadPosition(HeadPosition.x, HeadPosition.y - 1);
                    break;
                case E_MoveDir.Down:
                    SetHeadPosition(HeadPosition.x, HeadPosition.y + 1);
                    break;
                case E_MoveDir.Left:
                    SetHeadPosition(HeadPosition.x - 2, HeadPosition.y);
                    break;
                case E_MoveDir.Right:
                    SetHeadPosition(HeadPosition.x + 2, HeadPosition.y);
                    break;
                default:
                    break;
            }
        }


        public void AddLength(int count = 1)
        {
            Position pos = bodys[currentLength - 1].position;
            bodys[currentLength++] = new SnakeBody(pos.x, pos.y, E_BodyType.Body);
        }


        public void Reset()
        {
            for (int i = currentLength - 1; i > 0; i--)
            {
                bodys[i] = null;
            }
            currentLength = 1;
            SetHeadPosition(Game.width / 2, Game.height / 2);
            currentDir = E_MoveDir.Right;
        }

    }
}
