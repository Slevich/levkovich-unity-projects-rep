using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTouchInput : MonoBehaviour
{
    //Переменная, обозначающая минимальную длину свайпа в 100 пикселей.
    private float minSwipeLenght = 100f;
    //Переменная, обозначающая максимальное смещение по пиксилям по Y относительно начальной точки свайпа.
    private float maxYAxisOffset = 50f;
    //Точка Vector2 с координатами начала свайпа.
    private Vector2 swipeStartPosition;
    //Текущая точка, где находится палец на экране.
    private Vector2 swipeCurrentPosition;
    //Переменная, с текущим прикосновением на экране.
    private Touch swipeTouch;
    //Булевая переменная, обозначающая находится ли палец в допустимых пределах по Y координатам относительно начала точки свайпа.
    private bool yOffsetInBorder = true;
    //Булевая переменная, обозначающая, что свайп зафиксирован системой.
    private bool swipeDetected;
    //Булевая переменная, обозначающая двигается ли палец вправо или нет.
    private bool movingRight;

    void Update()
    {
        //Определяем, имеются ли прикосновения на экране.
        if (Input.touchCount == 1)
        {
            //Присваиваем переменной текущеен прикосновение, а другой - его текущую позицию.
            swipeTouch = Input.GetTouch(0);
            swipeCurrentPosition = swipeTouch.position;
            //Метод, фикирующий стартовую позицию свайпа в самом начале прикосновения.
            DetectSwipeStartPosition();
            //Метод, определяющий, находится ли в рамках значений по Y-оси палец на экране при свайпе.
            DetectYAxisOffset();

            //Делаем проверку, двигается ли палец по оси X направо.
            if (swipeTouch.deltaPosition.x > 0)
            {
                //Если двигается, булевая переменная обозначается true.
                movingRight = true;

                //Делаем проверку, длина свайпа больше минимально необходимого значения по оси X, а также не вышел ли он за границу по оси Y и не зафиксирован ли он.
                if (swipeCurrentPosition.x - swipeStartPosition.x >= minSwipeLenght && yOffsetInBorder && swipeDetected == false)
                {
                    //Выводим в консоль, что свайп зафиксирован.
                    Debug.Log("Свайп вправо");
                    //Переменная получает фиксацию свайпа.
                    swipeDetected = true;
                }
            }
            //Делаем проверку, двигается ли палец по оси X в противоположной направлении (влево) или стоит на месте.
            else if (swipeTouch.deltaPosition.x <= 0)
            {
                //В этом случае свайп вправо не фиксируется и движения направо не происходит.
                swipeDetected = false;
                movingRight = false;
            }
        }
    }

    //Метод, фиксирующий стартовое положение свайпа.
    private void DetectSwipeStartPosition()
    {
        //Если начато прикосновение, стартовая позиция фиксируется. Если же палец не двигается вправо (двигается влево / стоит на месте), фиксируется стартовая точка следующего движения пальца.
        if (swipeTouch.phase == TouchPhase.Began)
        {
            swipeStartPosition = swipeTouch.position;
        }
        else if (movingRight == false)
        {
            swipeStartPosition = swipeTouch.position;
        }
    }

    //Метод, фиксирующий смещение пальца по оси Y при свайпе вправо в определенных пределах.
    private void DetectYAxisOffset()
    {
        //Если модуль длины вектора движения пальца по оси Y больше чем допустимое значение, то палец вышел за границу и свайп не фиксируется. Если же меньше или равен, то палец не вышел за пределы.
        if (Mathf.Abs(swipeStartPosition.y - swipeCurrentPosition.y) > maxYAxisOffset)
        {
            yOffsetInBorder = false;
        }
        else if (Mathf.Abs(swipeStartPosition.y - swipeCurrentPosition.y) <= maxYAxisOffset)
        {
            yOffsetInBorder = true;
        }
    }
}
