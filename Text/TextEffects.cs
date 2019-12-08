using ConsoleGame.Behaviour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame.Text
{
    partial class Text
    {
        ConsoleColor cc = ConsoleColor.White;

        void EffectSetColor(sbyte color)
        {
            switch (color)
            {
                case 1: default: cc = ConsoleColor.White; break;
                case 2: cc = ConsoleColor.Red; break;
                case 3: cc = ConsoleColor.Green; break;
                case 4: cc = ConsoleColor.Blue; break;
            }
        }

        void EffectSlowtype(string text, sbyte modifier)
        {
            int speed = 50;
            switch (modifier)
            {
                case 1: speed = 200; break;
                case 2: speed = 100; break;
                case 3: speed = 50; break;
                case 4: speed = 25; break;
                case 5: speed = 10; break;
            }
            char[] chars = text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                //Console.Beep(200, 250);
                Thread.Sleep(speed);
                if (chars[i] == tagOpener)
                {
                    string workerString = text.Substring(i + 1, text.Length - 1 - i);
                    int closeTag = workerString.IndexOf(tagCloser);
                    workerString = workerString.Substring(0, closeTag);
                    sbyte type = -1, subtype = -1;
                    if (workerString.Contains(tagEquals) || workerString.Contains(tagClosure))
                        for (sbyte i2 = 0; i2 < nestedtags.Length; i2++)
                        {
                            if (workerString.StartsWith(nestedtags[i2][0]))
                            {
                                type = i2;
                                for (sbyte i3 = 0; i3 < nestedtags[i2].Length; i3++)
                                    if (workerString.EndsWith(nestedtags[i2][i3]))
                                        subtype = i3;
                                break;
                            }
                            else if (workerString.StartsWith($"{tagClosure}{nestedtags[i2][0]}"))
                            {
                                type = i2;
                                subtype = -1;
                                break;
                            }
                        }
                    if (type > -1)
                    {
                        SubtypeTextEffects(type, subtype);
                        i += closeTag + 2;
                    }
                }
                // Check if the character was new line and reset CursorLeft to our defined location
                byte Char = Convert.ToByte(chars[i]);
                if (Char == 0x0A || Char == 0x0D) {
                    currentPoint.Y++;
                    currentPoint.X = DefaultTextLeft;
                    Console.CursorLeft = DefaultTextLeft; 
                    continue; 
                };
                Screen.AppendChar((int)Screen.ScreenLayer.UIText2, new Vector2(currentPoint.X, currentPoint.Y), chars[i], cc);
                currentPoint.X++;
            }
        }
    }
}
