using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSpawnpoint : MonoBehaviour
{
    [SerializeField] GameObject graphics;

    private void Awake()
    {
        graphics.SetActive(false);
        //��������Ʈ ��ġ���� ����� ĸ���̶� ť��� �����
    }
}