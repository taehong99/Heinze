//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class MouseManager : MonoBehaviour
//{
//    [SerializeField] PlayerAttack 
//    void Update()
//    {
//        // 마우스 왼쪽 버튼을 클릭했을 때
//        if (Input.GetMouseButtonDown(0))
//        {
//            // UI를 클릭한 경우에는 공격을 실행하지 않음
//            if (!EventSystem.current.IsPointerOverGameObject())
//            {
//                // UI를 클릭하지 않은 경우에만 공격 실행
//                player.Attack();
//            }
//        }
//    }
//}
//
