﻿using UnityEngine;

namespace Geekbrains
{
	public abstract class BaseObjectScene : MonoBehaviour
	{
		private int _layer;
		private Color _color;
		private bool _isVisible;
		[HideInInspector] public Rigidbody Rigidbody;

		#region UnityFunction

		protected virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}
		#endregion

		#region Property

		/// <summary>
		/// Имя объекта
		/// </summary>
		public string Name
		{
			get => gameObject.name;
			set => gameObject.name = value;
		}

		/// <summary>
		/// Слой объекта
		/// </summary>
		public int Layer
		{
			get => _layer;

			set
			{
				_layer = value;
				AskLayer(transform, value);
			}
		}

		/// <summary>
		/// Цвет материала объекта
		/// </summary>
		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				AskColor(transform, _color);
			}
		}

		public bool IsVisible
		{
			get => _isVisible;
			set
			{
				_isVisible = value;
				var tempRenderer = GetComponent<Renderer>();
				if (tempRenderer)
					tempRenderer.enabled = _isVisible;
				if (transform.childCount <= 0) return;
				foreach (Transform d in transform)
				{
					tempRenderer = d.gameObject.GetComponent<Renderer>();
					if (tempRenderer)
						tempRenderer.enabled = _isVisible;
				}
			}
		}

		#endregion

		#region PrivateFunction
		/// <summary>
		/// Выставляет слой себе и всем вложенным объектам в независимости от уровня вложенности
		/// </summary>
		/// <param name="obj">Объект</param>
		/// <param name="lvl">Слой</param>
		private void AskLayer(Transform obj, int lvl)
		{
			obj.gameObject.layer = lvl;
			if (obj.childCount <= 0) return;
			foreach (Transform d in obj)
			{
				AskLayer(d, lvl);
			}
		}

		private void AskColor(Transform obj, Color color)
		{
			foreach (var curMaterial in obj.GetComponent<Renderer>().materials)
			{
				curMaterial.color = color;
			}
			if (obj.childCount <= 0) return;
			foreach (Transform d in obj)
			{
				AskColor(d, color);
			}
		}
		#endregion
		
		/// <summary>
		/// Выключает физику у объекта и его детей
		/// </summary>
		public void DisableRigidBody()
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();
			foreach (var rb in rigidbodies)
			{
				rb.isKinematic = true;
			}
		}

		/// <summary>
		/// Включает физику у объекта и его детей
		/// </summary>
		public void EnableRigidBody(float force)
		{
			EnableRigidBody();
			//Rigidbody.isKinematic = false;
			Rigidbody.AddForce(transform.forward * force);
		}

		/// <summary>
		/// Включает физику у объекта и его детей
		/// </summary>
		public void EnableRigidBody()
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();
			foreach (var rb in rigidbodies)
			{
				rb.isKinematic = false;
			}
		}

		/// <summary>
		/// Замораживает или размораживает физическую трансформацию объекта
		/// </summary>
		/// <param name="rigidbodyConstraints">Трансформацию которую нужно заморозить</param>
		public void ConstraintsRigidBody(RigidbodyConstraints rigidbodyConstraints)
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();
			foreach (var rb in rigidbodies)
			{
				rb.constraints = rigidbodyConstraints;
			}
		}

		public void SetActive(bool value)
		{
			IsVisible = value;

			var tempCollider = GetComponent<Collider>();
			if (tempCollider)
			{
				tempCollider.enabled = value;
			}
		}

	}
}