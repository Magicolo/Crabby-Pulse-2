using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public abstract class MonoBehaviourExtended : MonoBehaviour {

		public enum CoroutineStates {
			None,
			Playing,
			Paused,
			Stopped
		}
		
		#region Components
		GameObject _gameObject;
		public new GameObject gameObject { get { return _gameObject ? _gameObject : (_gameObject = base.gameObject); } }
		
		Animation _animation;
		public new Animation animation { get { return _animation ? _animation : (_animation = GetComponent<Animation>()); } }

		AudioSource _audio;
		public new AudioSource audio { get { return _audio ? _audio : (_audio = GetComponent<AudioSource>()); } }

		Camera _camera;
		public new Camera camera { get { return _camera ? _camera : (_camera = GetComponent<Camera>()); } }

		Collider _collider;
		public new Collider collider { get { return _collider ? _collider : (_collider = GetComponent<Collider>()); } }

		Collider2D _collider2D;
		public new Collider2D collider2D { get { return _collider2D ? _collider2D : (_collider2D = GetComponent<Collider2D>()); } }

		ConstantForce _constantForce;
		public new ConstantForce constantForce { get { return _constantForce ? _constantForce : (_constantForce = GetComponent<ConstantForce>()); } }

		GUIElement _guiElement;
		public new GUIElement guiElement { get { return _guiElement ? _guiElement : (_guiElement = GetComponent<GUIElement>()); } }
		
		GUIText _guiText;
		public new GUIText guiText { get { return _guiText ? _guiText : (_guiText = GetComponent<GUIText>()); } }

		GUITexture _guiTexture;
		public new GUITexture guiTexture { get { return _guiTexture ? _guiTexture : (_guiTexture = GetComponent<GUITexture>()); } }

		HingeJoint _hingeJoint;
		public new HingeJoint hingeJoint { get { return _hingeJoint ? _hingeJoint : (_hingeJoint = GetComponent<HingeJoint>()); } }

		Light _light;
		public new Light light { get { return _light ? _light : (_light = GetComponent<Light>()); } }

		NetworkView _networkView;
		public new NetworkView networkView { get { return _networkView ? _networkView : (_networkView = GetComponent<NetworkView>()); } }

		ParticleEmitter _particleEmitter;
		public new ParticleEmitter particleEmitter { get { return _particleEmitter ? _particleEmitter : (_particleEmitter = GetComponent<ParticleEmitter>()); } }

		ParticleSystem _particleSystem;
		public new ParticleSystem particleSystem { get { return _particleSystem ? _particleSystem : (_particleSystem = GetComponent<ParticleSystem>()); } }

		Renderer _renderer;
		public new Renderer renderer { get { return _renderer ? _renderer : (_renderer = GetComponent<Renderer>()); } }

		Rigidbody _rigidbody;
		public new Rigidbody rigidbody { get { return _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>()); } }

		Rigidbody2D _rigidbody2D;
		public new Rigidbody2D rigidbody2D { get { return _rigidbody2D ? _rigidbody2D : (_rigidbody2D = GetComponent<Rigidbody2D>()); } }

		Transform _transform;
		public new Transform transform { get { return _transform ? _transform : (_transform = GetComponent<Transform>()); } }
		#endregion
		
		readonly Dictionary<string, List<IEnumerator>> coroutineDict = new Dictionary<string, List<IEnumerator>>();
		readonly Dictionary<IEnumerator, CoroutineStates> coroutineStateDict = new Dictionary<IEnumerator, CoroutineStates>();

		public void StartCoroutine(string coroutineName, IEnumerator coroutine) {
			if (!coroutineDict.ContainsKey(coroutineName)) {
				coroutineDict[coroutineName] = new List<IEnumerator>();
			}
		
			coroutineDict[coroutineName].Add(coroutine);
			SetCoroutineState(coroutine, CoroutineStates.Playing);
			StartCoroutine(coroutine);
			RemoveCompletedCoroutines();
		}
	
		public void StartCoroutines(string coroutineName, params IEnumerator[] coroutines) {
			foreach (IEnumerator coroutine in coroutines) {
				StartCoroutine(coroutineName, coroutine);
			}
		}
		
		public void PauseCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			
			if (GetCoroutineState(coroutine) == CoroutineStates.Playing) {
				SetCoroutineState(coroutine, CoroutineStates.Paused);
				StopCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void PauseCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				SetCoroutineState(coroutine, CoroutineStates.Paused);
				StopCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void PauseAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				PauseCoroutines(key);
			}
		}
	
		public void PauseAllCoroutinesExcept(params string[] coroutineNames) {
			foreach (string key in coroutineDict.Keys) {
				if (!coroutineNames.Contains(key)) {
					PauseCoroutines(key);
				}
			}
		}
		
		public void ResumeCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			
			if (GetCoroutineState(coroutine) == CoroutineStates.Paused) {
				SetCoroutineState(coroutine, CoroutineStates.Playing);
				StartCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void ResumeCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				SetCoroutineState(coroutine, CoroutineStates.Playing);
				StartCoroutine(coroutine);
			}
			
			RemoveCompletedCoroutines();
		}
	
		public void ResumeAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				ResumeCoroutines(key);
			}
		}
	
		public void StopCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = GetCoroutine(coroutineName, index);
			StopCoroutine(coroutine);
			RemoveCoroutine(coroutineName, coroutine);
			RemoveCompletedCoroutines();
		}
	
		public void StopCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				StopCoroutine(coroutine);
			}
			
			RemoveCoroutines(coroutineName);
			RemoveCompletedCoroutines();
		}
	
		public new void StopAllCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				StopCoroutines(key);
			}
		}
	
		public void StopAllCoroutinesExcept(params string[] coroutineNames) {
			foreach (string key in coroutineDict.Keys) {
				if (!coroutineNames.Contains(key)) {
					StopCoroutines(key);
				}
			}
		}
		
		public IEnumerator GetCoroutine(string coroutineName, int index) {
			IEnumerator coroutine = null;
			
			try {
				coroutine = coroutineDict[coroutineName][index];
			}
			catch {
				Logger.LogError(string.Format("Coroutine named {0} was not found at index {1}.", coroutineName, index));
			}
			
			return coroutine;
		}
		
		public IEnumerator[] GetCoroutines(string coroutineName) {
			List<IEnumerator> coroutines = null;
			
			try {
				coroutines = coroutineDict[coroutineName];
			}
			catch {
				Logger.LogError(string.Format("Coroutines named {0} were not found.", coroutineName));
			}
			
			return coroutines.ToArray();
		}

		public Dictionary<string, IEnumerator[]> GetAllCoroutines() {
			Dictionary<string, IEnumerator[]> coroutines = new Dictionary<string, IEnumerator[]>();
			
			foreach (string key in coroutineDict.Keys) {
				coroutines[key] = coroutineDict[key].ToArray();
			}
			
			return coroutines;
		}
		
		public CoroutineStates GetCoroutineState(string coroutineName, int index) {
			return GetCoroutineState(GetCoroutine(coroutineName, index));
		}
		
		public bool CoroutineExists(string coroutineName, int index) {
			return coroutineDict.ContainsKey(coroutineName) && coroutineDict[coroutineName].Count > index;
		}
		
		public bool CoroutinesExist(string coroutineName) {
			return coroutineDict.ContainsKey(coroutineName);
		}
		
		CoroutineStates GetCoroutineState(IEnumerator coroutine) {
			CoroutineStates state = CoroutineStates.None;
			
			try {
				state = coroutineStateDict[coroutine];
			}
			catch {
				Logger.LogError(string.Format("Coroutine state was not found."));
			}
			
			return state;
		}
		
		void SetCoroutineState(IEnumerator coroutine, CoroutineStates state) {
			coroutineStateDict[coroutine] = state;
		}
		
		void RemoveCoroutine(string coroutineName, IEnumerator coroutine) {
			try {
				coroutineDict[coroutineName].Remove(coroutine);
				coroutineStateDict.Remove(coroutine);
			}
			catch {
				Logger.LogError(string.Format("Coroutine named {0} was not found.", coroutineName));
			}
		}
		
		void RemoveCoroutines(string coroutineName) {
			foreach (IEnumerator coroutine in GetCoroutines(coroutineName)) {
				RemoveCoroutine(coroutineName, coroutine);
			}
			
			coroutineDict.Remove(coroutineName);
		}
		
		void RemoveCompletedCoroutines() {
			foreach (string key in coroutineDict.Keys) {
				foreach (IEnumerator coroutine in GetCoroutines(key)) {
					if (coroutine.Current == null) {
						RemoveCoroutine(key, coroutine);
					}
				}
			}
		}
			
		public void InvokeMethod(string methodName, float delay, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, delay, arguments));
		}
	
		public void InvokeMethod(string methodName, params object[] arguments) {
			StartCoroutine(InvokeDelayed(methodName, 0, arguments));
		}
	
		public void InvokeMethodRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, delay, repeatRate, arguments));
		}
		
		public void InvokeMethodRepeating(string methodName, float repeatRate, params object[] arguments) {
			StartCoroutine(InvokeDelayedRepeating(methodName, 0, repeatRate, arguments));
		}
	
		IEnumerator InvokeDelayed(string methodName, float delay, params object[] arguments) {
			yield return new WaitForSeconds(delay);
			InvokeMethod(methodName, arguments);
		}
	
		IEnumerator InvokeDelayedRepeating(string methodName, float delay, float repeatRate, params object[] arguments) {
			yield return new WaitForSeconds(delay);
		
			while (this != null && enabled) {
				InvokeMethod(methodName, arguments);
				yield return new WaitForSeconds(repeatRate);
			}
		}
	}
}

