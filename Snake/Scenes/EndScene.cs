using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class EndScene : UISceneBase
    {
        public EndScene()
        {
            title = "游戏结束";
            text1 = "回到开始界面";
            text2 = "退出游戏";
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
