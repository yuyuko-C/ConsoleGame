using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Snake
{
    enum E_SceneType
    {
        Begin, Game, End
    }

    class Game
    {
        public const int width=80, height=20;
        private static ISceneUpdate nowScene;
        private static BeginScene beginScene;
        private static GameScene gameScene;
        private static EndScene endScene;

        public Game()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(936);

            beginScene = new BeginScene();
            gameScene = new GameScene();
            endScene = new EndScene();

            SwithToScene(E_SceneType.Begin);
        }

        public void Start()
        {
            while (true)
            {
                if (nowScene != null)
                {
                    nowScene.Update();
                }
            }
        }

        public static void SwithToScene(E_SceneType sceneType)
        {
            Console.Clear();
            switch (sceneType)
            {
                case E_SceneType.Begin:
                    nowScene = beginScene;
                    break;
                case E_SceneType.Game:
                    nowScene = gameScene;
                    gameScene.Reset();
                    break;
                case E_SceneType.End:
                    nowScene = endScene;
                    break;
                default:
                    break;
            }
        }
    }
}
