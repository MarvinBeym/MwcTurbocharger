using HutongGames.PlayMaker;
using MSCLoader;
using MwcModApi.Caching;
using MwcModApi.PaintingSystem;
using MwcModApi.Parts;
using MwcModApi.Parts.EventSystem;
using MwcModApi.Parts.Game;
using MwcModApi.Shopping;
using MwcModApi.Tools;
using MwcTurbocharger.Gui;
using MwcTurbocharger.ModPart;
using MwcTurbocharger.Turbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using MwcModApi.Parts.PartBox;
using MwcTurbocharger.Modifier;
using UnityEngine;

namespace MwcTurbocharger
{
	public class MwcTurbocharger : Mod
	{
		public override string ID => "MwcTurbocharger";
		public override string Name => "Turbocharger";
		public override string Author => "DonnerPlays";
		public override string Version => "1.0.1";
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
		private RacingCarbManifold racingCarbManifold;
		private BoostGauge boostGauge;
		private Intercooler intercooler;
		private IntercoolerRacingCarbManifoldTube intercoolerRacingCarbManifoldTube;
		private ExhaustHeader exhaustHeader;
		private TurboBigIntercoolerTube turboBigIntercoolerTube;
		private TurboBigBlowoffValve turboBigBlowoffValve;
		private TurboBig turboBig;
		private TurboBigExhaustOutletTube turboBigExhaustOutletTube;

		//GameParts
		private GamePart carb;
		private GamePart twoBarrelCarb;
		private GamePart racingCarb;
		private GamePart cylinderHead;

		private GamePart ceramicHeaders;
		private GamePart chromeHeadersA;
		private GamePart chromeHeadersC;
		private GamePart exhaustManifold;
		private GamePart exhausHeaders;
		private GamePart exhaustPipeRear;
		private GamePart exhaustMuffler;

		private GamePart dashboard;

		private Kit turboBigKit;
		private Kit racingCarbManifoldKit;

		public override void ModSetup()
		{
			SetupFunction(Setup.OnNewGame, OnNewGame);
			SetupFunction(
				Setup.OnLoad, () =>
				{
					ModConsole.Print($"{Name} [v{Version}] started loading");
					OnLoad();
					ModConsole.Print($"{Name} [v{Version}] finished loading");
				}
			);
			SetupFunction(Setup.ModSettings, ModSettings);
			SetupFunction(Setup.Update, Update);
			SetupFunction(Setup.OnSave, OnSave);
			SetupFunction(Setup.OnGUI, OnGUI);
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

			guiDebug = new GuiDebug(
				Screen.width - 310, 50, 300, "TURBO MOD DEBUG", new[]
				{
					new GuiDebugElement("DEBUG")
				}
			);

			assetsBundle = Helper.LoadAssetBundle(assetBundleName);
			TurboPart.LoadAssets(assetsBundle);
			partBaseInfo = new PartBaseInfo(this, assetsBundle, partsList);

			//GameParts

			carb = new GamePart("VINP_Carburettor", "Carburettor(VINXX)");
			twoBarrelCarb = new GamePart("VINP_Carburettor", "2 Barrel Carb(VINXX)");
			racingCarb = new GamePart("VINP_Carburettor", "4 Barrell Racing Carb(VINXX)");

			cylinderHead = new GamePart("VINP_Cylinderhead", "Cylinder Head(VINX0)");
			ceramicHeaders = new GamePart("VINP_ExhaustManifold", "Ceramic Coated Headers(VINXX)");
			chromeHeadersA = new GamePart("VINP_ExhaustManifold", "Chrome Headers(VINXX)", "HEADERSa03");
			exhaustPipeRear = new GamePart("VINP_ExhaustRear", "Exhaust Pipe Rear(VINXX)");
			exhaustMuffler = new GamePart("VINP_ExhaustMuffler", "Exhaust Muffler(VINXX)");
			chromeHeadersC = new GamePart("VINP_ExhaustManifold", "Chrome Headers(VINXX)", "HEADERSc02");
			exhaustManifold = new GamePart("VINP_ExhaustManifold", "Exhaust Manifold(VINXX)");
			exhausHeaders = new GamePart("VINP_ExhaustManifold", "Exhaust Headers(VINXX)");

			dashboard = new GamePart("VINP_Dashboard", "Dashboard(VINXX)");

			try {
				boostSave = Helper.LoadSaveOrReturnNew<Dictionary<string, float>>(this, boostSaveFile);
			} catch (Exception ex) {
				Logger.Error("Error while trying to deserialize save file", "Please check paths to save files", ex);
			}

			//ModParts
			racingCarbManifold = new RacingCarbManifold(racingCarb);
			boostGauge = new BoostGauge(dashboard);
			intercooler = new Intercooler();
			intercoolerRacingCarbManifoldTube = new IntercoolerRacingCarbManifoldTube(racingCarbManifold);
			exhaustHeader = new ExhaustHeader(cylinderHead);

			turboBig = new TurboBig(
				this,
				boostGauge,
				exhaustHeader,
				boostSave
			);
			turboBigIntercoolerTube = new TurboBigIntercoolerTube(turboBig);
			turboBigExhaustOutletTube = new TurboBigExhaustOutletTube(turboBig);
			turboBigBlowoffValve = new TurboBigBlowoffValve(turboBigIntercoolerTube);
			turboBig.DefineBoostChangingGameObject(turboBigBlowoffValve.boostChangingGameObject);

			racingCarb.AddEventListener(PartEvent.Time.Post, PartEvent.Type.InstallOnCar, () =>
			{
				turboBig.conditionStorage.UpdateCondition("racingCarb", true);
			});
			racingCarb.AddEventListener(PartEvent.Time.Post, PartEvent.Type.UninstallFromCar, () =>
			{
				turboBig.conditionStorage.UpdateCondition("racingCarb", false);
			});

			TurboLogicRequiredParts turboBigRequiredParts = new TurboLogicRequiredParts();
			turboBigRequiredParts.Add(turboBig);
			turboBigRequiredParts.Add(turboBigExhaustOutletTube);
			turboBigRequiredParts.Add(turboBigBlowoffValve);
			turboBigRequiredParts.Add(turboBigIntercoolerTube);
			turboBigRequiredParts.Add(exhaustHeader);
			turboBigRequiredParts.Add(racingCarbManifold);
			turboBigRequiredParts.Add(intercoolerRacingCarbManifoldTube);
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

			racingCarbManifoldKit = new Kit(
				"Racing Carburetor Kit",
				new Part[]
				{
					racingCarbManifold,
					intercoolerRacingCarbManifoldTube
				}
			);

			SetupShopItems();
			SetupPartInstallBlocking();
			SetupExhaustSystem();
			assetsBundle.Unload(false);
		}

