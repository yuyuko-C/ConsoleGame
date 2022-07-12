using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class GameScene : ISceneUpdate
    {
        public static Random random = new Random();
        Map map;
        Snake snake;
        Food food;

        int frameCount = 0;

        public GameScene()
        {
            map = new Map();

            snake = new Snake(Game.width / 2, Game.height / 2);
            snake.Draw();

            food = new Food(2, 2);
            food.RandomPosition(snake);
            food.Draw();
        }

        public void Reset()
        {
            snake.Reset();
            food.RandomPosition(snake);

            map.Draw();
            snake.Draw();
            food.Draw();
        }

        public void Update()
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        snake.SetCrawMoveDir(E_MoveDir.Up);
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        snake.SetCrawMoveDir(E_MoveDir.Down);
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        snake.SetCrawMoveDir(E_MoveDir.Left);
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        snake.SetCrawMoveDir(E_MoveDir.Right);
                        break;

                    default:
                        break;
                }
            }

            frameCount++;


            if (frameCount % 5555 == 0)
            {
                snake.CrawlMove();
                snake.Draw();
                // 判断结束逻辑必须放在移动逻辑下,不然第一节身体出现会直接判断失败
                //因为蛇在移动前,身体和头处于同一位置
                if (IsGameFail())
                {
                    Game.SwithToScene(E_SceneType.End);
                }

                if (IsSnakeEatFood())
                {
                    //添加长度必须在食物随机位置之前,不然会巧合的擦除掉食物的显示
                    snake.AddLength();
                    food.RandomPosition(snake);
                    //放里面可以增加性能
                    food.Draw();
                }

                frameCount = 0;
            }





        }

        private bool IsGameFail()
        {
            if (map.IsPositionInMap(snake.HeadPosition) || snake.IsPositionInBody(snake.HeadPosition, false))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsSnakeEatFood()
        {
            return snake.HeadPosition == food.position;
        }

    }
}
