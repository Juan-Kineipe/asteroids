using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUtils : MonoBehaviour
{
    public float screenBottom;
    public float screenTop;
    public float screenLeft;
    public float screenRight;

    public static ScreenUtils instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        this.screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        this.screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        this.screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        this.screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
