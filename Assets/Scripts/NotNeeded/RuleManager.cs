using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RuleManager : MonoBehaviour
{
    private static RuleManager instance;
    public List<GameObject> isObject = new List<GameObject>();

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this.gameObject);
    }
    public static RuleManager Instance
    {
        get
        {
            if(instance == null) return null;
            return instance;
        }
    }
    void Start()
    {
        foreach(GameObject ob in PlayerMove.Instance.allObjects)
        {
            if(ob.name == "Is(Clone)") isObject.Add(ob);
        }
    }

    void Update()
    {
        // 리셋버튼. r을 누르면 현재 씬을 불러옴.
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 문자가 이어지는지(baba is you, wall is you, wall is stop 등등) 체크함.
    public bool[] checkTrue(GameObject io, bool[] bools)
    {
        bool leftWall = false, topWall = false, leftBaba = false, topBaba = false, 
        leftFlag = false, topFlag = false, rightStop = false, bottomStop = false,
        rightYou = false, bottomYou = false, rightWin = false, bottomWin = false,
        rightPush = false, bottomPush = false, leftRock = false, topRock = false,
        topWater = false, leftWater =  false, rightSink = false, bottomSink = false;
        foreach(GameObject ao in PlayerMove.Instance.allObjects)
        {
            if((io.transform.position.x - 1 == ao.transform.position.x) && (io.transform.position.y == ao.transform.position.y))
            {
                if(ao.name == "WaterString(Clone)") leftWater = true;
                if(ao.name == "RockString(Clone)") leftRock = true;
                if(ao.name == "WallString(Clone)") leftWall = true;
                if(ao.name == "BabaString(Clone)") leftBaba = true;
                if(ao.name == "FlagString(Clone)") leftFlag = true;
            }
            if((io.transform.position.y + 1 == ao.transform.position.y) && (io.transform.position.x == ao.transform.position.x))
            {
                if(ao.name == "WaterString(Clone)") topWater = true;
                if(ao.name == "RockString(Clone)") topRock = true;
                if(ao.name == "WallString(Clone)") topWall = true;
                if(ao.name == "BabaString(Clone)") topBaba = true;
                if(ao.name == "FlagString(Clone)") topFlag = true;
            }
            if((io.transform.position.x + 1 == ao.transform.position.x) && (io.transform.position.y == ao.transform.position.y))
            {
                if(ao.name == "SinkString(Clone)") rightSink = true;
                if(ao.name == "Stop(Clone)") rightStop = true;
                if(ao.name == "You(Clone)") rightYou = true;
                if(ao.name == "Win(Clone)") rightWin = true;
                if(ao.name == "Push(Clone)") rightPush = true;
            }
            if((io.transform.position.y - 1 == ao.transform.position.y) && (io.transform.position.x == ao.transform.position.x))
            {
                if(ao.name == "SinkString(Clone)") bottomSink = true;
                if(ao.name == "Stop(Clone)") bottomStop = true;
                if(ao.name == "You(Clone)") bottomYou = true;
                if(ao.name == "Win(Clone)") bottomWin = true;
                if(ao.name == "Push(Clone)") bottomPush = true;
            }
        }
        if((leftBaba && rightStop) || (topBaba && bottomStop)) bools[0] = true;
        if((leftWall && rightStop) || (topWall && bottomStop)) bools[1] = true;
        if((leftFlag && rightStop) || (topFlag && bottomStop)) bools[2] = true;
        if((leftBaba && rightYou) || (topBaba && bottomYou)) bools[3] = true;
        if((leftWall && rightYou) || (topWall && bottomYou)) bools[4] = true;
        if((leftFlag && rightYou) || (topFlag && bottomYou)) bools[5] = true;
        if((leftBaba && rightWin) || (topBaba && bottomWin)) bools[6] = true;
        if((leftWall && rightWin) || (topWall && bottomWin)) bools[7] = true;
        if((leftFlag && rightWin) || (topFlag && bottomWin)) bools[8] = true;
        if((leftBaba && rightPush) || (topBaba && bottomPush)) bools[9] = true;
        if((leftWall && rightPush) || (topWall && bottomPush)) bools[10] = true;
        if((leftFlag && rightPush) || (topFlag && bottomPush)) bools[11] = true;

        if((leftRock && rightStop) || (topRock && bottomStop)) bools[12] = true;
        if((leftRock && rightWin) || (topRock && bottomWin)) bools[13] = true;
        if((leftRock && rightYou) || (topRock && bottomYou)) bools[14] = true;
        if((leftRock && rightPush) || (topRock && bottomPush)) bools[15] = true;

        if((leftWater && rightStop) || (topWater && bottomStop)) bools[16] = true;
        if((leftWater && rightWin) || (topWater && bottomWin)) bools[17] = true;
        if((leftWater && rightYou) || (topWater && bottomYou)) bools[18] = true;
        if((leftWater && rightPush) || (topWater && bottomPush)) bools[19] = true;

        if((leftBaba && rightSink) || (topBaba && bottomSink)) bools[20] = true;
        if((leftWater && rightSink) || (topWater && bottomSink)) bools[21] = true;
        if((leftRock && rightSink) || (topRock && bottomSink)) bools[22] = true;
        if((leftWall && rightSink) || (topWall && bottomSink)) bools[23] = true;
        if((leftFlag && rightSink) || (topFlag && bottomSink)) bools[24] = true;
        return bools; 
    }

    // 만약 문자가 이어진다면, 그 오브젝트의 bool값을 바꿔준다. 반대로 이어지지 않는다면 또 바꿔준다.
    public void isWhat()
    {
        bool[] bools = new bool[25];
        foreach(GameObject io in isObject)
        {
            bools = checkTrue(io, bools);
        }
        if(bools[0])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isStop = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isStop = false;
            }
        }
        if(bools[1])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isStop = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isStop = false;
            }
        }
        if(bools[2])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isStop = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isStop = false;
            }
        }
        if(bools[3])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isYou = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isYou = false;
            }
        }
        if(bools[4])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isYou = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isYou = false;
            }
        }
        if(bools[5])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isYou = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isYou = false;
            }
        }
        if(bools[6])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isWin = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isWin = false;
            }
        }
        if(bools[7])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isWin = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isWin = false;
            }
        }
        if(bools[8])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isWin = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isWin = false;
            }
        }
        if(bools[9])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isPush = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba") ob.GetComponent<Rule>().isPush = false;
            }
        }
        if(bools[10])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isPush = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isPush = false;
            }
        }
        if(bools[11])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isPush = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isPush = false;
            }
        }
        if(bools[12])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isStop = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isStop = false;
            }
        }
        if(bools[13])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isWin = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isWin = false;
            }
        if(bools[14])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isYou = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isYou = false;
            }
        }
        if(bools[15])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isPush = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isPush = false;
            }
        }
        if(bools[16])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isStop = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isStop = false;
            }
        }  
        if(bools[17])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isWin = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isWin = false;
            }
        }  
        if(bools[18])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isYou = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isYou = false;
            }
        } 

        PlayerMove.Instance.currentPlayer = new List<GameObject>();
        foreach(GameObject ob in PlayerMove.Instance.allObjects)
        {
            if(ob.GetComponent<Rule>().isYou) PlayerMove.Instance.currentPlayer.Add(ob);
        }
        
        if(bools[19])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isPush = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isPush = false;
            }
        } 
        if(bools[20])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba(Clone)") ob.GetComponent<Rule>().isSink = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Baba(Clone)") ob.GetComponent<Rule>().isSink = false;
            }
        } 
        if(bools[21])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isSink = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Water(Clone)") ob.GetComponent<Rule>().isSink = false;
            }
        } 
        if(bools[22])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isSink = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Rock(Clone)") ob.GetComponent<Rule>().isSink = false;
            }
        }
        if(bools[23])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isSink = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Wall(Clone)") ob.GetComponent<Rule>().isSink = false;
            }
        }
        if(bools[24])
        {
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isSink = true;
            }
        }else{
            foreach(GameObject ob in PlayerMove.Instance.allObjects)
            {
                if(ob.name == "Flag(Clone)") ob.GetComponent<Rule>().isSink = false;
            }
        }   
        }
    }

    // 이겼는지 확인하는 함수. 현재 선택된 플레이어와 다른 모든 오브젝트(isWin이 트루인)들을 검사하여
    // 포지션이 같다면 트루, 아니라면 폴스. 지금 생각해보니 현재 선택된 플레이어가 isWin인 경우도 검사해야할듯?
    public bool isWin()
    { 
        List<GameObject> allActive = new List<GameObject>();
        foreach (GameObject ap in PlayerMove.Instance.allObjects)
        {
            if(ap.activeSelf)
            {
                allActive.Add(ap);
            }
        }

        foreach(GameObject ob in PlayerMove.Instance.currentPlayer)
        {
            foreach(GameObject ao in allActive)
            {
                if(ob == ao) continue;
                if(ob.transform.position == ao.transform.position)
                {
                    if(ao.GetComponent<Rule>().isWin) return true;
                }
            }
        }
        return false;
    }
}
