using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    private float starttime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startTimer()
    {
        starttime = Time.time;
    }

    void endTimer()
    {
        float time_diff = Time.time - starttime;
        using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\7manka\Desktop\Projekt\kranMitGrabVer3\New Unity Project\Assets\Results\results.txt", true))
        {
            file.WriteLine('\n' + "time:" + time_diff.ToString());
        }
    }
}
