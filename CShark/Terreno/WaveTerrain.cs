using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
using TGC.Core.Textures;

namespace CShark.Terreno
{

    public class WaveTerrain : IRenderObject
    {
        private Texture Texture;
        private int totalVertices;
        private VertexBuffer vbTerrain;
        private CustomVertex.PositionTextured[] data;

        public WaveTerrain(Texture texture, int[,] heightmap, TGCVector3 centro) {
            Texture = texture;
            var vertices = 2 * 3 * (heightmap.GetLength(0) - 1) * (heightmap.GetLength(1) - 1);
            data = new CustomVertex.PositionTextured[vertices];
            vbTerrain = new VertexBuffer(typeof(CustomVertex.PositionTextured), vertices,
    D3DDevice.Instance.Device,
    Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
            
            //loadHeightmap(heightmap, 1, 1, centro);
        }

        public bool Enabled { get; set; }
        public TGCVector3 Center { get; private set; }
        public bool AlphaBlendEnable { get; set; }

        public void Render() {
            if (!Enabled)
                return;
            TexturesManager.Instance.clear(1);
            D3DDevice.Instance.Device.VertexDeclaration = TGCShaders.Instance.VdecPositionTextured;
            D3DDevice.Instance.Device.SetStreamSource(0, vbTerrain, 0);
            D3DDevice.Instance.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, totalVertices / 3);
        }

        public void Dispose() {
            if (vbTerrain != null) {
                vbTerrain.Dispose();
            }

            if (Texture != null) {
                Texture.Dispose();
            }
        }


        public void loadHeightmap(int[,] HeightmapData, float scaleXZ, float scaleY, TGCVector3 center) {
            Center = center;

            if (vbTerrain != null && !vbTerrain.Disposed) {
                vbTerrain.Dispose();
            }

            float width = HeightmapData.GetLength(0);
            float length = HeightmapData.GetLength(1);

            vbTerrain = new VertexBuffer(typeof(CustomVertex.PositionTextured), totalVertices,
                D3DDevice.Instance.Device,
                Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);

            //Cargar vertices
            var dataIdx = 0;
            data = new CustomVertex.PositionTextured[totalVertices];

            center.X = center.X * scaleXZ - width / 2 * scaleXZ;
            center.Y = center.Y * scaleY;
            center.Z = center.Z * scaleXZ - length / 2 * scaleXZ;

            for (var i = 0; i < width - 1; i++) {
                for (var j = 0; j < length - 1; j++) {
                    //Vertices
                    var v1 = new TGCVector3(center.X + i * scaleXZ, center.Y + HeightmapData[i, j] * scaleY,
                        center.Z + j * scaleXZ);
                    var v2 = new TGCVector3(center.X + i * scaleXZ, center.Y + HeightmapData[i, j + 1] * scaleY,
                        center.Z + (j + 1) * scaleXZ);
                    var v3 = new TGCVector3(center.X + (i + 1) * scaleXZ, center.Y + HeightmapData[i + 1, j] * scaleY,
                        center.Z + j * scaleXZ);
                    var v4 = new TGCVector3(center.X + (i + 1) * scaleXZ, center.Y + HeightmapData[i + 1, j + 1] * scaleY,
                        center.Z + (j + 1) * scaleXZ);

                    //Coordendas de textura
                    var t1 = new TGCVector2(i / width, j / length);
                    var t2 = new TGCVector2(i / width, (j + 1) / length);
                    var t3 = new TGCVector2((i + 1) / width, j / length);
                    var t4 = new TGCVector2((i + 1) / width, (j + 1) / length);

                    //Cargar triangulo 1
                    data[dataIdx] = new CustomVertex.PositionTextured(v1, t1.X, t1.Y);
                    data[dataIdx + 1] = new CustomVertex.PositionTextured(v2, t2.X, t2.Y);
                    data[dataIdx + 2] = new CustomVertex.PositionTextured(v4, t4.X, t4.Y);

                    //Cargar triangulo 2
                    data[dataIdx + 3] = new CustomVertex.PositionTextured(v1, t1.X, t1.Y);
                    data[dataIdx + 4] = new CustomVertex.PositionTextured(v4, t4.X, t4.Y);
                    data[dataIdx + 5] = new CustomVertex.PositionTextured(v3, t3.X, t3.Y);

                    dataIdx += 6;
                }
            }

            vbTerrain.SetData(data, 0, LockFlags.None);
        }
    }
}