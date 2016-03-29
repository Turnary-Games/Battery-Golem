using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProportionValue<T>
{
    public double Proportion { get; set; }
    public T Value { get; set; }
}

public static class ProportionValue
{
    public static ProportionValue<T> Create<T>(double proportion, T value)
    {
        return new ProportionValue<T> { Proportion = proportion, Value = value };
    }

    //static Random random = new Random();
    public static T ChooseByRandom<T>(
        this IEnumerable<ProportionValue<T>> collection)
    {
        double rnd = Random.Range(0.0f, 1.0f);
        foreach (var item in collection)
        {
            if (rnd < item.Proportion)
                return item.Value;
            rnd -= item.Proportion;
        }
        throw new UnityException("The proportions in the collection do not add up to 1.");
    }
}
