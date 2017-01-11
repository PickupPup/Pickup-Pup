using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogAdoptProfile : DogProfile {

    public Text breedText;
    public Text priceText;

    public override void SetProfile(int index, DogDescriptor dog)
    {
        base.SetProfile(index, dog);

        priceText.text = dog.CostToAdoptStr;
        breedText.text = dog.Breed.Breed;
    }
}
