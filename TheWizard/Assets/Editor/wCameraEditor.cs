using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(wCamera))]
public class wCameraEditor : Editor
{
    private wCamera Camera_Instance;

    public override void OnInspectorGUI( )
    {
        if ( !Camera_Instance )
            Camera_Instance = ( wCamera )target;

        EditorGUILayout.BeginVertical( );

        if(GUILayout.Button("Set Active Camera" ) )
        {
            Camera_Instance.SetAsActiveCamera( );
        }

        Camera_Instance.CameraType = 
            (W_CAMERA_TYPE)EditorGUILayout.EnumPopup( "Camera Type: ", Camera_Instance.CameraType );

        EditorGUILayout.Separator( );

        bool showDefaultData = false;

        switch ( Camera_Instance.CameraType )
        {
            case W_CAMERA_TYPE.STATIC:
                EditorGUILayout.LabelField( "---STATIC CAMERA---" );
                break;

            case W_CAMERA_TYPE.FOLLOW:
                EditorGUILayout.LabelField( "---FOLLOW CAMERA---" );

                /*
                 * Follow Transform
                 */
                Camera_Instance.TargetTransform =
                    ( Transform )EditorGUILayout.ObjectField( Camera_Instance.TargetTransform, typeof( Transform ), true );

                /*
                 * Follow Parameters
                 */
                Camera_Instance.TargetFollowSpeedX = 
                    EditorGUILayout.Slider("Follow Speed X:", Camera_Instance.TargetFollowSpeedX, wCamera.MinFollowSpeed, wCamera.MaxFollowSpeed );

                Camera_Instance.TargetFollowSpeedY =
                    EditorGUILayout.Slider( "Follow Speed Y:", Camera_Instance.TargetFollowSpeedY, wCamera.MinFollowSpeed, wCamera.MaxFollowSpeed );

                //Show default data at end
                showDefaultData = true;

                break;

            case W_CAMERA_TYPE.INPUT:
                EditorGUILayout.LabelField( "---USER INPUT CAMERA---" );
                showDefaultData = true;
                break;
        }

        if( showDefaultData )
        {
            EditorGUILayout.Separator( );
            EditorGUILayout.LabelField( "---OTHER SETTINGS---" );
            Camera_Instance.TargetZoomDistance =
                EditorGUILayout.Slider( "Zoom Distance:", Camera_Instance.TargetZoomDistance, wCamera.MinZoomDistance, wCamera.MaxZoomDistance );

            Camera_Instance.MinmumWorldX =
                EditorGUILayout.Slider( "Minimum World X", Camera_Instance.MinmumWorldX, -1000, 1000 );

            Camera_Instance.MinmumWorldY =
                EditorGUILayout.Slider( "Minimum World Y", Camera_Instance.MinmumWorldY, -1000, 1000 );

            Camera_Instance.MaximumWorldX =
    EditorGUILayout.Slider( "Maximum World X", Camera_Instance.MaximumWorldX, -1000, 1000 );

            Camera_Instance.MaximumWorldY =
                EditorGUILayout.Slider( "Maximum World Y", Camera_Instance.MaximumWorldY, -1000, 1000 );

        }

        EditorGUILayout.EndVertical( );
    }
}
