using Assimp;
using OpenTK.Mathematics;

namespace WillowEngine.Graphics
{
    public class MeshAnimation
    {
        public bool ShouldLoop
        {
            get { return _loop; }
            set { _loop = true; }
        }

        public Matrix4[] BoneMatrices
        {
            get
            {
                return _boneMatrices;
            }
        }

        private readonly Dictionary<string, Matrix4> _nodeTransforms = [];
        private readonly Matrix4[] _boneMatrices;
        private float _animationTime = 0.0f;
        private readonly float _animationDuration = 0.0f;
        private bool _loop;
        private readonly Animation _animation;

        public MeshAnimation(Animation animation)
        {
            _animation = animation;
            _boneMatrices = new Matrix4[_animation.NodeAnimationChannelCount];
        }

        public void Update()
        {
            _animationTime += Time.DeltaTime;

            if (_loop)
            {
                if (_animationTime > _animationDuration)
                {
                    _animationTime = 0.0f;
                }
            }
            else
            {
                _animationTime = MathHelper.Clamp(_animationTime, 0.0f, _animationDuration);
            }

            _nodeTransforms.Clear();
            foreach (var channel in _animation.NodeAnimationChannels)
            {
                var nodeTransform = InterpolateNode(channel, _animationTime);
                _nodeTransforms[channel.NodeName] = nodeTransform;
            }

            int index = 0;
            foreach (Matrix4 matrix in _nodeTransforms.Values)
            {
                _boneMatrices[index] = matrix;
            }
        }

        private static Matrix4 InterpolateNode(NodeAnimationChannel channel, float time)
        {
            var prevPositionKey = channel.PositionKeys[0];
            var nextPositionKey = channel.PositionKeys[channel.PositionKeys.Count - 1];

            foreach (var key in channel.PositionKeys)
            {
                if (key.Time > time)
                {
                    nextPositionKey = key;
                    break;
                }
                prevPositionKey = key;
            }

            var startPosition = new Vector3(prevPositionKey.Value.X, prevPositionKey.Value.Y, prevPositionKey.Value.Z);
            var endPosition = new Vector3(nextPositionKey.Value.X, nextPositionKey.Value.Y, nextPositionKey.Value.Z);
            float positionFactor = (float)((time - prevPositionKey.Time) / (nextPositionKey.Time - prevPositionKey.Time));
            var interpolatedPosition = Vector3.Lerp(startPosition, endPosition, positionFactor);

            var prevRotationKey = channel.RotationKeys[0];
            var nextRotationKey = channel.RotationKeys[^1];

            foreach (var key in channel.RotationKeys)
            {
                if (key.Time > time)
                {
                    nextRotationKey = key;
                    break;
                }
                prevRotationKey = key;
            }

            var startRotation = new OpenTK.Mathematics.Quaternion(prevRotationKey.Value.X, prevRotationKey.Value.Y, prevRotationKey.Value.Z, prevRotationKey.Value.W);
            var endRotation = new OpenTK.Mathematics.Quaternion(nextRotationKey.Value.X, nextRotationKey.Value.Y, nextRotationKey.Value.Z, nextRotationKey.Value.W);
            float rotationFactor = (float)((time - prevRotationKey.Time) / (nextRotationKey.Time - prevRotationKey.Time));
            var interpolatedRotation = OpenTK.Mathematics.Quaternion.Slerp(startRotation, endRotation, rotationFactor);

            var prevScaleKey = channel.ScalingKeys[0];
            var nextScaleKey = channel.ScalingKeys[^1];

            foreach (var key in channel.ScalingKeys)
            {
                if (key.Time > time)
                {
                    nextScaleKey = key;
                    break;
                }
                prevScaleKey = key;
            }

            var startScale = new Vector3(prevScaleKey.Value.X, prevScaleKey.Value.Y, prevScaleKey.Value.Z);
            var endScale = new Vector3(nextScaleKey.Value.X, nextScaleKey.Value.Y, nextScaleKey.Value.Z);
            float scaleFactor = (float)((time - prevScaleKey.Time) / (nextScaleKey.Time - prevScaleKey.Time));
            var interpolatedScale = Vector3.Lerp(startScale, endScale, scaleFactor);

            var translationMatrix = Matrix4.CreateTranslation(interpolatedPosition);
            var rotationMatrix = Matrix4.CreateFromQuaternion(interpolatedRotation);
            var scaleMatrix = Matrix4.CreateScale(interpolatedScale);

            return scaleMatrix * rotationMatrix * translationMatrix;
        }
    }
}
