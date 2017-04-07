using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInterract : MonoBehaviour {

    Animator animator;

    int affectionLevel = 0;
    int tapCount = 0;

    int hunger = 0;

    [SerializeField]
    Vector3 target = new Vector3(0, 0, 0);

	void Start () {
        animator = GetComponentInChildren<Animator>();

    }

    void Update () {
        if (transform.position != target)
        {
            animator.SetBool("Moving", true);
            Move();
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void OnMouseDown()
    {
        Pet();
    }

    public void Pet()
    {
        animator.SetTrigger("Pet");
        tapCount++;
        if(tapCount % 5 == 0)
        {
            affectionLevel++;
        }
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void Eat()
    {
        animator.SetTrigger("Eat");
        hunger = 0;
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime);
    }
}
