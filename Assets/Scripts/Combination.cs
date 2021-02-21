using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class Combination
{
    private int n = 0;
    private int r = 0;
    public List<List<int>> data = new List<List<int>> { };

    public Combination(int n, int r)
    {
        if (n < 0 || r < 0)
        {
            throw new Exception("Negative parameter in constructor");
        }
        if (n < r) throw new Exception("r is bigger than n");
        this.n = n;
        this.r = r;
        DoCombination(this.data, this.n, this.r);
    }
    public void DoCombination(List<List<int>> data,int n,int r)
    {
        data.Clear();
        switch (r)
        {
            case 3:
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        for (int k = j + 1; k < n; k++)
                        {
                            List<int> temp = new List<int> { i, j, k };
                            data.Add(temp);
                        }
                    }
                }
                break;
            default:
                throw new Exception("r must be 1,2 and 3");
        }

    }

    public void printCombination()
    {
        string temp = "";
        foreach(var nums in data)
        {
            temp = "";
            foreach (var num in nums)
            {
                temp += num + " "; 
            }
            Debug.Log(temp);
        }
    }
}
