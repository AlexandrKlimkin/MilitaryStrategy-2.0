/*
THIS FILE IS PART OF Animation Instancing PROJECT
AnimationInstancing.cs - The core part of the Animation Instancing library

©2017 Jin Xiaoyu. All Rights Reserved.
*/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace AnimationInstancing {
    public class AnimationManager : AnimationInstancingReferenceHolder<AnimationManager> {
        // A container to storage all animations info within game object
        public class InstanceAnimationInfo {
            public List<AnimationInfo> listAniInfo;
            public ExtraBoneInfo extraBoneInfo;
        }

        private Dictionary<GameObject, InstanceAnimationInfo> m_animationInfo;

        private void Awake() {
            m_animationInfo = new Dictionary<GameObject, InstanceAnimationInfo>();
        }

        public InstanceAnimationInfo FindAnimationInfo(GameObject prefab, AnimationInstancing instance) {
            Debug.Assert(prefab != null);
            InstanceAnimationInfo info = null;
            if (m_animationInfo.TryGetValue(prefab, out info)) {
                return info;
            }
            return CreateAnimationInfoFromFile(prefab);
        }

        private InstanceAnimationInfo CreateAnimationInfoFromFile(GameObject prefab) {
            Debug.Assert(prefab != null);
            var asset = Resources.Load<TextAsset>($"AnimationTexture/{prefab.name}");
            BinaryReader reader = new BinaryReader(new MemoryStream(asset.bytes));
            InstanceAnimationInfo info = new InstanceAnimationInfo();
            info.listAniInfo = ReadAnimationInfo(reader);
            info.extraBoneInfo = ReadExtraBoneInfo(reader);
            AnimationInstancingMgr.Instance.ImportAnimationTexture(prefab.name, reader);
            m_animationInfo.Add(prefab, info);
            return info;
        }

        private List<AnimationInfo> ReadAnimationInfo(BinaryReader reader) {
            int count = reader.ReadInt32();
            List<AnimationInfo> listInfo = new List<AnimationInfo>();
            for (int i = 0; i != count; ++i) {
                AnimationInfo info = new AnimationInfo();
                //info.animationNameHash = reader.ReadInt32();
                info.animationName = reader.ReadString();
                info.animationNameHash = info.animationName.GetHashCode();
                info.animationIndex = reader.ReadInt32();
                info.textureIndex = reader.ReadInt32();
                info.totalFrame = reader.ReadInt32();
                info.fps = reader.ReadInt32();
                info.rootMotion = reader.ReadBoolean();
                info.wrapMode = (WrapMode) reader.ReadInt32();
                if (info.rootMotion) {
                    info.velocity = new Vector3[info.totalFrame];
                    info.angularVelocity = new Vector3[info.totalFrame];
                    for (int j = 0; j != info.totalFrame; ++j) {
                        info.velocity[j].x = reader.ReadSingle();
                        info.velocity[j].y = reader.ReadSingle();
                        info.velocity[j].z = reader.ReadSingle();

                        info.angularVelocity[j].x = reader.ReadSingle();
                        info.angularVelocity[j].y = reader.ReadSingle();
                        info.angularVelocity[j].z = reader.ReadSingle();
                    }
                }
                int evtCount = reader.ReadInt32();
                info.eventList = new List<AnimationEvent>();
                for (int j = 0; j != evtCount; ++j) {
                    AnimationEvent evt = new AnimationEvent();
                    evt.function = reader.ReadString();
                    evt.floatParameter = reader.ReadSingle();
                    evt.intParameter = reader.ReadInt32();
                    evt.stringParameter = reader.ReadString();
                    evt.time = reader.ReadSingle();
                    evt.objectParameter = reader.ReadString();
                    info.eventList.Add(evt);
                }
                listInfo.Add(info);
            }
            listInfo.Sort(new ComparerHash());
            return listInfo;
        }

        private ExtraBoneInfo ReadExtraBoneInfo(BinaryReader reader) {
            ExtraBoneInfo info = null;
            if (reader.ReadBoolean()) {
                info = new ExtraBoneInfo();
                int count = reader.ReadInt32();
                info.extraBone = new string[count];
                info.extraBindPose = new Matrix4x4[count];
                for (int i = 0; i != info.extraBone.Length; ++i) {
                    info.extraBone[i] = reader.ReadString();
                }
                for (int i = 0; i != info.extraBindPose.Length; ++i) {
                    for (int j = 0; j != 16; ++j) {
                        info.extraBindPose[i][j] = reader.ReadSingle();
                    }
                }
            }
            return info;
        }
    }
}
