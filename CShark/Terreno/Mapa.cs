using BulletSharp;
using CShark.Fisica.Colisiones;
using CShark.Jugador;
using CShark.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
//using TGC.Core.Terrain;
using static TGC.Core.Terrain.TgcSkyBox;

namespace CShark.Terreno
{
    public class Mapa
    {
        private readonly string mediaDir = Game.Default.MediaDirectory;

        private TGCBox Box;
        public TGCVector3 Centro;

        private SkyBox Skybox;

        // Areas del mapa
        private Terrain FondoDelMar;
        private Terrain NivelDelMar;

        private ColisionesTerreno Colisiones;

        // Elementos
        //private TgcScene Vegetacion;
        private Isla Isla;
        private TgcScene Rocas;
        private TgcScene Extras;
        private Superficie Superficie;

        private static Mapa instancia;

        public static Mapa Instancia {
            get {
                if (instancia == null)
                    instancia = new Mapa();
                return instancia;
            }
        }

        private Mapa() {
            FondoDelMar = new Terrain();
            NivelDelMar = new Terrain();            
            CargarTerreno(FondoDelMar, @"Mapa\Textures\hm.jpg", @"Mapa\Textures\seafloor.jpg", 10000 / 512f, 1f, TGCVector3.Empty);
            CargarTerreno(NivelDelMar, @"Mapa\Textures\hm.jpg", @"Mapa\Textures\skybox-island-water.png", 10000 / 512f, 1f, new TGCVector3(0, AlturaMar,0));
            Centro = FondoDelMar.Center;
            Skybox = new SkyBox(Centro);
            Box = TGCBox.fromSize(Skybox.Center, Skybox.Size);
            Isla = new Isla();
            Colisiones = new ColisionesTerreno();
            Colisiones.Init(FondoDelMar.getData());
            Superficie = new Superficie();
            Superficie.CargarTerrains();
        }

        public void CargarRocas(TgcScene rocas) {
            Rocas = rocas;
        }

        public void CargarExtras(TgcScene extras) {
            Extras = extras;
        }

        public float XMin => Centro.X - Box.Size.X / 2f;
        public float XMax => Centro.X + Box.Size.X / 2f;
        public float YMin => Centro.Y;
        public float YMax => Centro.Y + Box.Size.Y / 2f;
        public float ZMin => Centro.Z - Box.Size.Z / 2f;
        public float ZMax => Centro.Z + Box.Size.Z / 2f;
        public float AlturaMar => 2800f;

        public void Update(float elapsedTime, Player player) {
            Superficie.Update(elapsedTime);
            Colisiones.Update();
        }

        public void AgregarBody(RigidBody body) {
            Colisiones.AgregarBody(body);
        }

        public void SacarBody(RigidBody body) {
            Colisiones.SacarBody(body);
        }

        public bool Colisionan(CollisionObject ob1, CollisionObject ob2) {
            return Colisiones.Colisionan(ob1, ob2);
        }

        public void Render(Player player) {
            FondoDelMar.Render();
            //NivelDelMar.Render();
            Skybox.Render();
            Isla.Render();
            //Vegetacion.RenderAll();
            Rocas.RenderAll();
            Extras.RenderAll();
            Superficie.Render();
        }

        public void Dispose() {
            FondoDelMar.Dispose();
            NivelDelMar.Dispose();
            Skybox.Dispose();
            Isla.Dispose();
            //Vegetacion.DisposeAll();
            Rocas.DisposeAll();
            Extras.DisposeAll();
            Superficie.Dispose();
        }

        private void CargarTerreno(Terrain terrain, string heightMapDir, string textureDir, float xz, float y, TGCVector3 position) {
            var loader = new TgcSceneLoader(); // No lo estamos usando ahora
            terrain.AlphaBlendEnable = true;
            terrain.loadHeightmap(mediaDir + heightMapDir, xz, y, position);
            terrain.loadTexture(mediaDir + textureDir);
            //Vegetacion = loader.loadSceneFromFile(media + "vegetation-TgcScene.xml");
        }
    }
}
