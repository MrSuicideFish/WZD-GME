using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class WorldObjectManager : MonoBehaviour
{
    public GameObject[] AssetsToPreload;

    //Cache
    private static GameObject cachedObjectContainer;
    private static GameObject CachedObjectContainer
    {
        get
        {
            if ( cachedObjectContainer == null )
            {
                cachedObjectContainer = new GameObject( "CachedObjectContainer" );
                cachedObjectContainer.transform.position = new Vector3( 0, 0, 0 );
            }

            return cachedObjectContainer;
        }
    }

    private static GameObject[] CachedObjects = new GameObject[0];

    public static GameObject GetObject(string objectName )
    {
        for(int i = 0; i < CachedObjects.Length; i++ )
        {
            if(objectName == RemoveCloneFromName( CachedObjects[i].name ) )
            {
                
            }
        }

        return null;
    }

    public static bool StashObject(ref GameObject newObject )
    {
        if ( newObject == null )
            return false;

        GameObject[] tmpContainer = new GameObject[CachedObjects.Length + 1];

        int i = 0;
        for(i = 0; i < CachedObjects.Length; i++ )
            tmpContainer[i] = CachedObjects[i];

        tmpContainer[i] = newObject;
        tmpContainer[i].transform.SetParent( CachedObjectContainer.transform, false );
        tmpContainer[i].SetActive( false );

        CachedObjects = tmpContainer;

        return true;
    }

    public static bool UnstashObject(string ObjectName )
    {
        GameObject objToUnstash = CachedObjects.FirstOrDefault( x => x.name == RemoveCloneFromName( x.name ) );

        if ( !objToUnstash )
            return false;

        return true;
    }

    public static string RemoveCloneFromName(string name )
    {
        return name.Substring( 0, name.Length - 7 );
    }

    private void Awake( )
    {
        for(int i = 0; i < AssetsToPreload.Length; i++ )
        {
            var newObj = GameObject.Instantiate( AssetsToPreload[i].gameObject );
            StashObject( ref newObj );
        }   
    }
}
