using MwcModApi.Parts;
using MwcModApi.Tools;
using MwcTurbocharger.ModPart;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigBlowoffValve : DerivablePart
	{
		protected override string partId => "turboBig-blowoff-valve";
		protected override string partName => "Racing Turbo Blowoff Valve";
		protected override Vector3 partInstallPosition => new Vector3(0f, 0.086f, 0.154f);
		protected override Vector3 partInstallRotation => new Vector3(0, 90, 0);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public TurboBigBlowoffValve(TurboBigIntercoolerTube parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(0, -0.044f, 0), new Vector3(90, 90, 0), 0.48f, 10);

			boostChangingGameObject = gameObject.FindChild("turboBig-blowoff-valve-main").gameObject;
		}

		public GameObject boostChangingGameObject { get; protected set; }
	}
}