using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigIntercoolerTube : DerivablePart
	{
		protected override string partId => "turboBig-intercooler-tube";
		protected override string partName => "Turbocharger Intercooler Tube";
		protected override Vector3 partInstallPosition => new Vector3(-0.13627f, 0.065891f, -0.25011f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public TurboBigIntercoolerTube(TurboBig parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddClampModel(new Vector3(0.12f, -0.031f, -0.23935f), new Vector3(0, 90, 0), 0.51f, 6);
			AddClampModel(new Vector3(0.078f, 0.019f, 0.19265f), new Vector3(10, 90, 0), 0.7f, 6);
		}
	}
}