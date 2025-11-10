using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomCheck : MonoBehaviour, IDataPersistence
{
    private AudioManager _audioManager;
    public List<TeleportButton> listButton;
    public GameObject playerIcon1;
    public GameObject playerIcon2;
    public LayerMask layerMask;
    public string currentTag = null;
    private bool isOutdoor = false;

    private Tablet _tablet;

    private void Start()
    {
        _tablet = FindAnyObjectByType<Tablet>();
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void SaveData(GameData data)
    {
        foreach (TeleportButton item in listButton)
        {
            if (data.locationVisited.ContainsKey(item.id))
            {
                data.locationVisited.Remove(item.id);
            }
            data.locationVisited.Add(item.id, item.visited);
        }
    }

    public void LoadData(GameData data)
    {
        foreach (TeleportButton item in listButton)
        {
           
            data.locationVisited.TryGetValue(item.id, out item.visited);
            
        }           
    }

    private void Update()
    {
        RaycastHit hit;
        Ray checkRoom = new Ray(transform.position - new Vector3(0, 1, 0), Vector3.down);

        if (Physics.Raycast(checkRoom, out hit, 2, layerMask))
        {
            if (currentTag != hit.transform.gameObject.tag)
            {                
                currentTag = hit.transform.gameObject.tag;
                AmbientSound(hit.transform.gameObject.layer);
                CheckButton();
            }
        }
    }

    private void CheckButton()
    {
        foreach (TeleportButton button in listButton)
        {
            if(button.id == "Loket Tiket")
            {
                if (!button.visited)
                {
                    _tablet.locationVisited++;
                }
                button.visited = true;
            }
            button.gameObject.SetActive(true);
            if (button.id == currentTag)
            {
                if (!button.visited)
                {
                    _tablet.locationVisited++;
                }
                
                button.visited = true;                        
                playerIcon1.transform.position = button.transform.position;
                playerIcon2.transform.position = button.transform.position;
                if(button.id == "Pintu Masuk Istana")
                {
                    button.gameObject.SetActive(true);
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
                            
            }
        }
    }

    private void AmbientSound(int layer)
    {
        if (layer == LayerMask.NameToLayer("Indoor"))
        {
            _audioManager.Stop("OutsideAmbient");
            _audioManager.Play("InsideAmbient");
            isOutdoor = false;
        }
        else if(!isOutdoor && layer == LayerMask.NameToLayer("Outdoor"))
        {
            _audioManager.Play("OutsideAmbient");
            _audioManager.Stop("InsideAmbient");
            isOutdoor = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position - new Vector3(0, 1, 0), Vector3.down * 2);
    }
}
