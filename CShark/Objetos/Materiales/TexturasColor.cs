using TGC.Core.Textures;

namespace CShark.Objetos
{
    public class TexturasColor
    {
        public static void CargarColores() {
            var path = Game.Default.MediaDirectory + @"Colores\";
            Colores = new TgcTexture[10];
            for (int i = 0; i < 10; i++)
                Colores[i] = TgcTexture.createTexture(path + (i + 1).ToString() + ".png");
        }

        public static TgcTexture[] Colores;
    }
}
