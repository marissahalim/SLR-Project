using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialControl : MonoBehaviour
{
    // References to knob controllers
    public KnobController knob1;
    public KnobController knob2;
    public KnobController knob3;

    // Timing and debugging options
    public float timeToNext = 2f; // Time between state transitions
    public bool debugMode = false; // Enable or disable debug logs in Unity Editor
    private float timer = 0; // Timer for state transitions

    // Serial port setup
    private SerialPort serialPort = new SerialPort("COM3", 38400); // Adjust COM port as needed
    private string serialData = ""; // Raw serial data received

    // Knob value mappings for each state
    private float[] knob1Values = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 0, 1, 2, 3, 4, 5, 6, 7, 8, 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    private float[] knob23Values = { 2020, 2030, 2040, 2050, 2060, 2070, 2080, 2090, 2100,
                                     2020, 2030, 2040, 2050, 2060, 2070, 2080, 2090, 2100,
                                     2020, 2030, 2040, 2050, 2060, 2070, 2080, 2090, 2100 };

    // Button states
    private bool isGreenDown = false;
    private bool isOrangeDown = false;
    private bool isRedDown = false;

    private bool greenWasReleased = false;
    private bool orangeWasReleased = false;
    private bool redWasReleased = false;

    // State machine enum
    public enum States
    {
        g_2020, g_2030, g_2040, g_2050, g_2060, g_2070, g_2080, g_2090, g_2100,
        o_2020, o_2030, o_2040, o_2050, o_2060, o_2070, o_2080, o_2090, o_2100,
        r_2020, r_2030, r_2040, r_2050, r_2060, r_2070, r_2080, r_2090, r_2100,
    }

    private States currentState = States.g_2020;

    // Initialize the serial port and set the default state
    void Awake()
    {
        OpenSerialPort();
        SetToGreen(); // Default initial state
    }

    void Update()
    {
        ReadSerialInput();

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
                UpdateKnobs();
                HandleGreenState();
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
                UpdateKnobs();
                HandleOrangeState();
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
                UpdateKnobs();
                HandleRedState();
                break;
        }
    }

    // Open the serial port for communication
    void OpenSerialPort()
    {
        try
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                serialPort.ReadTimeout = 100;
                if (debugMode) Debug.Log("Serial port opened.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    // Read data from the serial port
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

                    greenWasReleased = !isGreenDown && greenWasReleased;
                    orangeWasReleased = !isOrangeDown && orangeWasReleased;
                    redWasReleased = !isRedDown && redWasReleased;

                    if (debugMode) Debug.Log($"Serial Data: {serialData}");
                }
            }
        }
        catch (System.TimeoutException)
        {
            // Ignore timeouts
        }
    }

    // Update knob values based on the current state
    void UpdateKnobs()
    {
        knob1.SetValue(knob1Values[(int)currentState]);
        knob2.SetValue(knob23Values[(int)currentState]);
        knob3.SetValue(knob23Values[(int)currentState] + timer * 10 / timeToNext);
    }

    // Handle Green button logic
    void HandleGreenState()
    {
        if (isGreenDown)
        {
            timer += Time.deltaTime;
            if (timer >= timeToNext)
            {
                currentState = (currentState != States.g_2100) ? currentState + 1 : States.g_2020;
                timer = 0;
            }
        }

        if (greenWasReleased)
        {
            currentState = (currentState != States.g_2100) ? currentState + 1 : States.g_2020;
            greenWasReleased = false;
        }

        if (isOrangeDown)
        {
            currentState += 9;
            SetToOrange();
            timer = 0;
        }

        if (isRedDown)
        {
            currentState += 18;
            SetToRed();
            timer = 0;
        }
    }

    // Handle Orange button logic
    void HandleOrangeState()
    {
        if (isOrangeDown)
        {
            timer += Time.deltaTime;
            if (timer >= timeToNext)
            {
                currentState = (currentState != States.o_2100) ? currentState + 1 : States.o_2020;
                timer = 0;
            }
        }

        if (orangeWasReleased)
        {
            currentState = (currentState != States.o_2100) ? currentState + 1 : States.o_2020;
            orangeWasReleased = false;
        }

        if (isGreenDown)
        {
            currentState -= 9;
            SetToGreen();
            timer = 0;
        }

        if (isRedDown)
        {
            currentState += 9;
            SetToRed();
            timer = 0;
        }
    }

    // Handle Red button logic
    void HandleRedState()
    {
        if (isRedDown)
        {
            timer += Time.deltaTime;
            if (timer >= timeToNext)
            {
                currentState = (currentState != States.r_2100) ? currentState + 1 : States.r_2020;
                timer = 0;
            }
        }

        if (redWasReleased)
        {
            currentState = (currentState != States.r_2100) ? currentState + 1 : States.r_2020;
            redWasReleased = false;
        }

        if (isOrangeDown)
        {
            currentState -= 9;
            SetToOrange();
            timer = 0;
        }

        if (isGreenDown)
        {
            currentState -= 18;
            SetToGreen();
            timer = 0;
        }
    }

    // Set the knob colors for Green state
    void SetToGreen()
    {
        if (debugMode) Debug.Log("Set to Green");
        knob1.SetColor(new Color(0.48f, 0.82f, 0.29f, 1f)); // Green
        knob2.SetColor(new Color(0.48f, 0.82f, 0.29f, 1f)); // Green
        knob3.SetColor(new Color(0.48f, 0.82f, 0.29f, 1f)); // Green
    }

    // Set the knob colors for Orange state
    void SetToOrange()
    {
        if (debugMode) Debug.Log("Set to Orange");
        knob1.SetColor(new Color(0.85f, 0.57f, 0.04f, 1f)); // Orange
        knob2.SetColor(new Color(0.85f, 0.57f, 0.04f, 1f)); // Orange
        knob3.SetColor(new Color(0.85f, 0.57f, 0.04f, 1f)); // Orange
    }

    // Set the knob colors for Red state
    void SetToRed()
    {
        if (debugMode) Debug.Log("Set to Red");
        knob1.SetColor(new Color(0.85f, 0.19f, 0.04f, 1f)); // Red
        knob2.SetColor(new Color(0.85f, 0.19f, 0.04f));     // Red
        knob3.SetColor(new Color(0.85f, 0.19f, 0.04f));     // Red
    }
}
