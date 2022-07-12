using System;
#nullable enable


namespace AeroPlane_Chess
{
    enum E_SceneType { Begin, Game, End }
    enum E_GridType { Normal, Pause, Bomb, Tunnel }
    enum E_PlayerType { Human, Computer }
    enum E_GameFlow { HumanRound, ComputerRound, Finish }


    struct GridPoint
    {
        private int x, y;

        public GridPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Draw(string icon, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(icon);
        }

        public void Clear()
        {
            Console.SetCursorPosition(x, y);
            Console.Write("  ");
        }
    }

    struct ChessMap
    {
        public GridPoint[] grids;
        public E_GridType[] gridTypes;
        public int gridNumber;
        public int leftX, leftY;

        public ChessMap(int leftX, int leftY, int number)
        {
            grids = new GridPoint[number];
            gridTypes = new E_GridType[number];
            this.gridNumber = number;
            this.leftX = leftX; this.leftY = leftY;

            InitGridType();
            InitGridPosition();

        }

        private void InitGridPosition()
        {
            int stepX = 2, indexX = 0, indexY = 0;

            for (int i = 0; i < gridNumber; i++)
            {
                grids[i] = new GridPoint(leftX, leftY);
                if (indexX == 15)
                {
                    leftY++;
                    indexY++;
                    if (indexY == 2)
                    {
                        indexX = 0;
                        indexY = 0;
                        stepX = -stepX;
                    }
                }
                else
                {
                    leftX += stepX;
                    indexX++;
                }

            }
        }

        private void InitGridType()
        {
            Random random = new Random();
            #region 通过概率控制棋盘
            int randomNum = 0;
            for (int i = 0; i < gridNumber; i++)
            {
                randomNum = random.Next(0, 101);
                if (randomNum < 60 || i == 0 || i == gridNumber - 1)
                {
                    gridTypes[i] = E_GridType.Normal;
                }
                else if (randomNum < 80)
                {
                    gridTypes[i] = E_GridType.Bomb;
                }
                else if (randomNum < 90)
                {
                    gridTypes[i] = E_GridType.Tunnel;
                }
                else
                {
                    gridTypes[i] = E_GridType.Pause;
                }
            }
            #endregion

            return;

            #region 通过数量控制棋盘
            int bombNum = random.Next(3, 5);
            int pasueNum = random.Next(5, 10);
            int tunnelNum = random.Next(3, 8);
            // 记录棋盘格是否被使用
            bool[] usedGrid = new bool[gridNumber];
            // 初始化棋盘为普通格子
            for (int i = 0; i < gridNumber; i++)
            {
                gridTypes[i] = E_GridType.Normal;
            }

            // 生成炸弹格子
            for (int i = 0; i < bombNum; i++)
            {
                int index = random.Next(0, gridNumber);
                while (usedGrid[index] || index == 0 || index == gridNumber - 1)
                {
                    index = random.Next(0, gridNumber);
                }
                gridTypes[index] = E_GridType.Bomb;
                usedGrid[index] = true;
            }
            // 生成暂停格子
            for (int i = 0; i < pasueNum; i++)
            {
                int index = random.Next(0, gridNumber);
                while (usedGrid[index] || index == 0 || index == gridNumber - 1)
                {
                    index = random.Next(0, gridNumber);
                }
                gridTypes[index] = E_GridType.Pause;
                usedGrid[index] = true;
            }
            // 生成隧道格子
            for (int i = 0; i < tunnelNum; i++)
            {
                int index = random.Next(0, gridNumber);
                while (usedGrid[index] || index == 0 || index == gridNumber - 1)
                {
                    index = random.Next(0, gridNumber);
                }
                gridTypes[index] = E_GridType.Tunnel;
                usedGrid[index] = true;
            }

            #endregion
        }

        public void Draw()
        {
            for (int i = 0; i < gridNumber; i++)
            {
                switch (gridTypes[i])
                {
                    case E_GridType.Normal:
                        grids[i].Draw("□", ConsoleColor.White);
                        break;
                    case E_GridType.Pause:
                        grids[i].Draw("‖", ConsoleColor.White);
                        break;
                    case E_GridType.Bomb:
                        grids[i].Draw("●", ConsoleColor.Red);
                        break;
                    case E_GridType.Tunnel:
                        grids[i].Draw("¤", ConsoleColor.Yellow);
                        break;
                }
            }
        }
    }

    struct Player
    {
        public E_PlayerType type;
        public int index;
        public bool isPause;
        public string name;

