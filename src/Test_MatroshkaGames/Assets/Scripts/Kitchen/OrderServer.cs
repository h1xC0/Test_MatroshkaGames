using System.Collections.Generic;
using UnityEngine;

using CookingPrototype.Controllers;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(OrderPlace))]
	[RequireComponent(typeof(OrderPresenter))]
	public sealed class OrderServer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		private OrderPlace _orderPlace;
		private OrderPresenter _orderPresenter;

		private Customer _customer;

		private Vector2 _initialPosition;

		void Start() {
			_orderPlace = GetComponent<OrderPlace>();
			_orderPresenter = GetComponent<OrderPresenter>();
		}

		private void TryServeOrder() {
			var order = OrdersController.Instance.FindOrder(_orderPlace.CurOrder);
			if ( (order == null) || !GameplayController.Instance.TryServeOrder(_customer, order) ) {
				return;
			}

			_orderPlace.FreePlace();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_initialPosition = _orderPresenter.Visualizer.transform.position;
		}

		public void OnDrag(PointerEventData eventData)
		{
			var visualizer = (RectTransform)_orderPresenter.Visualizer.transform;

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(visualizer, eventData.position, eventData.pressEventCamera, out var globalMousePos))
			{
				visualizer.position = globalMousePos;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_orderPresenter.Visualizer.transform.position = _initialPosition;

			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, results);

			foreach (var raycastResult in results)
			{
				_customer = raycastResult.gameObject.GetComponentInParent<Customer>();

				if (_customer == null)
				{
					continue;
				}
				
				TryServeOrder();
				break;
			}
		}
	}
}
