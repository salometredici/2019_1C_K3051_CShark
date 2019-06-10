using System;
using TGC.Core.Sound;

namespace CShark.Utils
{
    public class MusicPlayer
    {
        private static TgcMp3Player mp3Player;

        private string currentFile;
        private static readonly string musicDirectory = Environment.CurrentDirectory + "\\" + Game.Default.MusicDirectory;
        private static readonly string mainTheme = "UnderPressure_DeepTrouble.mp3";
        private static readonly string menuTheme = "Warp_Room.mp3";
        private static readonly string chaseTheme = "Boulder.mp3";

        public MusicPlayer()
        {
            Init();
        }

        public void Init()
        {
            currentFile = musicDirectory + mainTheme;
            mp3Player = new TgcMp3Player
            {
                FileName = currentFile
            };
        }

        public static void SwitchMusic(bool abreMenu, bool enPersecucion)
        {
            LoadMp3(abreMenu, enPersecucion);
        }

        /// <summary>
        ///     Cargar un nuevo MP3 si hubo una variacion
        /// </summary>
        private static void LoadMp3(bool abreMenu, bool enPersecucion)
        {
            if (abreMenu)
            {
                ChangeMusic(musicDirectory + menuTheme);
            }
            else if (enPersecucion)
            {
                ChangeMusic(musicDirectory + chaseTheme);
            }
            else
            {
                ChangeMusic(musicDirectory + mainTheme);
            }
        }

        private static void ChangeMusic(string file)
        {
            mp3Player.closeFile();
            mp3Player.FileName = file;
            mp3Player.play(true);
        }

        public void Dispose()
        {
            mp3Player.closeFile();
        }
    }
}
