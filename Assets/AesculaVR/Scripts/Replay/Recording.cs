using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording : IMementoOriginator
{
    private float startTime;
    private List<KeyFrame> frames;

    public Recording()
    {
        startTime = Time.time;
        frames = new List<KeyFrame>(300);
    }

    //add a frame to the recording.
    public void AddFrame(KeyFrame keyFrame)
    {
        if (frames.Count + 1 >= frames.Capacity)
            frames.Capacity += 300;

        keyFrame.SetUp(Time.time - startTime);
        frames.Add(keyFrame);
    }

    //remove all frames from the recording.
    public void Clear(FramePool pool)
    {
        while (frames.Count > 0)
        {
            KeyFrame remove = frames[frames.Count - 1];
            frames.RemoveAt(frames.Count - 1);
            pool.Push(remove);
        }
    }

    #region IMementoOriginator

    [System.Serializable]
    public class Memento : IMemento
    {

        [SerializeField] private List<string> indexToPaths;
        [SerializeField] private List<KeyFrame.Memento> keyframes;

        public List<string> IndexToPaths => new List<string>(indexToPaths);
        public List<KeyFrame.Memento> Keyframes => new List<KeyFrame.Memento>(keyframes);

        public Memento(Recording recording)
        {
            MainManager manager = MainManager.GetManager();
            int count;

            //setup the index to the files.
            count = manager.TrackableObjectManager.TrackableObjectsReference.Count;
            indexToPaths = new List<string>(count);
            for (int i = 0; i < count; i++)
                indexToPaths.Add(manager.TrackableObjectManager.TrackableObjectsReference[i].SourceFile.Name());

            //setup the frames.
            count = recording.frames.Count;
            keyframes = new List<KeyFrame.Memento>(count);
            for (int i = 0; i < count; i++)
                keyframes.Add((KeyFrame.Memento)recording.frames[i].SaveMemento());
        }

    }

    public void RestoreMemento(IMemento memento)
    {
        throw new System.NotImplementedException();
    }

    public IMemento SaveMemento() => new Memento(this);

    #endregion
}

