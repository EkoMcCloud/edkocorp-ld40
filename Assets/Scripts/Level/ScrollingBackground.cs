using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EdkoCorpLD40.Managers;

namespace EdkoCorpLD40.Level
{
    public class ScrollingBackground : MonoBehaviour
    {
        public List<GameObject> layersTiles;

        public int repeat = 3;

        protected List<List<GameObject>> tiles;

        // Use this for initialization
        protected void Awake () 
        {
            Debug.Log("Awake");
            Debug.Log(layersTiles.Count);
            tiles = new List<List<GameObject>>();
            for (int i = 0;  i < layersTiles.Count; i++)  {
                GameObject toInstanciate = layersTiles[i];
                List<GameObject> repeatTiles = new List<GameObject>();
                for (int j = 0; j < repeat; j++) {
                    GameObject instance = Instantiate(toInstanciate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    float width = GetWidth(instance);
                    instance.transform.SetParent(transform);
                    instance.transform.position = new Vector3(width * j, Camera.main.transform.position.y, 0);
                    repeatTiles.Add(instance);
                }
                tiles.Add(repeatTiles);
            }
        }

        protected void Update()
        {
            for (int i = 0; i < tiles.Count; i++) {
                int ratio = i + 1;
                // float moveX = 60 * Time.deltaTime * - GameManager.instance.levelManager.trainSpeed * ratio;
                float moveX = 60 * Time.deltaTime * - (GameManager.instance.levelManager.trainSpeed / 100) / ratio;
                // Debug.Log("Move x : " + moveX);
                float yPosition = Camera.main.transform.position.y;
                // Debug.Log(moveX);
                for (int j = 0; j < tiles[i].Count; j++) {
                    tiles[i][j].transform.position = new Vector3(
                        tiles[i][j].transform.position.x + moveX,
                        yPosition,
                        tiles[i][j].transform.position.z
                    );
                }
            }
            
            RepositionBackground();
        }

        private void RepositionBackground()
        {
            for (int i = 0; i < tiles.Count; i++) {
                for (int j = 0; j < tiles[i].Count; j++) {
                    float width = GetWidth(tiles[i][j]);
                    float xLimit = Camera.main.transform.position.x - (width * tiles[i].Count / 2); 
                    // Debug.Log("Level : " + i + " => Limit : " + xLimit);
                    // Debug.Log("Camerapos : " + Camera.main.transform.position.x);
                    if (tiles[i][j].transform.position.x < xLimit) {
                        GameObject nextTile;
                        if(j == 0) {
                            nextTile = tiles[i][tiles[i].Count - 1];
                        } else {
                            nextTile = tiles[i][j - 1];
                        } 
                        // float newX = Camera.main.transform.position.x + (width * tiles[i].Count / 2); 
                        float newX = width + nextTile.transform.position.x; 
                        tiles[i][j].transform.position = new Vector3(
                            newX, 
                            tiles[i][j].transform.position.y, 
                            tiles[i][j].transform.position.z
                        );
                    }
                }
            }
            //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
            // Vector2 groundOffSet = new Vector2(sprite.size.x * 2f, 0);

            //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
            // transform.position = (Vector2) transform.position + groundOffSet;
        }

        private float GetWidth(GameObject gameObject)
        {
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            if (renderer != null) {
                return renderer.bounds.size.x;
            } else if (collider != null) {
                return collider.bounds.size.x;
            } else {
                Vector3 p1 = gameObject.transform.TransformPoint(0, 0, 0);
                Vector3 p2 = gameObject.transform.TransformPoint(1, 1, 0);
                return p2.x - p1.x;
            }
        }
    }
}