using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerinNoise {
    long seed;

    public PerinNoise(long seed){
        this.seed = seed;
    }

    private int random(int x , int range)
    {
        int n= (int)(((x + seed) ^ 5) % range);
        return n;
    }

    // chunk size = 
    public int getNoise(int x,int range)
    {
        int chunksize = 16;

        float noise = 0;

        range /= 2;

        while (chunksize > 0)
        {
            int chunkindex = x / chunksize;
            float prog = (x % chunksize) / (chunksize * 1f);

            // Random value a nd b 
            float leftRandom = random(chunkindex, range);   // make
            float rightRandom = random(chunkindex + 1, range);

            noise += (1 - prog) * leftRandom + prog * rightRandom; // interpolate function 

            chunksize /= 2;
            range /= 2;

            range = Mathf.Max(1, range);
        }

        return (int)Mathf.Round(noise);
    }
}
