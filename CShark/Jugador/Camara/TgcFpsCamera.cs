using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;
using CShark.Variables;
using CShark.Managers;
using CShark.Jugador;

namespace CShark.Model
{
    public class TgcFpsCamera : TgcCamera
    {
        public TGCMatrix cameraRotation;
        public TGCVector3 directionView;
        public float leftrightRot;
        public float updownRot;
        public TGCVector3 PositionEye;
        private Variable<float> VelocidadRotacion;
        private TgcD3dInput Input;
        private bool bloquear = false;
        private Player Player;

        public TgcFpsCamera(TgcD3dInput input, Player player) {
            Input = input;
            Player = player;
            PositionEye = player.Posicion;
            directionView = new TGCVector3(0, 0, -1);
            leftrightRot = 0;
            updownRot = 0;
            cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
            VelocidadRotacion = Configuracion.Instancia.VelocidadRotacion;
        }
        
        public override void UpdateCamera(float elapsedTime) {
            
            if (!bloquear)
            {
                leftrightRot -= -Input.XposRelative * VelocidadRotacion.Valor;
                updownRot -= Input.YposRelative * VelocidadRotacion.Valor;
                cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
                PositionEye += TGCVector3.TransformNormal(Player.MoveVector * elapsedTime, cameraRotation);
            }

            var cameraRotatedTarget = TGCVector3.TransformNormal(directionView, cameraRotation);
            var cameraFinalTarget = PositionEye + cameraRotatedTarget;
            var cameraOriginalUpVector = DEFAULT_UP_VECTOR;
            var cameraRotatedUpVector = TGCVector3.TransformNormal(cameraOriginalUpVector, cameraRotation);

            base.SetCamera(PositionEye, cameraFinalTarget, cameraRotatedUpVector);
        }

        public void Lock() {
            bloquear = !bloquear;
        }

    }
}
