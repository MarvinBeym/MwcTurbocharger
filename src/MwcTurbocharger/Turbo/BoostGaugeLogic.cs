using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.Parts;
using MwcModApi.Tools;
using System;
using MwcTurbocharger.ModPart;
using MwcTurbocharger.Turbo;
using UnityEngine;

namespace MwcTurbocharger
{
	public class BoostGaugeLogic : MonoBehaviour
	{
		public enum GaugeMode
		{
			Analog,
			Digital,
		};

		private BoostGauge boostGauge;
		private GameObject analogDigitalSwitch;
		private GameObject analogNeedle;
		private Animation analogNeedleAnimation;
		private int selectedColor = 0;

		private Color[] availableColors = new Color[]
		{
			Color.white,
			Color.blue,
			Color.red,
			Color.green,
			Color.yellow,
			Color.magenta,
		};

		private Material foregroundMaterial;

		public GaugeMode gaugeMode = GaugeMode.Analog;

		private const float minAngle = 45;
		private const float maxAngle = 315;

		private float boostSaved = 0;
		public float time = 0;
		public float timeComparer = 0.01f;
		public float reducer = 0.15f;


		public void Init(BoostGauge boostGauge)
		{
			this.boostGauge = boostGauge;


			analogDigitalSwitch = this.transform.FindChild("boost-gauge-button").gameObject;

			analogNeedle = this.transform.FindChild("boost-gauge-needle").gameObject;

			if (analogNeedle == null) {
				Logger.Error("Failed to find analog needle mesh on boost gauge");
			}
			analogNeedleAnimation = analogNeedle.GetComponent<Animation>();

			if (analogNeedleAnimation == null) {
				Logger.Error("Failed to find analog needle animation component on analog needle mesh");
			}

			foreach (Material material in this.transform.FindChild("boost-gauge-main").GetComponent<Renderer>()
				         .materials) {
				if (!material.name.Contains("boost-gauge-foreground")) {
					continue;
				}

				foregroundMaterial = material;
				break;
			}

			foregroundMaterial.SetColor("_Color", availableColors[selectedColor]);



			try
			{

				Color color = Color.white;
				color.a = 0.2f;
				foregroundMaterial.SetColor("_Color", color);
			} catch (Exception ex) {
				Logger.Warning("Setup of boost gauge digital display failed", ex);
			}
		}

		void Start()
		{
			analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
			boostGauge.SetDigitalText("");
		}

		void Update()
		{
			if (!CarH.hasPower || !analogDigitalSwitch.IsLookingAt()) {
				return;
			}

			GaugeMode nextGaugeMode = gaugeMode == GaugeMode.Analog ? GaugeMode.Digital : GaugeMode.Analog;
			UserInteraction.GuiInteraction(
				string.Format(
					$"[Left mouse] or [{cInput.GetText("Use")}] to switch to {nextGaugeMode}\n" +
					$"[SCROLL UP/Down] to change color"
				)
			);

			if (UserInteraction.MouseScrollWheel.Up) {
				selectedColor += 1;
				ChangeTextColor(selectedColor);
			}

			if (UserInteraction.MouseScrollWheel.Down) {
				selectedColor -= 1;
				ChangeTextColor(selectedColor);
			}

			if (UserInteraction.UseButtonDown || UserInteraction.LeftMouseDown) {
				SwitchGaugeMode(nextGaugeMode);
			}
		}

		private void ChangeTextColor(int newColorIndex)
		{
			newColorIndex = newColorIndex > availableColors.Length - 1 ? 0 : newColorIndex;
			newColorIndex = newColorIndex < 0 ? availableColors.Length - 1 : newColorIndex;
			selectedColor = newColorIndex;
			Color color = availableColors[newColorIndex];
			color.a = 0.6f;

			foregroundMaterial.SetColor("_Color", color);
		}

		public void SwitchedElectricityOn()
		{
			if (gaugeMode == GaugeMode.Analog) {
				analogNeedleAnimation.Play();
			} else {
				boostGauge.SetDigitalText(0);
			}

			ChangeTextColor(selectedColor);
		}

		public void SwitchedElectricityOff()
		{
			if (analogNeedleAnimation.isPlaying) {
				analogNeedleAnimation.Stop();
			}

			analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
			boostGauge.SetDigitalText("");

			Color color = Color.white;
			color.a = 0.2f;
			foregroundMaterial.SetColor("_Color", color);
		}

		private void SwitchGaugeMode(GaugeMode newGaugeMode)
		{
			UserInteraction.PlayTouch(boostGauge.gameObject);
			gaugeMode = newGaugeMode;
			switch (gaugeMode) {
				case GaugeMode.Analog:
					boostGauge.SetDigitalText("");
					break;
				case GaugeMode.Digital:
					boostGauge.SetDigitalText(0);
					analogNeedle.transform.localEulerAngles = new Vector3(0, 0, minAngle);
					break;
			}
		}

		public void SetBoost(float target, float boost, TurboConfiguration turboConfig)
		{
			if (!CarH.hasPower || analogNeedleAnimation.isPlaying) {
				return;
			}

			if (boostGauge.error) {
				boostGauge.error = false;
			}

			switch (gaugeMode) {
				case GaugeMode.Analog:
					analogNeedle.transform.localEulerAngles = new Vector3(0, 0, GetNeedleAngle(boost));
					break;
				case GaugeMode.Digital:
					boostGauge.SetDigitalText(boost);
					break;
			}
		}

		private float GetNeedleAngle(float valueMap, float minMap = 0f, float maxMap = 3)
		{
			return minAngle + (maxAngle - minAngle) * valueMap.Map(minMap, maxMap, 0, 1);
		}
	}
}