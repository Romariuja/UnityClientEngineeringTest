using System;
using System.Collections;
using System.Collections.Generic;
using Completed;
using UnityEngine;
using UnityEngine.UI;

public class FoodBar : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public int foodCap;							//foodCap

    private Slider slider;
    

    private void Start()
    {
        foodCap = GameManager.instance.foodCap;
        player.OnFoodUpdatedEvent += UpdateFoodBar;
        slider = gameObject.GetComponent<Slider>();
        player.OnFoodUpdatedEvent.Invoke(player.food);
    }

    private void OnDestroy()
    {
        player.OnFoodUpdatedEvent -= UpdateFoodBar;

    }

    private void UpdateFoodBar(float FoodPoints)
    {
        slider.value = ((float)FoodPoints) /foodCap;
    }
}
