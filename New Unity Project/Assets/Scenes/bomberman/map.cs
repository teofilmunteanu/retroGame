﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map : MonoBehaviour
{
    public static int h = 11;
    public static int w = 51;

    GameObject player;

    public static block[,] grid = new block[w, h];

    public static enemy[] enemies = new enemy[6];

    public static bool inside(int x, int y)
    {
        return (x >= 0 && x < w) && (y >= 0 && y < h);
    }

    public static bool characteresCanMove(int X, int Y)
    {
        if (X < 5 && Y < 5)
            return false;
        else
        {

            
            foreach (enemy Enemy in enemies)
            {
                int x = (int)Enemy.transform.position.x;
                int y = (int)Enemy.transform.position.y;
                if (x == X && y == Y)
                    return false;
                for (int i = 1; i <= Enemy.minRange / 2; ++i)
                {
                    //top and right
                    int newX = x + ((Enemy.onX) ? i : 0);
                    int newY = y + ((Enemy.onY) ? i : 0);
                    if (newX == X && newY == Y)
                        return false;

                    //left and down
                    newX = x - ((Enemy.onX) ? i : 0);
                    newY = y - ((Enemy.onY) ? i : 0);
                    if (newX == X && newY == Y)
                        return false;
                }
            }
        }
        return true;
    }

    public static void removeDestroyableWall(int x, int y)
    {
        print("remove");
        map.grid[x, y].tag = "empty";
        map.grid[x, y].loadNewSprite(1);
        map.grid[x, y].isDestroyable = false;
        map.grid[x, y].isUndestroyable = false;
        map.grid[x, y].isEmpty = true;
        map.grid[x, y].GetComponent<BoxCollider2D>().enabled = false;
    }

    public void placeBomb(int x, int y)
    {
        GameController.activBomb = true;
        GameController.bombTimer = 2f;
        GameController.pos = new Vector2(x, y);
        grid[x, y].loadNewSprite(0);
    }

    void BOOM(Vector2 v)
    {
        int[] dx = new int[4] { 1, 0, -1, 0 };
        int[] dy = new int[4] { 0, 1, 0, -1 };


        for (int i = 0; i < 4; ++i)
        {
            for(int j = 1; j <= 2; ++j)
            {
                int newX = (int)v.x + dx[i] * j;
                int newY = (int)v.y + dy[i] * j;

                if (grid[newX, newY].isUndestroyable)
                {
                    j = 3;
                }
                else
                {
                    if(grid[newX, newY].isDestroyable)
                    {
                        removeDestroyableWall(newX, newY);
                        j = 3;
                    }
                    else
                    {
                        foreach (enemy Enemy in enemies)
                        {
                            if (Enemy != null)
                            {
                                int x = (int)Enemy.transform.position.x;
                                int y = (int)Enemy.transform.position.y;

                                if (x == newX && y == newY)
                                {
                                    Destroy(Enemy);
                                    Enemy.GetComponent<SpriteRenderer>().sprite = null;
                                    Enemy.GetComponent<CircleCollider2D>().enabled = false;
                                    Enemy.GetComponent<BoxCollider2D>().enabled = false;
                                    
                                    GameController.noOfEnemies--;
                                }
                            }

                        }
                    }

                }

            }
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        int x = Mathf.RoundToInt(player.transform.position.x);
        int y = Mathf.RoundToInt(player.transform.position.y);

        if (Input.GetKeyDown(KeyCode.Space) && !GameController.activBomb)
        {             
            if(grid[x,y].CompareTag("empty"))
                placeBomb(x, y);

        }
        if(GameController.activBomb && GameController.bombTimer < 0)
        {
            BOOM(GameController.pos);
            grid[(int)GameController.pos.x,(int)GameController.pos.y].loadNewSprite(1);
            GameController.activBomb = false;
            GameController.bombTimer = 2f;

        }



    }

}