using ConsoleGame.Behaviour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Text
{
    partial class Text
    {
        #region Declarations

        #region TextFormattingInfo

        /// <summary>
        /// Every tag here is usable and will return Type and Subtype (sbyte),
        /// Adding new value doesn't require anything else besides implementing actual effect for it.
        /// <para>First string is the type, and every string after first one is subtype, which means using is like this: 'color=red'</para>
        /// </summary>
        private string[][] tags =
        {
            //                              TYPE ↓      SUBTYPES =>
            /* ID = 1    */ new string[] { "hacker",    "slow",     "normal",   "fast" },
            /* ID = 2    */ new string[] { "slowtype",  "slowest",  "slow",     "normal",   "fast",     "fastest" },
            /* ID = 3    */ new string[] { "rot" },
            /* ID = 4    */ new string[] { "srot" },
            /* ID = 5    */ new string[] { "inc" },
            /* ID = 6    */ new string[] { "glide" },
            /* ID = 7    */ new string[] { "bounce" },
        };

        private string[][] nestedtags =
        {
            new string[] {"color", "default", "red", "green", "blue" }
        };

        /// <summary>
        /// This is to define how our text should behave if there's no effector on it.
        /// </summary>
        sbyte DefaultTextStyle = -1;

        /// <summary>
        /// You can modify those characters as long as you'll remember to use the correct sytax afterwards.
        /// </summary>
        char tagOpener = '<',
            tagCloser = '>',
            tagEquals = '=',
            tagClosure = '/';
        
        public int DefaultTextLeft = 3;
        public int DefaultTextTop = 24;

        #endregion

        #region TextSettings

        /// <summary>
        /// This bool is used to freeze the rendering thread until text is parsed. It is required because RecursiveParseHTMLText returns values in async.
        /// </summary>
        public bool UnlockThreads = true;

        #endregion

        struct ParsedTextInfo
        {
            public ParsedTextInfo(string text, sbyte type = -1, sbyte subtype = -1)
            {
                this.text = text;
                this.type = type;
                this.subtype = subtype;
            }
            public string text { get; set; }
            public sbyte type { get; set; }
            public sbyte subtype { get; set; }
        }

        /// <summary>
        /// This dictionary stores all substrings contained in given text in order.
        /// This way we know which one is which, and we can apply effects to those.
        /// </summary>
        Dictionary<int, ParsedTextInfo> ParsedText = new Dictionary<int, ParsedTextInfo>();

        BeepSound beep = new BeepSound();
        Point currentPoint;

        #endregion

        /// <summary>
        /// That's a check for RecursiveParseHTMLText to see if there's a nested effect
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>State if it needs to be parsed again or returned to dictionary</returns>
        int CheckIfContainsBrackets(string text)
        {
            int returnval = 0;
            //check if the formatting is correct, since you probably don't want to end up with infinite loop here.
            string workerString = string.Empty;
            if (!string.IsNullOrWhiteSpace(workerString))
            {
                int startBracket = text.IndexOf(tagOpener) + 1,
                    endBracket = text.IndexOf(tagCloser);

                string tag = text.Substring(startBracket, endBracket - startBracket);
                workerString = text.Substring(endBracket, text.Length - endBracket);

                if (startBracket >= 0 && endBracket > 1)
                {
                    workerString = text.Substring(endBracket - 1, text.Length + 1 - endBracket);
                    if (workerString.Contains($"{tagOpener}{tagClosure}{tag}{tagCloser}"))
                    {
                        returnval += 1;
                        string endBracketString = $"{tagOpener}{tagClosure}{tag}{tagCloser}";
                        startBracket = workerString.IndexOf(endBracketString);
                        string returnString = workerString.Substring(0, startBracket);
                        returnval += CheckIfContainsBrackets(returnString);
                    }
                }
            }
            return returnval;
        }

        private void RecursiveParseHTMLText(string text, int id = -1)
        {
            bool errorInParsing = false;
            string workerString = string.Empty;
            id++;
            sbyte type = DefaultTextStyle;
            sbyte subtype = -1;

            if (!string.IsNullOrWhiteSpace(text))
            {
                int startBracket = text.IndexOf(tagOpener) + 1,
                    endBracket = text.IndexOf(tagCloser);

                if (startBracket > 1)
                {
                    string returnString = text.Substring(0, startBracket - 1);
                    string restOfString = text.Substring(startBracket - 1, text.Length + 1 - startBracket);
                    ParsedText.Add(id, new ParsedTextInfo { text = returnString, type = type, subtype = subtype });
                    RecursiveParseHTMLText(restOfString, id);
                    return;
                }
                else if (startBracket <= 0)
                {
                    ParsedText.Add(id, new ParsedTextInfo { text = text, type = type, subtype = subtype });
                    UnlockThreads = true;
                    return;
                }

                if (startBracket >= 0 && endBracket > 1)
                    workerString = text.Substring(startBracket, endBracket - startBracket);

                if (!string.IsNullOrWhiteSpace(workerString))
                {
                    if (workerString.Contains(tagEquals))
                    {
                        string[] workerStrings = workerString.Split(tagEquals);
                        for (sbyte i = 0; i < tags.Length; i++)
                            if (workerStrings[0].ToLower() == tags[i][0].ToLower()) type = i;

                        if (type > -1)
                            for (sbyte i = 0; i < tags[type].Length; i++)
                                if (workerStrings[1].ToLower() == tags[type][i].ToLower()) subtype = i;
                    }
                    else
                    {
                        for (sbyte i = 0; i < tags.Length; i++)
                            if (workerString.ToLower() == tags[i][0].ToLower()) type = i;
                    }
                }
                else
                    errorInParsing = true;

                if (type != -1 && !errorInParsing)
                {
                    workerString = text.Substring(endBracket + 1, text.Length - endBracket - 1);
                    string endBracketString = $"{tagOpener}{tagClosure}{tags[type][0]}{tagCloser}";
                    startBracket = workerString.IndexOf(endBracketString);
                    string returnString = workerString.Substring(0, startBracket);
                    if (workerString.Length > startBracket + endBracketString.Length)
                    {
                        string restOfString = workerString.Substring(startBracket + endBracketString.Length, workerString.Length - (startBracket + endBracketString.Length));
                        int newID = CheckIfContainsBrackets(returnString);
                        if (newID > 0) RecursiveParseHTMLText(restOfString, id);
                        RecursiveParseHTMLText(restOfString, id + newID);
                    }
                    ParsedText.Add(id, new ParsedTextInfo { type = type, subtype = subtype, text = returnString });
                }
                else
                {
                    UnlockThreads = true;
                }
            }
            UnlockThreads = true;
        }

        /// <summary>
        /// Our main function to create text effects.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <param name="modifier"></param>
        void GenerateTextEffect(string text, sbyte type, sbyte modifier)
        {
            switch (type)
            {
                case 1:
                    EffectSlowtype(text, modifier);
                    break;
                default:
                    if (text.Contains("\n"))
                    {
                        string[] workerStrings = text.Split('\n');
                        currentPoint.Y++;
                        for (int i = 0; i < workerStrings.Length; i++)
                        {
                            Screen.AppendString((int)Screen.ScreenLayer.UIText2, new Vector2(currentPoint.X, currentPoint.Y), workerStrings[i]);
                            currentPoint.X = DefaultTextLeft;
                        }
                    }
                    else
                    {
                        Screen.AppendString((int)Screen.ScreenLayer.UIText2, new Vector2(currentPoint.X, currentPoint.Y), text);
                        currentPoint.X += text.Length;
                    }
                    break;
            }
        }

        void SubtypeTextEffects(sbyte type, sbyte modifier)
        {
            switch (type)
            {
                case 0:
                    EffectSetColor(modifier);
                    break;
                default: break;
            }
        }

        public void DisplayText(string text)
        {
            RecursiveParseHTMLText(text);
            beep.InitBeep(1000, 250, 150);
            while (!UnlockThreads) { }
            currentPoint = new Point(DefaultTextLeft, DefaultTextTop);
            Screen.ClearLayer((int)Screen.ScreenLayer.UIText2);
            for (int i = 0; i < ParsedText.Count; i++)
            {
                if (!ParsedText.TryGetValue(i, out _)) continue; // safelock to prevent crashing on not assigned fieldss
                GenerateTextEffect(ParsedText[i].text, ParsedText[i].type, ParsedText[i].subtype);
            }
            ParsedText.Clear();
        }
    }
}