        public Player(int index, E_PlayerType type)
        {
            this.index = index;
            this.type = type;
            isPause = false;
            name = type == E_PlayerType.Human ? "玩家" : "电脑";
        }

        public void Draw(ChessMap map, int otherIndex)
        {
            if (index == otherIndex)
            {
                map.grids[index].Draw("Ｄ", ConsoleColor.Magenta);
            }
            else
            {
                if (type == E_PlayerType.Human)
                {
                    map.grids[index].Draw("Ｐ", ConsoleColor.Blue);
                }
                else
                {
                    map.grids[index].Draw("Ｃ", ConsoleColor.Green);
                }
            }
        }


    }

    struct World
    {
        private Random random;

        public int ThrowRoll(int min, int max)
        {
            if (random == null)
            {
                random = new Random();
            }
            return random.Next(min, max + 1);
        }

        public void Bomb(ref Player player)
        {
            player.index = Math.Max(0, player.index -= 5);
        }

        public void Pause(ref Player player)
        {
            player.isPause = true;
        }

        public void Exchange(ref Player a, ref Player b)
        {
            int temporaryIndex = a.index;
            a.index = b.index;
            b.index = temporaryIndex;
        }

        public void TriggerPoint(ChessMap map, ref Player player, ref Player other, ref MessageBox msgbox)
        {
            string msg;
            bool isBreak = false;
            while (true)
            {

                switch (map.gridTypes[player.index])
                {
                    case E_GridType.Normal:
                        msg = string.Format("{0}飞行到安全地点", player.name);
                        isBreak = true;
                        break;
                    case E_GridType.Pause:
                        Pause(ref player);
                        msg = string.Format("{0}飞行到休息点,暂停一回合", player.name);
                        isBreak = true;
                        break;
                    case E_GridType.Bomb:
                        Bomb(ref player);
                        msg = string.Format("{0}飞行到防空点,倒退5格", player.name);
                        break;
                    case E_GridType.Tunnel:
                        msg = string.Format("{0}进入时空隧道,随机执行一种", player.name);
                        msgbox.AppendLine(msg, ConsoleColor.White);
                        int randomNum = ThrowRoll(1, 91);
                        if (randomNum < 30)
                        {
                            Pause(ref player);
                            msg = string.Format("{0}随机到了休息点,暂停一回合", player.name);
                            isBreak = true;
                        }
                        else if (randomNum < 60)
                        {
                            Exchange(ref player, ref other);
                            msg = string.Format("{0}随机到了交换位置", player.name);
                        }
                        else
                        {
                            Bomb(ref player);
                            msg = string.Format("{0}随机到了防空点,倒退5格", player.name);
                        }
                        break;
                    default:
                        msg = "";
                        break;
                }

                msgbox.AppendLine(msg, ConsoleColor.White);
                if (isBreak)
                {
                    msgbox.AppendLine("按任意键继续", ConsoleColor.White);
                    break;
                }

            }
        }

        public bool Move(ChessMap map, int step, ref Player player, ref Player other, ref MessageBox msgbox)
        {
            msgbox.Clear();

            string msg;
            if (player.isPause)
            {
                msg = string.Format("{0}暂停中,按任意键继续", player.name);
                msgbox.Write(msg, 0, ConsoleColor.White, true);
                player.isPause = false;
                return false;
            }
            msg = string.Format("{0}投掷出了{1}点", player.name, step);
            msgbox.Write(msg, 0, ConsoleColor.White, true);


            player.index = Math.Min(map.gridNumber - 1, player.index += step);
            if (player.index == map.gridNumber - 1)
            {
                msg = string.Format("{0}到达终点,游戏结束.按任意键继续", player.name);
                msgbox.Write(msg, 1, ConsoleColor.White, true);
                return true;
            }
            else
            {
                TriggerPoint(map, ref player, ref other, ref msgbox);
            }
            return false;
        }

    }

    struct MessageBox
    {
        //width 与 height 表示可以有多少中文字符
        private int startRow, width, height;
        private int currentRow;
        private string blankRow;
        private string[] msgRows;

        public MessageBox(int startRow, int width, int height)
        {
            this.startRow = startRow;
            this.width = width;
            this.height = height;
            this.currentRow = 0;
            // 初始化空行
            this.blankRow = "";
            for (int i = 0; i < width; i++)
            {
                this.blankRow += "  ";
            }
            // 所有行填充空行
            this.msgRows = new string[height];
            for (int i = 0; i < height; i++)
            {
                this.msgRows[i] = blankRow;
            }
        }

