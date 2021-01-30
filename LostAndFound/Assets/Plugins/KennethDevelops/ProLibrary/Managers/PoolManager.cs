using System.Collections.Generic;
using System.Runtime.InteropServices;
using KennethDevelops.ProLibrary.DataStructures.Pool;
using KennethDevelops.ProLibrary.Util;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Managers{
    
    public class PoolManager : MonoBehaviour{
        
        private static Dictionary<string, PoolManager> _instances = new Dictionary<string, PoolManager>();

        [Header("Whether or not this Pool is destroyed when a Scene is loaded")]
        public bool destroyOnLoad = true;
        
        [Space]
        [Header("Amount of Objects the Pool will start with")]
        public int initialQuantity = 5;
        public GameObject prefab;

        private ObjectPool<IPoolObject> _pool;
        private int _lastIndex;

        private List<IPoolObject> _instantiated = new List<IPoolObject>();
        

        void Awake(){
            if (_instances.ContainsKey(gameObject.name)){
                Debug.Log("[ProLibrary - PoolManager] Found duplicated Pool: " + gameObject.name + ". " +
                          "Trying to add a Pool with the same gameObject name is not allowed. " +
                          "Please try renaming your gameObject.");
                Destroy(this);
                return;
            }
            
            _instances.Add(gameObject.name, this);
            _pool = new ObjectPool<IPoolObject>(InstanciatePoolObject, DisposePoolObject, initialQuantity);
            
            if (!destroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        public T AcquireObject<T>(Vector3 position, Quaternion rotation) where T : MonoBehaviour, IPoolObject{
            var poolObject = (T)_pool.AcquireObject();
            poolObject.gameObject.SetActive(true);
            
            poolObject.transform.position = position;
            poolObject.transform.rotation = rotation;

            poolObject.transform.parent = transform;

            poolObject.OnAcquire();
            
            _instantiated.Add(poolObject);

            return poolObject;
        }

        public void Dispose(IPoolObject poolObject){
            _pool.ReleaseObject(poolObject);
            _instantiated.Remove(poolObject);
        }

        /// <summary>
        /// Disposes all instantiated IPoolObjects
        /// </summary>
        public void Clear(){
            while(_instantiated.Count > 0) Dispose(_instantiated[0]); 
        }
        
        public static PoolManager GetInstance(string poolName){
            return _instances.ContainsKey(poolName) ? _instances[poolName] : null;
        }

        private IPoolObject InstanciatePoolObject(){
            var instance = Instantiate(prefab);
            instance.gameObject.name = prefab.name + " (" + _lastIndex + ")";
            instance.transform.parent = transform;
            _lastIndex++;
            return instance.GetComponent<IPoolObject>();
        }
        
        private IPoolObject DisposePoolObject(IPoolObject poolObject){
            poolObject.OnDispose();
            
            var monoBehaviour = (MonoBehaviour) poolObject;
            monoBehaviour.gameObject.SetActive(false);
            
            return poolObject;
        }

        void OnDestroy(){
            _instances.Remove(gameObject.name);
        }

    }
    
}