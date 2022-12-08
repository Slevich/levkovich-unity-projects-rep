using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [Header("Player movement component")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Canvas with all inteface on level")]
    [SerializeField] private LevelHUD levelCanvas;

    //Минимальная длина свайпа.
    private float minSwipeLenght = 100f;
    //Максимальное смещение пальца относительно оси свайпа.
    private float maxAxisOffset = 100f;
    //Стартовая позиция пальца на экране.
    private Vector2 swipeStartPosition;
    //Текущая позиция пальца на экране.
    private Vector2 swipeCurrentPosition;
    //Прикосновение пальца на экране.
    private Touch swipeTouch;
    //Булевая переменная, обозначающая сделан ли свайп или нет.
    private bool swipeDetected;
    //Булевая переменная, обозначающая доступен ли двойной тап.
    private bool doubleTapEnable;
    //Таймер, обозначающий временной промежуток между двумя тапами в двойном тапе.
    private float doubleTapTimer = 0.20f;
    //Таймер между тапами для обнуления.
    private float currentDoubleTapTimer;

    //Присваиваем таймеру для обнуления значение самого таймера на Старте.
    private void Start()
    {
        currentDoubleTapTimer = doubleTapTimer;
    }

    void Update()
    {
        //Если на экране один палец - делаем следующее...
        if (Input.touchCount == 1)
        {
            //Получаем прикосновение, передаем в переменную и считываем текущую позицию пальца на экране.
            swipeTouch = Input.GetTouch(0);
            swipeCurrentPosition = swipeTouch.position;
            //Debug.Log(swipeTouch.position.x + " " + swipeTouch.position.y);
            //Вызываем метод, определяющий стартовую позицию пальца на экране.
            DetectSwipeStartPosition();

            //Под условиями по какой оси и в какой направлении изменяется позиция пальца, и не сместился ли палец относительно оси свайпа больше допустимого значения, вызываем соответствующий метод свайпа.
            if (swipeTouch.deltaPosition.x > 0 && Mathf.Abs(swipeStartPosition.y - swipeCurrentPosition.y) <= maxAxisOffset) SwipeRight();
            else if (swipeTouch.deltaPosition.x < 0 && Mathf.Abs(swipeStartPosition.y - swipeCurrentPosition.y) <= maxAxisOffset) SwipeLeft();
            else if (swipeTouch.deltaPosition.y > 0 && Mathf.Abs(swipeStartPosition.x - swipeCurrentPosition.x) <= maxAxisOffset) SwipeUp();
            else if (swipeTouch.deltaPosition.y < 0 && Mathf.Abs(swipeStartPosition.x - swipeCurrentPosition.x) <= maxAxisOffset) SwipeDown();

            //Проверяем доступен ли двойной свайп и находится ли игрок в воздухе - вызываем метод прыжка.
            if (doubleTapEnable && playerMovement.isInAir == false) playerMovement.Jump();

            //Если касание окончено, обнуляем таймер для двойного тапа.
            if (swipeTouch.phase == TouchPhase.Ended) doubleTapTimer = currentDoubleTapTimer;

        }
        //Если на экране нет касаний - запускаем следующее...
        else if (Input.touchCount == 0)
        {
            //Запускаем таймер между касаниями, чтобы понять - можно ли получить двойной тап.
            doubleTapTimer -= Time.deltaTime;

            //Если таймер вышел (слишком большой разрыв между тапами), двойной тап не регистрируется.
            if (doubleTapTimer <= 0)
            {
               doubleTapEnable = false;
            }
            //Если таймер больше нуля и при этом игра не на паузе - двойной тап доступен, если на паузе - нет.
            else if (doubleTapTimer > 0)
            {
                if (levelCanvas.gameOnPause)
                {
                    doubleTapEnable = false;
                }
                else if (levelCanvas.gameOnPause == false)
                {
                    doubleTapEnable = true;
                }
            }
        }
    }

    //Метод, фиксирующий зафиксирован ли свайп вправо.
    private void SwipeRight()
    {
        //Если длина свайпа больше минимально необходимой и при этом свайпе еще не был задетекчен, то...
        if (swipeCurrentPosition.x - swipeStartPosition.x >= minSwipeLenght && swipeDetected == false)
        {
            //Вызываем метод остановки игрока, чтобы в движении резко сменить направление. Булевую переменную, отвечающую за движение вправо делаем истинной. А также детектим свайп.
            Debug.Log("Свайп вправо");
            playerMovement.StopPlayer();
            playerMovement.movingRight = true;
            swipeDetected = true;
        }
    }

    //Метод, фиксирующий зафиксирован ли свайп влево.
    private void SwipeLeft()
    {
        //Если длина свайпа больше минимально необходимой и при этом свайпе еще не был задетекчен, то...
        if (swipeStartPosition.x - swipeCurrentPosition.x >= minSwipeLenght && swipeDetected == false)
        {
            //Вызываем метод остановки игрока, чтобы в движении резко сменить направление. Булевую переменную, отвечающую за движение вправо делаем истинной. А также детектим свайп.
            Debug.Log("Свайп влево");
            playerMovement.StopPlayer();
            playerMovement.movingLeft = true;
            swipeDetected = true;
        }
    }

    //Метод, фиксирующий зафиксирован ли свайп вверх.
    private void SwipeUp()
    {
        //Если длина свайпа больше минимально необходимой и при этом свайпе еще не был задетекчен, то...
        if (swipeCurrentPosition.y - swipeStartPosition.y >= minSwipeLenght && swipeDetected == false)
        {
            //Вызываем метод остановки игрока, чтобы в движении резко сменить направление. Булевую переменную, отвечающую за движение вправо делаем истинной. А также детектим свайп.
            Debug.Log("Свайп вверх");
            playerMovement.StopPlayer();
            playerMovement.movingUp = true;
            swipeDetected = true;
        }
    }

    //Метод, фиксирующий зафиксирован ли свайп вниз.
    private void SwipeDown()
    {
        //Если длина свайпа больше минимально необходимой и при этом свайпе еще не был задетекчен, то...
        if (swipeStartPosition.y - swipeCurrentPosition.y >= minSwipeLenght && swipeDetected == false)
        {
            //Вызываем метод остановки игрока, чтобы в движении резко сменить направление. Булевую переменную, отвечающую за движение вправо делаем истинной. А также детектим свайп.
            Debug.Log("Свайп вниз");
            playerMovement.StopPlayer();
            playerMovement.movingDown = true;
            swipeDetected = true;
        }
    }
    
    //Метод, фиксирующий позицию пальца на старте. Также когда палец на экране, фиксация свайпа становится доступна.
    private void DetectSwipeStartPosition()
    {
        if (swipeTouch.phase == TouchPhase.Began)
        {
            swipeDetected = false;
            swipeStartPosition = swipeTouch.position;
        }
    }
}
