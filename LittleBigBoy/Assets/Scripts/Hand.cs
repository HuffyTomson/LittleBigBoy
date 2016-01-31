using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    public AnimationCurve curve;
    IEnumerator Start()
    {
        float t = 0;
        while (t < 1.0f)
        {
            yield return null;
            t += Time.deltaTime * 2.5f;
            transform.position += new Vector3(0, curve.Evaluate(t) * 10 * Time.deltaTime, 0);
        }

        Destroy(this.gameObject);
    }
}
