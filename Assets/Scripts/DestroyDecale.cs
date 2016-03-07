using UnityEngine;
using System.Collections;

public class DestroyDecale : MonoBehaviour
{

    #region Public Fields & Properties
	public float time = 5f;
    #endregion

    #region Private Fields & Properties
	private float alpha = 1f;

	Renderer _renderer;
    #endregion

	#region System Methods
	// Use this for initialization
	private void Start()
	{
		_renderer = GetComponent<Renderer> ();
	}

    // Update is called once per frame
    private void Update()
    {
		time -= Time.deltaTime;
		if (time < 0f) 
		{
			alpha -= (Time.deltaTime / 3f);
			Color texttureColor = _renderer.material.color;
			texttureColor.a = alpha;
			_renderer.material.color = texttureColor;
		}

		if (alpha < 0f)
		{
			Destroy (transform.parent.gameObject);
		}
    }
    #endregion
}
