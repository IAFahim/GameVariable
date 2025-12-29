using System;

namespace Unity.Entities
{
    public interface IComponentData {}
    public interface IBufferElementData {}
    public interface IEnableableComponent {}
    
    public struct Entity : IEquatable<Entity>
    {
        public int Index;
        public int Version;
        public bool Equals(Entity other) => Index == other.Index && Version == other.Version;
    }

    public partial struct SystemState
    {
        public void RequireForUpdate<T>() where T : unmanaged, IComponentData {}
        public EntityManager EntityManager => new EntityManager();
    }

    public struct EntityManager 
    {
        public T GetComponentData<T>(Entity entity) where T : unmanaged, IComponentData => default;
        public void SetComponentData<T>(Entity entity, T data) where T : unmanaged, IComponentData {}
    }

    public partial interface ISystem 
    {
        void OnCreate(ref SystemState state);
        void OnUpdate(ref SystemState state);
        void OnDestroy(ref SystemState state);
    }

    public struct RefRW<T> where T : unmanaged
    {
        public ref T ValueRW => throw new NotImplementedException();
    }

    public struct RefRO<T> where T : unmanaged
    {
        public T ValueRO => throw new NotImplementedException();
    }

    public static class SystemAPI
    {
        public static float TimeDeltaTime => 0.016f;

        public static bool IsComponentEnabled<T>(Entity entity) where T : IEnableableComponent
        {
            return false; // Mock implementation
        }
        

        public static System.Collections.Generic.IEnumerable<DynamicBuffer<T1>> Query<T1>() 
            where T1 : unmanaged, IBufferElementData
        {
            yield break;
        }

        public static System.Collections.Generic.IEnumerable<(DynamicBuffer<T1>, RefRW<T2>, RefRW<T3>)> Query<T1, T2, T3>() 
            where T1 : unmanaged, IBufferElementData
            where T2 : unmanaged, IComponentData
            where T3 : unmanaged, IComponentData
        {
            yield break;
        }

        public static System.Collections.Generic.IEnumerable<(DynamicBuffer<T1>, RefRW<T2>, RefRW<T3>, Entity)> QueryWithEntity<T1, T2, T3>() 
             where T1 : unmanaged, IBufferElementData
             where T2 : unmanaged, IComponentData
             where T3 : unmanaged, IComponentData
        {
             yield break;
        }
    }

    public struct DynamicBuffer<T> : System.Collections.Generic.IEnumerable<T> where T : unmanaged
    {
        public bool IsEmpty => true;
        public void Clear() {}
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() { yield break; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { yield break; }
    }
}

namespace Unity.Mathematics
{
    public struct float3 
    {
        public float x, y, z;
        public float3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static readonly float3 zero = new float3(0, 0, 0);
    }
    
    public struct float2
    {
        public float x, y;
        public float2(float x, float y) { this.x = x; this.y = y; }
    }
}

namespace Unity.Burst
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method)]
    public class BurstCompileAttribute : Attribute {}
}
