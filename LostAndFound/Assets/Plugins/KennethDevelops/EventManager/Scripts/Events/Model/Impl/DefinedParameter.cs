using System;
using System.Linq;
using KennethDevelops.Events;
using KennethDevelops.Serialization;
using UnityEngine;

[Serializable]
public class DefinedParameter{

	public string name;
	public ParameterDefinition.Type type;
	
	public bool foldout = true;
	
	private                  object _value;
	[SerializeField] private byte[] _valueData;
	
	[SerializeField] private GameObject _gameObjectValue;
	

	public object Value{
		get{
			if (type == ParameterDefinition.Type.GameObject)
				return _gameObjectValue;
			
			if (_value == null && _valueData.Any())
				_value = _valueData.LoadData<object>();
			
			return _value;
		}
		set{
			if (type == ParameterDefinition.Type.GameObject){
				_gameObjectValue = (GameObject)value;
				return;
			}

			if (_value != value && value != null){
				_valueData = value.GetData();
			}

			_value = value;
		}
	}

	public DefinedParameter(ParameterDefinition definition){
		name = definition.name;
		type = definition.type;
		
		if (type == ParameterDefinition.Type.GameObject || type == ParameterDefinition.Type.Custom)
			_value = null;
		else
			_value = definition.DefaultValue;
	}

}
