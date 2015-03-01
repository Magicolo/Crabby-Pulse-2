using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class PoolHierarchyManager {

		public Pool pool;
		
		public PoolHierarchyManager(Pool pool) {
			this.pool = pool;
		}
		
		public void Initialize(Pool pool) {
			this.pool = pool;
		}
		
		public void FreezeTransforms() {
			pool.transform.hideFlags = HideFlags.HideInInspector;
			pool.transform.Reset();
		}
	}
}