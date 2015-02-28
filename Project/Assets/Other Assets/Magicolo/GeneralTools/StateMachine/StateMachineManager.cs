using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class StateMachineManager : MonoBehaviourExtended {

	static StateMachineManager instance;
	static StateMachineManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<StateMachineManager>();
				
				if (instance == null) {
					GameObject gameObject = new GameObject("State Machine Manager");
					gameObject.hideFlags = HideFlags.HideInHierarchy;
					instance = gameObject.AddComponent<StateMachineManager>();
				}
			}
			
			return instance;
		}
	}
    
	readonly List<StateMachine> machines = new List<StateMachine>();
	readonly List<GameObject> machineObjects = new List<GameObject>();
	
	public static void AddMachine(StateMachine machine) {
		Instance.machines.Add(machine);
		Instance.machineObjects.Add(machine.gameObject);
	}
	
	public static void RemoveMachine(StateMachine machine) {
		if (instance != null) {
			Instance.machines.Remove(machine);
			Instance.machineObjects.Remove(machine.gameObject);
		}
	}
	
	void Awake() {
		this.SetExecutionOrder(15);
	}
	
	void Update() {
		for (int i = machines.Count - 1; i >= 0; i--) {
			StateMachine machine = machines[i];
			GameObject machineObject = machineObjects[i];
			
			if (machine.enabled && machineObject.activeInHierarchy) {
				machine.OnUpdate();
			}
		}
	}
	
	void FixedUpdate() {
		for (int i = machines.Count - 1; i >= 0; i--) {
			StateMachine machine = machines[i];
			GameObject machineObject = machineObjects[i];
			
			if (machine.enabled && machineObject.activeInHierarchy) {
				machine.OnFixedUpdate();
			}
		}
	}
		
	void LateUpdate() {
		for (int i = machines.Count - 1; i >= 0; i--) {
			StateMachine machine = machines[i];
			GameObject machineObject = machineObjects[i];
			
			if (machine.enabled && machineObject.activeInHierarchy) {
				machine.OnLateUpdate();
			}
		}
	}
	
	void OnDestroy() {
		instance = null;
	}
}

