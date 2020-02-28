using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToHighlight : MonoBehaviour
{
	public Material mat_highlight;
	public Material mat_normal;
	private Highlight[] camscript;
    private GameObject see;

    void Start()
	{
		camscript = Object.FindObjectsOfType<Highlight>();
	}

    // Update is called once per frame
    void Update()
    {
        foreach (Highlight script in camscript)
        {
            if (script.see != null)
            {
                see = script.see;
            }
        }
		   

        if(GameObject.ReferenceEquals(gameObject, see))
		{
			GetComponent<Renderer>().material = mat_highlight;
		}
		else
		{
			GetComponent<Renderer>().material = mat_normal;
		}
    }
}
