﻿using UnityEngine;

namespace Geekbrains
{
    public class Geekbrains : MonoBehaviour
    {
	    [SerializeField] private bool _isAllowScaling;
	    [SerializeField] private float ActiveDis;

	    private void OnDrawGizmos()
	  {
		Gizmos.DrawIcon(transform.position, "bot.jpg", _isAllowScaling);
	  }

      private void OnDrawGizmosSelected()
      {
#if UNITY_EDITOR
	      Transform t = transform;

	      //Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, t.localScale);
	      //Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

	      var flat = new Vector3(ActiveDis, 0, ActiveDis);
	      Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, flat);
	      Gizmos.DrawWireSphere(Vector3.zero, 5);
#endif
      }

	}
}
