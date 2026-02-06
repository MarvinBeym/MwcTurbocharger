using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSCLoader;
using MwcModApi.PaintingSystem;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Tools;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger.ModPart
{
	public class TurboBig : TurboPart
	{
		private readonly TurboBigBlowoffValve turboBigBlowoffValve;

		protected override string partName => "Racing Turbo";
		protected override string partId => "turboBig";
		protected override Vector3 partInstallPosition => new Vector3(-0.156245f, -0.08874601f, 0.0885f);
		protected override Vector3 partInstallRotation => new Vector3(0, 0, 0);
		protected override DisableCollision disableCollisionWhenInstalled => DisableCollision.InstalledOnParent;

		public TurboBig(
			MwcTurbocharger mod,
			BoostGauge boostGauge,
			Part parent,
			Dictionary<string, float> boostSave
		) : base(mod, boostGauge, parent, boostSave)
		{
			AddScrews(
				new[]
				{
					new Screw(new Vector3(0.077f, 0.0218f, 0.0295f), new Vector3(45, -90, 0), Screw.Type.Normal),
					new Screw(new Vector3(0.077f, 0.0218f, 0.092f), new Vector3(45, -90, 0), Screw.Type.Normal),
					new Screw(new Vector3(0.112f, -0.0132f, 0.092f), new Vector3(45, -90, 0), Screw.Type.Normal),
					new Screw(new Vector3(0.112f, -0.0132f, 0.0295f), new Vector3(45, -90, 0), Screw.Type.Normal),
				}, 0.85f
			);

			//PaintingSystem.Setup(partBaseInfo.mod, this, gameObject.FindChild("turboBig-center").FindChild("turboBig-compressor-turbine").gameObject);

			DefineSpinningTurbineGameObject(gameObject.transform.FindChild("compressor-turbine").gameObject);

			audioHandler.Add("turboLoop", this, "turbocharger_loop.wav", this.partBaseInfo.assetBundle, PartEvent.Type.InstallOnCar, true);
			audioHandler.Add("grinding", this, "grinding.wav", this.partBaseInfo.assetBundle, PartEvent.Type.InstallOnCar, true);
			audioHandler.Add("blowoff", this, "blowoff_valve.wav", this.partBaseInfo.assetBundle, PartEvent.Type.InstallOnCar);
		}

		protected override TurboConfiguration SetupTurboConfig()
		{
			return new TurboConfiguration()
			{
				boostBase = 1f,
				boostOffset = 0.15f,
				boostStartingRpm = 2200,
				boostStartingRpmOffset = 2000,
				minSettableBoost = 0.6f,
				boostSteepness = 1.3f,
				blowoffDelay = 0.2f,
				blowoffTriggerBoost = 0.6f,
				backfireThreshold = 4000,
				backfireRandomRange = 20,
				rpmMultiplier = 10,
				extraPowerMultiplicator = 1.5f,
				boostSettingSteps = 0.05f,
				soundboostMinVolume = 0.03f,
				soundboostMaxVolume = 0.08f,
				soundboostPitchMultiplicator = 3.5f,
				backfireDelay = 0.05f,
			};
		}

		protected override TurboConditionStorage SetupTurboConditions()
		{
			TurboConditionStorage turboConditionStorage = new TurboConditionStorage();
			turboConditionStorage.AddConditions(
				new Condition[]
				{
					new Condition("racingCarb", 0.5f)
				}
			);
			return turboConditionStorage;
		}
	}
}