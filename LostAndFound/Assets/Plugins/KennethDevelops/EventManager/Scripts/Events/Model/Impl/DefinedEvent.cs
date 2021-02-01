using System;
using KennethDevelops.DataStructures;
using KennethDevelops.ProLibrary.Util;
using UnityEngine;

namespace KennethDevelops.Events{

	[Serializable]
	public class DefinedEvent{
		
		[SerializeField] public DefinedParameterDictionary parameters = new DefinedParameterDictionary();
		[SerializeField] public string                     eventId;
		
		private EventDefinition _definition;
		

		public DefinedEvent(string eventId){
			this.eventId = eventId;
			_definition = EventManager.GetDefinition(eventId);

			foreach (var parameterDefinition in Definition.parameters.Values){
				var parameter = new DefinedParameter(parameterDefinition);
				parameters.Add(parameterDefinition.name, parameter);
			}
		}

		public void Trigger(float seconds = 0){
			if (seconds <= 0)
				EventManager.TriggerEvent(Definition.name, this);
			else
				CoroutineManager.StartCoroutine(new WaitForSecondsAndDo(seconds, () => EventManager.TriggerEvent(Definition.name, this)));
		}

		#region Setters

		public DefinedEvent SetBoolParameter(string name, bool value){
			return SetParameter(name, ParameterDefinition.Type.Bool, value);
		}

		public DefinedEvent SetIntParameter(string name, int value){
			return SetParameter(name, ParameterDefinition.Type.Int, value);
		}

		public DefinedEvent SetFloatParameter(string name, float value){
			return SetParameter(name, ParameterDefinition.Type.Float, value);
		}

		public DefinedEvent SetStringParameter(string name, string value){
			return SetParameter(name, ParameterDefinition.Type.String, value);
		}

		public DefinedEvent SetGameObjectParameter(string name, GameObject value){
			return SetParameter(name, ParameterDefinition.Type.GameObject, value);
		}

		public DefinedEvent SetCustomParameter(string name, object value){
			return SetParameter(name, ParameterDefinition.Type.Custom, value);
		}

		public DefinedEvent SetVector2Parameter(string name, Vector2 value){
			return SetParameter(name, ParameterDefinition.Type.Vector2, (SerializableVector2)value);
		}

		public DefinedEvent SetVector3Parameter(string name, Vector3 value){
			return SetParameter(name, ParameterDefinition.Type.Vector3, (SerializableVector3)value);
		}

		private DefinedEvent SetParameter<T>(string name, ParameterDefinition.Type type, T value){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Parameter '" + name + "' could not be found for event '" + Definition.name + "'");
				return this;
			}

			if (parameters[name].type != type){
				Debug.LogError("[EventManager] Parameter '" + name + "' is not defined as '" + type + "'");
				return this;
			}

			parameters[name].Value = value;
			return this;
		}

		#endregion

		#region Getters

		public bool GetBoolParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Bool parameter '" + name + "' in the '" +
				               Definition.name + "' event. Returning 'false'");
				return false;
			}
			else if (parameters[name].type != ParameterDefinition.Type.Bool){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '" + name + "' parameter as a Bool type when it actually is a '" + type + "' type (event: '" + Definition.name + "'. Returning 'false'");
				return false;
			}

			return (bool) parameters[name].Value;
		}

		public int GetIntParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Int parameter '" + name + "' in the '" +
				               Definition.name + "' event. Returning '0'");
				return 0;
			}
			else if (parameters[name].type != ParameterDefinition.Type.Int){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '" + name +
				               "' parameter as an Int type when it actually is a '" + type + "' type (event: '" +
				               Definition.name + "'. Returning '0'");
				return 0;
			}

			return (int) parameters[name].Value;
		}

		public float GetFloatParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Float parameter '" + name + "' in the '" + Definition.name + "' event. Returning '0f'");
				return 0f;
			}
			else if (parameters[name].type != ParameterDefinition.Type.Float){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '" + name + "' parameter as a Float type when it actually is a '" + type + "' type (event: '" + Definition.name +"'. Returning '0f'");
				return 0f;
			}

			return (float) parameters[name].Value;
		}

		public string GetStringParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined String parameter '"+ name + "' in the '" + Definition.name + "' event. Returning 'null'");
				return null;
			}
			else if (parameters[name].type != ParameterDefinition.Type.String){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a String type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning 'null'");
				return null;
			}

			return (string) parameters[name].Value;
		}

		public Vector2 GetVector2Parameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Vector2 parameter '"+ name + "' in the '" + Definition.name + "' event. Returning 'null'");
				return default(Vector2);
			}
			else if (parameters[name].type != ParameterDefinition.Type.Vector2){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a Vector2 type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning 'null'");
				return default(Vector2);
			}

			return (SerializableVector2) parameters[name].Value;
		}
		
		public Vector3 GetVector3Parameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Vector3 parameter '"+ name + "' in the '" + Definition.name + "' event. Returning 'null'");
				return default(Vector3);
			}
			else if (parameters[name].type != ParameterDefinition.Type.Vector3){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a Vector3 type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning 'null'");
				return default(Vector3);
			}

			return (SerializableVector3) parameters[name].Value;
		}

		public GameObject GetGameObjectParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined GameObject parameter '"+ name + "' in the '"+ Definition.name + "' event. Returning 'null'");
				return null;
			}
			else if (parameters[name].type != ParameterDefinition.Type.GameObject){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a GameObject type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning 'null'");
				return null;
			}

			return parameters[name].Value == null ? null : (GameObject) parameters[name].Value;
		}

		public object GetCustomParameter(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Custom parameter '"+ name + "' in the '"+ Definition.name + "' event. Returning 'null'");
				return null;
			}
			else if (parameters[name].type != ParameterDefinition.Type.Custom){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a Custom type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning 'null'");
				return null;
			}

			return parameters[name].Value;
		}

		public T GetCustomParameter<T>(string name){
			if (!parameters.ContainsKey(name)){
				Debug.LogError("[EventManager] Trying to retrieve an undefined Custom parameter '"+ name + "' in the '"+ Definition.name + "' event. Returning the default value");
				return default(T);
			}
			else if (parameters[name].type != ParameterDefinition.Type.Custom){
				var type = parameters[name].type.ToString();
				Debug.LogError("[EventManager] Trying to retrieve the '"+ name + "' parameter as a Custom type when it actually is a '"+ type + "' type (event: '"+ Definition.name + "'. Returning the default value");
				return default(T);
			}

			return parameters[name].Value == null ? default(T) : (T) parameters[name].Value;
		}

		#endregion
		
		private EventDefinition Definition{
			get{
				if (_definition == null) _definition = EventManager.GetDefinition(eventId);
				return _definition;
			}
		}

	}

	[Serializable]
	public class DefinedParameterDictionary : SerializableDictionary<string, DefinedParameter>{ }


}