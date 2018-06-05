using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class YousoroadOld : MonoBehaviour {
    enum Action { aDefault, aDown, aUp };

    public string tag;
    //public GameObject[] objects;
    public VideoPlayer videoPlayer;
    private Action action;
    public List<GameObject> objects;
    public List<List<GameObject>> rowObjects;
    public GameObject lookAtObject;


    private Material[] pColor;
    private bool colorOff;
    private float timeComparison;
    private int numRow;

    private float lastVal;


    // Use this for initialization
    void Start()
    {
        //Load Penlight Colors
        pColor = new Material[10];
        pColor[0] = Resources.Load("Materials/UO_Orange", typeof(Material)) as Material;
        pColor[1] = Resources.Load("Materials/UO_Lightpink", typeof(Material)) as Material;
        pColor[2] = Resources.Load("Materials/UO_Lightgreen", typeof(Material)) as Material;
        pColor[3] = Resources.Load("Materials/UO_Red", typeof(Material)) as Material;
        pColor[4] = Resources.Load("Materials/UO_Aqua", typeof(Material)) as Material;
        pColor[5] = Resources.Load("Materials/UO_White", typeof(Material)) as Material;
        pColor[6] = Resources.Load("Materials/UO_Yellow", typeof(Material)) as Material;
        pColor[7] = Resources.Load("Materials/UO_Purple", typeof(Material)) as Material;
        pColor[8] = Resources.Load("Materials/UO_Pink", typeof(Material)) as Material;
        pColor[9] = Resources.Load("Materials/UO_Black", typeof(Material)) as Material;

        // Add all objects with tag into list
        objects = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));

        // Sort objects by distance. Y component is ignored.
        objects.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector3.Distance(new Vector3(0.9f,0,32.38f), new Vector3(a.transform.position.x, 0, a.transform.position.z)).CompareTo
            ((Vector3.Distance(new Vector3(0.9f, 0, 32.38f), new Vector3(b.transform.position.x,0,b.transform.position.z))));
        });

        // Compute objects per row
        rowObjects = new List<List<GameObject>>();
        rowObjects.Add(new List<GameObject>());
        int currRow = 0;
        float threshold = 0.5f;
        float comparisonObject = 0.0f;

        if (objects.Count > 0)
            comparisonObject = objects[0].transform.position.z;

        for (int i = 0; i < objects.Count; i++)
        {
            if (Mathf.Abs(objects[i].transform.position.z - comparisonObject) < threshold)
            {
                rowObjects[currRow].Add(objects[i]);
            }
            else
            {
                rowObjects.Add(new List<GameObject>());
                rowObjects[currRow+1].Add(objects[i]);
                comparisonObject = objects[i].transform.position.z;
                currRow++;
            }
        }


        // Set random penlight color
        int randomColor = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            if (i%2 == 0)
                randomColor = Random.Range(0, 9);

            GameObject child = objects[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[randomColor];
        }

        // Orient penlights toward center screen
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.LookAt(lookAtObject.transform, objects[i].transform.up);
        }


        for (int i = 0; i < rowObjects.Count; i++) {
            string str = "Row: " + i + " Count: " + rowObjects[i].Count;
            Debug.Log(str);
        }


        // Set primative variables
        action = Action.aDefault;   // Default action state
        colorOff = false;           // Color off property
        timeComparison = 10.683f;   // Time to check against for start of Yousoroad
        numRow = 0;                 // Row counter
    }

    // Update is called once per frame
    void Update()
    {
        actionManager();

        //-45 to 45 deg and back
        switch (action)
        {
            case Action.aDefault:
                actionDefault();
                break;
            case Action.aDown:
                actionDown();
                break;
            case Action.aUp:
                actionYousoroad();
                break;
            default:
                break;
        }
    }

    void actionManager()
    {
        if (videoPlayer.time > 8.7f)
        {
            action = Action.aDown;
        }

        //16.80 end time
        //6 seconds of raising lights
        //85 rows total - 6pit - 45orch - 21 - (3) - (11)
        //0.07s per row
        if (videoPlayer.time >10.6f)
        {
            action = Action.aUp;
        }
    }

    void actionDefault()
    {
        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastVal, 0, 0));

        /*
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.rotation = objects[i].transform.rotation * localRotation;
            lastVal = val;
        }
        */

        for (int i = 0; i < rowObjects.Count; i++)
        {
            for (int j = 0; j < rowObjects[i].Count; j++)
            {
                rowObjects[i][j].transform.rotation = rowObjects[i][j].transform.rotation * localRotation;
                lastVal = val;
            }
        }
    }

    void actionDown()
    {
        disablePenlight();
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.rotation = Quaternion.Lerp(objects[i].transform.rotation, Quaternion.Euler(90, 0, 0), 0.05f);
        }
    }

    void actionYousoroad()
    {
        //10.6 start
        //16.80 end time
        //6 seconds of raising lights
        //86 rows total - 6pit - 45orch - 21 - (3) - (11)
        //72 rows current
        //0.083s per row/8

        //Debug.Log(videoPlayer.time);
        Debug.Log(numRow);
        if (numRow < 72)
            up(rowObjects[numRow]);

        if (videoPlayer.time > timeComparison)
        {
            timeComparison += 0.075f;
            numRow++;
            //Debug.Log(row);
        }
    }

    void up(List<GameObject> rowList)
    {

        /*
        //Debug.Log(i);
        int j = i * 8;
        while(j < (i*8)+8)
        {
            //Debug.Log(j);
            enablePenlightAqua(j);
            objects[j].transform.rotation = Quaternion.Lerp(objects[j].transform.rotation, Quaternion.Euler(0, 0, 0), 0.4f);
            j++;
        }
        */

        for (int i = 0; i < rowList.Count; i++) {
            enablePenlightAqua(rowList[i]);
            rowList[i].transform.rotation = Quaternion.Lerp(rowList[i].transform.rotation, Quaternion.Euler(0, 0, 0), 0.2f);
        }

    }

    void disablePenlight()
    {
        if (!colorOff)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject child = objects[i].transform.GetChild(0).gameObject;
                MeshRenderer mesh = child.GetComponent<MeshRenderer>();
                mesh.material = pColor[9];
            }

            colorOff = true;
        }
    }

    void enablePenlightAqua(GameObject g)
    {
            GameObject child = g.transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[4];
    }
}
