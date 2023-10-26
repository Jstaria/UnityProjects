using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ForceManager : MonoBehaviour
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private GameObject massPrefab;
    [SerializeField] private bool blackholeAttraction = true;

    private List<GameObject> blackholes = new List<GameObject>();
    private List<GameObject> masses = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        blackholes.Add(Instantiate(blackholePrefab, Vector3.zero, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if (masses.Count < 1) { return; }

        if (blackholeAttraction)
        {
            foreach (GameObject blackhole in blackholes)
            {
                foreach (GameObject mass in masses)
                {
                    mass.GetComponent<MassBehavior>().ApplyForce(blackhole.GetComponent<BlackholeBehavior>().GetAttractionForce(mass.transform.position));

                    foreach (GameObject mass2 in masses)
                    {
                        if (mass == mass2) { continue; }
                        mass2.GetComponent<MassBehavior>().ApplyForce(mass.GetComponent<MassBehavior>().GetAttractionForce(mass2.transform.position));
                    }
                }
            }
        }

        if (!blackholeAttraction)
        {
            foreach (GameObject mass in masses)
            {
                foreach (GameObject mass2 in masses)
                {
                    if (mass == mass2) { continue; }
                    mass2.GetComponent<MassBehavior>().ApplyForce(mass.GetComponent<MassBehavior>().GetAttractionForce(mass2.transform.position));
                }
            }
        }

        for (int i = 0; i < blackholes.Count; i++)
        {
            for (int j = 0; j < masses.Count; j++)
            {
                if (CheckCollision(blackholes[i].GetComponent<BlackholeBehavior>(), masses[j].GetComponent<MassBehavior>()))
                {
                    GameObject.Destroy(masses[j]);
                    masses.RemoveAt(j);
                    j--;
                }
            }
        }
    }

    public bool CheckCollision(BlackholeBehavior b, MassBehavior m)
    {
        return (b.transform.position - m.transform.position).magnitude < b.Radius + m.Radius || (b.transform.position - m.transform.position).magnitude > 200;
    }

    public void SpawnMass(InputAction.CallbackContext context)
    {
        float maxSpeed = 200;
        if (context.started)
        {
            masses.Add(Instantiate(massPrefab, GetMousePos(), Quaternion.identity));
            masses[masses.Count - 1].GetComponent<MassBehavior>().SetVel(new Vector2(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed)));
            masses[masses.Count - 1].GetComponent<MassBehavior>().MaxSpeed = Random.Range(1, 20);
            //masses[masses.Count - 1].GetComponent<MassBehavior>().Mass = Random.Range(1, 20);
        }
    }

    private Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
