using UnityEngine;
using System.Collections;

public class FitCanvas : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        var rectTransform = GetComponent<RectTransform>();
        var canvas = GetComponentInParent<Canvas>();
        if (rectTransform && canvas)
        {
            var canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
            rectTransform.sizeDelta = canvasSize;
        }
	}
}
