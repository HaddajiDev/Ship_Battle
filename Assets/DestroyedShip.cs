using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedShip : MonoBehaviour
{
    public SpriteRenderer ShipPart_1;
    public SpriteRenderer ShipPart_2;
    public SpriteRenderer cannon;
    public SpriteRenderer stand;
    public SpriteRenderer flag;
    public SpriteRenderer sail;
    public SpriteRenderer anchor_top;
    public SpriteRenderer anchor_bottom;
    public SpriteRenderer helm;

    public ShipCosmaticData shipCosmatic;
    public CanonSkins CannonCosmatic;
    public SailCosmaticData sailCosmatic;
    public HelmCosmaticData helmCosmatic;
    public FlagCosmaticData flagCosmatic;
    public AnchorCosmaticData anchorCosmatic;

    Destroy_Effect fo;

    

    void Start()
    {
        fo = GetComponent<Destroy_Effect>();
        if (fo.Ship)
        {
            ShipCosmatic shipCos = shipCosmatic.Get_Skin(GameManager.Instance.player_1._selectedShip);
            ShipPart_1.sprite = shipCos.half_1;
            ShipPart_2.sprite = shipCos.half_2;

            Cosmatic sailSkin = sailCosmatic.Get_Skin(GameManager.Instance.player_1._selectedSail);
            sail.sprite = sailSkin.Cover;

            Cosmatic flagSkin = flagCosmatic.Get_Skin(GameManager.Instance.player_1._selectedFlag);
            flag.sprite = flagSkin.Cover;

            Cosmatic helmSkin = helmCosmatic.Get_Skin(GameManager.Instance.player_1._selectedHelm);
            helm.sprite = helmSkin.Cover;

            CanonCosmaticData cannonSkin = CannonCosmatic.Get_Skin(GameManager.Instance.player_1._selectedCannon);
            cannon.sprite = cannonSkin.Cover;
            stand.sprite = cannonSkin.Stand;

            AnchorCosmatic anchorSkin = anchorCosmatic.Get_Skin(GameManager.Instance.player_1._selectedAnchor);
            anchor_top.sprite = anchorSkin.Top;
            anchor_bottom.sprite = anchorSkin.Bottom;
        }
        
    }

}
