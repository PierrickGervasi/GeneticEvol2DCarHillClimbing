using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

public interface IGene
{
    IGene MutatedCopy();
    IGene GeneCopy();
}

public class FloatGene : IGene
{
    public float value;
    protected float minimum;
    protected float maximum;

    public FloatGene(float val, float min, float max)
    {
        value = val;
        minimum = min;
        maximum = max;
    }

    public IGene MutatedCopy()
    {
        var mutatedValue = SampleGaussian(value);
        if (mutatedValue < minimum)
        {
            mutatedValue = minimum;
        } else if (mutatedValue > maximum)
        {
            mutatedValue = maximum;
        }

        return new FloatGene(mutatedValue, minimum, maximum);
    }

    public IGene GeneCopy()
    {
        return new FloatGene(value, minimum, maximum);
    }

    private static float SampleGaussian(float mean, float stddev = 0.15f)
    {
        float x1 = Random.Range(0.0f, 1.0f);
        float x2 = Random.Range(0.0f, 1.0f);

        float y1 = (float) (Math.Sqrt(-2.0f * Math.Log(x1)) * Math.Cos(2.0f * Math.PI * x2));
        return y1 * stddev + mean;
    }
}

public class BoolGene : IGene
{
    public bool value;

    public BoolGene(bool val)
    {
        value = val;
    }

    public IGene GeneCopy()
    {
        return new BoolGene(value);
    }

    public IGene MutatedCopy()
    {
        if (Random.Range(0.0f, 1.0f) > 0.7f)
        {
            return new BoolGene(!value);
        }
        else
        {
            return new BoolGene(value);
        }
    }
}

public class CarWheel
{
    public static readonly float DIAMETER_MINIMUM = 0.8f;
    public static readonly float DIAMETER_MAXIMUM = 1.8f;

    public static readonly float RATIO_MINIMUM = 0.0f;
    public static readonly float RATIO_MAXIMUM = 1.0f;

    public CarWheel(float x, float y, float d, bool motor)
    {
        // TODO: check bounds
        xRatio = x;
        yRatio = y;
        diameter = d;
        hasMotor = motor;
    }
    public float xRatio;
    public float yRatio;
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
    private IGene[] genes = new IGene[GENE_COUNT];
    
    public CarParameters()
    {
        genes[(int)GeneIndex.BODY_WIDTH] = new FloatGene(0f, CarBody.MINIMUM, CarBody.MAXIMUM);
        genes[(int)GeneIndex.BODY_HEIGHT] = new FloatGene(0f, CarBody.MINIMUM, CarBody.MAXIMUM);
        genes[(int)GeneIndex.WHEEL_0_X] = new FloatGene(0f, CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        genes[(int)GeneIndex.WHEEL_0_Y] = new FloatGene(0f, CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        genes[(int)GeneIndex.WHEEL_0_DIAMETER] = new FloatGene(CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MAXIMUM);
        genes[(int)GeneIndex.WHEEL_1_X] = new FloatGene(0f, CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        genes[(int)GeneIndex.WHEEL_1_Y] = new FloatGene(0f, CarWheel.RATIO_MINIMUM, CarWheel.RATIO_MAXIMUM);
        genes[(int)GeneIndex.WHEEL_1_DIAMETER] = new FloatGene(CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MINIMUM, CarWheel.DIAMETER_MAXIMUM);

        genes[(int)GeneIndex.WHEEL_0_MOTOR] = new BoolGene(false);
        genes[(int)GeneIndex.WHEEL_1_MOTOR] = new BoolGene(false);
    } 

    public IGene GetGene(int index)
    {
        return genes[index];
    }

    public void SetGene(int index, IGene val)
    {
        // TODO: check bounds
        genes[index] = val.GeneCopy();
    }

    public void SetCarBody(float width, float height)
    {
        // TODO: check bounds
        ((FloatGene)(genes[(int)GeneIndex.BODY_WIDTH])).value = width;
        ((FloatGene)(genes[(int)GeneIndex.BODY_HEIGHT])).value = height;
    }

    public void SetCarWheel(int index, float x, float y, float diameter, bool hasMotor)
    {
        // TODO: check bounds
        switch(index)
        {
            case 0:
            {
                
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_X])).value = x;
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_Y])).value = y;
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_DIAMETER])).value = diameter;
                ((BoolGene)(genes[(int)GeneIndex.WHEEL_0_MOTOR])).value = hasMotor;
                break;
            }
            case 1:
            {
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_X])).value = x;
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_Y])).value = y;
                ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_DIAMETER])).value = diameter;
                ((BoolGene)(genes[(int)GeneIndex.WHEEL_1_MOTOR])).value = hasMotor;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Unsupported wheel index");
        }
        
    }

    public CarBody GetCarBody()
    {
        var width = ((FloatGene)(genes[(int)GeneIndex.BODY_WIDTH])).value;
        var height = ((FloatGene)(genes[(int)GeneIndex.BODY_HEIGHT])).value;
        var cb = new CarBody(width, height);
        return cb;
    }

    public CarWheel GetWheel(int index)
    {
        CarWheel wheel = null;

        switch(index)
        {
            case 0:
            {
                bool motor = ((BoolGene)(genes[(int)GeneIndex.WHEEL_0_MOTOR])).value;
                var x = ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_X])).value;
                var y = ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_Y])).value;
                var diameter = ((FloatGene)(genes[(int)GeneIndex.WHEEL_0_DIAMETER])).value;
                wheel = new CarWheel(x, y, diameter, motor);
                break;
            }
            case 1:
            {
                bool motor = ((BoolGene)(genes[(int)GeneIndex.WHEEL_1_MOTOR])).value;
                var x = ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_X])).value;
                var y = ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_Y])).value;
                var diameter = ((FloatGene)(genes[(int)GeneIndex.WHEEL_1_DIAMETER])).value;
                wheel = new CarWheel(x, y, diameter, motor);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(index), "Unsupported wheel index");
        }

        return wheel;
    }
}
