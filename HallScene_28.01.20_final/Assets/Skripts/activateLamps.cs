using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateLamps : MonoBehaviour
{
    public bool activate;
    public Material mat_lampe_an;
    public Material mat_lampe_aus;

    // Start is called before the first frame update
    void Start()
    {
        activate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activate == true)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().material = mat_lampe_an;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().material = mat_lampe_aus;
            }
        }
    }
}
