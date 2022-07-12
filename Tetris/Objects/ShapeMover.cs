using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    enum E_MoveDir
    {
        Up, Down, Left, Right
    }
    enum E_ShapeType
    {
        Cube, Line, Tank, Left_N, Right_N, Left_L, Right_L
    }
    enum E_RotateType
    {
        Clockwise, CounterClockwise
    }


    class ShapeMover : IDraw
    {
        private Dictionary<E_ShapeType, PositionInfo> posDic = new Dictionary<E_ShapeType, PositionInfo>();
        private Dictionary<E_ShapeType, ConsoleColor> colorDic = new Dictionary<E_ShapeType, ConsoleColor>();
        private Brick[] bricks;
        private int currentState = 0;
        private E_ShapeType currentShapeType;
        private Random random;
        private Position startPos = new Position((Game.width / 4) * 2, -4);

        public ShapeMover()
        {
            posDic = new Dictionary<E_ShapeType, PositionInfo>()
            {
                { E_ShapeType.Cube,new PositionInfo(E_ShapeType.Cube)},
                { E_ShapeType.Line,new PositionInfo(E_ShapeType.Line)},
                { E_ShapeType.Tank,new PositionInfo(E_ShapeType.Tank)},
                { E_ShapeType.Left_N,new PositionInfo(E_ShapeType.Left_N)},
                { E_ShapeType.Right_N,new PositionInfo(E_ShapeType.Right_N)},
                { E_ShapeType.Left_L,new PositionInfo(E_ShapeType.Left_L)},
                { E_ShapeType.Right_L,new PositionInfo(E_ShapeType.Right_L)},
            };

            colorDic = new Dictionary<E_ShapeType, ConsoleColor>()
            {
                { E_ShapeType.Cube,ConsoleColor.Blue},
                { E_ShapeType.Line,ConsoleColor.Magenta},
                { E_ShapeType.Tank,ConsoleColor.Cyan},
                { E_ShapeType.Left_N,ConsoleColor.Yellow},
                { E_ShapeType.Right_N,ConsoleColor.DarkYellow},
                { E_ShapeType.Left_L,ConsoleColor.Green},
                { E_ShapeType.Right_L,ConsoleColor.DarkGreen},
            };
            random = new Random();
        }

        public void Draw()
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i] != null)
                {
                    bricks[i].Draw();
                }
            }
        }

        public void ClearDraw()
        {
            for (int i = 0; i < bricks.Length; i++)
            {
                if (bricks[i] != null)
                {
                    bricks[i].ClearDraw();
                }
            }
        }

        public void Reset()
        {
            ClearDraw();
            CreateRandomShape();
            Draw();
        }

        public void CreateRandomShape()
        {
            CreateShape((E_ShapeType)random.Next(0, 7), startPos);
        }

        public void SwitchState(E_RotateType rotateType)
        {
            switch (rotateType)
            {
                case E_RotateType.Clockwise:
                    if (++currentState >= posDic[currentShapeType].Count)
                    {
                        currentState = 0;
                    }
                    break;
                case E_RotateType.CounterClockwise:
                    if (--currentState < 0)
                    {
                        currentState = posDic[currentShapeType].Count - 1;
                    }
                    break;
                default:
                    break;
            }
            SetState(currentState);
        }

        public bool IsAbleSwitchState(E_RotateType rotateType, Map map)
        {
            int nextState = currentState;
            switch (rotateType)
            {
                case E_RotateType.Clockwise:
                    if (++nextState >= posDic[currentShapeType].Count)
                    {
                        nextState = 0;
                    }
                    break;
                case E_RotateType.CounterClockwise:
                    if (--nextState < 0)
                    {
                        nextState = posDic[currentShapeType].Count - 1;
                    }
                    break;
                default:
                    break;
            }

            Position[] posInfo = posDic[currentShapeType][nextState];

            // 判断是否超出地图边界
            Position temp;
            for (int i = 0; i < posInfo.Length; i++)
            {
                temp = bricks[0].Position + posInfo[i];
                // 判断是否超出地图边界
                if (map.IsPositionOutOfMap(temp))
                {
                    return false;
                }
                // 判断是否与动态方块重合
                if (map.IsPositionInDynamicMap(temp))
                {
                    return false;
                }
            }
            return true;
        }

        public void StepMove(E_MoveDir dir)
        {
            switch (dir)
            {
                case E_MoveDir.Up:
                    break;
                case E_MoveDir.Down:
                    StepMove(new Position(0, 1));
                    break;
                case E_MoveDir.Left:
                    StepMove(new Position(-2, 0));
                    break;
                case E_MoveDir.Right:
                    StepMove(new Position(2, 0));
                    break;
                default:
                    break;
            }
        }

        public bool IsAbleToMove(E_MoveDir dir, Map map)
        {
            Position nextStep;
            switch (dir)
            {
                case E_MoveDir.Up:
                    nextStep = new Position(0, -1);
                    break;
                case E_MoveDir.Down:
                    nextStep = new Position(0, 1);
                    break;
                case E_MoveDir.Left:
                    nextStep = new Position(-2, 0);
                    break;
                case E_MoveDir.Right:
                    nextStep = new Position(2, 0);
                    break;
                default:
                    nextStep = new Position(0, 0);
                    break;
            }
            for (int i = 0; i < bricks.Length; i++)
            {
                if (map.IsPositionOutOfMap(bricks[i].Position + nextStep))
                {
                    return false;
                }
                if (map.IsPositionInDynamicMap(bricks[i].Position + nextStep))
                {
                    return false;
                }
            }
            return true;
        }

        public void DeliverBricksToMap(Map map)
        {
            ChageColor(ConsoleColor.Gray);
            map.AddDynamicBricks(bricks);
        }

        private void CreateShape(E_ShapeType shapeType, Position pos)
        {
            currentShapeType = shapeType;
            currentShapeType = E_ShapeType.Left_L;
            bricks = new Brick[4] { new Brick(0, -5, colorDic[currentShapeType]), new Brick(0, -5, colorDic[currentShapeType]), new Brick(0, -5, colorDic[currentShapeType]), new Brick(0, -5, colorDic[currentShapeType]) };

            // 初始化位置信息
            bricks[0].Position = pos;
            SetState(random.Next(0, posDic[shapeType].Count - 1));
        }

        private void SetState(int index)
        {
            ClearDraw();
            currentState = index;
            Position[] posInfo = posDic[currentShapeType][index];
            for (int i = 1; i < bricks.Length; i++)
            {
                bricks[i].Position = posInfo[i - 1] + bricks[0].Position;
            }
            Draw();
        }

        private void StepMove(Position pos)
        {
            ClearDraw();
            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i].Position = bricks[i].Position + pos;
            }
            Draw();
        }

        private void ChageColor(ConsoleColor color)
        {
            ClearDraw();
            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i].color = color;
            }
            Draw();
        }
    }

}