		public void SetupShopItems()
		{
			var shopBaseInfo = new ShopBaseInfo(this, assetsBundle);
			var shopSpawnLocation = Shop.SpawnLocation.Fleetari.Counter;
			
			Shop shop = Shop.GetInstance();
			ShopLocation shopLocation = shop.GetShopLocation(ShopLocationOption.Fleetari);

			shop.Add(
				shopBaseInfo, shopLocation, new[]
				{
					new ShopItem("Turbocharger Kit", 4000, shopSpawnLocation, turboBigKit, "turboBig-kit.png"),
					new ShopItem("Turbocharger Blowoff Valve", 350, shopSpawnLocation, turboBigBlowoffValve, "turboBig-blowoff-valve.png"),
					new ShopItem("Racing Carb Manifold Kit", 2000, shopSpawnLocation, racingCarbManifoldKit, "racingCarb-manifold-kit.png"),
					new ShopItem("Intercooler", 900, shopSpawnLocation, intercooler, "intercooler.png"),
					new ShopItem("Boost Gauge", 75, shopSpawnLocation, boostGauge, "boost-gauge.png"),
					new ShopItem("Turbocharger Exhaust Header", 1200, shopSpawnLocation, exhaustHeader, "exhaust-header.png"),
				}
			);
		}

		public void SetupPartInstallBlocking()
		{
			var gamePartExhaustHeaders = new[]
			{
				chromeHeadersA,
				chromeHeadersC,
				exhausHeaders,
				exhaustManifold,
				ceramicHeaders
			};
			exhaustHeader.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, gamePartExhaustHeaders);

			foreach (var gamePart in gamePartExhaustHeaders) {
				gamePart.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, exhaustHeader);
			}

