using System;
using TGC.Core.Sound;

namespace CShark.Utils
{
    public class MusicPlayer
    {
        private TgcMp3Player mp3Player;

        private string currentFile;
        private readonly string musicDirectory = Environment.CurrentDirectory + "\\" + Game.Default.MusicDirectory;
        private readonly string mainTheme = "UnderPressure_DeepTrouble.mp3";
        private readonly string menuTheme = "Warp_Room.mp3";

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
            mp3Player.play(true);
        }

        public void SwitchMusic(bool abreMenu)
        {
            LoadMp3(abreMenu);
        }

        /// <summary>
        ///     Cargar un nuevo MP3 si hubo una variacion
        /// </summary>
        private void LoadMp3(bool abreMenu)
        {
            if (abreMenu)
            {
                ChangeMusic(musicDirectory + menuTheme);
            }
            else
            {
                ChangeMusic(musicDirectory + mainTheme);
            }
        }

        private void ChangeMusic(string file)
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
