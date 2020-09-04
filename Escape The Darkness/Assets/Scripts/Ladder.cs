using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum LadderParts { TOP, MIDDLE, BOTTOM };

    #region Serialzied Fields
    [SerializeField] LadderParts part = LadderParts.MIDDLE;
    [SerializeField] private Collider2D coll = default;
    [SerializeField] private LayerMask whatIsTop = default;

    //[SerializeField] private PlayerController player;
    public PlatformEffector2D pe = default;
    #endregion

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<PlayerController>())
        {
            PlayerController player = _collision.GetComponent<PlayerController>();

            switch (part)
            {
                case LadderParts.TOP:
                    player.TopLadder = true;

                    //PESwitcherOff();
                    break;

                case LadderParts.MIDDLE:
                    player.CanClimb = true;
                    player.Ladder = this;//Ladder 

                    //PESwitcherOn();
                    break;

                case LadderParts.BOTTOM:
                    player.BottomLadder = true;

                    //PESwitcherOff();
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.GetComponent<PlayerController>())
        {
            PlayerController player = _collision.GetComponent<PlayerController>();

            switch (part)
            {
                case LadderParts.MIDDLE:
                    player.CanClimb = false;

                    //PESwitcherOn();

                    break;

                case LadderParts.BOTTOM:
                    player.BottomLadder = false;

                    //PESwitcherOff();
                    break;

                case LadderParts.TOP:
                    player.TopLadder = false;

                    //PESwitcherOn();
                    break;

                default:
                    break;
            }
        }
    }

    public void PESwitcherOn()
    {
        pe.rotationalOffset = 180f;
    }

    public void PESwitcherOff()
    {
        pe.rotationalOffset = 0f;
    }
}
