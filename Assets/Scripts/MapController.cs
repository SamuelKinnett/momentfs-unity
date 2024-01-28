using Assets.Scripts.Extensions;
using Assets.Scripts.Objects;
using MomenTFS;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapController : MonoBehaviour
    {
        private RoomData roomData;
        private Camera camera;
        private CollisionMeshController collisionMeshController;
        private FlatCollisionGrid flatCollisionGrid;
        private BackgroundController backgroundController;

        public GameObject CollisionMesh;
        public GameObject FlatMesh;
        public GameObject BackgroundController;

        public String mapFilepath;

        public Vector3 cameraOrigin;
        public Vector3 cameraTarget;

        public float zoom;
        public float stretchX = 1.0f;
        public float stretchY = 1.0f;

        private float aspectRatio;

        // Start is called before the first frame update
        void Start() {
            collisionMeshController = CollisionMesh.GetComponent<CollisionMeshController>();
            flatCollisionGrid = FlatMesh.GetComponent<FlatCollisionGrid>();
            backgroundController = BackgroundController.GetComponent<BackgroundController>();

            roomData = new RoomReader().ReadRoomData($"{mapFilepath}.TFS", $"{mapFilepath}.MAP");
            camera = Camera.main;

            cameraOrigin = (roomData.MAPData.Settings.CameraOrigin.ToVector3() / 100f);

            Debug.Log(cameraOrigin);
            Debug.Log(cameraTarget);

            camera.transform.position = cameraOrigin;
            camera.transform.LookAt(new Vector3(0, 0, 0));
            zoom = roomData.MAPData.Settings.Zoom / 4096f / 10f;

            backgroundController.Initialise(roomData);

            var collisionMapData
                = new CollisionMapData(roomData.MAPData.Collision.CollisionMap, 3);

            float collisionMapSize = 10f * zoom;

            aspectRatio = 1.0f;
            flatCollisionGrid.Generate(collisionMapSize, aspectRatio, roomData.MAPData.Collision.CollisionMap);
        }

        public void OnDrawGizmos() {
            Gizmos.DrawSphere(cameraOrigin, 1.0f);
            Gizmos.DrawSphere(cameraTarget * stretchX, 1.0f);
            Gizmos.DrawLine(cameraOrigin, cameraTarget * stretchX);
        }

        // Update is called once per frame
        void Update() {
            camera.transform.position = cameraOrigin;
            camera.transform.LookAt(new Vector3(0, 0, 0));

            float collisionMapSize = 10f * zoom;
            flatCollisionGrid.Generate(collisionMapSize, aspectRatio, roomData.MAPData.Collision.CollisionMap);
        }
    }
}
