using System;
using UnityEngine;

namespace KennethDevelops.ProLibrary.DataStructures.Pool{
    
    public interface IPoolObject{

        void OnAcquire();
        void OnDispose();
        
    }
    
}