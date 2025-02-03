using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class test : MonoBehaviour
{
    float timeTaken = 0f;
    float targetTime = 5f;

    Vector3 startPos = new Vector3(0, 0, 0);
    Vector3 endPos = new Vector3(0, 0, 0);

    public TextAsset jsonFile;

    [SerializeField] private Dictionary<KeyCode,ValueAttribute> player;


    void Move()
    {
        if (timeTaken < targetTime)
        {
            timeTaken += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, timeTaken / targetTime);

        }
    }
    [System.Serializable]
    public class PlayerData
    {
        public int playerScore;
        public string playerName;

    }

    [System.Serializable]
    public class PlayerListWrapper
    {
        public List<PlayerData> playerList;
    }

    public List<PlayerData> playerList = new List<PlayerData>();
    void Start()
    {
        PlayerListWrapper wrapper = JsonUtility.FromJson<PlayerListWrapper>(jsonFile.text);

        // playerList = jsonData.playerData;
        playerList.AddRange(wrapper.playerList);

    }

}
