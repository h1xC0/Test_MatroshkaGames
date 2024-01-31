using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour, IPointerClickHandler {

		FoodPlace _place = null;

		private float _lastClick;
		private float _interval = 0.5f;
		
		void Start() {
			_place = GetComponent<FoodPlace>();
		}

		/// <summary>
		/// Освобождает место по двойному тапу если еда на этом месте сгоревшая.
		/// </summary>
		private void TryTrashFood() {
			if (_place.CurFood.CurStatus == Food.FoodStatus.Overcooked)
			{
				_place.FreePlace();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_lastClick + _interval > Time.time)
			{
				Debug.Log("Double Click");
				TryTrashFood();
			}
			else
			{
				Debug.Log("Single Click");
				_lastClick = Time.time;
			}
		}
	}
}
