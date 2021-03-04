using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanelController : MonoBehaviour
{
    RectTransform rectTransform;
    GameObject buttonPrefab;
    GameObject buttonParent;
    List<GameObject> buttonList = new List<GameObject> { };
    // Start is called before the first frame update
    void Start()
    {
        buttonPrefab = Resources.Load<GameObject>("CollectionButtonPrefab");
        buttonParent = GameObject.Find("CollectionPanel/CollectionButtonListPanel/ScrollView/Viewport/Content");
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
        this.gameObject.SetActive(false);
    }

    public void AddAllEpisodeButtons()
    {
        foreach(var item in EpisodeCollection.Instance.AllEpisode)
        {
        GameObject button = Instantiate(buttonPrefab, buttonParent.transform);
        buttonList.Add(button);
        button.transform.GetComponentInChildren<Text>().text = item.title;
        button.GetComponent<Button>().onClick.AddListener(() =>
        {

        });
        }
    }
    void OnEnable()
    {
        if(buttonPrefab != null)
        {
            while (buttonList.Count != 0)
            {
                Destroy(buttonList[0]);
                buttonList.RemoveAt(0);
            }
            AddAllEpisodeButtons();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
