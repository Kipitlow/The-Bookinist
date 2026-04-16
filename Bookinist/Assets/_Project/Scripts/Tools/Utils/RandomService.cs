using UnityEngine;

using System;

public class RandomService
{
    private System.Random _rng;
    public int Seed { get; private set; }

    public RandomService(int? seed = null)
    {
        Seed = seed ?? GenerateSeed();
        _rng = new System.Random(Seed);
    }

    private int GenerateSeed()
    {
        // Combines time + GUID hash for better randomness
        return Guid.NewGuid().GetHashCode() ^ DateTime.UtcNow.Ticks.GetHashCode();
    }

    /// <summary>
    /// Reset the RNG with a new seed
    /// </summary>
    public void Reseed(int newSeed)
    {
        Seed = newSeed;
        _rng = new System.Random(newSeed);
    }

    /// <summary>
    /// Returns int in [min, max)
    /// </summary>
    public int Range(int min, int max)
    {
        return _rng.Next(min, max);
    }

    /// <summary>
    /// Returns float in [0, 1)
    /// </summary>
    public float Value()
    {
        return (float)_rng.NextDouble();
    }

    /// <summary>
    /// Returns float in [min, max)
    /// </summary>
    public float Range(float min, float max)
    {
        return min + (float)_rng.NextDouble() * (max - min);
    }

    /// <summary>
    /// Returns true with given probability (0–1)
    /// </summary>
    public bool Chance(float probability)
    {
        return Value() < probability;
    }

    /// <summary>
    /// Pick a random element from an array
    /// </summary>
    public T Pick<T>(T[] array)
    {
        if (array == null || array.Length == 0)
            throw new ArgumentException("Array is null or empty");

        return array[Range(0, array.Length)];
    }

    /// <summary>
    /// Shuffle an array in-place (Fisher-Yates)
    /// </summary>
    public void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
