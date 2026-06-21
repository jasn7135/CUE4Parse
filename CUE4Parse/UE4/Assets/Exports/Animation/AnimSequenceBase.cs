using System;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Animation
{
    public abstract class UAnimSequenceBase : UAnimationAsset
    {
        public float SequenceLength;
        public float RateScale;
        public FAnimNotifyEvent[] Notifies;
        //public FRawCurveTracks RawCurveData; Uncomment when you care about editor AnimSequence assets

        public override void Deserialize(FAssetArchive Ar, long validPos)
        {
            base.Deserialize(Ar, validPos);

            SequenceLength = GetOrDefault<float>(nameof(SequenceLength));
            RateScale = GetOrDefault(nameof(RateScale), 1.0f);
            Notifies = GetOrDefault(nameof(Notifies), Array.Empty<FAnimNotifyEvent>());
            //RawCurveData = GetOrDefault<FRawCurveTracks>(nameof(RawCurveData));

            PostSerializeRawCurveData(Ar);
        }

        private void PostSerializeRawCurveData(FAssetArchive Ar)
        {
            if (Ar.Game is EGame.GAME_SuicideSquad or EGame.GAME_DaysGone) return;
            if (FFrameworkObjectVersion.Get(Ar) >= FFrameworkObjectVersion.Type.SmartNameRefactor) return;
            if (Ar.Ver < EUnrealEngineObjectUE4Version.SKELETON_ADD_SMARTNAMES) return;
            if (GetOrDefault<FStructFallback>("RawCurveData") is { } rawCurveData &&
                rawCurveData.TryGet("FloatCurves", out FStructFallback[] floatCurves, []))
            {
                Ar.Position += floatCurves.Length * sizeof(ushort);
            }
        }
    }
}
