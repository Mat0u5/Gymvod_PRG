﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSpaceInvaders
{

    //NOTE: This is not a class I created, I copied it from https://social.msdn.microsoft.com/forums/vstudio/en-US/18fe83f0-5658-4bcf-bafc-2e02e187eb80/beep-beep
    //It's purely for creating sounds
    internal class Beep
    {

        int Amplitude;
        int Frequency;
        int Duration;
        public void BeepBeep(int Amplitude, int Frequency, int Duration)
        {
            this.Amplitude = Amplitude;
            this.Frequency = Frequency;
            this.Duration = Duration;
            Thread thread = new Thread(new ThreadStart(beeep));//so that the beed doesnt stop the main thread
            thread.Start();
        }
        private void beeep()
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
                        SP.PlaySync();
                    }
                }
            }
        }
    }
}
