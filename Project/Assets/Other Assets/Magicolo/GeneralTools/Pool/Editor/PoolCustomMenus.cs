using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Magicolo.GeneralTools {
	public static class PoolCustomMenus {
	
		[MenuItem("Magicolo's Tools/Create/Pool")]
		static void CreatePool() {
			GameObject gameObject;
			Pool existingPool = Object.FindObjectOfType<Pool>();
		
			if (existingPool == null) {
				gameObject = new GameObject();
				gameObject.name = "Pool";
				gameObject.AddComponent<Pool>();
				Undo.RegisterCreatedObjectUndo(gameObject, "Pool Created");
			}
			else {
				gameObject = existingPool.gameObject;
			}
			
			Selection.activeGameObject = gameObject;
		}
	}
}