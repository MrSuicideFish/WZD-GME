using UnityEngine;
using System.Collections;

public enum W_CAMERA_TYPE
{
    STATIC,
    FOLLOW,
    INPUT
}

[RequireComponent(typeof(Camera))]
public class wCamera : MonoBehaviour
{
    //Static Members
    public static wCamera CurrentCamera { get; private set; }
    public static wCamera[] AllCameras { get { return FindObjectsOfType<wCamera>( ); } }

    public static readonly float MinFollowSpeed = 1.0f;
    public static readonly float MaxFollowSpeed = 10.0f;

    public static readonly float MinZoomDistance = 0.0f;
    public static readonly float MaxZoomDistance = 5.0f;

    public static bool CameraIsAvailableToChange = true;

    //Cache
    private Camera CameraComponent;
    [HideInInspector] public Transform TargetTransform;

    [HideInInspector] public float TargetFollowSpeedX;
    [HideInInspector] public float TargetFollowSpeedY;

    [HideInInspector] public float TargetInputSpeed;
    [HideInInspector] public float TargetZoomDistance;

    //Camera Flags
    public bool IsLocked { get; private set; }
    public bool IsActive { get { return CurrentCamera == this; } }
    public W_CAMERA_TYPE CameraType = W_CAMERA_TYPE.STATIC;

    //Query Methods

    private void InitializeCamera( )
    {
        CameraComponent = GetComponent<Camera>( );
        IsLocked = false;

        if ( !CurrentCamera )
            CurrentCamera = this;
    }

    private void FixedUpdate( )
    {
        if ( !IsLocked && IsActive )
        {
            ProcessCameraState( CameraType );
        }
    }

    protected virtual void ProcessCameraState(W_CAMERA_TYPE type )
    {
        float zoom = ( 3 ) + TargetZoomDistance;

        Vector3 calculatedPosition =
            new Vector3( transform.position.x, transform.position.y, -10 );

        switch ( CameraType )
        {
            case W_CAMERA_TYPE.STATIC:
                break;

            case W_CAMERA_TYPE.FOLLOW:

                if(TargetTransform)
                {
                    var xPos =
                        Mathf.Lerp( calculatedPosition.x, TargetTransform.position.x, TargetFollowSpeedX * Time.deltaTime );
                    var yPos =
                        Mathf.Lerp( calculatedPosition.y, TargetTransform.position.y, TargetFollowSpeedY * Time.deltaTime );

                    calculatedPosition = new Vector3( xPos, yPos, -10 );
                }
                else //No target
                {

                }

                break;

            case W_CAMERA_TYPE.INPUT:
                break;
        }

        transform.position = calculatedPosition;
        CameraComponent.orthographicSize = zoom;
    }

    public void SetAsActiveCamera( )
    {
        if ( CameraIsAvailableToChange )
        {
            var cams = AllCameras;
            for ( int i = 0; i < cams.Length; i++ )
            {
                cams[i].gameObject.SetActive( cams[i] == this );
            }

            CurrentCamera = this;
            CurrentCamera.InitializeCamera( );
        }
    }
}