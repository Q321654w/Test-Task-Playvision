using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TransformAnimationRecorder
{
    public static TransformAnimation[] CreateForObjects<T>(T[] objects,
        int physicFramesByAnimationFrame = 10,
        float simulationDeltaTime = 0.02f,
        int keyBufferSize = 4096,
        int maxFrameCount = 4096) where T : MonoBehaviour
    {
        var length = objects.Length;

        var rigidbodies = CacheRigidbodies(objects);
        var keyBuffers = AllocateBuffers(length, keyBufferSize);

        var activeScene = SceneManager.GetActiveScene();
        var physicScene = activeScene.GetPhysicsScene();

        var isSimulationContinue = true;
        var iteration = 0;

        Physics.simulationMode = SimulationMode.Script;

        while (iteration % physicFramesByAnimationFrame != 0 || iteration < maxFrameCount || isSimulationContinue)
        {
            if (iteration % physicFramesByAnimationFrame == 0)
                WriteFrameTo(keyBuffers, objects);

            physicScene.Simulate(simulationDeltaTime);

            iteration++;

            if (iteration % physicFramesByAnimationFrame == 0 && iteration >= maxFrameCount)
                isSimulationContinue = IsSimulationContinue(rigidbodies);
        }

        Physics.simulationMode = SimulationMode.FixedUpdate;

        RestoreObjectsState(objects, keyBuffers);

        var animations = CrateAnimationsFrom(length, keyBuffers, physicFramesByAnimationFrame, simulationDeltaTime);
        return animations;
    }

    private static void RestoreObjectsState<T>(T[] dices, List<TransformAnimationKey>[] keys) where T : MonoBehaviour
    {
        for (int i = 0; i < dices.Length; i++)
        {
            dices[i].transform.position = keys[i][0].Position;
            dices[i].transform.rotation = keys[i][0].Rotation;
        }
    }

    private static TransformAnimation[] CrateAnimationsFrom(int length,
        List<TransformAnimationKey>[] keyBuffers, 
        int animationStepTime,
        float simulationDeltaTime)
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

    private static Rigidbody[] CacheRigidbodies<T>(T[] dices) where T : MonoBehaviour
    {
        var rigidbodies = new Rigidbody[dices.Length];
        for (int i = 0; i < dices.Length; i++)
            rigidbodies[i] = dices[i].GetComponent<Rigidbody>();

        return rigidbodies;
    }

    private static bool IsSimulationContinue(Rigidbody[] rigidbodies)
    {
        foreach (var rigidbody in rigidbodies)
            if (!rigidbody.IsSleeping())
                return true;

        return false;
    }

    private static void WriteFrameTo<T>(List<TransformAnimationKey>[] keyBuffers, T[] dices) where T : MonoBehaviour
    {
        for (int i = 0; i < dices.Length; i++)
        {
            var diceTransform = dices[i].transform;
            var buffer = keyBuffers[i];
            buffer.Add(new TransformAnimationKey(diceTransform.position, diceTransform.rotation));
        }
    }
}