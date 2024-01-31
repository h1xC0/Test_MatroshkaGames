using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTransfer : MonoBehaviour, IPointerClickHandler {
		public bool                    OnlyTransferCooked = true;
		public List<AbstractFoodPlace> DestPlaces         = new List<AbstractFoodPlace>();

		FoodPlace _place = null;

		void Start() {
			_place = GetComponent<FoodPlace>();
		}

		private void TryTransferFood() {
			var food = _place.CurFood;

			if ( food == null ) {
				return;
			}

			if ( OnlyTransferCooked && (food.CurStatus != Food.FoodStatus.Cooked) ) {
				_place.TryPlaceFood(food);
				return;
			}
			foreach ( var place in DestPlaces ) {
				if ( !place.TryPlaceFood(food) ) {
					continue;
				}
				_place.FreePlace();
				return;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("Transfer Click");

			TryTransferFood();
		}
	}
}