        public void Write(string message, int row, ConsoleColor color, bool multiRow = false)
        {
            if (row >= height)
            {
                return;
            }

            Clear();

            // 拆分段落
            string[] messages = message.Split("\n");
            int paragraphCount = messages.Length;

            for (int i = 0; i < paragraphCount; i++)
            {
                if (multiRow)
                {
                    // 一段多行的情况
                    int needRowCount = (int)Math.Ceiling(messages[i].Length * 1f / width);
                    for (int j = 0; j < needRowCount; j++)
                    {
                        if (messages[i].Length > width)
                        {
                            AppendLine(message.Substring(width * j, width * (j + 1)), color);
                        }
                        else
                        {
                            AppendLine(message, color);
                        }
                    }
                }
                else
                {
                    WriteLine(message, i, color);
                }
            }
        }

        //只能显示在某一行
        public void WriteLine(string message, int row, ConsoleColor color)
        {
            if (row >= height)
            {
                return;
            }

            ClearLine(row);
            Console.ForegroundColor = color;
            SetCurrentRow(row);
            int returnIndex = message.IndexOf('\n');
            message = returnIndex != -1 ? message.Substring(0, returnIndex) : message;
            msgRows[row] = message.Length > width ? message.Substring(0, width) : message;
            Console.Write(msgRows[row]);
            currentRow = Math.Min(height - 1, ++row);
        }

        public void AppendLine(string message, ConsoleColor color)
        {
            if (currentRow + 1 < height)
            {
                WriteLine(message, currentRow, color);
            }
            else
            {
                // 整体上移
                for (int i = 0; i < currentRow; i++)
                {
                    ClearLine(i);
                    WriteLine(msgRows[i + 1], i, color);
                }
                WriteLine(message, currentRow, color);
            }
        }

        public void ClearLine(int row)
        {
            if (row >= height)
            {
                return;
            }
            msgRows[row] = blankRow;
            SetCurrentRow(row);
            Console.Write(msgRows[row]);
        }

        public void Clear()
        {
            for (int i = 0; i < height; i++)
            {
                ClearLine(i);
            }
            SetCurrentRow(0);
        }

        public void SetCurrentRow(int row = 0)
        {
            Console.SetCursorPosition(2, startRow + row);
            currentRow = row;
        }

    }


    class Program
    {
        static void BeginSceneInit(int width, int height)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            string gameName = "飞行棋";
            Console.SetCursorPosition((int)(width * 0.5f) - gameName.Length, (int)(height * 0.3f));
            Console.Write(gameName);
        }

