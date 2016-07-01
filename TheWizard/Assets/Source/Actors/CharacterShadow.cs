using UnityEngine;
using System.Collections;
using PlatformerCharacter2D = UnityStandardAssets._2D.PlatformerCharacter2D;

[RequireComponent( typeof( PlatformerCharacter2D ) )]
public class CharacterShadow : MonoBehaviour
{
    public SpriteRenderer CharacterSpriteRenderer;
    public GameObject ShadowPrefab;
    public float ShadowScale = 1.0f;

    //Cache
    private PlatformerCharacter2D PlatformCharacter;
    private Transform ShadowTransform;
    private Bounds CharacterBounds;

    void Awake( )
    {
        PlatformCharacter = GetComponent<PlatformerCharacter2D>( );

        ShadowTransform =
            ( ( GameObject )GameObject.Instantiate(
                ShadowPrefab, transform.position, Quaternion.Euler( 0, 0, 0 ) ) ).transform;
        ShadowTransform.SetParent( transform );
        ShadowTransform.localScale = new Vector3( ShadowScale, ShadowScale, ShadowScale );

        if ( CharacterSpriteRenderer != null )
            CharacterBounds = CharacterSpriteRenderer.bounds;
    }

    void LateUpdate( )
    {
        if ( CharacterSpriteRenderer.isVisible )
        {
            if ( ShadowTransform != null )
            {
                RaycastHit2D groundCast = PlatformCharacter.GROUND_CHECK( );

                if ( groundCast != default(RaycastHit2D) )
                {
                    //Do raycast for ground
                    var footPos =
                        new Vector3(
                            CharacterBounds.center.x,
                            CharacterBounds.center.y - CharacterBounds.extents.y,
                            0 ) * 1.0f;

                    var distToGround =
                        Vector2.Distance(
                            transform.TransformPoint( footPos ),
                            groundCast.point );

                    if ( distToGround < 5.5f )
                    {
                        //Enable shadow
                        if ( !ShadowTransform.gameObject.activeInHierarchy )
                            ShadowTransform.gameObject.SetActive( true );

                        var scaleFactor = 1.0f - Mathf.Round( ( distToGround / 6.0f ) * 100 ) / 100;

                        ShadowTransform.localPosition = footPos - new Vector3( 0, distToGround, 0 );
                        ShadowTransform.localScale = new Vector3( scaleFactor, scaleFactor, scaleFactor );
                    }
                    else
                    {
                        ShadowTransform.gameObject.SetActive( false );
                    }
                }
                else
                {
                    ShadowTransform.gameObject.SetActive( false );
                }
            }
        }
    }
}