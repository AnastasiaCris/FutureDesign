// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System;

using Niantic.ARDK.Networking.HLAPI.Data;

using UnityEngine;

// TODO: comment

namespace Niantic.ARDK.Networking.HLAPI.Object.Unity
{
  [RequireComponent(typeof(AuthBehaviour))]
  public sealed class NetTransform :
    NetworkedBehaviour
  {
    [SerializeField]
    private TransformPiece _replicatedPieces = TransformPiece.All;

    private UnreliableBroadcastTransformPacker _transformPacker;

#pragma warning disable 0618
    protected override void SetupSession(out Action initializer, out int order)
    {
      initializer = () =>
      {
        var descriptor = Owner.Auth.AuthorityToObserverDescriptor(TransportType.UnreliableUnordered);
        _transformPacker = new UnreliableBroadcastTransformPacker
        (
          "NetTransform",
          gameObject.transform,
          descriptor,
          _replicatedPieces,
          Owner.Group
        );
      };

      order = 0;
    }
#pragma warning restore 0618
    
    private void OnDestroy()
    {
      _transformPacker?.Unregister();
    }
  }
}
