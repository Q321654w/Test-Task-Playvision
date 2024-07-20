using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PhysicalTransformAnimationRecorder
{
    public static TransformAnimation[] CreateForObjects<T>
    (
        T[] objects,
        int physicFramesByAnimationFrame = 10,
        int keyBufferSize = 4096
    ) where T : MonoBehaviour
    {
        var length = objects.Length;
        var simulationDeltaTime = Time.fixedDeltaTime;
        
        var rigidbodies = CacheRigidbodies(objects);
        var keyBuffers = AllocateBuffers(length, keyBufferSize);

        var activeScene = SceneManager.GetActiveScene();
        var physicScene = activeScene.GetPhysicsScene();
        
        Simulate(physicFramesByAnimationFrame, keyBuffers, objects, physicScene, rigidbodies, simulationDeltaTime);

        RestoreObjectsState(objects, keyBuffers);

        var animations = CrateAnimationsFrom(length, keyBuffers, physicFramesByAnimationFrame, simulationDeltaTime);
        return animations;
    }

    private static void Simulate<T>
    (
        int physicFramesByAnimationFrame,
        List<TransformAnimationKey>[] keyBuffers,
        T[] objects,
        PhysicsScene physicScene,
        Rigidbody[] rigidbodies, 
        float simulationDeltaTime
    ) where T : MonoBehaviour
    {
        var isSimulationContinue = true;
        var iteration = 0;
        
        Physics.simulationMode = SimulationMode.Script;

        while (iteration % physicFramesByAnimationFrame != 0 || isSimulationContinue)
        {
            if (iteration % physicFramesByAnimationFrame == 0)
                WriteFrameTo(keyBuffers, objects);

            physicScene.Simulate(simulationDeltaTime);

            iteration++;

            if (iteration % physicFramesByAnimationFrame == 0)
                isSimulationContinue = IsSimulationContinue(rigidbodies);
        }

        Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    private static void RestoreObjectsState<T>(T[] objects, List<TransformAnimationKey>[] keys) where T : MonoBehaviour
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = keys[i][0].Position;
            objects[i].transform.rotation = keys[i][0].Rotation;
        }
    }

    private static TransformAnimation[] CrateAnimationsFrom
    (
        int length,
        List<TransformAnimationKey>[] keyBuffers, 
        int animationStepTime,
        float simulationDeltaTime
    )
    {
        var animations = new TransformAnimation[length];
        for (int i = 0; i < length; i++)
            animations[i] = new TransformAnimation(animationStepTime * simulationDeltaTime, keyBuffers[i].ToArray());

        return animations;
    }

    private static List<TransformAnimationKey>[] AllocateBuffers(int length, int keyBufferSize)
    {
        var keyBuffers = new List<TransformAnimationKey>[length];
        for (int i = 0; i < length; i++)
            keyBuffers[i] = new List<TransformAnimationKey>(keyBufferSize);

        return keyBuffers;
    }

    private static Rigidbody[] CacheRigidbodies<T>(T[] objects) where T : MonoBehaviour
    {
        var rigidbodies = new Rigidbody[objects.Length];
        for (int i = 0; i < objects.Length; i++)
            rigidbodies[i] = objects[i].GetComponent<Rigidbody>();

        return rigidbodies;
    }

    private static bool IsSimulationContinue(Rigidbody[] rigidbodies)
    {
        foreach (var rigidbody in rigidbodies)
            if (!rigidbody.IsSleeping())
                return true;

        return false;
    }

    private static void WriteFrameTo<T>(List<TransformAnimationKey>[] keyBuffers, T[] objects) where T : MonoBehaviour
    {
        for (int i = 0; i < objects.Length; i++)
        {
            var diceTransform = objects[i].transform;
            var buffer = keyBuffers[i];
            buffer.Add(new TransformAnimationKey(diceTransform.position, diceTransform.rotation));
        }
    }
}