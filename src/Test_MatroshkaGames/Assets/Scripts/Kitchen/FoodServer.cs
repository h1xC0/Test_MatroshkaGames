using System.Collections.Generic;

using UnityEngine;

using CookingPrototype.Controllers;
using UnityEngine.EventSystems;

namespace CookingPrototype.Kitchen {
	
	[RequireComponent(typeof(FoodPlace))]
	[RequireComponent(typeof(FoodPresenter))]
	public sealed class FoodServer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		private FoodPlace _place = null;
		private FoodPresenter _presenter;

		private Vector2 _initialPosition;

		private Customer _customer;

		void Start() {
			_place = GetComponent<FoodPlace>();
			_presenter = GetComponent<FoodPresenter>();
		}

		private bool TryServeFood() {
			if ( _place.IsFree || (_place.CurFood.CurStatus != Food.FoodStatus.Cooked) ) {
				return false;
			}
			var order = OrdersController.Instance.FindOrder(new List<string>(1) { _place.CurFood.Name });
			if ( (order == null) || !GameplayController.Instance.TryServeOrder(_customer, order) ) {
				return false;
			}

			_place.FreePlace();
			return true;
		}
		
		public void OnBeginDrag(PointerEventData eventData)
		{
			_initialPosition = _presenter.transform.position;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (_place.IsCooking) return;

			var visualizer = (RectTransform)_presenter.transform;

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(visualizer, eventData.position, eventData.pressEventCamera, out var globalMousePos))
			{
				visualizer.position = globalMousePos;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_place.IsCooking) return;

			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, results);
			foreach (var raycastResult in results)
			{
				_presenter.transform.position = _initialPosition;
				_customer = raycastResult.gameObject.GetComponentInParent<Customer>();

				if (_customer == null)
				{
					continue;
				}
				
				TryServeFood();
				break;
			}
		}
	}
}
