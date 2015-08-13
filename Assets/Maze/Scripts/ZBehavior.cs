using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Just a classic better monobehavior
/// 
///     Credit for _cachedComponents & general start goes to FlyClops (MonoBehavclops)
/// </summary>

public class ZBehaviour : MonoBehaviour 
{
	private Dictionary<Type,Component> _cachedComponents;

	public T Cached<T>() where T : Component
	{
		if (_cachedComponents == null) _cachedComponents = new Dictionary<Type,Component>();
		if (_cachedComponents.ContainsKey(typeof(T))) return (T)_cachedComponents[typeof(T)];
		T component = GetComponent<T>();
		_cachedComponents.Add(typeof(T),component);
		return component;
	}
}
