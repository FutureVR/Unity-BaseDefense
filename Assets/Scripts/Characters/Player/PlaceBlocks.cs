using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceBlocks : MonoBehaviour {

    //public enum blockNames {empty, wall};
    public GameObject[] blocks;
    public Person person;

    Camera cam;
    LineRenderer lineRenderer;
    public GameObject terrain;
    public Transform bottomRightCorner;
    //public GameObject wallPrefab;

    public Material line_material;
    public Material plane_material;

    List<List<int>> mapOfInts;
    List<List<GameObject>> mapOfObjects;

    public Vector3 blockSize;
    Vector3 worldSize;  
    int x_size;
    int z_size;

    bool Fire1_down = false;

	void Start ()
    {
        //person = GameObject.FindGameObjectWithTag("Player").GetComponent<Person>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = line_material;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        worldSize = terrain.transform.localScale * 10;
        x_size = (int)(worldSize.x / blockSize.x);
        z_size = (int)(worldSize.z / blockSize.z);

        foreach (GameObject b in blocks)
        {
            b.transform.localScale = blockSize;
        }

        initializeMaps();
	}

    int currBlock = 0;
    Vector3 initial_indexed_pos;
    Vector3 final_indexed_pos;
    Vector3 initial_pos;
    Vector3 final_pos;

    void Update ()
    {
        handle_keyboard_block_selection_input();
        handle_mouse_placement_input();
    }

    void handle_keyboard_block_selection_input()
    {
        if (Input.anyKeyDown)
        {
            string keyInput = Input.inputString;
            int selection = -1;

            if (keyInput.Length != 0) selection = keyInput[0] - 48;
            if (1 <= selection && selection <= blocks.Length) currBlock = selection - 1;
        }
    }

    void handle_mouse_placement_input()
    {
        Mouse_Down();

        //Sets the positions for the lineRenderer if the mouse is pressed down
        //Sets the final_indexed_pos
        if (Fire1_down == true)
        {
            //Set the final_indexed_pos depending on the shift key
            if (Input.GetButton("Snap"))
            {
                //Set the final position to the snapped version in one line
                Vector3 initial_to_final = cam.ScreenToWorldPoint(Input.mousePosition) - initial_pos;
                final_pos = initial_pos;

                //Test if the line should be horizontal
                if (Mathf.Abs(initial_to_final.x) > Mathf.Abs(initial_to_final.z))
                    final_pos.x = cam.ScreenToWorldPoint(Input.mousePosition).x;
                //If the line should be vertical
                else final_pos.z = cam.ScreenToWorldPoint(Input.mousePosition).z;

            }
            else
            {
                //Set the final position to the position of the mouse
                final_pos = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            //Deal with the line renderer
            lineRenderer.enabled = true;
            lineRenderer.SetWidth(blockSize.x / 4, blockSize.x / 4);
            List<Vector3> points = new List<Vector3>();

            //First Case: Placing blocks in area
            if (Input.GetButton("AreaSelection"))
            {
                Vector3 p0 = initial_pos;
                Vector3 p1 = new Vector3(initial_pos.x, initial_pos.y, final_pos.z);
                Vector3 p2 = final_pos;
                Vector3 p3 = new Vector3(final_pos.x, initial_pos.y, initial_pos.z);
                Vector3 p4 = initial_pos;

                p0.y = terrain.transform.position.y + blockSize.y + 1;
                p1.y = p0.y;
                p2.y = p0.y;
                p3.y = p0.y;

                points.Add(p0);
                points.Add(p1);
                points.Add(p2);
                points.Add(p3);
                points.Add(p4);

                lineRenderer.SetVertexCount(5);

            }
            //Second Case: Placing blocks in line
            else
            {
                Vector3 p0 = initial_pos;
                Vector3 p1 = final_pos;

                p0.y = terrain.transform.position.y + blockSize.y + 1;
                p1.y = p0.y;

                points.Add(p0);
                points.Add(p1);

                lineRenderer.SetVertexCount(2);
            }

            lineRenderer.SetPositions(points.ToArray());
        }

        //Calls the function to place blocks in a line once the mouse is released
        //Also disables the line renderer once objects are placed
        if (Input.GetButtonUp("Fire1"))
        {
            //Place blocks in area
            initial_indexed_pos = initial_pos - bottomRightCorner.position;
            final_indexed_pos = final_pos - bottomRightCorner.position;

            if (Input.GetButton("AreaSelection"))
            {
                float rowLength = final_indexed_pos.x - initial_indexed_pos.x;
                float columnLength = final_indexed_pos.z - initial_indexed_pos.z;

                int iterations = Mathf.CeilToInt(Mathf.Abs(columnLength) / blockSize.z);
                float z_increment = blockSize.z * Mathf.Sign(columnLength);

                for (int i = 0; i < iterations; i++)
                {
                    Vector3 from_pos = initial_indexed_pos;
                    from_pos.z = initial_indexed_pos.z + (i * z_increment);
                    Vector3 to_pos = final_indexed_pos;
                    to_pos.z = from_pos.z;

                    placeBlockLine(from_pos, to_pos);
                }
            }
            //Place blocks in line
            else
            {
                placeBlockLine(initial_indexed_pos, final_indexed_pos);
            }

            Fire1_down = false;
            lineRenderer.enabled = false;
        }
    }

    void Mouse_Down()
    {
        //Sets the initial position when the mouse is first pressed
        if (Input.GetButtonDown("Fire1") && Fire1_down == false)
        {
            initial_pos = cam.ScreenToWorldPoint(Input.mousePosition);
            Fire1_down = true;
        }
    }

    void placeBlockLine(Vector3 initial_indexed_pos, Vector3 final_indexed_pos)
    {
        float increment_distance = blockSize.x;
        Vector3 increment_vector = final_indexed_pos - initial_indexed_pos;
        int blockTotal = Mathf.FloorToInt(Vector3.Magnitude(increment_vector) / increment_distance) + 1;
        increment_vector = Vector3.Normalize(increment_vector) * increment_distance;

        Vector3 curr_vector = initial_indexed_pos;

        for (int i = 0; i < blockTotal; i++)
        {
            placeBlockAtPoint(curr_vector);
            curr_vector += increment_vector;
        }
    }

    void placeBlockAtPoint(Vector3 indexed_pos)
    {
        int index_x = Mathf.FloorToInt(indexed_pos.x / blockSize.x);
        int index_z = Mathf.FloorToInt(indexed_pos.z / blockSize.z);

        //Deletes blocks
        if (mapOfInts[index_z][index_x] != 0 && currBlock == 0)
        {
            GameObject garbageBlock = mapOfObjects[index_z][index_x];
            Destroy(garbageBlock);

            mapOfObjects[index_z][index_x] = blocks[0];
            mapOfInts[index_z][index_x] = 0;
        }
        //Adds blocks
        else if (mapOfInts[index_z][index_x] != currBlock)
        {
            //Check if player has enough money
            BaseBlock baseScript = blocks[currBlock].GetComponent<BaseBlock>();
            if ((person.woodCarrying - baseScript.woodCost > 0) && (person.metalCarrying - baseScript.metalCost > 0))
            {
                Vector3 worldPos = blockSize.x * new Vector3(index_x, 0, index_z) + bottomRightCorner.position + new Vector3(blockSize.x / 2, 0, blockSize.z / 2);
                GameObject newBlock = (GameObject)Instantiate(blocks[currBlock], worldPos, Quaternion.identity);
                newBlock.transform.position += new Vector3(0, newBlock.transform.localScale.y / 2, 0);

                mapOfInts[index_z][index_x] = currBlock;
                mapOfObjects[index_z][index_x] = newBlock;

                person.woodCarrying -= baseScript.woodCost;
                person.metalCarrying -= baseScript.metalCost;
            }
        }
    }

    void initializeMaps()
    {
        mapOfInts = new List<List<int>>();
        mapOfObjects = new List<List<GameObject>>();
        for (int z = 0; z < z_size; z++)
        {
            mapOfInts.Add(new List<int>());
            mapOfObjects.Add(new List<GameObject>());

            for (int x = 0; x < x_size; x++)
            {
                mapOfInts[z].Add(0);
                mapOfObjects[z].Add(blocks[0]);
            }
        }
    }

}
