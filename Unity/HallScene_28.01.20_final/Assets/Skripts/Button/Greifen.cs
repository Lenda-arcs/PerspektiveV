using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Greifen : MonoBehaviour
{

    private bool movement;

    public GameObject rope;
    public GameObject ziel_obj;
    public GameObject abstellort;
    public GameObject grabber;
    public GameObject lamps;
    public bool moving;
    public float grab_rad;
    private bool grabber_free;
    private float startPos;
    private float last_float;
    private string scene_name;
    private bool ziel_collison;


    // Start is called before the first frame update
    void Start()
    {
        movement = false;
        grabber_free = true;
        moving = false;
        startPos = rope.transform.position.y;
        last_float = -0.1f;
        ziel_collison = ziel_obj.GetComponent<collision_detect>().obj_collision;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabber_free)
        {
            RaycastHit hit;
            Vector3 rayOrigin = rope.transform.position;
            rayOrigin.y -= rope.transform.lossyScale.y / 2;
            Ray downRay = new Ray(rayOrigin, -Vector3.up);
            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay, out hit))
            {
                // Moving Down
                if (hit.distance >= 0.1 && movement)
                {
                    rope.transform.localScale += new Vector3(0, 0.05f, 0);
                    rope.transform.position -= new Vector3(0, 0.025f, 0);

                    grabber.transform.position -= new Vector3(0, 0.05f, 0);
                    moving = true;
                    
                }

                // Object hit
                else if (movement)
                {
                    movement = false;
                    grabber.AddComponent<FixedJoint>();
                    FixedJoint magnetJoint = grabber.GetComponent<FixedJoint>();
                    magnetJoint.connectedBody = ziel_obj.GetComponent<Rigidbody>();
                }

                // Moving Up
                else if (!movement && !(rope.transform.position.y == startPos))
                {
                    rope.transform.localScale += new Vector3(0, -0.05f, 0);
                    rope.transform.position -= new Vector3(0, -0.025f, 0);

                    grabber.transform.position -= new Vector3(0, -0.05f, 0);
                }

                // End of Movement and occupied
                else if (grabber.GetComponent<FixedJoint>() != null)
                {
                    moving = false;
                    grabber_free = false;
                }
            }
        }
        else
        {
            RaycastHit hit_2;
            Vector3 rayOrigin_2 = ziel_obj.transform.position;
            rayOrigin_2.y -= ziel_obj.transform.lossyScale.y / 2;
            Ray downRay_2 = new Ray(rayOrigin_2, -Vector3.up);
            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay_2, out hit_2))
            {
                if (hit_2.distance >= 0.1 && !ziel_collison && movement)
                {
                    rope.transform.localScale += new Vector3(0, 0.05f, 0);
                    rope.transform.position -= new Vector3(0, 0.025f, 0);

                    grabber.transform.position -= new Vector3(0, 0.05f, 0);
                    moving = true;
                }

                else if (movement)
                {
                    movement = false;
                    Destroy(grabber.GetComponent<FixedJoint>());
                }

                else if (!movement && !(rope.transform.position.y == startPos))
                {
                    rope.transform.localScale += new Vector3(0, -0.05f, 0);
                    rope.transform.position -= new Vector3(0, -0.025f, 0);

                    grabber.transform.position -= new Vector3(0, -0.05f, 0);
                }

                else if (grabber.GetComponent<FixedJoint>() == null)
                {
                    moving = false;
                    grabber_free = true;
                }
            }
        }
    }

    public void AktiviereMovementGreifen()
    {
        RaycastHit hit;
        Vector3 rayOrigin_kran = rope.transform.position;
        rayOrigin_kran.y -= rope.transform.lossyScale.y / 2;
        Ray downRay_kran = new Ray(rayOrigin_kran, -Vector3.up);

        Vector3 rayOrigin_ziel = ziel_obj.transform.position;
        rayOrigin_ziel.y -= ziel_obj.transform.lossyScale.y / 2;
        Ray downRay_ziel = new Ray(rayOrigin_ziel, -Vector3.up);

        // Cast a ray straight downwards.
        if (grabber_free && Physics.Raycast(downRay_kran, out hit))
        {
            Vector3 hit_pos = hit.point;
            Vector3 ziel_pos = ziel_obj.transform.position;
            
            float hit_ziel_dist = Mathf.Sqrt(Mathf.Pow((hit_pos.x - ziel_pos.x), 2) + Mathf.Pow((hit_pos.z - ziel_pos.z), 2));

            // Hier in Datei schreiben
            if (!((last_float - 0.001) <= hit_ziel_dist && hit_ziel_dist <= (last_float + 0.001)))
            {
                string hit_ziel_dist_str = "pick:" + hit_ziel_dist.ToString() + ',';
                string logpath = System.IO.Directory.GetCurrentDirectory() + @"\Log\results.txt";
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(logpath, true))
                {
                    file.Write(hit_ziel_dist_str);
                }
            }

            last_float = hit_ziel_dist;

            if (hit_ziel_dist < grab_rad)
            {
                movement = true;
            }
            else
            {
                // Aufgerufen, wenn Greifen an falscher Stelle
                StartCoroutine(fail());
                IEnumerator fail()
                {
                    // This will wait 1 second like Invoke could do, remove this if you don't need it

                    grabber.GetComponent<AudioSource>().Play();
                    float timePassed = 0;
                    while (timePassed < 1)
                    {
                        // Code to go left here
                        timePassed += Time.deltaTime;
                        lamps.GetComponent<activateLamps>().activate = true;
                        yield return null;
                    }
                    lamps.GetComponent<activateLamps>().activate = false;
                }
            }
        }
        else if (!grabber_free && Physics.Raycast(downRay_ziel, out hit))
        {
            Vector3 hit_pos = hit.point;
            Vector3 abstellort_pos = abstellort.transform.position;

            float hit_abstellort_dist = Mathf.Sqrt(Mathf.Pow((hit_pos.x - abstellort_pos.x), 2) + Mathf.Pow((hit_pos.z - abstellort_pos.z), 2));
            Debug.Log(last_float != hit_abstellort_dist);
            // Hier in Datei schreiben
            if (!((last_float - 0.001) <= hit_abstellort_dist && hit_abstellort_dist <= (last_float + 0.001)))
            {
                string hit_abstellort_dist_str = hit_abstellort_dist.ToString();
                string logpath = System.IO.Directory.GetCurrentDirectory() + @"\Log\results.txt";
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(logpath, true))
                {
                    file.WriteLine("\n" + "put:" + hit_abstellort_dist_str);
                }
            }

            last_float = hit_abstellort_dist;

            if (!grabber_free)
            {
                movement = true;
            }
        }
    }
}

