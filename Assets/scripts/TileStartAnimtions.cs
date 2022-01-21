using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// class that responsible for one tile faling
/// </summary>
public class TileStartAnimtions : MonoBehaviour
        {
            private bool isThisBigTile;
            private float scaleTile;
            private float startPositionX;
            private bool isTileDragByMouse;
            private float startPosX;
            private float startPosY;
            private const float startPositionY=6;
            
            /// <summary>
            /// function that it gives the random position ,rotation and scale
            /// </summary>
            void Start()
            {
                Rigidbody2D rigidbody2DTile = GetComponent<Rigidbody2D>();
                float gravityScaleTile = 1.7f;
                rigidbody2DTile.gravityScale = gravityScaleTile;
                if(isThisBigTile)
                {
                    scaleTile = Random.Range(1.1f, 1.3f);
                }
                else
                {
                    scaleTile = Random.Range(0.3f, 0.9f);
                }
                transform.localScale = new Vector2(scaleTile, scaleTile);
                float rotationCubeZ = Random.Range(-45f, 45f);
                transform.eulerAngles = new Vector3(0, 0, rotationCubeZ);
                
                if(Random.Range(1, 3) == 1)
                {
                    startPositionX = Random.Range(9f, 4f); 
                }
                else
                {
                    startPositionX = Random.Range(-9f, -4f); 
                }
                transform.position = new Vector2(startPositionX, startPositionY);
            }
            
            /// <summary>
            /// the function check if the user pressing the tile and if so she move it
            /// </summary>
            void Update()
            {
                if (isTileDragByMouse)
                {
                    tileDragByTheMouse();
                }
            }

            /// <summary>
            /// function that being called when user press the tile in the background
            /// the function indicate that the user is dragging the tile
            /// </summary>
            private void OnMouseDown()
            {
                isTileDragByMouse = true;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                startPosX = mousePos.x - this.transform.position.x;
                startPosY = mousePos.y - this.transform.position.y;
            }

            
            /// <summary>
            /// function that being called when user stop pressing the tile in the background
            /// the function indicate that the user stoped dragging the tile
            /// </summary>
            private void OnMouseUp()
            {
                isTileDragByMouse = false;
            }

            /// <summary>
            /// the funciton move the tile to the curser location
            /// </summary>
            private void tileDragByTheMouse()
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector2(mousePos.x - startPosX, mousePos.y - startPosY);
            }
            
            public bool IsThisBigTile
            {
                get => isThisBigTile;
                set => isThisBigTile = value;
            }
        }
