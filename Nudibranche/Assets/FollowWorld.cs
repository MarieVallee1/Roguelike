using UnityEngine;

public class FollowWorld : MonoBehaviour
{

    [SerializeField] private Transform lookAt;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = cam.WorldToScreenPoint(lookAt.position + offset);
        Debug.Log(pos);

        cam.WorldToScreenPoint(lookAt.position);

        if ((Vector2)transform.position != pos)
        {
            transform.position = pos;
        }
    }
}