        static void BeiginScenLoop(int width, int height, ref E_SceneType sceneType)
        {
            bool exitScene = false;
            int selectIndex = 0;
            string selection1 = "开始游戏";
            string selection2 = "退出游戏";

            //开始界面的循环
            while (true)
            {
                Console.ForegroundColor = selectIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.SetCursorPosition((int)(width * 0.5f) - selection1.Length, (int)(height * 0.3f) + 5);
                Console.Write(selection1);
                Console.ForegroundColor = selectIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.SetCursorPosition((int)(width * 0.5f) - selection2.Length, (int)(height * 0.3f) + 7);
                Console.Write(selection2);


                if (exitScene)
                {
                    break;
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        selectIndex = Math.Max(0, --selectIndex);
                        break;
                    case ConsoleKey.S:
                        selectIndex = Math.Min(1, ++selectIndex);
                        break;
                    case ConsoleKey.J:
                        if (selectIndex == 0)
                        {
                            sceneType = E_SceneType.Game;
                            exitScene = true;
                        }
                        else if (selectIndex == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }

        }


        static void GameSceneInit(int width, int height)
        {
            Console.Clear();
            string wallIcon = "■";
            ConsoleColor wallColor = ConsoleColor.Red;

            #region 初始化墙体

            Console.ForegroundColor = wallColor;
            for (int i = 0; i < height; i++)
            {
                //左侧墙体
                Console.SetCursorPosition(0, i);
                Console.Write(wallIcon);

                //右侧墙体
                Console.SetCursorPosition(width - 2, i);
                Console.Write(wallIcon);
            }

            int infoHead = (int)(height * 0.6f);
            int infoBottom = (int)(height * 0.8f);
            for (int i = 0; i < width; i += 2)
            {
                //顶部
                Console.SetCursorPosition(i, 0);
                Console.Write(wallIcon);

                //中部
                Console.SetCursorPosition(i, infoHead);
                Console.Write(wallIcon);

                //中部
                Console.SetCursorPosition(i, infoBottom);
                Console.Write(wallIcon);

                //底部
                Console.SetCursorPosition(i, height);
                Console.Write(wallIcon);

            }

            #endregion

            #region 初始化提示信息

            int helpStartRow = infoHead + 1;

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(2, helpStartRow);
            Console.Write("游戏帮助");
            Console.SetCursorPosition(2, helpStartRow + 2);
            Console.Write("□:普通格子");
            Console.SetCursorPosition(2 + 6 * 2 + 6, helpStartRow + 2);
            Console.Write("●:炸弹,倒退5格");
            Console.SetCursorPosition(2, helpStartRow + 3);
            Console.Write("‖:休息点,暂停一回合");
            Console.SetCursorPosition(2, helpStartRow + 4);
            Console.Write("¤:时空隧道,随机暂停/炸弹/交换位置");
            Console.SetCursorPosition(2, helpStartRow + 5);
            Console.Write("Ｐ:玩家  Ｃ:电脑   Ｄ:玩家和电脑重合");


            Console.SetCursorPosition(2, infoBottom + 1);
            Console.Write("按任意键投掷骰子");

            #endregion
        }

        static void GameSceneLoop(int width, int height, ref E_SceneType sceneType)
        {
            World world = new World();
            E_GameFlow gameFlow = E_GameFlow.HumanRound;
            ChessMap map = new ChessMap(8, 3, 150);
            MessageBox msgBox = new MessageBox((int)(height * 0.8f) + 1, 23, 7);
            Player human = new Player(0, E_PlayerType.Human);
            Player computer = new Player(0, E_PlayerType.Computer);


            while (true)
            {
                // 画面更新逻辑
                map.Draw();
                human.Draw(map, computer.index);
                computer.Draw(map, human.index);

                Console.ReadKey(true);

                // 游戏退出逻辑
                if (gameFlow == E_GameFlow.Finish)
                {
                    sceneType = E_SceneType.End;
                    break;
                }

                // 流程逻辑
                int rollPoint = world.ThrowRoll(1, 6);
                switch (gameFlow)
                {
                    case E_GameFlow.HumanRound:
                        gameFlow = world.Move(map, rollPoint, ref human, ref computer, ref msgBox) ? E_GameFlow.Finish : E_GameFlow.ComputerRound;
                        break;
                    case E_GameFlow.ComputerRound:
                        gameFlow = world.Move(map, rollPoint, ref computer, ref human, ref msgBox) ? E_GameFlow.Finish : E_GameFlow.HumanRound;
                        break;
                    case E_GameFlow.Finish:
                        break;
                }
            }

        }


        static void EndSceneInit(int width, int height)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            string gameName = "游戏结束";
            Console.SetCursorPosition((int)(width * 0.5f) - gameName.Length, (int)(height * 0.3f));
            Console.Write(gameName);
        }

        static void EndScenLoop(int width, int height, ref E_SceneType sceneType)
        {
            bool exitScene = false;
            int selectIndex = 0;
            string selection1 = "重新开始";
            string selection2 = "退出游戏";

            //结束界面的循环
            while (true)
            {
                Console.ForegroundColor = selectIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.SetCursorPosition((int)(width * 0.5f) - selection1.Length, (int)(height * 0.3f) + 5);
                Console.Write(selection1);
                Console.ForegroundColor = selectIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.SetCursorPosition((int)(width * 0.5f) - selection2.Length, (int)(height * 0.3f) + 7);
                Console.Write(selection2);


                if (exitScene)
                {
                    break;
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        selectIndex = Math.Max(0, --selectIndex);
                        break;
                    case ConsoleKey.S:
                        selectIndex = Math.Min(1, ++selectIndex);
                        break;
                    case ConsoleKey.J:
                        if (selectIndex == 0)
                        {
                            Console.Clear();
                            sceneType = E_SceneType.Game;
                            exitScene = true;
                        }
                        else if (selectIndex == 1)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }
            }

        }



        static void Main(string[] args)
        {
            E_SceneType currentScene = E_SceneType.Begin;


            int width = 50, height = 40;

            Console.CursorVisible = false;
            // 设置窗口大小
            Console.SetWindowSize(width, height + 2);
            Console.SetBufferSize(width, height + 2);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(936);
            

            while (true)
            {
                switch (currentScene)
                {
                    case E_SceneType.Begin:
                        BeginSceneInit(width, height);
                        BeiginScenLoop(width, height, ref currentScene);
                        break;
                    case E_SceneType.Game:
                        GameSceneInit(width, height);
                        GameSceneLoop(width, height, ref currentScene);
                        break;
                    case E_SceneType.End:
                        EndSceneInit(width, height);
                        EndScenLoop(width, height, ref currentScene);
                        break;
                }
            }
        }
    }
}
