// KHOGDEN 001115381
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class Level : MonoBehaviour
    {
        public static Level instance;
        [SerializeField] float levelGenerateSpeedRate = 0.01f;
        [SerializeField] Text seedInput;
        [SerializeField] GameObject killzoneHolders;

        [Header("Level Attributes")]
        [SerializeField] int width = 50;
        [SerializeField] int height = 50;
        private int seed;

        [Header("Tiles")]
        [SerializeField] GameObject floor;
        [SerializeField] GameObject playerSpawn;
        [SerializeField] GameObject wall;
        [SerializeField] GameObject softWall;
        [SerializeField] GameObject killzone;
        [SerializeField] Color[] playerSpawnColours = new Color[4];

        [Header("Decrease Level Size Settings")]
        private bool pauseTimer = true;
        [SerializeField] float replaceTileTime = 2f;
        private float replaceTileTimer;

        // KH - List of tiles in the level. Over time they'll be replaced with killzone tiles.
        private List<GameObject> tilesToReplace = new List<GameObject>();

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            instance = this;
        }

        // KH - Called upon the first frame.
        private void Start()
        {
            // KH - Plays menu music.
            MusicPlayer.instance.Play("Last Student Standing");
        }

        // Update is called once per frame
        void Update()
        {
            if (!pauseTimer)
            {
                // KH - Tick down the timer until it reaches zero.
                if (replaceTileTimer > 0f)
                    replaceTileTimer -= Time.deltaTime;
                else if (replaceTileTimer < 0f)
                    replaceTileTimer = 0f;

                // KH - Once at zero, replace a random tile with a killzone tile.
                if (replaceTileTimer == 0f)
                {
                    replaceTileTimer = replaceTileTime;
                    //ReplaceRandomTile();
                }
            }
        }

        // KH - Calls the 'IGenerateLevel()' IEnumerator.
        public void GenerateLevel()
        {
            StartCoroutine(IGenerateLevel());
        }

        // KH - Pick out a random tile from the replace tiles list to turn into a killzone tile.
        public void ReplaceRandomTile()
        {
            // KH - Destroy the tile getting replaced.
            GameObject tile = tilesToReplace[Random.Range(0, tilesToReplace.Count)];
            Vector2 pos = tile.transform.position;

            // KH - Replace with a killzone tile where the prior tile was.
            tilesToReplace.Remove(tile);
            Destroy(tile);
            Instantiate(killzone, pos, Quaternion.identity);
        }

        // KH - Generate the level and spawn players.
        IEnumerator IGenerateLevel()
        {
            int spawnIndex = 0;
            MusicPlayer.instance.Stop();

            // KH - Generate the level with the user's inputted seed.
            if (seedInput.text != "")
                seed = int.Parse(seedInput.text);
            else
            {
                int seedRange = 1000000;
                seed = Random.Range(-seedRange, seedRange);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    yield return new WaitForSeconds(levelGenerateSpeedRate);

                    Vector2 pos = new Vector2((width / 2f) - x - 0.5f, (height / 2f) - y - 0.5f);
                    if (x == 1 && y == 1 || x == width - 2 && y == 1 || x == 1 && y == height - 2 || x == width - 2 && y == height - 2)
                    {
                        // KH - Instantiate player spawn tiles.
                        GameObject t = Instantiate(playerSpawn, pos, Quaternion.identity, transform);
                        t.GetComponent<SpriteRenderer>().color = playerSpawnColours[spawnIndex];
                        t.GetComponent<Spawn>().PlayerIndex = spawnIndex + 1;

                        spawnIndex++;
                    }
                    else if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                    {
                        // KH - Instantiate wall tiles surrounding the outside of the level
                        GameObject t = Instantiate(wall, pos, Quaternion.identity, transform);
                        tilesToReplace.Add(t);
                    }
                    else
                    {
                        // KH - Instantiate floor tile.
                        GameObject t = Instantiate(floor, pos, Quaternion.identity, transform);
                        tilesToReplace.Add(t);

                        // KH - Use perlin noise and seed to decide if a soft wall goes on top of this floor.
                        float p = Mathf.PerlinNoise(x * seed + 0.5f, y * seed + 0.5f);
                        if (p > 0.5f)
                            Instantiate(softWall, pos, Quaternion.identity, transform);
                    }
                }
            }

            // KH - Spawn all players once the level is generated.
            Spawn[] spawns = FindObjectsOfType<Spawn>();
            for(int i = 0; i < spawns.Length; i++)
            {
                // Spawn a playable character for each player playing.
                if (GameManager.instance.GetPlayer(spawns[i].PlayerIndex - 1).InputDevice != Controllers.PlayerController.InputDevice.none)
                    spawns[i].SpawnPlayer();
            }

            // KH - Begin the process of progressively replacing tiles with killzone tiles.
            pauseTimer = false;
            killzoneHolders.SetActive(true);
        }
    }
}