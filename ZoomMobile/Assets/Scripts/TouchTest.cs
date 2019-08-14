using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    [SerializeField] float velZoomOrthographic;
    [SerializeField] float velZoomPerspective;
    [SerializeField] int frequencia;

    Camera camera;
    Touch[] toques;
    Touch toque;
    float tempo;
    int delta, aux;

    // Start is called before the first frame update
    void Start()
    {
        tempo = 0;
        delta = 0;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        tempo += Time.deltaTime;
        aux = Mathf.FloorToInt(tempo);
        if (aux > delta)
        {
            delta = aux + frequencia;
            toques = Input.touches;

            foreach(Touch t in toques)
            {
                Debug.Log("ID: " + t.fingerId + " (" + t.position.x + ", " + t.position.y + ")");
            }
        }
        
        // DETECCAO DE MOVIMENTO DE PINCA
        if (Input.touchCount == 2)
        {
            Zoom();
        }
    }

    void Zoom()
    {
        Touch toqueZero = Input.GetTouch(0);
        Touch toqueUm = Input.GetTouch(1);

        Vector2 PosAntToqueZero = toqueZero.position - toqueZero.deltaPosition;
        Vector2 PosAntToqueUm = toqueUm.position - toqueUm.deltaPosition;

        float DistAnterior = (PosAntToqueZero - PosAntToqueUm).magnitude;
        float DistAgora = (toqueZero.position - toqueUm.position).magnitude;

        float DeltaDistancia = DistAnterior - DistAgora;

        if (camera.orthographic)
        {
            camera.orthographicSize += DeltaDistancia * velZoomOrthographic;
            camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            camera.orthographicSize = Mathf.Min(camera.orthographicSize, 10f);
        }
        else
        {
            camera.fieldOfView += DeltaDistancia * velZoomPerspective;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);
        }
    }
}
