using CShark.Jugador;
using CShark.Terreno.Burbujas;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace CShark.Terreno
{
    public class Burbujeador : IDisposable
    {
        public List<BurbujaAleatoria> Burbujas;
        public TgcMesh MeshBurbuja;
        public TgcTexture TexturaBurbuja;
        private Effect effect;
        float time = 0;
        int radio = 10000;
        Random random;
        float limiteSuperficie;
        TGCVector3 posPlayer;

        public Burbujeador(TGCVector3 player, float limite) {
            var path = Game.Default.MediaDirectory + @"Textures\burbu.png";
            var sphere = Game.Default.MediaDirectory + @"Mapa\Sphere-TgcScene.xml";
            var tex = TgcTexture.createTexture(D3DDevice.Instance.Device, path);
            Burbujas = new List<BurbujaAleatoria>();
            MeshBurbuja = new TgcSceneLoader().loadSceneFromFile(sphere).Meshes[0];
            MeshBurbuja.changeDiffuseMaps(new[] {
                tex
            });
            effect = TGCShaders.Instance.LoadEffect(Game.Default.ShadersDirectory + "Burbuja.fx");
            random = new Random();
            limiteSuperficie = limite;
            posPlayer = player;
            spawnear();
        }

        private void spawnear() {
            Burbujas = new List<BurbujaAleatoria>();
            for (int i = 0; i < 10; i++) {
                Burbujas.Add(Nueva());
            }
        }

        private BurbujaAleatoria Nueva() {
            var x = random.Next((int)posPlayer.X - radio, (int)posPlayer.X + radio);
            var y = random.Next((int)posPlayer.Y - 1000, (int)posPlayer.Y + 1000);
            var z = random.Next((int)posPlayer.Z - radio, (int)posPlayer.Z + radio);
            return new BurbujaAleatoria(x, y, z, MeshBurbuja, effect, limiteSuperficie, this);
        }

        public void Reemplazar(BurbujaAleatoria burbuja) {
            Burbujas.Remove(burbuja);
            burbuja.Dispose();
            Burbujas.Add(Nueva());
        }


        public void Update(Player player, float elapsedTime) {
            if (player.Sumergido) {
                posPlayer = player.Posicion;
                time += elapsedTime;
                //to list trukito villero para alterar el burbujas adentro de l update
                Burbujas.ToList().ForEach(b => b.Update(elapsedTime));
            }
        }

        public void Render() {
            Burbujas.ForEach(b => b.Render(time));
        }

        public void Dispose() {
            Burbujas.ForEach(b => b.Dispose());
        }
    }
}
