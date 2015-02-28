using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class PoolPrefabManager {

		public PoolPrefab[] prefabs = new PoolPrefab[0];
		public Pool pool;
		
		Dictionary<string, PoolPrefab> prefabDict;
		
		public PoolPrefabManager(Pool pool) {
			this.pool = pool;
		}
		
		public void Initialize(Pool pool) {
			this.pool = pool;
		}
		
		public void BuildPrefabDict() {
			prefabDict = new Dictionary<string, PoolPrefab>();
			
			foreach (PoolPrefab prefab in prefabs) {
				prefabDict[prefab.Name] = prefab;
			}
		}
		
		public PoolPrefab GetPrefab(string prefabName) {
			PoolPrefab prefab = null;
			
			try {
				prefab = prefabDict[prefabName];
			}
			catch {
				Logger.LogError(string.Format("Prefab named {0} was not found.", prefabName));
			}
			
			return prefab;
		}
	}
}
