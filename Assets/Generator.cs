using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Generator : MonoBehaviour {

    public GameObject dirt_prefab;
    public List<GameObject> all_blocks;
    
    // height and width 
    float minX;
    float maxX ;
    float minY;
    float maxY ;


    int leftmaxy = -5;
    int leftminy = -15;

    int rightmaxx =  5;
    int rightminx = -5;

    public int max_column_height;
    float width;
    float height;

    int roughness = 60;
    float resolution;

    int depth;

    // save the mountain space in float: 

    public float[] mountain;
    void Start () {
        max_column_height = 0;

        minX = -1f;
        minY = -2.4f;
        maxY = -0.8f;

        mountain = new float[roughness];
        mountain[0] = minY;
        mountain[mountain.Length - 1] = minY;

        width = dirt_prefab.transform.lossyScale.x;



        mountain[mountain.Length / 2 + 1] = maxY;
        mountain[mountain.Length / 2] = maxY;

        midpoint(0, mountain.Length / 2, 6f);
        midpoint(mountain.Length / 2 + 1, mountain.Length - 1, 6f);

        generate();
    }

    // Using the midpoint method to get noise 
    void midpoint(int s, int e, float r)
    {

        int middle = (s + e) / 2;

        if (middle != s)
        {
            float newHeight = (mountain[s] + mountain[e]) / 2;
            mountain[middle] = Mathf.Max(newHeight + Random.Range(-r, r), 0);

            midpoint(s, middle, r / 2);
            midpoint(middle, e, r / 2);
        }
    }

    	void generate(){
		for(int i = 0; i < mountain.Length; i++){
			GameObject temp = Instantiate(dirt_prefab, new Vector3(minX + i*width/3 ,mountain[i], -2), new Quaternion());
            all_blocks.Add(temp);
            temp.transform.parent = this.transform;
            temp.transform.localScale += new Vector3(0, 1, 0);
			temp.transform.position += new Vector3((float)(minX * 1), (float) (maxY * 4.5), 0);

		}
	}



}


