using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace QuestOfWonders
{
    public static class SoundSystem
    {
        public static Dictionary<string, SoundPlayer> playingSounds = new Dictionary<string, SoundPlayer>();

        public static void playSound(string soundLocation, bool loop)
        {
            if (frmMain.soundOn)
            {
                System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(soundLocation);
                SoundPlayer player = new SoundPlayer(stream);
                //player.SoundLocation = soundLocation;
                if (playingSounds.ContainsKey(soundLocation))
                    stopSound(soundLocation);
                if (loop)
                {
                    try
                    {
                        player.PlayLooping();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    player.Play();
                }
                playingSounds.Add(soundLocation, player);
            }
        }

        public static void stopAllSounds()
        {
            List<string> playingSnds;
            playingSnds = playingSounds.Keys.ToList<string>();

            foreach (string str in playingSnds)
            {
                stopSound(str);
            }
        }
        public static void stopSound(string soundLocation)
        {
            if (playingSounds.ContainsKey(soundLocation))
            {
                playingSounds[soundLocation].Stop();
                playingSounds.Remove(soundLocation);
            }
        }

    }
}
