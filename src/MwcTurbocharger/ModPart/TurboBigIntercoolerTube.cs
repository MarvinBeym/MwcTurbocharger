using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigIntercoolerTube : DerivablePart
	{
		protected override string partId => "turboBig-intercooler-tube";
		protected override string partName => "Racing Turbo Intercooler Tube";
		protected override Vector3 partInstallPosition => new Vector3(-0.13627f, 0.065891f, -0.25011f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		public TurboBigIntercoolerTube(TurboBig parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{ 
			AddClampModel(new Vector3(0.12f, -0.031f, -0.239f), new Vector3(0, 90, 0), 0.5f, 10);
		}
	}
}