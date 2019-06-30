using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Sound;

namespace CShark.Utils
{
    public class EffectsPlayer
    {
        public TgcDirectSound DirectSound;
        public static TgcStaticSound CoinSound;
        public static TgcStaticSound BiteSound;
        public static TgcStaticSound UnderWaterSound;
        public static TgcStaticSound BackgroundSound;
        public static SoundEffect SonidoFondo;

        public EffectsPlayer(TgcDirectSound _DirectSound)
        {
            DirectSound = _DirectSound;
       
            LoadSoundEffects();
            SonidoFondo = SoundEffect.Background;
            Play(SonidoFondo);
        }

        private void LoadSoundEffects()
        {
            CoinSound = new TgcStaticSound();
            CoinSound.loadSound(Game.Default.MusicDirectory + "Coin.wav", DirectSound.DsDevice);
            BiteSound = new TgcStaticSound();
            BiteSound.loadSound(Game.Default.MusicDirectory + "Bite.wav", DirectSound.DsDevice);
            UnderWaterSound = new TgcStaticSound();
            UnderWaterSound.loadSound(Game.Default.MusicDirectory + "UnderWater.wav", DirectSound.DsDevice);
            BackgroundSound = new TgcStaticSound();
            BackgroundSound.loadSound(Game.Default.MusicDirectory + "Background.wav", DirectSound.DsDevice);
        }

        public static void Play(SoundEffect sound)
        {
            switch (sound)
            {
                case SoundEffect.Coin:
                    CoinSound.play(false);
                    break;
                case SoundEffect.Bite:
                    BiteSound.play(false);
                    break;
                case SoundEffect.UnderWater:
                    SonidoFondo = sound;
                    UnderWaterSound.play(true);
                    break;
                case SoundEffect.Background:
                    SonidoFondo = sound;
                    BackgroundSound.play(true);
                    break;
                default:
                    return;
            }
        }

        public enum SoundEffect
        {
            Coin,
            Bite,
            UnderWater,
            Background
        }

        public void Dispose()
        {
            CoinSound.dispose();
            BiteSound.dispose();
            BackgroundSound.dispose();
            UnderWaterSound.dispose();
        }
    }
}
