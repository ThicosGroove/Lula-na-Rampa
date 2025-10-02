using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static void  ClearList()
    {
        GamePlayManager.Instance.objList.RemoveAll(item => item == null);
    }
}
