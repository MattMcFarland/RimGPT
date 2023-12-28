using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimGPT
{
	public class RimGPTSettings : ModSettings
	{
		public List<Persona> personas =
		[
			new Persona()
			{
				name = "Tynan",
				azureVoiceLanguage = "English",
				azureVoice = "en-AU-TimNeural",
				azureVoiceStyle = "",
				azureVoiceStyleDegree = 1f,
				speechRate = 0.2f,
				speechPitch = -0.1f,
				phrasesLimit = 20,
				phraseBatchSize = 20,
				phraseDelayMin = 30f,
				phraseDelayMax = 60f,
				phraseMaxWordCount = 48,
				historyMaxWordCount = 800,
				personality = "You invented Rimworld. You will inform the player about what they should do next. You never talk to or about Brrainz. You soley address the player directly.",
				personalityLanguage = "English"
			},
			new Persona()
			{
				name = "Brrainz",
				azureVoiceLanguage = "English",
				azureVoice = "en-US-JasonNeural",
				azureVoiceStyle = "whispering",
				azureVoiceStyleDegree = 1.25f,
				speechRate = 0.2f,
				speechPitch = -0.1f,
				phrasesLimit = 20,
				phraseBatchSize = 20,
				phraseDelayMin = 25f,
				phraseDelayMax = 55f,
				phraseMaxWordCount = 24,
				historyMaxWordCount = 200,
				personality = "You are a mod developer. You mostly respond to Tynan but sometimes talk to the player. You are sceptical about everything Tynan says. You support everything the player does in the game.",
				personalityLanguage = "English"
			}
		];
		public string ChatGPTModelPrimary = Tools.chatGPTModels.First();
		public string ChatGPTModelSecondary = Tools.chatGPTModels.First();
		public int ModelSwitchRatio = 10;
		public bool UseSecondaryModel = false;
		public bool enabled = true;
		public string chatGPTKey = "";

		public string azureSpeechKey = "";
		public string azureSpeechRegion = "";
		public float speechVolume = 4f;
		public bool showAsText = true;
		public long charactersSentOpenAI = 0;
		public long charactersSentAzure = 0;

		// for backwards compatibility --------
		public string azureVoiceLanguage;
		public string azureVoice;
		public string azureVoiceStyle;
		public float azureVoiceStyleDegree = 0;
		public float speechRate = 0;
		public float speechPitch = 0;
		public int phrasesLimit = 0;
		public int phraseBatchSize = 0;
		public float phraseDelayMin = 0;
		public float phraseDelayMax = 0;
		public int phraseMaxWordCount = 0;
		public int historyMaxWordCount = 0;
		public string personality;
		public string personalityLanguage;

		// reporting settings

		// Power Insight settings
		public bool reportEnergyStatus = true;
		public int reportEnergyFrequency = 8000;
		public bool reportEnergyImmediate = false;

		// Research Insight settings
		public bool reportResearchStatus = true;
		public int reportResearchFrequency = 60000;
		public bool reportResearchImmediate = false;

		// Thoughts & Mood Insight settings
		public bool reportColonistThoughts = true;
		public int reportColonistThoughtsFrequency = 60000;
		public bool reportColonistThoughtsImmediate = false;

		// Interpersonal Insight settings
		public bool reportColonistOpinions = false; // Initially disabled
		public int reportColonistOpinionsFrequency = 60000;
		public bool reportColonistOpinionsImmediate = false;

		// Detailed Colonist Insight settings
		public bool reportColonistRoster = false; // Initially disabled
		public int reportColonistRosterFrequency = 60000;
		public bool reportColonistRosterImmediate = false;

		// Rooms Insight settings
		public bool reportRoomStatus = false; // Initially disabled
		public int reportRoomStatusFrequency = 60000;
		public bool reportRoomStatusImmediate = false;
		// ------------------------------------

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look(ref personas, "personas", LookMode.Deep);
			Scribe_Values.Look(ref enabled, "enabled", true);
			Scribe_Values.Look(ref chatGPTKey, "chatGPTKey");
			Scribe_Values.Look(ref UseSecondaryModel, "UseSecondaryModel", defaultValue: false);
			Scribe_Values.Look(ref ModelSwitchRatio, "ModelSwitchRatio", defaultValue: 10);
			Scribe_Values.Look(ref ChatGPTModelPrimary, "ChatGPTModelPrimary", Tools.chatGPTModels.First());
			Scribe_Values.Look(ref ChatGPTModelSecondary, "ChatGPTModelSecondary", Tools.chatGPTModels.First());

			Scribe_Values.Look(ref azureSpeechKey, "azureSpeechKey");
			Scribe_Values.Look(ref azureSpeechRegion, "azureSpeechRegion");
			Scribe_Values.Look(ref speechVolume, "speechVolume", 4f);
			Scribe_Values.Look(ref showAsText, "showAsText", true);

			// Power Insight settings
			Scribe_Values.Look(ref reportEnergyStatus, "reportEnergyStatus", defaultValue: true);
			Scribe_Values.Look(ref reportEnergyFrequency, "reportEnergyFrequency", defaultValue: 8000);
			Scribe_Values.Look(ref reportEnergyImmediate, "reportEnergyImmediate", defaultValue: false);

			// Research Insight settings
			Scribe_Values.Look(ref reportResearchStatus, "reportResearchStatus", defaultValue: true);
			Scribe_Values.Look(ref reportResearchFrequency, "reportResearchFrequency", defaultValue: 60000);
			Scribe_Values.Look(ref reportResearchImmediate, "reportResearchImmediate", defaultValue: false);

			// Thoughts & Mood Insight settings
			Scribe_Values.Look(ref reportColonistThoughts, "reportColonistThoughts", defaultValue: true);
			Scribe_Values.Look(ref reportColonistThoughtsFrequency, "reportColonistThoughtsFrequency", defaultValue: 60000);
			Scribe_Values.Look(ref reportColonistThoughtsImmediate, "reportColonistThoughtsImmediate", defaultValue: false);

			// Interpersonal Insight settings
			Scribe_Values.Look(ref reportColonistOpinions, "reportColonistOpinions", defaultValue: false);
			Scribe_Values.Look(ref reportColonistOpinionsFrequency, "reportColonistOpinionsFrequency", defaultValue: 60000);
			Scribe_Values.Look(ref reportColonistOpinionsImmediate, "reportColonistOpinionsImmediate", defaultValue: false);

			// Detailed Colonist Insight settings
			Scribe_Values.Look(ref reportColonistRoster, "reportColonistRoster", defaultValue: false);
			Scribe_Values.Look(ref reportColonistRosterFrequency, "reportColonistRosterFrequency", defaultValue: 60000);
			Scribe_Values.Look(ref reportColonistRosterImmediate, "reportColonistRosterImmediate", defaultValue: false);

			// Rooms Insight settings
			Scribe_Values.Look(ref reportRoomStatus, "reportRoomStatus", defaultValue: false);
			Scribe_Values.Look(ref reportRoomStatusFrequency, "reportRoomStatusFrequency", defaultValue: 60000);
			Scribe_Values.Look(ref reportRoomStatusImmediate, "reportRoomStatusImmediate", defaultValue: false);


			// for backwards compatibility ---------------------------------------------
			Scribe_Values.Look(ref azureVoiceLanguage, "azureVoiceLanguage", "-");
			Scribe_Values.Look(ref azureVoice, "azureVoice", "en-CA-LiamNeural");
			Scribe_Values.Look(ref azureVoiceStyle, "azureVoiceStyle", "default");
			Scribe_Values.Look(ref azureVoiceStyleDegree, "azureVoiceStyleDegree", 1f);
			Scribe_Values.Look(ref speechRate, "speechRate", 0f);
			Scribe_Values.Look(ref speechPitch, "speechPitch", 0f);
			Scribe_Values.Look(ref phrasesLimit, "phrasesLimit", 20);
			Scribe_Values.Look(ref phraseBatchSize, "phraseBatchSize", 20);
			Scribe_Values.Look(ref phraseDelayMin, "phraseDelayMin", 10f);
			Scribe_Values.Look(ref phraseDelayMax, "phraseDelayMax", 20f);
			Scribe_Values.Look(ref phraseMaxWordCount, "phraseMaxWordCount", 40);
			Scribe_Values.Look(ref historyMaxWordCount, "historyMaxWordCount", 200);
			Scribe_Values.Look(ref personality, "personality", AI.defaultPersonality);
			Scribe_Values.Look(ref personalityLanguage, "personalityLanguage", "-");
			// -------------------------------------------------------------------------

			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (azureVoice != null && personas.NullOrEmpty())
				{
					ChatGPTModelPrimary = Tools.chatGPTModels.First();
					personas ??= [];
					personas.Add(new Persona()
					{
						name = "Default",
						azureVoiceLanguage = azureVoiceLanguage,
						azureVoice = azureVoice,
						azureVoiceStyle = azureVoiceStyle,
						azureVoiceStyleDegree = azureVoiceStyleDegree,
						speechRate = speechRate,
						speechPitch = speechPitch,
						phrasesLimit = phrasesLimit,
						phraseBatchSize = phraseBatchSize,
						phraseDelayMin = phraseDelayMin,
						phraseDelayMax = phraseDelayMax,
						phraseMaxWordCount = phraseMaxWordCount,
						historyMaxWordCount = historyMaxWordCount,
						personality = personality,
						personalityLanguage = personalityLanguage
					});
				}
			}
		}

		public bool IsConfigured =>
			 chatGPTKey?.Length > 0 && ((azureSpeechKey?.Length > 0 && azureSpeechRegion?.Length > 0) || showAsText);

		private Vector2 scrollPosition = Vector2.zero;
		public static Persona selected = null;
		public static int selectedIndex = -1;
		public static readonly Color listBackground = new(32 / 255f, 36 / 255f, 40 / 255f);
		public static readonly Color highlightedBackground = new(74 / 255f, 74 / 255f, 74 / 255f, 0.5f);
		public static readonly Color background = new(74 / 255f, 74 / 255f, 74 / 255f);

		public void DoWindowContents(Rect inRect)
		{
			string prevKey;
			Rect rect;

			float spacing = 20f; // Alternatively adjust the spacing if needed
			float totalSpaceBetweenColumns = spacing * 2;

			// New calculation for column widths
			float slimColumnFactor = 0.5f; // Middle column is 50% of the others
			float availableSpace = inRect.width - totalSpaceBetweenColumns; // Total available width minus the spacing
			float normalColumnWidth = (availableSpace / (2 + slimColumnFactor)); // Width for the 1st and 3rd columns
			float middleColumnWidth = normalColumnWidth * slimColumnFactor; // Width for the middle (slimmer) column


			var list = new Listing_Standard { ColumnWidth = normalColumnWidth };
			list.Begin(inRect);

			// for three columns with 20px spacing
			var width = normalColumnWidth;

			list.Label("FFFF00", "OpenAI - ChatGPT", $"{charactersSentOpenAI} chars total");
			prevKey = chatGPTKey;
			list.TextField(ref chatGPTKey, "API Key (paste only)", true, () => chatGPTKey = "");
			if (chatGPTKey != "" && chatGPTKey != prevKey)
			{
				Tools.ReloadGPTModels();
				AI.TestKey(
					 response => LongEventHandler.ExecuteWhenFinished(() =>
					 {
						 var dialog = new Dialog_MessageBox(response);
						 Find.WindowStack.Add(dialog);
					 })
				);
			}

			if (chatGPTKey != "")
			{
				// Button for selecting the primary ChatGPT model (e.g., GPT-3.5)
				list.Label("Primary ChatGPT Model");
				rect = list.GetRect(UX.ButtonHeight);
				TooltipHandler.TipRegion(rect, "Set the primary AI model used by default for generating insights. For example, choosing 'GPT-3.5' could be your standard model.");
				if (Widgets.ButtonText(rect, ChatGPTModelPrimary))
					UX.GPTVersionMenu(l => ChatGPTModelPrimary = l);

				// Add some vertical spacing between buttons
				list.Gap(10f);
				list.CheckboxLabeled("Alternate between two models", ref UseSecondaryModel);
				if (UseSecondaryModel == true)
				{
					// Button for selecting the secondary ChatGPT model (e.g., GPT-4)
					list.Gap(10f);
					list.Label("Secondary ChatGPT Model");
					list.Gap(10f);
					rect = list.GetRect(UX.ButtonHeight);
					TooltipHandler.TipRegion(rect, "Set an alternative AI model to switch to based on the Model Switch Ratio. For instance, if 'GPT-4' is chosen as the secondary option, there can be shifts between 'GPT-3.5' and 'GPT-4'.");
					if (Widgets.ButtonText(rect, ChatGPTModelSecondary))
						UX.GPTVersionMenu(l => ChatGPTModelSecondary = l);
					list.Gap(10f);
					list.Slider(ref ModelSwitchRatio, 1, 20, f => $"Ratio: {f}:1", 1, "Adjust the frequency at which the system switches between the primary and secondary AI models. The 'Model Switch Ratio' value determines after how many calls to the primary model the system will switch to the secondary model for one time. A lower ratio means more frequent switching to the secondary model.\n\nExample: With a ratio of '1', there is no distinction between primary and secondary—each call alternates between the two. With a ratio of '10', the system uses the primary model nine times, and then the secondary model once before repeating the cycle.");

				}

			}


			list.Gap(16f);

			list.Label("FFFF00", "Azure - Speech Services", $"{charactersSentAzure} chars sent");
			var prevRegion = azureSpeechRegion;
			list.TextField(ref azureSpeechRegion, "Region");
			if (azureSpeechRegion != prevRegion)
				Personas.UpdateVoiceInformation();
			list.Gap(6f);
			prevKey = azureSpeechKey;
			list.TextField(ref azureSpeechKey, "API Key (paste only)", true, () => azureSpeechKey = "");
			if (azureSpeechKey != "" && azureSpeechKey != prevKey && azureSpeechRegion.NullOrEmpty() == false)
				TTS.TestKey(new Persona(), () => Personas.UpdateVoiceInformation());

			list.Gap(16f);

			list.Label("FFFF00", "Miscellaneous");
			list.Slider(ref speechVolume, 0f, 10f, f => $"Speech volume: {f.ToPercentage(false)}", 0.01f);
			list.CheckboxLabeled("Show speech as subtitles", ref showAsText);
			list.Gap(6f);
			rect = list.GetRect(UX.ButtonHeight);
			if (Widgets.ButtonText(rect, "Reset Stats"))
			{
				charactersSentOpenAI = 0;
				charactersSentAzure = 0;
			}
			list.NewColumn();
			list.ColumnWidth = middleColumnWidth;

			list.Gap(16f);
			// Create the AI Insights button
			if (Widgets.ButtonText(list.GetRect(UX.ButtonHeight), "AI Insights"))
			{
				ShowDetailedReportSettings();
			}

			// Add some vertical spacing between buttons if needed
			list.Gap(10f);

			list.Label("FFFF00", "Active personas", "", "All these personas are active.");

			var height = inRect.height - UX.ButtonHeight - 24f - list.CurHeight;
			var outerRect = list.GetRect(height);
			var listHeight = height;
			var innerRect = new Rect(0f, 0f, list.ColumnWidth, listHeight);

			Widgets.DrawBoxSolid(outerRect, listBackground);
			Widgets.BeginScrollView(outerRect, ref scrollPosition, innerRect, true);

			var list2 = new Listing_Standard();
			list2.Begin(innerRect);

			var rowHeight = 24;
			var i = 0;
			var y = 0f;
			foreach (var persona in personas)
			{
				PersonaRow(new Rect(0, y, innerRect.width, rowHeight), persona, i++);
				y += rowHeight;
			}

			list2.End();

			Widgets.EndScrollView();

			var bRect = list.GetRect(24);
			if (Widgets.ButtonImage(bRect.LeftPartPixels(24), Graphics.ButtonAdd[1]))
			{
				Event.current.Use();
				selected = new Persona();
				personas.Add(selected);
				selectedIndex = personas.IndexOf(selected);
			}
			bRect.x += 32;
			if (Widgets.ButtonImage(bRect.LeftPartPixels(24), Graphics.ButtonDel[selected != null ? 1 : 0]))
			{
				Event.current.Use();
				_ = personas.Remove(selected);
				var newCount = personas.Count;
				if (newCount == 0)
				{
					selectedIndex = -1;
					selected = null;
				}
				else
				{
					while (newCount > 0 && selectedIndex >= newCount)
						selectedIndex--;
					selected = personas[selectedIndex];
				}
			}
			bRect.x += 32;
			var dupable = selected != null;
			if (Widgets.ButtonImage(bRect.LeftPartPixels(24), Graphics.ButtonDup[dupable ? 1 : 0]) && dupable)
			{
				Event.current.Use();
				var namePrefix = Regex.Replace(selected.name, @" \d+$", "");
				var existingNames = personas.Select(p => p.name).ToHashSet();
				for (var n = 1; true; n++)
				{
					var newName = $"{namePrefix} {n}";
					if (existingNames.Contains(newName) == false)
					{
						var xml = selected.ToXml();
						selected = new Persona();
						Persona.PersonalityFromXML(xml, selected);
						selected.name = newName;
						personas.Add(selected);
						selectedIndex = personas.IndexOf(selected);
						break;
					}
				}
			}

			list.NewColumn(); //------------------------------------------------------------------------------------------------------------------
			list.ColumnWidth = normalColumnWidth;

			if (selected != null)
			{
				var curY = list.curY;
				_ = list.Label("Persona Name");
				list.curY = curY;
				var cw = list.ColumnWidth / 3f;
				list.curX += cw;
				list.ColumnWidth -= cw;
				selected.name = list.TextEntry(selected.name);
				list.curX -= cw;
				list.ColumnWidth += cw;
				list.Gap(16f);

				list.Languages(LanguageDatabase.AllLoadedLanguages, selected.azureVoiceLanguage, l => l.DisplayName, l =>
				{
					selected.azureVoiceLanguage = l == null ? "-" : l.FriendlyNameEnglish;
					Personas.UpdateVoiceInformation();
				}, width / 2, 0);
				list.Voices(selected, width / 2, 1);
				if (UX.HasVoiceStyles(selected))
					list.VoiceStyles(selected, width, 2);
				list.Gap(30f);

				list.Gap(16f);

				list.Slider(ref selected.azureVoiceStyleDegree, 0f, 2f, f => $"Style degree: {f.ToPercentage(false)}", 0.01f);
				list.Slider(ref selected.speechRate, -0.5f, 0.5f, f => $"Speech rate: {f.ToPercentage()}", 0.01f);
				list.Slider(ref selected.speechPitch, -0.5f, 0.5f, f => $"Speech pitch: {f.ToPercentage()}", 0.01f);

				list.Gap(16f);

				float buttonWidth = (rect.width - 40) / 3; // Spacing between buttons is 20, hence 40 for two spaces.

				rect = list.GetRect(UX.ButtonHeight);
				Rect buttonRect = new Rect(rect.x, rect.y, buttonWidth, rect.height);

				if (Widgets.ButtonText(buttonRect, "Personality"))
					Dialog_Personality.Show(selected);

				buttonRect.x += buttonWidth + 20; // Move x position by button width plus spacing

				if (Widgets.ButtonText(buttonRect, selected.personalityLanguage == "-" ? "Language" : selected.personalityLanguage))
					UX.LanguageChoiceMenu(Tools.commonLanguages, l => l, l => selected.personalityLanguage = l ?? "-");

				buttonRect.x += buttonWidth + 20; // Move x position by button width plus spacing

				if (Widgets.ButtonText(buttonRect, "Test"))
					TTS.TestKey(selected, null);

				list.Gap(16f);

				_ = list.Label("Sending game information", -1, "RimGPT limits when and what it sends to ChatGPT. It collects phrases from the game and other personas until after some time it sends some of the phrases batched together to create a comment.");
				list.Slider(ref selected.phrasesLimit, 1, 100, n => $"Max phrases: {n}", 1, "How many unsent phrases should RimGPT keep at a maximum?");
				selected.phraseBatchSize = Mathf.Min(selected.phraseBatchSize, selected.phrasesLimit);
				list.Slider(ref selected.phraseBatchSize, 1, selected.phrasesLimit, n => $"Batch size: {n} phrases", 1, "How many phrases should RimGPT send batched together in its data to ChatGPT?");
				list.Gap(16f);
				_ = list.Label("Delay between comments", -1, "To optimize, RimGPT collects text and phrases and only sends it in intervals to ChatGPT to create comments.");
				list.Slider(ref selected.phraseDelayMin, 5f, 1200f, f => $"Min: {Mathf.FloorToInt(f + 0.01f)} sec", 1f, 2, "RimGPT creates comments in intervals. What is the shortest time between comments?");
				if (selected.phraseDelayMin > selected.phraseDelayMax)
					selected.phraseDelayMin = selected.phraseDelayMax;
				var oldMax = selected.phraseDelayMax;
				list.Slider(ref selected.phraseDelayMax, 5f, 1200f, f => $"Max: {Mathf.FloorToInt(f + 0.01f)} sec", 1f, 2, "RimGPT creates comments in intervals. What is the longest time between comments?");
				if (selected.phraseDelayMax < selected.phraseDelayMin)
					selected.phraseDelayMax = selected.phraseDelayMin;
				if (oldMax > selected.phraseDelayMax)
					selected.nextPhraseTime = DateTime.Now.AddSeconds(selected.phraseDelayMin);
				list.Gap(16f);
				_ = list.Label("Comments");
				list.Slider(ref selected.phraseMaxWordCount, 1, 160, n => $"Max words: {n}", 1, "RimGPT instructs ChatGPT to generate comments that are no longer than this amount of words.");
				list.Gap(16f);
				_ = list.Label("History");
				list.Slider(ref selected.historyMaxWordCount, 200, 1200, n => $"Max words: {n}", 1, "RimGPT lets ChatGPT create a history summary that is then send together with new requests to form some kind of memory for ChatGPT. What is the maximum size of the history?");
				list.Gap(16f);

				width = (list.ColumnWidth - 2 * 20) / 3;
				rect = list.GetRect(UX.ButtonHeight);
				rect.width = width;
				if (Widgets.ButtonText(rect, "Copy"))
				{
					var share = selected.ToXml();
					if (share.NullOrEmpty() == false)
						GUIUtility.systemCopyBuffer = share;
				}
				rect.x += width + 20;
				if (Widgets.ButtonText(rect, "Paste"))
				{
					var text = GUIUtility.systemCopyBuffer;
					if (text.NullOrEmpty() == false)
						Persona.PersonalityFromXML(text, selected);
				}
				rect.x += width + 20;
				if (Widgets.ButtonText(rect, "Defaults"))
				{
					var xml = new Persona().ToXml();
					Persona.PersonalityFromXML(xml, selected);
				}
			}

			list.End();
		}

		public static void PersonaRow(Rect rect, Persona persona, int idx)
		{
			if (persona == selected)
				Widgets.DrawBoxSolid(rect, background);
			else if (Mouse.IsOver(rect))
				Widgets.DrawBoxSolid(rect, highlightedBackground);

			var tRect = rect;
			tRect.xMin += 3;
			tRect.yMin += 1;
			_ = Widgets.LabelFit(tRect, persona.name);

			if (Widgets.ButtonInvisible(rect))
			{
				selected = persona;
				selectedIndex = idx;
			}
		}

		private void ShowDetailedReportSettings()
		{
			// Create an instance of our custom window for detailed report settings
			Find.WindowStack.Add(new DetailedReportSettingsWindow(this));
		}

		// Define a new class inheriting from RimWorld's 'Window' for our settings
		private class DetailedReportSettingsWindow : Window
		{
			// Reference to the settings to allow us to modify them
			private RimGPTSettings settings;

			// Constructor to pass in the settings reference
			public DetailedReportSettingsWindow(RimGPTSettings settings)
			{
				this.settings = settings;

				// Set properties for the window
				doCloseX = true;

			}

			// Override the method to specify the initial size of the window
			public override Vector2 InitialSize => new Vector2(800f, 600f);


			public override void DoWindowContents(Rect inRect)
			{
				Listing_Standard listing = new Listing_Standard();
				listing.Begin(inRect);
				Text.Font = GameFont.Medium;
				listing.Label("AI Insights Configuration");

				// Switch back to the small font for standard text and options
				Text.Font = GameFont.Small;

				// Description paragraph explaining permanently monitored aspects by the AI
				string description = "RimGPT automatically monitors a range of essential data points to create adaptive and responsive " +
									 "personas. This includes game state, weather, colonist activities, messages, alerts, letters, and resources.\n\n" +
									 "Below you can enable additional insight feeds for more in-depth analysis:";
				listing.Label(description);
				string restartNotification = "Changes require a restart to take effect.";
				GUI.color = Color.yellow;
				listing.Label(restartNotification);
				GUI.color = Color.white;
				listing.GapLine(18f);

				float yOffset = listing.CurHeight; // Current Y position after the headers and description
				float rowHeight = 60f; // Set the height for each row
				float gapBetweenRows = 10f; // Gap between rows

				// Reset font size after headers
				Text.Font = GameFont.Small;

				// Power Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Power Insight",
					ref settings.reportEnergyFrequency,
					ref settings.reportEnergyStatus,
					ref settings.reportEnergyImmediate,
					"Provides the AI with detailed power grid statistics, updating at the set frequency of in-game time."
				);
				yOffset += rowHeight + gapBetweenRows;
				// Research Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Research Insight",
					ref settings.reportResearchFrequency,
					ref settings.reportResearchStatus,
					ref settings.reportResearchImmediate,
					"Allows the AI to be aware of all researched tech and progress, updating at the set frequency of in-game time."
				);
				yOffset += rowHeight + gapBetweenRows;
				// Thoughts & Mood Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Thoughts & Mood Insight",
					ref settings.reportColonistThoughtsFrequency,
					ref settings.reportColonistThoughts,
					ref settings.reportColonistThoughtsImmediate,
					"Enables periodic analysis by the AI of colonists' thoughts and mood impacts, as per the set frequency of in-game time."
				);
				yOffset += rowHeight + gapBetweenRows;
				// Interpersonal Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Interpersonal Insight",
					ref settings.reportColonistOpinionsFrequency,
					ref settings.reportColonistOpinions,
					ref settings.reportColonistOpinionsImmediate,
					"Feeds the AI a holistic view of interpersonal dynamics and opinions periodically, based on the frequency setting of in-game time."
				);
				yOffset += rowHeight + gapBetweenRows;
				// Detailed Colonist Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Detailed Colonist Insight(Experimental)",
					ref settings.reportColonistRosterFrequency,
					ref settings.reportColonistRoster,
					ref settings.reportColonistRosterImmediate,
					"Provides continuous updates to the AI on all colonists' details, including demographics and health. Adjust frequency (in-game time) carefully."
				);
				yOffset += rowHeight + gapBetweenRows;
				// Rooms Insight settings
				DrawSettingRow(
					new Rect(inRect.x, yOffset, inRect.width, rowHeight),
					"Rooms Insight(Experimental)",
					ref settings.reportRoomStatusFrequency,
					ref settings.reportRoomStatus,
					ref settings.reportRoomStatusImmediate,
					"Activates comprehensive room reporting to the AI, covering cleanliness, wealth, etc., at the defined frequency of in-game time."
				);
				listing.End();
			}

			private void DrawSettingRow(Rect overallRect, string label, ref int frequencySetting, ref bool enabled, ref bool immediate, string tooltip)
			{
				float padding = 8f; // Padding between controls

				// Calculate widths for each control's Rect based on the percentage of the overallRect's width
				float sliderWidth = overallRect.width * 0.7f - padding;
				float enabledWidth = (overallRect.width * 0.15f) - padding;
				float immediateWidth = (overallRect.width * 0.15f) - padding;

				// Rect for the slider
				Rect sliderRect = new Rect(overallRect.x, overallRect.y, sliderWidth, overallRect.height);
				Listing_Standard sliderListing = new Listing_Standard();
				sliderListing.Begin(sliderRect);
				sliderListing.Label(label);
				if (enabled) {
					sliderListing.Slider(ref frequencySetting, 250, 420000, n => $"Frequency: {FormatFrequencyLabel(n)}  ", 1, tooltip);
				}				
				sliderListing.End();

				// If the slider is disabled, overlay a 'Disabled' label
				if (!enabled)
				{
					 Rect disabledOverlayRect = new Rect(sliderRect.x + (sliderRect.width / 2)-16, sliderRect.y, sliderRect.width / 2-16, sliderRect.height);

					// More opaque overlay
					GUI.color = new Color(0f, 0f, 0f, 0.8f); // Dark overlay with higher opacity
					Widgets.DrawBoxSolid(disabledOverlayRect, GUI.color);
					GUI.color = Color.white; // Reset color to default
					Text.Anchor = TextAnchor.MiddleCenter; // Center text
					GUI.color = Color.red; // Text color
					Widgets.Label(disabledOverlayRect, "Disabled");
					GUI.color = Color.white; // Reset text color to default
					Text.Anchor = TextAnchor.UpperLeft; // Reset text anchor to default
				}

				// Rect for the enabled checkbox
				Rect enabledRect = new Rect(sliderRect.xMax + padding, overallRect.y + 16f, enabledWidth, overallRect.height);
				Listing_Standard enabledListing = new Listing_Standard();
				enabledListing.Begin(enabledRect);
				enabledListing.CheckboxLabeled("Enabled:", ref enabled, tooltip);
				enabledListing.End();

				// Rect for the immediate checkbox
				Rect immediateRect = new Rect(enabledRect.xMax + padding, overallRect.y + 16f, immediateWidth, overallRect.height);
				Listing_Standard immediateListing = new Listing_Standard();
				immediateListing.Begin(immediateRect);
				immediateListing.CheckboxLabeled("Immediate:", ref immediate, tooltip: "If checked, the AI will gain " + label + " insight immediately when starting or loading a game.");
				immediateListing.End();
			}

			private float ReadableTicks(int ticks)
			{
				const int TicksPerDay = 60000;
				const int TicksPerHour = 2500;

				if (ticks >= TicksPerDay)
				{
					return ticks / (float)TicksPerDay; // Returns days when ticks are more than or equal to a day
				}
				else
				{
					return ticks / (float)TicksPerHour / 24f; // Returns a fraction of a day when ticks are less than a day
				}
			}

			private string FormatFrequencyLabel(int ticks)
			{
				float readableValue = ReadableTicks(ticks);
				if (readableValue >= 1)
				{
					return $"{readableValue:0.#} days"; // Days
				}
				else
				{
					return $"{readableValue * 24:0.##} hrs"; // Hours (multiplied by 24 to convert from fraction of a day to hours)
				}
			}


		}





	}
}