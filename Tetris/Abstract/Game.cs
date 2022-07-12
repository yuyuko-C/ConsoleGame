using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Tetris
{
    enum E_SceneType
    {
        Begin, Game, End
    }

    class Game
    {
        public const int width = 40, height = 35;
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

            beginScene = new BeginScene(5);
            gameScene = new GameScene();
            endScene = new EndScene(5);

            SwithToScene(E_SceneType.Game);
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
