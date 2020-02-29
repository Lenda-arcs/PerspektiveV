using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinten : MonoBehaviour
{
    // Start is called before the first frame update

    private bool movement;

    public GameObject kran;
    public GameObject ziel;

    private GameObject Scriptholder;
    private Greifen greifSkript;

    public float movementSpeed;

    void Start()
    {
        movement = false;
        Scriptholder = GameObject.Find("ScriptHolder");
        greifSkript = (Greifen)Scriptholder.GetComponent(typeof(Greifen));
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit_kran;
        Vector3 rayorigin_kran = kran.transform.position;
        rayorigin_kran.z -= kran.transform.GetComponent<Renderer>().bounds.size.z / 2;

        RaycastHit hit_ziel;
        Vector3 rayorigin_ziel = ziel.transform.position;
        rayorigin_ziel.z -= ziel.transform.GetComponent<Renderer>().bounds.size.z / 2;

        bool kranRay = Physics.Raycast(rayorigin_kran, kran.transform.TransformDirection(Vector3.back), out hit_kran);
        bool zielRay = Physics.Raycast(rayorigin_ziel, ziel.transform.TransformDirection(Vector3.back), out hit_ziel);
        

        if (kranRay && zielRay)
        {
            
            if (movement && !greifSkript.moving && hit_kran.distance > 0.1 && hit_ziel.distance > 0.1)
            {
                kran.transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
            }
        }
    }

    public void AktiviereMovementHinten()
    {
        movement = true;
    }

    public void DeaktiviereMovementHinten()
    {
        movement = false;
    }

}
