using UnityEngine;

public class UIMapSelector : MonoBehaviour
{

    public void SelectMap(int selectedMap)
    {
        if (selectedMap == 0)
        {
            SceneDataManager._instance.isSelect = false;
            Debug.Log("Select Map: Random");
        }
        else
        {
            SceneDataManager._instance.isSelect = true;
            SceneDataManager._instance.mapNum = selectedMap - 1;
            Debug.LogFormat("Select Map:{0}", (GameMap)selectedMap - 1);
        }
    }
}
