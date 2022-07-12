using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tetris
{
    class GameScene : ISceneUpdate
    {
        public static Random random = new Random();
        Map map;
        ShapeMover shapeMover;
        Thread inputThread;
        private bool inputThreadRunning;

        public GameScene()
        {
            map = new Map(this);
            map.Draw();
            shapeMover = new ShapeMover();
            shapeMover.CreateRandomShape();
            inputThread = new Thread(InputThreadFunc);
            inputThread.IsBackground = true;
            inputThreadRunning = true;
            inputThread.Start();
        }

        public void Reset()
        {
            map.Reset();
            shapeMover.Reset();
        }

        private void InputThreadFunc()
        {
            while (inputThreadRunning)
            {
                if (Console.KeyAvailable)
                {
                    lock (shapeMover)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.UpArrow:
                                break;
                            case ConsoleKey.DownArrow:
                                if (shapeMover.IsAbleToMove(E_MoveDir.Down, map))
                                {
                                    shapeMover.StepMove(E_MoveDir.Down);
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (shapeMover.IsAbleToMove(E_MoveDir.Left, map))
                                {
                                    shapeMover.StepMove(E_MoveDir.Left);
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                if (shapeMover.IsAbleToMove(E_MoveDir.Right, map))
                                {
                                    shapeMover.StepMove(E_MoveDir.Right);
                                }
                                break;
                            case ConsoleKey.Q:
                                if (shapeMover.IsAbleSwitchState(E_RotateType.CounterClockwise, map))
                                {
                                    shapeMover.SwitchState(E_RotateType.CounterClockwise);
                                }
                                break;
                            case ConsoleKey.E:
                                if (shapeMover.IsAbleSwitchState(E_RotateType.Clockwise, map))
                                {
                                    shapeMover.SwitchState(E_RotateType.Clockwise);
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                while (shapeMover.IsAbleToMove(E_MoveDir.Down, map))
                                {
                                    shapeMover.StepMove(E_MoveDir.Down);
                                }
                                shapeMover.DeliverBricksToMap(map);
                                shapeMover.CreateRandomShape();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void Update()
        {
            lock (shapeMover)
            {
                //map.Draw();
                if (shapeMover.IsAbleToMove(E_MoveDir.Down, map))
                {
                    shapeMover.StepMove(E_MoveDir.Down);
                }
                else
                {
                    shapeMover.DeliverBricksToMap(map);
                    shapeMover.CreateRandomShape();
                }
            }
            Thread.Sleep(400);
        }

        public void StopInputThread()
        {
            inputThreadRunning = false;
            inputThread = null;
        }

    }
}
