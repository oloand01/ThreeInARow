using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileImageController : MonoBehaviour
{
	public RectTransform donut { get; private set; }
	public Image donutImage { get; private set; }
	public Vector3 targetPosition { get;  set; }

	void Awake()
    {
		donut = GetComponent<RectTransform>();
		donutImage = GetComponent<Image>();
	}
}
