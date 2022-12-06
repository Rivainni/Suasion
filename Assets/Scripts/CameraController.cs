using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] path;
    [SerializeField] float speed;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[index].position, Time.deltaTime * speed);
        if (transform.position == path[index].position)
        {
            if (index == path.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }
}
