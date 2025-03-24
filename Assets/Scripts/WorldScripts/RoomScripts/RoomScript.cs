using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class RoomScript : MonoBehaviour
{
    [SerializeField] CinemachineCamera RoomCamera;
    CinemachinePositionComposer CameraPositionComposer;

    EnemyScript[] Enemies;

    DoorScript[] Doors;

    UnloadableObstacle[] Obstacles;

    private IEnumerator CameraSnapCoroutine;

    bool ActiveRoom = false;
    
    private void Awake()
    {
        CameraPositionComposer = RoomCamera.GetComponent<CinemachinePositionComposer>();
    }
    void Start()
    {
        LoadGameObjectFolders();
        if (Doors != null)
        {
            foreach (DoorScript door in Doors)
            {
                door.EnteredThisRoom += RoomEnter;
                door.LeftThisRoom += RoomExit;
            }
        }
        if (!ActiveRoom)
        {
            DeactivateEnemies();
            foreach(UnloadableObstacle obstacle in Obstacles)
            {
                obstacle.gameObject.SetActive(false);
            }
        }
        CheckRoomBlockers();
    }
    private void DeactivateEnemies()
    {
        foreach (EnemyScript enemy in Enemies)
        {
            enemy.EnemyDied += CheckRoomBlockers;
            enemy.DeactivateEnemy();
        }
    }
    private void CheckRoomBlockers()
    {
        bool _lock = false;
        foreach(EnemyScript enemy in Enemies)
        {
            if (enemy != null)
            {
                if (enemy.isBlockingRoom())
                {
                    _lock = true;
                    break;
                }
            }
        }
        LockRoom(_lock);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActiveRoom = true;
            LoadStuff();
            RoomCamera.gameObject.SetActive(true);
        }
    }
    private void ActivateEnemies()
    {
        foreach (EnemyScript enemy in Enemies)
        {
            if (enemy != null)
            {
                //I activate the enemy first so the enemy can "realize" if its dead or not and either remain active or disable itself
                enemy.gameObject.SetActive(true);
                enemy.ActivateEnemy();
            }
        }
    }
    private void ActivateObstacles()
    {
        foreach(UnloadableObstacle obstacle in Obstacles)
        {
            obstacle.gameObject.SetActive(true);
            obstacle.ResetPosition();
        }
    }
    private void LoadGameObjectFolders()
    {
        Enemies = GetComponentsInChildren<EnemyScript>();
        Doors = GetComponentsInChildren<DoorScript>();
        Obstacles = GetComponentsInChildren<UnloadableObstacle>();
    }
    private void RoomEnter()
    {
        CancelInvoke(nameof(UnloadStuff));
        // there was "LockRoom(true);" here but i was redundant due to next line, I put a comment here in case I forgot :P
        CheckRoomBlockers();
        LoadStuff();
        ReloadEnemies();
        if(CameraSnapCoroutine != null) StopCoroutine(CameraSnapCoroutine);
        CameraSnapCoroutine = CameraSnap();
        StartCoroutine(CameraSnapCoroutine);
    }
    private void ReloadEnemies()
    {
        foreach(EnemyScript enemy in Enemies)
        {
            enemy.OnRoomEnter();
        }
    }
    private void RoomExit()
    {
        RoomCamera.gameObject.SetActive(false);
        Invoke(nameof(UnloadStuff), 0.5f);    
    }
    private void LoadStuff()
    {
        ActivateEnemies();
        ActivateObstacles();
    }
    private void UnloadStuff()
    {
        foreach(EnemyScript enemy in Enemies)
        {
            enemy.OnRoomExit();
            enemy.gameObject.SetActive(false);
        }
        foreach(UnloadableObstacle obstacle in Obstacles)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
    private void LockRoom(bool Lock)
    {
        foreach(DoorScript door in Doors)
        {
            door.LockDoor(Lock);
        }
    }
    private IEnumerator CameraSnap()
    {
        float a = CameraPositionComposer.Damping.x;
        float b = CameraPositionComposer.Damping.y;
        CameraPositionComposer.Damping.x = 0f;
        CameraPositionComposer.Damping.y = 0f;
        RoomCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CameraPositionComposer.Damping.x = a;
        CameraPositionComposer.Damping.y = b;
    }
}
