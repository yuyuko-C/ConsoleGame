using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    abstract class UISceneBase : ISceneUpdate
    {
        protected string title;
        protected string option1, option2;

        protected int currentSelect = 0;
        protected int titleRow = 5;

        public UISceneBase(int titleRow)
        {
            this.titleRow = titleRow;
        }

        protected abstract void KeyJDown();

        public void Update()
        {
            // 设置颜色
            Console.ForegroundColor = ConsoleColor.White;
            // 显示标题
            Console.SetCursorPosition(Game.width / 2 - title.Length, titleRow);
            Console.Write(title);

            // 显示选项
            Console.SetCursorPosition(Game.width / 2 - option1.Length, titleRow + 3);
            Console.ForegroundColor = currentSelect == 0 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(option1);
            Console.SetCursorPosition(Game.width / 2 - option2.Length, titleRow + 5);
            Console.ForegroundColor = currentSelect == 1 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(option2);

            // 检测输入
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    --currentSelect;
                    if (currentSelect < 0)
                    { currentSelect = 0; }
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    ++currentSelect;
                    if (currentSelect > 1)
                    { currentSelect = 1; }
                    break;
                case ConsoleKey.J:
                    KeyJDown();
                    break;
                default:
                    break;
            }
        }
    }
}
