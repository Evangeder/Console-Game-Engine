using ConsoleGame.Behaviour;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public static class Screen
    {
        //default console size is 120x30
        // This is 3D array, because of render layers
        private static int Layers = 10;
        public readonly static int Width = 120;
        public readonly static int Height = 30;
        private static ScreenPoint[,,] ScreenLayers = new ScreenPoint[Layers, Width, Height];
        private static ScreenPoint[,] ScreenRender = new ScreenPoint[Width, Height];

        public static List<ObjectBehaviour> objectBehaviours = new List<ObjectBehaviour>();

        public static bool FrameUpdate = false;
        public static bool FrameLateUpdate = false;

        private static ManualResetEvent mre = new ManualResetEvent(false);

        public enum ScreenLayer : int
        {
            UIText2 = 9,
            UI2 = 8,
            UIText1 = 7,
            UI1 = 6,
            Character = 5,
            Enemies = 4,
            Foreground = 3,
            Details2 = 2,
            Details = 1,
            Background = 0
        }

        struct ScreenPoint
        {
            public ScreenPoint(char character = ' ') // spacebar means transparency
            {
                Character = character;
                ForeColor = ConsoleColor.White;
            }
            public char Character { get; set; }
            public ConsoleColor ForeColor { get; set; }
        }

        public static void AppendChar(int layer, Vector2 location, char character, ConsoleColor color = ConsoleColor.White)
        {
            //mre.WaitOne();
            if (Mathf.RoundToInt(location.x) >= Width || Mathf.RoundToInt(location.y) >= Height) return;
            if (location.x < 0f || location.y < 0f) return;
            ScreenLayers[layer, Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y)] = new ScreenPoint { Character = character, ForeColor = color };
            //mre.Set();
        }

        public static void AppendString(int layer, Vector2 location, string text, ConsoleColor color = ConsoleColor.White)
        {
            //mre.WaitOne();
            if (Mathf.RoundToInt(location.x) >= Width || Mathf.RoundToInt(location.y) >= Height) return;
            if (location.x < 0f || location.y < 0f) return;
            if (text.Contains("\n"))
            {
                string[] sub = text.Split('\n');
                for (int y = 0; y < sub.Length; y++)
                {
                    char[] characters = sub[y].ToCharArray();
                    int counter = 0;
                    for (int x = Mathf.RoundToInt(location.x); x < Width; x++, counter++)
                        ScreenLayers[layer, x, Mathf.RoundToInt(location.y) + y] = new ScreenPoint { Character = characters[counter], ForeColor = color };
                }
            } else
            {
                char[] characters = text.ToCharArray();
                int counter = 0;
                for (int x = Mathf.RoundToInt(location.x); x < Width && counter < characters.Length; x++, counter++)
                    ScreenLayers[layer, x, Mathf.RoundToInt(location.y)] = new ScreenPoint { Character = characters[counter], ForeColor = color };
            }
            //mre.Set();
        }

        public static bool IsColliding(int layer, Vector2 location)
        {
            if (Mathf.RoundToInt(location.x) >= Width || Mathf.RoundToInt(location.y) >= Height) return false;
            if (location.x < 0f || location.y < 0f) return false;
            if (ScreenLayers[layer, Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y)].Character == ' ' ||
                ScreenLayers[layer, Mathf.RoundToInt(location.x), Mathf.RoundToInt(location.y)].ForeColor == ConsoleColor.Black) 
                return false;
            else 
                return true;
        }

        public static void ClearLayer(int layer)
        {
            //mre.WaitOne();
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    ScreenLayers[layer, x, y] = new ScreenPoint(' ');
            //mre.Set();
        }

        static byte TryConvertToByte(char Char)
        {
            if (Convert.ToInt32(Char) > 254)
                return 254;

            return Convert.ToByte(Char);
        }

        public static void RenderLoop()
        {
            long Debug_CurrentFrame = 0;
            int fps = 0;
            int batches = 1;
            Stopwatch sw = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            Console.Title = Settings.GameName;
            var commandBuilder = new List<Action>();
            Console.SetWindowSize(Width + 1, Height + 2);
            while (true)
            {
                foreach (ObjectBehaviour ob in objectBehaviours)
                {
                    if (!ob.WasStartCalled)
                    {
                        ob.Start();
                        ob.WasStartCalled = true;
                    }
                    ob.Update();
                }

                sw.Restart();
                Console.CursorVisible = false;
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                commandBuilder.Clear();
                sb.Clear();

                if (Settings.ShowDebugInfo)
                {
                    if (Debug_CurrentFrame > Settings.FramesPerSecond)
                    {
                        Console.Title = $"{Settings.GameName} // {(Settings.FrameLimiter ? $"Framelimiter ON - {Settings.FramesPerSecond} FPS" : $"{fps}")} // Batches: {batches}";
                        Debug_CurrentFrame = 0;
                    }
                    batches = 1;
                    Debug_CurrentFrame++;
                }

                ConsoleColor cc = ConsoleColor.DarkMagenta;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        bool FoundChar = false;
                        for (int l = Layers - 1; l >= 0; l--)
                        {
                            if (TryConvertToByte(ScreenLayers[l, x, y].Character) > 20)
                            {
                                if (ScreenLayers[l, x, y].Character != ' ')
                                {
                                    if (cc == ScreenLayers[l, x, y].ForeColor || cc == ConsoleColor.DarkMagenta)
                                    {
                                        cc = ScreenLayers[l, x, y].ForeColor;
                                        sb.Append(ScreenLayers[l, x, y].Character);
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = cc;
                                        Console.Write(sb.ToString());

                                        if (Settings.ShowDebugInfo) batches++;
                                        sb = new StringBuilder();
                                        cc = ScreenLayers[l, x, y].ForeColor;
                                        sb.Append(ScreenLayers[l, x, y].Character);
                                    }
                                    FoundChar = true;
                                    break;
                                }
                            }
                        }
                        if (!FoundChar) sb.Append(' ');
                    }
                    sb.Append('\n');
                }

                foreach (ObjectBehaviour ob in objectBehaviours)
                    ob.LateUpdate();

                if (sb.ToString().Length > 0)
                {
                    Console.ForegroundColor = cc;
                    Console.Write(sb.ToString());
                }
                Console.WriteLine();

                sw.Stop();

                if (sw.ElapsedMilliseconds > 0)
                    fps = (int)sw.ElapsedMilliseconds;

                Time.deltaTime = Math.Abs((1000f / (float)Settings.FramesPerSecond - sw.ElapsedMilliseconds)/1000f);

                if (Settings.FrameLimiter)
                    if ((1000 / Settings.FramesPerSecond - (int)sw.ElapsedMilliseconds) > 0) Thread.Sleep(1000 / Settings.FramesPerSecond - (int)sw.ElapsedMilliseconds);
            }
        }
    }
}
