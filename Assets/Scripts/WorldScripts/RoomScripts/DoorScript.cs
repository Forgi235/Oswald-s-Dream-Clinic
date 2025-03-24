using System;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Transform TravelPoint;
    [SerializeField] DoorScript ConnectedDoor;
    [SerializeField] BoxCollider2D DoorBlockerCollider;

    public event Action DoorEntered;

    public event Action LeftThisRoom;
    public event Action EnteredThisRoom;

    [SerializeField] private EnemyScript LinkedEnemy;
    private bool LinkedEnemyAlive = false;
    private bool LastLockDoorCommand = false;

    private void Start()
    {
        ConnectedDoor.DoorEntered += RoomEntered;
        if (LinkedEnemy != null )
        {
            LinkedEnemyAlive = true;
            LinkedEnemy.EnemyDied += LinkedEnemyDied;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnDoorEnter();
        }
    }
    public void LockDoor(bool Lock)
    {
        LastLockDoorCommand = Lock;
        if (LinkedEnemyAlive) return;
        DoorBlockerCollider.enabled = Lock;
    }

    private void RoomEntered()
    {
        EnteredThisRoom?.Invoke();
        PlayerMovement.PM.transform.position = TravelPoint.position;
    }
    public Vector3 GetTravelPoint()
    {
        return TravelPoint.position;
    }
    private void OnDoorEnter()
    {
        DoorEntered?.Invoke();
        LeftThisRoom?.Invoke();
    }
    private void LinkedEnemyDied()
    {
        LinkedEnemyAlive = false;
        LockDoor(LastLockDoorCommand);
    }
}
