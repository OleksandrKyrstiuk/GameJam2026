using UnityEngine;

public class ClothesInteractable : Interactable
{
    [Header("Clothes (visible on start)")]
    [SerializeField] private GameObject clothesObject;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [SerializeField] private bool hideOnce = true;

    private bool alreadyHidden;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        HideClothes();

        return taskDone;
    }

    private void HideClothes()
    {
        if (hideOnce && alreadyHidden)
            return;

        if (clothesObject == null)
        {
            Debug.LogWarning($"ClothesInteractable on '{gameObject.name}': Clothes Object is not assigned!", this);
            return;
        }

        alreadyHidden = true;

        clothesObject.SetActive(false);
    }
}
