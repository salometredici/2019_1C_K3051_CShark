using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace CShark.Utilidades
{
    public class MeshLoader : IDisposable
    {
        public static MeshLoader Instance { get; } = new MeshLoader();

        private Dictionary<string, TgcMesh> Meshes;

        private MeshLoader() {
            Meshes = new Dictionary<string, TgcMesh>();
        }

        public void LoadMesh(TgcMesh mesh) {
            if (!Exists(mesh.Name))
                Meshes.Add(mesh.Name, mesh);
        }

        public void LoadScene(TgcScene scene) {
            scene.Meshes.ForEach(m => LoadMesh(m));
        }

        private bool Exists(string name) {
            return Meshes.ContainsKey(name);
        }

        public TgcMesh GetInstance(string name) {
            return Meshes[name].createMeshInstance(name);
        }

        public TgcMesh GetInstance(string name, TgcMesh meshData) {
            var bb = meshData.BoundingBox;
            var position = bb.calculateBoxCenter();
            var rotation = meshData.Rotation;
            var scale = meshData.Scale;
            var instance = GetInstance(name);
            instance.AutoTransformEnable = false;
            instance.Transform = 
                TGCMatrix.Scaling(scale) *
                TGCMatrix.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
                TGCMatrix.Translation(position);
            return instance;
        }

        public void Dispose() {
            foreach (var mesh in Meshes.Values)
                mesh.Dispose();
        }
    }
}
