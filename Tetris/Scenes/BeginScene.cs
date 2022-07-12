using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class BeginScene : UISceneBase
    {
        public BeginScene(int titleRow) : base(titleRow)
        {
            title = "俄罗斯方块";
            option1 = "开始游戏";
            option2 = "退出游戏";
        }


        protected override void KeyJDown()
        {
            if (currentSelect == 0)
            {
                Game.SwithToScene(E_SceneType.Game);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
