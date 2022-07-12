using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class EndScene : UISceneBase
    {
        public EndScene(int titleRow) : base(titleRow)
        {
            title = "游戏结束";
            option1 = "回到开始界面";
            option2 = "退出游戏";
        }


        protected override void KeyJDown()
        {
            if (currentSelect == 0)
            {
                Game.SwithToScene(E_SceneType.Begin);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
