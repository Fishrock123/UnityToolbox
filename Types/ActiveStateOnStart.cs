using UnityEngine;

namespace UnityToolbox
{
	public class ActiveStateOnStart : MonoBehaviour
	{
		public bool Active;
		[MustBeAssigned] public GameObject Target;

		private void Awake()
		{
			Target.gameObject.SetActive(Active);
		}
	}
}
