using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public class StateMachineMonoBehaviour : MonoBehaviourExtended {

		public StateMachine machine;
	
		public new string name { get { return machine.name; } }
		public new bool enabled { get { return machine.enabled; } }
		public new string tag { get { return machine.tag; } }
		public new HideFlags hideFlags { get { return machine.hideFlags; } }
	
		public new GameObject gameObject { get { return machine.gameObject; } }
		public new Animation animation { get { return machine.animation; } }
		public new AudioSource audio { get { return machine.audio; } }
		public new Camera camera { get { return machine.camera; } }
		public new Collider collider { get { return machine.collider; } }
		public new Collider2D collider2D { get { return machine.collider2D; } }
		public new ConstantForce constantForce { get { return machine.constantForce; } }
		public new GUIElement guiElement { get { return machine.guiElement; } }
		public new GUIText guiText { get { return machine.guiText; } }
		public new GUITexture guiTexture { get { return machine.guiTexture; } }
		public new HingeJoint hingeJoint { get { return machine.hingeJoint; } }
		public new Light light { get { return machine.light; } }
		public new NetworkView networkView { get { return machine.networkView; } }
		public new ParticleEmitter particleEmitter { get { return machine.particleEmitter; } }
		public new ParticleSystem particleSystem { get { return machine.particleSystem; } }
		public new Renderer renderer { get { return machine.renderer; } }
		public new Rigidbody rigidbody { get { return machine.rigidbody; } }
		public new Rigidbody2D rigidbody2D { get { return machine.rigidbody2D; } }
		public new Transform transform { get { return machine.transform; } }

		public new void BroadcastMessage(string methodName, object parameter, SendMessageOptions options) {
			machine.BroadcastMessage(methodName, parameter, options);
		}
	
		public new void BroadcastMessage(string methodName, object parameter) {
			machine.BroadcastMessage(methodName, parameter);
		}
	
		public new void BroadcastMessage(string methodName) {
			machine.BroadcastMessage(methodName);
		}
	
		public new void BroadcastMessage(string methodName, SendMessageOptions options) {
			machine.BroadcastMessage(methodName, options);
		}

		public new bool CompareTag(string tag) {
			return machine.CompareTag(tag);
		}

		public new Component GetComponent(System.Type type) {
			return machine.GetComponent(type);
		}
	
		public new T GetComponent<T>() where T : Component {
			return machine.GetComponent<T>();
		}
	
		public new Component GetComponent(string type) {
			return machine.GetComponent(type);
		}

		public new Component GetComponentInChildren(System.Type type) {
			return machine.GetComponentInChildren(type);
		}
	
		public new T GetComponentInChildren<T>() where T : Component {
			return machine.GetComponentInChildren<T>();
		}
	
		public new Component GetComponentInParent(System.Type type) {
			return machine.GetComponentInParent(type);
		}
	
		public new T GetComponentInParent<T>() where T : Component {
			return machine.GetComponentInParent<T>();
		}
	
		public new Component[] GetComponents(System.Type type) {
			return machine.GetComponents(type);
		}
	
		public new T[] GetComponents<T>() where T : Component {
			return machine.GetComponents<T>();
		}
	
		public new void GetComponents<T>(List<T> result) where T : Component {
			machine.GetComponents<T>(result);
		}
	
		public new void GetComponents(System.Type type, List<Component> result) {
			machine.GetComponents(type, result);
		}

		public new Component[] GetComponentsInChildren(System.Type type) {
			return machine.GetComponentsInChildren(type);
		}
	
		public new Component[] GetComponentsInChildren(System.Type type, bool includeInactive) {
			return machine.GetComponentsInChildren(type, includeInactive);
		}
		
		public new T[] GetComponentsInChildren<T>() where T : Component {
			return machine.GetComponentsInChildren<T>();
		}
	
		public new T[] GetComponentsInChildren<T>(bool includeInactive) where T : Component {
			return machine.GetComponentsInChildren<T>(includeInactive);
		}
	
		public new void GetComponentsInChildren<T>(List<T> result) where T : Component {
			machine.GetComponentsInChildren<T>(result);
		}

		public new void GetComponentsInChildren<T>(bool includeInactive, List<T> result) where T : Component {
			machine.GetComponentsInChildren<T>(includeInactive, result);
		}
	
		public new Component[] GetComponentsInParent(System.Type type) {
			return machine.GetComponentsInParent(type);
		}
	
		public new Component[] GetComponentsInParent(System.Type type, bool includeInactive) {
			return machine.GetComponentsInParent(type, includeInactive);
		}
		
		public new T[] GetComponentsInParent<T>() where T : Component {
			return machine.GetComponentsInParent<T>();
		}
	
		public new T[] GetComponentsInParent<T>(bool includeInactive) where T : Component {
			return machine.GetComponentsInParent<T>(includeInactive);
		}
	}
}
