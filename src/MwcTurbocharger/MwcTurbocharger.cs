using HutongGames.PlayMaker;
using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.PaintingSystem;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcModApi.Parts.PartBox;
using MwcModApi.Shopping;
using MwcModApi.Tools;
using MwcTurbocharger.Gui;
using MwcTurbocharger.ModPart;
using MwcTurbocharger.Turbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace MwcTurbocharger
{
	public class MwcTurbocharger : Mod
	{
		public override string ID => "MwcTurbocharger";
		public override string Name => "Turbocharger";
		public override string Author => "DonnerPlays";
		public override string Version => "1.0";
		public override string Description => "A Turbocharger for the main car (corris)";
		public override MSCLoader.Game SupportedGames => MSCLoader.Game.MyWinterCar;

		public const string assetBundleName = "MwcTurbocharger.Assets.turbocharger.unity3d";

		//saveFiles
		private const string boostSaveFile = "boost_saveFile.json";

		public GuiDebug guiDebug;
		public Dictionary<string, float> boostSave;



		//Mod Settings
		public static SettingsCheckBox debugGuiSetting;
		public static SettingsCheckBox rotateTurbineSetting;
		public static SettingsCheckBox backfireEffectSetting;
		public static SettingsSliderInt turboVolumeSetting;
		public static SettingsSliderInt blowoffVolumeSetting;
		public static SettingsSliderInt backfireVolumeSetting;

		internal static PartBaseInfo partBaseInfo;
		internal static List<Part> partsList = new List<Part>();

		public AssetBundle assetsBundle;
		//ECU-Mod Communication
		private bool ecuModInstalled = false;

		//ModParts
		private Manifold manifold;
		private BoostGauge boostGauge;
		private Intercooler intercooler;
		private IntercoolerManifoldTube intercoolerManifoldTube;
		private ExhaustHeader exhaustHeader;
		private TurboBigIntercoolerTube turboBigIntercoolerTube;
		private TurboBigBlowoffValve turboBigBlowoffValve;
		private TurboBig turboBig;
		private TurboBigExhaustOutletTube turboBigExhaustOutletTube;

		//GameParts
		private GamePart weberCarb;
		private GamePart cylinderHead;

		private GamePart ceramicHeaders;
		private GamePart chromeHeaders;
		private GamePart exhaustManifold;
		private GamePart exhausHeaders;

		private GamePart dashboard;

		private Kit turboBigKit;
		private Kit ManifoldKit;


		public override void ModSetup()
		{
			SetupFunction(Setup.OnNewGame, OnNewGame);
			SetupFunction(Setup.OnLoad, () =>
			{
				ModConsole.Print($"{Name} [v{Version} started loading");
				OnLoad();
				ModConsole.Print($"{Name} [v{Version} finished loading");
			});
			SetupFunction(Setup.ModSettings, ModSettings);
			SetupFunction(Setup.Update, Update);
		}

		public void OnNewGame()
		{
			MwcModApi.MwcModApi.NewGameCleanUp(this);
			TurboPart.Save(this, boostSaveFile, new TurboPart[0]);
		}

		public void OnLoad()
		{
			
			Logger.InitLogger(this);

			ecuModInstalled = ModLoader.IsModPresent("DonnerTech_ECU_Mod");


			guiDebug = new GuiDebug(Screen.width - 310, 50, 300, "TURBO MOD DEBUG", new[]
			{
				new GuiDebugElement("DEBUG")
			});

			assetsBundle = Helper.LoadAssetBundle(this, "turbocharger.unity3d");
			TurboPart.LoadAssets(assetsBundle);
			partBaseInfo = new PartBaseInfo(this, assetsBundle, partsList);

			//GameParts
			weberCarb = new GamePart("VINP_Carburettor", "4 Barrell Racing Carb(VINXX)");
			cylinderHead = new GamePart("VINP_Cylinderhead", "Cylinder Head(VINX0)");

			ceramicHeaders = new GamePart("VINP_ExhaustManifold", "Ceramic Coated Headers(VINXX)");
			chromeHeaders = new GamePart("VINP_ExhaustManifold", "Chrome Headers(VINXX)");
			exhaustManifold = new GamePart("VINP_ExhaustManifold", "Exhaust Manifold(VINXX)");
			exhausHeaders = new GamePart("VINP_ExhaustManifold", "Exhaust Headers(VINXX)");

			dashboard = new GamePart("VINP_Dashboard", "Dashboard(VINXX)");

			try
			{
				boostSave = Helper.LoadSaveOrReturnNew<Dictionary<string, float>>(this, boostSaveFile);
			}
			catch (Exception ex)
			{
				Logger.New("Error while trying to deserialize save file", "Please check paths to save files", ex);
			}

			//ModParts
			//manifold = new Manifold(weberCarb);
			//boostGauge = new BoostGauge(dashboard);
			//intercooler = new Intercooler();
			//intercoolerManifoldTube = new IntercoolerManifoldTube(manifold);
			exhaustHeader = new ExhaustHeader(cylinderHead);
			//turboBigIntercoolerTube = new TurboBigIntercoolerTube(intercooler);
			//turboBigBlowoffValve = new TurboBigBlowoffValve(turboBigIntercoolerTube);
			/*
			turboBig = new TurboBig(
				this,
				boostGauge,
				turboBigBlowoffValve,
				exhaustHeader,
				boostSave
			);
			turboBigExhaustOutletTube = new TurboBigExhaustOutletTube(turboBig);

			TurboLogicRequiredParts turboBigRequiredParts = new TurboLogicRequiredParts();
			turboBigRequiredParts.Add(turboBig);
			turboBigRequiredParts.Add(turboBigExhaustOutletTube);
			turboBigRequiredParts.Add(turboBigBlowoffValve);
			turboBigRequiredParts.Add(turboBigIntercoolerTube);
			turboBigRequiredParts.Add(exhaustHeader);
			turboBigRequiredParts.Add(manifold);
			turboBigRequiredParts.Add(intercoolerManifoldTube);
			turboBigRequiredParts.Add(intercooler);
			turboBig.DefineRequiredParts(turboBigRequiredParts);

			turboBigKit = new Kit(
				"Turbocharger Kit",
				new Part[]
				{
					turboBig,
					turboBigIntercoolerTube,
					turboBigExhaustOutletTube,
				}
			);

			ManifoldKit = new Kit(
				"Weber Kit",
				new Part[]
				{
					manifold,
					intercoolerManifoldTube
				});
			*/

			SetupShopItems();
			SetupPartInstallBlocking();
			assetsBundle.Unload(false);
		}

		public void SetupShopItems()
		{
			var shopBaseInfo = new ShopBaseInfo(this, assetsBundle);
			var shopSpawnLocation = Shop.SpawnLocation.Fleetari.Counter;

			Shop shop = Shop.GetInstance();
			ShopLocation shopLocation = shop.GetShopLocation(ShopLocationOption.Fleetari);

			shop.Add(shopBaseInfo, shopLocation, new[]
			{
				//new ShopItem("Turbocharger Kit", 8100, shopSpawnLocation, turboBigKit),
				//new ShopItem("Turbocharger Blowoff Valve", 1350, shopSpawnLocation, turboBigBlowoffValve),
				//new ShopItem("Intercooler", 3000, shopSpawnLocation, intercooler),
				//new ShopItem("Boost Gauge", 180, shopSpawnLocation, boostGauge),
				new ShopItem("Turbocharger Exhaust Header", 2100, shopSpawnLocation, exhaustHeader),
			});
		}

		public void SetupPartInstallBlocking()
		{
			var gamePartExhaustHeaders = new[]
			{
				chromeHeaders,
				exhausHeaders,
				exhaustManifold,
				ceramicHeaders
			};
			exhaustHeader.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, gamePartExhaustHeaders);

			foreach (var gamePart in gamePartExhaustHeaders)
			{
				gamePart.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, exhaustHeader);
			}
		}

		public void ModSettings()
		{
			Settings.AddHeader("DEBUG");
			debugGuiSetting = Settings.AddCheckBox("debugGuiSetting", "Show DEBUG GUI");
			Settings.AddButton("Reset Part positions (uninstalled)", PosReset);
			Settings.AddHeader("Settings");

			rotateTurbineSetting = Settings.AddCheckBox("rotateTurbineSetting", "Allow turbo turbine rotation");
			backfireEffectSetting = Settings.AddCheckBox("backfireEffectSetting", "Allow backfire effect for turbo");

			Settings.AddHeader("Volume", Color.clear);
			turboVolumeSetting =
				Settings.AddSlider("turboVolumeSetting", "Turbo Sound Volume (loop)", 0, 200, 100);
			blowoffVolumeSetting =
				Settings.AddSlider("blowoffVolumeSetting", "Blowoff Sound Volume", 0, 200, 100);
			backfireVolumeSetting =
				Settings.AddSlider("backfireVolumeSetting ", "Backfire Sound Volume", 0, 200, 100);


			TransmissionHandler.SetupSettings(this);
			GearRatiosHandler.SetupSettings(this);

			/*
			Settings.AddHeader(this, "", Color.clear);
			boostBase = Settings.AddSlider(this,"boostBaseSetting", "Boost Base", 0, 2f, 0.8f);
			boostStartingRpm = Settings.AddSlider(this,"boostStartingRpmSetting", "Boost Starting Rpm", 0, 7000, 2400);
			boostStartingRpmOffset = Settings.AddSlider(this, "boostStartingRpmOffsetSetting", "Boost Starting Rpm Offset", 1000, 5000, 1000);
			boostMin = Settings.AddSlider(this,"boostMinSetting", "Boost Min", -0.2f, 1f, -0.04f);
			minSettableBoost = Settings.AddSlider(this,"minSettableBoostSetting", "Mit Settable Boost", 0.3f, 1.8f, 0.4f);
			boostSteepness = Settings.AddSlider(this,"boostSteepnessSetting", "Boost Steepness", 0.8f, 2f, 1f);
			blowoffDelay = Settings.AddSlider(this,"blowoffDelaySetting", "Blowoff Delay", 0.1f, 1.4f, 0.8f);
			blowoffTriggerBoost = Settings.AddSlider(this,"blowoffTriggerBoostSetting", "Blowoff Trigger Boost", 0.1f, 1f, 0.75f);
			backfireThreshold = Settings.AddSlider(this,"backfireThresholdSetting", "Backfire RPM Threshold", 1000, 6000, 5500f);
			backfireRandomRange = Settings.AddSlider(this,"backfireRandomRangeSetting", "Backfire Random Range", 1, 60, 20);
			rpmMultiplier = Settings.AddSlider(this,"rpmMultiplierSetting", "RPM Multiplier", 1, 30, 10f);
			extraPowerMultiplicator = Settings.AddSlider(this,"extraPowerMultiplicatorSetting", "Extra Power Multiplicator", 0.2f, 3f, 1.5f);
			boostSettingSteps = Settings.AddSlider(this, "boostSettingStepsSetting", "Boost Setting Steps", 0.01f, 0.2f, 0.05f);
			soundboostMinVolume = Settings.AddSlider(this,"soundboostMinVolumeSetting", "Soundboost Min Volume", 0.005f, 0.3f, 0.03f);
			soundboostMaxVolume = Settings.AddSlider(this,"soundboostMaxVolumeSetting", "Soundboost Max Volume", 0.01f, 0.5f, 0.08f);
			soundboostPitchMultiplicator = Settings.AddSlider(this,"soundboostPitchMultiplicatorSetting", "Soundboost Pitch Multiplicator", 0.5f, 8f, 5f);
			backfireDelay = Settings.AddSlider(this, "backfireDelaySetting", "Delay between a backfire trigger", 0.001f, 0.5f, 0.1f, null, 4);
			*/

			Settings.AddText( "Copyright © Marvin Beym 2020-2024");
		}

		public void Update()
		{
			TransmissionHandler.Handle();
			GearRatiosHandler.Handle();

			HandleExhaustSystem();
		}

		private void HandleExhaustSystem()
		{
			//ToDo: implement for MWC
		}


		private void PosReset()
		{
			try
			{
				//manifoldTwinCarb_kit.ResetToDefault();
				//Manifold_kit.ResetToDefault();
				//turboBig_kit.ResetToDefault();
				//turboSmall_kit.ResetToDefault();
				partsList.ForEach(delegate (Part part) { part.ResetToDefault(); });
			}
			catch (Exception ex)
			{
				Logger.New("Resetting positions failed", ex);
			}
		}
	}
}
