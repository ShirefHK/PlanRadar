using System.IO;
using System.Threading.Tasks;
using GLTFast.Loading;
using UnityEngine;
namespace GLTFast
{
  
    public class GtflLOAD : GltfAssetBase
    {
        [Tooltip("URL to load the glTF from.")]
        public string url;

        [Tooltip("Automatically load at start.")]
        public bool loadOnStartup = true;

        [Tooltip("Override scene to load (-1 loads glTFs default scene)")]
        public int sceneId = -1;

        [Tooltip("If checked, url is treated as relative StreamingAssets path.")]
        public bool streamingAsset = false;

        /// <summary>
        /// Latest scene's instance.  
        /// </summary>
        public GameObjectInstantiator.SceneInstance sceneInstance { get; protected set; }

        public string FullUrl => streamingAsset
            ? Path.Combine(Application.streamingAssetsPath, url)
            : url;

        protected virtual async void Start()
        {
            if (loadOnStartup && !string.IsNullOrEmpty(url))
            {
                // Automatic load on startup
                await Load(FullUrl);
                GameManger.instance.success = true;
            }
        }
       
      public override async Task<bool> Load(
            string url,
            IDownloadProvider downloadProvider = null,
            IDeferAgent deferAgent = null,
            IMaterialGenerator materialGenerator = null,
            ICodeLogger logger = null
            )
        {
            logger = logger ?? new ConsoleLogger();
            var success = await base.Load(url, downloadProvider, deferAgent, materialGenerator, logger);
            if (success)
            {
                if (deferAgent != null) await deferAgent.BreakPoint();
                // Auto-Instantiate
                if (sceneId >= 0)
                {
             
                   
                    InstantiateScene(sceneId, logger);
              
                }
                else
                {
                    Instantiate(logger);
                }
            }
            return success;
        }

        protected override IInstantiator GetDefaultInstantiator(ICodeLogger logger)
        {
            return new GameObjectInstantiator(importer, transform, logger);
        }

        protected override void PostInstantiation(IInstantiator instantiator, bool success)
        {
            sceneInstance = (instantiator as GameObjectInstantiator).sceneInstance;
            base.PostInstantiation(instantiator, success);
        }

        /// <summary>
        /// Removes previously instantiated scene(s)
        /// </summary>
        public override void ClearScenes()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            sceneInstance = null;
        }
    }
}
