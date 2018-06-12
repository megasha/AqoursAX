using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Kananrail : MonoBehaviour {
    public class MyTransform
    {
        public Vector3 pos;
        public Vector3 rot;
    }

    enum Action { Default, Down, Kananrail, Together };

    enum SEC { CENTER_PIT_YOU,     // Kananrail splits this line and below
               CENTER_PIT_KAN, 
               LEFT_PIT_KAN,
               LEFT_PIT_NORM,
               RIGHT_PIT_KAN,
               RIGFHT_PIT_NORM,
               LEFT_LODGE_YOU,
               LEFT_LODGE_KAN,
               RIGHT_LODGE_YOU,
               RIGHT_LODGE_KAN,
               LEFT_LEFT_ORCH_NORM, // Kananrail/Normals this line and below
               LEFT_ORCH_KAN,
               CENTER_ORCH_NORM,
               RIGHT_ORCH_KAN,
               RIGHT_RIGHT_ORCH_NORM,
               LEFT_LEFT_LODGE_KAN,
               CENTER_LODGE_NORM,
               RIGHT_RIGHT_LODGE_KAN,
               MAX  };

    enum COLOR  {   ORANGE,
                    LIGHT_PINK,
                    LIGHT_GREEN,
                    RED,
                    AQUA,
                    WHITE,
                    YELLOW,
                    PURPLE,
                    PINK,
                    BLACK,
                    MAX };

    public VideoPlayer videoPlayer;
    public GameObject lookAtObject;

    public GameObject leftPitParent;
    public GameObject centerPitParent;
    public GameObject rightPitParent;
    public GameObject leftLeftOrchestraParent;
    public GameObject leftOrchestraParent;
    public GameObject centerOrchestraParent;
    public GameObject rightOrchestraParent;
    public GameObject rightRightOrchestraParent;
    public GameObject leftLeftLodgeParent;
    public GameObject leftLodgeParent;
    public GameObject centerLodgeParent;
    public GameObject rightLodgeParent;
    public GameObject rightRightLodgeParent;

    private Material[] pColor;

    // Master List
    private List<List<GameObject>> secList;

    // Random List
    private List<GameObject> randomListNormal;
    private List<GameObject> randomListAll;


    /* Kananrail Split Lists */
    private List<GameObject> centerPitYousoroad;
    private List<GameObject> centerPitKananrail;

    private List<GameObject> leftPitKananrail;
    private List<GameObject> leftPitNormalSplit;

    private List<GameObject> rightPitKananrail;
    private List<GameObject> rightPitNormalSplit;

    private List<GameObject> leftLodgeYousoroad;
    private List<GameObject> leftLodgeKananrail;

    private List<GameObject> rightLodgeYousoroad;
    private List<GameObject> rightLodgeKananrail;

    /* Kananrail/Normal Lists */
    private List<GameObject> leftLeftOrchestraNormal;
    private List<GameObject> leftOrchestraKananrail;
    private List<GameObject> centerOrchestraNormal;
    private List<GameObject> rightOrchestraKananrail;
    private List<GameObject> rightRightOrchestraNormal;
    private List<GameObject> leftLeftLodgeKananrail;
    private List<GameObject> centerLodgeNormal;
    private List<GameObject> rightRightLodgeKananrail;

    // Row Lists
    private List<List<GameObject>> rowsCenterPitYousoroad;
    private List<List<GameObject>> rowsCenterPitKananrail;

    private List<List<GameObject>> rowsLeftPitYousoroad;
    private List<List<GameObject>> rowsLeftPitKananrail;

    private List<List<GameObject>> rowsRightPitYousoroad;
    private List<List<GameObject>> rowsRightPitKananrail;

    private List<List<GameObject>> rowsLeftLodgeYousoroad;
    private List<List<GameObject>> rowsLeftLodgeKananrail;

    private List<List<GameObject>> rowsRightLodgeYousoroad;
    private List<List<GameObject>> rowsRightLodgeKananrail;

    private List<List<GameObject>> rowsLeftLeftOrchestraNormal;
    private List<List<GameObject>> rowsLeftOrchestraKananrail;
    private List<List<GameObject>> rowsCenterOrchestraNormal;
    private List<List<GameObject>> rowsRightOrchestraKananrail;
    private List<List<GameObject>> rowsRightRightOrchestraNormal;
    private List<List<GameObject>> rowsLeftLeftLodgeKananrail;
    private List<List<GameObject>> rowsCenterLodgeNormal;
    private List<List<GameObject>> rowsRightRightLodgeKananrail;

    //Rotation lists
    private List<List<MyTransform>> rotSecList;
    private List<MyTransform> rotRandomListAll;

    private Action action;

    private bool colorOff = false;
    private int currColor = 0;
    private float lastRotationVal;
    private int tempTimeCounter = 0;
    private float tempTimeMark = 0.0f;
    private int numRow = 0;
    private float timeComparisonKananrail = 14.3f; // TODO
    private float timeComparisonTogether = 24.5f; // TODO
    int togetherCounter = 0;

    void Start () {
        // Load Penlight Colors
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

        secList = new List<List<GameObject>>();

        // Generate Non-Split Lists
        leftLeftOrchestraNormal = getListFromParent(leftLeftOrchestraParent);
        leftOrchestraKananrail = getListFromParent(leftOrchestraParent);
        centerOrchestraNormal = getListFromParent(centerOrchestraParent);
        rightOrchestraKananrail = getListFromParent(rightOrchestraParent);
        rightRightOrchestraNormal = getListFromParent(rightRightOrchestraParent);
        leftLeftLodgeKananrail = getListFromParent(leftLeftLodgeParent);
        centerLodgeNormal = getListFromParent(centerLodgeParent);
        rightRightLodgeKananrail = getListFromParent(rightRightLodgeParent);

        // Generate Yousoroad/Kananrail splits
        centerPitYousoroad = getListFromTagParent("CenterPitRoad", centerPitParent);
        centerPitKananrail = getListFromTagParent("CenterPitRail", centerPitParent);

        leftPitKananrail = getListFromTagParent("LeftPitRail", leftPitParent);
        leftPitNormalSplit = getListFromTagParent("LeftPitNormal", leftPitParent);

        rightPitKananrail = getListFromTagParent("RightPitRail", rightPitParent);
        rightPitNormalSplit = getListFromTagParent("RightPitNormal", rightPitParent);

        leftLodgeYousoroad = getListFromTagParent("LeftLodgeRoad", leftLodgeParent);
        leftLodgeKananrail = getListFromTagParent("LeftLodgeRail", leftLodgeParent);

        rightLodgeYousoroad = getListFromTagParent("RightLodgeRoad", rightLodgeParent);
        rightLodgeKananrail = getListFromTagParent("RightLodgeRail", rightLodgeParent);

        // Set master list
        secList.Add(centerPitYousoroad);
        secList.Add(centerPitKananrail);
        secList.Add(leftPitKananrail);
        secList.Add(leftPitNormalSplit);
        secList.Add(rightPitKananrail);
        secList.Add(rightPitNormalSplit);
        secList.Add(leftLodgeYousoroad);
        secList.Add(leftLodgeKananrail);
        secList.Add(rightLodgeYousoroad);
        secList.Add(rightLodgeKananrail);
        secList.Add(leftLeftOrchestraNormal);
        secList.Add(leftOrchestraKananrail);
        secList.Add(centerOrchestraNormal);
        secList.Add(rightOrchestraKananrail);
        secList.Add(rightRightOrchestraNormal);
        secList.Add(leftLeftLodgeKananrail);
        secList.Add(centerLodgeNormal);
        secList.Add(rightRightLodgeKananrail);

        createRotSecList();

        orientPenlights();
        sortPenlights();
        randomizePenlightColors();
        createRows();
        createRandomListNormal();
        createRandomListAll();





        //test();


        /* Test 
        for (int i = 0; i < leftPitNormal.Count; i++)
        {
            GameObject child = leftPitNormal[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[0];
        }
        */

    }

    void test()
    {
        /* Master List Color Test */
        /*
        for (int i = 0; i < (int) SEC.MAX; i++)
        {
            testSetPenlight(secList[i], 1);
        }
        */

        /* Yousoroad Test */
        /*
        testSetPenlight(centerPitYousoroad,4);
        testSetPenlight(centerPitKananrail,0);

        testSetPenlight(leftLodgeYousoroad,4);
        testSetPenlight(leftLodgeKananrail,0);

        testSetPenlight(rightLodgeYousoroad,4);
        testSetPenlight(rightLodgeKananrail,0);

        testSetPenlight(leftPitNormal,0);
        testSetPenlight(rightPitNormal,0);
        testSetPenlight(leftLeftOrchestraNormal,0);
        testSetPenlight(leftOrchestraNormal,0);
        testSetPenlight(centerOrchestraYousoroad,4);
        testSetPenlight(rightOrchestraNormal,0);
        testSetPenlight(rightRightOrchestraNormal,0);
        testSetPenlight(leftLeftLodgeNormal,0);
        testSetPenlight(centerLodgeYousoroad,4);
        testSetPenlight(rightRightLodgeNormal,0);
        */

        /* Yousoroad/Kananrail Test */
        /*
        testSetPenlight(centerPitYousoroad, 4);
        testSetPenlight(centerPitKananrail, 2);

        testSetPenlight(leftLodgeYousoroad, 4);
        testSetPenlight(leftLodgeKananrail, 2);

        testSetPenlight(rightLodgeYousoroad, 4);
        testSetPenlight(rightLodgeKananrail, 2);

        //testSetPenlight(leftPitNormal, 0);
        //testSetPenlight(rightPitNormal, 0);
        testSetPenlight(leftLeftOrchestraNormal, 0);
        testSetPenlight(leftOrchestraNormal, 2);
        testSetPenlight(centerOrchestraYousoroad, 4);
        testSetPenlight(rightOrchestraNormal, 2);
        testSetPenlight(rightRightOrchestraNormal, 0);
        testSetPenlight(leftLeftLodgeNormal, 2);
        testSetPenlight(centerLodgeYousoroad, 4);
        testSetPenlight(rightRightLodgeNormal, 2);

        testSetPenlight(leftPitNormalSplit, 0);
        testSetPenlight(leftPitKananrail, 2);

        testSetPenlight(rightPitNormalSplit, 0);
        testSetPenlight(rightPitKananrail, 2);
        */
    }

    void createRotSecList()
    {
        rotSecList = new List<List<MyTransform>>();

        for (int i = 0; i < secList.Count; i++)
        {
            rotSecList.Add(new List<MyTransform>());

            for (int j = 0; j < secList[i].Count; j++)
            {
                MyTransform tmpTransform = new MyTransform();
                tmpTransform.pos = new Vector3(0, 0, 0);
                tmpTransform.rot = new Vector3(0, 0, 0);
                rotSecList[i].Add(tmpTransform);
            }
        }
    }

    void createRandomListNormal()
    {
        randomListNormal = new List<GameObject>();
        Random rnd = new Random();

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            if (i != (int) SEC.CENTER_PIT_KAN || i != (int)SEC.LEFT_PIT_KAN || i != (int)SEC.RIGHT_PIT_KAN ||
                i != (int)SEC.LEFT_LODGE_KAN || i != (int)SEC.RIGHT_LODGE_KAN || i != (int)SEC.LEFT_ORCH_KAN
                 || i != (int)SEC.RIGHT_ORCH_KAN || i != (int)SEC.LEFT_LEFT_LODGE_KAN || i != (int)SEC.RIGHT_RIGHT_LODGE_KAN) {
                for (int j = 0; j < secList[i].Count; j++)
                {
                    randomListNormal.Add(secList[i][j].transform.gameObject);
                }
            }
        }

        int c = randomListNormal.Count;
        while (c > 1)
        {
            c--;
            int k = Random.Range(0, c + 1);
            GameObject g = randomListNormal[k];
            randomListNormal[k] = randomListNormal[c];
            randomListNormal[c] = g;
        }
    }

    void createRandomListAll()
    {
        randomListAll = new List<GameObject>();
        rotRandomListAll = new List<MyTransform>();
        Random rnd = new Random();

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                randomListAll.Add(secList[i][j].transform.gameObject);
                rotRandomListAll.Add(rotSecList[i][j]);
            }
        }

        int c = randomListAll.Count;
        while (c > 1)
        {
            c--;
            int k = Random.Range(0, c + 1);
            GameObject g = randomListAll[k];
            randomListAll[k] = randomListAll[c];
            randomListAll[c] = g;

            MyTransform v = rotRandomListAll[k];
            rotRandomListAll[k] = rotRandomListAll[c];
            rotRandomListAll[c] = v;
        }
    }

    void orientPenlights()
    {
        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                secList[i][j].transform.LookAt(lookAtObject.transform, secList[i][j].transform.up);
            }
        }
    }

    void sortPenlights()
    {   
        sortSection(leftLeftOrchestraNormal, new Vector3(72,0,100), new Vector3(150.29f, 0, -65.16f));
        sortSection(leftOrchestraKananrail, new Vector3(41, 0, 80), new Vector3(89, 0, -90));
        sortSection(centerOrchestraNormal, new Vector3(7, 0, 80), new Vector3(6, 0, -97));
        sortSection(rightOrchestraKananrail, new Vector3(-26, 0, 80), new Vector3(-74, 0, -87));
        sortSection(rightRightOrchestraNormal, new Vector3(-48, 0, 82.42f), new Vector3(-120f, 0, -72.43f));

        sortSection(rightRightLodgeKananrail, new Vector3(-65.05f, 0, -96.72f), new Vector3(-82.521f, 0, -181.32f));
        sortSection(rightLodgeKananrail, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        sortSection(rightLodgeYousoroad, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        sortSection(centerLodgeNormal, new Vector3(7.85f, 0, -108.48f), new Vector3(6.717f, 0, -188.15f));
        sortSection(leftLodgeKananrail, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        sortSection(leftLodgeYousoroad, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        sortSection(leftLeftLodgeKananrail, new Vector3(79.86f, 0, -101.29f), new Vector3(98.7f, 0, -182.22f));
    }

    void randomizePenlightColors()
    {
        int randomColor = 0;
        int randomSelect = 0;

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                if (j % 2 == 0)
                {
                    randomColor = Random.Range(0, 9);
                    randomSelect = Random.Range(0, 6);

                    if ((randomColor != (int) COLOR.LIGHT_GREEN) && (randomSelect != 0))
                    {
                        randomColor = (int) COLOR.LIGHT_GREEN;
                    }
                }

                GameObject child = secList[i][j].transform.GetChild(0).gameObject;
                MeshRenderer mesh = child.GetComponent<MeshRenderer>();
                mesh.material = pColor[randomColor];
            }
        }
    }

    void createRows()
    {
        rowsLeftLeftOrchestraNormal = new List<List<GameObject>>();
        rowsLeftLeftOrchestraNormal.Add(new List<GameObject>());
        rowsLeftOrchestraKananrail = new List<List<GameObject>>();
        rowsLeftOrchestraKananrail.Add(new List<GameObject>());
        rowsCenterOrchestraNormal = new List<List<GameObject>>();
        rowsCenterOrchestraNormal.Add(new List<GameObject>());
        rowsRightOrchestraKananrail = new List<List<GameObject>>();
        rowsRightOrchestraKananrail.Add(new List<GameObject>());
        rowsRightRightOrchestraNormal = new List<List<GameObject>>();
        rowsRightRightOrchestraNormal.Add(new List<GameObject>());
        rowsRightRightLodgeKananrail = new List<List<GameObject>>();
        rowsRightRightLodgeKananrail.Add(new List<GameObject>());
        rowsRightLodgeKananrail = new List<List<GameObject>>();
        rowsRightLodgeKananrail.Add(new List<GameObject>());
        rowsRightLodgeYousoroad = new List<List<GameObject>>();
        rowsRightLodgeYousoroad.Add(new List<GameObject>());
        rowsCenterLodgeNormal = new List<List<GameObject>>();
        rowsCenterLodgeNormal.Add(new List<GameObject>());
        rowsLeftLodgeKananrail = new List<List<GameObject>>();
        rowsLeftLodgeKananrail.Add(new List<GameObject>());
        rowsLeftLodgeYousoroad = new List<List<GameObject>>();
        rowsLeftLodgeYousoroad.Add(new List<GameObject>());
        rowsLeftLeftLodgeKananrail = new List<List<GameObject>>();
        rowsLeftLeftLodgeKananrail.Add(new List<GameObject>());

        
        arrangeSectionToRows(ref rowsLeftLeftOrchestraNormal, leftLeftOrchestraNormal, new Vector3(72, 0, 100), new Vector3(150.29f, 0, -65.16f));
        arrangeSectionToRows(ref rowsLeftOrchestraKananrail, leftOrchestraKananrail, new Vector3(41, 0, 80), new Vector3(89, 0, -90));
        arrangeSectionToRows(ref rowsCenterOrchestraNormal, centerOrchestraNormal, new Vector3(7, 0, 80), new Vector3(6, 0, -97));
        arrangeSectionToRows(ref rowsRightOrchestraKananrail, rightOrchestraKananrail, new Vector3(-26, 0, 80), new Vector3(-74, 0, -87));
        arrangeSectionToRows(ref rowsRightRightOrchestraNormal, rightRightOrchestraNormal, new Vector3(-48, 0, 82.42f), new Vector3(-120f, 0, -72.43f));
        arrangeSectionToRows(ref rowsRightRightLodgeKananrail, rightRightLodgeKananrail, new Vector3(-65.05f, 0, -96.72f), new Vector3(-82.521f, 0, -181.32f));
        arrangeSectionToRows(ref rowsRightLodgeKananrail, rightLodgeKananrail, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        arrangeSectionToRows(ref rowsRightLodgeYousoroad, rightLodgeYousoroad, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        arrangeSectionToRows(ref rowsCenterLodgeNormal, centerLodgeNormal, new Vector3(7.85f, 0, -108.48f), new Vector3(6.717f, 0, -188.15f));
        arrangeSectionToRows(ref rowsLeftLodgeKananrail, leftLodgeKananrail, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        arrangeSectionToRows(ref rowsLeftLodgeYousoroad, leftLodgeYousoroad, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        arrangeSectionToRows(ref rowsLeftLeftLodgeKananrail, leftLeftLodgeKananrail, new Vector3(79.86f, 0, -101.29f), new Vector3(98.7f, 0, -182.22f));
        //NOTE: PIT IS NOT ADDED SINCE ITS 1 ROW TOTAL
    }

    void arrangeSectionToRows(ref List<List<GameObject>> rowList, List<GameObject> normList, Vector3 baseA, Vector3 baseB)
    {
        Vector3 onNormal = baseB - baseA;

        int currRow = 0;
        float threshold = 2.7f;
        float comparisonObject = 0.0f;
        float currProjection = 0;

        if (normList.Count > 0)
            comparisonObject = getProjection(onNormal, new Vector3(normList[0].transform.position.x, 0, normList[0].transform.position.z) - baseA);

        for (int i = 0; i < normList.Count; i++)
        {
            currProjection = getProjection(onNormal, new Vector3(normList[i].transform.position.x, 0, normList[i].transform.position.z) - baseA);

            if (Mathf.Abs(currProjection - comparisonObject) < threshold)
            {
                rowList[currRow].Add(normList[i]);
            }
            else
            {
                rowList.Add(new List<GameObject>());
                rowList[currRow + 1].Add(normList[i]);
                comparisonObject = currProjection;
                currRow++;
            }
        }
    }

    void sortSection(List<GameObject> currList, Vector3 baseA, Vector3 baseB)
    {
        Vector3 onNormal = baseB - baseA;

        currList.Sort(delegate (GameObject a, GameObject b)
        {
            return getProjection(onNormal, new Vector3(a.transform.position.x, 0, a.transform.position.z) - baseA).CompareTo
            (getProjection(onNormal, new Vector3(b.transform.position.x, 0, b.transform.position.z) - baseA));
        });
    }

    void sortSection(List<GameObject> currList)
    {
        currList.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector3.Distance(new Vector3(lookAtObject.transform.position.x, 0, lookAtObject.transform.position.z),
                new Vector3(a.transform.position.x, 0, a.transform.position.z)).CompareTo
            ((Vector3.Distance(new Vector3(lookAtObject.transform.position.x, 0, lookAtObject.transform.position.z),
            new Vector3(b.transform.position.x, 0, b.transform.position.z))));
        });
    }

    float getProjection(Vector3 vecA, Vector3 vecB)
    {
        return Vector3.Project(vecB,vecA).magnitude;
    }

    void testSetPenlight(List<GameObject> list, int light)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject child = list[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[light];
        }
    }

    // Update is called once per frame
    void Update () {
        if (!videoPlayer.isPlaying)
            return;

        actionManager();

        switch (action)
        {
            case Action.Default:
                actionDefault();
                break;
            case Action.Down:
                actionDown();
                break;
            case Action.Kananrail:
                actionKananrail();
                break;
            case Action.Together:
                actionTogether();
                break;
            default:
                break;
        }
    }

    void actionManager()
    {
        if (videoPlayer.time > 9.07f) // TODO: BOIII
        {
            action = Action.Down;
        }

        //16.80 end time
        //6 seconds of raising lights
        //85 rows total - 6pit - 45orch - 21 - (3) - (11)
        //0.07s per row
        if (videoPlayer.time > 14.20f) // 1.5
        {
            action = Action.Kananrail;
        }
        
        if (videoPlayer.time > 24.5f) // 22.3f
        {
            action = Action.Together;
        }
    }

    void actionTogether()
    {
        float dist = 0.2f;

        if (videoPlayer.time > (timeComparisonTogether + dist))
        {
            timeComparisonTogether += dist;
            togetherCounter++;
        }

        if (togetherCounter < 4)
        {
            enableGreenLightRandom(togetherCounter);
            actionDefault(togetherCounter);
        }
        else
            actionDefault();
    }

    void enableGreenLightRandom(int currCounter)
    {
        int portion = randomListNormal.Count / 2;
        int start = currCounter * portion;
        int end = start + portion;

        if (currCounter > 1)
        {
            start = 0;
            end = randomListNormal.Count;
        }

        for (int i = start; i < end; i++)
        {
            GameObject child = randomListNormal[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[(int) COLOR.LIGHT_GREEN];
        }
    }

    void enableGreenLight(List<GameObject> currRow)
    {
        for (int i = 0; i < currRow.Count; i++)
        {
            GameObject child = currRow[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[(int) COLOR.LIGHT_GREEN];
        }
    }

    
    void upRow(List<GameObject> currRow, int section)
    {
        int rotRate = -1000;
        enableGreenLight(currRow);

        for (int i = 0; i < currRow.Count; i++)
        {
            if (currRow[i].transform.localEulerAngles.x > 0)
            {
                if (currRow[i].transform.localEulerAngles.x + rotRate * Time.deltaTime > 0)
                    currRow[i].transform.Rotate(rotRate * Time.deltaTime, 0, 0);
                else
                {
                    currRow[i].transform.Rotate(-currRow[i].transform.localEulerAngles.x, 0, 0);
                }
            }
        }
    }
    

    /*
    void upRow(List<GameObject> currRow, int section)
    {
        int rotRate = -1000;
        enableGreenLight(currRow);

        for (int i = 0; i < currRow.Count; i++)
        {
            if (rotSecList[section][i].rot.x > 0)
            {
                if (rotSecList[section][i].rot.x + (rotRate * Time.deltaTime) > 0)
                {
                    Quaternion localRotation = Quaternion.Euler(new Vector3(rotRate * Time.deltaTime, 0, 0));
                    currRow[i].transform.rotation = currRow[i].transform.rotation * localRotation;
                    rotSecList[section][i].rot = new Vector3(rotSecList[section][i].rot.x + (rotRate * Time.deltaTime), 0, 0);
                }

                else
                {
                    Quaternion localRotation = Quaternion.Euler(new Vector3(-rotSecList[section][i].rot.x, 0, 0));
                    currRow[i].transform.rotation = currRow[i].transform.rotation * localRotation;
                    rotSecList[section][i].rot = new Vector3(0, 0, 0);
                }
            }
        }

    }
    */


    void shakeRows(List<List<GameObject>> section, int rows)
    {
        int rowMod = rows;
        if (rows > section.Count)
        {
            rowMod = section.Count;
        }

        for (int i = 0; i < rowMod; i++)
        {
            for (int j = 0; j < section[i].Count; j++)
            {
                section[i][j].transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin((float)videoPlayer.time * 25) * 20);
            }
        }
    }

    void shakeRows(List<GameObject> section)
    {
        for (int i = 0; i < section.Count; i++)
        {
            section[i].transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin((float)videoPlayer.time * 25) * 20);
        }
    }

    void shakeRowManager(int i)
    {
        // Shake effect
        if (i >= 1)
        {
            shakeRows(centerPitKananrail);
            shakeRows(leftPitKananrail);
            shakeRows(rightPitKananrail);
        }

        if (i >= 2)
        {
            shakeRows(rowsLeftOrchestraKananrail, i - 1);
            shakeRows(rowsRightOrchestraKananrail, i - 1);
        }

        if (i >= rowsCenterOrchestraNormal.Count + 2) // TODO find biggest orchestra
        {
            shakeRows(rowsLeftLeftLodgeKananrail, i - rowsCenterOrchestraNormal.Count - 1);
            shakeRows(rowsLeftLodgeKananrail, i - rowsCenterOrchestraNormal.Count - 1);
            shakeRows(rowsRightLodgeKananrail, i - rowsCenterOrchestraNormal.Count - 1);
            shakeRows(rowsRightRightLodgeKananrail, i - rowsCenterOrchestraNormal.Count - 1);
        }
    }

    void upRowManager(int i)
    {
        int maxRows = rowsCenterOrchestraNormal.Count + rowsRightLodgeYousoroad.Count + 1;

        // Pit
        if (i == 0)
        {
            upRow(centerPitKananrail, (int) SEC.CENTER_PIT_KAN);
            upRow(leftPitKananrail, (int)SEC.LEFT_PIT_KAN);
            upRow(rightPitKananrail, (int)SEC.RIGHT_PIT_KAN);
            //centerPitYousoroad leftLodgeYousoroad rightLodgeYousoroad
        }

        // Orchestra
        else if ((i < rowsLeftOrchestraKananrail.Count + 1) || (i < rowsRightOrchestraKananrail.Count + 1))
        {
            if (i < rowsLeftOrchestraKananrail.Count + 1)
                upRow(rowsLeftOrchestraKananrail[i - 1], (int)SEC.LEFT_ORCH_KAN);

            if (i < rowsRightOrchestraKananrail.Count + 1)
                upRow(rowsRightOrchestraKananrail[i - 1], (int)SEC.RIGHT_ORCH_KAN);
        }

        // Lodge
        else
        {
            // Left Left Lodge
            if (i - rowsLeftOrchestraKananrail.Count - 1 < rowsLeftLeftLodgeKananrail.Count)
            {
                upRow(rowsLeftLeftLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.LEFT_LEFT_LODGE_KAN);
            }
            else if (i - rowsRightOrchestraKananrail.Count - 1 < rowsLeftLeftLodgeKananrail.Count)
            {
                upRow(rowsLeftLeftLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.LEFT_LEFT_LODGE_KAN);
            }

            // Left Lodge
            if (i - rowsLeftOrchestraKananrail.Count - 1 < rowsLeftLodgeKananrail.Count)
            {
                upRow(rowsLeftLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.LEFT_LODGE_KAN);
            }
            else if (i - rowsRightOrchestraKananrail.Count - 1 < rowsLeftLodgeKananrail.Count)
            {
                upRow(rowsLeftLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.LEFT_LODGE_KAN);
            }

            // Right Lodge
            if (i - rowsLeftOrchestraKananrail.Count - 1 < rowsRightLodgeKananrail.Count)
            {
                upRow(rowsRightLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.RIGHT_LODGE_KAN);
            }
            else if (i - rowsRightOrchestraKananrail.Count - 1 < rowsRightLodgeKananrail.Count)
            {
                upRow(rowsRightLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.RIGHT_LODGE_KAN);
            }

            // Right Right Lodge
            if (i - rowsLeftOrchestraKananrail.Count - 1 < rowsRightRightLodgeKananrail.Count)
            {
                upRow(rowsRightRightLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.RIGHT_RIGHT_LODGE_KAN);
            }
            else if (i - rowsRightOrchestraKananrail.Count - 1 < rowsRightRightLodgeKananrail.Count)
            {
                upRow(rowsRightRightLodgeKananrail[i - rowsLeftOrchestraKananrail.Count - 1], (int)SEC.RIGHT_RIGHT_LODGE_KAN);
            }
        }
    }

    void actionKananrail()
    {
        // TODO determine max rows
        int maxRows = rowsCenterOrchestraNormal.Count + rowsRightLodgeYousoroad.Count + 1;

        if (numRow < maxRows)
            upRowManager(numRow);

        shakeRowManager(numRow);

        if (videoPlayer.time > timeComparisonKananrail) // TODO
        {
            timeComparisonKananrail += 0.1f;
            numRow++;
        }
    }

    
    void actionDown()
    {
        int rotRate = 3;
        disablePenlight();

        if (lastRotationVal + rotRate < 90) {
            for (int i = 0; i < secList.Count; i++)
            {
                for (int j = 0; j < secList[i].Count; j++)
                {
                    Quaternion localRotation = Quaternion.Euler(new Vector3(rotRate, 0, 0));
                    secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
                }
            }

            lastRotationVal += rotRate;
        }
    }
    
    

    /*
    void actionDown()
    {
        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;


        int rotRate = 3;
        disablePenlight();

        int start = 0;
        int end = 0;

        if (videoPlayer.time > 12.37f)
        {
            start = (randomListAll.Count * 5) / 16;
            end = randomListAll.Count;
        }
        else if (videoPlayer.time > 12.36f)
        {
            start = (randomListAll.Count * 14) / 16;
            end = (randomListAll.Count * 15) / 16;
        }
        else if (videoPlayer.time > 12.125f)
        {
            start = (randomListAll.Count * 13) / 16;
            end = (randomListAll.Count * 14) / 16;
        }
        else if (videoPlayer.time > 11.89f)
        {
            start = (randomListAll.Count * 12) / 16;
            end = (randomListAll.Count * 13) / 16;
        }
        else if (videoPlayer.time > 11.655f)
        {
            start = (randomListAll.Count * 11) / 16;
            end = (randomListAll.Count * 12) / 16;
        }
        else if (videoPlayer.time > 11.42f)
        {
            start = (randomListAll.Count * 10) / 16;
            end = (randomListAll.Count * 11) / 16;
        }
        else if (videoPlayer.time > 11.185f)
        {
            start = (randomListAll.Count * 9) / 16;
            end = (randomListAll.Count * 10) / 16;
        }
        else if (videoPlayer.time > 10.95f)
        {
            start = (randomListAll.Count * 8) / 16;
            end = (randomListAll.Count * 9) / 16;
        }
        else if (videoPlayer.time > 10.715f)
        {
            start = (randomListAll.Count * 7) / 16;
            end = (randomListAll.Count * 8) / 16;
        }
        else if (videoPlayer.time > 10.48f)
        {
            start = (randomListAll.Count * 6) / 16;
            end = (randomListAll.Count * 7) / 16;
        }
        else if (videoPlayer.time > 10.245f)
        {
            start = (randomListAll.Count * 5) / 16;
            end = (randomListAll.Count * 6) / 16;
        }
        else if (videoPlayer.time > 10.01f)
        {
            start = (randomListAll.Count * 4) / 16;
            end = (randomListAll.Count * 5) / 16;
        }
        else if (videoPlayer.time > 9.775f)
        {
            start = (randomListAll.Count * 3) / 16;
            end = (randomListAll.Count * 4) / 16;
        }
        else if (videoPlayer.time > 9.54f)
        {
            start = (randomListAll.Count * 2) / 16;
            end = (randomListAll.Count * 3) / 16;
        }
        else if (videoPlayer.time > 9.305f)
        {
            start = randomListAll.Count / 16;
            end = (randomListAll.Count * 2) / 16;
        }
        else // 9.07
        {
            start = 0;
            end = randomListAll.Count / 16;
        }

        for (int i = start; i < end; i++)
        {
            if (rotRandomListAll[i].rot.x + rotRate < 90)
            {
                Quaternion localRotation = Quaternion.Euler(new Vector3(rotRate, 0, 0));
                randomListAll[i].transform.rotation = randomListAll[i].transform.rotation * localRotation;
                rotRandomListAll[i].rot = new Vector3(rotRandomListAll[i].rot.x + rotRate,0,0);
            }
        }

        COMMENT
        for (int i = end; i < randomListAll.Count; i++)
        {
            Quaternion localRotation = Quaternion.Euler(new Vector3(val - rotRandomListAll[i].rot.x, 0, 0));
            randomListAll[i].transform.rotation = randomListAll[i].transform.rotation * localRotation;
            rotRandomListAll[i].rot = new Vector3(val, 0, 0);
        }
     
    }
*/

     /*
    void actionDefault()
    {
        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;

        for (int i = 0; i < secList.Count; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                Quaternion localRotation = Quaternion.Euler(new Vector3(val - rotSecList[i][j].rot.x, 0, 0));
                secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
                rotSecList[i][j].rot = new Vector3(val,0,0);
            }
        }

        lastRotationVal = val;
    }
    */

    
    void actionDefault()
    {
        float val = Mathf.Sin((float)videoPlayer.time * 17.5f);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));

        for (int i = 0; i < secList.Count; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
            }
        }

        lastRotationVal = val;
    }
    

    
    void actionDefault(int currCounter)
    {
        int portion = randomListNormal.Count / 2;
        int start = currCounter * portion;
        int end = start + portion;

        float val = Mathf.Sin((float)videoPlayer.time * 17.5f);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));


        if (currCounter > 1)
        {
            start = 0;
            end = randomListNormal.Count;
        }

        for (int i = start; i < end; i++)
        {
            // THIS ISNT RIGHT
            //randomList[i].transform.rotation = Quaternion.Euler(0, randomList[i].transform.eulerAngles.y, 0);
            randomListNormal[i].transform.rotation = Quaternion.Euler(Mathf.Sin((float)videoPlayer.time * 17.5f) * 45, 0, 0);
            //randomList[i].transform.rotation = randomList[i].transform.rotation * localRotation;

        }

        lastRotationVal = val;

    }
    

        /*
    void actionDefault(int currCounter)
    {
        int portion = randomListAll.Count / 3;
        int start = currCounter * portion;
        int end = start + portion;

        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;


        if (currCounter > 2)
        {
            start = 0;
            end = randomListNormal.Count;
        }

        for (int i = start; i < end; i++)
        {
            Quaternion localRotation = Quaternion.Euler(new Vector3(val - rotRandomListAll[i].rot.x, 0, 0));
            randomListAll[i].transform.rotation = randomListAll[i].transform.rotation * localRotation;
            rotRandomListAll[i].rot = new Vector3(val, 0, 0);
        }
    }
    */

    void disablePenlight()
    {
        //8 Sect 0.235
        // Start 9.07  - End 12.86
        int start = 0;
        int end = 0;

        if (videoPlayer.time > 12.37f)
        {
            start = (randomListAll.Count * 5) / 16;
            end = randomListAll.Count;
        }
        else if (videoPlayer.time > 12.36f)
        {
            start = (randomListAll.Count * 14) / 16;
            end = (randomListAll.Count * 15) / 16;
        }
        else if (videoPlayer.time > 12.125f)
        {
            start = (randomListAll.Count * 13) / 16;
            end = (randomListAll.Count * 14) / 16;
        }
        else if (videoPlayer.time > 11.89f)
        {
            start = (randomListAll.Count * 12) / 16;
            end = (randomListAll.Count * 13) / 16;
        }
        else if (videoPlayer.time > 11.655f)
        {
            start = (randomListAll.Count * 11) / 16;
            end = (randomListAll.Count * 12) / 16;
        }
        else if (videoPlayer.time > 11.42f)
        {
            start = (randomListAll.Count * 10) / 16;
            end = (randomListAll.Count * 11) / 16;
        }
        else if (videoPlayer.time > 11.185f)
        {
            start = (randomListAll.Count * 9) / 16;
            end = (randomListAll.Count * 10) / 16;
        }
        else if (videoPlayer.time > 10.95f)
        {
            start = (randomListAll.Count * 8) / 16;
            end = (randomListAll.Count * 9) / 16;
        }
        else if (videoPlayer.time > 10.715f)
        {
            start = (randomListAll.Count * 7) / 16;
            end = (randomListAll.Count * 8) / 16;
        }
        else if (videoPlayer.time > 10.48f)
        {
            start = (randomListAll.Count * 6) / 16;
            end = (randomListAll.Count * 7) / 16;
        }
        else if (videoPlayer.time > 10.245f)
        {
            start = (randomListAll.Count * 5) / 16;
            end = (randomListAll.Count * 6) / 16;
        }
        else if (videoPlayer.time > 10.01f)
        {
            start = (randomListAll.Count * 4) / 16;
            end = (randomListAll.Count * 5) / 16;
        }
        else if (videoPlayer.time > 9.775f)
        {
            start = (randomListAll.Count * 3) / 16;
            end = (randomListAll.Count * 4) / 16;
        }
        else if (videoPlayer.time > 9.54f)
        {
            start = (randomListAll.Count * 2) / 16;
            end = (randomListAll.Count * 3) / 16;
        }
        else if (videoPlayer.time > 9.305f)
        {
            start = randomListAll.Count / 16;
            end = (randomListAll.Count * 2) / 16;
        }
        else // 9.07
        {
            start = 0;
            end = randomListAll.Count / 16;
        }

        for (int i = start; i < end; i++)
        {
            GameObject child = randomListAll[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[9];
        }
    }

    /*
    void disablePenlight()
    {
        //8 Sect 0.47375
        // Start 9.07  - End 12.86
        int start = 0;
        int end = 0;

        for (int i = 0; i < secList.Count; i++)
        {
            if (videoPlayer.time > 10.57f)
            {
                start = (secList[i].Count * 3) / 4;
                end = secList[i].Count;
            }
            else if (videoPlayer.time > 10.07f)
            {
                start = secList[i].Count / 2;
                end = (secList[i].Count*3) / 4;
            }
            else if (videoPlayer.time > 9.57f)
            {
                start = secList[i].Count / 4;
                end = secList[i].Count / 2;
            }
            else
            {
                start = 0;
                end = secList[i].Count/4;
            }

            for (int j = start; j < end; j++)
            {
                GameObject child = secList[i][j].transform.GetChild(0).gameObject;
                MeshRenderer mesh = child.GetComponent<MeshRenderer>();
                mesh.material = pColor[9];
            }
        }
    }*/

    void changeAllColor()
    {
        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            testSetPenlight(secList[i], currColor);
        }
    }

    List<GameObject> getListFromParent(GameObject parent)
    {
        List<GameObject> retList = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            retList.Add(parent.transform.GetChild(i).gameObject);
        }

        return retList;
    }

    List<GameObject> getListFromTagParent(string str, GameObject parent)
    {
        List<GameObject> retList = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.tag == str)
            {
                retList.Add(child.gameObject);
            }
        }

        return retList;
    }
}
