﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;

namespace CShark.NPCs
{
    public interface IAnimal
    {
        TGCVector3 Tamaño { get; set; }
        void Moverse(float elapsedTime);
    }
}