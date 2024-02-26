using System.Collections;
using UnityEngine;

public class EnemyDragon : MonoBehaviour
{
    private float minSpeed = 2f;
    private float maxSpeed = 15f;
    private float minTimeBetweenEggDrops = 0.4f;
    private float maxTimeBetweenEggDrops = 1.5f;
    private float behaviorTimer = 0f; // Таймер для изменения поведения дракона



    public GameObject dragonEggPrefab;
    public float speed = 4f;
    public float timeBetweenEggDrops = 1f;
    public float leftRightDistance = 10f;
    public float dynamicChanceDirection = 0.1f;
    public float speedSpread = 0.5f; // промежуток возможного изменения скорости
    public float timeBetweenEggDropsSpread = 0.1f; // промежуток возможного изменения времени
    public float timeToChangeBehavior = 3f; // значение таймера изменения направления

    void Start()
    {
        Invoke("DropEgg", 2f);
    }

    void DropEgg()
    {
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.0f);
        GameObject egg = Instantiate<GameObject>(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke("DropEgg", timeBetweenEggDrops);
    }

    void Update()
    {
        // Уменьшаем таймер изменения направления
        if (timeToChangeBehavior >= 0f)   // Если установить значение таймера меньше нуля, то поведение дракона, будет оставаться неизменным
            behaviorTimer += Time.deltaTime;

        // Если таймер достиг предела, меняем параметры
        if (behaviorTimer >= timeToChangeBehavior && timeToChangeBehavior >= 0f)
        {
            behaviorTimer = 0f;
            ChangeDragonBehavior();
        }
        // Двигаем дракона
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Проверяем границы и меняем направление при необходимости
        if (Mathf.Abs(pos.x) > leftRightDistance)
        {
            speed = Mathf.Abs(speed) * Mathf.Sign(pos.x) * -1f; // Меняем направление с сохранением знака
        }
    }

    void ChangeDragonBehavior()
    {
        float randomSpeedChange;

        // Меняем ускорение и таймер
        if (Mathf.Abs(speed) <= Mathf.Abs(minSpeed)) {
            randomSpeedChange = speedSpread;
        } else if (Mathf.Abs(speed) >= Mathf.Abs(maxSpeed)) {
            randomSpeedChange = -speedSpread;
        } else {
            randomSpeedChange = (Random.value > 0.5f) ? -speedSpread : speedSpread;
        }

        float newSpeed = Mathf.Clamp(Mathf.Abs(speed) + randomSpeedChange, minSpeed, maxSpeed);
        speed = newSpeed * Mathf.Sign(speed);
        // Меняем время между сбросами яиц
        float randomTimeBetweenEggDrops;
        if (timeBetweenEggDrops <= minTimeBetweenEggDrops) {
            randomTimeBetweenEggDrops = timeBetweenEggDropsSpread;
        } else if (speed >= maxTimeBetweenEggDrops) {
            randomTimeBetweenEggDrops = -timeBetweenEggDropsSpread;
        } else {
            randomTimeBetweenEggDrops = (Random.value > 0.5f) ? -timeBetweenEggDropsSpread : timeBetweenEggDropsSpread;
        }
        timeBetweenEggDrops = Mathf.Clamp(randomTimeBetweenEggDrops + timeBetweenEggDrops, minTimeBetweenEggDrops, maxTimeBetweenEggDrops);

        // Промежуток для chanceDirection (0.05f; 0.15f)
        float minChanceDirection = 0.05f;
        float maxChanceDirection = 0.15f;

        // Вычисляем шанс смены направления в зависимости от скорости
        float speedFactor = Mathf.Clamp01((Mathf.Abs(speed) - minSpeed) / (maxSpeed - minSpeed));
        float dynamicChanceDirection = Mathf.Lerp(minChanceDirection, maxChanceDirection, speedFactor);

        Debug.Log(speed + " ; " + timeBetweenEggDrops + " ; " + dynamicChanceDirection.ToString("F2"));
    }

    void FixedUpdate()
    {
        // Меняем направление с учетом динамического шанса
        if (Random.value < dynamicChanceDirection/10f) {
            speed *= -1;
        }
    }
}
