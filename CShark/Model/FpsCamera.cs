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

namespace CShark.Model
{
    public class TgcFpsCamera : TgcCamera
    {
        public TGCMatrix cameraRotation;
        private TGCVector3 directionView;
        public float leftrightRot;
        public float updownRot;
        private TGCVector3 PositionEye;

        private TgcD3dInput Input;
        private Variable<float> VelocidadMovimiento;
        private Variable<float> VelocidadRotacion;
        private bool bloquear = false;

        public TgcFpsCamera(TGCVector3 positionEye, TgcD3dInput input) {
            Input = input;
            PositionEye = positionEye;
            VelocidadMovimiento = Configuracion.Instancia.VelocidadMovimiento;
            VelocidadRotacion = Configuracion.Instancia.VelocidadRotacion;
            directionView = new TGCVector3(0, 0, -1);
            leftrightRot = FastMath.PI_HALF;
            updownRot = -FastMath.PI / 10.0f;
            cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
        }

        public override void UpdateCamera(float elapsedTime) {
            var moveVector = TGCVector3.Empty;
            
            if (Input.keyDown(Key.W))
                moveVector += new TGCVector3(0, 0, -1) * VelocidadMovimiento.Valor;
            if (Input.keyDown(Key.S))
                moveVector += new TGCVector3(0, 0, 1) * VelocidadMovimiento.Valor;
            if (Input.keyDown(Key.D))
                moveVector += new TGCVector3(-1, 0, 0) * VelocidadMovimiento.Valor;
            if (Input.keyDown(Key.A))
                moveVector += new TGCVector3(1, 0, 0) * VelocidadMovimiento.Valor;

            if (!bloquear)
            {
                leftrightRot -= -Input.XposRelative * VelocidadRotacion.Valor;
                updownRot -= Input.YposRelative * VelocidadRotacion.Valor;
                cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
                PositionEye += TGCVector3.TransformNormal(moveVector * elapsedTime, cameraRotation);
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
