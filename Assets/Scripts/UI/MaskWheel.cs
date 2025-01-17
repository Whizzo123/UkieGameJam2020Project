﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskWheel : MonoBehaviour
{

    public GameObject container;
    private MaskWheelComponent[] wheelSegments;
    private MaskWheelComponent selectedSegment;

    void Awake()
    {
        container.SetActive(false);
        wheelSegments = container.GetComponentsInChildren<MaskWheelComponent>(true);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            container.SetActive(true);
            FindObjectOfType<PauseSystem>().PauseGame();
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            if (selectedSegment != null)
            {
                Debug.Log("Switching mask to: " + selectedSegment.equippedMask);
                FindObjectOfType<SwitchMask>().Switch((int)selectedSegment.equippedMask);
            }
            container.SetActive(false);
            FindObjectOfType<PauseSystem>().ResumeGame();
        }
    }

    public MaskWheelComponent GrabFreeComponent()
    {
        Debug.Log("Wheel Segments: " + wheelSegments.Length);
        foreach (MaskWheelComponent wheelComponent in wheelSegments)
        {
            if(wheelComponent.equippedMask == MASKS.NONE)
            {
                Debug.Log("Returning wheel segment");
                return wheelComponent;
            }
        }
        return null;
    }

    public void AddMaskToWheel(MASKS maskToAdd, MaskWheelSprite sprite)
    {
        MaskWheelComponent segment = wheelSegments[(int)maskToAdd];
       
        if (segment != null)
        {
            Debug.Log("Adding mask to wheel");
            segment.equippedMask = maskToAdd;
            segment.SetUpSprites(sprite);
        }
        else
            Debug.LogError("Have filled up the mask wheel");
    }

    public void SetSegmentSelected(MaskWheelComponent component)
    {
        selectedSegment = component;
    }

    public void SetSegmentDeselected()
    {
        selectedSegment = null;
    }
}
