using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all the positions, rotations of every trackable objects.
/// </summary>
public class KeyFrame : IPoolable, IMementoOriginator
{
    public class TrackableObjectFrame : IPoolable, IMementoOriginator
    {

        [System.Serializable]
        public class MeasureFrame
        {
            [SerializeField] private int type;
            [SerializeField] private Vector3 value;

            public int Type => type;
            public Vector3 Value => value;

            public MeasureFrame(Measure measure)
            {
                if(measure is VectorMeasure)
                    type = (int)VectorMeasure.Type;
                else if (measure is PlaneMeasure)
                    type = (int)PlaneMeasure.Type;
                else if (measure is PointMeasure)
                    type = (int)PointMeasure.Type;
                else
                    type = (int)Measure.Type; //will throw an error.

                this.value = measure.Value;
            }
        }

        private static int NullIndex => -1;

        private int index;
        private Vector3 position;
        private Vector3 rotation;
        private List<MeasureFrame> measures;

        public virtual void SetUp(int index, TrackableObject trackable)
        {
            this.index = index;
            this.position = trackable.transform.position;
            this.rotation = trackable.transform.rotation.eulerAngles;

            int count = trackable.MeasuresReference.Count;
            this.measures = new List<MeasureFrame>(count);
            for (int i = 0; i < count; i++)
                this.measures.Add(new MeasureFrame(trackable.MeasuresReference[i]));

        }

        #region IPoolable

        public void OnPoppedFromPool()
        {
            index = NullIndex;
            position = Vector3.zero;
            rotation = Vector3.zero;
        }

        public void OnPushedToPool()
        {
            index = NullIndex;
            position = Vector3.zero;
            rotation = Vector3.zero;
        }

        #endregion

        #region IMementoOriginator

        [System.Serializable]
        public class Memento : IMemento
        {
            [SerializeField] private int index;
            [SerializeField] private Vector3 position;
            [SerializeField] private Vector3 rotation;
            [SerializeField] private List<MeasureFrame> measures;

            public int Index => index;
            public Vector3 Position => position;
            public Vector3 Rotation => rotation;
            public List<MeasureFrame> Measures => measures;

            public Memento(TrackableObjectFrame frame)
            {
                this.index = frame.index;
                this.position = frame.position;
                this.rotation = frame.rotation;
                this.measures = new List<MeasureFrame>(frame.measures);
            }
        }

        public IMemento SaveMemento() => new Memento(this);

        public void RestoreMemento(IMemento memento)
        {
            Memento m = (Memento)memento;
            this.rotation = m.Rotation;
            this.position = m.Position;
            this.index = m.Index;
        }
        #endregion

    }

    public class TrackableObjectFrameWithMeasures : TrackableObjectFrame
    {
     
    }

    private class FramePool : ObjectPool
    {
        public override void Fill(int amount = 1)
        {
            for(int i = 0; i < amount; i++)
                this.Push(new TrackableObjectFrame());
        }
    }

    private MainManager mainManager;

    private static int NullTime  => -1;
    private float time;

    private FramePool framePool;
    private List<TrackableObjectFrame> trackableObjects;

    public KeyFrame()
    {
        mainManager = MainManager.GetManager();
        trackableObjects = new List<TrackableObjectFrame>(mainManager.TrackableObjectManager.TrackableObjectsReference.Count);
        framePool = new FramePool();
    }


    public void SetUp(float time)
    {
        this.time = time;
        List<TrackableObject> trackableObjects = mainManager.TrackableObjectManager.TrackableObjectsReference;

        for (int i = 0; i < trackableObjects.Count; i++)
        {
            TrackableObjectFrame frame = (TrackableObjectFrame)framePool.Pop();
            frame.SetUp(i, trackableObjects[i]);
            this.trackableObjects.Add(frame);
        }

    }

    #region IPoolable

    public void OnPoppedFromPool() => TearDown();
    public void OnPushedToPool  () => TearDown();

    private void TearDown()
    {
        time = NullTime;        

        for(int i = trackableObjects.Count -1; i >= 0; i--)
        {
            framePool.Push(trackableObjects[i]);
        }

        trackableObjects.Clear();
    }

    #endregion

    #region IMementoOriginator

    [System.Serializable]
    public class Memento : IMemento
    {
        [SerializeField] private float time;
        [SerializeField] private List<TrackableObjectFrame.Memento> objects;

        public float Time => time;
        public List<TrackableObjectFrame.Memento> Objects => new List<TrackableObjectFrame.Memento>(objects);

        public Memento(KeyFrame keyFrame)
        {
            this.time = keyFrame.time;
            this.objects = new List<TrackableObjectFrame.Memento>(keyFrame.trackableObjects.Count);

            for (int i = 0; i < keyFrame.trackableObjects.Count; i++)
            {
                objects.Add((TrackableObjectFrame.Memento)keyFrame.trackableObjects[i].SaveMemento());
            }
        }
    }

    public IMemento SaveMemento() => new Memento(this);

    public void RestoreMemento(IMemento memento)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
