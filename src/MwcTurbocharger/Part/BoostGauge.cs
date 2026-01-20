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
		protected override Vector3 partInstallPosition => new Vector3(0f, 0f, 0f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);

		protected BoostGaugeLogic logic;

		public BoostGaugeLogic.GaugeMode gaugeMode => logic.gaugeMode;

		public BoostGauge(GamePart parent) : base(parent, MwcTurbocharger.partBaseInfo)
		{
			AddScrew(new Screw(new Vector3(0f, -0.0270f, 0.003f), new Vector3(-90, 0, 0),
				Screw.Type.Normal, 0.4f, 8));

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