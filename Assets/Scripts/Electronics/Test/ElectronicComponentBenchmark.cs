using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicComponentBenchmark : MonoBehaviour
{
    public int numOfNodes = 100;
    public int maxBranchNodes = 10;
    public float chance = 0.1f;

    void Start()
    {
        ElectronicComponent prevEC = null;

        for (int i = 0; i < numOfNodes; i++)
        {
            GameObject go = new GameObject("Test");
            go.transform.position = new Vector3(i * 2, 0, 0);

            var connectionTest = go.AddComponent<ConnectionTest>();

            if (Random.value < chance)
            {
                ElectronicComponent prevECVert = connectionTest;

                int size = Random.Range(1, maxBranchNodes);
                for (int j = 0; j < size; j++)
                {
                    GameObject goVert = new GameObject("Test");
                    goVert.transform.position = new Vector3(i * 2, (j + 1) * 2, 0);

                    var cTVert = goVert.AddComponent<ConnectionTest>();

                    cTVert.ConnectTo(prevECVert);

                    if (Random.value < 0.05f)
                        goVert.AddComponent<PowerSource>();

                    prevECVert = cTVert;
                }
            }

            if (i != 0)
                connectionTest.ConnectTo(prevEC);

            prevEC = connectionTest;
        }


        prevEC.gameObject.AddComponent<PowerSource>();

        prevEC.UpdatePowerState();
    }
}
