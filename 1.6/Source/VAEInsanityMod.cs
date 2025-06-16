using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using static Verse.Widgets;

namespace VAEInsanity
{
    [HotSwappable]
    public class VAEInsanityMod : Mod
    {
        private VAEInsanityModSettings settings;

        public VAEInsanityMod(ModContentPack content) : base(content)
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                this.settings = GetSettings<VAEInsanityModSettings>();
            });
        }

        private float sectionScrollHeight = int.MaxValue;
        private Vector2 scrollPosition = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float widthWithoutScrollBar = inRect.width - 16;
            var viewRect = new Rect(inRect.x, inRect.y, widthWithoutScrollBar, sectionScrollHeight);
            sectionScrollHeight = 0;
            BeginScrollView(inRect, ref scrollPosition, viewRect);

            Vector2 pos = new Vector2(inRect.x, inRect.y); // Initialize position

            // Consistent label offset for list items
            float labelXOffsetForListItems = 10;

            // Draw the checkbox for self-harm content
            CheckboxLabeled(new Rect(pos.x, pos.y, widthWithoutScrollBar - 5, 24), "VAEI_EnableSelfHarmContent".Translate(), ref VAEInsanityModSettings.selfHarmEnabled);
            pos.y += 24;

            // Draw the label for sanity loss effects
            Label(new Rect(pos.x, pos.y, widthWithoutScrollBar, 24), "VAEI_SanityLossEffects".Translate());
            pos.y += 24;

            DrawSection(ref pos, widthWithoutScrollBar, labelXOffsetForListItems, "VAEI_General".Translate(),
                new List<Action>
                {
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.twistedMeatValue, ref pos, widthWithoutScrollBar,
                        "VEAI_EatingTwistedMeat".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.marriageCeremonyValue, ref pos, widthWithoutScrollBar,
                        "VAEI_MarriageCeremony".Translate(), 0f, 0.1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.partyValue, ref pos, widthWithoutScrollBar,
                        "VAEI_Party".Translate(), 0f, 0.1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.observingVoidDrawings, ref pos, widthWithoutScrollBar,
                        "VAEI_ObservingVoidDrawings".Translate(), -0.05f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.pitGateCollapsing, ref pos, widthWithoutScrollBar,
                        "VAEI_PitGateCollapsing".Translate(), 0f, 0.1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.voidClosing, ref pos, widthWithoutScrollBar,
                        "VAEI_VoidClosing".Translate(), 0f, 1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.recoveringFromMadness, ref pos, widthWithoutScrollBar,
                        "VAEI_RecoveringFromMadness".Translate(), 0f, 0.5f, labelXOffsetForListItems),
                });

            DrawSection(ref pos, widthWithoutScrollBar, labelXOffsetForListItems, "VAEI_PassiveEffectsPerDay".Translate(),
                new List<Action>
                {
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.highSanityValue, ref pos, widthWithoutScrollBar,
                        "VAEI_HighSanity".Translate(), 0f, 0.1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.lowSanityValue, ref pos, widthWithoutScrollBar,
                        "VAEI_LowSanity".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.duplicateSanityEffect, ref pos, widthWithoutScrollBar,
                        "VAEI_BeingDuplicated".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.unnaturalCorpseSanityEffect, ref pos, widthWithoutScrollBar,
                        "VAEI_UnnaturalCorpse".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.labyrinthSanityEffect, ref pos, widthWithoutScrollBar,
                        "VAEI_BeingInLabyrinth".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.unnaturalDarknessSanityEffect, ref pos, widthWithoutScrollBar,
                        "VAEI_BeingInUnnaturalDarkness".Translate(), -0.5f, -0.001f, labelXOffsetForListItems)
                });

            DrawSection(ref pos, widthWithoutScrollBar, labelXOffsetForListItems, "VAEI_PerformingJobs".Translate(),
                new List<Action>
                {
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.meditatingValue, ref pos, widthWithoutScrollBar,
                        "VAEI_Meditating".Translate(), 0f, 0.1f, labelXOffsetForListItems),
                    () => DrawCheckboxAndSlider(VAEInsanityModSettings.readingTomeValue, ref pos, widthWithoutScrollBar,
                        "VAEI_ReadingTome".Translate(), -0.1f, -0.001f, labelXOffsetForListItems),
                });

            DrawList(ref pos, widthWithoutScrollBar, "VAEI_InteractionEffects".Translate(), VAEInsanityModSettings.interactionEffects, labelXOffsetForListItems, -0.1f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_NonDisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.nonDisturbingInitiatorEffects, labelXOffsetForListItems, 0f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_DisturbingInitiatorEffects".Translate(), VAEInsanityModSettings.disturbingInitiatorEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_HediffEffects".Translate(), VAEInsanityModSettings.hediffEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_UsedThingsEffects".Translate(), VAEInsanityModSettings.usedThingsEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_RitualEffects".Translate(), VAEInsanityModSettings.ritualEffects, labelXOffsetForListItems, 0f, 0.1f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_InvokerEffects".Translate(), VAEInsanityModSettings.invokerEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_TargetEffects".Translate(), VAEInsanityModSettings.targetEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_ChanterEffects".Translate(), VAEInsanityModSettings.chanterEffects, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_SuppressingEntities".Translate(), VAEInsanityModSettings.suppressingEntities, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_StudyingEntities".Translate(), VAEInsanityModSettings.studyingEntities, labelXOffsetForListItems, -0.1f, -0.001f);
            DrawList(ref pos, widthWithoutScrollBar, "VAEI_KillingEntities".Translate(),
                VAEInsanityModSettings.killingEntities, labelXOffsetForListItems, 0f, 0.1f, listHeightOffset: 24 * 2);
            pos.y -= 10;
            DrawCheckboxAndSlider(VAEInsanityModSettings.killingShamblerValue, ref pos, widthWithoutScrollBar,
                "VAEI_KillingShambler".Translate(), 0f, 0.1f, labelXOffsetForListItems);
            DrawCheckboxAndSlider(VAEInsanityModSettings.killingNociosphereValue, ref pos, widthWithoutScrollBar,
                "VAEI_KillingNociosphere".Translate(), 0f, 0.1f, labelXOffsetForListItems);
            pos.y += 10;
            sectionScrollHeight = pos.y - inRect.y;
            EndScrollView();
        }

        private void DrawSection(ref Vector2 pos, float widthWithoutScrollBar, float labelXOffsetForListItems,
            string sectionLabel, List<Action> drawSettingsActions)
        {
            Label(new Rect(pos.x, pos.y, widthWithoutScrollBar, 24), sectionLabel);
            pos.y += 24;

            DrawMenuSection(new Rect(pos.x, pos.y, widthWithoutScrollBar, drawSettingsActions.Count * 24f + 10));
            pos.y += 5;

            drawSettingsActions.ForEach(action => action());
            pos.y += 10;
        }

        // Updated DrawList to accept min and max values for the sliders
        private void DrawList<T>(ref Vector2 pos, float width, string sectionLabel, Dictionary<T, SanityEffect> list,
            float labelXOffset, float minSliderValue, float maxSliderValue, float listHeightOffset = 0) where T : Def
        {
            // Draw section label
            Label(new Rect(pos.x, pos.y, width, 24), sectionLabel);
            pos.y += 24;

            // Calculate the height of the list section based on the number of items
            float listHeight = (list.Count * 24 + 10) + listHeightOffset;

            // Define the menu section rect and draw it
            Rect listRect = new Rect(pos.x, pos.y, width, listHeight);
            DrawMenuSection(listRect);
            pos.y += 5;

            // Draw each item in the list
            foreach (var kvp in list.ToList())
            {
                var defLabel = (string)kvp.Key.LabelCap;
                if (kvp.Key is RitualOutcomeEffectDef ritualDef)
                {
                    var behaviour = DefDatabase<RitualPatternDef>.AllDefs.FirstOrDefault(x => x.ritualOutcomeEffect == ritualDef
                    && x.shortDescOverride.NullOrEmpty() is false);
                    if (behaviour != null)
                    {
                        defLabel = behaviour.shortDescOverride.CapitalizeFirst();
                    }
                    else
                    {
                        defLabel = ritualDef.defName;
                    }
                }

                if (kvp.Value.isSingleSlider)
                {
                    DrawCheckboxAndSlider(kvp.Value, ref pos, width, defLabel, minSliderValue, maxSliderValue, labelXOffset);
                }
                else
                {
                    DrawCheckboxAndFloatRange(kvp.Value, ref pos, width, defLabel, minSliderValue, maxSliderValue, labelXOffset);
                }
            }

            pos.y += 10;
        }

        // Refactored to use SanityEffect
        public void DrawCheckboxAndFloatRange(SanityEffect effect, ref Vector2 pos, float width, string label, float minRangeValue, float maxRangeValue, float labelXOffset = 0)
        {
            // Draw the checkbox
            float checkboxXOffset = width - 30;
            Checkbox(new Vector2(checkboxXOffset, pos.y), ref effect.enabled);

            // Set consistent label width and slider width
            float labelWidth = 250;  // Adjust based on layout needs
            float sliderWidth = width - labelWidth - 50 - labelXOffset - 5;

            // Adjust label position and draw the FloatRange slider next to the label
            GUI.color = effect.enabled ? Color.white : Color.grey;
            Label(new Rect(pos.x + labelXOffset, pos.y, labelWidth, 24),
                          label + ": " + effect.sanityValue.min.ToStringPercent() + " - " + effect.sanityValue.max.ToStringPercent());

            FloatRange(new Rect(pos.x + labelXOffset + labelWidth + 5, pos.y - 5, sliderWidth, 24),
                (int)pos.y, ref effect.sanityValue, minRangeValue, maxRangeValue, null);

            GUI.color = Color.white;

            pos.y += 24; // Increment y position for the next item, keeping the label and slider on the same line
        }

        // Refactored to use SanityEffect
        public void DrawCheckboxAndSlider(SanityEffect effect, ref Vector2 pos, float width, string label, float minSliderValue, float maxSliderValue, float labelXOffset = 0)
        {
            // Draw the checkbox
            float checkboxXOffset = width - 30;
            Checkbox(new Vector2(checkboxXOffset, pos.y), ref effect.enabled);

            // Set consistent label width and slider width
            float labelWidth = 250;  // Adjust based on layout needs
            float sliderWidth = width - labelWidth - 50 - labelXOffset;

            // Adjust label position with the offset and draw the slider next to the label
            GUI.color = effect.enabled ? Color.white : Color.grey;
            Label(new Rect(pos.x + labelXOffset, pos.y, labelWidth, 24),
                          label + ": " + effect.sanityValue.max.ToStringPercent());

            var value = HorizontalSlider(new Rect(pos.x + labelXOffset + labelWidth, pos.y, sliderWidth, 24),
                effect.sanityValue.max, minSliderValue, maxSliderValue, middleAlignment: true);
            effect.sanityValue = new FloatRange(value);
            GUI.color = Color.white;

            pos.y += 24; // Increment y position for the next item, keeping the label and slider on the same line
        }

        public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo, float gap = 0f, GameFont sliderLabelFont = GameFont.Small, Color? sliderLabelColor = null)
        {
            Rect rect2 = rect;
            rect2.xMin += 8f;
            rect2.xMax -= 8f;
            GUI.color = sliderLabelColor ?? RangeControlTextColor;
            string text = range.min.ToStringByStyle(valueStyle) + " - " + range.max.ToStringByStyle(valueStyle);
            if (labelKey != null)
            {
                text = labelKey.Translate(text);
            }
            GameFont font = Text.Font;
            Text.Font = sliderLabelFont;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
            GUI.DrawTexture(position, BaseContent.WhiteTex);
            float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
            float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
            float num3 = num;
            float num4 = num2;
            Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
            GUI.DrawTexture(position2, FloatRangeSliderTex);
            Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
            GUI.DrawTexture(position3, FloatRangeSliderTex);
            if (curDragEnd != 0 && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
            {
                draggingId = 0;
                curDragEnd = RangeEnd.None;
                SoundDefOf.DragSlider.PlayOneShotOnCamera();
                Event.current.Use();
            }
            bool flag = false;
            if (Mouse.IsOver(rect) || draggingId == id)
            {
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != draggingId)
                {
                    draggingId = id;
                    float x = Event.current.mousePosition.x;
                    if (x < position2.xMax)
                    {
                        curDragEnd = RangeEnd.Min;
                    }
                    else if (x > position3.xMin)
                    {
                        curDragEnd = RangeEnd.Max;
                    }
                    else
                    {
                        float num5 = Mathf.Abs(x - position2.xMax);
                        float num6 = Mathf.Abs(x - (position3.x - 16f));
                        curDragEnd = ((num5 < num6) ? RangeEnd.Min : RangeEnd.Max);
                    }
                    flag = true;
                    Event.current.Use();
                    SoundDefOf.DragSlider.PlayOneShotOnCamera();
                }
                if (flag || (curDragEnd != 0 && UnityGUIBugsFixer.MouseDrag()))
                {
                    float value = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
                    value = Mathf.Clamp(value, min, max);
                    if (curDragEnd == RangeEnd.Min)
                    {
                        if (value != range.min)
                        {
                            range.min = Mathf.Min(value, max - gap);
                            if (range.max < range.min + gap)
                            {
                                range.max = range.min + gap;
                            }
                            CheckPlayDragSliderSound();
                        }
                    }
                    else if (curDragEnd == RangeEnd.Max && value != range.max)
                    {
                        range.max = Mathf.Max(value, min + gap);
                        if (range.min > range.max - gap)
                        {
                            range.min = range.max - gap;
                        }
                        CheckPlayDragSliderSound();
                    }
                    if (Event.current.type == EventType.MouseDrag)
                    {
                        Event.current.Use();
                    }
                }
            }
            Text.Font = font;
        }

        public override string SettingsCategory()
        {
            return "VAEI_ModSettingsName".Translate();
        }
    }
}