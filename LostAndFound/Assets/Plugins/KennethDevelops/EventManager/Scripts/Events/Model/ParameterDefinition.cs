using System;
using KennethDevelops.Serialization;
using UnityEngine;

namespace KennethDevelops.Events{
    [Serializable]
    public class ParameterDefinition{

        public string name;
        public Type type = Type.String;

        private object _defVal = "";
        public object DefaultValue{
            get{ return _defVal;  }
            set{ _defVal = value; }
        }

        [SerializeField] private byte[] _defaultValue;
        public                   bool   foldout = true;

        
        public ParameterDefinition Load(){
            if (_defaultValue.Length != 0)
                DefaultValue = _defaultValue.LoadData<object>();
            
            return this;
        }
        
        public ParameterDefinition Save(){
            _defaultValue = DefaultValue.GetData();
            return this;
        }

        //IMPORTANT: If you add more types, please don't change the int values of the existent ones
        //If you do, already defined events with parameters WILL change their types
        //You can re-order them as you wish as long as you don't change this values
        public enum Type{
            Bool       = 0,
            Int        = 1,
            Float      = 2,
            String     = 3,
            Vector2    = 6,
            Vector3    = 7,
            GameObject = 4,
            Custom     = 5
        }
    }
}