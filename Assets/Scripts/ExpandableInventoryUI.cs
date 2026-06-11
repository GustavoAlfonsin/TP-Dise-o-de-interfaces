using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableInventoryUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backpackButton;
    [SerializeField] private Button closeButton;

    [Header("Inventory Items")]
    [SerializeField] private RectTransform[] itemSlots;

    [Header("Animation")]
    [SerializeField] private float slotSpacing = 70f;
    [SerializeField] private float animationDuration = 0.25f;

    private bool isOpen;
    private Vector2[] openPositions;
    private Vector2 closedPosition;

    private void Awake()
    {
        backpackButton.onClick.AddListener(ToggleInventory);
        closeButton.onClick.AddListener(CloseInventory);

        openPositions = new Vector2[itemSlots.Length];

        for (int i = 0; i < itemSlots.Length; i++)
        {
            openPositions[i] = itemSlots[i].anchoredPosition;
        }

        closedPosition = openPositions[0];

        ForceClosed();
    }

    private void ToggleInventory()
    {
        if (isOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    private void OpenInventory()
    {
        isOpen = true;

        StopAllCoroutines();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].gameObject.SetActive(true);

            Vector2 targetPosition = new Vector2(
                closedPosition.x + slotSpacing * (i + 1),
                closedPosition.y
            );

            StartCoroutine(MoveSlot(itemSlots[i], targetPosition));
        }

        closeButton.gameObject.SetActive(true);
    }

    private void CloseInventory()
    {
        isOpen = false;

        StopAllCoroutines();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            StartCoroutine(MoveSlotAndHide(itemSlots[i], closedPosition));
        }

        closeButton.gameObject.SetActive(false);
    }

    private void ForceClosed()
    {
        isOpen = false;

        foreach (RectTransform slot in itemSlots)
        {
            slot.anchoredPosition = closedPosition;
            slot.gameObject.SetActive(false);
        }

        closeButton.gameObject.SetActive(false);
    }

    private IEnumerator MoveSlot(RectTransform slot, Vector2 targetPosition)
    {
        Vector2 startPosition = slot.anchoredPosition;
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float t = timer / animationDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            slot.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        slot.anchoredPosition = targetPosition;
    }

    private IEnumerator MoveSlotAndHide(RectTransform slot, Vector2 targetPosition)
    {
        yield return MoveSlot(slot, targetPosition);

        if (!isOpen)
            slot.gameObject.SetActive(false);
    }
}
