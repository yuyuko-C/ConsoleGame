using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class BeginScene : UISceneBase
    {
        public BeginScene()
        {
            title = "贪吃蛇";
            text1 = "开始游戏";
            text2 = "退出游戏";
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
