using UnityEngine;

namespace DungeonGeneration {
    public class Room {
        public int X_Length { get; private set; }
        public int Z_Length { get; private set; }

        public Vector3 Position { get; set;
            //get {
            //    return GO.transform.position;
            //}
            //set {
            //    GO.transform.position = value;
            //}
        }

        //public GameObject GO;
        //private GameObject _BlockPrefab;


        public Room(/*GameObject blockPrefab, */int x_Length, int z_Length, Vector3 position, Transform parent/*, int index*/) {
            //_BlockPrefab = blockPrefab;
            //GO = new GameObject($"Room_{index}_({x_Length}x{z_Length})");
            //GO.transform.position = position;
            //GO.transform.parent = parent;
            Position = position;
            this.X_Length = x_Length;
            this.Z_Length = z_Length;
            //GenerateBlocks();
        }

        public Room(GameObject blockPrefab, int x_Length, int z_Length) {
            //_BlockPrefab = blockPrefab;
            //GO = new GameObject($"Room_({x_Length}x{z_Length})");
            //GO.transform.position = Vector3.zero;
            this.X_Length = x_Length;
            this.Z_Length = z_Length;
            //GenerateBlocks();
        }

        public void Destroy() {
            //Object.DestroyImmediate(GO);
            //GO = null;
        }

        public Vector3 LeftUpVert() {
            return new Vector3(Position.x - X_Length /** 3*/, 0, Position.z + Z_Length/* * 3*/);
        }
        public Vector3 RightDownVert() {
            return new Vector3(Position.x + X_Length /** 3*/, 0, Position.z - Z_Length/* * 3*/);
        }

        //public bool TooCloseTo(Room room) {
        //    return Position.x + X_Length * 3 > room.Position.x - room.X_Length * 3 || Position.z + Z_Length * 3 > room.Position.z - room.Z_Length * 3 ||
        //        Position.x - X_Length * 3 < room.Position.x + room.X_Length * 3 || Position.z - Z_Length * 3 < room.Position.z + room.Z_Length * 3;
        //}

        public bool Intersects(Room room) {
            var thisLeftUp = LeftUpVert();
            var thisRightDown = RightDownVert();
            var otherLeftUp = room.LeftUpVert();
            var otherRightDown = room.RightDownVert();
            
            var thisLeft = thisLeftUp.x;
            var thisUp = thisLeftUp.z;
            var thisRight = thisRightDown.x;
            var thisDown = thisRightDown.z;

            var otherLeft = otherLeftUp.x;
            var otherUp = otherLeftUp.z;
            var otherRight = otherRightDown.x;
            var otherDown = otherRightDown.z;

            return !(thisUp <=  otherDown || thisDown >= otherUp || thisRight <= otherLeft || thisLeft >= otherRight);
        }

        //private void GenerateBlocks() {
        //    for (int i = 0; i < X_Length; i++) {
        //        for (int j = 0; j < Z_Length; j++) {
        //            var block = Object.Instantiate(_BlockPrefab, GO.transform);
        //            block.name = ($"({i},{j}_block)");
        //            block.transform.localPosition = 
        //                new Vector3(
        //                i * 6 - X_Length * 3 + 3,
        //                0, 
        //                j * 6 - Z_Length * 3 + 3
        //                );
        //        }
        //    }
        //}
    }
}