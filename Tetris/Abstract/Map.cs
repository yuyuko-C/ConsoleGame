using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Map : IDraw
    {
        private List<Brick> staticBricks;
        private List<Brick>[] dynamicBricks;
        private GameScene gameScene;
        private uint score;
        private string blankRow;

        public int width, height;

        public Map(GameScene gameScene)
        {
            this.gameScene = gameScene;
            staticBricks = new List<Brick>();
            height = Game.height - 6;
            dynamicBricks = new List<Brick>[height - 1];
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                dynamicBricks[i] = new List<Brick>();
            }

            for (int i = 0; i < Game.width; i += 2)
            {
                staticBricks.Add(new Brick(i, height));
                width++;
            }
            for (int i = 0; i < height; i++)
            {
                staticBricks.Add(new Brick(0, i));
                staticBricks.Add(new Brick(Game.width - 2, i));
            }

            blankRow = new string(' ', (width - 2) * 2);
        }

        public void Draw()
        {
            for (int i = 0; i < staticBricks.Count; i++)
            {
                if (staticBricks[i] != null)
                { staticBricks[i].Draw(); }
            }
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                for (int j = 0; j < dynamicBricks[i].Count; j++)
                {
                    if (dynamicBricks[i][j] != null)
                    { dynamicBricks[i][j].Draw(); }
                }
            }

            Console.SetCursorPosition(2, height + 1);
            Console.Write(blankRow);
            Console.SetCursorPosition(2, height + 1);
            Console.Write("得分:{0}", score);
        }

        public void ClearDraw()
        {
            for (int i = 0; i < staticBricks.Count; i++)
            {
                if (staticBricks[i] != null)
                { staticBricks[i].ClearDraw(); }
            }
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                for (int j = 0; j < dynamicBricks[i].Count; j++)
                {
                    if (dynamicBricks[i][j] != null)
                    { dynamicBricks[i][j].ClearDraw(); }
                }
            }

        }

        public bool IsPositionOutOfMap(Position pos)
        {
            return pos.x < 2 || pos.x >= Game.width - 2 || pos.y >= height;
        }

        public bool IsPositionInStaticMap(Position pos)
        {
            for (int i = 0; i < staticBricks.Count; i++)
            {
                if (staticBricks[i].Position == pos)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPositionInDynamicMap(Position pos)
        {
            int row = height - pos.y - 1;
            if (row >= dynamicBricks.Length)
            {
                return false;
            }
            for (int i = 0; i < dynamicBricks[row].Count; i++)
            {
                if (dynamicBricks[row][i].Position == pos)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddDynamicBricks(Brick[] bricks)
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                // 将方块加入到对应的行
                int row = height - bricks[i].Position.y - 1;
                if (row >= dynamicBricks.Length)
                {
                    gameScene.StopInputThread();
                    Game.SwithToScene(E_SceneType.End);
                    return;
                }
                List<Brick> brickRow = dynamicBricks[row];
                brickRow.Add(bricks[i]);

            }
            ClearFullLine();
            Draw();
        }

        public void Reset()
        {
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                for (int j = 0; j < dynamicBricks[i].Count; j++)
                {
                    dynamicBricks[i][j].ClearDraw();
                }
                dynamicBricks[i].Clear();
            }
            score = 0;
            Draw();

        }


        private void ClearOneLine(int index)
        {
            List<Brick> fullLine = dynamicBricks[index];

            // 清空动态方块的显示
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                for (int j = 0; j < dynamicBricks[i].Count; j++)
                {
                    if (dynamicBricks[i][j] != null)
                    { dynamicBricks[i][j].ClearDraw(); }
                }
            }
            // 清空这满的一行
            fullLine.Clear();

            // 将上面的位置改变并改变列表位置
            for (int j = index + 1; j < dynamicBricks.Length; j++)
            {
                // 改变这些list内方块的位置
                for (int x = 0; x < dynamicBricks[j].Count; x++)
                {
                    dynamicBricks[j][x].Position = dynamicBricks[j][x].Position + new Position(0, 1);
                }
                // 然后把所有list的序号往前滚动
                dynamicBricks[j - 1] = dynamicBricks[j];
            }
            // 满的一行填补到最后一个位置
            dynamicBricks[dynamicBricks.Length - 1] = fullLine;
        }

        private void ClearFullLine()
        {
            for (int i = 0; i < dynamicBricks.Length; i++)
            {
                // 检查行是否已经满了
                if (dynamicBricks[i].Count == width - 2)
                {
                    score += 10;
                    ClearOneLine(i);
                    ClearFullLine();
                }
            }
        }


    }
}
