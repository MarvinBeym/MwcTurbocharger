using MwcModApi.Parts;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBigIntercoolerTube : DerivablePart
	{
		protected override string partId => "turboBig-intercooler-tube";
		protected override string partName => "Racing Turbo Intercooler Tube";
		protected override Vector3 partInstallPosition => new Vector3(0f, 0f, 0f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		public TurboBigIntercoolerTube(Intercooler parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{ 
			AddClampModel(new Vector3(0.065f, -0.235f, -0.2475f),
				new Vector3(0, 90, -90), new Vector3(0.68f, 0.68f, 0.68f));
			AddScrew(new Screw(new Vector3(0.0645f, -0.2120f, -0.2730f),
				new Vector3(-90, 0, 0), Screw.Type.Normal, 0.4f, 8));
		}



	}
}