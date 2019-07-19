using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollingRawImage : MonoBehaviour {

	private float _horizontalSpeed;
	private float _verticalSpeed;

	RawImage myRawImage;

	public void Start(){
		_verticalSpeed = 0.15f;
		_horizontalSpeed = 0f;
		myRawImage = GetComponent<RawImage> ();
	}
	public void Update()
	{
		Rect currentUv = myRawImage.uvRect;
		currentUv.x -= Time.deltaTime * _horizontalSpeed;
		currentUv.y -= Time.deltaTime * _verticalSpeed;

		if(currentUv.x <= -1f || currentUv.x >= 1f)
		{
			currentUv.x = 0f;
		}

		if(currentUv.y <= -1f || currentUv.y >= 1f)
		{
			currentUv.y = 0f;
		}

		myRawImage.uvRect = currentUv;
	}
}
