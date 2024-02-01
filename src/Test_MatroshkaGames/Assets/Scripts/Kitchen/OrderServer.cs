using System.Collections.Generic;
using UnityEngine;

using CookingPrototype.Controllers;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {

	[RequireComponent(typeof(OrderPlace))]
	public sealed class OrderServer : MonoBehaviour, IPointerClickHandler {
		private OrderPlace _orderPlace;

		void Start() {
			_orderPlace = GetComponent<OrderPlace>();
		}

		private void TryServeOrder() {
			var order = OrdersController.Instance.FindOrder(_orderPlace.CurOrder);
			var customer = CustomersController.Instance.FindCustomer(order);
			if (!GameplayController.Instance.TryServeOrder(customer, order)) {
				return;
			}

			_orderPlace.FreePlace();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			TryServeOrder();
		}
	}
}
