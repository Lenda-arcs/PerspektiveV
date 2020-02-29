using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string scene_name = SceneManager.GetActiveScene().name;

        string logpath = System.IO.Directory.GetCurrentDirectory() + @"\Log\results.txt";
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(logpath, true))
        {
            file.WriteLine(scene_name);
        }
    }

}
