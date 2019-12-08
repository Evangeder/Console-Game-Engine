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
    class GameLogic
    {
        private Text.Text text = new Text.Text();
        BitmapToAscii drawing = new BitmapToAscii();

        public void Init()
        {
            Thread ScreenRenderingThread = new Thread(new ThreadStart(Screen.RenderLoop));
            ScreenRenderingThread.Start();
            Thread KeyPressingThread = new Thread(new ThreadStart(Input.RegisterKeys));
            KeyPressingThread.Start();
        }

        int chatboxHeight = Screen.Height - 12;

        public void Intro()
        {
            Init();
            AnimateFakeProgressBar();
            text.DefaultTextTop = Screen.Height - 10;
            Thread.Sleep(1000);
            Thread.Sleep(1000);
            Screen.ClearLayer((int)Screen.ScreenLayer.UI1);
            Screen.ClearLayer((int)Screen.ScreenLayer.UIText2);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(118,17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(20,17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(40, 17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(60, 17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(80, 17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(100, 17)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(drawing.DrawGreenRect(new Vector2(0, 0), new Vector2(118, 8)), 2, new Vector2(0, 0), true, BitmapToAscii.Pivot.TopLeft, true);
            drawing.Draw(Properties.Resources.Skeleton, 3, new Vector2(46, 1), true, BitmapToAscii.Pivot.TopLeft, true);
            AnimateChatIn();
            DrawHP();
            text.DisplayText("<slowtype=fastest>Movement: [UP]/[LEFT]/[RIGHT] Arrows.\nGravity will pull you down when you're red.\nPress [SPACEBAR] to continue.</slowtype>"); WaitForAccept();
            text.DisplayText("<slowtype=fastest>Let the test begin!</slowtype>"); WaitForAccept();
            Screen.ClearLayer((int)Screen.ScreenLayer.UIText2);
            ChangeChatboxToCombatBox();
            Heart heart = new Heart();
            heart.AllowMovement = true;
            while (!heart.DestroyMe) { Screen.AppendChar(8, new Vector2(-1, -1), ' '); } // wait the battle
            heart.Visible = false;
            heart.AllowMovement = false;
            heart.gameObject.transform.position = new Vector2(3, chatboxHeight + 9);
            ChangeCombatBoxToChatbox();
            text.DisplayText("<slowtype>Oh, you dodged that? Nice.\nRemember, that's just a start!</slowtype>"); WaitForAccept();
            heart.Visible = true;
            DrawButtons();
            text.DisplayText("<slowtype=fastest>What will you do?</slowtype>");
            //Screen.ClearLayer((int)Screen.ScreenLayer.UIText2);
            //AnimateChatOut();
        }

        //everything from below here is just placeholder to make this "presentation" work :P

        void WaitForAccept()
        {
            while (!Input.GetKeyDown(ConsoleKey.Spacebar)) { }
        }

        void AnimateChatIn()
        {
            for (int x = 59; x >= 0; x--)
            {
                drawing.Draw(drawing.DrawChatBoxRect(new Vector2(x, chatboxHeight), new Vector2(120 - x*2 - 2, 6)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
                Thread.Sleep((int)(Time.deltaTime * 100f));
            }
        }

        void AnimateChatOut()
        {
            for (int x = 0; x < 60; x++)
            {
                drawing.Draw(drawing.DrawChatBoxRect(new Vector2(x, chatboxHeight), new Vector2(120 - x * 2 - 2, 6)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
                Thread.Sleep((int)(Time.deltaTime * 100f));
            }
        }

        void DrawButtons()
        {
            drawing.Draw(drawing.DrawButton(new Vector2(1, chatboxHeight + 8), new Vector2(21, 2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
            Screen.AppendString(7, new Vector2(5, chatboxHeight + 9), "FIGHT");
            drawing.Draw(drawing.DrawButton(new Vector2(32, chatboxHeight + 8), new Vector2(21, 2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
            Screen.AppendString(7, new Vector2(36, chatboxHeight + 9), "ACT");
            drawing.Draw(drawing.DrawButton(new Vector2(64, chatboxHeight + 8), new Vector2(21, 2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
            Screen.AppendString(7, new Vector2(68, chatboxHeight + 9), "ITEM");
            drawing.Draw(drawing.DrawButton(new Vector2(96, chatboxHeight + 8), new Vector2(21, 2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
            Screen.AppendString(7, new Vector2(100, chatboxHeight + 9), "MERCY");
        }

        void DrawHP()
        {
            Screen.AppendString(5, new Vector2(1, Screen.Height - 1), "██████████", ConsoleColor.Green);
            Screen.AppendString(5, new Vector2(12, Screen.Height - 1), "10 HP", ConsoleColor.White);
        }

        void ChangeChatboxToCombatBox()
        {
            int y1 = 0, y2 = 0;
            for (int x = 0; x < 50; x++)
            {
                y1++;
                if (y1 > 10)
                {
                    y1 = 0;
                    y2++;
                }
                drawing.Draw(drawing.DrawChatBoxRect(new Vector2(x, chatboxHeight - y2), new Vector2(120 - x * 2 - 2, 6 + y2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
                Thread.Sleep((int)(Time.deltaTime * 100f));
            }
        }

        void ChangeCombatBoxToChatbox()
        {
            int y1 = 0, y2 = 5;
            for (int x = 49; x >= 0; x--)
            {
                y1++;
                if (y1 > 10)
                {
                    y1 = 0;
                    y2--;
                }
                drawing.Draw(drawing.DrawChatBoxRect(new Vector2(x, chatboxHeight - y2), new Vector2(120 - x * 2 - 2, 6 + y2)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
                Thread.Sleep((int)(Time.deltaTime * 100f));
            }
            drawing.Draw(drawing.DrawChatBoxRect(new Vector2(0, chatboxHeight), new Vector2(120 - 2, 6)), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
        }

        void AnimateFakeProgressBar()
        {
            Random rand = new Random(DateTime.Now.Second);
            for (int x = 0; x < Screen.Width - 2; x++)
            {
                drawing.Draw(drawing.DrawProgressBar(x), Screen.ScreenLayer.UI1, new Vector2(0, 0), false, BitmapToAscii.Pivot.TopLeft, true);
                Screen.AppendString(6, new Vector2(2, Screen.Height - 3), "Loading.....");
                int sleeptime = rand.Next(0, 10);
                if (sleeptime < 5) sleeptime = 1;
                Thread.Sleep((int)(sleeptime * Time.deltaTime*100f));
            }
            
        }

    }
}
