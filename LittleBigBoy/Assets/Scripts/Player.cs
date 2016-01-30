using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Transform parrent;
    Vector3 downPosition;

    bool mouseDown = false;
    Vector3 lastMousePos = Vector3.zero;
    Vector3 rotationDelta = Vector3.zero;
    public Vector3 rotationAmt = Vector3.zero;

    enum Tools
    {
        HAND,
        FOOD,
    }

    Tools selectedTool;

	void Update ()
    {
        if (Input.GetKeyDown("1"))
            selectedTool = Tools.HAND;
        if (Input.GetKeyDown("2"))
            selectedTool = Tools.FOOD;


        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
            downPosition = Input.mousePosition;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            lastMousePos = Input.mousePosition;

            Vector3 mouseDelta = Input.mousePosition -downPosition;
            //Debug.Log(mouseDelta);
            if(mouseDelta.magnitude < 10)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit))
                {
                    switch(selectedTool)
                    {
                        case Tools.HAND:
                            CuboidManager.Instance.KickFish(hit.point, 15.0f);
                            break;
                        case Tools.FOOD:
                            CuboidManager.Instance.FeedFish(hit.point);
                            break;
                    }
                    CuboidManager.Instance.KickFish(hit.point, 15.0f);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (mouseDown)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
            rotationDelta += mouseDelta * 0.0125f;
        }

        rotationDelta = new Vector3(rotationDelta.x * 0.90f,
                                    rotationDelta.y * 0.90f,
                                    rotationDelta.z * 0.90f);

        rotationAmt += new Vector3(  0, rotationDelta.x, 0);

        parrent.rotation = Quaternion.Lerp(parrent.rotation, Quaternion.Euler(rotationAmt), Time.deltaTime * 4);
    }
}
