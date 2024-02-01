using System.Collections.Generic;

using UnityEngine;

using CookingPrototype.Controllers;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {
	
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodServer : MonoBehaviour, IPointerClickHandler {

		private FoodPlace _place = null;

		void Start() {
			_place = GetComponent<FoodPlace>();
		}

		private bool TryServeFood() {
			if ( _place.IsFree || (_place.CurFood.CurStatus != Food.FoodStatus.Cooked) ) {
				return false;
			}
			var order = OrdersController.Instance.FindOrder(new List<string>(1) { _place.CurFood.Name });
			var customer = CustomersController.Instance.FindCustomer(order);
			if (!GameplayController.Instance.TryServeOrder(customer, order)) {
				return false;
			}

			_place.FreePlace();
			return true;
		}
		
		public void OnPointerClick(PointerEventData eventData)
		{
			TryServeFood();
		}
	}
}
