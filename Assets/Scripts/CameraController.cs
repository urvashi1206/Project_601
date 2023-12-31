using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    GameObject characterObj;

    private List<GameObject> hitObjs;

    // Start is called before the first frame update
    void Start()
    {
        hitObjs = new();
        SceneManager.sceneLoaded += OnLevelLoaded;
        enabled = false; // don't update until level is loaded
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> objsThisFrame = new();

        // Camera is orthographic, not perspective; need to raycast to the player from the point 
        // on the camera's near clip plane that's perpendicular to the camera's right axis.
        Ray r = new(characterObj.transform.position, -transform.forward);
        float sphereCastRadius = 0.5f;
        if (new Plane(transform.forward, transform.position).Raycast(r, out float dist))
        {
            RaycastHit[] hits = Physics.SphereCastAll(r.GetPoint(dist), sphereCastRadius, transform.forward, dist - sphereCastRadius);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent(out GridComponent c))
                {
                    objsThisFrame.Add(hit.transform.gameObject);

                    // if the hit wasn't in the existing list (meaning it's a new hit)
                    if (!hitObjs.Remove(hit.transform.gameObject))
                        c.SetFade(0.2f);
                }
            }
        }

        // All remaining objs in list were not hit; set them back to opaque material
        foreach (GameObject obj in hitObjs)
            obj.GetComponent<GridComponent>().SetFade(1f);

        hitObjs = objsThisFrame;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        hitObjs.Clear();
        characterObj = GameObject.FindWithTag("Player");
        enabled = true;
    }
}
