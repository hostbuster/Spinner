using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public GameObject floortile;
	// grid
	private static int grid_size = 100;
	private static int grid_height = 5;
	private int[,,] grid = new int[grid_height, grid_size, grid_size];
	private float offset = (float)(grid_size) / 2;
	// movement
	private Vector3 velocity = new Vector3(0, 0, 0);
	private float thurst = 20.0f;
	private float damping = 0.95f;
	private float gravity = 9.8f;
	// Colors
	private Color[] colors = new Color[grid_height];
	// Raycasting
	// private float distance = 5.0f;

	void InitializeGridLevel(int level, int probability) {
		for(int z = 0; z < grid_size; z++)
		{
			for(int x = 0; x < grid_size; x++)
			{
				int show = Random.Range(0,probability);
				if (show == 0) {
					if (level > 0) {
						if (grid [level - 1, x, z] == 1) {
							grid [level, x, z] = 1;
						}
					} else {
						grid [level, x, z] = 1;
					}
				}
			}
		}
	}

	// Put the prefab object in scene
	void CreateGridObjects() {
		IntitalizeColors ();

		for(int y = 0; y < grid_height; y++)
		{
			for (int z = 0; z < grid_size; z++) {
				for (int x = 0; x < grid_size; x++) {
					if (grid [y, x, z] == 1) {
						GameObject temp = (GameObject)Instantiate (floortile, new Vector3 (x-offset, y, z-offset), Quaternion.identity);
						temp.GetComponent<Renderer>().material.color = colors [y];
					}
				}
			}
		}
	}

	void IntitalizeColors() {
		for(int y = 0; y < grid_height; y++)
		{
			colors [y].g = (1.0f / grid_height) * (y+1);
		}
	}

	void InitializeGrid () {
		for(int y = 0; y < grid_height; y++)
		{
			InitializeGridLevel (y, 2+(y*2));
		}
	}

	void InitializeGridSimple () {
		for(int z = -grid_size; z <=grid_size; z++)
		{
			for(int x = -grid_size; x <=grid_size; x++)
			{
				int show = Random.Range(0,2);
				if (show == 0) {
					Instantiate (floortile, new Vector3 (x, 0, z), Quaternion.identity);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		InitializeGrid();
		CreateGridObjects ();
	}
	


	void Update()
	{
		if (Input.GetKey (KeyCode.RightShift)) {
			velocity+=new Vector3(0,thurst * Time.deltaTime,0);
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			velocity+=new Vector3(thurst * Time.deltaTime,0,0);
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			velocity+=new Vector3(-thurst * Time.deltaTime,0,0);
			// Debug.Log (velocity);
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			velocity+=new Vector3(0,0,-thurst * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			velocity+=new Vector3(0,0,thurst * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}

		// Update Velocity and Position
		velocity.y -= gravity * Time.deltaTime;
		velocity.x *= damping;
		velocity.z *= damping;
		transform.position += velocity * Time.deltaTime;
		float ty = GetTerrainHeight();
		if (transform.position.y < ty) {
			transform.position = new Vector3(transform.position.x, ty, transform.position.z);
			velocity.y = 0.0f;
		}
		/*
		if (velocity.magnitude > 0.5f) {
				
		} else {
			velocity = new Vector3 (0, 0, 0);
		}
		*/
		// GetTerrainHeight();

	}

	private float GetTerrainHeight() {
		int x = Mathf.FloorToInt (transform.position.x+offset);
		int z = Mathf.FloorToInt (transform.position.z+offset);
		for(int y = grid_height-1; y > -1; y--)
		{
			// Debug.Log (string.Format("{0} {1} {2}",x,z,y));
			if (grid [y, x, z] == 1) {
				return (float)y+1;
			}
		}
		return 0.0f;
		// Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance);
	}

}
