using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver_Fail : MonoBehaviour
{
    private bool fail;

    // Start is called before the first frame update
    void Start()
    {
        fail = false;
    }

    // Update is called once per frame
    void Update()
    {
        fail = GetComponent<Reciever_Halterung>().fail;

        if (fail)
        {
            // Aufgerufen, wenn Greifen an falscher Stelle
            StartCoroutine(fail());
            IEnumerator fail()
            {
                // This will wait 1 second like Invoke could do, remove this if you don't need it

                GetComponentInChildren<AudioSource>().Play();
                float timePassed = 0;
                while (timePassed < 1)
                {
                    // Code to go left here
                    timePassed += Time.deltaTime;
                    GetComponentInChildren<activateLamps>().activate = true;
                    yield return null;
                }
                GetComponentInChildren<activateLamps>().activate = false;
            }
        }
    }
}
