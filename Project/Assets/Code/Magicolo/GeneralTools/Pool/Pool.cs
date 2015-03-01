using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.GeneralTools;

[ExecuteInEditMode]
public class Pool : MonoBehaviour {

	static Pool instance;
	static Pool Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<Pool>();
			}
			
			return instance;
		}
	}
	
	#region Components
	public PoolPrefabManager prefabManager;
	public PoolPoolManager poolManager;
	public PoolEditorHelper editorHelper;
	public PoolHierarchyManager hierarchyManager;
	#endregion
	
	public void Initialize() {
		if (SingletonCheck()) {
			return;
		}
		
		InitializeManagers();
		
		if (Application.isPlaying) {
			StartAll();
		}
	}
	
	public void InitializeManagers() {
		prefabManager = prefabManager ?? new PoolPrefabManager(Instance);
		prefabManager.Initialize(Instance);
		
		poolManager = poolManager ?? new PoolPoolManager(Instance);
		poolManager.Initialize(Instance);
		
		hierarchyManager = hierarchyManager ?? new PoolHierarchyManager(Instance);
		hierarchyManager.Initialize(Instance);
	}
	
	public void InitializeHelpers() {
		editorHelper = editorHelper ?? new PoolEditorHelper(Instance);
		editorHelper.Initialize(Instance);
	}
	
	public void StartAll() {
		poolManager.Start();
	}
	
	public bool SingletonCheck() {
		if (Instance != null && Instance != this) {
			if (!Application.isPlaying) {
				Logger.LogError(string.Format("There can only be one {0}.", GetType().Name));
			}
			
			gameObject.Remove();
				
			return true;
		}
			
		return false;
	}

	void Awake() {
		Initialize();
	}
	
	#if UNITY_EDITOR
	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnReloadScripts() {
		if (Instance != null) {
			Instance.InitializeHelpers();
		}
	}
	#endif
}
