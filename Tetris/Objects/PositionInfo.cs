using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class PositionInfo
    {
        private List<Position[]> info = new List<Position[]>();

        public PositionInfo(E_ShapeType shapeType)
        {
            switch (shapeType)
            {
                case E_ShapeType.Cube:
                    info.Add(new Position[] { new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_ShapeType.Line:
                    info.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(4, 0) });
                    info.Add(new Position[] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
                    break;
                case E_ShapeType.Tank:
                    info.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(2, 0) });
                    info.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(0, 1) });
                    break;
                case E_ShapeType.Left_N:
                    info.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(-2, 1) });
                    info.Add(new Position[] { new Position(-2, -1), new Position(0, -1), new Position(2, 0) });
                    info.Add(new Position[] { new Position(2, -1), new Position(2, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(-2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_ShapeType.Right_N:
                    info.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(2, 1) });
                    info.Add(new Position[] { new Position(2, 0), new Position(-2, 1), new Position(0, 1) });
                    info.Add(new Position[] { new Position(-2, -1), new Position(-2, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(0, -1), new Position(2, -1), new Position(-2, 0) });
                    break;
                case E_ShapeType.Left_L:
                    info.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(4, 0) });
                    info.Add(new Position[] { new Position(2, 0), new Position(0, 1), new Position(0, 2) });
                    info.Add(new Position[] { new Position(-4, 0), new Position(-2, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(0, -2), new Position(0, -1), new Position(-2, 0) });
                    break;
                case E_ShapeType.Right_L:
                    info.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(-4, 0) });
                    info.Add(new Position[] { new Position(0, -2), new Position(0, -1), new Position(2, 0) });
                    info.Add(new Position[] { new Position(2, 0), new Position(4, 0), new Position(0, 1) });
                    info.Add(new Position[] { new Position(-2, 0), new Position(0, 1), new Position(0, 2) });
                    break;
                default:
                    break;
            }
        }

        public Position[] this[int index]
        {
            get
            {
                if (index < 0) index = 0;
                else if (index >= info.Count) index = info.Count - 1;
                return info[index];
            }
        }

        public int Count { get => info.Count; }
    }
}
