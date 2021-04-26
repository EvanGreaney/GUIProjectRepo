using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    public GameObject[] Tetrominoes;

    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //when called, spawns another Tetromino to beign to fall
    public void NewTetromino()
    {
        Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
