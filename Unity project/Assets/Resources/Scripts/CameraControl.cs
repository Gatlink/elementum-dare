using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
#if UNITY_EDITOR
    public bool QWERTY;
#endif

    public float speed = 70.0f;

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
                var distance = Vector3.Distance(_target.position, transform.position);
                var interpolation = 1 - Mathf.Exp(-distance * Time.deltaTime * 0.5f - 0.1f);
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
                transform.position = transform.position + transform.right * Time.deltaTime * speed;
            if (Input.GetKey(KeyCode.D))
                transform.position = transform.position - transform.right * Time.deltaTime * speed;
            if (Input.GetKey(KeyCode.S))
                transform.position = transform.position + transform.forward * Time.deltaTime * speed;
            if ((QWERTY && Input.GetKey(KeyCode.W)) || (!QWERTY && Input.GetKey(KeyCode.Z)))
                transform.position = transform.position - transform.forward * Time.deltaTime * speed;

            if (Input.GetKeyDown(KeyCode.Tab))
                Target = Selector.Selected;
        }
	}
}