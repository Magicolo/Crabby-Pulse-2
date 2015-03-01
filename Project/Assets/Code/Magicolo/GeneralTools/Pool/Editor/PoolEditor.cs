using System.Collections;
using Magicolo.EditorTools;
using Magicolo.GeneralTools;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[CustomEditor(typeof(Pool))]
	public class PoolEditor : CustomEditorBase {

		Pool pool;
		
		public override void OnEnable() {
			base.OnEnable();
			
			pool = (Pool)target;
			pool.SetExecutionOrder(-15);
			pool.hierarchyManager.FreezeTransforms();
		}
		
		public override void OnInspectorGUI() {
			pool.hierarchyManager.FreezeTransforms();
			
			Begin();
			
			ShowPrefabs();
			
			End();
		}

		void ShowPrefabs() {
			throw new System.NotImplementedException();
		}
	}
}