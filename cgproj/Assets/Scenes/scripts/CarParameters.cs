using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeneIndex
{
    BODY_WIDTH,
    BODY_HEIGHT,
    WHEEL_0_X,
    WHEEL_0_Y,
    WHEEL_0_MOTOR,
    WHEEL_0_DIAMETER,
    WHEEL_1_X,
    WHEEL_1_Y,
    WHEEL_1_MOTOR,
    WHEEL_1_DIAMETER,
}

public class CarWheel
{
    public static readonly float DIAMETER_MINIMUM = 0.8f;
    public static readonly float DIAMETER_MAXIMUM = 1.8f;

    public CarWheel(float x, float y, float d, bool motor)
    {
        // TODO: check bounds
        xPosition = x;
        yPosition = y;
        diameter = d;
        hasMotor = motor;
    }
    public float xPosition;
    public float yPosition;
    public float diameter;
    public bool hasMotor;
}

public class CarBody
{
    public static readonly float MINIMUM = 1.5f;
    public static readonly float MAXIMUM = 3.5f;

    public CarBody(float w, float h)
    {
        // TODO: check bounds
        width = w;
        height = h;
    }

    public float width;
    public float height;
}

public class CarParameters : ScriptableObject
{
    public static readonly int GENE_COUNT = 10;
    private float[] genes = new float[GENE_COUNT];

    public float GetGene(int index)
    {
        return 0.0f;
    }

    public void SetGene(int index, float val)
    {
        // TODO: check bounds
        genes[index] = val;
    }

    public void SetCarBody(float width, float height)
    {
        // TODO: check bounds
        genes[(int)GeneIndex.BODY_WIDTH] = width;
        genes[(int)GeneIndex.BODY_HEIGHT] = height;
    }

    public void SetCarWheel(int index, float x, float y, float diameter, bool hasMotor)
    {
        // TODO: check bounds
        switch(index)
        {
            case 0:
            {
                genes[(int)GeneIndex.WHEEL_0_X] = x;
                genes[(int)GeneIndex.WHEEL_0_Y] = y;
                genes[(int)GeneIndex.WHEEL_0_DIAMETER] = diameter;
                genes[(int)GeneIndex.WHEEL_0_MOTOR] = Convert.ToSingle(y);
                break;
            }
            case 1:
            {
                genes[(int)GeneIndex.WHEEL_1_X] = x;
                genes[(int)GeneIndex.WHEEL_1_Y] = y;
                genes[(int)GeneIndex.WHEEL_1_DIAMETER] = diameter;
                genes[(int)GeneIndex.WHEEL_1_MOTOR] = Convert.ToSingle(y);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Unsupported wheel index");
        }
        
    }

    public CarBody GetCarBody()
    {
        var cb = new CarBody(genes[(int)GeneIndex.BODY_WIDTH], genes[(int)GeneIndex.BODY_HEIGHT]);
        return cb;
    }

    public CarWheel GetWheel(int index)
    {
        CarWheel wheel = null;

        switch(index)
        {
            case 0:
            {
                bool motor = Convert.ToBoolean(genes[(int)GeneIndex.WHEEL_0_MOTOR]);
                wheel = new CarWheel(genes[(int)GeneIndex.WHEEL_0_X], genes[(int)GeneIndex.WHEEL_0_Y], genes[(int)GeneIndex.WHEEL_0_DIAMETER], motor);
                break;
            }
            case 1:
            {
                bool motor = Convert.ToBoolean(genes[(int)GeneIndex.WHEEL_1_MOTOR]);
                wheel = new CarWheel(genes[(int)GeneIndex.WHEEL_1_X], genes[(int)GeneIndex.WHEEL_1_Y], genes[(int)GeneIndex.WHEEL_1_DIAMETER], motor);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Unsupported wheel index");
        }

        return wheel;
    }
}
