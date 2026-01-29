using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class BoostGauge : DerivablePart
	{
		protected override string partId => "boost-gauge";
		protected override string partName => "Boost Gauge";
		protected override Vector3 partInstallPosition => new Vector3(0.13f, -0.141f, 0.165f);
		protected override Vector3 partInstallRotation => new Vector3(80, 180, 180);

		protected BoostGaugeLogic logic;

		public BoostGaugeLogic.GaugeMode gaugeMode => logic.gaugeMode;

		public BoostGauge(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddScrew(new Screw(new Vector3(0, -0.0369f, 0), new Vector3(-90, 0, 0), 0.4f, 10, Screw.Type.Normal, Screw.transformStep / 2));

			logic = AddEventBehaviour<BoostGaugeLogic>(PartEvent.Type.InstallOnCar);
			logic.Init(this);
		}

		public void SetBoost(float boostGaugeTarget, float boostBeforeRelease, TurboConfiguration config)
		{
			logic.SetBoost(boostGaugeTarget, boostBeforeRelease, config);
		}

		public void SetDigitalText(string text)
		{
			logic.SetDigitalText(text);
		}
	}
}