using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework;

namespace TikiEngine
{
    public class MultiVoice : GameComponent
    {
        private bool _playing = false;

        private SpeechSynthesizer speaker1;
        private SpeechSynthesizer speaker2;
        private Queue<string> messages1;
        private Queue<string> messages2;
        private int volume = 100;

        public float Volume
        {
            get { return (float)volume / 100; }
            set {
                int vol = (int)(value * 100);

                volume = vol;
                speaker1.Volume = volume;
                speaker2.Volume = volume;
            }
        }

        public int Delay = 70;
        public int check = 0;

        public MultiVoice()
            :base(GI.Game)
        {
            speaker1 = new SpeechSynthesizer();
            speaker2 = new SpeechSynthesizer();
            speaker1.SetOutputToDefaultAudioDevice();
            speaker2.SetOutputToDefaultAudioDevice();
            speaker1.Rate = -3;
            speaker2.Rate = -3;

            speaker1.Volume = volume;
            speaker2.Volume = volume;

            speaker1.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Senior);
            speaker2.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Senior);

            messages1 = new Queue<string>();
            messages2 = new Queue<string>();
        }

        public void addMessage(string msg)
        {
            addMessage(msg, 0);
        }

        public void addMessage(string msg,bool playNow)
        {
            addMessage(msg, 0);
            if (playNow)
                play();
        }

        public void addMessage(string msg, int speaker)
        {
            switch (speaker)
            {
                case 0:
                    messages1.Enqueue(msg);
                    messages2.Enqueue(msg);
                    break;
                case 1:
                    messages1.Enqueue(msg);
                    break;
                case 2:
                    messages2.Enqueue(msg);
                    break;
            }
        }

        public void playIfChecked(object sender, EventArgs e)
        {
            _playing = false;
            //check++;
            //if (check == 2)
            //    play();
            play();
        }

        public void play()
        {
            Action del = playAsync;

            del.BeginInvoke(
                r => ((Action)r.AsyncState).EndInvoke(r),
                del
            );
        }

        private void playAsync()
        {
            check = 0;
            if (messages1.Count > 0)
            {
                _playing = true;
                string msg = messages1.Dequeue();
                speaker1.SpeakAsync(msg);
            }

            System.Threading.Thread.CurrentThread.Join(Delay);

            if (messages2.Count() > 0)
            {
                _playing = true;
                string msg = messages2.Dequeue();
                speaker2.SpeakAsync(msg);
            }
            speaker1.SpeakCompleted += new EventHandler<System.Speech.Synthesis.SpeakCompletedEventArgs>(playIfChecked);
            //speaker2.SpeakCompleted += new EventHandler<System.Speech.Synthesis.SpeakCompletedEventArgs>(playIfChecked);
        }

        #region Properties
        public bool Playing
        {
            get { return _playing; }
        }
        #endregion
    }
}
