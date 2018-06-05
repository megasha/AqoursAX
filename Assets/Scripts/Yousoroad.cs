using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Yousoroad : MonoBehaviour {
    enum Action { Default, Down, Yousoroad, Together };

    enum SEC { CENTER_PIT_YOU,     // Yousoroad splits this line and below
               CENTER_PIT_KAN, 
               LEFT_LODGE_YOU,
               LEFT_LODGE_KAN,
               RIGHT_LODGE_YOU,
               RIGHT_LODGE_KAN,
               LEFT_PIT_NORM,      // Yousoroad/Normals this line and below
               RIGHT_PIT_NORM, 
               LEFT_LEFT_ORCH_NORM,
               LEFT_ORCH_NORM,
               CENTER_ORCH_YOU,
               RIGHT_ORCH_NORM,
               RIGHT_RIGHT_ORCH_NORM,
               LEFT_LEFT_LODGE_NORM,
               CENTER_LODGE_YOU,
               RIGHT_RIGHT_LODGE_NORM,
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
    private List<GameObject> randomList;
    private List<GameObject> randomListAll;

    /* Yousoroad Split Lists */
    private List<GameObject> centerPitYousoroad;
    private List<GameObject> centerPitKananrail;

    private List<GameObject> leftLodgeYousoroad;
    private List<GameObject> leftLodgeKananrail;

    private List<GameObject> rightLodgeYousoroad;
    private List<GameObject> rightLodgeKananrail;

    /* DELETE OR PUT IN KANANRAIL */
    /*
    private List<GameObject> leftPitKananrail;
    private List<GameObject> leftPitNormalSplit;

    private List<GameObject> rightPitKananrail;
    private List<GameObject> rightPitNormalSplit;
    */




    /* Yousoroad/Normal Lists */
    private List<GameObject> leftPitNormal;
    private List<GameObject> rightPitNormal;
    private List<GameObject> leftLeftOrchestraNormal;
    private List<GameObject> leftOrchestraNormal;
    private List<GameObject> centerOrchestraYousoroad;
    private List<GameObject> rightOrchestraNormal;
    private List<GameObject> rightRightOrchestraNormal;
    private List<GameObject> leftLeftLodgeNormal;
    private List<GameObject> centerLodgeYousoroad;
    private List<GameObject> rightRightLodgeNormal;

    // Row Lists
    private List<List<GameObject>> rowsCenterPitYousoroad;
    private List<List<GameObject>> rowsCenterPitKananrail;

    private List<List<GameObject>> rowsLeftLodgeYousoroad;
    private List<List<GameObject>> rowsLeftLodgeKananrail;

    private List<List<GameObject>> rowsRightLodgeYousoroad;
    private List<List<GameObject>> rowsRightLodgeKananrail;

    private List<List<GameObject>> rowsLeftPitNormal;
    private List<List<GameObject>> rowsRightPitNormal;
    private List<List<GameObject>> rowsLeftLeftOrchestraNormal;
    private List<List<GameObject>> rowsLeftOrchestraNormal;
    private List<List<GameObject>> rowsCenterOrchestraYousoroad;
    private List<List<GameObject>> rowsRightOrchestraNormal;
    private List<List<GameObject>> rowsRightRightOrchestraNormal;
    private List<List<GameObject>> rowsLeftLeftLodgeNormal;
    private List<List<GameObject>> rowsCenterLodgeYousoroad;
    private List<List<GameObject>> rowsRightRightLodgeNormal;

    private Action action;

    private bool colorOff = false;
    private int currColor = 0;
    private float lastRotationVal;
    private int tempTimeCounter = 0;
    private float tempTimeMark = 0.0f;
    private int numRow = 0;
    private float timeComparisonYousoroad = 10.683f;
    private float timeComparisonTogether = 23.3f;
    int togetherCounter = 0;





    // Yousoroad: centerPit, centerOrchestra, centerLodge
    // Normal: leftLeftOrchestra, leftOrchestra, rightOrchestra, rightRightOrchestra
    //        leftLeftLodgew, rightRightLodge

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
        // Generate Non-Split Lists
        leftPitNormal = getListFromParent(leftPitParent);
        rightPitNormal = getListFromParent(rightPitParent);
        leftLeftOrchestraNormal = getListFromParent(leftLeftOrchestraParent);
        leftOrchestraNormal = getListFromParent(leftOrchestraParent);
        centerOrchestraYousoroad = getListFromParent(centerOrchestraParent);
        rightOrchestraNormal = getListFromParent(rightOrchestraParent);
        rightRightOrchestraNormal = getListFromParent(rightRightOrchestraParent);
        leftLeftLodgeNormal = getListFromParent(leftLeftLodgeParent);
        centerLodgeYousoroad = getListFromParent(centerLodgeParent);
        rightRightLodgeNormal = getListFromParent(rightRightLodgeParent);

        // Generate Yousoroad/Kananrail splits
        centerPitYousoroad = getListFromTagParent("CenterPitRoad", centerPitParent);
        centerPitKananrail = getListFromTagParent("CenterPitRail", centerPitParent);

        leftLodgeYousoroad = getListFromTagParent("LeftLodgeRoad", leftLodgeParent);
        leftLodgeKananrail = getListFromTagParent("LeftLodgeRail", leftLodgeParent);

        rightLodgeYousoroad = getListFromTagParent("RightLodgeRoad", rightLodgeParent);
        rightLodgeKananrail = getListFromTagParent("RightLodgeRail", rightLodgeParent);

        // Set master list
        secList.Add(centerPitYousoroad);
        secList.Add(centerPitKananrail);
        secList.Add(leftLodgeYousoroad);
        secList.Add(leftLodgeKananrail);
        secList.Add(rightLodgeYousoroad);
        secList.Add(rightLodgeKananrail);
        secList.Add(leftPitNormal);
        secList.Add(rightPitNormal);
        secList.Add(leftLeftOrchestraNormal);
        secList.Add(leftOrchestraNormal);
        secList.Add(centerOrchestraYousoroad);
        secList.Add(rightOrchestraNormal);
        secList.Add(rightRightOrchestraNormal);
        secList.Add(leftLeftLodgeNormal);
        secList.Add(centerLodgeYousoroad);
        secList.Add(rightRightLodgeNormal);


        /* DELETE OR PUT IN KANANRAIL */
        /*
        leftPitKananrail = getListFromTagParent("LeftPitRail", leftPitParent);
        leftPitNormalSplit = getListFromTagParent("LeftPitNormal", leftPitParent);

        rightPitKananrail = getListFromTagParent("RightPitRail", rightPitParent);
        rightPitNormalSplit = getListFromTagParent("RightPitNormal", rightPitParent);
        */

        orientPenlights();
        sortPenlights();
        randomizePenlightColors();
        createRows();
        createRandomList();
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

    void createRandomList()
    {
        randomList = new List<GameObject>();
        Random rnd = new Random();

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            if (i != (int) SEC.CENTER_PIT_YOU || i != (int)SEC.LEFT_LODGE_YOU || i != (int)SEC.RIGHT_LODGE_YOU ||
                i != (int)SEC.CENTER_ORCH_YOU || i != (int)SEC.CENTER_LODGE_YOU) {
                for (int j = 0; j < secList[i].Count; j++)
                {
                    randomList.Add(secList[i][j].transform.gameObject);
                }
            }
        }

        int c = randomList.Count;
        while (c > 1)
        {
            c--;
            int k = Random.Range(0, c + 1);
            GameObject g = randomList[k];
            randomList[k] = randomList[c];
            randomList[c] = g;
        }
    }

    void createRandomListAll()
    {
        randomListAll = new List<GameObject>();
        Random rnd = new Random();

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                randomListAll.Add(secList[i][j].transform.gameObject);
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
        }
    }

    void orientPenlights()
    {
        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                secList[i][j].transform.LookAt(lookAtObject.transform, secList[i][j].transform.up);
                //secList[i][j].transform.Rotate(new Vector3(90, 0, 0));
            }
        }
    }

    void sortPenlights()
    {
        // Sort leftLeftOrchestraNormal
        
        sortSection(leftLeftOrchestraNormal, new Vector3(72,0,100), new Vector3(150.29f, 0, -65.16f));
        sortSection(leftOrchestraNormal, new Vector3(41, 0, 80), new Vector3(89, 0, -90));
        sortSection(centerOrchestraYousoroad, new Vector3(7, 0, 80), new Vector3(6, 0, -97));
        sortSection(rightOrchestraNormal, new Vector3(-26, 0, 80), new Vector3(-74, 0, -87));
        sortSection(rightRightOrchestraNormal, new Vector3(-48, 0, 82.42f), new Vector3(-120f, 0, -72.43f));
        sortSection(rightRightLodgeNormal, new Vector3(-65.05f, 0, -96.72f), new Vector3(-82.521f, 0, -181.32f));
        sortSection(rightLodgeKananrail, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        sortSection(rightLodgeYousoroad, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        sortSection(centerLodgeYousoroad, new Vector3(7.85f, 0, -108.48f), new Vector3(6.717f, 0, -188.15f));
        sortSection(leftLodgeKananrail, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        sortSection(leftLodgeYousoroad, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        sortSection(leftLeftLodgeNormal, new Vector3(79.86f, 0, -101.29f), new Vector3(98.7f, 0, -182.22f));
        

        /*
        sortSection(leftLeftOrchestraNormal);
        sortSection(leftOrchestraNormal);
        sortSection(centerOrchestraYousoroad);
        sortSection(rightOrchestraNormal);
        sortSection(rightRightOrchestraNormal);
        sortSection(rightRightLodgeNormal);
        sortSection(rightLodgeKananrail);
        sortSection(rightLodgeYousoroad);
        sortSection(centerLodgeYousoroad);
        sortSection(leftLodgeKananrail);
        sortSection(leftLodgeYousoroad);
        sortSection(leftLeftLodgeNormal);
        sortSection(centerPitYousoroad);
        sortSection(centerPitKananrail);
        sortSection(leftPitNormal);
        sortSection(rightPitNormal);
        */
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

                    if ((randomColor != (int) COLOR.AQUA) && (randomSelect != 0))
                    {
                        randomColor = (int) COLOR.AQUA;
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
        rowsLeftOrchestraNormal = new List<List<GameObject>>();
        rowsLeftOrchestraNormal.Add(new List<GameObject>());
        rowsCenterOrchestraYousoroad = new List<List<GameObject>>();
        rowsCenterOrchestraYousoroad.Add(new List<GameObject>());
        rowsRightOrchestraNormal = new List<List<GameObject>>();
        rowsRightOrchestraNormal.Add(new List<GameObject>());
        rowsRightRightOrchestraNormal = new List<List<GameObject>>();
        rowsRightRightOrchestraNormal.Add(new List<GameObject>());
        rowsRightRightLodgeNormal = new List<List<GameObject>>();
        rowsRightRightLodgeNormal.Add(new List<GameObject>());
        rowsRightLodgeKananrail = new List<List<GameObject>>();
        rowsRightLodgeKananrail.Add(new List<GameObject>());
        rowsRightLodgeYousoroad = new List<List<GameObject>>();
        rowsRightLodgeYousoroad.Add(new List<GameObject>());
        rowsCenterLodgeYousoroad = new List<List<GameObject>>();
        rowsCenterLodgeYousoroad.Add(new List<GameObject>());
        rowsLeftLodgeKananrail = new List<List<GameObject>>();
        rowsLeftLodgeKananrail.Add(new List<GameObject>());
        rowsLeftLodgeYousoroad = new List<List<GameObject>>();
        rowsLeftLodgeYousoroad.Add(new List<GameObject>());
        rowsLeftLeftLodgeNormal = new List<List<GameObject>>();
        rowsLeftLeftLodgeNormal.Add(new List<GameObject>());

        
        arrangeSectionToRows(ref rowsLeftLeftOrchestraNormal, leftLeftOrchestraNormal, new Vector3(72, 0, 100), new Vector3(150.29f, 0, -65.16f));
        arrangeSectionToRows(ref rowsLeftOrchestraNormal, leftOrchestraNormal, new Vector3(41, 0, 80), new Vector3(89, 0, -90));
        arrangeSectionToRows(ref rowsCenterOrchestraYousoroad, centerOrchestraYousoroad, new Vector3(7, 0, 80), new Vector3(6, 0, -97));
        arrangeSectionToRows(ref rowsRightOrchestraNormal, rightOrchestraNormal, new Vector3(-26, 0, 80), new Vector3(-74, 0, -87));
        arrangeSectionToRows(ref rowsRightRightOrchestraNormal, rightRightOrchestraNormal, new Vector3(-48, 0, 82.42f), new Vector3(-120f, 0, -72.43f));
        arrangeSectionToRows(ref rowsRightRightLodgeNormal, rightRightLodgeNormal, new Vector3(-65.05f, 0, -96.72f), new Vector3(-82.521f, 0, -181.32f));
        arrangeSectionToRows(ref rowsRightLodgeKananrail, rightLodgeKananrail, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        arrangeSectionToRows(ref rowsRightLodgeYousoroad, rightLodgeYousoroad, new Vector3(-19.81f, 0, -110.01f), new Vector3(-25.68f, 0, -188.15f));
        arrangeSectionToRows(ref rowsCenterLodgeYousoroad, centerLodgeYousoroad, new Vector3(7.85f, 0, -108.48f), new Vector3(6.717f, 0, -188.15f));
        arrangeSectionToRows(ref rowsLeftLodgeKananrail, leftLodgeKananrail, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        arrangeSectionToRows(ref rowsLeftLodgeYousoroad, leftLodgeYousoroad, new Vector3(34.505f, 0, -110.26f), new Vector3(39.69f, 0, -189.36f));
        arrangeSectionToRows(ref rowsLeftLeftLodgeNormal, leftLeftLodgeNormal, new Vector3(79.86f, 0, -101.29f), new Vector3(98.7f, 0, -182.22f));
        //NOTE: PIT IS NOT ADDED SINCE ITS 1 ROW TOTAL

        /*
        arrangeSectionToRowsV2(ref rowsLeftLeftOrchestraNormal, leftLeftOrchestraNormal);
        arrangeSectionToRowsV2(ref rowsLeftOrchestraNormal, leftOrchestraNormal);
        arrangeSectionToRowsV2(ref rowsCenterOrchestraYousoroad, centerOrchestraYousoroad);
        arrangeSectionToRowsV2(ref rowsRightOrchestraNormal, rightOrchestraNormal);
        arrangeSectionToRowsV2(ref rowsRightRightOrchestraNormal, rightRightOrchestraNormal);
        arrangeSectionToRowsV2(ref rowsRightRightLodgeNormal, rightRightLodgeNormal);
        arrangeSectionToRowsV2(ref rowsRightLodgeKananrail, rightLodgeKananrail);
        arrangeSectionToRowsV2(ref rowsRightLodgeYousoroad, rightLodgeYousoroad);
        arrangeSectionToRowsV2(ref rowsCenterLodgeYousoroad, centerLodgeYousoroad);
        arrangeSectionToRowsV2(ref rowsLeftLodgeKananrail, leftLodgeKananrail);
        arrangeSectionToRowsV2(ref rowsLeftLodgeYousoroad, leftLodgeYousoroad);
        arrangeSectionToRowsV2(ref rowsLeftLeftLodgeNormal, leftLeftLodgeNormal);
        */
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

    void arrangeSectionToRowsV2(ref List<List<GameObject>> rowList, List<GameObject> normList)
    {
        int currRow = 0;
        float threshold = 0.8f;
        float comparisonObject = 0.0f;
        float currDist = 0;

        if (normList.Count > 0)
            comparisonObject = Vector3.Distance(Vector3.Scale(lookAtObject.transform.position, new Vector3(1,0,1)), Vector3.Scale(normList[0].transform.position, new Vector3(1,0,1)));

        for (int i = 0; i < normList.Count; i++)
        {
            currDist = Vector3.Distance(Vector3.Scale(lookAtObject.transform.position, new Vector3(1, 0, 1)), Vector3.Scale(normList[i].transform.position, new Vector3(1, 0, 1)));

            if (Mathf.Abs(currDist - comparisonObject) < threshold)
            {
                rowList[currRow].Add(normList[i]);
            }
            else
            {
                rowList.Add(new List<GameObject>());
                rowList[currRow + 1].Add(normList[i]);
                comparisonObject = currDist;
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
            case Action.Yousoroad:
                actionYousoroad();
                break;
            case Action.Together:
                actionTogether();
                break;
            default:
                break;
        }












        /*
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            currColor = (currColor - 1) % (int) COLOR.MAX;
            changeAllColor();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currColor = (currColor + 1) % (int)COLOR.MAX;
            changeAllColor();
        }
        */

        /*
        if (Time.time > tempTimeMark)
        {
            //leftLeftLodgeNormal[tempTimeCounter].transform.Rotate(-90, 0, 0);
            /*
            for (int i = 0; i < rowsLeftLeftLodgeNormal[tempTimeCounter].Count; i++)
            {
                rowsLeftLeftLodgeNormal[tempTimeCounter][i].transform.Rotate(-90, 0, 0);
            }
            tempTimeCounter++;
            tempTimeMark += 1.00f;
           
        }
        */

        /*
        float val = Mathf.Sin(Time.time * 14);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));
        lastRotationVal = val;

        for (int i = 0; i < (int)SEC.MAX; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
            }
        }
        */

    }

    void actionManager()
    {
        if (videoPlayer.time > 7.11f)
        {
            action = Action.Down;
        }

        //16.80 end time
        //6 seconds of raising lights
        //85 rows total - 6pit - 45orch - 21 - (3) - (11)
        //0.07s per row
        if (videoPlayer.time > 10.6f)
        {
            action = Action.Yousoroad;
        }
        
        if (videoPlayer.time > 23.3f)
        {
            action = Action.Together;
        }
    }

    void actionTogether()
    {
        float dist = 0.35f;

        if (videoPlayer.time > (timeComparisonTogether + dist))
        {
            timeComparisonTogether += dist;
            togetherCounter++;
        }

        if (togetherCounter < 3)
        {
            enableBlueLightRandom(togetherCounter);
            actionDefault(togetherCounter);
        }
        else
            actionDefault();

       // actionDefault();
    }

    void enableBlueLightRandom(int currCounter)
    {
        int portion = randomList.Count / 2;
        int start = currCounter * portion;
        int end = start + portion;

        if (currCounter > 1)
        {
            start = 0;
            end = randomList.Count;
        }

        for (int i = start; i < end; i++)
        {
            GameObject child = randomList[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[(int) COLOR.AQUA];
        }
    }

    void enableBlueLight(List<GameObject> currRow)
    {
        for (int i = 0; i < currRow.Count; i++)
        {
            GameObject child = currRow[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[(int) COLOR.AQUA];
        }
    }

    void upRow(List<GameObject> currRow)
    {
        int rotRate = -1000;
        enableBlueLight(currRow);

        //Debug.Log(currRow[0].transform.localEulerAngles.x);

        for (int i = 0; i < currRow.Count; i++)
        {
            if (currRow[i].transform.localEulerAngles.x > 0 )
            {
                if (currRow[i].transform.localEulerAngles.x + rotRate * Time.deltaTime > 0)
                    currRow[i].transform.Rotate(rotRate * Time.deltaTime, 0, 0);
                //Quaternion localRotation = Quaternion.Euler(new Vector3(rotRate, 0, 0));
                //secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
                else
                {
                    currRow[i].transform.Rotate(-currRow[i].transform.localEulerAngles.x, 0, 0);
                }
            }
        }

    }

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
                section[i][j].transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin((float) videoPlayer.time * 25) * 20);
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
            shakeRows(centerPitYousoroad);
        }

        if (i >= 2)
        {
            shakeRows(rowsCenterOrchestraYousoroad, i - 1);
        }

        if (i >= rowsCenterOrchestraYousoroad.Count + 2)
        {
            shakeRows(rowsLeftLodgeYousoroad, i - rowsCenterOrchestraYousoroad.Count - 1);
            shakeRows(rowsRightLodgeYousoroad, i - rowsCenterOrchestraYousoroad.Count - 1);
            shakeRows(rowsCenterLodgeYousoroad, i - rowsCenterOrchestraYousoroad.Count - 1);
        }
    }

    void upRowManager(int i)
    {
        int maxRows = rowsCenterOrchestraYousoroad.Count + rowsRightLodgeYousoroad.Count + 1;

        // Pit
        if (i == 0)
        {
            upRow(centerPitYousoroad);
            //centerPitYousoroad leftLodgeYousoroad rightLodgeYousoroad
        }

        // Orchestra
        else if (i < rowsCenterOrchestraYousoroad.Count + 1)
        {
            upRow(rowsCenterOrchestraYousoroad[i - 1]);
        }

        // Lodge
        else
        {
            if (i - rowsCenterOrchestraYousoroad.Count - 1 < rowsLeftLodgeYousoroad.Count)
                upRow(rowsLeftLodgeYousoroad[i - rowsCenterOrchestraYousoroad.Count - 1]);

            if (i - rowsCenterOrchestraYousoroad.Count - 1 < rowsRightLodgeYousoroad.Count)
                upRow(rowsRightLodgeYousoroad[i - rowsCenterOrchestraYousoroad.Count - 1]);

            if (i - rowsCenterOrchestraYousoroad.Count - 1 < rowsCenterLodgeYousoroad.Count)
                upRow(rowsCenterLodgeYousoroad[i - rowsCenterOrchestraYousoroad.Count - 1]);
        }
    }

    void actionYousoroad()
    {
        // 59 Rows
        int maxRows = rowsCenterOrchestraYousoroad.Count + rowsRightLodgeYousoroad.Count + 1;

        if (numRow < maxRows)
            upRowManager(numRow);

        shakeRowManager(numRow);

        if (videoPlayer.time > timeComparisonYousoroad)
        {
            timeComparisonYousoroad += 0.1f;
            numRow++;
        }
    }

    void actionDown()
    {
        int rotRate = 3;
        disablePenlight();

        /*
        for (int i = 0; i < secList.Count; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                if (secList[i][j].transform.localEulerAngles.x + rotRate < 90 || secList[i][j].transform.localEulerAngles.x > 300)
                {
                    secList[i][j].transform.Rotate(rotRate, 0, 0);
                    //Quaternion localRotation = Quaternion.Euler(new Vector3(rotRate, 0, 0));
                    //secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
                }
            }
        }
        */

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

    void actionDefault()
    {
        float val = Mathf.Sin((float)videoPlayer.time * 15);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));

        for (int i = 0; i < secList.Count; i++)
        {
            for (int j = 0; j < secList[i].Count; j++)
            {
                secList[i][j].transform.rotation = secList[i][j].transform.rotation * localRotation;
                //secList[i][j].transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time * 14) * 45, 0, 0);
                //secList[i][j].transform.localRotation = Quaternion.Lerp(secList[i][j].transform.localRotation, Quaternion.Euler(90, 0, 0), 0.05f);

            }
        }

        lastRotationVal = val;

        //Debug.Log(secList[0][0].transform.localEulerAngles.x);

    }

    void actionDefault(List<GameObject> list)
    {
        float val = Mathf.Sin((float)videoPlayer.time * 15);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));

        for (int i = 0; i < list.Count; i++)
        {
               list[i].transform.rotation = list[i].transform.rotation * localRotation;
                //secList[i][j].transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time * 14) * 45, 0, 0);
                //secList[i][j].transform.localRotation = Quaternion.Lerp(secList[i][j].transform.localRotation, Quaternion.Euler(90, 0, 0), 0.05f);
        }

        lastRotationVal = val;

        //Debug.Log(secList[0][0].transform.localEulerAngles.x);

    }

    void actionDefault(int currCounter)
    {
        int portion = randomList.Count / 2;
        int end = currCounter * portion;

        float val = Mathf.Sin((float)videoPlayer.time * 15);
        val = val * 45;

        Quaternion localRotation = Quaternion.Euler(new Vector3(val - lastRotationVal, 0, 0));


        if (currCounter > 1)
        {
            end = randomList.Count;
        }

        for (int i = 0; i < end; i++)
        {
            // THIS ISNT RIGHT
            //randomList[i].transform.rotation = Quaternion.Euler(0, randomList[i].transform.eulerAngles.y, 0);
            randomList[i].transform.rotation = Quaternion.Euler(Mathf.Sin((float)videoPlayer.time * 15) * 45, 0, 0);
            //randomList[i].transform.rotation = randomList[i].transform.rotation * localRotation;

        }

        lastRotationVal = val;

    }

    /*
    void disablePenlight()
    {
        // Start 7.11  - End 8.8
        int start = 0;
        int end = 0;

        for (int i = 0; i < secList.Count; i++)
        {
            if (videoPlayer.time > 8.37f)
            {
                start = (secList[i].Count * 3) / 4;
                end = secList[i].Count;
            }
            else if (videoPlayer.time > 7.95f)
            {
                start = secList[i].Count / 2;
                end = (secList[i].Count*3) / 4;
            }
            else if (videoPlayer.time > 7.53f)
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
    }
    */

    void disablePenlight()
    {
        // Start 7.11  - End 8.8
        // 0.21125 8 splits
        int start = 0;
        int end = 0;

        /*
        if (videoPlayer.time > 8.589f)
        {
            start = (randomListAll.Count * 7) / 8;
            end = randomListAll.Count;
        }
        */
        if (videoPlayer.time > 8.378f)
        {
            start = (randomListAll.Count  * 6) / 8;
            end = randomListAll.Count;
        }
        else if (videoPlayer.time > 8.16f)
        {
            start = (randomListAll.Count * 5) / 8;
            end = (randomListAll.Count * 6) / 8;
        }
        else if (videoPlayer.time > 7.95f)
        {
            start = (randomListAll.Count * 4) / 8;
            end = (randomListAll.Count * 5) / 8;
        }
        else if (videoPlayer.time > 7.74f)
        {
            start = (randomListAll.Count * 3) / 8;
            end = (randomListAll.Count * 4) / 8;
        }
        else if (videoPlayer.time > 7.532f)
        {
            start = (randomListAll.Count * 2) / 8;
            end = (randomListAll.Count * 3) / 8;
        }
        else if (videoPlayer.time > 7.32f)
        {
            start = randomListAll.Count / 8;
            end = (randomListAll.Count * 2) / 8;
        }
        else
        {
            start = 0;
            end = randomListAll.Count / 8;
        }

        for (int i = start; i < end; i++)
        {
            GameObject child = randomListAll[i].transform.GetChild(0).gameObject;
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            mesh.material = pColor[9];
        }
    }

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
