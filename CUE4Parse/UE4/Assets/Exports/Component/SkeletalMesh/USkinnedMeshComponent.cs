using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports.Component.SkeletalMesh;

public class USkinnedMeshComponent : UMeshComponent
{
    public FPackageIndex GetSkeletalMesh()
    {
        var skeletalMesh = GetSkeletalMesh("SkeletalMesh"); // deprecated in 5.1 so fallback below
        if (skeletalMesh.IsNull) skeletalMesh = GetSkeletalMesh("SkinnedAsset");

        return skeletalMesh;
    }

    public FPackageIndex GetSkeletalMesh(string parameterName)
    {
        var mesh = new FPackageIndex();
        var current = this;
        while (current is not null)
        {
            mesh = current.GetOrDefault(parameterName, new FPackageIndex());
            if (!mesh.IsNull) break;
            current = current.GetArchetype() as USkinnedMeshComponent;
        }

        return mesh;
    }

    public bool SetSkeletalMeshIfNull(FPackageIndex mesh)
    {
        if (GetSkeletalMesh().IsNull)
        {
            SetSkeletalMesh(mesh);
            return true;
        }
        return false;
    }

    public void SetSkeletalMesh(FPackageIndex mesh)
    {
        PropertyUtil.Set(this, "SkeletalMesh", mesh);
    }
}
