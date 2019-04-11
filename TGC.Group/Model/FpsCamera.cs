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

namespace TGC.Group.Model
{
    public class TgcFpsCamera : TgcCamera
    {
        private TGCMatrix cameraRotation;
        private TGCVector3 directionView;
        private float leftrightRot;
        private float updownRot;
        private TGCVector3 positionEye;

        private TgcD3dInput Input { get; }
        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }
        private bool bloquear = false;

        public TgcFpsCamera(TGCVector3 positionEye, float moveSpeed, float rotationSpeed, TgcD3dInput input) {
            this.Input = input;
            this.positionEye = positionEye;
            this.RotationSpeed = rotationSpeed;
            this.MovementSpeed = moveSpeed;
            this.directionView = new TGCVector3(0, 0, -1);
            this.leftrightRot = FastMath.PI_HALF;
            this.updownRot = -FastMath.PI / 10.0f;
            this.cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
        }

        public override void UpdateCamera(float elapsedTime) {
            var moveVector = TGCVector3.Empty;
            
            if (Input.keyDown(Key.W))
                moveVector += new TGCVector3(0, 0, -1) * MovementSpeed;
            if (Input.keyDown(Key.S))
                moveVector += new TGCVector3(0, 0, 1) * MovementSpeed;
            if (Input.keyDown(Key.D))
                moveVector += new TGCVector3(-1, 0, 0) * MovementSpeed;
            if (Input.keyDown(Key.A))
                moveVector += new TGCVector3(1, 0, 0) * MovementSpeed;

            if (!bloquear)
            {
                leftrightRot -= -Input.XposRelative * RotationSpeed;
                updownRot -= Input.YposRelative * RotationSpeed;
                cameraRotation = TGCMatrix.RotationX(updownRot) * TGCMatrix.RotationY(leftrightRot);
                positionEye += TGCVector3.TransformNormal(moveVector * elapsedTime, cameraRotation);
            }

            var cameraRotatedTarget = TGCVector3.TransformNormal(directionView, cameraRotation);
            var cameraFinalTarget = positionEye + cameraRotatedTarget;
            var cameraOriginalUpVector = DEFAULT_UP_VECTOR;
            var cameraRotatedUpVector = TGCVector3.TransformNormal(cameraOriginalUpVector, cameraRotation);

            base.SetCamera(positionEye, cameraFinalTarget, cameraRotatedUpVector);
        }

        public void Lock() {
            bloquear = !bloquear;
        }

    }
}
