using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRandomizer : MonoBehaviour
{

    /* On generation of a non unique character unit, their design (hair style/color, skin tone, eye color, etc) will
     * all be generated from this script. What this will do is assist in keeping the character's looks consistent across reclassing
     * as well as add variety in the units the player uses, even when using the same units. */

    [Header("Sprite Renderers")]
    public SpriteRenderer torso;
    public SpriteRenderer head;
    public SpriteRenderer neck;
    public SpriteRenderer leftArm;
    public SpriteRenderer leftShoulder;
    public SpriteRenderer rightArm;
    public SpriteRenderer rightShoulder;
    public SpriteRenderer leftThigh;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightThigh;
    public SpriteRenderer rightLeg;
    private SpriteRenderer primaryHair;
    private SpriteRenderer secondaryHair;
    public SpriteRenderer eyes;
    public SpriteRenderer[] frontHairOptions;
    public SpriteRenderer[] backHairOptions;
   

    [Header("Color Options")]
    public Color[] primarySkinTones;
    public Color[] associatedSecondarySkinTones;
    public Color[] primaryHairColors;
    public Color[] associatedSecondaryHairColors;
    public Color[] hairHighlightColors;
    public Color[] eyeColors;

    // Selected Colors
    private Color primarySkin;
    private Color secondarySkin;
    private Color primaryHairColor;
    private Color secondaryHairColor;

    private void Start()
    {
        int skinTone = selectSkinTone();
        primarySkin = primarySkinTones[skinTone];
        secondarySkin = associatedSecondarySkinTones[skinTone];
        GenerateAndReplaceSprites();
    }

    void GenerateAndReplaceSprites()
    {
        GetHairTypes();
        HandleSkinTones();
    }

    void GetHairTypes()
    {
        int fhc = Random.Range(0, primaryHairColors.Length);
        primaryHairColor = primaryHairColors[fhc];
        secondaryHairColor = associatedSecondaryHairColors[fhc];

        // Then select hair types for front and back
        int fr = Random.Range(0, frontHairOptions.Length);
        primaryHair = frontHairOptions[fr];
        primaryHair.gameObject.SetActive(true);
        primaryHair.color = primaryHairColor;

        int br = Random.Range(0, backHairOptions.Length);
        secondaryHair = backHairOptions[br];
        secondaryHair.gameObject.SetActive(true);
        secondaryHair.color = secondaryHairColor;
    }

    void HandleSkinTones()
    {
        HandleSprite(torso, true);
        HandleSprite(head, true);
        HandleSprite(neck, false);
        HandleSprite(leftArm, true);
        HandleSprite(leftShoulder, true);
        HandleSprite(rightArm, false);
        HandleSprite(rightShoulder, false);
        HandleSprite(leftThigh, true);
        HandleSprite(leftLeg, true);
        HandleSprite(rightThigh, false);
        HandleSprite(rightLeg, false);
    }

    void HandleSprite(SpriteRenderer sprt, bool isPrimary)
    {
        Color cToUse = isPrimary ? primarySkin : secondarySkin;
        sprt.color = cToUse;
    }

    int selectSkinTone()
    {
        int ind = Random.Range(0, primarySkinTones.Length);
        return ind;
    }

}