			carb.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, racingCarbManifold);
			twoBarrelCarb.BlockOtherPartInstallOnEvent(PartEvent.Type.Install, racingCarbManifold);
		}

		public void ModSettings()
		{
			Settings.AddHeader("DEBUG");
			debugGuiSetting = Settings.AddCheckBox("debugGuiSetting", "Show DEBUG GUI");
			Settings.AddButton("Reset Part positions (uninstalled)", PosReset);
			Settings.AddHeader("Settings");

			rotateTurbineSetting = Settings.AddCheckBox("rotateTurbineSetting", "Allow turbo turbine rotation");
			//backfireEffectSetting = Settings.AddCheckBox("backfireEffectSetting", "Allow backfire effect for turbo");

			Settings.AddHeader("Volume", Color.clear);
			turboVolumeSetting =
				Settings.AddSlider("turboVolumeSetting", "Turbo Sound Volume (loop)", 0, 200, 100);
			blowoffVolumeSetting =
				Settings.AddSlider("blowoffVolumeSetting", "Blowoff Sound Volume", 0, 200, 100);
			//backfireVolumeSetting = Settings.AddSlider("backfireVolumeSetting ", "Backfire Sound Volume", 0, 200, 100);

			TransmissionHandler.SetupSettings(this);
            
			Settings.AddText("Copyright © Marvin Beym 2020-2026");
		}

		public void Update()
		{
			TransmissionHandler.Handle();
		}

		public void OnSave()
		{
			TurboPart.Save(this, boostSaveFile, new TurboPart[]
			{
				turboBig,
			});
		}

		public void OnGUI()
		{
			if (debugGuiSetting.GetValue())
			{
				TurboPart turboInstalled = null;
				if (turboBig.installed)
				{
					turboInstalled = turboBig;
				}

				guiDebug.Handle(new GuiDebugInfo[]
				{
					new GuiDebugInfo("DEBUG", "Engine RPM", ((int)CarH.drivetrain.rpm).ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Turbo pressure (bar)", turboInstalled == null ? "NOT INSTALLED" : turboInstalled.boost.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Turbo rpm", turboInstalled == null ? "NOT INSTALLED" : turboInstalled.rpm.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Turbo boost set", turboInstalled == null ? "NOT INSTALLED" : turboInstalled.setBoost.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Power multiplier", CarH.drivetrain.powerMultiplier.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "KM/H", ((int)CarH.drivetrain.differentialSpeed).ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Torque", CarH.drivetrain.torque.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "HP", CarH.drivetrain.currentPower.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Clutch Max Torque", CarH.drivetrain.clutchMaxTorque.ToStringOrEmpty()),
					new GuiDebugInfo("DEBUG", "Clutch Torque Multiplier",
						CarH.drivetrain.clutchTorqueMultiplier.ToStringOrEmpty()),
				});
			}
		}

		private void SetupExhaustSystem()
		{
			exhaustHeader.AddEventListener(PartEvent.Time.Post, PartEvent.Type.InstallOnCar, () =>
			{
				GameObject fireCylinderHead = Cache.Find("MuzzleCylHead");
				if (!fireCylinderHead)
				{
					return;
				}

				PlayMakerFSM fireCylinderAnimation = fireCylinderHead.FindFsm("Animate");
				if (fireCylinderAnimation == null)
				{
					return;
				}

				fireCylinderAnimation.enabled = false;
				fireCylinderHead.SetActive(false);
			});
			exhaustHeader.AddEventListener(PartEvent.Time.Post, PartEvent.Type.UninstallFromCar, () =>
			{
				GameObject fireCylinderHead = Cache.Find("MuzzleCylHead");
				if (!fireCylinderHead) {
					return;
				}
				
				PlayMakerFSM fireCylinderAnimation = fireCylinderHead.FindFsm("Animate");
				if (fireCylinderAnimation == null)
				{
					return;
				}

				fireCylinderAnimation.enabled = true;
				fireCylinderHead.SetActive(true);
			});

			GameObject exhaustSmoke = CarH.car.FindChild("ExhaustSmoke", true);
			GameObject exhaustFromEngine = Cache.Find("CORRIS/Simulation/ExhaustCorris/FromEngine");
			GameObject exhaustFromPipeRear = Cache.Find("CORRIS/Simulation/ExhaustCorris/FromPipeRear");
			GameObject exhaustFromMufflerRear = Cache.Find("CORRIS/Simulation/ExhaustCorris/FromMufflerRear");

			exhaustHeader.AddEventListener(PartEvent.Time.Post, PartEvent.Type.InstallOnCar, () =>
			{
				if (turboBigExhaustOutletTube.installedOnCar)
				{
					return;
				}
				exhaustSmoke.transform.SetParent(exhaustHeader.transform, false);
			});

			exhaustHeader.AddEventListener(PartEvent.Time.Post, PartEvent.Type.UninstallFromCar, () =>
			{
				exhaustSmoke.transform.SetParent(exhaustFromEngine.transform, false);
			});

			turboBigExhaustOutletTube.AddEventListener(PartEvent.Time.Post, PartEvent.Type.InstallOnCar, () =>
			{
				if (exhaustPipeRear.installedOnCar && !exhaustMuffler.installedOnCar) {
					exhaustSmoke.transform.SetParent(exhaustFromPipeRear.transform, false);
				} else if (exhaustMuffler.installedOnCar && exhaustPipeRear.installedOnCar) {
					exhaustSmoke.transform.SetParent(exhaustFromMufflerRear.transform, false);
				} else {
					exhaustSmoke.transform.SetParent(turboBigExhaustOutletTube.transform, false);
				}
			});

			turboBigExhaustOutletTube.AddEventListener(PartEvent.Time.Post, PartEvent.Type.UninstallFromCar, () =>
			{
				if (exhaustHeader.installedOnCar)
				{
					exhaustSmoke.transform.SetParent(exhaustHeader.transform, false);
					return;
				}

				exhaustSmoke.transform.parent.SetParent(exhaustFromEngine.transform, false);
			});
		}

		private void PosReset()
		{
			try {
				racingCarbManifoldKit.ResetToDefault();
				turboBigKit.ResetToDefault();
				partsList.ForEach(delegate(Part part) { part.ResetToDefault(); });
			} catch (Exception ex) {
				Logger.Warning("Resetting positions failed", ex);
			}
		}
	}
}