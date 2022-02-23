// using System.Collections;
// using System.Collections.Generic;
// using AssetBundleBrowser.AssetBundleModel;
// using UnityEngine;
//
// public class AssetBundlesCacheClearer : MonoBehaviour
// {
//     [Button]
//     public bool _ClearCache;
//     
//     public void OnClearCache()
//     {
//         ClearCache();
//     }
//
//     public void ClearCache()
//     {
//         var bundles = AssetBundle.GetAllLoadedAssetBundles();
//         foreach (var assetBundle in bundles)
//         {
//             assetBundle.Unload(true);
//         }
//
//         AssetBundleBrowser.AssetBundleModel.Model.DataSource.RemoveUnusedAssetBundleNames();
//         var pathes = AssetBundleBrowser.AssetBundleModel.Model.DataSource.GetAssetPathsFromAssetBundle("structures");
//         foreach (var path in pathes)
//         {
//             Debug.Log(path);   
//         }
//     }
// }
