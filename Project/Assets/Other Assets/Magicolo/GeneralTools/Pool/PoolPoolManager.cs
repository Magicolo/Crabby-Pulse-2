using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class PoolPoolManager {

		public PoolPool[] pools = new PoolPool[0];
		public Pool pool;
    	
		Dictionary<string, PoolPool> poolDict;
		
		public PoolPoolManager(Pool pool) {
			this.pool = pool;
		}
		
		public void Initialize(Pool pool) {
			this.pool = pool;
		}
    	
		public void Start() {
			foreach (PoolPool poolPool in pools) {
				poolPool.Start();
			}
		}
		
		public void BuildPoolDict() {
			poolDict = new Dictionary<string, PoolPool>();
			
			foreach (PoolPool poolPool in pools) {
				poolDict[poolPool.Name] = poolPool;
			}
		}
		
		public PoolPool GetPool(string poolName) {
			PoolPool poolPool = null;
			
			try {
				poolPool = poolDict[poolName];
			}
			catch {
				Logger.LogError(string.Format("Prefab named {0} was not found.", poolName));
			}
			
			return poolPool;
		}
	}
}
