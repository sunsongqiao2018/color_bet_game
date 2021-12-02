using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsPool : MonoBehaviour
{
    public static ChipsPool Instance;
    public int poolSize = 10;
    public List<GameObject> pooledChips;
    private Stack<GameObject> activeChips;
    [SerializeField] GameObject chip;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        activeChips = new Stack<GameObject>();
        pooledChips = new List<GameObject>();
        StockPool(poolSize);
    }
    private void StockPool(int size)
    {
        GameObject tmp;
        for (int i = 0; i < size; i++)
        {
            tmp = Instantiate(chip);
            tmp.SetActive(false);
            pooledChips.Add(tmp);
            tmp.transform.SetParent(transform);
        }
    }
    public void GetChip()
    {
        if (activeChips.Count >= pooledChips.Count) StockPool(poolSize / 2);
        GameObject chip = pooledChips[activeChips.Count];
        chip.transform.position = new Vector3(Random.Range(-.6f, .6f), transform.position.y, transform.position.z + Random.Range(-.2f, .2f));
        chip.SetActive(true);
        activeChips.Push(chip);
    }

    public void RemoveChip()
    {
        GameObject chip = activeChips.Pop();
        chip.SetActive(false);
    }

    public void RemoveAllChips()
    {
        while (activeChips.Count > 0)
        {
            RemoveChip();
        }
    }
    public void SetChips(int amount)
    {
        RemoveAllChips();
        for (int i = 0; i < amount; i++)
        {
            GetChip();
        }
    }
}
