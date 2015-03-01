using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo {
	[AddComponentMenu("Magicolo/State Machine")]
	public class StateMachine : MonoBehaviourExtended {

		[SerializeField] StateLayer[] layers = new StateLayer[0];
		Dictionary<string, IStateLayer> nameLayerDict;
		
		void Awake() {
			StateMachineManager.AddMachine(this);
			BuildLayerDict();
			
			foreach (StateLayer layer in layers) {
				layer.OnAwake();
			}
		}
	
		//		void OnEnable() {
		//			StateMachineManager.AddMachine(this);
		//		}
		//
		//		void OnDisable() {
		//			StateMachineManager.RemoveMachine(this);
		//		}
		
		void Start() {
			foreach (StateLayer layer in layers) {
				layer.OnStart();
			}
		}
		
		void OnDestroy() {
			StateMachineManager.RemoveMachine(this);
			
			State[] states = GetComponents<State>();
			foreach (State state in states) {
				if (layers.Contains(state.layer)) {
					Destroy(state);
				}
			}
			
			foreach (StateLayer layer in layers) {
				Destroy(layer);
			}
		}
		
		public void OnUpdate() {
			foreach (StateLayer layer in layers) {
				layer.OnUpdate();
			}
		}
	
		public void OnFixedUpdate() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnFixedUpdate();
			}
		}
		
		public void OnLateUpdate() {
			for (int i = 0; i < layers.Length; i++) {
				layers[i].OnLateUpdate();
			}
		}

//		public void OnCollisionEnter(Collision collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionEnter(collision);
//			}
//		}
//	
//		public void OnCollisionStay(Collision collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionStay(collision);
//			}
//		}
//
//		public void OnCollisionExit(Collision collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionExit(collision);
//			}
//		}
//	
//		public void OnCollisionEnter2D(Collision2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionEnter2D(collision);
//			}
//		}
//	
//		public void OnCollisionStay2D(Collision2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionStay2D(collision);
//			}
//		}
//
//		public void OnCollisionExit2D(Collision2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].CollisionExit2D(collision);
//			}
//		}
//	
//		public void OnTriggerEnter(Collider collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerEnter(collision);
//			}
//		}
//	
//		public void OnTriggerStay(Collider collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerStay(collision);
//			}
//		}
//
//		public void OnTriggerExit(Collider collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerExit(collision);
//			}
//		}
//	
//		public void OnTriggerEnter2D(Collider2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerEnter2D(collision);
//			}
//		}
//	
//		public void OnTriggerStay2D(Collider2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerStay2D(collision);
//			}
//		}
//
//		public void OnTriggerExit2D(Collider2D collision) {
//			for (int i = 0; i < layers.Length; i++) {
//				layers[i].TriggerExit2D(collision);
//			}
//		}

		public T GetLayer<T>() where T : IStateLayer {
			return (T)GetLayer(typeof(T).Name);
		}
		
		public IStateLayer GetLayer(System.Type layerType) {
			return GetLayer(layerType.Name);
		}
		
		public IStateLayer GetLayer(string layerName) {
			IStateLayer layer = null;
			
			try {
				layer = nameLayerDict[layerName];
			}
			catch {
				Logger.LogError(string.Format("Layer named {0} was not found.", layerName));
			}
			
			return layer;
		}
		
		public IStateLayer GetLayer(int layerIndex) {
			IStateLayer layer = null;
			
			try {
				layer = layers[layerIndex];
			}
			catch {
				Logger.LogError(string.Format("Layer at index {0} was not found.", layerIndex));
			}
			
			return layer;
		}
		
		public IStateLayer[] GetLayers() {
			return layers.Clone() as IStateLayer[];
		}
		
		void BuildLayerDict() {
			nameLayerDict = new Dictionary<string, IStateLayer>();
			
			foreach (StateLayer layer in layers) {
				if (layer != null) {
					nameLayerDict[layer.GetType().Name] = layer;
				}
			}
		}
	}
}
