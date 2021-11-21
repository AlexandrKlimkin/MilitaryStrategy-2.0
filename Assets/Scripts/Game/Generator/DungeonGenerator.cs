using EditorUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DelaunayTriangulator;
using System.Linq;

namespace DungeonGeneration {
    public class DungeonGenerator : MonoBehaviour {
        public GameObject BlockPrefab;
        public int RoomsNumber;

        [Range(0, 1)]
        public float BigRoomChance; 

        public Vector2Int RoomXMaxMinSize;
        public Vector2Int RoomZMaxMinSize;

        public Vector2Int SmallRoomXMaxMinSize;
        public Vector2Int SmallRoomZMaxMinSize;

        public const int BLOCK_SIZE = 6;

        private List<Room> _Rooms;
        private List<Room> _BigRooms;

        private float _MinX;
        private float _MaxX;
        private float _MinZ;
        private float _MaxZ;

        Transform _Root;
        private List<Vertex> _Points;
        private List<Triad> _Triangles;

        public void GenerateDungeon() {
            ClearDungeon();
            var root = new GameObject("Rooms");
            root.transform.SetParent(transform);
            _Root = root.transform;
            EditorCoroutine.Start(CreateRoomsRoutine());
        }

        public void ClearDungeon() {
            if(_Root)
                DestroyImmediate(_Root.gameObject);
            _Rooms = new List<Room>();
            _BigRooms = new List<Room>();
            _Points = new List<Vertex>();
            _Triangles = new List<Triad>();
            _MinX = 0;
            _MaxX = 0;
            _MinZ = 0;
            _MaxZ = 0;
        }

        private IEnumerator CreateRoomsRoutine() {
            for (int i = 0; i < RoomsNumber; i++) {
                var sizeRand = Random.value;
                int minx;
                int maxx;
                int minz;
                int maxz;
                bool bigRoom = false;
                if(sizeRand > BigRoomChance) {
                    minx = SmallRoomXMaxMinSize.x;
                    maxx = SmallRoomXMaxMinSize.y;
                    minz = SmallRoomZMaxMinSize.x;
                    maxz = SmallRoomZMaxMinSize.y;
                }
                else {
                    minx = RoomXMaxMinSize.x;
                    maxx = RoomXMaxMinSize.y;
                    minz = RoomZMaxMinSize.x;
                    maxz = RoomZMaxMinSize.y;
                    bigRoom = true;
                }
                var x_length = Random.Range(minx, maxx);
                var z_length = Random.Range(minz, maxz);
                //var circleRand = Random.insideUnitCircle * 50f;
                var randX = Random.Range(-3, 4);
                var randY = Random.Range(-3, 4);
                var room = new Room(/*BlockPrefab, */x_length, z_length, new Vector3(randX, 0, randY), _Root/*, i*/);
                _Rooms.Add(room);
                if (bigRoom)
                    _BigRooms.Add(room);
                //Finding Overlaping rect
                var leftUp = room.LeftUpVert();
                var rightDown = room.RightDownVert();
                //_MinX = Mathf.Min(_MinX, leftUp.x);
                //_MaxZ = Mathf.Max(_MaxZ, leftUp.z);
                //_MaxX = Mathf.Max(_MaxX, rightDown.x);
                //_MinZ = Mathf.Min(_MinZ, rightDown.z);
                yield return null;
            }
            EditorCoroutine.Start(SeparateRooms());
        }

        private IEnumerator SeparateRooms() {
            var zeros = 0;
            Vector3[] vec = new Vector3[_Rooms.Count];
            while (zeros != vec.Length) {
                vec = new Vector3[_Rooms.Count];
                for (int i = 0; i < _Rooms.Count; i++) {
                    int n = 0;
                    vec[i] = Vector3.zero;
                    for (int j = 0; j < _Rooms.Count; j++) {
                        if (_Rooms[i].Intersects(_Rooms[j])) {
                            vec[i] += (_Rooms[j].Position - _Rooms[i].Position);
                            n++;
                        }
                    }
                    if (n > 0) {
                        Vector3 v = vec[i];
                        v.x = (v.x / n);
                        v.y = (v.y / n);
                        v.z = (v.z / n);
                        var norm = v.normalized;
                        vec[i] = new Vector3(Mathf.Round(norm.x), 0, Mathf.Round(norm.z));
                    }
                }
                for (int i = 0; i < _Rooms.Count; i++) {
                    _Rooms[i].Position -= vec[i];
                }
                for (int i = 0; i < vec.Length; i++) {
                    if (vec[i] == Vector3.zero)
                        zeros++;
                }
                yield return null;
            }
        }

        public void Triangulate() {
            Triangulator angulator = new Triangulator();
            _Points = _BigRooms.Select(_=> new Vertex(_.Position.x, _.Position.z)).ToList();
            _Triangles = angulator.Triangulation(_Points, false);
        }

        public class Node {
            public readonly Vertex Vertex;
            public readonly List<Vertex> Neighbours;

            public Node(Vertex vertex, List<Vertex> neighbours) {
                this.Vertex = vertex;
                this.Neighbours = neighbours;
            }
        }

        public class Edge {
            public readonly Node Node1;
            public readonly Node Node2;
            public readonly float Distance;

            public Edge(Node node1, Node node2) {
                this.Node1 = node1;
                this.Node2 = node2;
                Distance = Mathf.Sqrt(Mathf.Pow(Node1.Vertex.x - Node2.Vertex.x, 2) + Mathf.Pow(Node1.Vertex.y - Node2.Vertex.y, 2));
            }
        }

        private void CreateGraph() {

        }

        //Prim's algorithm
        public void MinimalTree() {
            var pointsLeft = _Points.ToList();
            var nodes = new List<Node>();
            var edges = new List<Edge>();

            while(pointsLeft.Count > 0) {
                var point = pointsLeft.First();
                var neighbours = new List<Vertex>();
                nodes.Add(new Node(point, neighbours));

            }
        }

        private void OnDrawGizmos() {
            if (_Rooms == null)
                return;
            foreach (var room in _Rooms) {
                var leftUp = room.LeftUpVert();
                var rightDown = room.RightDownVert();
                var leftDown = new Vector3(leftUp.x, 0, rightDown.z);
                var rightUp = new Vector3(rightDown.x, 0, leftUp.z);
                Handles.DrawSolidRectangleWithOutline(new Vector3[] {leftDown, leftUp, rightUp, rightDown}, new Color(0,0,0.5f,0.5f), new Color(0, 0, 1f, 1f));
            }
            Handles.color = Color.green;
            if (_Triangles != null && _BigRooms.Count > 2) {
                foreach (var triangle in _Triangles) {
                    Handles.DrawPolyLine(new Vector3[] { _BigRooms[triangle.a].Position, _BigRooms[triangle.b].Position, _BigRooms[triangle.c].Position });
                }
            }
            Handles.color = Color.red;
            foreach(var room in _BigRooms) {
                Handles.DrawSolidDisc(room.Position, Vector3.up, 0.5f);
            }
        }
    }
}