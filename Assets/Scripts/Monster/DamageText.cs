using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    TextMeshPro text;
    public Color color = Color.red;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // �ؽ�Ʈ ��ġ
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * alphaSpeed); // �ؽ�Ʈ ���İ�
        text.color = color;
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
