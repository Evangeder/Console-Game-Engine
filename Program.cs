using ConsoleGame.Behaviour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            GameLogic gameLogic = new GameLogic();
            gameLogic.Intro();
            Console.ReadLine();
            while (Console.ReadKey().Key != ConsoleKey.Escape) { }
            Environment.Exit(0);
        }

        /* notes
        
        Console.Beep(200, 5); gives good footstep sound


        */
    }

    public class BeepSound
    {
        [DllImport("Kernel32.dll")]
        public static extern bool Beep(UInt32 frequency, UInt32 duration);
        public static void BeepMe()
        {
            uint Frequency = 500;
            uint DurationInMS = 100;
            Beep(Frequency, DurationInMS);
        }

        MemoryStream MS;
        BinaryWriter BW;
        SoundPlayer SP;
        bool isReady = false;

        public void InitBeep(int Amplitude, int Frequency, int Duration, bool Sync = true)
        {
            double A = ((Amplitude * (System.Math.Pow(2, 15))) / 1000) - 1;
            double DeltaFT = 2 * Math.PI * Frequency / 44100.0;

            int Samples = 441 * Duration / 10;
            int Bytes = Samples * 4;
            int[] Hdr = { 0X46464952, 36 + Bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, Bytes };
            MS = new MemoryStream(44 + Bytes);
            BW = new BinaryWriter(MS);
            for (int I = 0; I < Hdr.Length; I++)
            {
                BW.Write(Hdr[I]);
            }
            for (int T = 0; T < Samples; T++)
            {
                short Sample = System.Convert.ToInt16(A * Math.Sin(DeltaFT * T));
                BW.Write(Sample);
                BW.Write(Sample);
            }
            BW.Flush();
            MS.Seek(0, SeekOrigin.Begin);
            SP = new SoundPlayer(MS);
            isReady = true;
        }

        public void Play(bool Sync = true)
        {
            BeepMe();
            //if (Sync) SP.PlaySync();
            //else SP.Play();
        }

        public static void BeepBeep(int Amplitude, int Frequency, int Duration, bool Sync = true)
        {
            double A = ((Amplitude * (System.Math.Pow(2, 15))) / 1000) - 1;
            double DeltaFT = 2 * Math.PI * Frequency / 44100.0;

            int Samples = 441 * Duration / 10;
            int Bytes = Samples * 4;
            int[] Hdr = { 0X46464952, 36 + Bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, Bytes };
            using (MemoryStream MS = new MemoryStream(44 + Bytes))
            {
                using (BinaryWriter BW = new BinaryWriter(MS))
                {
                    for (int I = 0; I < Hdr.Length; I++)
                    {
                        BW.Write(Hdr[I]);
                    }
                    for (int T = 0; T < Samples; T++)
                    {
                        short Sample = System.Convert.ToInt16(A * Math.Sin(DeltaFT * T));
                        BW.Write(Sample);
                        BW.Write(Sample);
                    }
                    BW.Flush();
                    MS.Seek(0, SeekOrigin.Begin);
                    using (SoundPlayer SP = new SoundPlayer(MS))
                    {
                        if (Sync) SP.PlaySync();
                        else SP.Play();
                    }
                }
            }
        }
    }
}
