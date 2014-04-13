using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
#if UNITY_EDITOR
    public bool QWERTY;
#endif

    public float Speed = 70f;
    public float ZoomSpeed = 150f;

    private Transform _target;
    public Unit Target
    {
        set
        {
            if (value == null)
            {
                _target = null;
            }
            else
            {
                _target = value.transform;
            }
        }
    }

	private void Update ()
	{
        if (_target != null)
        {
            if (transform.position != _target.position)
            {
                float distance = Vector3.Distance(_target.position, transform.position);
                float interpolation = 1 - Mathf.Exp(-distance * Time.deltaTime * 0.5f - 0.1f);
                transform.position = Vector3.MoveTowards(transform.position, _target.position, interpolation * 10f);
            }
            else
            {
                Camera.main.transform.LookAt(_target.transform);
                Target = null;
            }
        }
        else
        {
            if ((QWERTY && Input.GetKey(KeyCode.Q)) || (!QWERTY && Input.GetKey(KeyCode.A)))
                transform.Rotate(0f, Mathf.PI / 2f, 0f);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(0f, -Mathf.PI / 2f, 0f);
            if ((QWERTY && Input.GetKey(KeyCode.A)) || (!QWERTY && Input.GetKey(KeyCode.Q)))
                transform.position = transform.position + transform.right * Time.deltaTime * Speed;
            if (Input.GetKey(KeyCode.D))
                transform.position = transform.position - transform.right * Time.deltaTime * Speed;
            if (Input.GetKey(KeyCode.S))
                transform.position = transform.position + transform.forward * Time.deltaTime * Speed;
            if ((QWERTY && Input.GetKey(KeyCode.W)) || (!QWERTY && Input.GetKey(KeyCode.Z)))
                transform.position = transform.position - transform.forward * Time.deltaTime * Speed;

            if (Input.GetKeyDown(KeyCode.Tab))
                Target = Selector.Selected;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Vector3 To = Camera.main.transform.position + Camera.main.transform.forward * ZoomSpeed * scroll;
                iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", To, "easetype", iTween.EaseType.easeOutQuad, "time", 0.3f));
            }
        }
	}
}