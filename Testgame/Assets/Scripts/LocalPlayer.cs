using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    //public int speed;

    void UseMethodById(int _id, Object[] _objects) 
    {
        switch (_id)
        {
            case 0:
                Move(_objects[0], _objects[1]);
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move(Vector3 _position, Vector3 _rotation)
    {
        transform.position = _position;
        transform.rotation = Quaternion.Euler(_rotation);
    }
}
