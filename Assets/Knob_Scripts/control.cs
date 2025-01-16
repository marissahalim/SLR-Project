using System.IO.Ports;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class control : MonoBehaviour
{

    public string portName;
    SerialPort serialPort = null;
    private string serialData = ""; // Raw serial data received
    string[] lastButtonStates = { "0", "0", "0", "0", "0" };


    public KnobController knob1;
    // public KnobController knob2;
    // public KnobController knob3;

    public float timeToNext = 2f;

    public enum States
    {
        g_2020, g_2030, g_2040, g_2050, g_2060, g_2070, g_2080, g_2090, g_2100,
        o_2020, o_2030, o_2040, o_2050, o_2060, o_2070, o_2080, o_2090, o_2100,
        r_2020, r_2030, r_2040, r_2050, r_2060, r_2070, r_2080, r_2090, r_2100,
    };

    public States currentState = States.g_2020;

    [Header("Scenarios")]
    public ImageLoader scenarioImgs;
    // public ImageLoader intImgs;
    // public ImageLoader hiImgs;


    //float[] knob1Values = {0,1,2,3,4,5,6,7,8,0,1,2,3,4,5,6,7,8,0,1,2,3,4,5,6,7,8};
    float[] knob23Values = {2020,2030,2040,2050,2060,2070,2080,2090,2100,
    2020,2030,2040,2050,2060,2070,2080,2090,2100,
    2020,2030,2040,2050,2060,2070,2080,2090,2100};

    bool isGreenDown = false;
    bool isOrangeDown = false;
    bool isRedDown = false;

    bool greenWasReleased = false;
    bool orangeWasReleased = false;
    bool redWasReleased = false;


    float timer = 0;

    void Awake()
    {
        OpenSerialPort();
        setToGreen();
    }
    void Update()
    {
        if (serialPort != null)
        {
            ReadSerialInput();
        }

        Debug.Log((int)currentState);
        // State machine
        switch (currentState)
        {
            case States.g_2020:
            case States.g_2030:
            case States.g_2040:
            case States.g_2050:
            case States.g_2060:
            case States.g_2070:
            case States.g_2080:
            case States.g_2090:
            case States.g_2100:
                //knob1.SetValue(knob1Values[(int)currentState]);
                knob1.SetValue(knob23Values[(int)currentState]);
                //knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isGreenDown)
                {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext)
                    {
                        currentState = (currentState != States.g_2100) ? currentState + 1 : States.g_2020;
                        timer = 0;
                        //update image of low risk scenario
                        scenarioImgs.UpdateImage((int)currentState);
                    }
                }
                if (greenWasReleased)
                {
                    currentState = (currentState != States.g_2100) ? currentState + 1 : States.g_2020;
                    timer = 0;
                    greenWasReleased = false;
                    //update image of low risk scenario
                    scenarioImgs.UpdateImage((int)currentState);
                }
                if (isOrangeDown)
                {
                    currentState += 9;
                    timer = 0;
                    setToOrange();
                }
                if (isRedDown)
                {
                    currentState += 18;
                    timer = 0;
                    setToRed();
                }
                break;

            case States.o_2020:
            case States.o_2030:
            case States.o_2040:
            case States.o_2050:
            case States.o_2060:
            case States.o_2070:
            case States.o_2080:
            case States.o_2090:
            case States.o_2100:
                //knob1.SetValue(knob1Values[(int)currentState]);
                knob1.SetValue(knob23Values[(int)currentState]);
                //knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isOrangeDown)
                {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext)
                    {
                        currentState = (currentState != States.o_2100) ? currentState + 1 : States.o_2020;
                        timer = 0;
                        //update image of low risk scenario
                        scenarioImgs.UpdateImage((int)currentState);
                    }
                }
                if (orangeWasReleased)
                {
                    currentState = (currentState != States.o_2100) ? currentState + 1 : States.o_2020;
                    timer = 0;
                    orangeWasReleased = false;
                    //update image of low risk scenario
                    scenarioImgs.UpdateImage((int)currentState);
                }
                if (isGreenDown)
                {
                    currentState -= 9;
                    timer = 0;
                    setToGreen();
                }
                if (isRedDown)
                {
                    currentState += 9;
                    timer = 0;
                    setToRed();
                }
                break;

            case States.r_2020:
            case States.r_2030:
            case States.r_2040:
            case States.r_2050:
            case States.r_2060:
            case States.r_2070:
            case States.r_2080:
            case States.r_2090:
            case States.r_2100:
                //knob1.SetValue(knob1Values[(int)currentState]);
                knob1.SetValue(knob23Values[(int)currentState]);
                //knob3.SetValue(knob23Values[(int)currentState] + timer*10/timeToNext);
                if (isRedDown)
                {
                    timer += Time.deltaTime;
                    if (timer >= timeToNext)
                    {
                        currentState = (currentState != States.r_2100) ? currentState + 1 : States.r_2020;
                        timer = 0;
                        //update image of low risk scenario
                        scenarioImgs.UpdateImage((int)currentState);
                    }
                }
                if (redWasReleased)
                {
                    currentState = (currentState != States.r_2100) ? currentState + 1 : States.r_2020;
                    timer = 0;
                    redWasReleased = false;
                    //update image of low risk scenario
                    scenarioImgs.UpdateImage((int)currentState);
                }
                if (isOrangeDown)
                {
                    currentState -= 9;
                    timer = 0;
                    setToOrange();
                }
                if (isGreenDown)
                {
                    setToGreen();
                    currentState -= 18;
                    timer = 0;
                }
                break;
        }

    }

    void OpenSerialPort()
    {
        try
        {
            if (portName != "") {
                serialPort = new SerialPort(portName, 38400); 
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    serialPort.ReadTimeout = 100;
                    Debug.Log("Serial port opened.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void ReadSerialInput()
    {
        try
        {
            if (serialPort.IsOpen && serialPort.BytesToRead > 0)
            {
                serialData = serialPort.ReadLine();
                string[] buttonStates = serialData.Trim().Split(',');

                if (buttonStates.Length >= 3)
                {
                    isGreenDown = buttonStates[0] == "1";
                    isOrangeDown = buttonStates[1] == "1";
                    isRedDown = buttonStates[2] == "1";

                    greenWasReleased = (lastButtonStates[0] == "1" && !isGreenDown);
                    orangeWasReleased = (lastButtonStates[1]) == "1" && !isOrangeDown;
                    redWasReleased = (lastButtonStates[2] == "1" && !isRedDown); 
 
                    lastButtonStates = buttonStates;
                    Debug.Log($"Serial Data: {serialData}");
                }
            }
        }
        catch (System.TimeoutException)
        {
            // Ignore timeouts
        }
    }

    public void HandleKnob(float value)
    {
        Debug.Log("Handle Knob   " + value);
    }


    public void GreenMouseDownHandle()
    {
        isGreenDown = true;
    }

    public void GreenMouseUpHandle()
    {
        isGreenDown = false;
        greenWasReleased = true;
    }

    public void OrangeMouseDownHandle()
    {
        isOrangeDown = true;
    }

    public void OrangeMouseUpHandle()
    {
        isOrangeDown = false;
        orangeWasReleased = true;
    }
    public void RedMouseDownHandle()
    {
        isRedDown = true;
    }

    public void RedMouseUpHandle()
    {
        isRedDown = false;
        redWasReleased = true;
    }

    public void setToGreen()
    {
        knob1.SetColor(new Color(0.48f, 0.82f, 0.29f, 1f));
        //knob2.SetColor( new Color(0.48f, 0.82f, 0.29f, 1f));
        //knob3.SetColor( new Color(0.48f, 0.82f, 0.29f, 1f));
    }
    public void setToOrange()
    {
        knob1.SetColor(new Color(0.85f, 0.57f, 0.04f, 1f));
        //knob2.SetColor( new Color(0.85f, 0.57f, 0.04f, 1f));
        //knob3.SetColor( new Color(0.85f, 0.57f, 0.04f, 1f));
    }
    public void setToRed()
    {
        knob1.SetColor(new Color(0.85f, 0.19f, 0.04f, 1f));
        //knob2.SetColor( new Color(0.85f, 0.19f, 0.04f, 1f));
        //knob3.SetColor( new Color(0.85f, 0.19f, 0.04f, 1f));
    }

}
