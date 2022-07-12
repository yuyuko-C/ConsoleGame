using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    abstract class UISceneBase : ISceneUpdate
    {
        protected string title;
        protected string text1, text2;

        protected int currentSelect = 0;

        protected abstract void KeyJDown();

        public void Update()
        {
            // 设置颜色
            Console.ForegroundColor = ConsoleColor.White;
            // 显示标题
            Console.SetCursorPosition(Game.width / 2 - title.Length, 5);
            Console.Write(title);

            // 显示选项
            Console.SetCursorPosition(Game.width / 2 - text1.Length, 8);
            Console.ForegroundColor = currentSelect == 0 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(text1);
            Console.SetCursorPosition(Game.width / 2 - text2.Length, 9);
            Console.ForegroundColor = currentSelect == 1 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write(text2);


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
