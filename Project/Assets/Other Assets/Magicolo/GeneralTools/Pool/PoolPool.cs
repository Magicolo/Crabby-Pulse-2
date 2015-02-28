using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]  
	public class PoolPool : INamable {

		[SerializeField]
		string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public PoolPrefab prefab;
		public Pool pool;
		
		public PoolPool(Pool pool) {
			this.pool = pool;
		}
		
		public void Initialize(Pool pool) {
			this.pool = pool;
		}

		public void Start() {
			
		}
	}
}
