using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class RampaBehaviour : MonoBehaviour
{
	public float vel = 0.1f;
	public Renderer quad;
	bool canMove = false;

    [SerializeField] Material[] material;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }


    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += StartMoving;
        GameplayEvents.Win += WinMovement;
        GameplayEvents.GameOver += StopMoving;
        GameplayEvents.ReachPalace += StopMoving;

        UtilityEvents.GamePause += StopMoving;
        UtilityEvents.GameResume += StartMoving;

        //TrainingEvents.RewardLoss += RewardLoss; 
        //TrainingEvents.GetStar += GetStar;
        //TrainingEvents.RewardWin += RewardWin;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= StartMoving;      
        GameplayEvents.Win += WinMovement;
        GameplayEvents.GameOver -= StopMoving;
        GameplayEvents.ReachPalace -= StopMoving;

        UtilityEvents.GamePause -= StopMoving;
        UtilityEvents.GameResume -= StartMoving;

        //TrainingEvents.RewardLoss -= RewardLoss;
        //TrainingEvents.GetStar -= GetStar;
        //TrainingEvents.RewardWin -= RewardWin;
    }

    void StartMoving()
    {
        canMove = true;
    }

    void StopMoving()
    {
        canMove = false;
    }

    void WinMovement()
    {
        vel *= 2f;
    }

    void Update()
	{
        if (canMove)
        {
            Vector2 offset = new Vector2(0, vel * Time.deltaTime);
            quad.material.mainTextureOffset += offset;
        }		
	}

    public void RewardLoss()
    {
        mesh.material = material[1];

        StartCoroutine(SetMeshBack());
    }

    public void RewardWin()
    {
        mesh.material = material[2];

        StartCoroutine(SetMeshBack());
    }

    public void GetStar()
    {
        mesh.material = material[3];

        StartCoroutine(SetMeshBack());
    }

    IEnumerator SetMeshBack()
    {
        yield return new WaitForSeconds(0.4f);

        mesh.material = material[0];
    }
}