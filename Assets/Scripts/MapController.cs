using Assets.Scripts.Extensions;
using Assets.Scripts.Objects;
using MomenTFS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            cameraOrigin = (roomData.MAPData.Settings.CameraOrigin.ToVector3() / 10f);
            cameraTarget = (roomData.MAPData.Settings.CameraTranslation.ToVector3() / 10f);

            Debug.Log(cameraOrigin);
            Debug.Log(cameraTarget);

            camera.transform.position = cameraOrigin;
            camera.transform.LookAt(cameraTarget);
            zoom = roomData.MAPData.Settings.Zoom;
            //camera.transform.position += ((cameraTarget - cameraOrigin).normalized * (roomData.MAPData.Settings.Zoom)) / 10f;
            //camera.transform.rotation = Quaternion.Euler(roomData.MAPData.Settings.CameraTranslation.ToVector3());

            backgroundController.Initialise(roomData);

            var collisionMapData
                = new CollisionMapData(roomData.MAPData.Collision.CollisionMap, 3);
            //collisionMesh.transform.position = new Vector3(-100, 0, -100);
            //collisionMeshController.Initialise(100, 10, 100, 2);
            //collisionMeshController.GenerateMesh(collisionMapData);

            flatCollisionGrid.Generate(-(zoom / 2.0f), -(zoom / 2.0f), zoom / 100f, roomData.MAPData.Collision.CollisionMap);
            aspectRatio = roomData.TFSData.ImageSize.X / (float)roomData.TFSData.ImageSize.Y;
        }

        public void OnDrawGizmos() {
            Gizmos.DrawSphere(cameraOrigin, 1.0f);
            Gizmos.DrawSphere(cameraTarget * stretchX, 1.0f);
            Gizmos.DrawLine(cameraOrigin, cameraTarget * stretchX);
        }

        // Update is called once per frame
        void Update() {
            camera.transform.position = cameraOrigin;
            camera.transform.LookAt(cameraTarget * stretchX);

            flatCollisionGrid.transform.localScale = new Vector3(stretchX, 1, stretchX);
        }
    }
}
