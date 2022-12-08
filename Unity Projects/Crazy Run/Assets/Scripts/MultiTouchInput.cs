using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchInput : MonoBehaviour
{
    [Header("Infelicty of MultiTouch in procents (decimal number)")]
    [SerializeField] private float procentsOfAllowedDistanceIncrease;

    //Переменные, получающие в себя данные о двух прикосновениях для мультижеста.
    private Touch touch1;
    private Touch touch2;
    //Две переменные, получающие в себя координаты текущих позиций нахождения пальцев на экране.
    private Vector2 touch1Position;
    private Vector2 touch2Position;
    //Переменная, получающее в себя число, обозначающее стартовое расстояние между двумя точками прикосновений.
    private float startTouchesDistance;
    //Переменная, получающая в себя число, обозначающее текущее расстояние между двумя точками прикосновений.
    private float currentTouchesDistance;
    //Переменная, получающее в себя число, обозначающее минимальное число, на которое должно измениться положение пальцев для фиксации жеста увеличения. 
    private float minDistanceIncrease;
    //Переменные, обозначающие направление движения пальцев при жесте увеличения.
    private float touch1XDirection;
    private float touch1YDirection;
    private float touch2XDirection;
    private float touch2YDirection;

    private void Update()
    {
        //Получаем 2 прикосновения на экране для мультижеста.
        if (Input.touchCount == 2)
        {
            //Передаем полученные касания и их позиции в переменные.
            touch1 = Input.GetTouch(0);
            touch2 = Input.GetTouch(1); 
            touch1Position = touch1.position;
            touch2Position = touch2.position;
            //Выполняем метод, определяющий направление движения пальцев.
            DetectTouchesDirections();
            //Определяем текущее расстояние между точками двух прикосновений.
            currentTouchesDistance = Vector2.Distance(touch1Position, touch2Position);
            //Проверяем, начато ли прикосновение.
            if (touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                //Записываем в переменную расстояние, на котором находятся точки прикосновений при начале мультижеста.
                startTouchesDistance = Vector2.Distance(touch1Position, touch2Position);
                //Записываем в переменную значение, обозначающее на какое число должно увеличиться расстояние между точками от первоначального, чтобы зафиксировался мультижест.
                minDistanceIncrease = startTouchesDistance * procentsOfAllowedDistanceIncrease;
            }
            //Проверяем, двигаются ли прикосновения, больше ли текущее расстояние между ними минимальной погрешности, разнонаправлены ли движения пальцев.
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved && currentTouchesDistance > startTouchesDistance + minDistanceIncrease && touch1XDirection != touch2XDirection && touch1YDirection != touch2YDirection)
            {
                //Выводим в консоль сообщение о том, что жест зафиксирован.
                Debug.Log("Жест увеличение");
            }
        }
    }

    //Метод, определяющий направление движения пальца. Его суть в том, что при делении изменения позиции по той или иной оси, на модуль самого себя, мы получаем число 1 или -1, обозначающее направление движения пальцев при мультижесте.
    private void DetectTouchesDirections()
    {
        touch1XDirection = touch1.deltaPosition.x / Mathf.Abs(touch1.deltaPosition.x);
        touch1YDirection = touch1.deltaPosition.y / Mathf.Abs(touch1.deltaPosition.y);
        touch2XDirection = touch2.deltaPosition.x / Mathf.Abs(touch2.deltaPosition.x);
        touch2YDirection = touch2.deltaPosition.y / Mathf.Abs(touch2.deltaPosition.y);
    }
}
